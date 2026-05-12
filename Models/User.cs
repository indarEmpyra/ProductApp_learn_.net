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
