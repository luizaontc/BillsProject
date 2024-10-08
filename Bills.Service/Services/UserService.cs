using AutoMapper;
using Bills.Domain.Dto.Users;
using Bills.Domain.Entities;
using Bills.Service.Interface;
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
        private readonly string _secretKey;

        public UserService(BillsProjectContext context
                          , IMapper mapper
                          , IHashService hashService
                          , IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _hashService = hashService;
            _secretKey = configuration["JwtSettings:secretkey"];
        }

        public async Task<string> Authenticate(LoginDto dto)
        {
            try
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
                    throw new UnauthorizedAccessException("Credenciais inválidas.");
                }

                // Verificar a senha
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
            catch (Exception ex)
            {

                throw;
            }
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
