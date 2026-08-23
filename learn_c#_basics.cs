//? NameSpace
//? Classes - static, non-static, abstract, sealed, partial, nested, generic
//? Methods - static, non-static, abstract, virtual, override, Injection of methods from different namespaces
//? Access Modifiers
//? Injection: Constructor Injection, Method Injection, Property Injection
//? Interfaces
//? Extension Methods
//? Design patterns - Singleton, Factory, Observer, Strategy, Decorator, Adapter, Facade, Proxy, Command, Template Method, Builder, Composite, State, Mediator, Flyweight
//? Attributes: Create custom attributes, use predefined attributes, reflection to read attributes

//? How memory is allocated for static and non-static classes and methods
//? How multi-threading works in C# and how to make static methods thread-safe
//? Async await and Task Parallel Library (TPL) for asynchronous programming









//? Namespace
//MARK: Namespace


// Yes, you can absolutely use a method from a different namespace. 
// To do this, you either type out the fully qualified name of the method's class (e.g., NamespaceName.ClassName.MethodName()) or use an import directive at the top of your file (like using NamespaceName; in C# or import in Java) so you can call the method directly.

//# Why Do We Have Different Namespaces?
// Namespaces serve as virtual folders or logical containers for your code. Without them, managing code in medium to large applications would be a nightmare for two primary reasons:

// 1.Preventing Naming Conflicts(Name Collisions)In programming, you cannot have two items with the exact same name in the same scope. Namespaces solve this by creating distinct boundaries.
// The Problem: If you are building an online store, you might write a class called User.If you also import a third-party payment library, that library might also have a class called User.Without namespaces, your program will crash because it cannot tell them apart.
// The Solution: Namespaces isolate the names. You can seamlessly use MyStore.Users.Profile and PayPal.API.Users.Profile in the exact same file without any errors.

// 2. Organizing Code Cleanly
// As codebases grow to hundreds or thousands of files, namespaces act like a clean filing cabinet. They group related classes, interfaces, and methods together.For example, a large system will group all database code under Company.App.Data, user interface code under Company.App.UI, and security logic under Company.App.Security.

//#How to Use a Method From Another Namespace
// Imagine you have a utility method located in a separate namespace called Utilities. Here are the two ways you can access it in your main code:Approach 1: Import the Namespace (Recommended)By importing the namespace at the top of your file, you tell the program where to look. This keeps your actual code clean and easy to read.csharp

// 1. Import the external namespace at the very top
// using Utilities; 

// namespace MainApplication 
// {
//     class Program 
//     {
//         static void Main() 
//         {
//             // 2. Call the method normally
//             Calculator.Add(5, 10); 
// Only static classes and methods can be called without instantiation. If the method is not static, you must create an object of the class first before calling it.
//         }
//     }
// }
// Use code with caution.
// Approach 2: Use the Fully Qualified Name, If you only need to use a method once, or if you have two namespaces with identical class names and need to be explicit, you can type out the full path.
// csharp
// namespace MainApplication 
// {
//     class Program 
//     {
//         static void Main() 
//         {
//             // Call the method by writing out its entire path
//             Utilities.Calculator.Add(5, 10); 
//             Only static classes and methods can be called without instantiation. If the method is not static, you must create an object of the class first before calling it.
//         }
//     }
// }

//# Pre-defined methods from a predefined class and predefined namespace
// System.Math.Abs: Finds the positive value of a negative number using Math in System.
// System.Math.Pow: Raises a number to a power using Math in System.
// System.String.Concat: Joins two text strings together using String in System.
// System.DateTime.Now: Gets the current date and time using DateTime in System.

//Note: System is a namespace. 
// It is the most fundamental namespace in C# and contains the core classes you use every day.Here is how namespaces, classes, and methods fit together, along with how to use the using directive to clean up your code.

//# Understanding the Hierarchy
// Think of a namespace as a digital filing cabinet.
// Namespace(System): The cabinet.It groups related code together to prevent naming conflicts.
// Class (Math): A drawer inside the cabinet.It groups related fields and methods.
// Method (Sqrt): A specific tool inside the drawer that performs an action.

//# Example 1: Without a Using Directive
// Without a using directive, you must type the fully qualified name(the entire path) to access a method.
// csharp
// class Program
// {
//     static void Main()
//     {
//         // You must specify the full path: Namespace.Class.Method()
//         double result = System.Math.Sqrt(25);
//         System.Console.WriteLine(result); 
//     }
// }
// Use code with caution.

//#Example 2: With a Using Directive
// The using directive tells the compiler where to look for classes. It lets you skip typing the namespace name every time.
// csharp// This imports the System namespace

// using System; 

// class Program
// {
//     static void Main()
//     {
//         // Now you can directly use the classes inside System
//         double result = Math.Sqrt(25);
//         Console.WriteLine(result); 
//     }
// }
// Use code with caution.

//# Example 3: Working with System.IO
// The System.IO namespace contains classes for reading and writing files. It is a sub-namespace inside System.csharp

// using System;
// using System.IO; 

