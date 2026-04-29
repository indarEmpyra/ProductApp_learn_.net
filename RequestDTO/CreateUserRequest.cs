
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ProductApp.RequestDTO
{
  public class CreateUserRequest
  {
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(100)]
    public string? LastName { get; set; }

    [Required]
    [MaxLength(150)]
    [EmailAddress]
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
  }
}
