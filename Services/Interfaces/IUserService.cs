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
    Task<User?> GetUserByIdAsync(int id);
    Task<User> CreateUserAsync(User user);
    Task<User?> UpdateUserAsync(int id, User user);
    Task<bool> DeleteUserAsync(int id);
  }
}


//! This will explain variables, methods, and access modifiers used in the IUserService interface:
// class Program
// {
//   static void Main(string[] args)
//   {

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