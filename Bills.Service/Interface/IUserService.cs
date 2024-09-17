using Bills.Domain.Dto.Users;
using Bills.Domain.Entities;

namespace Bills.Service.Interface
{
    public interface IUserService
    {
        Task<string> CreateUser(CreateUserDto user);
        Task<User> GetUser(int id);
        Task<string> UpdateUser(User user);
    }
}
