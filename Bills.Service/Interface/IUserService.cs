using Bills.Domain.Dto.Users;
using Bills.Domain.Entities;
using System.Security.Claims;

namespace Bills.Service.Interface
{
    public interface IUserService
    {
        Task<string> CreateUser(UserDto dto);
        Task<User> GetUser(int id);
        Task<string> UpdateUser(UserDto dto);
        Task<string> Authenticate(LoginDto dto);
        Task<User> ChangePassword(string newPass, long id);
        Task<User> GeneratePasswordToken(string email);
    }
}
