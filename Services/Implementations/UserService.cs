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





//# Do we really need to import System.Collections.Generic to use List<T>? or System.Threading.Tasks to use Task<T>?
// Yes, in a typical .NET project, you need to import the System.Collections.Generic namespace to use List<T> and System.Threading.Tasks to use Task<T>.

//# But, we are using Task<List<User>> without importing System.Threading.Tasks, and List<User> without importing System.Collections.Generic, how is that possible?
// This is possible because of implicit global usings, a feature introduced in .NET 6. If your project has implicit 
// global usings enabled (which is common in new .NET projects), then certain namespaces are automatically included in every file,
//  allowing you to use types like List<T> and Task<T> without explicitly importing their namespaces in each file.


//# Without "using System.Collections.Generic;"?

// This will fail:
// List<int> numbers = new List<int>();

// ✅ You must write:
// System.Collections.Generic.List<int> numbers =
//     new System.Collections.Generic.List<int>();

// System.Collections.Generic.List<string> tasks =
//     new System.Collections.Generic.List<string>();


// In .NET 6+ you may notice:
// 👉 You sometimes don’t need using System

//# Why?
// Implicit global usings

// Your project might auto-include:

// global using System;
// global using System.Collections.Generic;

// So this works without explicit using:

// List<int> nums = new();
// Example: List<int> nums = new(); works without using System.Collections.Generic; because of implicit global usings 
// enabled in the .csproj file.
// This is a feature that allows you to avoid having to write common using directives in every file

//# Where is this defined?
// In .csproj:
// <ImplicitUsings>enable</ImplicitUsings>

//# In .NET 6 and later, implicit global usings are enabled by default in ASP.NET Core projects. 
// This means you don't have to explicitly import commonly used namespaces like:
// - System
// - System.Collections.Generic
// - System.Linq
// - System.Threading.Tasks
// - Microsoft.AspNetCore.Builder
// - Microsoft.Extensions.DependencyInjection
// - Microsoft.Extensions.Hosting

namespace ProductApp.Services.Implementations
{
  public class UserService : IUserService

  // IUserService(Interface) is a contract that defines the methods and properties that a class must implement.
  // UserService(Implementation) is a concrete class that provides the actual implementation of the methods defined in the IUserService interface.

  // IUserService
  // This means the class implements an interface.

  //   Why.NET uses interfaces everywhere
  // For:
  // dependency injection
  // testing
  // loose coupling


  //# Does dependency injection container support only the built-in pairing of interface and implementation?
  // No, the dependency injection container in .NET supports any pairing of interface and implementation, 
  // allowing you to register and resolve dependencies for any interface and its corresponding implementation,
  // not just the built-in ones. You can register your own interfaces and implementations as needed 
  // for your application, and the DI container will handle the resolution of those dependencies at runtime.

  // Example:
  // public interface IMyService
  // {
  //     void DoSomething();
  // }

  // public class MyService : IMyService
  // {
  //     public void DoSomething()
  //     {
  // Implementation code here
  //     }
  // }








  // Example:

  // Controller depends on IUserService

  // But actual runtime object is:

  // UserService

  // This allows swapping implementations easily.

  // Example:

  // MockUserService
  // CachedUserService
  // RemoteUserService

