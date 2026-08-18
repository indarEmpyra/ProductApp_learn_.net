
//? Optional chaining and Null coalescing operators
// // If customer, Address, or City is null, result will be null
// string? city = customer?.Address?.City;

// // It works seamlessly with method calls too
// customer?.SendEmail();


// // Safely accesses the first order if the orders list is not null
// Order? firstOrder = customer?.Orders?[0];

// // If any part of the chain is null, it falls back to "Unknown"
// string city = customer?.Address?.City ?? "Unknown";

// // customer.Age is an 'int', but age becomes 'int?' because customer might be null
// int? age = customer?.Age; 


//? Ternary operator
int time = 20;
string greeting = (time < 18) ? "Good day." : "Good evening.";

//? String methods:
// string.IsNullOrEmpty()
// string.IsNullOrWhiteSpace()
// string.Substring()
// string.Replace()
// string.Split()
// string.Join()
// string.Trim()
// string.contains()

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



// Car class with a constructor:
// public class Car
// {
//    public string Color;

//    public Car(string color)
//    {
//       this.Color = color;
//    }
// } 

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
// static --> Belongs to the type itself rather than to a specific object. It can be accessed without creating an instance of the class.
// const --> A compile-time constant that cannot be changed after it is declared. 
// It must be initialized at the time of declaration and cannot be modified later. It is implicitly static, 
// meaning it belongs to the type rather than an instance of the type.

// Example of Modifiers in C#:
// public class ExampleClass
// {
//     public string PublicProperty { get; set; } // Can be accessed from anywhere
//     private string PrivateProperty { get; set; } // Can only be accessed within this class
//     protected string ProtectedProperty { get; set; } // Can be accessed within this class and derived classes
//     internal string InternalProperty { get; set; } // Can be accessed within the same assembly
//     protected internal string ProtectedInternalProperty { get; set; } // Can be accessed within the same assembly and derived classes
//     readonly string ReadonlyProperty = "This is a readonly property"; // Can only be assigned in the declaration or constructor
//     static string StaticProperty = "This is a static property"; // Belongs to the type itself, not an instance
//     const string ConstProperty = "This is a constant property"; // A compile-time constant that cannot be changed

//     public void PublicMethod() { /* ... */ } // Can be called from anywhere
//     private void PrivateMethod() { /* ... */ } // Can only be called within this class
//     protected void ProtectedMethod() { /* ... */ } // Can be called within this class and derived classes
//     internal void InternalMethod() { /* ... */ } // Can be called within the same assembly  - assembly means compiled project, the class which is compiled into a dll or exe file
//     protected internal void ProtectedInternalMethod() { /* ... */ } // Can be called within the same assembly and derived classes
// }


//# How public methods can be accessed from other classes?
// Example:
// public class ExampleClass
// {
//     public void PublicMethod() { /* ... */ } // Can be called from anywhere
// }

// public class AnotherClass
// {
//     public void CallPublicMethod()
//     {
//         ExampleClass example = new ExampleClass();
//         example.PublicMethod(); // Can be called from anywhere
//     }
// }  

//# If we don't want to instantiate a class to call its method, we can make the method static.
// Example:
// public class ExampleClass
// {
//     public static void StaticMethod() { /* ... */ } // Can be called without instantiating the class
// }

// public class AnotherClass
// {
//     public void CallStaticMethod()
//     {
//         ExampleClass.StaticMethod(); // Can be called without instantiating the class
//     }
// }

//Note: When a class is made static, it means that the class cannot be instantiated and can only contain static members (methods, properties, fields, etc.).

//# String must be a static method to be called without instantiation then how are we able to create string using string str = new String()?
// You cannot call a regular instance method on a class without making an object first. However, string in C# is special. new string(...) works because it calls a constructor, which is a special creation method, not a normal method.Why new string(...) worksA constructor is a special method used to build a new object.

//# How are we able to call IsNullOrEmpty() method without instantiation of string class?
// You can call string.IsNullOrEmpty() without instantiating a string object because it is a static method.How It WorksStatic Keyword: In C#, the IsNullOrEmpty method is defined with the static keyword inside the String class.Class-Level Belonging: Static methods belong to the class itself, not to any specific instance of the class.No Instance Needed: You call static methods by using the class name directly (e.g., string.IsNullOrEmpty(...)) instead of a variable name.
//Code Example
// csharp
// You do not need to create an object like this:
// string str = new string(); 

// Instead, you call it directly on the class:
// bool isEmpty = string.IsNullOrEmpty(""); 

