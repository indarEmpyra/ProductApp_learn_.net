using Microsoft.AspNetCore.Mvc;
using ProductApp.Services.Interfaces;
using ProductApp.Models;
using ProductApp.RequestDTO;
using ProductApp.ResponseDTO;
using ProductApp.Helpers;

namespace ProductApp.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class UserController : ControllerBase
  // ControllerBase is a base class for an MVC controller without view support. 
  // It provides common functionality for handling HTTP requests and generating responses, such as model binding, validation, and formatting.
  {
    private readonly IUserService _userService;
    // The IUserService is injected into the UserController through constructor injection.
    // This allows the controller to use the service to perform operations related to users, such as retrieving, creating, updating, and deleting user data.

    // What is constructor injection?
    // Constructor injection is a design pattern used in dependency injection where dependencies are provided to a class through its constructor. 
    // In this pattern, the required dependencies are passed as parameters to the class's constructor, allowing the class to receive and 
    // use those dependencies without having to create them internally. This promotes loose coupling and makes it easier to manage dependencies, 
    // especially in larger applications.

    public UserController(IUserService userService)
    {
      _userService = userService;
      // The constructor takes an IUserService as a parameter and assigns it to the private readonly field _userService.
      // This allows the UserController to use the IUserService instance to perform operations related to users,
      // such as retrieving, creating, updating, and deleting user data.
    }

    [HttpGet("/users")]
    public async Task<IActionResult> GetAllUsers()
    {
      var users = await _userService.GetAllUsersAsync();
      var response = users.ToUserResponses();
      return ApiResponseHelper.Success(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
      var user = await _userService.GetUserByIdAsync(id);
      if (user == null) return ApiResponseHelper.NotFound();

      var response = user.ToUserResponse();
      return ApiResponseHelper.Success(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
      // Validate request
      var validationResult = ValidationHelper.ValidateUser(request.ToUser());
      if (!validationResult.IsValid)
        return ApiResponseHelper.BadRequest(validationResult.ErrorMessage);

      var user = request.ToUser();
      await _userService.CreateUserAsync(user);

      var response = user.ToUserResponse();
      return ApiResponseHelper.Created(response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest request)
    {
      var existingUser = await _userService.GetUserByIdAsync(id);
      if (existingUser == null) return ApiResponseHelper.NotFound();

      // Validate request
      var tempUser = request.ToUser();
      tempUser.Id = id; // Set ID for validation
      var validationResult = ValidationHelper.ValidateUser(tempUser);
      if (!validationResult.IsValid)
        return ApiResponseHelper.BadRequest(validationResult.ErrorMessage);

      existingUser.UpdateFromRequest(request);
      var updated = await _userService.UpdateUserAsync(id, existingUser);
      if (updated == null) return ApiResponseHelper.InternalServerError();

      var response = updated.ToUserResponse();
      return ApiResponseHelper.Success(response, "User updated successfully");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
      var deleted = await _userService.DeleteUserAsync(id);
      if (!deleted) return ApiResponseHelper.NotFound();

      return ApiResponseHelper.Success(message: $"User {id} deleted successfully");
    }
  }
}




// #region and #endregion are preprocessor directives used in C# and other languages (like Apex or C++)
//  to define a block of code that can be expanded or collapsed within an Integrated Development Environment (IDE) like Visual Studio or VS Code