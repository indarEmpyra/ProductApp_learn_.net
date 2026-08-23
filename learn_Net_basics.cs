//? Layers in .Net Application

//# Controller Layer (API Endpoints)
// [ApiController]
// [Route("tasks")]
// public class TaskController : ControllerBase
// {
//     private readonly ITaskService _taskService;

//     public TaskController(ITaskService taskService)
//     {
//         _taskService = taskService;
//     }

//     [HttpGet]
//     [Authorize(Roles = "Admin,User")]
//     public async Task<IActionResult> GetTasks()
//     {
//         var tasks = await _taskService.GetTasks();
//         return Ok(tasks);
//     }
// }

//# Service Layer (Business Logic)
// public class TaskService : ITaskService
// {
//     private readonly ITaskRepository _repo;

//     public TaskService(ITaskRepository repo)
//     {
//         _repo = repo;
//     }

//     public async Task<List<Task>> GetTasks()
//     {
//         return await _repo.GetAll();
//     }
// }


//# Repository Layer (Data Access)
// public class TaskRepository : ITaskRepository
// {
//     private readonly AppDbContext _context;

//     public TaskRepository(AppDbContext context)
//     {
//         _context = context;
//     }

//     public async Task<List<Task>> GetAll()
//     {
//         return await _context.Tasks.ToListAsync();
//     }
// }

//# Entity (Database Model)
// public class Task
// {
//     public int Id { get; set; }
//     public string Title { get; set; }
//     public bool IsCompleted { get; set; }
// }


// Why.NET Uses Interfaces Everywhere

// You’ll notice:

// ITaskService
// ITaskRepository

// Example:

// ITaskService
// TaskService

// Why?

// Because Dependency Injection container uses interfaces.

//# But Modern .NET Has a Twist

// Many teams skip the repository layer when using Entity Framework.
// Instead they do:

// Controller
//    ↓
// Service
//    ↓
// DbContext

// Because EF already acts like a repository + unit of work.

// Example:

// public class TaskService
// {
//   private readonly AppDbContext _context;

//   public TaskService(AppDbContext context)
//   {
//     _context = context;
//   }

//   public async Task<List<Task>> GetTasks()
//   {
//     return await _context.Tasks.ToListAsync();
//   }
// }


/****************************************************************************************************************************************** */


//? Conventions:
// 1. Class names should be in PascalCase (e.g., TaskService, ITaskRepository).
// 2. Method names should also be in PascalCase (e.g., GetTasks, AddTask).
// 3. Interface names should start with "I" followed by PascalCase (e.g., ITaskService, ITaskRepository).
// 4. Private fields should be in camelCase and prefixed with an underscore (e.g., _taskService, _repo).
// 5. Constants should be in PascalCase (e.g., MaxItems, DefaultTimeout).
// 6. Use meaningful names for variables, methods, and classes to improve code readability.
// 7. Use singular names for classes that represent a single entity (e.g., Task, User) and plural names for collections (e.g., Tasks, Users). 
// 8. Use verb-noun pairs for method names to indicate the action being performed (e.g., GetTasks, AddTask).
// 9. Use namespaces to organize code into logical groups (e.g., MyApp.Services, MyApp.Repositories).

