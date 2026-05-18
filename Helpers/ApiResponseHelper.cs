using Microsoft.AspNetCore.Mvc;

namespace ProductApp.Helpers
{
  /// <summary>
  /// Static helper class for creating standardized API responses.
  /// Ensures consistent response formats across all controllers.
  /// </summary>
  public static class ApiResponseHelper

  //? Why did we make it static? Because we don't need to create an instance of ApiResponseHelper to use its methods.
  // Static classes are useful for grouping related utility methods that don't require any instance data.

  //? What will happen if I don't make it static? 
  // If we don't make the ApiResponseHelper class static, we would need to create an instance of it every time we want to use its methods.
  // For example, we would have to do something like this in our controllers:
  // var apiResponseHelper = new ApiResponseHelper();

  {
    // Creates a successful response with data.
    // IActionResult is an interface in ASP.NET Core that represents the result of an action method.

    //? what's the difference between IActionResult and ActionResult<T>?
    // IActionResult is a non-generic interface that represents any type of action result, such as OkResult, NotFoundResult, etc. 
    // It does not specify the type of data being returned.
    // ActionResult<T> is a generic class that inherits from ActionResult and allows you to specify the type of data being returned. 
    // It can return either a specific type T or any other action result. For example, you can return ActionResult<User> which 
    //can return a User object or an error response like NotFound(). 

    // Using ActionResult<T> can provide better type safety and clarity in your API responses, while IActionResult offers more flexibility
    // if you need to return different types of results from the same action method.  


    // ? Why do we use OkObjectResult, CreatedResult, etc. instead of just returning the ApiResponse object directly?
    // OkObjectResult, CreatedResult, etc. are specific implementations of IActionResult that represent different HTTP status codes. 
    // They allow us to set the appropriate status code for the response while also returning our standardized ApiResponse object in the body.  

    public static IActionResult Success(object data = null, string message = null)
    {
      return new OkObjectResult(new ApiResponse
      {
        Success = true,
        Message = message ?? "Operation completed successfully",
        Data = data
      });
    }

    /// <summary>
    /// Creates a successful response for creation operations.
    /// </summary>
    public static IActionResult Created(object data = null, string message = "Resource created successfully")
    {
      return new CreatedResult(string.Empty, new ApiResponse
      {
        Success = true,
        Message = message,
        Data = data
      });
    }

    /// <summary>
    /// Creates a not found response.
    /// </summary>
    public static IActionResult NotFound(string message = null)
    {
      return new NotFoundObjectResult(new ApiResponse
      {
        Success = false,
        Message = message ?? Constants.UserNotFound,
        Data = null
      });
    }

    /// <summary>
    /// Creates a bad request response.
    /// </summary>
    public static IActionResult BadRequest(string message = null, object data = null)
    {
      return new BadRequestObjectResult(new ApiResponse
      {
        Success = false,
        Message = message ?? Constants.InvalidRequest,
        Data = data
      });
    }

    /// <summary>
    /// Creates an internal server error response.
    /// </summary>
    public static IActionResult InternalServerError(string message = null)
    {
      return new ObjectResult(new ApiResponse
      {
        Success = false,
        Message = message ?? Constants.DatabaseError,
        Data = null
      })
      {
        StatusCode = 500
      };
    }

    /// <summary>
    /// Creates an unauthorized response.
    /// </summary>
    public static IActionResult Unauthorized(string message = "Unauthorized access")
    {
      return new UnauthorizedObjectResult(new ApiResponse
      {
        Success = false,
        Message = message,
        Data = null
      });
    }

    /// <summary>
    /// Creates a forbidden response.
    /// </summary>
    public static IActionResult Forbidden(string message = "Access forbidden")
    {
      return new ObjectResult(new ApiResponse
      {
        Success = false,
        Message = message,
        Data = null
      })
      {
        StatusCode = 403
      };
    }

    /// <summary>
    /// Creates a no content response.
    /// </summary>
    public static IActionResult NoContent(string message = "Operation completed")
    {
      return new NoContentResult();
    }

    /// <summary>
    /// Standard API response structure.
    /// </summary>
    public class ApiResponse
    {
      public bool Success { get; set; }
      public string Message { get; set; }
      public object Data { get; set; }
      public DateTime Timestamp { get; set; } = DateTimeHelper.UtcNow;
    }
  }
}