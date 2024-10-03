using AutoMapper;
using Bills.Domain.Dto.Users;
using Bills.Domain.Entities;
using Bills.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace Bills.Service.Services
{
    public class UserService : IUserService
    {
        private readonly BillsProjectContext _context;
        private readonly IMapper _mapper;
        private readonly IHashService _hashService;

        public UserService(BillsProjectContext context
                          , IMapper mapper
                          , IHashService hashService)
        {
            _context = context;
            _mapper = mapper;
            _hashService = hashService;
        }

        public async Task<bool> Authenticate(LoginDto dto)
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

                return _hashService.VerifyPassword(dto.password, user.PasswordHash, user.PasswordSalt);
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
