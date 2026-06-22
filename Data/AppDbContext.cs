using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;

//# using Microsoft.EntityFrameworkCore;
// if missing, install it [dotnet add package Microsoft.EntityFrameworkCore] 
// is this a global installation? No, you need to run this command in the project directory where your .csproj file is located.
// This is required to use the DbContext and related EF Core functionality in your application.

using ProductApp.Models;
// We need this line to access our Product and User models, which are likely defined in the ProductApp.Models namespace.

namespace ProductApp.Data
// We created this namespace to organize our data access code. 
// It helps to keep our project structured and makes it easier to find related classes.

{
  public class AppDbContext : DbContext
  // AppDbContext is the class that represents the session with the database and allows us to query and save data.  
  // this inherits from DbContext, which is the base class for working with Entity Framework Core. 
  // By inheriting from DbContext, AppDbContext gains all the functionality needed to interact with the database, such as querying and saving data. 
  {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    // This constructor is used to pass the configuration options for the DbContext, such as the connection string and other settings.
    //# The options parameter is of type DbContextOptions<AppDbContext>, which contains the configuration for the context.
    // The base(options) call passes these options to the base DbContext class, allowing it to be properly configured when the context is instantiated.
    // This constructor is necessary for dependency injection to work correctly.

    //# base(options) calls the constructor of the base class (DbContext) and passes the options to it.
    // This is important because the DbContext needs to be configured with the options provided, such as the connection 
    // string and other settings, in order to function properly.

    // Is this the way we we inherit and use base classes in C#? Yes, this is a common pattern in C#. 
    // When you create a class that inherits from a base class, you can use the base keyword to call the constructor of the base class 
    // and pass any necessary parameters. 
    // This allows you to ensure that the base class is properly initialized when your derived class is instantiated. 
    // In this case, AppDbContext is inheriting from DbContext, and by calling base(options), we are ensuring that the DbContext 
    // is configured with the options provided when AppDbContext is created.
    {
      // The constructor is empty because all the necessary configuration is handled by the base class (DbContext) through the options parameter.

      // What could we put here? You could add any additional initialization code that you want to run when an instance of AppDbContext is created. 
      // For example, you could set up logging, initialize certain properties, or perform any other setup tasks that are specific to your application's needs. 
      // However, in most cases, the base constructor will handle the necessary configuration, and you may not need to add anything here. 
    }

    // Add your DbSets here
    // public DbSet<Product> Products { get; set; }
    // public DbSet<Category> Categories { get; set; }
    // For example, if you have a Product model, you would add:
    public DbSet<Product> Products { get; set; }

    public DbSet<User> Users { get; set; }

    // DbSet<Product> Products means that there is a table called Products mapped to Product class

    // Override OnModelCreating if you need to configure your model further
    // This method is called when the model for a derived context has been initialized, but before the model has been locked down
    // and used to initialize the context.

    // You can use this method to configure the model using the ModelBuilder API, 
    // such as setting up relationships, configuring table names, etc.

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
      // Add your model configurations here
    }
  }
}


// ? If I only register AppDbContext in Program.cs, how does my service know about TaskItem or tables?
// DbContext is the bridge between your code and database schema. When you define a DbSet<T> in your DbContext, 
// EF Core uses that to understand how to map your C# classes to database tables.  
// public DbSet<Product> Products { get; set; }
// public DbSet<User> Users { get; set; }



// ! How it works:

// EF Core sees DbSet<Product> inside AppDbContext and knows to create/manage a Products table in the database.
// Each property in the Product model becomes a column in that table.
// Each instance of Product represents a row.
// Usage example:

// Think of DbSet<T> as EF Core's way of saying: "This C# class maps to a database table, and here's how you interact with it."


//? Database connection strings are typically stored in appsettings.json under a "ConnectionStrings" section.
// For example: This is of SQL express localdb connection string, which is commonly used for development purposes. 
// It connects to a local SQL Server instance (localdb) and uses a database named ProductDb.
//  "Server=localhost;Database=JobsDb_Dev",

// This is the connection string for a local SQL Server instance (localdb) with a database named ProductDb.
// The Trusted_Connection=true; part indicates that Windows Authentication is used to connect to the database, 
// and TrustServerCertificate=True; allows the client to trust the server's SSL certificate without validation 
// (useful for development environments).
//  "Server=(localdb)\\mssqllocaldb;Database=ProductDb;Trusted_Connection=true;TrustServerCertificate=True;"





//? Where entities are “connected” to DbContext?
// Inside your AppDbContext:

// public class AppDbContext : DbContext
// {
//     public DbSet<TaskItem> Tasks { get; set; }
// 

//? What this means
// DbSet<TaskItem> Tasks

// Tells Entity Framework:

// "There is a table called Tasks
// mapped to TaskItem class"

// TaskItem (Entity)
//    ↓
// Defined inside DbContext
//    ↓
// DbContext registered in DI
//    ↓
// Injected into Service
//    ↓
// Service can access _context.Tasks


