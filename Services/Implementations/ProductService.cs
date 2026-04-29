using ProductApp.Data;
using ProductApp.Models;
using ProductApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ProductApp.Services.Implementations
{
  public class ProductService : IProductService
  {
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
      _context = context;
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
      return await _context.Products.ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
      return await _context.Products.FindAsync(id);
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
      _context.Products.Add(product);
      await _context.SaveChangesAsync();
      return product;
    }

    public async Task<Product?> UpdateProductAsync(int id, Product product)
    {
      var existingProduct = await _context.Products.FindAsync(id);
      if (existingProduct == null) return null;

      existingProduct.Name = product.Name;
      existingProduct.Description = product.Description;
      existingProduct.Price = product.Price;
      existingProduct.Quantity = product.Quantity;

      await _context.SaveChangesAsync();
      return existingProduct;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
      var product = await _context.Products.FindAsync(id);
      if (product == null) return false;

      _context.Products.Remove(product);
      await _context.SaveChangesAsync();
      return true;
    }
  }
}



// ? This class implements the IProductService interface, providing concrete implementations for all the methods defined in the interface.
// public	Access modifier — this method is accessible from anywhere
// async	Marks the method as asynchronous; allows use of await inside it
// Task<List<Product>>	The return type — a Task wrapping a List<Product>
// GetAllProductsAsync	Method name — the Async suffix is a convention for async methods
// ()	No parameters needed


// ? Why Task<List<Product>> instead of just List<Product>?
// Because the method is async, it can't return a plain value directly. Instead:
// Task represents an ongoing async operation (like a promise)
// Task<List<Product>> means: "this operation will eventually give you a List<Product>"
// The caller uses await to unwrap the result:


// ToListAsync() is defined in the Microsoft.EntityFrameworkCore namespace — specifically as an extension method on IQueryable<T>.
// Source: Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions
// Breaking down the full line:

// ? return await _context.Products.ToListAsync();
// _context.Products	Returns an IQueryable<Product> — a query not yet executed
// .ToListAsync()	Sends the SQL query to the DB and returns results as List<Product>
// await	Waits for the async DB call to finish without blocking the thread
// return	Returns the List<Product> to the caller

// ?Is ToListAsync() a convention?
// Yes — it follows the Async suffix convention in .NET. EF Core provides async versions of all common LINQ terminal operations:

// Sync (blocks thread),	Async (non-blocking)
// .ToList()	
// .ToListAsync()
// .FirstOrDefault()	
// .FirstOrDefaultAsync()
// .Find()	
// .FindAsync()
// .Count()	
// .CountAsync()
// .Any()	
// .AnyAsync()
// .SingleOrDefault()	
// .SingleOrDefaultAsync()
// .SaveChanges()  
// .SaveChangesAsync()

// Rule of thumb: In a web API, always prefer the Async versions for DB calls. 
// This frees up the thread to serve other HTTP requests while waiting for the database — instead of sitting idle and blocking.

// ? Why it needs using Microsoft.EntityFrameworkCore:

// ToListAsync() doesn't exist on IQueryable<T> by default 
// — it's added via extension methods from that namespace. 
// Without the using, you'd get a compile error even though .ToList() (LINQ, synchronous) would still work.