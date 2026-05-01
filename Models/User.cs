using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

    // [ForeignKey("RoleId")]
    // public Role Role { get; set; }

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