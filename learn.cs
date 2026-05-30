//? Classes?
// Classes are reference types that can contain fields, properties, methods, events, and other members. 
// They are used to create objects that can have state and behavior. 
// Classes can be instantiated using the new keyword, and they support inheritance and polymorphism.

// Example of a Class and why do we need to instantiate it?
// A class is a blueprint for creating objects. It defines the properties and behaviors that the objects created from the class will have.
// For example, consider a class called "Car". The Car class might have properties like "Make", "Model", and "Year", and methods like "StartEngine()" and "StopEngine()".
// To use the Car class, you need to create an instance of it, which is called an object. You can create an instance of the Car class like this:
// Car myCar = new Car();
// By instantiating the Car class, you can create a specific car object (myCar) that has its own state (values for Make, Model, Year) 
// and can perform actions (StartEngine, StopEngine).  

// public class Car
// {
//    public string Color;
// }

// Equivalent Node JS:

// class Car {
//   constructor(color){
//      this.color = color
//   }
// }

//# Modifiers:
// private --> Only within the same class or struct.
// private protected --> Same class, or derived classes only inside the same assembly.
// protected --> Same class, or any derived class (even in other assemblies).
// internal --> Anywhere within the same compiled assembly (project).
// protected internal --> Same assembly, plus any derived class regardless of assembly location.
// public --> Completely unrestricted across all classes and assemblies.
// readonly --> Can only be assigned in the declaration or in the constructor of the class. It cannot be modified elsewhere.


//? Interfaces?
// Interfaces are contracts that define a set of methods and properties that a class must implement.
// Use case: Interfaces are used to achieve abstraction and multiple inheritance in C#. They allow you to define a contract that classes can implement, 
// ensuring that they provide specific functionality.
// For example, you might have an interface called "IVehicle" that defines methods like "Start()" and "Stop()". 
// Any class that implements the IVehicle interface must provide an implementation for these methods, allowing you to work with 
// different types of vehicles (e.g., Car, Bike) through a common interface.  

// Can classes implement multiple interfaces?
// Yes, in C#, a class can implement multiple interfaces. This allows a class to inherit behavior from multiple sources, which is a way to achieve multiple inheritance in C#.
// For example, you could have an interface called "IDrivable" with a method "  Drive()", and another interface called "IFlyable" with a method "Fly()".
// A class called "FlyingCar" could implement both interfaces, providing implementations for both the Drive() and 
// Fly() methods, allowing it to be used as both a drivable and flyable vehicle.  

// While a class can implement multiple interfaces, it can only inherit from one base class. Class can inherit from an 
// interface and a base class at the same time, but it cannot inherit from multiple classes. This is because C# does not support multiple inheritance of 
// classes to avoid ambiguity and complexity that can arise from it. However, by implementing multiple interfaces, 
// a class can still achieve a form of multiple inheritance by providing implementations for the methods defined in those interfaces. 


// What is ITaskService?
// It is an interface.
// Example:
// public interface ITaskService
// {
//     Task<List<Task>> GetTasks();
// }

// public class TaskService : ITaskService
// {
//     public IEnumerable<string> Get()
//     {
//         return new[] { "Hot", "Cold", "Rainy" };
//     }
// }
// Interface → ITaskService
// Implementation → TaskService


// public IEnumerable<string> Get()
// {
//     return new[] { "Hot", "Cold", "Rainy" };
// }

// Break this into parts.

// public
// Method is accessible outside the class.

// IEnumerable<string>
// This is the return type.
// Meaning:
// a collection of strings

// Examples:

// List<string>
// Array<string>
// Collection<string>

// IEnumerable is an interface for collections.

// Equivalent Node return:

// ["Hot","Cold","Rainy"]



//? Structs (value types)?
// Structs are value types that are typically used to represent small, lightweight objects. 
// They are stored on the stack and have value semantics, meaning that when you assign a struct to a new variable, it creates a copy of the 
// struct rather than referencing the same object in memory.
// Use case: Structs are often used for simple data structures that do not require inheritance or complex behavior. 
// They are ideal for representing small, immutable data types such as points, rectangles, or complex numbers.
// For example, you might define a struct called "Point" that has two properties, "X" and "Y", to represent a point in a 2D space. 
// Since structs are value types, when you create a new Point and assign it to another variable, it creates a copy of the Point, 
// allowing you to work with independent instances of the Point struct without affecting each other.  