// class Program
// {
//     static void Main()
//     {
//         string path = "example.txt";
//         string content = "Hello, C#!";

//         // Predefined WriteAllText method from predefined File class
//         File.WriteAllText(path, content);

//         // Predefined ReadAllText method
//         string readText = File.ReadAllText(path);
//         Console.WriteLine(readText);
//     }
// }

//Note: # system and system.IO are two different name spaces ?? If no, then why did we have to import Syatem.IO again ?? wasn't just the system enough ??
// No. Importing System is not enough to use System.IO. 
// While they are closely related, C# treats them as two distinct namespaces.

//# The Nested Folder Concept
// Think of namespaces like folders on your computer:
// System is the main folder. It contains basic tools like Console and Math.
// System.IO is a subfolder inside the main folder. It contains file-handling tools like File and Directory.
// In C#, importing a main folder does not automatically import its subfolders. Each sub-namespace must be explicitly imported.

// Code Comparison
// Here is how the compiler sees your shortcuts based on your using directives:
// If you only write using System;✅
//   You can type Console instead of System.Console.❌ 
//   You cannot type File. You must type the full path: System.IO.File.
// If you write both using System; and using System.IO;✅ 
// You can type Console.✅ 
// You can type File.

// Why does C# work this way?
// Performance: It keeps the compiler efficient by only searching the specific locations you ask it to search.
// Naming Conflicts: It prevents name clashes. If System automatically imported every sub-namespace, a class named File in one subfolder might conflict with a class named File in another subfolder.


//# Both System.Collections and System.Collections.Generic are namespaces.You are completely right about the nested structure. Here is how it breaks down:
// The Nested Structure
// System: This is the top-level (root) namespace provided by Microsoft.
// Collections: This is a sub-namespace inside System.
// Generic: This is a sub-namespace inside Collections.

// In C#, dots (.) are used to show this hierarchy.Visual HierarchyYou can picture it like folders on your computer:text📁 System (Root Namespace)
//    │
//    └── 📁 Collections (Sub-namespace)
//         │   📄 ArrayList, Hashtable, IEnumerable (Classes/Interfaces)
//         │
//         └── 📁 Generic (Sub-namespace)
//                 📄 List<T>, Dictionary<TKey, TValue>, IEnumerable<T>
// Use code with caution.Why are they built this way?Developers nest namespaces to group related code logically.Everything related to storing data goes inside Collections.The specific tools that use modern "Generics" (type safety) are grouped one step deeper inside Generic so they do not get mixed up with the older tools.

//# Namespaces
// A namespace is a container used to organize code files and prevent name conflicts.
// System.Collections: Contains older, non-generic collection types(like ArrayList or Hashtable). It provides the classic, non-generic IEnumerable interface.
// System.Collections.Generic: Contains modern, type-safe collection types(like List<T> or Dictionary<TKey, TValue>). It provides the generic IEnumerable<T> interface used in the previous example.

// Classes and Interfaces
// In the specific snippet you provided, System.Collections and System.Collections.Generic are namespaces, so they contain types rather than being classes themselves.Here are the prominent classes and interfaces that live inside them:
// Inside System.Collections.Generic
// List<T>: A dynamic array class that automatically resizes.
// Dictionary<TKey, TValue>: A class representing a collection of key-value pairs.
// Queue<T>: A class that stores items in a first-in, first-out (FIFO) order.
// Stack<T>: A class that stores items in a last-in, first-out (LIFO) order.
// IEnumerable<T>: Note: This is an interface, not a class. It defines the structure for forward-only iteration over a generic collection.

// Inside System.Collections
// ArrayList: An older, non-generic class that stores dynamically resizing arrays of objects.
// Hashtable: An older, non-generic class that stores key-value pairs of objects.
// IEnumerable: Note: This is an interface, not a class. It defines the structure for forward-only iteration over a non - generic collection.

//# Essential Namespaces Under System:

// System.Collections.Generic: Defines type-safe generic collection classes like List<T>, Dictionary<TKey, TValue>, and Queue<T>.
// System.IO: Contains types that handle reading and writing to files, streams, and directories.System.Linq: Provides classes and interfaces that support Language Integrated Query queries for filtering and transforming data.
// System.Text: Holds classes like StringBuilder and support for abstract character encodings(ASCII, UTF-8).
// System.Text.Json: Offers high-performance, low-allocating tools for serializing and deserializing JSON text.
// System.Threading.Tasks: Simplifies concurrent and asynchronous programming using tasks via async and await.
// System.Net.Http: Contains the HttpClient class and components for modern programming over HTTP networks.
// System.Data: Represents the foundational components of the ADO.NET architecture used for accessing relational databases
// System.Diagnostics: Provides classes to interact with system processes, event logs, performance counters, and debugging tools


//# Works perfectly without 'using System;'
// string text = "Hello World"; 
// string upper = text.ToUpper(); 

// // Throws a compilation error unless 'using System;' is declared at the top
// String text = "Hello World"; 

// // Works without 'using System;'
// string.Concat("A", "B"); 

// // Requires 'using System;'
// String.Concat("A", "B"); 

