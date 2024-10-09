using AutoMapper;
using Bills.Domain.Dto.Users;
using Bills.Domain.Entities;
using Bills.Service.Interface;
using Bills.Service.Interface.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Bills.Service.Services
{
    public class UserService : IUserService
    {
        private readonly BillsProjectContext _context;
        private readonly IMapper _mapper;
        private readonly IHashService _hashService;
        private readonly ISendMailService _sendMailService;
        private readonly string _secretKey;

        public UserService(BillsProjectContext context
                          , IMapper mapper
                          , IHashService hashService
                          , IConfiguration configuration
                          , ISendMailService sendMailService)
        {
            _context = context;
            _mapper = mapper;
            _hashService = hashService;
            _sendMailService = sendMailService;
            _secretKey = configuration["JwtSettings:secretkey"];
        }

        public async Task<string> Authenticate(LoginDto dto)
        {

            User user = null;

            if (!string.IsNullOrEmpty(dto.email))
            {
                user = await _context.Users.SingleOrDefaultAsync(u => u.Email == dto.email);
            }
            else if (!string.IsNullOrEmpty(dto.username))
            {
                user = await _context.Users.SingleOrDefaultAsync(u => u.Username == dto.username);
            }

            if (user == null)
            {
                throw new ArgumentException("User not Found.");
            }

            if (!_hashService.VerifyPassword(dto.password, user.PasswordHash, user.PasswordSalt))
            {
                throw new UnauthorizedAccessException("Credenciais inválidas.");
            }

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("UserId", user.Id.ToString())
                };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "yourdomain.com",
                audience: "yourdomain.com",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> CreateUser(UserDto dto)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    User newUser = _mapper.Map<User>(dto);

                    newUser.PasswordSalt = _hashService.GenerateSalt(128);
                    newUser.PasswordHash = _hashService.ComputeHash(dto.pass, newUser.PasswordSalt);

                    await _context.Users.AddAsync(newUser);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return "Ok";
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Error while creating user: " + ex.Message);
                }
            }
        }

        public async Task<User> ChangePassword(string newPass, long id)
        {
            User user = await _context.Users.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (user == null)
                throw new ArgumentException("User not found!");

            if (_hashService.VerifyPassword(newPass, user.PasswordHash, user.PasswordSalt))
                throw new ArgumentException("The new password can not be the same as old.");

            user.PasswordSalt = _hashService.GenerateSalt(128);
            user.PasswordHash = _hashService.ComputeHash(newPass, user.PasswordSalt);

            return user;
        }
        public async Task<User> GeneratePasswordToken(string email)
        {
            try
            {
                User user = _context.Users.Where(x => x.Email == email).FirstOrDefault();

                if (user == null)
                    throw new ArgumentException("User not found!");

                var token = GenerateToken();

                UserPasswordToken uToken = new UserPasswordToken()
                {
                    UserId = user.Id,
                    Token = token,
                    ExpiresIn = DateTime.Now.AddMinutes(10),
                    IsUsed = false,
                };

                _context.UserPasswordTokens.Add(uToken);

                bool sendMail = await _sendMailService.SendMail("henriquebonzao@hotmail.com", email, "Recuperar senha!", token, null, null);

                if (!sendMail)
                {
                    _context.UserPasswordTokens.Remove(uToken);
                    throw new Exception("Failed to send Mail.");
                }

                _context.SaveChanges();

                return user;
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        private string GenerateToken()
        {
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                var tokenData = new byte[32]; // 32 bytes = 256 bits de segurança
                rng.GetBytes(tokenData);
                return Convert.ToBase64String(tokenData);
            }
        }

        public async Task<User> GetUser(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<string> UpdateUser(UserDto user)
        {
            throw new NotImplementedException();
        }
    }
}
