using Microsoft.AspNetCore.Mvc;

namespace ProductApp.Helpers
{
  /// <summary>
  /// Static helper class for creating standardized API responses.
  /// Ensures consistent response formats across all controllers.
  /// </summary>
  public static class ApiResponseHelper
  {
    /// <summary>
    /// Creates a successful response with data.
    /// </summary>
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