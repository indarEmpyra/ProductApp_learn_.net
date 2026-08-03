using System.ComponentModel.DataAnnotations;
using ProductApp.Models;

namespace ProductApp.RequestDTO
{
  public class ChangeUserStatusRequest
  {
    [Required]
    public UserStatus Status { get; set; }
  }
}
