namespace ProductApp.Helpers
{
  /// <summary>
  /// Static helper class for common DateTime operations.
  /// Ensures consistent date/time handling across the application.
  /// </summary>
  public static class DateTimeHelper
  {
    /// <summary>
    /// Gets the current UTC date and time.
    /// Use this instead of DateTime.Now for consistency.
    /// </summary>
    public static DateTime UtcNow => DateTime.UtcNow;

    /// <summary>
    /// Converts a DateTime to UTC if it's not already.
    /// </summary>
    public static DateTime ToUtc(this DateTime dateTime)
    {
      return dateTime.Kind == DateTimeKind.Utc ? dateTime : dateTime.ToUniversalTime();
    }

    /// <summary>
    /// Checks if a date is in the past (compared to UTC now).
    /// </summary>
    public static bool IsInThePast(this DateTime dateTime)
    {
      return dateTime.ToUtc() < UtcNow;
    }

    /// <summary>
    /// Checks if a date is in the future (compared to UTC now).
    /// </summary>
    public static bool IsInTheFuture(this DateTime dateTime)
    {
      return dateTime.ToUtc() > UtcNow;
    }

    /// <summary>
    /// Calculates the age in years from a birth date.
    /// </summary>
    public static int CalculateAge(this DateTime birthDate)
    {
      var today = UtcNow.Date;
      var age = today.Year - birthDate.Year;

      if (birthDate.Date > today.AddYears(-age))
        age--;

      return age;
    }

    /// <summary>
    /// Formats a DateTime to a standard ISO 8601 string.
    /// </summary>
    public static string ToIso8601String(this DateTime dateTime)
    {
      return dateTime.ToString("yyyy-MM-ddTHH:mm:ssZ");
    }

    /// <summary>
    /// Gets the start of the day for a given DateTime.
    /// </summary>
    public static DateTime StartOfDay(this DateTime dateTime)
    {
      return dateTime.Date;
    }

    /// <summary>
    /// Gets the end of the day for a given DateTime.
    /// </summary>
    public static DateTime EndOfDay(this DateTime dateTime)
    {
      return dateTime.Date.AddDays(1).AddTicks(-1);
    }

    /// <summary>
    /// Checks if two DateTime values are on the same day.
    /// </summary>
    public static bool IsSameDay(this DateTime dateTime1, DateTime dateTime2)
    {
      return dateTime1.Date == dateTime2.Date;
    }

    /// <summary>
    /// Adds business days to a DateTime, skipping weekends.
    /// </summary>
    public static DateTime AddBusinessDays(this DateTime dateTime, int businessDays)
    {
      var result = dateTime;
      var direction = businessDays < 0 ? -1 : 1;
      businessDays = Math.Abs(businessDays);

      while (businessDays > 0)
      {
        result = result.AddDays(direction);
        if (result.DayOfWeek != DayOfWeek.Saturday && result.DayOfWeek != DayOfWeek.Sunday)
        {
          businessDays--;
        }
      }

      return result;
    }
  }
}