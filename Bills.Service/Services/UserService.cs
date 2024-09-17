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

        public UserService(BillsProjectContext context
                          , IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<string> CreateUser(CreateUserDto dto)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    User newUser = _mapper.Map<User>(dto);

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

        public Task<string> UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
