using Bills.Domain.Dto;
using Bills.Domain.Dto.Users;
using Bills.Domain.Entities;
using Bills.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace Bills.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] CreateUserDto dto)
        {
            try
            {
                await _userService.CreateUser(dto);
                return Ok("User created!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }

        [HttpGet]
        public async Task<ActionResult<User>> GetUser([FromQuery] int id)
        {
            try
            {
                var user = await _userService.GetUser(id);

                if (user == null)
                    throw new Exception("User does not exist!");

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }
    }
}
