using ProductApp.Models;
using ProductApp.RequestDTO;
using ProductApp.ResponseDTO;

namespace ProductApp.Helpers
{
  /// <summary>
  /// Static helper class for mapping between domain models and DTOs.
  /// This reduces code duplication and centralizes mapping logic.
  /// </summary>
  public static class MappingHelper
  {
    /// <summary>
    /// Maps a User entity to a UserResponse DTO.
    /// </summary>
    public static UserResponse ToUserResponse(this User user)
    {
      if (user == null) return null;

      return new UserResponse
      {
        Id = user.Id,
        FirstName = user.FirstName,
        LastName = user.LastName,
        // Email = user.Email, // Uncomment if needed
        // PhoneNumber = user.PhoneNumber, // Uncomment if needed
        CreatedDate = user.CreatedDate,
        IsActive = user.IsActive,
        Status = user.Status
      };
    }

    /// <summary>
    /// Maps a collection of User entities to UserResponse DTOs.
    /// </summary>
    public static IEnumerable<UserResponse> ToUserResponses(this IEnumerable<User> users)
    {
      return users?.Select(ToUserResponse) ?? Enumerable.Empty<UserResponse>();
    }

    /// <summary>
    /// Maps a CreateUserRequest DTO to a User entity.
    /// </summary>
    public static User ToUser(this CreateUserRequest request)
    {
      if (request == null) return null;

      return new User
      {
        FirstName = request.FirstName,
        LastName = request.LastName,
        Email = request.Email,
        PhoneNumber = request.PhoneNumber,
        IsActive = Constants.DefaultIsActive,
        CreatedDate = Constants.DefaultCreatedDate
      };
    }

    /// <summary>
    /// Maps an UpdateUserRequest DTO to a User entity for updates.
    /// </summary>
    public static User ToUser(this UpdateUserRequest request)
    {
      if (request == null) return null;

      return new User
      {
        FirstName = request.FirstName,
        LastName = request.LastName,
        Email = request.Email,
        PhoneNumber = request.PhoneNumber,
        IsActive = request.IsActive
      };
    }

    /// <summary>
    /// Updates an existing User entity with data from UpdateUserRequest.
    /// </summary>
    public static void UpdateFromRequest(this User user, UpdateUserRequest request)
    {
      if (user == null || request == null) return;

      user.FirstName = request.FirstName;
      user.LastName = request.LastName;
      user.Email = request.Email;
      user.PhoneNumber = request.PhoneNumber;
      user.IsActive = request.IsActive;
      user.UpdatedDate = DateTime.UtcNow;
    }
  }
}