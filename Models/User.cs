using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProductApp.Models;

namespace ProductApp.Models
{
  public class User
  {

    // Primary Key
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }

    [MaxLength(50)]
    public string LastName { get; set; }

    // Unique + Required
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [Phone]
    public string PhoneNumber { get; set; }


    [Required]
    [ForeignKey("RoleId")]
    // How will we know which role a user belongs to? We can add a RoleId foreign key property to the User class, 
    // and then use the [ForeignKey] attribute to specify that it references the Role entity. 
    // This way, we can establish a relationship between users and roles in our database.

    // Without mentioning table name in the [ForeignKey] attribute, EF will look for a table named "Role" by convention.
    public int RoleId { get; set; }

    public bool IsActive { get; set; } = true;

    // Date Created (UTC)
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    // Last Updated
    public DateTime? UpdatedDate { get; set; }

    // Foreign Key
    // public int RoleId { get; set; }

    [ForeignKey("RoleId")]
    public Role Role { get; set; }

    // public int Id { get; set; }
    // public required string FirstName { get; set; }
    // public string? LastName { get; set; }
    // public required string Email { get; set; }
    // public string? PhoneNumber { get; set; }
    // public DateTime CreatedDate { get; set; }
    // public bool IsActive { get; set; }
  }
}


// ✔ Required fields
// ✔ Max length
// ✔ Default values
// ✔ Primary & foreign keys
// ✔ Constraints
// ✔ Auditing fields (CreatedAt, UpdatedAt)
// ✔ Date/DateTime handling


// Types of constraints:
// ✔ Primary Key: Uniquely identifies each record (e.g., Id)
// ✔ Foreign Key: Establishes relationships between tables (e.g., RoleId in User)
// ✔ Unique Constraint: Ensures values in a column are unique (e.g., Email)
// ✔ Not Null Constraint: Ensures a column cannot have null values (e.g., Name, Email)
// ✔ Default Value: Automatically assigns a value if none is provided (e.g., IsActive defaults to true)
// ✔ Check Constraint: Ensures values in a column meet specific conditions (e.g., Price > 0)
// ✔ Auditing Fields: Track when records are created or updated (e.g., CreatedDate, UpdatedDate)
// ✔ Date/DateTime Handling: Use DateTime for timestamps, and consider using UTC to avoid timezone issues (e.g., CreatedDate = DateTime.UtcNow)  


// ✔ Primary key
// ✔ Required fields
// ✔ Max lengths
// ✔ Unique constraints
// ✔ Relationships
// ✔ Audit fields (CreatedAt, UpdatedAt)
// ✔ Indexes
// ✔ Default values
// ✔ Concurrency handling




// Navigation properties
// ?What are navigation properties? 
// They are properties that represent the relationships between entities in an ORM (Object-Relational Mapping) framework like Entity Framework. 
// They allow you to navigate from one entity to related entities. 
// For example, if a User belongs to an Organization, you can have a navigation property in the User class that references the Organization entity.
// This makes it easier to work with related data without having to manually query the database for related records.

// ?How do I define navigation properties in my entity classes?
// You can define navigation properties by adding properties to your entity classes that reference other entity types. 
// For example, in the User class, you can add a navigation property for the Organization it belongs to, like this:
// public Organization? Organization { get; set; }
// This indicates that each User is associated with one Organization.
// You can also define navigation properties for collections of related entities.
// For example, if a User can create multiple Jobs, you can add a collection navigation property like this:
// public ICollection<Job> JobsCreated { get; set; } = new List<Job>();

//? Does navigation properties mean Foreign key relationship in database?
// Navigation properties represent the relationships between entities in your code, but they do not automatically create foreign key relationships in the database.
// To establish a foreign key relationship in the database, you need to define the foreign key property in your entity 
// class and configure the relationship using data annotations or the fluent API in Entity Framework Core.
// For example, if you have an OrganizationId property in the User class that is a foreign key to the Organization entity, 
// you can decorate it with the [ForeignKey] attribute to indicate that it is related to the Organization navigation property.
// Alternatively, you can use the fluent API in the DbContext's OnModelCreating method to configure the relationships using the HasOne and WithMany methods.


// ?How do I specify foreign key relationships in Entity Framework Core?
// You can specify foreign key relationships using data annotations or the fluent API.
// Using data annotations, you can use the [ForeignKey] attribute on the foreign key property to indicate which navigation property it relates to.
// For example, if you have an OrganizationId property in the User class that is a foreign key to the Organization entity, 
// you can decorate it with [ForeignKey("Organization")] to indicate that it is related to the Organization navigation property.
// Alternatively, you can use the fluent API in the DbContext's OnModelCreating method to configure the relationships using the HasOne and WithMany methods.  