//? Extension method
// Extension methods allow you to add new methods to existing types without modifying the original type or creating a new derived type.
// They are defined as static methods in a static class, and the first parameter of the method specifies the type that the method operates on, preceded by the "this" keyword.

// Example of an Extension Method:
// public static class StringExtensions
// {
//     public static bool IsNullOrEmpty(this string str)
//     {
//         return string.IsNullOrEmpty(str);
//     }
// }

//# What makes this an extension method?
// The method IsNullOrEmpty is defined as a static method in a static class (StringExtensions), and the first parameter of the method is 
// preceded by the "this" keyword, which indicates that it is an extension method for the string type. This allows us to call IsNullOrEmpty 
// on any string instance as if it were a method defined in the string class itself. 

// In this example, we define an extension method called IsNullOrEmpty for the string type. This allows us to call IsNullOrEmpty on any string instance, like this:
// string myString = "Hello";
// bool isEmpty = myString.IsNullOrEmpty(); // This will return false because myString is not null or empty.




//? Interfaces?
// Interfaces are contracts that define a set of methods and properties that a class must implement.
// Use case: Interfaces are used to achieve abstraction and multiple inheritance in C#. They allow you to define a contract that classes can implement, 
// ensuring that they provide specific functionality.
// For example, you might have an interface called "IVehicle" that defines methods like "Start()" and "Stop()". 
// Any class that implements the IVehicle interface must provide an implementation for these methods, allowing you to work with 
// different types of vehicles (e.g., Car, Bike) through a common interface.  

//# Can classes implement multiple interfaces?
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
//     public Task<List<Task>> GetTasks() // must be "GetTasks" and not "GetTask" because the interface defines it as "GetTasks"
//     {
//         // must return Task<List<Task>>
//         return Task.FromResult(new List<Task>());
//     }
// }

// Interface → ITaskService
// Implementation → TaskService

//# What does implementation mean?
// Implementation refers to the actual code that defines how a method or property behaves when it is called. In the context of interfaces, implementation means providing the specific details of how each method defined in the interface should be executed.

//#Will the derived class have to implement all methods of the interface?
// Yes — implementing an interface means implementing every member it declares. A class can't compile if it's missing even one.

// public interface ITaskService
// {
//     Task<List<Task>> GetTasks();
//     Task<Task> GetTaskById(int id);
//     Task AddTask(Task task);
// }

// public class TaskService : ITaskService
// {
//     public Task<List<Task>> GetTasks() { ... }
//     public Task<Task> GetTaskById(int id) { ... }
//     // missing AddTask → compiler error:
//     // "'TaskService' does not implement interface member 'ITaskService.AddTask(Task)'"
// }

//Note: A few exceptions/nuances worth knowing:

//# Abstract classes are exempt. An abstract class can implement some members and leave the rest abstract, deferring full implementation to whatever concrete class derives from it.

//# what does partial keyword mean in c#?
// In C#, the partial keyword allows you to split the definition of a class, struct, interface, or method across multiple physical files. When your application is compiled, the C# compiler gathers all these scattered pieces and merges them into a single, cohesive type.💡 Why Use the partial Keyword?The partial modifier exists primarily to solve two real-world development challenges:Separation of Auto-Generated Code: Code-generation tools (like the Windows Forms Designer or Entity Framework) can output UI layouts or database models into one file. You can then write your custom business logic in a completely separate file. When the tool regenerates its file, your manual code is never overwritten.Large Codebases & Collaboration: It allows multiple developers to work on different aspects of the same massive class simultaneously without triggering file conflicts in source control.📂 How Partial Classes Work (Example)Instead of cramming everything into one file, you mark the class with partial in both files:File 1: Employee_Authentication.cscsharpnamespace Company.Project

// {
//     public partial class Employee
//     {
//         public string Username { get; set; }

//         public void Login() 
//         {
//             // Login logic here
//         }
//     }
// }
// Use code with caution.File 2: Employee_Payroll.cscsharpnamespace Company.Project
// {
//     public partial class Employee
//     {
//         public decimal Salary { get; set; }

