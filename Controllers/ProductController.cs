using Microsoft.AspNetCore.Mvc;
using ProductApp.Services.Interfaces;
using ProductApp.Models;

namespace ProductApp.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class ProductController : ControllerBase
  {
    private readonly IProductService _productService;
    public ProductController(IProductService productService)
    {
      _productService = productService;
    }


    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
      var products = await _productService.GetAllProductsAsync();
      return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
      var product = await _productService.GetProductByIdAsync(id);
      if (product == null)
      {
        return NotFound();
      }
      return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] Product product)
    {
      await _productService.CreateProductAsync(product);
      return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
    {
      await _productService.UpdateProductAsync(id, product);
      return Ok(new { message = $"Updated product {id}" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
      await _productService.DeleteProductAsync(id);
      return Ok(new { message = $"Deleted product {id}" });
    }
  }
}