// string (Lowercase)| C# Reserved
// String (Uppercase)| .NET Framework Class


//# The Difference: Properties vs. Fields
// Field: A variable declared directly inside a class to hold raw data. | private int age; | 
// Encapsulation Poor, exposing fields directly breaks encapsulation because data cannot be validated before being saved

// Property:A wrapper around a field that acts like a method but looks like a variable.| public int Age { get; set; }. Excellent encapsulation. You can intercept data access using get and set blocks to run validation logic.


//? Classes?
// Classes are reference types that can contain fields, properties, methods, events, and other members. 
// They are used to create objects that can have state and behavior. 
// Classes can be instantiated using the new keyword, and they support inheritance and polymorphism.

//# Example of a Class and why do we need to instantiate it?
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

// # difference between class with constructor and class without constructor
// In C#, every class has a constructor, even if you do not write one explicitly. The primary difference lies in whether you use the compiler-generated default constructor or define a custom constructor to control object initialization


// Class Without Explicit Constructor
// When you omit a constructor, C# provides an empty, parameterless one behind the scenes. It allocates memory and sets fields to their default language types.

// public class User
// {
//     public string Name; // Defaults to null
//     public int Age;     // Defaults to 0
// }

// // Instantiation:
// User user = new User(); 
// user.Name = "Alice"; // Set manually after creation
// user.Age = 30;


// Class With Custom Constructor
// Writing your own constructor stops the compiler from generating the automatic default one. This forces developers to supply specific arguments right at the moment of creation 

// public class User
// {
//     public string Name { get; } // Read-only property
//     public int Age { get; }    // Read-only property

//     // Custom parameterized constructor
//     public User(string name, int age)
//     {
//         Name = name;
//         Age = age;
//     }
// }

// // Instantiation:
// // User user = new User(); // <-- This line would now throw a compiler error!
// User user = new User("Alice", 30); // Values are locked in immediately

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

//# If the method does not have the static keyword, it belongs to an object, not the class itself. You must create an instance (object) of that class before calling the method

//#Note: In C#, a static class cannot be inherited and cannot be instantiated

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

//# A non-static class can still contain static methods. If the method is marked as static, it belongs to the class itself. You do not need to create an object; you can call it 

//# A static class's methods automatically belong to the class itself. You cannot create an instance of a static class, so you must call its methods directly using the class name.

//# Quick Rules to Remember
// Instance Methods: Require new ClassName().methodName().
// Static Methods: Require ClassName.methodName().
// Access Modifiers: Ensure the method is marked as public or has the appropriate visibility so the other class can see it

// To access a method of another static class, you must call the method using the class name directly, rather than creating an object instance. Because static classes cannot be instantiated, their methods belong directly to the class blueprint itself.The standard syntax across most object-oriented programming languages is:textClassName.MethodName(arguments);

// To access a method belonging to a static class (or a static method within a regular class), you must call it directly using the class name. You do not—and cannot—create an instance of that class using the new keyword

// The other static class
// public static class Calculator 
// {
//     public static int Add(int a, int b) => a + b;
// }

// // Accessing it from a separate class
// public class Program 
// {
//     public static void Main() 
//     {
//         // Direct call using the class name
//         int result = Calculator.Add(5, 10); 
//     }
// }

//Note: When a class is made static, it means that the class cannot be instantiated and can only contain static members (methods, properties, fields, etc.).

//# String must be a static method to be called without instantiation then how are we able to create string using string str = new String()?
// You cannot call a regular instance method on a class without making an object first. However, string in C# is special. new string(...) works because it calls a constructor, which is a special creation method, not a normal method.Why new string(...) works
// A constructor is a special method used to build a new object.




//# How are we able to call IsNullOrEmpty() method without instantiation of string class?
// You can call string.IsNullOrEmpty() without instantiating a string object because it is a static method.

// How It Works
// Static Keyword: In C#, the IsNullOrEmpty method is defined with the static keyword inside the String class.Class-Level Belonging: Static methods belong to the class itself, not to any specific instance of the class.No Instance Needed: You call static methods by using the class name directly (e.g., string.IsNullOrEmpty(...)) instead of a variable name.
//Code Example
// csharp
// You do not need to create an object like this:
// string str = new string(); 

// Instead, you call it directly on the class:
// bool isEmpty = string.IsNullOrEmpty(""); 


// We make a class static primarily to create a blueprint that cannot be instantiated or inherited, acting strictly as a stateless container for shared data and utility behaviors.
// By declaring a class as static, you signal to the compiler and other developers that the class contains only global, reusable functionality that operates independently of any object state.

//# What does stateless container mean? what does stateless mean in this context?
// Stateless means that the class does not maintain any internal state or instance-specific data. In other words, it does not have instance variables that can change over time or differ between objects because there are no objects of a static class. All its members (methods, properties, fields) are static and shared across the entire application.

// Key Reasons to Use a Static Class:

// Enforce Design Constraints: It acts as a safety guard. The compiler actively prevents developers from accidentally instantiating the class using the new keyword or adding instance-level variables.

