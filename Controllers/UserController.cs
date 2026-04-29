using Microsoft.AspNetCore.Mvc;
using ProductApp.Services.Interfaces;
using ProductApp.Models;
using ProductApp.RequestDTO;
using ProductApp.ResponseDTO;

namespace ProductApp.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class UserController : ControllerBase
  {
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
      _userService = userService;
    }

    [HttpGet("/users")]
    public async Task<IActionResult> GetAllUsers()
    {
      var users = await _userService.GetAllUsersAsync();
      var response = users.Select(u => new UserResponse
      {
        Id = u.Id,
        FirstName = u.FirstName,
        LastName = u.LastName,
        // Email = u.Email,
        // PhoneNumber = u.PhoneNumber,
        CreatedDate = u.CreatedDate,
        IsActive = u.IsActive
      });
      return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
      var user = await _userService.GetUserByIdAsync(id);
      if (user == null) return NotFound();

      var response = new UserResponse
      {
        Id = user.Id,
        FirstName = user.FirstName,
        LastName = user.LastName,
        // Email = user.Email,
        // PhoneNumber = user.PhoneNumber,
        CreatedDate = user.CreatedDate,
        IsActive = user.IsActive
      };
      return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
      var user = new User
      {
        FirstName = request.FirstName,
        LastName = request.LastName,
        Email = request.Email,
        PhoneNumber = request.PhoneNumber
      };

      await _userService.CreateUserAsync(user);

      var response = new UserResponse
      {
        Id = user.Id,
        FirstName = user.FirstName,
        LastName = user.LastName,
        // Email = user.Email,
        // PhoneNumber = user.PhoneNumber,
        CreatedDate = user.CreatedDate,
        IsActive = user.IsActive
      };
      return CreatedAtAction(nameof(GetUser), new { id = user.Id }, response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest request)
    {
      var user = new User
      {
        FirstName = request.FirstName,
        LastName = request.LastName,
        Email = request.Email,
        PhoneNumber = request.PhoneNumber,
        IsActive = request.IsActive
      };

      var updated = await _userService.UpdateUserAsync(id, user);
      if (updated == null) return NotFound();

      var response = new UserResponse
      {
        Id = updated.Id,
        FirstName = updated.FirstName,
        LastName = updated.LastName,
        // Email = updated.Email,
        // PhoneNumber = updated.PhoneNumber,
        CreatedDate = updated.CreatedDate,
        IsActive = updated.IsActive
      };
      return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
      var deleted = await _userService.DeleteUserAsync(id);
      if (!deleted) return NotFound();
      return Ok(new { message = $"Deleted user {id}" });
    }
  }
}