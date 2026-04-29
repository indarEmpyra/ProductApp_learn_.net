using ProductApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductApp.Services.Interfaces
{
  public interface IProductService
  {
    Task<List<Product>> GetAllProductsAsync();
    Task<Product?> GetProductByIdAsync(int id);
    Task<Product> CreateProductAsync(Product product);
    Task<Product?> UpdateProductAsync(int id, Product product);
    Task<bool> DeleteProductAsync(int id);
  }
}