// ?How to use these navigation in services and controllers?
// In your services and controllers, you can use the navigation properties to access related data.
// For example, if you have a User entity and you want to access the Organization it belongs to, you can use the Organization navigation property like this:
// var user = _dbContext.Users.Include(u => u.Organization).FirstOrDefault(u => u.Id == userId);
// This will load the User along with its related Organization data in a single query, 
// allowing you to access the Organization details directly from the User entity without needing to make a separate query to the database.  


//? Why use navigation properties instead of just foreign key IDs?
// Navigation properties provide a more intuitive and object-oriented way to work with related data in your code.
// They allow you to access related entities directly through the navigation property, which can simplify your code and reduce the need 
//for manual queries to retrieve related data.
// For example, instead of having to query the database to get the Organization details for a User using the OrganizationId foreign key, 
//you can simply access the Organization navigation property directly from the User entity.
// This can make your code cleaner and easier to read, as it abstracts away the underlying database queries and allows you to work with your data in a more natural way.


//? is it possible to have navigation properties without foreign key properties in the entity class?
// Yes, it is possible to have navigation properties without explicitly defining foreign key properties in the entity class.
// In Entity Framework Core, if you define a navigation property without a corresponding foreign key property,
// EF will automatically create a shadow foreign key in the database to maintain the relationship between the entities.
// For example, if you have a User class with a navigation property to an Organization, but you do not define an OrganizationId foreign key property, 
// EF will still create a foreign key in the database to link the User to the Organization based on the navigation property. 
// However, it is generally recommended to define explicit foreign key properties in your entity classes, 
// as this can provide better clarity and control over the relationships between entities, and can also improve performance by allowing you to work with the 
//foreign key values directly without needing to load


// In .NET Entity Framework (EF) Core, 
// navigation properties are technically enough to define a relationship by convention, but it is strongly recommended to also 
// include explicit foreign key (FK) properties.While EF Core can automatically manage relationships using shadow properties (internal FKs not present in your C# class),
// omitting explicit FKs often leads to more complex code for common tasks like updates or filtering.

// 1. Relationship Configuration Comparison
// Navigation Properties Only: EF Core uses "shadow properties" for the FK. To update a relationship, you must usually load the full related entity from the
// database and assign it to the navigation property.
// With Explicit FK Properties: Known as a "Foreign Key Association". You can update a relationship by simply setting the ID value (e.g., product.CategoryId = 5)
// without needing to fetch the Category object first, which is much more efficient.

// 2. Benefits of Including Explicit Foreign Keys
// Including an explicit property like public int CategoryId { get; set; } alongside your navigation property public Category Category { get; set; } offers several advantages:
// Performance: You can change relationships with a single database call by updating the ID directly, rather than two calls (one to fetch the related object and one to save).
// Ease of Use in Web APIs: Disconnected entities (like those coming from a front-end JSON request) usually only provide the ID. Having an explicit FK property makes it trivial to map these incoming requests to your database entities.
// Readability & Control: It explicitly shows the database schema structure within your code and allows you to easily configure nullability (e.g., int? for an optional relationship vs int for a required one).
// Serialization: When sending entities over a network, the FK property ensures the relationship data is preserved even if the full related object (the navigation property) is not included in the payload
















// ?Is there a shortcut to generate schema from the table? 
// Yes, you can use Entity Framework Core's scaffolding feature to generate model classes from an existing database schema. 
// This process is called "reverse engineering" and it allows you to create C# classes that represent your database tables, 
// along with a DbContext class that manages the connection to the database. You can use the following command in the terminal to scaffold your models:

// // dotnet ef dbcontext scaffold "YourConnectionString" Microsoft.EntityFrameworkCore.SqlServer -o Models

// // This command will connect to your database using the provided connection string, read the schema, and generate model 
// classes in the specified output directory (in this case, "Models"). Each table in your database will be represented as a C# class, 
// and the columns will be represented as properties of those classes. This can save you a lot of time when working with an 
// existing database, as you won't have to manually create your model classes.   


//? Is there a way to generate the database schema from the model classes?
// // You start with code (model classes) and generate the database schema from it.
// You create model classes (like Product and User) and then use EF Core migrations to create the database schema based on those models.
// The migration captures the changes you made to your model and generates code to apply those changes to the database. 
//This way, you can keep your database schema in sync with your model classes as they evolve over time. 

