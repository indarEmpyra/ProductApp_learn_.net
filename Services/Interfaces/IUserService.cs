using ProductApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductApp.Services.Interfaces
{
  public interface IUserService
  {
    Task<List<User>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(int id);
    Task<User> CreateUserAsync(User user);
    Task<User?> UpdateUserAsync(int id, User user);
    Task<bool> DeleteUserAsync(int id);
  }
}