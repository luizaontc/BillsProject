using Bills.Domain.Dto.Users;
using Bills.Domain.Entities;
using Bills.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace Bills.Service.Services
{
    public class UserService : IUserService
    {
        private readonly BillsProjectContext _context;

        public UserService(BillsProjectContext context)
        {
            _context = context;
        }
        public async Task<string> CreateUser(CreateUserDto user)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    User newUser = new User();
                    //{
                    //    Name = user.Name,
                    //    Birthday = user.Birthday,
                    //    Currency = user.Currency,
                    //    Document = user.Document,
                    //    Status = user.Status
                    //};

                    await _context.Users.AddAsync(newUser);
                    await _context.SaveChangesAsync();

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
