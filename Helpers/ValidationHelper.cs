using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using ProductApp.Models;

namespace ProductApp.Helpers
{
  /// <summary>
  /// Static helper class for common validation operations.
  /// Centralizes validation logic and makes it reusable across the application.
  /// </summary>
  public static class ValidationHelper
  {
    // Email validation regex pattern
    private const string EmailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

    // Phone validation regex pattern (basic international format)
    private const string PhonePattern = @"^\+?[\d\s\-\(\)]{10,}$";

    /// <summary>
    /// Validates if the provided email address is in a valid format.
    /// </summary>
    public static bool IsValidEmail(string email)
    {
      if (string.IsNullOrWhiteSpace(email)) return false;
      return Regex.IsMatch(email, EmailPattern, RegexOptions.IgnoreCase);
    }

    /// <summary>
    /// Validates if the provided phone number is in a valid format.
    /// </summary>
    public static bool IsValidPhoneNumber(string phoneNumber)
    {
      if (string.IsNullOrWhiteSpace(phoneNumber)) return false;
      return Regex.IsMatch(phoneNumber, PhonePattern);
    }

    /// <summary>
    /// Validates if the provided name meets the application's requirements.
    /// </summary>
    public static bool IsValidName(string name)
    {
      if (string.IsNullOrWhiteSpace(name)) return false;
      return name.Length <= Constants.MaxNameLength && !ContainsInvalidCharacters(name);
    }

    /// <summary>
    /// Checks if a string contains potentially harmful characters.
    /// </summary>
    private static bool ContainsInvalidCharacters(string input)
    {
      // Basic check for common injection characters
      char[] invalidChars = { '<', '>', '&', '"', '\'', ';' };
      return input.IndexOfAny(invalidChars) >= 0;
    }

    /// <summary>
    /// Validates a User entity using DataAnnotations and custom rules.
    /// </summary>
    public static ValidationResult ValidateUser(User user)
    {
      if (user == null)
        return new ValidationResult("User cannot be null");

      var validationContext = new ValidationContext(user);
      var dataAnnotationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

      if (!Validator.TryValidateObject(user, validationContext, dataAnnotationResults, true))
      {
        return new ValidationResult(string.Join("; ", dataAnnotationResults.Select(r => r.ErrorMessage)));
      }

      // Custom validations
      if (!IsValidEmail(user.Email))
        return new ValidationResult("Invalid email format");

      if (!IsValidPhoneNumber(user.PhoneNumber))
        return new ValidationResult("Invalid phone number format");

      if (!IsValidName(user.FirstName))
        return new ValidationResult("Invalid first name");

      if (!string.IsNullOrEmpty(user.LastName) && !IsValidName(user.LastName))
        return new ValidationResult("Invalid last name");

      return ValidationResult.Success;
    }

    /// <summary>
    /// Sanitizes user input by trimming whitespace and removing potentially harmful characters.
    /// </summary>
    public static string SanitizeInput(string input)
    {
      if (string.IsNullOrWhiteSpace(input)) return string.Empty;

      // Trim whitespace
      input = input.Trim();

      // Remove potentially harmful characters (basic sanitization)
      input = Regex.Replace(input, @"[<>&""';]", string.Empty);

      return input;
    }

    /// <summary>
    /// Custom validation result class for detailed error reporting.
    /// </summary>
    public class ValidationResult
    {
      public bool IsValid => string.IsNullOrEmpty(ErrorMessage);
      public string ErrorMessage { get; }

      public ValidationResult(string errorMessage)
      {
        ErrorMessage = errorMessage;
      }

      public static ValidationResult Success => new ValidationResult(null);
    }
  }
}