//? Model Conventions:
// 1. Use PascalCase for property names (e.g., FirstName, LastName).
// 2. Use singular names for classes that represent a single entity (e.g., Task, User) and plural names for collections (e.g., Tasks, Users).
// 3. Use data annotations to specify validation rules and constraints (e.g., [Required], [StringLength(100)]).
// 4. Use nullable reference types to indicate whether a property can be null or not (e.g., string? Nickname).
// 5. Use appropriate data types for properties based on the data they represent (e.g., int for whole numbers, DateTime for dates).
// 6. Use navigation properties to represent relationships between entities (e.g., public ICollection<Task> Tasks { get; set; }).
// 7. Use foreign key properties to represent relationships between entities (e.g., public int UserId { get; set; }).
// 8. Use [Key] attribute to specify the primary key of an entity (e.g., [Key] public int Id { get; set; }).
// 9. Use [Table] attribute to specify the database table name for an entity (e.g., [Table("Tasks")]).
// 10. Use [Column] attribute to specify the database column name for a property (e.g., [Column("TaskTitle")] public string Title { get; set; }). 
// 11. Use [NotMapped] attribute to specify that a property should not be mapped to a database column (e.g., [NotMapped] public string FullName { get; set; }).
// 12. Use [ForeignKey] attribute to specify the foreign key relationship between entities (e.g., [ForeignKey("UserId")] public User User { get; set; }).
// 13. Use [InverseProperty] attribute to specify the inverse navigation property for a relationship (e.g., [InverseProperty("Tasks")] public User User { get; set; }).
// 14. Use [Required] attribute to specify that a property is required and cannot be null (e.g., [Required] public string Title { get; set; }).
// 15. Use [StringLength] attribute to specify the maximum length of a string property (e.g., [StringLength(100)] public string Title { get; set; }).
// 16. Use [Range] attribute to specify the valid range of values for a numeric property (e.g., [Range(1, 100)] public int Priority { get; set; }).
// 17. Use [DataType] attribute to specify the data type of a property (e.g., [DataType(DataType.EmailAddress)] public string Email { get; set; }).
// 18. Use [Display] attribute to specify the display name and format for a property (e.g., [Display(Name = "Task Title")] public string Title { get; set; }).  
// 19. Use [Timestamp] attribute to specify a property that should be used for concurrency checking (e.g., [Timestamp] public byte[] RowVersion { get; set; }).
// 20. Use [ConcurrencyCheck] attribute to specify a property that should be used for concurrency checking (e.g., [ConcurrencyCheck] public int Version { get; set; }).
// 21. Use [Index] attribute to specify that a property should be indexed in the database (e.g., [Index] public string Title { get; set; }).
// 22. Use [Unique] attribute to specify that a property should have a unique constraint in the database (e.g., [Unique] public string Email { get; set; }).
// 23. Use [DefaultValue] attribute to specify the default value for a property (e.g., [DefaultValue(0)] public int Priority { get; set; }).
// 24. Use[JsonIgnore] attribute to specify that a property should be ignored during JSON serialization (e.g., [JsonIgnore] public string Password { get; set; })
// 25. Use [JsonProperty] attribute to specify the JSON property name for a property (e.g., [JsonProperty("task_title")] public string Title { get; set; }).
// 

// NOTE: Very Important

//? What's the MediatR + CQRS?
// MediatR is a library that implements the Mediator pattern, which allows for decoupling of request handling and 
// response processing in an application. It provides a way to send requests and receive responses without having 
// to know the details of how the requests are handled. MediatR is often used 
// in conjunction with the CQRS (Command Query Responsibility Segregation) pattern, which separates read and 
// write operations into different models.

// CQRS is a design pattern that separates the read and write operations of an application into different models. 
// It allows for better scalability and performance by optimizing the read and write operations separately. 
// CQRS is often used in applications that have complex business logic or require high performance, 
// as it allows for more efficient handling of read and write operations. While MediatR can be used to 
// implement the Mediator pattern in a CQRS architecture, it is not a requirement for using CQRS.


//? Controller -> Service Interface -> Service Implementation -> Repository Interface -> Repository Implementation -> DbContext 
//? Vs 
//? MediatR + CQRS pattern

// CQRS (Command Query Responsibility Segregation) is a design pattern that separates 
// the read and write operations of an application into different models. 

// MediatR is a library that implements the Mediator pattern, 
// which allows for decoupling of request handling and response processing in an application. 

// Nuget Package: MediatR
// MediatR is often used in conjunction with the CQRS pattern, as it provides a way to send requests and receive 
// responses without having to know the details of how the requests are handled.  

// In a traditional architecture, the flow of data and control is typically as follows:
// Controller -> Service Interface -> Service Implementation -> Repository Interface -> Repository Implementation -> DbContext
// In a CQRS architecture using MediatR, the flow of data and control is typically as follows:
// Controller -> MediatR -> Command/Query Handler -> Repository Interface -> Repository Implementation -> DbContext

// The main difference between the two architectures is that in a traditional architecture, the controller 
// directly calls the service interface,  
// which then calls the service implementation and repository interface. In a CQRS architecture using MediatR, 
// the controller sends a command or query to MediatR,
// which then invokes the appropriate command or query handler to handle the request. 
// This allows for better separation of concerns and more flexible handling of requests,
// as the command or query handlers can be implemented independently of the controller and service layers. 
// Additionally, CQRS allows for better scalability and performance by separating read and write operations 
// into different models, while a traditional architecture may not provide the same level of optimization 
// for read and write operations.

// In summary, while both architectures can be used to build applications, a CQRS architecture using 
// MediatR provides better separation of concerns and more flexible handling of requests, while a traditional 
// architecture may be simpler to implement but may not provide the same level of optimization for read and write operations.  
// 

//? Which approach is better, traditional architecture or CQRS architecture using MediatR?
// The choice between a traditional architecture and a CQRS architecture using MediatR depends on the
// specific requirements and complexity of the application being built.
// A traditional architecture may be simpler to implement and may be sufficient for applications with
// straightforward business logic and low scalability requirements. It can be easier to understand and maintain, especially
// for smaller teams or projects with limited resources. However, it may not provide the same level of 
// optimization for read and write operations,  
// On the other hand, a CQRS architecture using MediatR may be more suitable for applications with complex business logic, 
// high scalability requirements, or the need for optimized read and write operations.
// 