// Grouping Utility Methods: It cleanly bundles global "pure functions" (helper methods) that only operate on their input parameters, like the Microsoft Learn documented .NET Math class (Math.Max(), Math.Round()).

// Enable Extension Methods: In languages like C#, extension methods (which add new capabilities to existing types) must be defined inside a top-level static class.

// Prevent Architectural Overhead: Because there are no objects to track, the application domain loads the class once into memory for its entire lifecycle, removing the overhead of creating and destroying temporary objects.

// Namespace Organization: It provides a clean, localized namespace for constants or configuration flags that belong to a single domain (e.g., AppConfig.Version), preventing global scope pollution.


//# Core Technical Characteristics
// Feature
// Static Class Behavior
// Instantiation- Strictly forbidden; cannot use the new operator.
// Inheritance- Implicitly sealed; it cannot be inherited or act as a base class.
// Allowed Members - Can only contain static methods, static fields, static properties, and a single static constructor.Memory Allocation - Stored in a specialized global memory space for the lifetime of the application runtime.



//# In C#, a static class cannot be instantiated or inherited because it is designed strictly as a stateless container for utility methods and constants. The C# compiler enforces these constraints at compile time to ensure global availability and memory efficiency

//$ Why It Cannot Be Instantiated:
// No instance members: A static class can only contain static fields, methods, properties, and events. Because there is no instance state to track or manage, creating an object of the class serves no purpose.

// No instance constructors: You cannot use the new keyword because the compiler prevents the class from having an instance constructor. It can only have a single static constructor to initialize global data.

// Syntax Error: Attempting var obj = new MyStaticClass(); will throw a compiler error.


//$ Why It Cannot Be Inherited:
// Implicitly sealed: The compiler automatically marks every static class as sealed behind the scenes. In C#, a sealed class completely blocks object-oriented inheritance.

// Cannot act as a base or derived class: A static class cannot inherit from any other class except System.Object, nor can any other class use it as a base class.

//$ Example:

// Declaring a static class
// public static class Calculator
// {
//     // All members must be marked 'static'
//     public static double Pi = 3.14159;

//     public static int Add(int a, int b)
//     {
//         return a + b;
//     }
// }

// public class Program
// {
//     public static void Main()
//     {
//         // Correct Usage: Access directly via the class name
//         int sum = Calculator.Add(5, 10); 

//         // COMPILER ERROR: Cannot create an instance of the static class
//         // Calculator calc = new Calculator(); 
//     }
// }

// COMPILER ERROR: Cannot derive from static class 'Calculator'
// public class AdvancedCalculator : Calculator { } 




//? Interfaces?
// Interfaces are contracts that define a set of methods and properties that a class must implement.
// Use case: Interfaces are used to achieve abstraction and multiple inheritance in C#. They allow you to define a contract that classes can implement, 
// ensuring that they provide specific functionality.
// For example, you might have an interface called "IVehicle" that defines methods like "Start()" and "Stop()". 
// Any class that implements the IVehicle interface must provide an implementation for these methods, allowing you to work with 
// different types of vehicles (e.g., Car, Bike) through a common interface.  

//Note:  In C#, classes do not technically "inherit" from an interface; instead, they implement it. An interface acts as a strict contract containing method and property declarations without bodies. Any class signing this contract must provide the concrete code for those members


// 1. Simple Single Interface ImplementationThis foundational example shows how unrelated classes (a Car and a Phone) can both fulfill a contract requiring battery status checks.
// using System;

// // 1. Define the interface contract
// public interface IBatteryCharged
// {
//     // Interface members are public by default and cannot contain fields
//     int GetBatteryPercentage();
//     void Charge();
// }

// // 2. Class implementing the interface
// public class Smartphone : IBatteryCharged
// {
//     private int _batteryLevel = 50;

//     public int GetBatteryPercentage()
//     {
//         return _batteryLevel;
//     }

//     public void Charge()
//     {
//         _batteryLevel = 100;
//         Console.WriteLine("Smartphone fully charged.");
//     }
// }

// // 3. Unrelated class implementing the same interface
// public class ElectricCar : IBatteryCharged
// {
//     private int _chargeLevel = 20;

//     public int GetBatteryPercentage()
//     {
//         return _chargeLevel;
//     }

//     public void Charge()
//     {
//         _chargeLevel = 100;
//         Console.WriteLine("Tesla Supercharger connected. Car fully charged.");
//     }
// }


// 2. Multiple Interface ImplementationBecause C# does not support multiple inheritance with normal classes, interfaces are used to assign multiple independent behaviors to a single class.csharpusing System;

// public interface IPrintable
// {
//     void Print();
// }

// public interface IScannable
// {
//     void Scan();
// }

// // The class implements both contracts, separated by a comma
// public class AllInOnePrinter : IPrintable, IScannable
// {
//     public void Print()
//     {
//         Console.WriteLine("Printing document copy...");
//     }

//     public void Scan()
//     {
//         Console.WriteLine("Scanning document to PDF format...");
//     }
// }


// 3. Explicit Interface Implementation (Resolving Conflicts)When a class implements two interfaces that share a method with the exact same name and signature, you use explicit implementation to prevent compiler ambiguity.csharpusing System;

