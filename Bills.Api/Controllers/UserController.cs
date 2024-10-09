using Bills.Domain.Dto;
using Bills.Domain.Dto.Users;
using Bills.Domain.Entities;
using Bills.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
        public async Task<ActionResult<User>> CreateUser([FromBody] UserDto dto)
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

        [HttpPost("/login")]
        public async Task<ActionResult<string>> DoLogin([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await _userService.Authenticate(dto);

                if (string.IsNullOrEmpty(user))
                    return Unauthorized("Credenciais inválidas.");

                return Ok(user);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}"); 
            }
        }

        [HttpPost("/changePassword")]
        public async Task<ActionResult<string>> ChangePassword([FromBody] LoginDto dto)
        {
            try
            {
                if (string.IsNullOrEmpty(dto.password) || string.IsNullOrEmpty(dto.id.ToString()))
                    throw new ArgumentException("Please fill the password!");

                var user = await _userService.ChangePassword(dto.password, (long)dto.id);

                return Ok(user);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpPost("/generateChangePasswordToken")]
        public async Task<ActionResult<string>> GenerateChangePasswordToken([FromQuery]string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                    return BadRequest("Email cannot be null");

                var user = _userService.GeneratePasswordToken(email);

                return "ok";
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }
    }
}