//         public void ProcessPayment() 
//         {
//             // Payroll logic here
//         }
//     }
// }
// Use code with caution.The Compiled Result:The compiler treats this as a single Employee class containing Username, Salary, Login(), and ProcessPayment().⚠️ Strict Rules for Partial TypesFor the compiler to successfully merge your partial items, they must follow these strict requirements:Same Namespace & Assembly: Every partial part must share the exact same namespace and reside within the same compiled module or project assembly.Matching Modifiers: Access levels (such as public, private, or internal) must match exactly across all parts.Inheritance Merging: If one part inherits an interface, the entire combined class implements it. If one part declares a base class, all parts inherit that base class.Abstract/Sealed Cascade: If you mark a single part as abstract or sealed, the final merged class becomes abstract or sealed automatically.⚙️ What is a Partial Method?The partial keyword can also be applied to methods. This is heavily utilized by code generators to create optional lifecycle hooks:The Signature: A tool declares a method signature in the auto-generated file.The Option: You can optionally implement that method in your custom file to run code when that event happens.The Compilation Magic: If you choose not to implement the partial method, the compiler completely erases the signature and all calls to it. This means zero performance penalty at runtime.csharp// Inside the auto-generated file:
// partial void OnNameChanged(); 

// // Inside your custom file (Optional):
// partial void OnNameChanged()
// {
//     Console.WriteLine("Name changed!");
// }


// public abstract class TaskServiceBase : ITaskService
// {
//     public abstract Task<List<Task>> GetTasks();
//     public Task<Task> GetTaskById(int id) => ...; // implemented here
//     public abstract Task AddTask(Task task);
// }
// Default interface members (C# 8+). An interface can provide a body itself; implementers then only need to override it if they want different behavior.


// public interface ITaskService
// {
//     Task<List<Task>> GetTasks();
//     Task LogAccess() => Task.CompletedTask; // has a default body
// }
// Here TaskService only has to implement GetTasks().

// Explicit interface implementation still counts. Writing Task<List<Task>> ITaskService.GetTasks() satisfies the requirement — it's just only callable through an ITaskService reference, not directly off the class.

// So for a plain, non-abstract class with a plain interface (no default bodies), the rule is strict: implement all of it, or it won't compile.


//# Another example:

// public IEnumerable<string> Get()

// returning INumerable<string> means that the method can return any collection of strings that implements the IEnumerable<string> interface.
// This allows for flexibility in the type of collection that can be returned, as it could be a List<string>, an array of strings, a HashSet<string>, 
// or any other collection type that implements IEnumerable<string>. 
// {
//     return new[] { "Hot", "Cold", "Rainy" };
//     return new List<string> { "Hot", "Cold", "Rainy" };
//     return new HashSet<string> { "Hot", "Cold", "Rainy" };
//     return new Collection<string> { "Hot", "Cold", "Rainy" };
//     return new string[] { "Hot", "Cold", "Rainy" };

//     return a dictionary
//     return new Dictionary<string, string>
//     {
//         { "Weather1", "Hot" },
//         { "Weather2", "Cold" },
//         { "Weather3", "Rainy" }
//     };
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


//# Class implementing multiple interfaces and inheriting from a base class
// public class FlyingCar : Vehicle, IDrivable, IFlyable
// {
//     public void Drive()
//     {
//         // Implementation of Drive method
//     }

//     public void Fly()
//     {
//         // Implementation of Fly method
//     }
// }

// In this example, the FlyingCar class inherits from a base class called Vehicle and implements two interfaces, IDrivable and IFlyable. 
// This allows the FlyingCar class to provide implementations for the methods defined in both interfaces, while
// also inheriting any properties or methods defined in the Vehicle base class. This demonstrates how a class can implement multiple interfaces 
// and inherit from a base class simultaneously in C#.



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
// By enabling nullable reference types, you can improve code safety and reduce the likelihood of null reference exceptions by explicitly marking reference types as nullable or non-nullable.

// Example:
// public class Person
// {
//     public string Name { get; set; } // Non-nullable reference type
//    public string? Nickname { get; set; } // Nullable reference type
// 

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
// Example of a Delegate:
// public delegate void EventHandler(object sender, EventArgs e);



//? Operators in C#
// 1. Arithmetic Operators: +, -, *, /, %
// 2. Assignment Operators: =, +=, -=, *=, /=, %=
// 3. Comparison Operators: ==, !=, >, <, >=, <=
// 4. Logical Operators: &&, ||, !
// 5. Bitwise Operators: &, |, ^, ~, <<, >>
// 6. Ternary Operator: ?:
// 7. Null-coalescing Operator: ??


// Optional chaining operator (?.) is used to access members of an object that may be null without throwing a NullReferenceException.
// Example:
// string name = person?.Name; // This will return null if person is null, instead of throwing an exception.

// Null-coalescing operator (??) is used to provide a default value when an expression evaluates to null.
// Example:
// string name = person.Name ?? "Unknown"; // This will return "Unknown" if person.Name is null, instead of returning null.