// public interface ILeftTurn
// {
//     void Signal();
// }

// public interface IRightTurn
// {
//     void Signal();
// }

// public class AutonomousVehicle : ILeftTurn, IRightTurn
// {
//     // Explicit implementation: Prefix the method name with the Interface name
//     // Note: Access modifiers like 'public' are omitted here
//     void ILeftTurn.Signal()
//     {
//         Console.WriteLine("Blinking left amber light.");
//     }

//     void IRightTurn.Signal()
//     {
//         Console.WriteLine("Blinking right amber light.");
//     }
// }

// // How to call explicit interfaces in your code:
// class Program
// {
//     static void Main()
//     {
//         AutonomousVehicle car = new AutonomousVehicle();

//         // Direct call fails because the methods are bound explicitly to the interfaces.
//         // You must cast the object to the specific interface type first:
//         ILeftTurn leftSide = (ILeftTurn)car;
//         leftSide.Signal(); // Outputs: Blinking left amber light.

//         IRightTurn rightSide = (IRightTurn)car;
//         rightSide.Signal(); // Outputs: Blinking right amber light.
//     }
// }

//# Examples of class inheriting any predefined interface in c#

// Yes, a class in C# can inherit a predefined interface by using a colon after the class name, followed by the interface name. The class must then provide code for every method and property listed inside that interface.Common Predefined Interfaces
// IComparable - Compares two objects to sort them properly.
// IDisposable - Cleans up system resources or files when done.
// IEnumerable - Allows a collection of items to be looped through with a foreach statement.
// IEquatable - Checks if two objects are equal by value

//#// A class inheriting the predefined IDisposable interface
// using System;

// // A class inheriting the predefined IDisposable interface
// public class FileHandler : IDisposable
// {
//     public void Dispose()
//     {
//         // Put cleanup code here
//         Console.WriteLine("Resources cleaned up.");
//     }
// }

//# To use IEnumerable in C#, you implement its GetEnumerator() method, which allows your custom class to be looped through using a foreach loop.Here is a complete, runnable example using the generic version (IEnumerable<T>), which is the standard choice in modern C#.csharpusing System;
// using System.Collections;
// using System.Collections.Generic;

// // A custom collection class that implements IEnumerable
// public class ProductList : IEnumerable<string>
// {
//     private string[] _products = { "Laptop", "Smartphone", "Tablet" };

//     // Required by IEnumerable<T>
//     public IEnumerator<string> GetEnumerator()
//     {
//         foreach (string product in _products)
//         {
//             yield return product; // Hands over items one by one
//         }
//     }

//     // Required by older, non-generic IEnumerable (boiler-plate code)
//     IEnumerator IEnumerable.GetEnumerator()
//     {
//         return GetEnumerator();
//     }
// }

// class Program
// {
//     static void Main()
//     {
//         ProductList storeItems = new ProductList();

//         // Because ProductList is IEnumerable, we can use foreach
//         foreach (string item in storeItems)
//         {
//             Console.WriteLine(item);
//         }
//     }
// }

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


//$ C# class example with properties, fields, methods, static methods, constructor and Main method which also inherits from a class and an interface 

// using System;

// namespace CSharpExample
// {
//     // 1. Base Class
//     public class Vehicle
//     {

// PROTECTED FIELD: Accessible within this class and derived classes (like Car)
// protected string registrationId;

// PUBLIC FIELD: Accessible from anywhere (Generally discouraged for raw fields)
// public string vehicleType;

//         // Field in base class
//         protected string brand;

//         // Base class constructor
//         public Vehicle(string brand)
//         {
//             this.brand = brand;
//         }

//         // Method in base class
//         public void StartEngine()
//         {
//             Console.WriteLine($"The {brand}'s engine is starting...");
//         }
//     }

//     // 2. Interface defining a contract
//     public interface IDriveable
//     {
//         // Interface Property
//         int TopSpeed { get; }

//         // Interface Method
//         void Drive();
//     }

//     // 3. Derived Class inheriting from Vehicle and implementing IDriveable
//     public class Car : Vehicle, IDriveable
//     {
//         // A. Fields (Private data variables)
//         private string model;
//         private int currentSpeed;

//         // B. Static Field (Belongs to the class itself, shared across instances)
//         private static int totalCarsCreated = 0;

//         // C. Properties (Encapsulated accessors for fields)
//         public string Model
//         {
//             get { return model; }
//             set { model = value; }
//         }

// //      PRIVATE FIELD: Only accessible inside this specific class
//         private int currentFuel;

//         // PUBLIC PROPERTY: Fully open to the public for reading and writing
//         public string Model { get; set; }

//         // PROTECTED PROPERTY: Only accessible within Car and any class that inherits from Car
//         protected bool IsEngineRunning { get; set; }

//         // Interface Property implementation
//         public int TopSpeed { get; private set; }

//         // D. Constructor (Initializes fields and calls base class constructor)
//         public Car(string brand, string model, int topSpeed) : base(brand)
//         {
//             this.model = model;
//             this.TopSpeed = topSpeed;
//             this.currentSpeed = 0;

