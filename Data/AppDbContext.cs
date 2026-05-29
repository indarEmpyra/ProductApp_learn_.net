using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;

//# using Microsoft.EntityFrameworkCore;
// if missing, install it [dotnet add package Microsoft.EntityFrameworkCore] 
// is this a global installation? No, you need to run this command in the project directory where your .csproj file is located.
// This is required to use the DbContext and related EF Core functionality in your application.

using ProductApp.Models;
// We need this line to access our Product and User models, which are likely defined in the ProductApp.Models namespace.

namespace ProductApp.Data

{
  public class AppDbContext : DbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
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