// 🔥 So flow becomes:
// TaskItem (Entity)
//    ↓
// Defined inside DbContext
//    ↓
// DbContext registered in DI
//    ↓
// Injected into Service
//    ↓
// Service can access _context.Tasks


// 🧠 In your service:
// private readonly AppDbContext _context;

// public TaskService(AppDbContext context)
// {
//     _context = context;
// }



//# 🔥 Summary
// Entities → defined in DbContext
// DbContext → registered in DI
// Service → gets DbContext injected
// Service → accesses entities via DbSet



//-------------------------------------------------------------------------------------------------//

//? Instead of using a connection string, you can also configure the DbContext to use an in-memory database for testing purposes.
// This is useful for unit testing or when you want to quickly set up a database without needing to configure a real database server.
// In your Program.cs, you can configure the DbContext to use an in-memory database like this:
// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseInMemoryDatabase("TestDb"));
// This will create an in-memory database named "TestDb" that you can use for testing.


//? How to save anything to the in-memory/RAM?
// To save data to the in-memory database, you would use the same code as you would with a real database.
// For example, in your service or controller, you can do something like this:
// var newProduct = new Product { Name = "Sample Product", Price = 9.99m };
// _context.Products.Add(newProduct);
// _context.SaveChanges();
// This code creates a new Product instance, adds it to the Products DbSet, and then calls SaveChanges() to persist the changes to the in-memory database.

//? Instead of inheriting from DbContext, can we inherit from a custom baseClass that itself inherits from DbContext?
// Yes, you can create a custom base class that inherits from DbContext and then have your AppDbContext inherit from that custom base class.
// For example:
// public class MyBaseDbContext : DbContext
// {
//     public MyBaseDbContext(DbContextOptions options) : base(options)
//     {
//     }
//     // You can add common functionality or properties here that you want to share across multiple DbContext classes.

// For example, have a method to format the response sent to the client in a specific way, 
// you can add that functionality in MyBaseDbContext and then have AppDbContext inherit from it.

// public FormatResponse<T>(T data) 
// {
//     // How to format the response sent to the client in a specific way
//     // For example, you can wrap the data in a standard response object with additional metadata
//    return new StandardResponse<T>
//    {
//        Data = data,
//        Timestamp = DateTime.UtcNow,
//        Status = "Success"
//    };
//
// }



// }

// public class AppDbContext : MyBaseDbContext
// {
//     public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
//     {
//     }
//     public DbSet<Product> Products { get; set; }
//     public DbSet<User> Users { get; set; }
// }

// In this example, MyBaseDbContext is a custom base class that inherits from DbContext, and AppDbContext inherits from MyBaseDbContext. 
// This allows you to add any shared functionality or properties in MyBaseDbContext that can be used by multiple DbContext classes if needed. 

//? why would we inherit from a custom base class instead of directly from DbContext?
// Inheriting from a custom base class can be useful if you have common functionality or properties that you want to share across multiple DbContext classes.
// For example, if you have multiple DbContext classes for different parts of your application (e.g., one for main application data and another for logging), 
// you might want to put shared logic, such as common configurations, logging, or utility methods, in a custom base class.
// This way, you can avoid code duplication and keep your DbContext classes cleaner and more focused on their specific responsibilities.

//? Can we have multiple DbContext classes in the same project?
// Yes, you can have multiple DbContext classes in the same project.
// For example, you might have one DbContext for your main application data and another DbContext for logging or auditing purposes.
// Each DbContext can be configured separately and can have its own set of DbSet properties that represent different tables in the database.
// Just make sure to register each DbContext in your Program.cs with the appropriate configuration.






//? Example of AppDbContext inheriting from a custom base class that itself inherits from DbContext and the custom base 
//? class should have a method to format the response sent to the client in a specific way:

// // 1. Custom Base Class inheriting DbContext
// public abstract class CustomBaseDbContext : DbContext
// {
//   protected CustomBaseDbContext(DbContextOptions options) : base(options) { }

//   // Custom method to format the client response
//   public string FormatResponseForClient<T>(T data)
//   {
//     var options = new JsonSerializerOptions
//     {
//       PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
//       WriteIndented = true,
//       // Add any additional formatting rules here
//     };

//     return JsonSerializer.Serialize(data, options);
//   }
// }

// // 2. AppDbContext inheriting the custom base class
// public class AppDbContext : CustomBaseDbContext
// {
//   public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

//   public DbSet<Product> Products { get; set; }
// }

// // Example Entity
// public class Product
// {
//   public int Id { get; set; }
//   public string Name { get; set; }
//   public decimal Price { get; set; }
// }


// How to use the FormatResponseForClient method in your service or controller:
// public class ProductService
// {
//   private readonly AppDbContext _context;
//   public ProductService(AppDbContext context)
//   {
//   _context = context;
// }
//   public string GetFormattedProducts()
//   {
//     var products = _context.Products.ToList();
//     return _context.FormatResponseForClient(products);
//   }
// }
// In this example, the ProductService retrieves a list of products from the database and then uses the FormatResponseForClient 
// method from the AppDbContext to format the response before sending it to the client.