// Null-coalescing assignment operator (??=) is used to assign a value to a variable only if it is currently null.
// Example:
// string name;
// name ??= "Unknown"; // This will assign "Unknown" to name only if name is currently null, otherwise it will keep its existing value.

// The null-forgiving operator (!) is used to suppress the compiler's warning about potential null reference exceptions when you are 
//  certain that a value will not be null.
// Example:
// string name = person!.Name; // This tells the compiler that person will not be null, even if it cannot be statically determined, 
// and allows you to access the Name property without a warning about potential null reference exceptions. 

// The null-conditional operator (?.) is used to access members of an object that may be null without throwing a NullReferenceException.
// Example: 
// string name = person?.Name; // This will return null if person is null, instead of throwing an exception.

// What is this? operator in this: public string? Nickname { get; set; }
// The "?" operator in the property declaration "public string? Nickname { get; set; }" indicates that the "Nickname" property is a nullable reference type.
// This means that the "Nickname" property can either hold a string value or be null.

//# how to mention not-null in method parameters?
// You can use the nullable reference types feature in C# to indicate that a method parameter should
// not be null. By enabling nullable reference types, you can use the non-nullable reference type syntax to indicate that a parameter should not accept null values.
// Example:
// public void PrintName(string name)

// Make something nullable:
// public void PrintName(string? age)

// In this example, the parameter "name" is of type string, which is a non-nullable reference type.
// This means that when you call the PrintName method, you must provide a non-null string value for the "name" parameter. 
// If you try to pass null to this method, the compiler will generate a warning, indicating that you are trying to assign a null 
// value to a non-nullable reference type.
// By using non-nullable reference types in your method parameters, you can improve code safety and reduce the likelihood of 
// null reference exceptions by explicitly indicating that certain parameters should not accept null values.   

//# How to mention not nullable /nullable in model properties?
// You can use the nullable reference types feature in C# to indicate that a model property should not be null. 
// By enabling nullable reference types, you can use the non-nullable reference type syntax to indicate that a property should not accept null values.
// Example:
// public class Person
// {
// Not nullable reference type
//     public string Name { get; set; } 

// Nullable reference type
//     public string? Nickname { get; set; }
// }

// In this example, the property "Name" is of type string, which is a non-nullable reference type. This means that when you create an instance 
// of the Person class, you must provide a non-null string value for the "Name" property.
// If you try to assign null to the "Name" property, the compiler will generate a warning, indicating that you are trying to assign 
// a null value to a non-nullable reference type.
// By using non-nullable reference types in your model properties, you can improve code safety and reduce the likelihood of null reference 
// exceptions by explicitly indicating that certain properties should not accept null values. 


//? What's difference between var name = "John"; and string name = "John";?
// The difference between "var name = "John";" and "string name = "John";" is that the first one uses implicit typing with the "var" 
// keyword, while the second one uses explicit typing with the "string" keyword.
// In the first case, the compiler infers the type of the variable "name" based on the assigned value "John", which is a string. 
// So, "var name = "John";" is equivalent to "string name = "John";" in this context.
// The main advantage of using "var" is that it can make the code more concise and  readable, especially when the type is obvious 
// from the context. However, it can also make the code less clear if the type is not immediately apparent, so it's important to use 
// "var" judiciously to maintain code readability. 

//# const string name = "John"; // This is a compile-time constant that cannot be changed after it is declared. 
// It must be initialized at the time of declaration and cannot be modified later. It is implicitly static, meaning it 
// belongs to the type rather than an instance of the type.
// readonly string name = "John"; // This is a readonly field that can only be assigned in the declaration or in the constructor of the class. 
// It cannot be modified elsewhere. It is not implicitly static, meaning it belongs to an instance of the class rather than the type itself.

//? Arrow Functions in C# (Lambda Expressions)
// Lambda expressions are a concise way to represent anonymous methods using a special syntax.  
// Example of a Lambda Expression:
// Func<int, int> square = x => x * x;
// In this example, we define a lambda expression that takes an integer parameter x and returns the square of x.
// Lambda expressions are often used in LINQ queries and for defining delegates or expression trees.

//? Casting in C#
// Casting is the process of converting a value from one type to another.
// Example of Casting:
// int number = 10;
// double doubleNumber = (double)number; // Explicit casting from int to double
// In this example, we explicitly cast the integer variable "number" to a double type using the (double) syntax.
// C# also supports implicit casting, where the conversion is done automatically when there is no risk of data loss, such as when converting 
// from a smaller numeric type to a larger numeric type (e.g., int to double).  


