namespace ProductApp.Helpers
{
  /// <summary>
  /// Application-wide constants following industry best practices.
  /// Use static readonly for non-compile-time constants and const for compile-time constants.
  /// </summary>
  public static class Constants
  {
    // API Route Constants
    public const string ApiBaseRoute = "api";
    public const string UsersRoute = "users";
    public const string ProductsRoute = "products";

    // Default Values
    public const bool DefaultIsActive = true;
    public const int DefaultPageSize = 10;
    public const int MaxPageSize = 100;

    // Validation Constants
    public const int MaxNameLength = 50;
    public const int MaxEmailLength = 100;
    public const int MaxPhoneLength = 20;

    // Error Messages
    public const string UserNotFound = "User not found";
    public const string ProductNotFound = "Product not found";
    public const string InvalidRequest = "Invalid request data";
    public const string DatabaseError = "An error occurred while processing your request";

    // Date/Time Constants
    public static readonly DateTime DefaultCreatedDate = DateTime.UtcNow;

    // Role Constants (if using string-based roles)
    public const string AdminRole = "Admin";
    public const string UserRole = "User";
    public const string ModeratorRole = "Moderator";
  }
}