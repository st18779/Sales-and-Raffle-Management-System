using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Interfaces;
using static server.DTOs.UserDto;

namespace server.Controllers
{

    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
    private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService= userService;
        }

        [HttpGet]
        [Route("usersList")]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetUsersList()
        {
            var users = await _userService.GetUsersList();
            return Ok(users);
        }

        [HttpGet]
        [Route("GetUserById/{id}")]
        public async Task<ActionResult<UserResponseDto>> GetUserById(string id)
        {
            var user = await _userService.GetUserById(id);
            if(user == null)
                return NotFound(new { message = $"User with ID {id} does not exist!" });
            return Ok(user);
        }

        [HttpGet]
        [Route("GetUserByEmail/{email}")]
        public async Task<ActionResult<UserResponseDto>> GetUserByEmail(string email)
        {
            var user = await _userService.GetUserByEmail(email);
            if (user == null)
                return NotFound(new { message = $"User with email {email} does not exist!" });
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserResponseDto>> CreateUser([FromBody] UserCreateDto user)
        {
            try{
                var createdUser = await _userService.CreateUser(user);
                if (createdUser == null)
                    return BadRequest(new { message = $"User with ID {user.Id} already exists!" });
                return Ok(createdUser);
            }
            catch(Exception){
                return BadRequest(new { message = "Could not create user." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(string id)
        {
            var user = await _userService.DeleteUser(id);
            if(!user)
                return NotFound(new { message = $"User with ID {id} does not exist!" });
            return Ok("Deleted!");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserResponseDto>> UpdateUser(string id, [FromBody] UserUpdateDto user)
        {
            var updatedUser = await _userService.UpdateUser(id,user);
            if(updatedUser == null)
                return NotFound(new { message = $"User with ID {id} does not exist!" });
            return Ok(updatedUser);
        }
    }
}