// Example of Struct:
// public struct Point
// {
//     public int X { get; set; }
//     public int Y { get; set; }
// }



//? Records (immutable types, very common in DTOs)
// Records are reference types that are designed to be immutable and provide built-in functionality for value-based equality.
// They are often used for data transfer objects (DTOs) or any scenario where you want to represent data that should not change after it is created.
// Use case: Records are ideal for representing data that is meant to be immutable, such as configuration settings, data transfer objects, 
// or any scenario where you want to ensure that the data cannot be modified after it is created.
// For example, you might define a record called "Person" that has properties like "FirstName", "LastName", and "Age". 
// Once you create an instance of the Person record, its properties cannot be changed, ensuring that the data remains consistent and immutable throughout its lifecycle.

// Example of a Record:
// public record Person(string FirstName, string LastName, int Age);

// Records provide built-in support for value-based equality, meaning that two record instances with the same property values are considered equal,
// even if they are different instances in memory. This makes records particularly useful for scenarios where you want to compare data based on 
//its content rather than its reference, such as when working with DTOs or any scenario where you want to ensure that the data is immutable and consistent. 

// Records vs Classes:
// The main difference between records and classes is that records are designed to be immutable and provide value-based equality, 
// while classes are mutable and provide reference-based equality.
// Records are ideal for representing data that should not change after it is created, while classes are better suited for scenarios 
// where you need to represent objects with mutable state and complex behavior. 

//? Nullable reference types
// Nullable reference types are a feature in C# that allows you to indicate whether a reference type can be null or not.
// By enabling nullable reference types, you can improve code safety and reduce the likelihood of null reference exceptions by 
// explicitly marking reference types as nullable or non-nullable.
// Use case: Nullable reference types are useful for improving code safety and reducing the likelihood of null reference exceptions. 
// They allow you to explicitly indicate whether a reference type can be null or not, which can help catch potential null reference issues at compile time.
// For example, if you have a method that takes a string parameter, you can mark it as nullable (string?) to indicate that it can accept null values, 
// or you can mark it as non-nullable (string) to indicate that it should not accept null values.
// By using nullable reference types, you can catch potential null reference issues at compile time, improving code safety and 
// reducing the likelihood of runtime exceptions caused by null references. 




//? Enums
// Enums (short for "enumerations") are a distinct value type that consists of a set of named constants called the enumerator list.
// Enums are used to represent a fixed set of related values in a more readable and maintainable way. 
// They can be used to define a variable that can only take one of a limited set of values, improving code clarity and reducing the likelihood of errors.  

// Example of an Enum:
// public enum DayOfWeek
// {
//     Sunday,
//     Monday,
//     Tuesday,
//     Wednesday,
//     Thursday,  
//     Friday,
//     Saturday
// }

// Use case: 
// Enums are ideal for representing a fixed set of related values, such as days of the week, months of the year, or status codes.
// For example, you might use an enum to represent the different states of an order in an e-commerce application, such as "Pending", "Processing", "Shipped", and "Delivered".
// By using an enum, you can improve code readability and maintainability, as it provides a clear and concise way to represent a set of related values, 
// and it also helps to prevent invalid values from being assigned to variables that use the enum type.  

//? Delegates
// Delegates are a type that represents references to methods with a specific parameter list and return type.
// They are used to pass methods as arguments to other methods, allowing for flexible and reusable code
// Use case: Delegates are often used for event handling, callbacks, and implementing the observer pattern.
// For example, you might define a delegate called "EventHandler" that takes an object sender and EventArgs as parameters.
// You can then use this delegate to handle events in your application, such as a button click event in a user interface. 
// By using delegates, you can decouple the event handling logic from the event source, allowing for more modular and maintainable code.   


/***************************************************************************/


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