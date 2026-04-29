namespace ProductApp.ResponseDTO
{
  public class UserResponse
  {
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; }
    // public string Email { get; set; } = string.Empty;
    // public string? PhoneNumber { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; }
  }
}