//    - dotnet ef migrations add InitialCreate // Creates a new migration named "InitialCreate" based on the current state of your model classes. 
// This generates code that describes how to create the database schema (tables, columns, constraints) based on your model classes.
//    - dotnet ef migrations add UpdateModel
//    - dotnet ef database update 


//? Data types

// Id → int (integer, whole number)
// Name → string (text of characters size can vary  but typically limited to 255 characters in databases
// but if you want more you can use text or varchar(max) in SQL Server  or just string in C# which can handle large text)
// Description → string (text)
// Price → decimal (floating-point number)
// Quantity → int (integer, whole number)
// CreatedDate → DateTime (date and time)





//? model attributes
// [Required] → property must have a value (not null or empty)
// [MaxLength(100)] → string property cannot exceed 100 characters
// [Range(0.01, 10000)] → numeric property must be between 0.01 and 10000
// [Key] → property is the primary key of the table
// [ForeignKey("OtherEntityId")] → property is a foreign key referencing another entity
// [DatabaseGenerated(DatabaseGeneratedOption.Identity)] → value is generated by the database (e.g., auto-incrementing primary key)
// [NotMapped] → property is not mapped to a database column (used for calculated fields or properties that should not be stored in the database)
// [Index] → creates a database index on the property for faster queries (available in EF Core 5.0 and later)
// [ConcurrencyCheck] → property is used for concurrency control to prevent conflicting updates 


// ✔ Required fields
// ✔ Max length
// ✔ Default values - how to set default values for properties (e.g., CreatedDate = DateTime.UtcNow)
// ✔ Primary & foreign keys
// ✔ Constraints
// ✔ Auditing fields (CreatedAt, UpdatedAt)
// ✔ Date/DateTime handling


// ? This class represents a product entity in the database. It has properties for Id, Name, Description, Price, Quantity, and CreatedDate.
// When used with Entity Framework Core, it will automatically create a database table based on this class. The Id property will be the primary key of the table, and the other properties will be columns in the table.
// This allows us to easily perform CRUD operations on products in our database using Entity Framework Core. Migrations will help us keep our database schema in sync with our model classes as we make changes to them over time.  



// Request:
// {
//   "name": "I-Mac",
//   "description": "Awesome desktop",
//   "price": 120,
//   "quantity": 10,
// } -->

// AbandonedMutexException 
// "errors": {
//     "$": [
//       "The JSON object contains a trailing comma at the end which is not supported in this mode. 
//      Change the reader options. Path: $ | LineNumber: 5 | BytePositionInLine: 0."
//     ],
//     "product": [
//       "The product field is required."
//     ]
//   },

// This is not a code bug — your request body has a trailing comma in the JSON. For example:


/*----------------------------------------------------------------------------------------------------------*/



// EF Core maps this class → database table
// Each property → column
// Id → automatically becomes Primary Key

// This class is a simple POCO (Plain Old CLR Object) that represents a product in our application.
// It has properties for Id, Name, Description, Price, Quantity, and CreatedDate.
// When we use Entity Framework Core, it will automatically create a database table based on this class.
// The Id property will be the primary key of the table, and the other properties will be columns in the table. 
// This allows us to easily perform CRUD operations on products in our database using Entity Framework Core.
// Migrations will help us keep our database schema in sync with our model classes as we make changes to them over time.

// DTO (Data Transfer Object) is a design pattern used to transfer data between software application subsystems.
/// Represents a product entity in the database.
/// This is a POCO (Plain Old CLR Object) that maps to a database table via Entity Framework Core.
/// </summary>
/// <remarks>
/// <para>
/// The Product class serves as the domain model for product-related data. Each property corresponds
/// to a column in the database table, with the Id property automatically designated as the primary key.
/// </para>
/// <para>
/// When used with DTOs (Data Transfer Objects), this model is typically mapped to a ProductDto class
/// for API responses and requests. This separation allows you to:
/// - Control which properties are exposed to clients (e.g., hide CreatedDate from responses)
/// - Flatten or restructure the data for specific API endpoints
/// - Prevent overposting attacks by only accepting specific fields
/// - Maintain a clear contract between your API and consumers
/// </para>
/// <para>
/// A typical DTO mapping pattern would look like:
/// Product (Domain Model) → ProductDto (Data Transfer Object) → JSON/API Response


// ? Scaffolding a model in ASP.NET Core using Entity Framework Core:

