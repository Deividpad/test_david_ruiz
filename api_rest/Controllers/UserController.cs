using api_rest.Dto;
using api_rest.Exception;
using api_rest.Models;
using api_rest.Services;
using Microsoft.AspNetCore.Mvc;

namespace api_rest.Controllers; 

[ApiController]
[Route("api/v1/[controller]")]
public class UserController : ControllerBase
{

    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet("get-users")]
    public async Task<ActionResult<List<UserDTO>>> Get()
    {
        var users = await _userService.GetUsers();
        return Ok(users);
    }
    
    [HttpGet("get-user-by-id/{userId}")]
    public async Task<ActionResult<UserDTO>> GetById(int userId)
    {
        try
        {
            var user = await _userService.GetUserById(userId);
            return Ok(user);
        }
        catch (UserNotFoundException  ex)
        {
            Console.WriteLine(ex);
            return NotFound(new { message = ex.Message });
        }
        catch (System.Exception ex)
        {
            // Handle other exceptions (optional)
            return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
        }
        
    }
    
    [HttpPost("save-user")]
    public async Task<ActionResult<User>> SaveUser([FromBody] UserDTO userDto)
    {
        try
        {
            var user = await _userService.SaveUser(userDto);
            return Ok(user);
        }
        catch (System.Exception ex)
        {
            // Handle other exceptions (optional)
            return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
        }
    }
    
    [HttpPut("update-user/{userId}")]
    public async Task<ActionResult<User>> UpdateUser(int userId, [FromBody] UserDTO userDto)
    {
        try
        {
            var user = await _userService.UpdateUser(userId, userDto);
            return Ok(user);
        }
        catch (UserNotFoundException  ex)
        {
            Console.WriteLine(ex);
            return NotFound(new { message = ex.Message });
        }
        catch (System.Exception ex)
        {
            // Handle other exceptions (optional)
            return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
        }
    }
    
    [HttpDelete("delete-user/{userId}")]
    public async Task<ActionResult> DeleteUser(int userId)
    {
        return Ok(await _userService.DeleteUser(userId));
    }
}