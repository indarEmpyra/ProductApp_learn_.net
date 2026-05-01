using Microsoft.EntityFrameworkCore;
// if missing, install it [dotnet add package Microsoft.EntityFrameworkCore] 
// is this a global installation? No, you need to run this command in the project directory where your .csproj file is located.
// This is required to use the DbContext and related EF Core functionality in your application.
using ProductApp.Models;

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




// ! How it works:

// EF Core sees DbSet<Product> inside AppDbContext and knows to create/manage a Products table in the database.
// Each property in the Product model becomes a column in that table.
// Each instance of Product represents a row.
// Usage example:

// Think of DbSet<T> as EF Core's way of saying: "This C# class maps to a database table, and here's how you interact with it."