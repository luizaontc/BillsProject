using Bills.Domain.Dto.Users;
using Bills.Domain.Entities;

namespace Bills.Service.Interface
{
    public interface IUserService
    {
        Task<string> CreateUser(UserDto dto);
        Task<User> GetUser(int id);
        Task<string> UpdateUser(UserDto dto);
        Task<bool> Authenticate(LoginDto dto);
    }
}
