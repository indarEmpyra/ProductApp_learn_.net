using ProductApp.Data;
using ProductApp.Models;
using ProductApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

//@ Namespaces must be imported explicitly (usually) to use classes and methods defined in other namespaces.
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;

// What these give you:

// System -->	Console, DateTime, String
// System.Collections.Generic	--> List<T>, Dictionary
// System.Linq -->	Where, Select, ToList, etc. (LINQ methods)
// System.Threading.Tasks -->	Task, Task<T>, async/await support


// Without "using"??

// This will fail:
// List<int> numbers = new List<int>();

// ✅ You must write:
// System.Collections.Generic.List<int> numbers =
//     new System.Collections.Generic.List<int>();

// System.Collections.Generic.List<string> tasks =
//     new System.Collections.Generic.List<string>();


// In .NET 6+ you may notice:
// 👉 You sometimes don’t need using System

// Why?
// Implicit global usings

// Your project might auto-include:

// global using System;
// global using System.Collections.Generic;

// So this works without explicit using:

// List<int> nums = new();
// Example: List<int> nums = new(); works without using System.Collections.Generic; because of implicit global usings 
//enabled in the .csproj file.
// This is a feature that allows you to avoid having to write common using directives in every file

// Where is this defined?
// In .csproj:
// <ImplicitUsings>enable</ImplicitUsings>

namespace ProductApp.Services.Implementations
{
  public class UserService : IUserService
  {
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
      _context = context;
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
      return await _context.Users.ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
      return await _context.Users.FindAsync(id);
    }

    public async Task<User> CreateUserAsync(User user)
    {
      _context.Users.Add(user);
      await _context.SaveChangesAsync();
      return user;
    }

    public async Task<User?> UpdateUserAsync(int id, User user)
    {
      var existingUser = await _context.Users.FindAsync(id);
      if (existingUser == null) return null;

      existingUser.FirstName = user.FirstName;
      existingUser.LastName = user.LastName;
      existingUser.Email = user.Email;
      existingUser.PhoneNumber = user.PhoneNumber;
      existingUser.IsActive = user.IsActive;

      await _context.SaveChangesAsync();
      return existingUser;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
      var user = await _context.Users.FindAsync(id);
      if (user == null) return false;

      _context.Users.Remove(user);
      await _context.SaveChangesAsync();
      return true;
    }
  }
}


// ? This class implements the IUserService interface, providing concrete implementations for all the methods defined in the interface.

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
// .ToList()	.ToListAsync()
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

// Rule of thumb: In a web API, always prefer the Async versions for DB calls. This frees up the thread to serve other HTTP requests while waiting for the database — instead of sitting idle and blocking.

// ? Why it needs using Microsoft.EntityFrameworkCore:

// ToListAsync() doesn't exist on IQueryable<T> by default 
// — it's added via extension methods from that namespace. 
// Without the using, you'd get a compile error even though .ToList() (LINQ, synchronous) would still work.





//! LINQ db methods

// Where: filters records based on a condition.
// Example: _users.Where(u => u.IsActive) retrieves all active users.

// Select, SelectMany: selects specific columns or transforms data into new objects.
// Example: _users.Select(u => new { u.FirstName, u.LastName }) creates an anonymous type with just the first and last names.

// Ordering: OrderBy, ThenBy: sorts data in ascending or descending order.
// Example: _users.OrderBy(u => u.LastName) sorts users by last name.
// OrderByDescending(key) sorts in descending order, and ThenBy allows for secondary sorting (e.g., OrderBy(u => u.LastName).ThenBy(u => u.FirstName)).
// ThenBy is used for secondary sorting after an initial OrderBy. For example, if you want to sort users first by last name and then by first name, you would use:
// _users.OrderBy(u => u.LastName).ThenBy(u => u.FirstName)

// Joining: Join, GroupJoin: combines data from multiple tables based on related keys.
// Example: _users.Join(_orders, u => u.Id, o => o.UserId, (u, o) => new { User = u, Order = o }) joins users with their orders.
// Other types of join examples: Left Join (DefaultIfEmpty), Cross Join (SelectMany without a where condition)  

// Grouping: GroupBy: groups records based on a key and allows aggregation.
// Example: _orders.GroupBy(o => o.UserId) groups orders by user ID, allowing you to perform aggregate functions like Count, Sum, etc., on each group.

// Aggregation: Count, Sum, Min, Max, Average: performs mathematical calculations on a column.
// Example: _orders.Count() returns the total number of orders, while _orders.Sum(o => o.TotalAmount) calculates the total revenue from all orders.
// max, min, average examples: _orders.Max(o => o.TotalAmount) gives the highest order amount, _orders.Min(o => o.TotalAmount) gives the lowest, 
// and _orders.Average(o => o.TotalAmount) calculates the average order amount. 

// Partitioning: Take, Skip: used for paging (e.g., "take the first 10" or "skip the first 20").
// Example: _users.Skip(20).Take(10) retrieves users 21-30 for pagination.

// Quantifiers: Any, All, Contains: checks if any or all elements meet a specific condition.
// Example: _users.Any(u => u.Email == email) checks if any user has the specified email, while _users.All(u => u.IsActive) checks if all users are active.




// ToList(): Executes the query and returns a List<T>.
// Example: _context.Products.ToList() retrieves all products from the database and returns them as a List<Product>.

// ToArray(): Executes the query and returns an array.
// Example: _context.Products.ToArray() retrieves all products and returns them as a Product[].

// First() / FirstOrDefault(): Retrieves the first matching record.
// Example: _context.Products.First(p => p.Id == id) retrieves the first product with the specified ID; throws an error if not found.
// FirstOrDefault() does the same but returns null if no match is found instead of throwing an error.

// Single() / SingleOrDefault(): Retrieves exactly one record; throws an error if more than one exists.
// Example: _context.Products.Single(p => p.Id == id) retrieves the single product with the specified ID; throws an error if not found or if multiple matches exist.
// SingleOrDefault() returns null if no match is found, but still throws an error if multiple matches exist.

// Insert(): Adds a new record to the table.
// Example: _context.Products.Add(newProduct) adds a new product to the database context, and SaveChanges() commits it to the database.
// Insert multiple records: _context.Products.AddRange(newProducts) adds a list of new products in one operation, 
// which is more efficient than adding them one by one.

// Update(): Modifies existing records.
// Example: var product = _context.Products.Find(id); product.Name = "New Name"; _context.SaveChanges() updates the product's name in the database.

// Delete(): Removes records from the database.
// Example: var product = _context.Products.Find(id); _context.Products.Remove(product); _context.SaveChanges() deletes the product from the database.
// is this the optimized way to delete records?
// For deleting multiple records, it's more efficient to use a batch delete approach rather than fetching each record and deleting them one by one.
// For example, using a library like Entity Framework Plus, you can perform a batch delete with a single database call:
// _context.Products.Where(p => p.IsDiscontinued).Delete() deletes all discontinued products in one operation,
// which is much faster than fetching each product and deleting them individually.  


// BulkCopy(): Efficiently inserts large amounts of data.
// Example: Using SqlBulkCopy in ADO.NET to insert a DataTable of products into the database in one operation, 
// which is much faster than inserting records one by one.