//# CQRS (Command Query Responsibility Segregation) 
// is a higher-level architectural pattern. It splits the conceptual model of your application into two distinct parts:
// Commands: Data mutations (Create, Update, Delete).
// Queries: Data reads (Select, Fetch).Here is a breakdown of what CQRS actually entails, how it differs from simple handler organization, and why it matters

// The Real Difference:
// Your Description (In-Process Messaging): Separating requests (interfaces/DTOs) from handlers (implementations) decouples your application layers. You can do this while still using the exact same database, tables, and ORM models for both reading and writing.
// CQRS (Architectural Segregation): CQRS means the code path, business logic, and potentially the data storage for reading data are entirely separate from writing data.


//# Levels of CQRS Implementation

// CQRS is a spectrum. You can implement it at different levels of complexity:

// 1.Logical Separation(Single Database)
// Write Side: Uses a heavy ORM (like Entity Framework or Hibernate) to handle complex business rules, validation, and database transactions.
// Read Side: Bypasses the heavy ORM entirely. It uses lightweight SQL queries (like Dapper) or views to fetch raw data directly into DTOs for maximum speed.

// 2.Physical Separation(Dual Databases)
// Write Database: A relational database optimized for fast updates and data integrity (normalized structure).
// Read Database: A NoSQL database or cache optimized for lightning-fast search and retrieval (denormalized structure).
// Synchronization: An event-driven mechanism (like Kafka or RabbitMQ) updates the read database whenever a command alters the write database (Eventual Consistency).⚖️ 

// Why Use CQRS?
// Optimized Performance: Read operations usually outnumber write operations 10:1.You can scale your read database independently.
// Simplified Security: It is easier to ensure that only authorized commands can mutate state, while read endpoints remain isolated.
// Schema Flexibility: The UI often requires data formatted differently than how it is stored. The read model can match the UI exactly.

//# I thought controller-service-repository was the standard way to build .NET applications. Why is CQRS becoming more popular?
// While the controller-service-repository pattern has been a standard way to build .NET applications,
// CQRS is becoming more popular due to its ability to provide better separation of concerns, scalability, 
// and performance optimization for read and write operations.  
// CQRS allows for more flexible handling of requests, as the command and query handlers can be implemented 
// independently of the controller and service layers. 
// 

//# is there any other way which industry is using to build .NET applications?
// Yes, there are other architectural patterns and approaches that the industry is using to build .NET
// applications, depending on the specific requirements and complexity of the application being built. Some of these approaches include:
// 1. Clean Architecture: This approach emphasizes separation of concerns and the use of interfaces to decouple the different 
// layers of the application. It promotes a clear separation between the domain, application, and infrastructure layers, 
// allowing for better maintainability and testability.
// 2. Hexagonal Architecture (Ports and Adapters): This approach focuses on creating a
// flexible and modular architecture by defining clear boundaries between the core domain logic and the external 
// systems (e.g., databases, APIs). It allows for easier testing and integration with different external systems. 
// 3. Microservices Architecture: This approach involves breaking down the application into smaller, independent 
// services that can be developed, deployed, and scaled independently. Each service is responsible for a specific 
// business capability and communicates with other services through APIs or messaging systems. 
// This approach allows for better scalability, fault isolation, and flexibility in technology choices.  
// 

//? How to implement CQRS in .NET using MediatR?
// To implement CQRS in .NET using MediatR, you can follow these steps:
// 1. Install the MediatR NuGet package:
//    Install-Package MediatR

// How to install MediatR in .NET 6 or .NET 7?
// You can install MediatR in .NET 6 or .NET 7 using the NuGet Package Manager or the .NET CLI. Here are the steps for both methods:
// NuGet Package Manager:
// Install-Package MediatR
// .NET CLI:
// dotnet add package MediatR

// 2. Define your command and query classes:
// public class CreateProductCommand : IRequest<Product> { 
// Properties for the product to be created
// int Id { get; set; }
//  }
// public class GetProductByIdQuery : IRequest<Product> { 
// Properties for the product ID
// int Id { get; set; }

//  }

// 3. Implement the command and query handlers:
// public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Product> { ... }
// public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Product> { ... }

// 4. Register MediatR in the Startup.cs or Program.cs:
// services.AddMediatR(typeof(Startup).Assembly);

// 5. Use MediatR in your controller:
// public class ProductsController : ControllerBase
// {
//     private readonly IMediator _mediator;
//     public ProductsController(IMediator mediator)
//     {
//         _mediator = mediator;
//     }
//     public async Task<IActionResult> CreateProduct(CreateProductCommand command)
//     {
//         var result = await _mediator.Send(command);
//         return Ok(result);
//     }
// }
