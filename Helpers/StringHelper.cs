using System.Text;
using System.Text.RegularExpressions;

namespace ProductApp.Helpers
{
  /// <summary>
  /// Static helper class for common string operations.
  /// Provides utility methods for string manipulation and validation.
  /// </summary>
  public static class StringHelper
  {
    /// <summary>
    /// Checks if a string is null, empty, or consists only of white-space characters.
    /// </summary>
    public static bool IsNullOrWhiteSpace(this string value)
    {
      return string.IsNullOrWhiteSpace(value);
    }

    /// <summary>
    /// Checks if a string is null or empty.
    /// </summary>
    public static bool IsNullOrEmpty(this string value)
    {
      return string.IsNullOrEmpty(value);
    }

    /// <summary>
    /// Capitalizes the first letter of each word in a string.
    /// </summary>
    public static string ToTitleCase(this string value)
    {
      if (value.IsNullOrWhiteSpace()) return value;

      return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
    }

    /// <summary>
    /// Truncates a string to a specified length and adds ellipsis if truncated.
    /// </summary>
    public static string Truncate(this string value, int maxLength, string suffix = "...")
    {
      if (value.IsNullOrEmpty() || value.Length <= maxLength) return value;

      return value.Substring(0, maxLength - suffix.Length) + suffix;
    }

    /// <summary>
    /// Removes all non-alphanumeric characters from a string.
    /// </summary>
    public static string RemoveNonAlphanumeric(this string value)
    {
      if (value.IsNullOrEmpty()) return value;

      return Regex.Replace(value, @"[^a-zA-Z0-9]", string.Empty);
    }

    /// <summary>
    /// Converts a string to a URL-friendly slug.
    /// </summary>
    public static string ToSlug(this string value)
    {
      if (value.IsNullOrEmpty()) return value;

      // Convert to lowercase
      value = value.ToLower();

      // Remove non-alphanumeric characters except spaces and hyphens
      value = Regex.Replace(value, @"[^a-z0-9\s-]", string.Empty);

      // Replace spaces with hyphens
      value = Regex.Replace(value, @"\s+", "-");

      // Remove multiple consecutive hyphens
      value = Regex.Replace(value, @"-+", "-");

      // Trim hyphens from start and end
      return value.Trim('-');
    }

    /// <summary>
    /// Generates a random string of specified length using alphanumeric characters.
    /// </summary>
    public static string GenerateRandomString(int length)
    {
      const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
      var random = new Random();
      var result = new StringBuilder(length);

      for (int i = 0; i < length; i++)
      {
        result.Append(chars[random.Next(chars.Length)]);
      }

      return result.ToString();
    }

    /// <summary>
    /// Masks sensitive information like credit card numbers or phone numbers.
    /// </summary>
    public static string MaskSensitiveInfo(this string value, int visibleStart = 4, int visibleEnd = 4, char maskChar = '*')
    {
      if (value.IsNullOrEmpty() || value.Length <= visibleStart + visibleEnd) return value;

      var start = value.Substring(0, visibleStart);
      var end = value.Substring(value.Length - visibleEnd);
      var maskLength = value.Length - visibleStart - visibleEnd;

      return start + new string(maskChar, maskLength) + end;
    }

    /// <summary>
    /// Checks if a string contains only alphabetic characters.
    /// </summary>
    public static bool IsAlphabetic(this string value)
    {
      if (value.IsNullOrEmpty()) return false;
      return Regex.IsMatch(value, @"^[a-zA-Z]+$");
    }

    /// <summary>
    /// Checks if a string contains only numeric characters.
    /// </summary>
    public static bool IsNumeric(this string value)
    {
      if (value.IsNullOrEmpty()) return false;
      return Regex.IsMatch(value, @"^[0-9]+$");
    }

    /// <summary>
    /// Checks if a string contains only alphanumeric characters.
    /// </summary>
    public static bool IsAlphanumeric(this string value)
    {
      if (value.IsNullOrEmpty()) return false;
      return Regex.IsMatch(value, @"^[a-zA-Z0-9]+$");
    }

    /// <summary>
    /// Converts the first character of a string to uppercase.
    /// </summary>
    public static string FirstCharToUpper(this string value)
    {
      if (value.IsNullOrEmpty()) return value;

      return char.ToUpper(value[0]) + value.Substring(1);
    }

    /// <summary>
    /// Safely gets a substring without throwing exceptions.
    /// </summary>
    public static string SafeSubstring(this string value, int startIndex, int length)
    {
      if (value.IsNullOrEmpty()) return string.Empty;

      if (startIndex < 0) startIndex = 0;
      if (startIndex >= value.Length) return string.Empty;

      length = Math.Min(length, value.Length - startIndex);
      return value.Substring(startIndex, length);
    }
  }
}