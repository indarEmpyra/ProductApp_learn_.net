using Microsoft.AspNetCore.Mvc;
using ProductApp.Services.Interfaces;
using ProductApp.Models;
using ProductApp.RequestDTO;
using ProductApp.ResponseDTO;
using ProductApp.Helpers;

namespace ProductApp.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  //? [ApiController] is an attribute that indicates that the class is an API controller. 
  // It enables features such as automatic model validation and binding source inference.

  //? Can we create custom attributes?
  // Yes, you can create custom attributes in C# by defining a class that inherits from the System.Attribute base class. 
  // These attributes act as metadata that you can attach to program elements like classes, methods, or properties and retrieve later 
  // using Reflection.Steps to Create a Custom AttributeDefine the Class: Create a public class and inherit from System.Attribute. 
  // By convention, the class name should end with the suffix Attribute (e.g., AuthorAttribute), though you can omit the suffix when applying it.
  // Specify Attribute Usage: Use the [AttributeUsage] attribute on your new class to define where it can be applied (e.g., only to classes or methods) 
  // and whether multiple instances are allowed on a single element.

  //? [Route("api/[controller]")] defines the route template for the controller. 
  // The [controller] token is replaced with the name of the controller, which in this case is "User". 
  // So the base route for this controller will be "api/user". 

  //? Where did the assumption of "api/user" come from?
  // The route template "api/[controller]" uses the [controller] token, which is replaced by the name of the controller class without the "Controller" suffix. 
  // Since the class is named UserController, the [controller] token will be replaced with "User". Therefore, the base route for this controller will be "api/user".




  public class UserController : ControllerBase

  // What ControllerBase provides:
  // Ok()
  // BadRequest()
  // NotFound()
  // Created()
  // InternalServerError()
  // Json()
  // File()
  // Redirect()
  // RedirectToAction()
  // RedirectToRoute()
  // Content()
  // Empty()
  // FileContent()
  // FileStream()
  // FilePath()
  // And many more helper methods for generating HTTP responses.

  //? ControllerBase is a base class for an MVC controller without view support. 
  // It provides common functionality for handling HTTP requests and generating responses, such as model binding, validation, and formatting.

  //? We could also inherit from a custom base controller (e.g., ApiControllerBase) if we wanted to centralize
  // common logic for all our API controllers, such as standardized response handling, error logging, etc. 
  // With that, the code would look like this:
  // public class UserController : ApiControllerBase

  //? ApiControllerBase would be a custom base controller that we create to encapsulate common functionality for all our API controllers, 
  // such as standardized response handling, error logging, etc. 
  // By inheriting from ApiControllerBase, UserController would gain access to that shared functionality, allowing us to avoid 
  // code duplication and maintain consistency across our API controllers.

  //? example of ApiControllerBase:
  // public class ApiControllerBase : ControllerBase 
  // {
  //     protected IActionResult ApiResponse(object data = null, string message = null, bool success = true)
  //     {
  //         var response = new ApiResponse
  //         {
  //             Success = success,
  //             Message = message ?? (success ? "Operation completed successfully" : "An error occurred"),
  //             Data = data
  //         };
  //         return Ok(response);
  //     }

  //     protected IActionResult ApiError(string errorMessage, int statusCode = 500)
  //     {
  //         var response = new ApiResponse
  //         {
  //             Success = false,
  //             Message = errorMessage,
  //             Data = null
  //         };
  //         return StatusCode(statusCode, response);
  //     }

  //# how to use ApiResponse and ApiError in UserController:
  // public class UserController : ApiControllerBase
  // {
  //     [HttpGet("/users")]
  //     public async Task<IActionResult> GetAllUsers()
  //     {
  //        try {
  //          var users = await _userService.GetAllUsersAsync();
  //          var response = users.ToUserResponses();
  //          return ApiResponse(response);
  //         }
  //        catch (Exception ex) {
  //            return ApiError("An error occurred while retrieving users: " + ex.Message);
  //         }
  //     }

  //# if we don't define ApiResponse and ApiError in a base controller then we will have to handle response formatting manually, like this:
  // public class UserController : ControllerBase
  // {
  //    [HttpGet("/users")]
  //    public async Task<IActionResult> GetAllUsers()
  //    {
  //        try {
  //          var users = await _userService.GetAllUsersAsync();
  //          var response = users.ToUserResponses();
  //          return Ok(new ApiResponse { Success = true, Message = "Users retrieved successfully", Data = response });
  //         }
  //        catch (Exception ex) {
  //            return StatusCode(500, new ApiResponse { Success = false, Message = "An error occurred while retrieving users: " + ex.Message, Data = null });
  //         }
  //     }



  {
    private readonly IUserService _userService;

    // The IUserService is injected into the UserController through constructor injection.
    // This allows the controller to use the service to perform operations related to users, such as retrieving, creating, updating, and deleting user data.

    //? What is dependency injection?
    // Dependency injection is a design pattern used to implement inversion of control (IoC) in
    // which an object receives its dependencies from an external source rather than creating them itself.
    // This promotes loose coupling, easier testing, and better maintainability of the code.
    // In dependency injection, there are typically three main components:
    // 1. Service: The class that provides a specific functionality or service (e.g., IUserService).
    // 2. Client: The class that depends on the service to perform its operations (e.g., UserController).
    // 3. Injector: The component responsible for creating and injecting the dependencies into the client (e.g., a dependency injection container or framework).
    // By using dependency injection, you can easily swap out implementations of the service, mock dependencies for testing, and manage the 
    // lifecycle of dependencies more effectively.

    //? Types of Dependency Injection:
    // 1. constructor injection?
    // Constructor injection is a design pattern used in dependency injection where dependencies are provided to a class through its constructor. 
    // In this pattern, the required dependencies are passed as parameters to the class's constructor, allowing the class to receive and 
    // use those dependencies without having to create them internally. This promotes loose coupling and makes it easier to manage dependencies, 
    // especially in larger applications.

    // Use case: When the dependency is required for the class to function properly and should be immutable after the object is created.
    // i.e., when the dependency is essential for the class's operation

    // Real world example: A UserController that depends on an IUserService to perform user-related operations.
    // Example: public UserController(IUserService userService) { _userService = userService; }

    // 2. Property Injection: Dependencies are set through public properties after the object is created.
    // Use case: When the dependency is optional or can be changed after the object is created.

    // Real world example: A UserController that has an optional ILogger property for logging purposes.
    // Example: public ILogger Logger { get; set; }

    // 3. Method Injection: Dependencies are passed as parameters to methods when needed.
    // Use case: When the dependency is only needed for a specific method and not for the entire class.
    // Real world example: A UserController that has a method to perform an operation that requires an IUserService, 
    // but the service is not needed for other methods in the controller. 
    // Example: public void SomeMethod(IUserService userService) { ... }


    public UserController(IUserService userService)
    // userService is an instance of the IUserService interface that is injected into the UserController through constructor injection.
    {
      _userService = userService;
      // The constructor of the UserController class takes an IUserService as a parameter and assigns it to the private readonly field _userService.
      // This allows the UserController to use the IUserService instance to perform operations related to users,
      // such as retrieving, creating, updating, and deleting user data.
    }

    [HttpGet("/users")]

    //? [HttpGet("/users")] is an attribute that specifies that this action method should handle HTTP GET requests to the "/users" endpoint.
    // We can also add a route prefix to the controller (e.g., [Route("api/[controller]")]) 
    // and then use [HttpGet] without parameters to handle GET requests to "api/user".

    // We can also add custom attributes to the action method for additional functionality, such as authorization or caching.
    // Example of a custom attribute for logging:
    // [LogAction]
    // Example of a custom attribute for Authorization:
    // [Authorize(Roles = "Admin")]

    // These attributes can be defined as follows:
    // public class LogActionAttribute : ActionFilterAttribute
    // {
    //     public override void OnActionExecuting(ActionExecutingContext context)
    //     {
    //         // Log the action execution details here
    //         base.OnActionExecuting(context);
    //     }
    // }


    //NOTE: The [Authorize] attribute is a built-in attribute provided by the ASP.NET Core framework for handling authorization in web applications.

    //# Office code example of using [Authorize] attribute:

    // [Authorize(AuthenticationSchemes = gstrTokenKeyAuthenticationScheme, Roles = "Admin, Staff, Respondent, ExternalAPIClient")]
    // [HttpPost("/api/JobOrder")]
    // [SwaggerOperation(Summary = "Upserts a JobOrder.",
    // Description = "Upserts a JobOrder.",
    // Tags = new[] { "Job Orders" })]
    // [SwaggerOperationFilter(typeof(TokenKeyHeaderFilter))]
    // [SwaggerResponse(StatusCodes.Status200OK, $"Returns the Id of the joborder and a message.")]

    // In this example, the [Authorize] attribute is used to restrict access to the CreateJobOrder action method to users 
    // who are authenticated using the specified authentication scheme and belong to one of the specified roles (Admin, Staff, Respondent, ExternalAPIClient).
    // This means that only users who meet these criteria will be able to access the CreateJobOrder endpoint and perform the operation defined in that action method.

    // The [Authorize] attribute can be applied at the controller level to protect all actions within the controller, or at the action level to protect specific actions.

    //# But, is this a good practice to have [Authorize] attribute at the controller level or action level?
    // It depends on the specific requirements of your application and how you want to structure your authorization logic.
    // Applying the [Authorize] attribute at the controller level can be a good practice if you want to protect all actions within the controller 
    // with the same authorization requirements.
    // This can help to reduce code duplication and ensure consistency in your authorization logic across all actions in that controller.
    // However, if you have different authorization requirements for different actions within the same controller, it may be better to apply the [Authorize] 
    // attribute at the action level. This allows you to specify different roles or policies for each action, providing more 
    // granular control over access to specific endpoints.
    // Ultimately, the choice of where to apply the [Authorize] attribute should be guided by the specific needs of your application and 
    // how you want to manage your authorization logic for maintainability and clarity. 

    //# In the above example, why are they mentioning swagger attributes like [SwaggerOperation] and [SwaggerResponse] where swagger should be a 
    // generic documentation tool and need not get defined on each controller?
    // The Swagger attributes like [SwaggerOperation] and [SwaggerResponse] are used to provide additional metadata for API documentation generation.
    // While Swagger is a generic documentation tool, these attributes allow you to customize the documentation for specific endpoints, such as 
    // providing summaries, descriptions, tags, and response types for each action method.
    // By using these attributes, you can enhance the generated API documentation with more detailed information about each endpoint, making it 
    // easier for developers to understand the purpose and behavior of the API.
    // However, if you want to avoid adding these attributes to each controller or action method, you can also configure Swagger to use 
    // conventions or apply global filters to automatically generate documentation based on your API structure and conventions.
    // This way, you can maintain a cleaner controller code while still providing comprehensive API documentation through Swagger.  




    //? Is Authorize attribute a custom attribute?
    // No, the Authorize attribute is not a custom attribute. It is a built-in attribute provided by the ASP.NET Core framework for handling authorization in web applications.
    // The Authorize attribute is used to restrict access to controllers or action methods based on the user's authentication status or roles. It can be applied at the controller level to protect all actions within the controller, or at the action level to protect specific actions.
    // Example usage: [Authorize(Roles = "Admin")] would restrict access to users who are in the "Admin" role.

    //? Could you list some other built-in attributes in ASP.NET Core?
    // 1. [HttpPost]: Specifies that an action method should handle HTTP POST requests.
    // 2. [HttpPut]: Specifies that an action method should handle HTTP PUT requests.
    // 3. [HttpDelete]: Specifies that an action method should handle HTTP DELETE requests.
    // 4. [Route]: Defines a route template for a controller or action method.
    // 5. [FromBody]: Indicates that a parameter should be bound from the body of the HTTP request.
    // 6. [FromQuery]: Indicates that a parameter should be bound from the query string.
    // 7. [FromRoute]: Indicates that a parameter should be bound from the route data.
    // 8. [FromForm]: Indicates that a parameter should be bound from form data.
    // 9. [FromHeader]: Indicates that a parameter should be bound from a request header.
    // 10. [FromServices]: Indicates that a parameter should be bound from a service in the dependency injection container.
    // 11. [ValidateAntiForgeryToken]: Validates that a request includes a valid anti-forgery token to prevent cross-site request forgery (CSRF) attacks.
    // 12. [AllowAnonymous]: Allows anonymous access to a controller or action method, bypassing any authorization requirements.
    // 13. [Produces]: Specifies the type of response that an action method can produce.
    // 14. [Consumes]: Specifies the type of request body that an action method can consume.
    // 15. [ApiController]: Indicates that a class is an API controller and enables features such as automatic model validation and binding source inference.
    // 16. [NonAction]: Indicates that a method is not an action method and should not be callable as an endpoint.
    // 17. [Obsolete]: Marks a method or class as obsolete, indicating that it should not be used and may be removed in future versions.
    // 18. [ResponseCache]: Specifies the caching behavior for responses from an action method.
    // 19. [ProducesResponseType]: Specifies the type of response and status code that an action method can return.
    // 20. [ApiExplorerSettings]: Configures the behavior of API Explorer, which generates metadata for API documentation and client generation.



    public async Task<IActionResult> GetAllUsers()
    // Task<IActionResult> indicates that this action method is asynchronous and returns an IActionResult, 
    // which is a common return type for action methods in ASP.NET Core. 
    // Task indicates that the method is asynchronous and can be awaited, allowing for non-blocking operations 
    // when performing tasks such as database access or external API calls.

    //? Does just Task make it asynchronous or we need async keyword too?
    // Both Task and the async keyword are needed to make a method asynchronous in C#.
    // The async keyword is used to indicate that a method is asynchronous and can contain await expressions, while Task is the return
    // type that represents the asynchronous operation.
    // Example: public async Task<IActionResult> GetAllUsers() { ... }

    // IActionResult allows for flexibility in the type of response that can be returned, such as Ok, NotFound, BadRequest, etc.

    // This action method handles HTTP GET requests to the "/users" endpoint. It retrieves all users from the IUserService, 
    // converts them to response DTOs, and returns a standardized API response.
    {
      var users = await _userService.GetAllUsersAsync();
      // The GetAllUsersAsync method of the IUserService is called to retrieve a list of all users.
      // _userService is the instance of IUserService that was injected into the UserController through constructor injection.

      var response = users.ToUserResponses();
      // The retrieved users are then converted to response DTOs using the ToUserResponses extension method.
      // This method likely maps the User domain models to UserResponse DTOs, which are designed for returning data to the client in a standardized format.

      return ApiResponseHelper.Success(response);
      // Finally, the ApiResponseHelper.Success helper method is called to return a standardized API response with the list of user responses.

      //? Is this a standard way to return response object or should we directly return Ok(response) or something like that?
      // Using a helper method like ApiResponseHelper.Success can be a good practice for returning standardized API responses, 
      // as it allows you to maintain consistency in the structure of your responses across different endpoints.
      // However, you can also directly return Ok(response) or other IActionResult types if you prefer a more straightforward approach.

      //? Is there a better way to maintain standardized responses without using a helper method?
      // Another way to maintain standardized responses without using a helper method is to create a custom base controller (e.g., ApiControllerBase) 
      // that includes methods for generating standardized responses.
      // For example, you could define a method like ApiResponse in the base controller that takes care of formatting the response in a consistent way.
      // Then, in your UserController, you would inherit from ApiControllerBase and use the ApiResponse method to return responses in a standardized format.
      // This approach allows you to centralize the response formatting logic in the base controller, reducing code duplication and ensuring consistency 
      // across all your API controllers without needing a separate helper class. 

    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
      var user = await _userService.GetUserByIdAsync(id);
      if (user == null) return ApiResponseHelper.NotFound();

      var response = user.ToUserResponse();
      return ApiResponseHelper.Success(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
      // Validate request
      var validationResult = ValidationHelper.ValidateUser(request.ToUser());
      if (!validationResult.IsValid)
        return ApiResponseHelper.BadRequest(validationResult.ErrorMessage);

      var user = request.ToUser();
      await _userService.CreateUserAsync(user);

      var response = user.ToUserResponse();
      return ApiResponseHelper.Created(response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest request)
    {
      var existingUser = await _userService.GetUserByIdAsync(id);
      if (existingUser == null) return ApiResponseHelper.NotFound();

      //# Validate request
      // Is validation done in the controller or in the service layer?
      // Validation can be done in both the controller and service layer, depending on the specific requirements and design of the application.
      // In the controller, you can perform basic validation on the incoming request data, such as checking for required fields, data formats, 
      // and simple business rules. This helps to ensure that the data is in a valid state before it reaches the service layer.
      // In the service layer, you can perform more complex validation that may involve business logic, interactions with other services, 
      // or database checks. This allows you to enforce more specific rules and constraints that are relevant to the application's domain.
      // Ultimately, the choice of where to perform validation depends on the specific needs of your application and how you want to structure 
      // your code for maintainability and separation of concerns. It's common to see a combination of both controller-level validation for 
      // basic checks and service-level validation for more complex rules.  

      // I would prefer all validations to be done in the service layer to keep the controller thin and focused on handling HTTP requests and responses. 

      //# Is there an common standard for validation? Could you help with an example of how to implement validation in the service layer?
      // A common standard for validation in the service layer is to use a validation framework such as FluentValidation or Data Annotations. 
      // These frameworks allow you to define validation rules for your models and automatically validate them when needed.

      //#FluentValidation: 
      // Example using FluentValidation:  
      // First, you would define a validator for your User model:
      // public class UserValidator : AbstractValidator<User>
      // {
      //     public UserValidator()
      //     {
      //         RuleFor(user => user.FirstName).NotEmpty().WithMessage("First name is required.");
      //         RuleFor(user => user.LastName).NotEmpty().WithMessage("Last name is required.");
      //         RuleFor(user => user.Email).NotEmpty().EmailAddress().WithMessage("A valid email is required.");
      //     }
      // }
      // Then, in your service layer, you would use the validator to validate the User object before performing any operations:
      // public async Task<User> CreateUserAsync(User user) 
      // {
      //     var validator = new UserValidator();
      //     var validationResult = validator.Validate(user);
      //     if (!validationResult.IsValid)
      //     {
      //         throw new ValidationException(validationResult.Errors);
      //     }
      //     // Proceed with creating the user if validation is successful  
      //     // ... (code to create user)
      // }
      // In this example, the UserValidator defines the validation rules for the User model, and  
      // the CreateUserAsync method in the service layer uses the validator to validate the User object before proceeding with the creation logic.


      //# Data annotations:
      // You can also use data annotations for validation in the service layer.

      // Is the Data annotation and RequestDTO same, because we check for the correct request in Request DTO as well?
      // Data annotations and RequestDTOs serve different purposes in the context of validation.
      // Data annotations are attributes that you can apply to properties of your model classes to specify validation rules, 
      // such as [Required], [MaxLength], [EmailAddress], etc.  
      // RequestDTOs are used to define the structure of the data that is expected in a request, and can also include validation 
      // attributes to ensure the data is valid before it reaches the service layer.

      // But, we can mention data annotations [Required], [MaxLength], [EmailAddress], etc.  in the RequestDTO to validate the incoming request data before it reaches the service layer. 
      // This allows us to catch validation errors early and return appropriate responses to the client without having to execute any business logic in the service layer.
      // However, we can also perform additional validation in the service layer that may involve more complex business rules or interactions with other services or the database.
      // So, while there may be some overlap in the validation rules defined in the RequestDTO


      // Example:
      // public class User
      // {
      //     [Required(ErrorMessage = "First name is required.")]
      //     public string FirstName { get; set; }
      //     [Required(ErrorMessage = "Last name is required.")]
      //     public string LastName { get; set; }
      //     [Required(ErrorMessage = "Email is required.")]
      //     [EmailAddress(ErrorMessage = "A valid email is required.")]
      //     public string Email { get; set; }
      // }
      // Then, in your service layer, you can validate the User object using the Validator class
      // public async Task<User> CreateUserAsync(User user)
      // {
      //     var validationContext = new ValidationContext(user);
      //     var validationResults = new List<ValidationResult>();
      //     bool isValid = Validator.TryValidateObject(user, validationContext, validationResults, true);
      //     if (!isValid)
      //     {
      //         var errorMessages = validationResults.Select(vr => vr.ErrorMessage).ToList();
      //         throw new ValidationException(string.Join("; ", errorMessages));
      //     }
      //     // Proceed with creating the user if validation is successful
      //     // ... (code to create user)
      // }
      // In this example, the User class has data annotations that define the validation rules for its properties, and the CreateUserAsync
      //  method in the service layer uses the Validator class to validate the User object before proceeding with the creation logic. 
      // If validation fails, a ValidationException is thrown with the error messages.
      // This approach allows you to keep validation logic within the service layer while still leveraging the benefits of data 
      // annotations for defining validation rules directly on the model properties. 
      // It also helps to maintain separation of concerns, as the controller remains focused on handling HTTP requests and responses, 
      // while the service layer is responsible for business logic and validation. 


      var tempUser = request.ToUser();
      tempUser.Id = id; // Set ID for validation
      var validationResult = ValidationHelper.ValidateUser(tempUser);
      if (!validationResult.IsValid)
        return ApiResponseHelper.BadRequest(validationResult.ErrorMessage);

      existingUser.UpdateFromRequest(request);
      var updated = await _userService.UpdateUserAsync(id, existingUser);
      if (updated == null) return ApiResponseHelper.InternalServerError();

      var response = updated.ToUserResponse();
      return ApiResponseHelper.Success(response, "User updated successfully");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
      var deleted = await _userService.DeleteUserAsync(id);
      if (!deleted) return ApiResponseHelper.NotFound();

      return ApiResponseHelper.Success(message: $"User {id} deleted successfully");
    }
  }
}




// #region and #endregion are preprocessor directives used in C# and other languages (like Apex or C++)
//  to define a block of code that can be expanded or collapsed within an Integrated Development Environment (IDE) like Visual Studio or VS Code



// What are the most used built-in classes in C#?
// 1. System.String: Represents a sequence of characters and provides methods for string manipulation.
// 2. System.Int32: Represents a 32-bit signed integer and provides methods for numeric operations.
// 3. System.Collections.Generic.List<T>: Represents a strongly typed list of objects that can be accessed by index and provides methods for adding, removing, and searching elements.
// 4. System.DateTime: Represents an instant in time and provides methods for date and time manipulation.
// 5. System.IO.File: Provides static methods for creating, copying, deleting, moving, and opening files, and helps in the creation of FileStream objects.
// 6. System.IO.Stream: Represents a sequence of bytes and provides methods for reading and writing data to streams.
// 7. System.Threading.Tasks.Task: Represents an asynchronous operation and provides methods for managing and awaiting tasks.
// 8. System.Exception: Represents errors that occur during application execution and provides methods for handling exceptions.
// 9. System.Console: Provides methods for input and output to the console.
// 10. System.Linq.Enumerable: Provides static methods for querying objects that implement IEnumerable<T> and supports LINQ queries.  
// 11. System.Collections.Generic.Dictionary<TKey, TValue>: Represents a collection of key-value pairs that are organized based on the hash code of the key and provides methods for adding, removing, and searching elements.
// 12. System.Collections.Generic.HashSet<T>: Represents a set of values and provides methods for adding, removing, and searching elements while ensuring that all elements are unique.
// 13. System.Collections.Generic.Queue<T>: Represents a first-in, first-out collection of objects and provides methods for adding, removing, and peeking elements.
// 14. System.Collections.Generic.Stack<T>: Represents a last-in, first-out collection of objects and provides methods for adding, removing, and peeking elements.
// 15. System.Collections.Generic.LinkedList<T>: Represents a doubly linked list and provides methods for adding, removing, and searching elements in the list.
// 16. System.Collections.Generic.SortedList<TKey, TValue>: Represents a collection of key-value pairs that are sorted by the keys and provides methods for adding, removing, and searching elements.
// 17. System.Collections.Generic.SortedSet<T>: Represents a collection of values that are sorted and provides methods for adding, removing, and searching elements while ensuring that all elements are unique.
// 18. System.Collections.Generic.SortedDictionary<TKey, TValue>: Represents a collection of key-value pairs that are sorted by the keys and provides methods for adding, removing, and searching elements.
// 19. System.Collections.Generic.SortedSet<T>: Represents a collection of values that are sorted and provides methods for adding, removing, and searching elements while ensuring that all elements are unique.
// 20. System.Collections.Generic.SortedDictionary<TKey, TValue>: Represents a collection of key-value pairs that are sorted by the keys and provides methods for adding, removing, and searching elements.   


// Built-in classes .net core framework which are commonly used in web applications include:
// 1. System.Net.Http.HttpClient: Used for sending HTTP requests and receiving HTTP responses from a resource identified by a URI.
// 2. System.Text.Json.JsonSerializer: Provides functionality to serialize objects to JSON and deserialize JSON to objects.
// 3. System.ComponentModel.DataAnnotations: Contains attributes that are used to define metadata for data models, such as validation rules and display formats.
// 4. System.IO.File: Provides static methods for creating, copying, deleting, moving, and opening files, and helps in the creation of FileStream objects.
// 5. System.IO.Stream: Represents a sequence of bytes and provides methods for reading and writing data to streams.
// 6. System.Threading.Tasks.Task: Represents an asynchronous operation and provides methods for managing and awaiting tasks.
// 7. System.Exception: Represents errors that occur during application execution and provides methods for handling exceptions.
// 8. System.Console: Provides methods for input and output to the console.
// 9. System.Linq.Enumerable: Provides static methods for querying objects that implement IEnumerable<T> and supports LINQ queries.
// 10. System.Collections.Generic.List<T>: Represents a strongly typed list of objects that can be accessed by index and provides methods for adding, removing, and searching elements.

// Built-in classes like ControllerBase, IActionResult, and attributes like HttpGet, HttpPost, etc. are part of the 
// ASP.NET Core framework and are commonly used in web applications to handle HTTP requests and generate responses. 
// These classes and attributes provide a structured way to define API endpoints, handle routing, and manage the flow of data between 
// the client and server in a web application.

// Common Return type classes follow:
// 2. System.Web.Mvc.IActionResult: An interface that represents the result of an action method, allowing for flexibility in the type of response that can be returned.
// 3. System.Web.Mvc.OkObjectResult: Represents an HTTP 200 OK response with a value.
// 4. System.Web.Mvc.CreatedResult: Represents an HTTP 201 Created response with a value and a location header.
// 5. System.Web.Mvc.BadRequestObjectResult: Represents an HTTP 400 Bad Request response with a value.
// 6. System.Web.Mvc.NotFoundResult: Represents an HTTP 404 Not Found response.
// 7. System.Web.Mvc.InternalServerErrorResult: Represents an HTTP 500 Internal Server Error response.  
// 8. System.Web.Mvc.JsonResult: Represents a JSON response.
// 9. System.Web.Mvc.FileResult: Represents a file response.  
// 10. System.Web.Mvc.RedirectResult: Represents a redirection response to a specified URL.
// 11. System.Web.Mvc.RedirectToActionResult: Represents a redirection response to a specified action and controller.
// 12. System.Web.Mvc.RedirectToRouteResult: Represents a redirection response to a specified route.
// 13. System.Web.Mvc.ContentResult: Represents a response that returns content of a specified type.
// 14. System.Web.Mvc.EmptyResult: Represents a response that does not return any content.
// 15. System.Web.Mvc.FileContentResult: Represents a response that returns a file from a byte array.
// 16. System.Web.Mvc.FileStreamResult: Represents a response that returns a file from a stream.
// 17. System.Web.Mvc.FilePathResult: Represents a response that returns a file from a specified path.  


// commonly used built-in Classes

// 1. System.Web.Mvc.ControllerBase: The base class for an MVC controller without view support.



