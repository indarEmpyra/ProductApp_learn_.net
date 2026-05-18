using ProductApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace ProductApp.Services.Interfaces
{
  public interface IUserService
  {
    Task<List<User>> GetAllUsersAsync();
    // Task<List<User>> indicates that this method is asynchronous and returns a List of User objects when awaited.
    // The method name GetAllUsersAsync follows the convention of ending with "Async" to indicate that it is an asynchronous method.
    // This method is likely responsible for retrieving all user records from a data source, such as a database, and returning them as a list of User objects.

    // We must define the same method in implementing class (UserService) and provide the actual implementation for retrieving users from the data source.

    //? Why is this separation of interface and implementation useful?
    // 1. Abstraction: It allows us to define a contract (interface) that specifies what operations can be performed without 
    // exposing the details of how those operations are implemented.
    // 2. Loose Coupling: It promotes loose coupling between components, making it easier to change the implementation 
    // without affecting the code that depends on the interface.
    // 3. Testability: It makes it easier to write unit tests by allowing us  to mock the interface and test the 
    // behavior of the code that depends on it without needing to rely on the actual implementation.    

    // with this, what we can do is, we can create a mock implementation of IUserService for testing purposes, 
    // which allows us to test the UserController without needing to set up a real database or data source.



    Task<User?> GetUserByIdAsync(int id);
    // Task<User?> indicates that this method is asynchronous and returns a User object or null when awaited.
    // The method name GetUserByIdAsync follows the convention of ending with "Async" to indicate that it is an asynchronous method.
    // This method is likely responsible for retrieving a single user record based on the provided id parameter. 
    // If a user with the specified id exists, it returns the User object; otherwise, it returns null.
    Task<User> CreateUserAsync(User user);
    // Task<User> indicates that this method is asynchronous and returns a User object when awaited.
    // The method name CreateUserAsync follows the convention of ending with "Async" to indicate that it is an asynchronous method.
    // This method is likely responsible for creating a new user record in the data source based on the provided User object. 
    // It takes a User object as a parameter, which contains the details of the user to be created, and returns the created User object, which may include additional information such as an assigned id or timestamps.
    Task<User?> UpdateUserAsync(int id, User user);
    // Task<User?> indicates that this method is asynchronous and returns a User object or null when awaited.
    // The method name UpdateUserAsync follows the convention of ending with "Async" to indicate that it is an asynchronous method.
    // This method is likely responsible for updating an existing user record in the data source based on the provided id and User object.
    // It takes an id parameter to identify which user to update and a User object containing the updated details.
    // If the update is successful and the user exists, it returns the updated User object; otherwise, it returns null.
    Task<bool> DeleteUserAsync(int id);
    // Task<bool> indicates that this method is asynchronous and returns a boolean value when awaited.
    // The method name DeleteUserAsync follows the convention of ending with "Async" to indicate that it is an asynchronous method.
    // This method is likely responsible for deleting a user record from the data source based on the provided id parameter.
    // It takes an id parameter to identify which user to delete and returns true if the deletion was successful, 
    // or false if the user with the specified id does not exist or if the deletion failed for some reason.  
  }
}


//? what's the difference between class, interface, record and namespace in c#?

//! This will explain variables, methods, and access modifiers used in the IUserService interface:
// class Program
// {
//   static void Main(string[] args)
//   {

//# static
// Use the static modifier to declare a static member, which belongs to the type itself rather than to a specific object. 
// The static modifier can be used with classes, fields, methods, properties, operators, events, and constructors, but it cannot be used 
// with indexers, destructors, or types other than classes. For more information, see Static Classes and Static Class Members (C# Programming Guide).

// The static modifier also has more specific uses in C#, such as defining extension methods (which can only be defined inside of a static class), 
// defining interop methods, etc. 
// It's also worth noting that all static classes are sealed in C#, because without a constructor, they cannot be inherited from.

// api response helper
// namespace ProductApp.Helpers
// {
//   public static class ApiResponseHelper
//   {}

//? Why did we make it static? Because we don't need to create an instance of ApiResponseHelper to use its methods.
// Static classes are useful for grouping related utility methods that don't require any instance data.

//? What will happen if I don't make it static? 
// If we don't make the ApiResponseHelper class static, we would need to create an instance of it every time we want to use its methods.
// For example, we would have to do something like this in our controllers:
// var apiResponseHelper = new ApiResponseHelper();

//# Access Modifiers in C#:
// public : No restrictions; accessible from any code in any assembly.
// private: Restricted to the containing class or struct only.
// protected: Accessible within the same class or by derived classes.
// internal: Accessible only within the same assembly (project).
// protected internal: Accessible within the same assembly OR from derived classes in another assembly.
// private protected: Accessible only within the same assembly AND by derived classes (C# 7.2+).