//? What's scaffolding?
// Scaffolding is a code generation technique that allows you to quickly create boilerplate code for common tasks, such as CRUD operations, 
// based on your model classes. It can save you a lot of time when setting up your application, especially for standard operations that follow a predictable pattern.  
// In ASP.NET Core, you can use the dotnet-aspnet-codegenerator tool to scaffold controllers, views, and API endpoints based on your model classes and DbContext.
// This tool generates code that follows best practices and conventions, allowing you to focus on implementing your business logic rather than writing repetitive code.  
// For example, if you have a Product model and an AppDbContext, you can run the following command to scaffold a ProductController with CRUD actions:
// dotnet aspnet-codegenerator controller -name ProductController -m Product -dc AppDbContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries
// This command will generate a ProductController with actions for Create, Read, Update, and Delete 
// operations based on the Product model and AppDbContext. It will also generate views for these actions if you are using MVC (not applicable for Web API).




// 1. Database → Code (Reverse Engineering)
//You already have a database
// 👉 You generate:
// Entities
// DbContext
// Command: dotnet ef dbcontext scaffold "connection-string" Microsoft.EntityFrameworkCore.SqlServer -o Models
//
// 2. Code → Database (Migrations)
// You start with code (model classes) and generate the database schema from it.
// You create model classes (like Product and User) and then use EF Core migrations to create the database schema based on those models.
// The migration captures the changes you made to your model and generates code to apply those changes to the database. 
//This way, you can keep your database schema in sync with your model classes as they evolve over time. 

//    - dotnet ef migrations add InitialCreate // Creates a new migration named "InitialCreate" based on the current state of your model classes. 
// This generates code that describes how to create the database schema (tables, columns, constraints) based on your model classes.
//    - dotnet ef migrations add UpdateModel
//    - dotnet ef database update 

// After creating the migration, you apply it to the database using: dotnet ef database update. This process ensures that your database schema matches your model classes as they evolve over time.

// 3. Model → Controller (Code Generation)
// You create a model class (like Product) and then use scaffolding to generate a controller with CRUD actions based on that model.
// Command: dotnet aspnet-codegenerator controller -name ProductController -m Product -dc AppDbContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries
// This command generates a ProductController with actions for Create, Read, Update, and Delete operations based on the Product model and AppDbContext. It also generates views for these actions if you are using MVC (not applicable for Web API).
// This scaffolding process helps you quickly set up a controller with standard CRUD operations based on your



// After running the migration commands, EF Core will create a Products table in your database with columns corresponding to the properties defined in the Product class.
// You can then perform CRUD operations on products in the database using Entity Framework Core's DbContext. 
// Migrations are a way to keep your database schema in sync with your EF Core model classes.
// When you create or modify your model classes (like Product or User), you need to create a migration to update the database schema accordingly. The migration captures the changes you made to your model and generates code to apply those changes to the database. 
// To create a migration, you use the following command in the terminal: dotnet ef migrations add InitialCreate. After creating the migration, you apply it to the database using: dotnet ef database update. This process ensures that your database schema matches your model classes as they evolve over time.  
// Note: If you make changes to your model classes (e.g., add a new property), you need to create a new migration to update the database schema accordingly.
// Each migration captures the changes you made to your model and generates code to apply those changes to the database. This way, you can keep your database schema in sync with your model classes as they evolve over time.  
// For example, if you add a new property to the Product class (e.g., public string Category { get; set; }), you would need to create a new migration to add the Category column to the Products table in the database. You would run: dotnet ef migrations add AddCategoryToProduct, and then apply it with: dotnet ef database update.
// This process allows you to manage changes to your database schema in a structured way as your application evolves.
// Migrations are a powerful feature of Entity Framework Core that help you manage changes to your database schema over time. 
// They allow you to evolve your database schema as your application requirements change, without losing existing data. By creating and applying migrations, you can keep your database schema in sync with your model classes, ensuring that your application continues to function correctly as you make changes to your models.






//? How to update the entity in the Db after making changes i.e removing columns and adding new columns or changing constraints?
// When you make changes to your entity classes, such as adding or removing properties, changing data types, or modifying constraints, you need to create a new migration to update the database schema accordingly. 
// You can do this using the Entity Framework Core command-line tools or the Package Manager Console in Visual Studio.
// For example, if you are using the command-line tools, you can run the following command to create a new migration after making changes to your entity classes:
// dotnet ef migrations add UpdateJobEntity
// This will generate a new migration file with the necessary code to update the database schema based on the changes you made to the Job entity.
// After creating the migration, you need to apply it to the database using the following command:

// dotnet ef database update

// This will execute the migration and update the database schema to reflect the changes you made to the Job entity. Make sure to review the generated migration code to ensure it accurately reflects the changes you intended to make before applying it to the database. 