//             // Increment the static counter every time a new instance is created
//             totalCarsCreated++;
//         }

//         // E. Instance Method (Implements interface method)
//         public void Drive()
//         {
//             currentSpeed = 50;
//             Console.WriteLine($"Driving the {brand} {model} at {currentSpeed} mph.");
//         }

//         // F. Overloaded Instance Method (Specific to this class)
//         public void Accelerate(int speedIncrease)
//         {
//             currentSpeed += speedIncrease;
//             if (currentSpeed > TopSpeed) currentSpeed = TopSpeed;
//             Console.WriteLine($"Accelerating! Current speed: {currentSpeed} mph.");
//         }

//         // G. Static Method (Can be called without creating an object instance)
//         public static void DisplayTotalCars()
//         {
//             Console.WriteLine($"Total cars manufactured so far: {totalCarsCreated}");
//         }

//         // H. Main Method (The application entry point)
//         public static void Main(string[] args)
//         {
//             // Call static method before creating instances
//             Car.DisplayTotalCars();

//             // Instantiate a new Car object (triggers the constructor)
//             Car myCar = new Car("Tesla", "Model S", 155);

//             // Access inherited method from Vehicle base class
//             myCar.StartEngine();

//             // Access implemented method from IDriveable interface
//             myCar.Drive();

//             // Access specific instance method and public property
//             myCar.Accelerate(40);
//             Console.WriteLine($"This car's structural limit is: {myCar.TopSpeed} mph.");

//             // Call static method again to see the updated count
//             Car.DisplayTotalCars();
//         }
//     }
// }

//$ ***************************************************************************************************************/

// Corrected Syntax
// private readonly ITaskRepository _repo;


// Understanding the Keywords
// private: Restricts access to this field. Only code inside the TaskService class can see or use _repo. External classes cannot access it.

// readonly: Ensures immutability. This keyword specifies that the field can only be assigned a value in two places
// 1:Inline where it is declared (e.g., private readonly int x = 5;).Inside the Constructor of the class (as seen in your example).Once the constructor finishes executing, the reference cannot be changed to point to a different repository.

// 2. ITaskRepository / IMediator: These are Interfaces. Instead of depending on a concrete, hardcoded class (like SqlTaskRepository), the service depends on a flexible contract.


// Why is this pattern used? (Dependency Injection)
// This pattern is called Constructor Dependency Injection. Instead of a class creating its own dependencies using the new keyword, the dependencies are "injected" from the outside through the constructor.


// using System;
// using System.Threading.Tasks;

// namespace ArchitectureExample
// {
//     // 1. Base Class for application services
//     public abstract class BaseService
//     {
//         protected readonly string ServiceLayerName = "Core Application Layer";
//     }

//     // 2. Mock Interfaces for the dependencies
//     public interface ITaskRepository 
//     { 
//         void SaveTask(string taskName); 
//     }

//     public interface IMediator 
//     { 
//         void PublishEvent(string eventName); 
//     }

//     // 3. Interface defining our service contract
//     public interface ITaskService
//     {
//         void CreateTask(string name);
//     }

//     // 4. Concrete implementation using Constructor Injection
//     public class TaskService : BaseService, ITaskService
//     {
//         // YOUR SNIPPETS (Corrected access modifiers)
//         private readonly ITaskRepository _repo;
//         private readonly IMediator _mediator;

//         // Constructor: The runtime DI container automatically passes these objects in
//         public TaskService(ITaskRepository repo, IMediator mediator)
//         {
//             // The readonly fields are safely assigned here
//             _repo = repo;
//             _mediator = mediator;
//         }

//         // Instance Method implementing interface logic
//         public void CreateTask(string name)
//         {
//             Console.WriteLine($"Running layer: {ServiceLayerName}");

//             // Using the injected dependencies safely
//             _repo.SaveTask(name);
//             _mediator.PublishEvent($"TaskCreated: {name}");
//         }

//         // Main Method simulating how a modern application hooks this together
//         public static void Main(string[] args)
//         {
//             // Simulate the system creating the concrete dependencies
//             ITaskRepository mockRepo = new SqlTaskRepository();
//             IMediator mockMediator = new SimpleMediator();

//             // Inject the dependencies into the service constructor
//             TaskService service = new TaskService(mockRepo, mockMediator);

//             // Execute application logic
//             service.CreateTask("Finish C# Tutorial");
//         }
//     }

//     // Concrete mock classes to make the application runnable
//     public class SqlTaskRepository : ITaskRepository
//     {
//         public void SaveTask(string taskName) => Console.WriteLine($"[SQL Database] Saved task: '{taskName}'");
//     }

//     public class SimpleMediator : IMediator
//     {
//         public void PublishEvent(string eventName) => Console.WriteLine($"[Mediator Hub] Broadcasted event -> {eventName}");
//     }
// }


// Major Benefits of This Approach
// Testability (Mocking): Because TaskService depends on the interface ITaskRepository rather than a real database connection, you can easily pass a "Fake/Mock" repository during testing. Your unit tests can verify logic without modifying a live database.