//#     public int x = 10; 
// This is a field declaration. It defines a variable named x of type int that belongs to the class Program.
// Example usage of x would be: Console.WriteLine(x); which would print the value of x to the console, which is 10.

//#     private int y = 20; 
// This is also a field declaration, but with private access modifier. 
// It defines a variable named y of type int that can only be accessed within the Program class.
// Example usage of y would be: Console.WriteLine(y); but this would only work if it's called from within the Program class,
// since y is private. If you try to access y from outside the Program class, you will get a compile-time error.

//#     public void MyMethod(){ ... }
// This is a method declaration. It defines a method named MyMethod that returns void (no value) and takes no parameters.
// Example usage of MyMethod would be: MyMethod(); which would call the method and execute any code inside it.

//#    public int Add(int a, int b){ return a + b; }
// This methods return a value of "int" type.

//#    private void HelperMethod(){ ... }
// This is a private method, which means it can only be called from within the Program class.
// Example usage of HelperMethod would be: HelperMethod(); but this would only work if it's called from within the
// Program class, since it's private. If you try to call HelperMethod from outside the Program class, 
// you will get a compile-time error.

//#    public static void StaticMethod(){ ... }
// This is a static method, which means it belongs to the class itself rather than an instance of the class.
// Example usage of StaticMethod would be: Program.StaticMethod(); which would call the static method without
// needing to create an instance of the Program class. 
// Static methods are often used for utility functions that don't require any instance data. 

//#    public List<string> GetNames(){ return new List<string>(); }
// This method returns a List of strings. You can call it like this:
// List<string> names = GetNames(); which would assign the returned List of strings to the variable names.  

//#    public Dictionary<string, int> GetAges(){ return new Dictionary<string, int>(); }
// This method returns a Dictionary where the key is a string and the value is an int.

//#    List<int> numbers = new List<int>(1, 2, 3); 
//     numbers variable name is of type List<int>, which is a generic collection type
//     defined in the System.Collections.Generic namespace.

//     numbers.Add(1);
//     numbers.AddRange(IEnumerable collection): Adds all elements from another collection to the end of the current list.
//     numbers.RemoveAt(0);

//    List<string> fruits = new List<string> { "Apple", "Banana" };
//    fruits.Add("Cherry");           // List is now: Apple, Banana, Cherry
//    fruits.RemoveAt(1);             // Removes "Banana"
//    bool hasApple = fruits.Contains("Apple"); // returns true
//    fruits.Sort();    // Sorts the list alphabetically: Apple, Cherry
//    Console.WriteLine(fruits[0]); // Output: Apple            

//#   Dictionary<string, int> ages = new Dictionary<string, int>();
//     ages variable name is of type Dictionary<string, int>, which is a generic collection type
//     defined in the System.Collections.Generic namespace. It represents a collection of key-value pairs,
//     where the key is of type string and the value is of type int.
//     ages.Add("Alice", 30);
//     ages.Add("Bob", 25);
//     int aliceAge = ages["Alice"]; // Retrieves the age of Alice, which is 30
//     ages.Remove("Bob"); // Removes the entry for Bob


//#   Queue<string> queue = new Queue<string>();
// Queue: Implements First-In-First-Out (FIFO) logic
// queue.Enqueue("First");
// queue.Enqueue("Second");
// Console.WriteLine(queue.Dequeue()); // Output: First

//#   Stack<int> numbers = new Stack<int>();
// Stack: Implements Last-In-First-Out (LIFO)
// numbers.Push(1);
// numbers.Push(2);
// Console.WriteLine(numbers.Pop()); // Output: 2

//#   HashSet<int> uniqueNumbers = new HashSet<int> { 1, 2, 2, 3 };
// HashSet: An unordered collection that contains only unique elements
// Console.WriteLine(uniqueNumbers.Count); // Output: 3

//   }
// }


//$ Performance: Use HashSet<T> if you need to check for existence frequently, as it is much faster than searching a List<T>.
// Memory: HashSet<T> may use more memory than List<T> due to its internal structure, especially if the number of elements is small.
// Use List<T> when you need to maintain the order of elements or when you need to access elements by index. 
// Use HashSet<T> when you need to ensure uniqueness and don't care about order.

//$ Modern Syntax: For C# 12 and later, you can use Collection Expressions (e.g., List<int> list = [1, 2, 3];) to simplify initialization
//  of collections, and you can also use target-typed new expressions (e.g., List<int> list = new();) to avoid
//   repeating the type on the right-hand side when it's already clear from the left-hand side.  