//? <> (Generic Types in C#)
// Generic types allow you to define classes, interfaces, and methods with a placeholder for the type of data they store or use. 
// This allows for code reusability and type safety.
// Example of a Generic Type:
// public class Box<T>
// {
//     private T _value;

//     public void Add(T value)
//     {
//         _value = value;
//     }

//     public T Get()
//     {
//         return _value;
//     }
// } 

//? Collections in C#
// Collections are data structures that can hold multiple values. They are part of the System.Collections namespace and include various 
// types such as List<T>, Dictionary<TKey, TValue>, HashSet<T>, and IEnumerable<T>. 
// Collections provide methods for adding, removing, and manipulating data, making it easier to work with groups of related objects.

//# Example of List<T> Collection: this returns an array of strings
// List<string> names = new List<string>();
// names.Add("Alice");
// names.Add("Bob");
// In this example, we create a List of strings and add two names to the list.

//# Example of Dictionary<TKey, TValue> Collection: This returns an object with key value pairs
// Dictionary<string, int> ages = new Dictionary<string, int>();
// ages.Add("Alice", 30);
// ages.Add("Bob", 25);
// In this example, we create a Dictionary that maps strings (names) to integers (ages) and add two entries to the dictionary.

//# Example of HashSet<T> Collection: This returns an array of unique strings
// HashSet<string> uniqueNames = new HashSet<string>();
// uniqueNames.Add("Alice");
// uniqueNames.Add("Bob");
// uniqueNames.Add("Alice"); // This will not be added again because HashSet does not allow duplicates.
// In this example, we create a HashSet of strings and add three names to the set. 
// The second "Alice" will not be added because HashSet does not allow duplicate values.

//# Example of IEnumerable<T> Collection: This returns an enumerable sequence of strings
// IEnumerable<string> enumerableNames = new List<string> { "Alice", "Bob", "Charlie" };
// In this example, we create an IEnumerable of strings using a List.
// what does IEnumerable<string> mean?
// IEnumerable<string> is an interface that represents a sequence of strings that can be enumerated.

// How?
// IEnumerable<T> provides a way to iterate over a collection of items of type T (in this case, string) without exposing the underlying collection type.
// It allows you to use a foreach loop or LINQ queries to work with the collection of strings in a flexible and efficient way, 
// regardless of the specific collection type being used (e.g., List<string>, HashSet<string>, etc.). 

// How is this different from List<string>?
// List<string> is a specific implementation of a collection that provides additional functionality such as adding, removing, and 
// accessing elements by index, while IEnumerable<string> is a more general interface that only provides the ability to enumerate over a sequence 
// of strings without any additional functionality.

// If so, why would we use IEnumerable<string> instead of List<string>?
// We would use IEnumerable<string> when we want to work with a sequence of strings without needing the additional functionality provided by List<string>.
// Using IEnumerable<string> allows us to write more flexible and reusable code, as it can work with any collection type that implements the 
// IEnumerable<string> interface, while List<string> is a specific collection type that may not be necessary in all cases.  

// Does using List<string> instead of IEnumerable<string> make our code less flexible?
// Yes, using List<string> instead of IEnumerable<string> can make our code less flexible because it ties our code to a specific collection type
//  (List<string>) rather than allowing us to work with any collection type that implements the IEnumerable<string> interface.

// Using IEnumerable<string> allows us to write code that can work with a wider range of collection types, such as arrays, HashSet<string>, 
// or any other collection that implements IEnumerable<string>,

// is it safe to say that IEnumerable<string> is more like a blanket term for any collection of strings, while List<string> 
//is a specific type of collection that provides additional functionality?
// Yes, it is safe to say that IEnumerable<string> is a more general term for any collection of strings that can be enumerated, 
// while List<string> is a specific type of collection that provides additional functionality such as adding, removing, and accessing elements by index.


// IEnumerable<string> is more flexible and can be used with any collection type that implements the IEnumerable<string> interface, while List<string> 
// is a specific collection type that provides additional features but is less flexible in terms of the types of collections it can work with.    


//# Example of using LINQ with Collections:
// List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
// var evenNumbers = numbers.Where(n => n % 2 == 0).ToList();
// In this example, we use LINQ to filter the list of numbers and create a new list that contains only the even numbers. 
// The lambda expression n => n % 2 == 0 is used to specify the condition for filtering the numbers.  


//? Attributes in C#
// Attributes are a way to add metadata to your code, which can be used to provide additional information about classes, methods, properties, and other code elements.
// Example:
// [Obsolete("This method is obsolete. Use NewMethod instead.")]
// [Authorize(Roles = "Admin")]
// 


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