// Loose Coupling: If you choose to swap your storage layer from an SQL Database to a MongoDB Database, you only need to create a MongoTaskRepository that implements ITaskRepository. The TaskService code never has to change.

// Thread Safety: Marking fields as readonly guarantees that different parts of your program cannot accidentally swap out the operational dependencies under the hood while your code is actively running.


//? Dependency Injection (DI) and Inversion of Control (IoC)
// Dependency Injection (DI) is a design pattern that allows a class to receive its dependencies from an external source rather than creating them internally. This promotes loose coupling and makes the code more testable and maintainable. Inversion of Control (IoC) is a broader principle where the control of object creation and dependency management is inverted from the class itself to an external entity, often a DI container.

//# Constructor Injection is a software design pattern where a class's required dependencies are passed directly into its constructor as parameters. Instead of the class creating its own dependent objects internally using the new keyword, an external framework or caller supplies those dependencies at the exact moment the class is instantiated.

// This technique is the industry standard for achieving loose coupling, making applications significantly easier to test, maintain, and extend.

// Standard Implementation Example
// In a standard C# implementation, dependencies are represented by interfaces. The injected reference is typically saved into a private readonly field so it cannot be modified after initialization

// csharp// 1. Define the abstraction
// public interface IEmailService
// {
//     void SendEmail(string to, string message);
// }

// // 2. Client class using Constructor Injection
// public class NotificationManager
// {
//     private readonly IEmailService _emailService;

//     // The dependency is injected here via the constructor parameters
//     public NotificationManager(IEmailService emailService)
//     {
//         _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
//     }

//     public void NotifyUser(string email, string text)
//     {
//         // Use the injected service safely
//         _emailService.SendEmail(email, text);
//     }
// }


//# Modern Syntax: Primary Constructors (C# 12+)
// If you are using modern .NET, you can drastically reduce boilerplate code by utilizing C# 12 Primary Constructors. This syntax inline-declares the parameters directly on the class declaration definition:

// The dependency is declared alongside the class name
// public class ModernNotificationManager(IEmailService emailService)
// {
//     public void NotifyUser(string email, string text)
//     {
//         // The parameter is automatically in scope throughout the class
//         emailService.SendEmail(email, text);
//     }
// }

//# Key Benefits
// Guaranteed Initialization: A class cannot be instantiated without providing its dependencies. This entirely prevents runtime NullReferenceException errors caused by missing setups.

// Immutability: Saving the injected service into a readonly field ensures that the dependency cannot be accidentally changed or overwritten later during execution.

// Frictionless Unit Testing: You can easily swap out production classes with mock objects (e.g., using frameworks like Moq or NSubstitute) to test the class logic in complete isolation.

// Self-Documenting Code: The constructor parameter signature acts as an explicit contract, telling other developers exactly what the class needs to function

//# How it fits into .NET?
// While you can pass dependencies manually, modern C# applications rely on a built-in IoC/DI Container provided by Microsoft. When you register your services in the application startup (e.g., Program.cs), the container automatically looks at your constructors, resolves the requested parameters, and instantiates the classes dynamically


//# 1. Alternative Dependency Injection Types

// While constructor injection is the industry standard, two other DI patterns are used for specific use cases:
// Method Injection: Passing a dependency directly into a single method as a parameter. This is heavily utilized in ASP.NET Core Minimal APIs via the [FromServices] attribute when only one specific endpoint needs the service.

// Property (Setter) Injection: Setting a dependency through a public property after object creation. This is less common but useful for optional dependencies or frameworks requiring parameterless constructors.

//# 2. Service Lifetime Management
// In modern C#, you rarely inject dependencies manually. Instead, you use the built-in .NET Dependency Injection Container, which requires understanding the three core service lifetimes:

// Transient: A new instance of the dependency is created every single time it is requested.
// Scoped: A single instance is created per client request (highly critical for managing database contexts in web applications).
// Singleton: One single instance is created once and shared across the entire application lifetime

//# Closely Related Architectural Patterns
// If you like the structural decoupling provided by constructor injection, you will frequently see these companion patterns in production codebases:
// The Options Pattern: Used to inject strongly-typed configuration settings into a class via the IOptions<T> interface, following the exact same workflow as constructor injection.

// Factory Pattern: Used alongside DI when you cannot determine a dependency at compile time and need to generate objects dynamically at runtime based on user input or state.

// Mediator Pattern (CQRS): Often implemented using libraries like MediatR, this pattern reduces direct constructor-to-constructor dependencies by routing all internal application communication through a single injected mediator service.



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
//     Sunday = 1,
//     Monday = 2,
//     Tuesday = 3,
//     Wednesday = 4,
//     Thursday = 5,  
//     Friday = 6,
//     Saturday = 7
// }

// Use case:
// Enums are ideal for representing a fixed set of related values, such as days of the week, months of the year, or status codes.
// For example, you might use an enum to represent the different states of an order in an e-commerce application, such as "Pending", "Processing", "Shipped", and "Delivered".
// By using an enum, you can improve code readability and maintainability, as it provides a clear and concise way to represent a set of related values,
// and it also helps to prevent invalid values from being assigned to variables that use the enum type.