  {
    private readonly AppDbContext _context;

    // private means only this class can access _context
    // readonly Means: Value can only be assigned once Usually in constructor. After that it cannot change.

    // Example:
    // _repo = repo;
    // But later this would fail:
    // _repo = newRepo; // not allowed

    // Why used?

    // immutability
    // thread safety
    // good practice

    //# AppDbContext

    // This is the type of the variable.
    // It says:

    // _context must implement AppDbContext
    // _context is a reference to an object that can perform database operations defined in AppDbContext, 
    // such as querying and saving data related to users, products, etc.

    // Example interface:

    // public interface AppDbContext
    // {
    //     Task<List<Task>> GetAll();
    // }

    //#  _context

    // This is the variable name.
    // Convention in .NET:
    // _privateField

    // Examples:

    // _userService
    // _context
    // _logger


    public UserService(AppDbContext context)
    {
      _context = context;
    }

    //# This is the constructor. A constructor runs when the object is created.
    // Why this constructor exists?
    // To inject dependencies.

    // ASP.NET automatically creates objects using Dependency Injection.

    // Example runtime flow:
    // Controller needs TaskService
    // ↓
    // DI container creates TaskService
    // ↓
    // DI injects ITaskRepository

    //# _context = context;
    // This assigns the injected AppDbContext instance to the private readonly field _context, allowing the service to use it 
    // for database operations throughout the class.
    //  This assigns the injected dependency to the class field.

    // Can you elaborate on this assignment and why it's important?
    // This assignment is crucial because it allows the UserService class to use the AppDbContext instance that is provided by the Dependency Injection container.
    // By assigning the injected context to the private readonly field _context, the service can perform database operations such as querying users,
    //  adding new users, updating existing users, and deleting users throughout the class methods.
    // Without this assignment, the service would not have access to the database context and would not 
    // be able to interact with the database, making it impossible to implement the required functionality defined in the IUserService interface.

    // JS equivalent: this.repo = repo

    public async Task<List<User>> GetAllUsersAsync()

    // Public async method
    // named GetAllUsersAsync
    // returns a Promise(Task)
    // that resolves to a List of User objects

    //# async means the method contains asynchronous operations and can use await.

    //# Return type: Task<List<User>>
    // In .NET: Task = Promise
    // Return type: Promise<User[]> : A return type of user objects array wrapped in a promise, indicating an asynchronous operation 
    // that will eventually produce an array of user objects.

    // possible return types:
    // Task<User> - returns a single user asynchronously
    // Task<List<User>> - returns a list of users asynchronously
    // Task<UserResponse> - returns a single user response DTO asynchronously
    // Task<bool> - returns a boolean indicating success/failure of an async operation (e.g., delete)
    // Task<int> - returns an integer result asynchronously (e.g., number of records affected)
    // Task<string> - returns a string result asynchronously (e.g., a message or status)
    // Task - returns a void result asynchronously (e.g., for operations that don't return data)


    //# List<User> means: "a list of User objects"
    // List of User objects

    // Equivalent Node:

    // User[]

    // Example class:

    // public class User
    //     {
    //       public int Id { get; set; }
    //       public string Title { get; set; }
    //     }

    //     Then:

    // List<User>

    // Means:

    // [
    //   User,
    //   User,
    //   User
    // ]

    //# Task<List<User>> means: "an asynchronous operation that will eventually return a List<User>"
    {
      return await _context.Users.ToListAsync();

      //# _context.Users returns an IQueryable<User> representing the Users table in the database.
      // .ToListAsync() executes the query and returns the results as a List<User> asynchronously.
      // await is used to wait for the asynchronous operation to complete without blocking the thread.  

      //# Why not just return _context.Users.ToList()?
      // Because ToList() is synchronous and would block the thread while waiting for the database operation to complete, 
      // which can lead to performance issues in a web application.


      //? When does the _context gets populated with data from the database?
      // The _context is populated with data from the database when you execute a query against it, such as calling ToListAsync() on a DbSet.
      // Until you execute a query, the _context is just a representation of the database and does not contain any data. When you call a method like ToListAsync(), 
      // it sends a query to the database, retrieves the data, and populates the _context with the results, which are then returned as a List<User>. 

      // So, when we inject AppDbContext into DI container, it creates an instance of AppDbContext, but it does not automatically populate it with data. 
      // The data is only fetched when you execute a query against the DbSet properties of the context, such as _context.Users.ToListAsync().

      //# _context holding the whole database, doesn't this consume a lot of memory?
      // No, the _context itself does not hold the entire database in memory. It is a lightweight object that represents a session with the database. 
      // The actual data is only loaded into memory when you execute a query against the DbSet properties, such as _context.Users.ToListAsync().
      // When you execute a query, only the relevant data is fetched from the database and loaded into memory. 
      // The _context manages the connection to the database and tracks changes to entities, but it does not load the entire database into memory at once. 
      // This allows for efficient data access and manipulation without consuming excessive memory resources.   

      // Is it that, when we say _context.Users, it does not immediately fetch all users from the database, but rather creates a queryable object that can be further refined with LINQ methods, 
      // and only when we call ToListAsync() does it execute the query and fetch the relevant data?
      // Yes, that's correct. When you access _context.Users, it returns an IQueryable<User> which is a queryable object that represents the Users table in the database.
      // You can further refine this queryable object using LINQ methods like Where, Select, etc. However, no data is fetched from the database at this point. 
      // The actual database query is only executed when you call a terminal method like ToListAsync(), which then fetches the relevant 
      // data based on the defined query and loads it into memory as a List<User>. This allows for efficient querying and data retrieval without loading unnecessary data into memory. 

      //# How does it manage to fetch only relevant data without loading the entire database?
      // The _context uses a feature called "lazy loading" and "query translation" to fetch only the relevant data. When you execute a 
      // query against a DbSet, such as _context.Users.ToListAsync(),
      // the Entity Framework translates that LINQ query into SQL and sends it to the database. The database then executes the SQL query 
      // and returns only the relevant records that match the query criteria.
      // This means that only the data that is needed for the specific query is loaded into memory instead of the entire database. 
      // The _context manages this process efficiently, allowing you to work with large databases without consuming excessive memory.   

    }


    //# When used with Entity Framework, LINQ converts your query into SQL automatically.
    // var users = dbContext.Users
    //   .Where(u => u.Age > 25)
    //     .Select(u => new { u.Name, u.Email })
    //     .ToList();

    // Generated SQL:

    // SELECT Name, Email
    // FROM Users
    // WHERE Age > 25



    //# Very Important Concept: Deferred Execution

    // LINQ queries do not run immediately.
    // Example:
    // var query = users.Where(u => u.Age > 25);
    // Nothing executes yet.
    // Execution happens when you call:

    // ToList()
    // First()
    // Count()
    // Any()

    // Example:
    // var result = users
    //     .Where(u => u.Age > 25)
    //     .ToList();

    // Now the query executes.
    // This is very important for performance.


    //# LINQ with Join (SQL Equivalent)

    // var result =
    //     from order in db.Orders
    //     join user in db.Users
    //     on order.UserId equals user.Id
    //     select new
    //     {
    //         user.Name,
    //         order.Amount
    //     };

    // SQL generated:

    // SELECT Users.Name, Orders.Amount
    // FROM Orders
    // JOIN Users ON Orders.UserId = Users.Id


    //# LINQ Syntax Types
    // There are two styles.

    // 1️⃣ Method syntax (MOST COMMON)
    // var users = db.Users
    //     .Where(u => u.Age > 25)
    //     .Select(u => u.Name);

    // 2️⃣ Query syntax (SQL-like)
    // var users =
    //     from u in db.Users
    //     where u.Age > 25
    //     select u.Name;

    //# When LINQ Can Be Dangerous

    // If used wrongly:

    // var users = db.Users.ToList();

    // var result = users.Where(u => u.Age > 25);

    // Problem:
    // Loads entire table into memory
    // Then filters in memory — very inefficient for large tables

    // Correct:

    // var users = db.Users
    //     .Where(u => u.Age > 25)
    //     .ToList();

    // Query runs in database.


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

//public async Task<List<User>> GetAllUsersAsync()
// ? This class implements the IUserService interface, providing concrete implementations for all the methods defined in the interface.

// public	Access modifier — this method is accessible from anywhere
// async	Marks the method as asynchronous; allows use of await inside it
// Task<List<Product>>	The return type — a Task wrapping a List<Product>
// GetAllProductsAsync	Method name — the Async suffix is a convention for async methods
// ()	No parameters needed

// where is User defined?
// User is defined in the ProductApp.Models namespace, likely as a class representing a user entity in the database,
// with properties like Id, FirstName, LastName, Email, etc.

// But, we don't want to return User type directly, instead want to use a DTO (Data Transfer Object) like UserResponse to 
// control what data is sent back to the client and to decouple our internal data model from the API contract then how can we return UserResponse instead of User?
// To return UserResponse instead of User, you can modify the GetAllUsersAsync method to project the User entities into UserResponse DTOs before returning them.
// Here's how you can do it using LINQ's Select method:
// public async Task<List<UserResponse>> GetAllUsersAsync()
// {
//   return await _context.Users
//     .Select(u => new UserResponse
//     {
//       Id = u.Id,
//       FirstName = u.FirstName,
//       LastName = u.LastName,
//       CreatedDate = u.CreatedDate,
//       IsActive = u.IsActive
//     })
//     .ToListAsync();
// }   


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






//? EF Core Deep Dive (Architect Level)

// Now we go deep into performance + internals.

// 🧠 1. Tracking vs No Tracking
// Tracking means EF keeps track of changes to entities. This allows updates but uses more memory and is slower for read-only queries.

// Default:

// _context.Tasks.ToListAsync();

// 👉 EF tracks objects

// Problem
// More memory usage
// Slower for read-only queries
// Solution
// _context.Tasks
//     .AsNoTracking()
//     .ToListAsync();
// Rule
// Read-only → use AsNoTracking()

// 🧠 2. N+1 Problem

// Bad:

// var tasks = await _context.Tasks.ToListAsync();

// foreach (var t in tasks)
// {
//     var project = t.Project; // triggers extra queries
// }
// Fix
// var tasks = await _context.Tasks
//     .Include(t => t.Project)
//     .ToListAsync();
// Node Equivalent
// .populate("project")

// 🧠 3. Select Projection (VERY IMPORTANT)

// Bad:

// _context.Tasks.ToListAsync();

// Better:

// _context.Tasks
//     .Select(t => new {
//         t.Id,
//         t.Title
//     })
//     .ToListAsync();
// Why?
// Fetch only needed columns
// Better performance

// 🧠 4. SaveChanges Behavior
// _context.Tasks.Add(task);
// await _context.SaveChangesAsync();

// Internally:

// 1. Track entity
// 2. Detect changes
// 3. Generate SQL
// 4. Execute SQL


// 🧠 5. Transactions
// using var transaction = await _context.Database.BeginTransactionAsync();

// try
// {
//     // multiple DB operations
//     await _context.SaveChangesAsync();

//     await transaction.CommitAsync();
// }
// catch
// {
//     await transaction.RollbackAsync();
// }

// 🧠 6. Performance Tips

// ✔ Use AsNoTracking() for reads
// ✔ Use projections (Select)
// ✔ Avoid loading full entities
// ✔ Use pagination

// 🧠 7. Pagination Example
// _context.Tasks
//     .Skip(0)
//     .Take(10)
//     .ToListAsync();


// //TODO: 🧠 Architect-Level Summary

// 1️⃣ CQRS + MediatR(matches your office Handlers folder)
// 2️⃣ Background jobs(Hangfire / HostedService)
// 3️⃣ Kafka / async messaging(aligns with your microservices goal)
// 4️⃣ Caching(Redis)



//? “DI brings dependencies automatically”
// ✅ Correct — but refine it:
// DI container creates objects ONLY if you register them

// Example:

// builder.Services.AddScoped<ITaskService, TaskService>();

// 👉 Without this, DI cannot resolve the dependency

// 🧠 Also important:

// DI resolves dependencies recursively

// Controller → Service → DbContext

// .NET will:

// 1. Create Controller
// 2. See it needs Service
// 3. Create Service
// 4. See it needs DbContext
// 5. Create DbContext