//? Enums with explicit values
// By default the first member is 0 and each following member is the previous value + 1. You can override
// any member with an explicit value, and members after it that don't specify their own value just keep
// auto-incrementing from wherever the last explicit value left off (not from 0, and not from their
// declaration position).

// public enum DayOfWeek
// {
//     Sunday = 1,
//     Monday = 2,
//     Tuesday,     // 3 (Monday + 1)
//     Wednesday,   // 4
//     Thursday,    // 5
//     Friday,      // 6
//     Saturday     // 7
// }

// Why bother assigning values at all?
// - Matching an external contract: e.g. mirroring a day-of-week convention where 1 = Sunday (some
//   systems/APIs, and even .NET's own built-in System.Enum.DayOfWeek, use 0 = Sunday - so an explicit
//   value lets your enum agree with whatever source you're integrating with, instead of relying on
//   declaration order matching by coincidence).
// - Wire/storage stability: since enums serialize to their underlying int by default (see the numeric
//   vs string enum note elsewhere in this codebase), explicit values let you insert a new member in the
//   middle later without silently reassigning the numbers of everything below it and breaking anything
//   that persisted the old numbers (a DB column, a saved API payload, etc).
// - Bit flags: with [Flags], members are deliberately given power-of-two values (1, 2, 4, 8, ...) so they
//   can be OR'd together into a single combined value - auto-incrementing (0,1,2,3) would not work for that.

// Gotcha: two members CAN share the same explicit value (they become interchangeable aliases of the same
// underlying number), which is easy to do by accident if you hardcode values instead of using the
// auto-increment default:
// public enum Status
// {
//     Active = 1,
//     Enabled = 1   // compiles fine - Active and Enabled are now indistinguishable at runtime
// }

//? Delegates
// Delegates are a type that represents references to methods with a specific parameter list and return type.
// They are used to pass methods as arguments to other methods, allowing for flexible and reusable code
// Use case: Delegates are often used for event handling, callbacks, and implementing the observer pattern.
// For example, you might define a delegate called "EventHandler" that takes an object sender and EventArgs as parameters.
// You can then use this delegate to handle events in your application, such as a button click event in a user interface. 
// By using delegates, you can decouple the event handling logic from the event source, allowing for more modular and maintainable code.   
// Example of a Delegate:
// public delegate void EventHandler(object sender, EventArgs e);


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
// int time = 20;
// string greeting = (time < 18) ? "Good day." : "Good evening.";

//? Null Coalescing operator

//? Nullable operator

//? String methods:
// string.IsNullOrEmpty()
// string.IsNullOrWhiteSpace()
// string.Substring()
// string.Replace()
// string.Split()
// string.Join()
// string.Trim()
// string.contains()


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


//? Extension method
// Extension methods allow you to add new methods to existing types without modifying the original type or creating a new derived type.
// They are defined as static methods in a static class, and the first parameter of the method specifies the type that the method operates on, preceded by the "this" keyword.

// Example of an Extension Method:
// public static class StringExtensions
// {
//     public static bool IsNullOrEmpty(this string str)
//$ this keyword indicates that this method is an extension method for the string type
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

//? Attributes in C#
// Attributes are a way to add metadata to your code, which can be used to provide additional information about classes, methods, properties, and other code elements.
// Example:
// [Obsolete("This method is obsolete. Use NewMethod instead.")]
// [Authorize(Roles = "Admin")]
// 

// In C#, attributes are declarative tags used to add metadata or extra behavioral context to program elements like classes, methods, properties, or entire assemblies. This metadata can be consumed by the compiler, external tools, or read at runtime using Reflection

// Attributes are enclosed in square brackets [ ] and placed immediately above the target element. By convention, attribute class names end with the suffix Attribute, but you can omit it when applying them

// [Serializable] // Tells the compiler/runtime this class can be serialized
// public class User
// {
//   [Obsolete("Use NewMethod instead.")] // Triggers a compiler warning if used
//   public void OldMethod() { }
// }

// The.NET ecosystem provides thousands of pre-defined attributes across different namespaces.
// [Obsolete] – Generates a compiler warning or error when the marked code is used.
// [Serializable] – Indicates that a class can be converted into a byte stream.


// //# Creating a Custom Attribute
// // You can create custom attributes by inheriting from the System.Attribute base class. You can optionally use the [AttributeUsage] attribute to limit where your custom tag can be applied.

// csharpusing System;

// // Limit this attribute to only Classes and Properties
// [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
// public class DeveloperInfoAttribute : Attribute
// {
//   public string Name { get; }
//   public string Project { get; set; } // Optional/named parameter

//   // Positional parameter (required via constructor)
//   public DeveloperInfoAttribute(string name)
//   {
//     Name = name;
//   }
// }

// // Applying the custom attribute
// [DeveloperInfo("Alice", Project = "Billing System")]
// public class AccountService
// {
//   // ... code ...
// }



