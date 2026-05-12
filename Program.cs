using ProductApp.Data;
// This is required to use AppDbContext when configuring the DbContext with SQL Server in the dependency injection container.
// using directives to include necessary namespaces for the application.


using Microsoft.EntityFrameworkCore;
// This is required to use the UseSqlServer() extension method when configuring the 
// DbContext with SQL Server in the dependency injection container.

using ProductApp.Services.Interfaces;
using ProductApp.Services.Implementations;
// This is required to register the IProductService and IUserService interfaces and their implementations 
// (ProductService and UserService) in the dependency injection container.
// This allows us to inject these services into our controllers and other parts of the application where needed.

using ProductApp.Middleware;
// This is required to use the RequestLoggingMiddleware in the middleware pipeline configuration 
// (app.UseMiddleware<RequestLoggingMiddleware>()).

//? How to inject custom middleware in ASP.NET Core? 
// You can create a custom middleware class (like RequestLoggingMiddleware)
// that implements the IMiddleware interface or has an Invoke/InvokeAsync method.
// Then, you can register it in the dependency injection container (if it implements IMiddleware) and add it to the middleware
// pipeline using app.UseMiddleware<YourMiddleware>() in Program.cs.    


//? Inject multiple middlewares in ASP.NET Core?
// You can inject multiple middlewares in the ASP.NET Core middleware pipeline by calling app.UseMiddleware<YourMiddleware>()
// for each middleware you want to add.  
// The order in which you add the middlewares matters, as it determines how requests are processed and how responses are generated.
// For example, if you have two custom middlewares, RequestLoggingMiddleware and AuthenticationMiddleware, 
// you can add them to the pipeline like this:  
// app.UseMiddleware<RequestLoggingMiddleware>();
// app.UseMiddleware<AuthenticationMiddleware>();


var builder = WebApplication.CreateBuilder(args);
// This line initializes a new instance of the WebApplicationBuilder class, which is used to configure and build the web application.
// The builder provides access to services, configuration, and other settings needed to set up the application. 
// It takes command-line arguments (args) which can be used for configuration purposes 
// (e.g., setting environment variables, specifying URLs, etc.). 
// The builder is the starting point for configuring the application's services and middleware before building and running the app.

builder.Services.AddEndpointsApiExplorer();
// This is required for minimal APIs to generate OpenAPI documentation. 
// It registers the services needed to discover and describe your API endpoints, which is essential for Swagger to 
// generate accurate documentation. Even if you're using controllers, 
// it's a good idea to include this to ensure all endpoints are documented correctly.

builder.Services.AddSwaggerGen();
// Add swagger for API documentation and testing
// The AddSwaggerGen() method registers the services required to generate Swagger documentation for your API.
// dotnet add package Swashbuckle.AspNetCore
// Swashbuckle.AspNetCore package (which provides AddSwaggerGen() and UseSwaggerUI()) is missing — 
// only Microsoft.AspNetCore.OpenApi is installed.



// Package installed. Now run the app and open:

// http://localhost:5186/swagger

// That's it — everything in Program.cs was already correctly set up (AddSwaggerGen(), UseSwagger(), UseSwaggerUI()). 
// The only missing piece was the Swashbuckle.AspNetCore NuGet package.



builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add dbContext to the service container
// This allows us to inject AppDbContext into our controllers and services using dependency injection.
// The UseSqlServer() method configures the DbContext to use SQL Server as the database provider, 
// and it retrieves the connection string from the app's configuration (e.g., appsettings.json) using the key "DefaultConnection".


// Add Controllers
builder.Services.AddControllers();
// This registers the services required for using controllers in the application.
// It allows you to define API endpoints using controller classes decorated with attributes like [ApiController], [Route], [HttpGet], etc.
// This is essential for building a Web API using the controller-based approach in ASP.NET Core.


// Add Services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUserService, UserService>();
// AddScoped → A new instance of the service will be created for each HTTP request and shared within that request.


// ? What this line does:
// It tells ASP.NET's built-in IoC container: "Whenever something asks for IProductService, create and give it a ProductService."

// ? What is Dependency Injection (DI)?
// Instead of a class creating its own dependencies, they are provided from outside. 
// Compared to tightly coupled code where a class creates its own dependencies, DI promotes loose coupling and makes
// it easier to test your application by allowing you to inject mock implementations of your services during testing.  
// ASP.NET sees IProductService in the constructor and automatically resolves and injects a ProductService instance.
// Why use the Interface (IProductService) instead of ProductService directly?
// The controller depends on the abstraction, not the implementation. This means you can swap implementations without touching the controller:

// ? What does AddScoped mean?
// It controls the lifetime of the created instance:
// Method	Lifetime	When to use:

// AddSingleton	One instance for the entire app	Stateless shared services
// Example: a configuration service that reads settings once and provides them throughout the app. 
// i.e. a service that provides application-wide settings or utilities that do not maintain any state and can be shared across all requests.

// AddScoped	One instance per HTTP request	DB-related services (like yours)
// AddTransient	New instance every time it's requested	Lightweight, stateless utilities
// Example: a service that generates random values or performs simple calculations without maintaining any state.
// i.e. a service that performs simple, stateless operations that do not require shared state and can be created and disposed of quickly.


// AddScoped is the right choice here because AppDbContext (EF Core) is also scoped 
//— one DB context per request keeps DB operations consistent within a single request.

// The full flow:
// HTTP Request
//     ↓
// ProductsController needs IProductService
//     ↓
// ASP.NET DI container creates ProductService
//     ↓
// ProductService needs AppDbContext
//     ↓
// DI creates AppDbContext (also scoped)
//     ↓
// Request completes → both are disposed

// What does this enable us to do? 
// It allows us to inject IProductService into our controllers and other services without having to manually create instances of ProductService or AppDbContext.
// How does this help in catering to multiple requests?
// Each HTTP request gets its own instance of ProductService and AppDbContext, ensuring that data operations are isolated per request
// and preventing issues with shared state across requests.


//
var app = builder.Build();
// This builds the WebApplication based on the configuration defined in the builder.
// After this line, the app variable contains the configured application, and you can start adding middleware

// how to read environment variables in ASP.NET Core? Use builder.Configuration.GetValue<string>("MyVariable") 
// or builder.Configuration.GetSection("MySection").GetValue<string>("MyVariable")
// Example: var myVariable = builder.Configuration.GetValue<string>("MyVariable");
// You can also use Environment.GetEnvironmentVariable("MyVariable") to read environment variables directly from the system, 
// but using builder.Configuration is recommended as it integrates with the app's configuration system and supports various configuration sources 
// (like appsettings.json, environment variables, user secrets, etc.).
// Environment.IsEnvironment("MyEnvironment") to conditionally enable features based on environment settings.

// app.Environment provides information about the hosting environment (Development, Staging, Production).
// This allows you to conditionally enable features (like Swagger) only in development, which is a common best practice for security reasons.

if (app.Environment.IsDevelopment()) // Only enable Swagger in development environment for security reasons
// In production, you typically don't want to expose detailed API documentation publicly.
// You can configure this further by using environment variables or appsettings.json to control when Swagger is enabled.

{
  app.UseSwagger(); // Generate swagger.json at runtime
  app.UseSwaggerUI(); // Serve the Swagger UI at /swagger, which reads swagger.json and renders the interactive API docs
}

app.UseHttpsRedirection();
// Redirect HTTP requests to HTTPS for better security

app.UseAuthorization();
// Enable authorization middleware (if you have any [Authorize] attributes in your controllers)

// Add custom middleware to log requests and responses
app.UseMiddleware<RequestLoggingMiddleware>();

app.MapControllers();
// Map controller routes (e.g., /api/product) to the corresponding controller actions based on attributes like [HttpGet], [HttpPost], etc.

app.Run();
// Start the application and listen for incoming HTTP requests. 
// The app will run until you stop it (e.g., Ctrl+C in the terminal).





/* ______________________________________________________________________________________________________________ */


//? Read Properties folder launchSettings.json to see the configured URLs and launch profiles. 
// By default, ASP.NET Core apps have two profiles: "http" and "https".
// The app runs on different ports. Use these URLs:

// HTTP: http://localhost:5186
// HTTPS: https://localhost:7199
// And since this is a Web API (no UI), go directly to an endpoint, e.g.:

// http://localhost:5186/api/product

// Note: dotnet run uses the http profile by default. To use HTTPS, run dotnet run --launch-profile https.


//? Dotnet run with launch profile:

// dotnet run --launch-profile dev
// run the app in development mode with the dev profile (which sets ASPNETCORE_ENVIRONMENT to Development):

// you can define environment variables in the launchSettings.json profiles, 
// and they will be set when you run the app using that profile.


//? Migrations are a way to keep your database schema in sync with your EF Core model classes.
// When you create or modify your model classes (like Product or User), you need to create a migration to update the database schema accordingly.
// The migration captures the changes you made to your model and generates code to apply those changes to the database.
// To create a migration, you use the following command in the terminal:

// Run the EF Core migration commands to create the database schema based on your models:
//    - dotnet ef migrations add InitialCreate
//    - dotnet ef database update
// The first command creates a new migration named "InitialCreate" that captures the current state of your model classes.
// The second command applies the migration to the database, creating the necessary tables and columns based on

// Why we need to remove old migration files from the Migrations folder?
// If you have already applied migrations to your database and then delete the migration files from the Migrations folder, you will lose the history of those migrations.
// This can lead to issues when trying to apply new migrations, as EF Core relies on the migration history to determine what changes need to be applied to the database.
// If you want to start fresh with a new migration history, you can delete the existing migration files and then 
// create a new initial migration that captures the current state of your model classes.  

// dotnet ef remove migration InitialCreate
// This command removes the last migration (InitialCreate) from the Migrations folder and updates the migration history.

// Why to remove the Migrations folder from the project?
// The Migrations folder contains the generated migration files that represent changes to your database schema.
// In a production application, you typically don't want to include these generated files in your source control repository,
// as they can be regenerated as needed and may contain environment-specific details.





//? Environment Variables and User Secrets:
// In ASP.NET Core, you can manage sensitive configuration data (like API keys, connection strings, etc.) 
// using environment variables or the User Secrets feature during development.

// appsettings.json is great for non-sensitive configuration, 
// but you should avoid putting secrets there, especially if your code is in a public repository.

// appsettings.development.json is used for development-specific settings, but it’s still not ideal for secrets.

//? User Secrets 
// is a feature that allows you to store sensitive data in a secure way during development without hardcoding it in
// your code or configuration files.
// You can initialize User Secrets in your project and then set secrets using the command line. 
// For example, to set an API key secret, you would run:
// dotnet user-secrets init
// dotnet user-secrets set "ApiKey" "secret-value"

// Wht's the best practice to store db connection string as it always has login credentials in it?
// The best practice is to store the database connection string in a secure location, such as environment 
// variables or using the User Secrets feature during development.
// In production, you can use environment variables or a secure secrets management service (like Azure Key Vault, AWS Secrets Manager, etc.) 
// to store and access your connection strings securely.
// This way, you avoid hardcoding sensitive information in your code or configuration files, and you can manage and rotate secrets without 
// needing to change your application code.   




//? Logging:
// ASP.NET Core has built-in logging support. You can configure logging in appsettings.json and use the ILogger<T> 
// interface to log messages in your controllers and services.
// For example, you can inject ILogger<ProductService> into your ProductService and use it to log information, warnings, errors, etc.
// This helps you monitor the behavior of your application and troubleshoot issues in production.  

// How to enable logging for whole application?
// You can configure logging in the appsettings.json file under the "Logging" section.
// For example, to enable debug logging for the entire application, you can set the "Default" log level to "Debug":
// "Logging": {
//   "LogLevel": {
//     "Default": "Debug",
//     "Microsoft": "Information",
//     "System": "Information"
//   }
// }

// This configuration will enable debug logging for all categories, while keeping Microsoft and System logs at the Information level to reduce noise from framework logs.
// Where can I see the logs?
// By default, ASP.NET Core logs to the console, so you can see the logs in the terminal where you run your application.
// You can also configure additional logging providers (like file logging, Azure Application Insights, etc.) to store logs in different locations based on your needs.

// ? Best practices for logging in ASP.NET Core:
// 1. Use structured logging with ILogger<T> to capture contextual information.
// 2. Avoid logging sensitive information (like passwords, connection strings, etc.).
// 3. Use appropriate log levels (Trace, Debug, Information, Warning, Error, Critical) to categorize log messages.
// 4. Consider using correlation IDs to trace requests across distributed systems.





//? Middleware:
// Middleware are components that form the request processing pipeline in ASP.NET Core.
// They can inspect, modify, or short-circuit HTTP requests and responses.
// You can create custom middleware (like RequestLoggingMiddleware) to perform tasks such as logging, authentication, error handling, etc.
// Middleware is added to the pipeline using app.UseMiddleware<YourMiddleware>() in Program.cs, and the order of 
// middleware matters because it determines how requests are processed and how responses are generated. 
// examples: app.UseAuthentication(), app.UseAuthorization(), app.UseStaticFiles(), app.UseRouting(), etc.

//? Log all requests and responses in ASP.NET Core?
// You can create a custom middleware (like RequestLoggingMiddleware) that logs the details of incoming HTTP requests and outgoing HTTP responses.
// In this middleware, you can use the ILogger<T> to log the request method, URL, headers, body, and the response status code, headers, and body.
// Then, you can add this middleware to the pipeline in Program.cs using app.UseMiddleware<RequestLoggingMiddleware>() to ensure 
//that all requests and responses are logged as they pass through the middleware pipeline. 



//? Dependency Injection (DI):
// What is Dependency Injection (DI)?
// Dependency Injection is a design pattern that allows you to remove hard-coded dependencies between classes and make them more flexible and testable.
// Instead of a class creating its own dependencies, they are provided from outside (injected).
// In ASP.NET Core, DI is built-in and widely used to manage the dependencies of your classes, such as controllers, services, and DbContext. 
// You can register services in the DI container and then inject them where needed, promoting loose coupling and easier testing.  

// Constructor injection

// What are dependencies of classes? why does the class need it?
// Dependencies are the other classes or services that a class relies on to perform its functions.
// For example, a ProductService might depend on an AppDbContext to access the database and perform CRUD operations on products and
// a ProductController might depend on IProductService to handle business logic related to products.

// By using DI, you can inject the AppDbContext into the ProductService instead of having the ProductService create its own instance of AppDbContext.
// This allows for better separation of concerns, easier testing (you can inject mock dependencies), and more flexible code
// (you can swap implementations without changing the dependent class).  



// ASP.NET Core has built-in support for dependency injection, which allows you to manage the dependencies of your classes
// in a clean and maintainable way.
// You can register services (like IProductService and IUserService) in the DI container using builder.Services.AddScoped(),
// AddSingleton(), or AddTransient() depending on the desired lifetime of the service.
// This promotes loose coupling and makes it easier to test your application by allowing you to inject mock implementations 
// of your services during testing.    

// Hypothetically, if we didn't have DI, we would have to manually create instances of our services and their dependencies in our controllers,
// which would lead to tightly coupled code and make it difficult to manage dependencies, especially as the application grows in complexity. 
// With DI, we can simply declare our dependencies in the constructor of our controllers or services, and the framework will 
// take care of providing the correct instances when needed.


//? What are DLLs? How do we generate them and what are they used for?
// DLL stands for Dynamic Link Library. It is a file format used for holding multiple codes and procedures for Windows programs. 
// In the context of .NET applications, a DLL is a compiled assembly that contains code (classes, methods, etc.) that can be reused by other applications.
// When you build a .NET application, the source code is compiled into one or more DLL files (assemblies) that contain the executable code.
// These DLLs can be referenced by other projects or applications, allowing for code reuse and modular design.
// For example, if you have a class library project that contains shared logic (like services or utilities), it will be compiled into a DLL
// that can be referenced by your main application project.

//? How do we create the build? And, does the build contain DLLs?
// When you run dotnet build or dotnet run, the .NET CLI compiles your source code into assemblies (DLL files) that contain the executable code.
// The build process generates these DLLs in the output directory (e.g., bin/Debug/net6.0/), and these DLLs are 
// what the .NET runtime executes when you run your application.
// The build process also includes any necessary dependencies (like NuGet packages) in the output directory, so when you run the application,
// it has everything it needs to execute.

//? How to deploy .Net app on IIS server?
// To deploy a .NET application on IIS, you typically follow these steps:
// 1. Publish your application using dotnet publish, which compiles the application and prepares it for deployment 
// and creates you a folder with all the necessary files, including the DLLs. 

// 2. Copy the published files (including the DLLs) to the IIS server.
// Do we need to copy the source code to the IIS server? No, you only need to copy the published files (including the DLLs) to the IIS server.

// 3. Set up an IIS website or application pointing to the directory where you copied the published files.
// 4. Configure the application pool to use the correct .NET runtime version.
// 5. Ensure that the necessary permissions are set for the application to run (e.g., read/write permissions for logs, etc.).
// 6. Start the IIS website and test the deployment by accessing the application through the configured URL.

// How do we create the webConfig and why is it needed?
// The web.config file is an XML file used to configure settings for an ASP.NET application when hosted on IIS.
// It is needed to specify how IIS should handle requests for the application, including settings for the .NET runtime, security,
// and other application-specific configurations.

// Does the created webconfig file already have right set up done based on the defined route, requests and other items??  


// When you publish a .NET application for IIS, the web.config file is automatically generated in the publish output directory.
// This file contains the necessary configuration for IIS to run your application correctly, such as the handler mappings for ASP.NET Core,
// environment variables, and other settings required for the application to function properly on IIS.

// Are there any other files which we would need for deployment?






// In modern .NET applications(.NET 6 and later), the Startup.cs and Program.cs files have been merged into a single Program.cs file.
// In older versions (.NET 5 and earlier), they were two separate files that worked together to launch your application.
// The Core Difference:
// Program.cs (The Entry Point): 
// This is the "main door" of your application.In older apps, its only job was to create a "Host" (the web server environment) and then call the Startup class.

// Startup.cs(The Configuration): This was the "interior designer" file where you decided how the app actually behaved. 
// It contained two critical methods:ConfigureServices: Where you registered services for Dependency Injection (DI).
// Configure: Where you set up the HTTP request pipeline (middleware).

//? What changed in .NET 6, 7, 8, and 9? 
// Microsoft introduced the Minimal Hosting Model, which removes Startup.cs by default to reduce boilerplate code. Now, everything happens directly in Program.cs

// 1. Builder Creation: 
// var builder = WebApplication.CreateBuilder(args); 
// 2. Service Registration: (Equivalent to ConfigureServices) You add services directly to 
// builder.Services.App 
// 3. Building: var app = builder.Build(); 
// 4. Middleware Setup: 
// (Equivalent to Configure) You add middleware directly to the app object 
// (e.g., app.UseStaticFiles()).Execution: app.Run();





//? does .net also have something similar package.json of project to check installed packages and their versions?
// In modern .NET, the equivalent of a package.json file is the .csproj (C# Project) file.Instead of a separate JSON file, 
// .NET stores its package dependencies directly within this XML-based project file using <PackageReference> elements.
//





// Program.cs
//    ↓
// Builds:
//    - DI Container
//    - Middleware Pipeline
//    - Config
//    ↓
// Request comes in
//    ↓
// Middleware pipeline executes
//    ↓
// Routing finds Controller
//    ↓
// DI creates Controller (and dependencies)
//    ↓
// Controller calls Service
//    ↓
// Service uses DbContext
//    ↓
// Response returned





// In a .NET web app:

// - Most parts of the application are implemented as classes (controllers, services, middleware, etc.), but .NET also supports other 
// constructs like interfaces, records, and structs.

// - Program.cs is the entry point (with implicit Main method in modern .NET) and is responsible for:
//     - configuring services (DI container)
//     - setting up middleware pipeline
//     - starting the application

// - Dependency Injection is central:
//     - Only registered services are created automatically
//     - Dependencies are resolved recursively

// - Namespaces are used for logical grouping and organization, not for wiring dependencies

// - Not all classes need to be registered in DI:
//     - Services and infrastructure → registered
//     - DTOs, entities → not registered
// Won't we need to register entities in DI container? No, entities (like Product, User) are typically not registered in 
// the DI container because they are not services that need to be injected. 
// They are usually created and managed by the DbContext (EF Core) when performing database operations

// - Framework libraries (like System, EntityFrameworkCore) are namespaces that provide reusable classes

// - .NET follows conventions (like Controller naming, attributes), but does not strictly enforce architecture






//TODO Questions to go deep
// How DI container works internally
// Request lifecycle deep dive (thread, async, pipeline)
// Scoped vs Singleton pitfalls (very important in real apps)
// Threading model(async/await + thread pool)
// How Kestrel handles concurrent requests
// How EF manages connection pooling

//TODO To understand:
// ✔ How .NET handles requests
// ✔ How DI creates objects
// ✔ How middleware controls flow
// ✔ How EF translates LINQ → SQL
// ✔ How to optimize DB queries


// I’ll walk you through a real request lifecycle, including:

// object creation
// memory scope
// DI resolution
// middleware flow

// We’ll use your current app:

// GET /tasks
// 🧠 0. Before Any Request (App Startup Phase)

// When app starts (dotnet run):

// Program.cs executes
// var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddScoped<ITaskService, TaskService>();
// builder.Services.AddDbContext<AppDbContext>(...);

// var app = builder.Build();

// app.UseMiddleware<ExceptionMiddleware>();
// app.UseAuthentication();
// app.UseAuthorization();
// app.MapControllers();

// app.Run();
// 🧠 What gets created in memory?
// [ GLOBAL APP MEMORY ]
// ----------------------------------

// ✔ DI Container (ServiceProvider)
//    - knows:
//      ITaskService → TaskService
//      AppDbContext → how to create

// ✔ Middleware pipeline (ordered list)
//    1. ExceptionMiddleware
//    2. Authentication
//    3. Authorization
//    4. Endpoint routing

// ✔ Config (appsettings.json)

// ✔ Server (Kestrel)
//   - listens on http://localhost:5186
// What does kestrel do? It’s the web server that handles incoming HTTP requests and sends responses back to clients. 
// Does it come with .NET? Yes, Kestrel is the default cross-platform web server included with ASP.NET Core.

// ----------------------------------

// ⚠️ Important:

// NO controllers, NO services, NO DbContext created yet


// 🚀 1. Request Comes In
// Client → GET /tasks

// 🧠 What is created?
// [ REQUEST SCOPE CREATED ]

// ✔ HttpContext (VERY IMPORTANT)
//    - Request
//    - Response
//    - User
//    - Items (per-request storage)

// ✔ IServiceScope (DI scope)
//    - Scoped services will live here

// ----------------------------------

// 👉 This is per request memory

// 🔁 2. Middleware Pipeline Starts

// 🧱 Step 1: Exception Middleware
// await _next(context);

// Memory:

// ✔ ExceptionMiddleware instance
//    (usually singleton)

// ✔ Uses HttpContext (request-specific)
// 🧱 Step 2: Authentication Middleware
// Reads JWT from header

// Updates:

// HttpContext.User = ClaimsPrincipal
// 🧱 Step 3: Authorization Middleware

// Checks:
// Does user have access?

// If not:
// Short-circuit → return 401

// 🎯 3. Routing → Controller Selection
// Route: GET /tasks
// → TaskController.GetTasks()
// 🧠 4. Controller Creation (DI kicks in)

// Now DI container is used.

// Step-by-step object creation:
// 1️⃣ Create Controller
// public TaskController(ITaskService taskService)

// DI sees:

// Needs ITaskService
// 2️⃣ Resolve ITaskService

// From registration:

// AddScoped<ITaskService, TaskService>()

// So:

// Create TaskService instance (SCOPED)
// 3️⃣ Create TaskService
// public TaskService(AppDbContext context)

// DI sees:

// Needs AppDbContext
// 4️⃣ Create DbContext
// AddDbContext<AppDbContext>()

// So:

// Create AppDbContext (SCOPED)
//    + open DB connection (lazy or on first use)

// 🧠 Memory Now Looks Like:
// [ REQUEST SCOPE MEMORY ]
// ----------------------------------

// HttpContext

// TaskController instance
// TaskService instance
// AppDbContext instance

// ----------------------------------

// 👉 All are SCOPED → same instance reused in this request

// ⚡ 5. Controller Action Executes
// var tasks = await _taskService.GetTasks();
// Inside Service:
// _context.Tasks.ToListAsync();

// 🧠 What happens internally
// 1. LINQ expression built
// 2. EF converts → SQL
// 3. SQL sent to DB
// 4. DB returns rows
// 5. Rows mapped → TaskItem objects

// 🧠 Memory now:
// List<TaskItem> created in memory

// 🔁 6. Response Creation

// Controller:
// return Ok(tasks);

// Framework does:
// Object → JSON serialization

// Memory:
// JSON string created

// 🔁 7. Response Goes Back Through Middleware

// Pipeline reverses:

// Controller
//   ↑
// Authorization
//   ↑
// Authentication
//   ↑
// ExceptionMiddleware
// 📤 8. Response Sent to Client
// [
//   { "id": 1, "title": "Task 1" }
// ]

// 🧠 9. Request Ends → Memory Cleanup
// VERY IMPORTANT 🔥

// [ REQUEST SCOPE DESTROYED ]
// ✔ TaskController disposed
// ✔ TaskService disposed
// ✔ AppDbContext disposed
// ✔ DB connection released

// 🧠 What remains?
// [ GLOBAL MEMORY ]

// ✔ DI container
// ✔ Middleware pipeline
// ✔ App config
// 🔥 Visual Summary
// APP START
//    ↓
// Build DI + Middleware

// ----------------------------------

// REQUEST START
//    ↓
// Create HttpContext + Scope
//    ↓
// Middleware 1
//    ↓
// Middleware 2
//    ↓
// Routing
//    ↓
// DI creates:
//    Controller
//    → Service
//    → DbContext
//    ↓
// Controller executes
//    ↓
// Service calls DB
//    ↓
// Response created
//    ↓
// Middleware (reverse)
//    ↓
// Response sent

// ----------------------------------

// REQUEST END
//    ↓
// Dispose all scoped objects
// 🧠 CRITICAL INSIGHTS (Architect Level)
// 🔥 1. Scoped Lifetime = Per Request
// One request → one DbContext → one Service instance
// 🔥 2. Controllers are NOT reused
// New controller created per request
// 🔥 3. DI is Lazy
// Objects created ONLY when needed
// 🔥 4. Middleware is Global
// Same middleware handles ALL requests
// 🔥 5. Memory Isolation
// Each request is isolated (thread-safe)



// ⚠️ Common Real-World Bug (Important)

// If you use:
// AddSingleton<AppDbContext>() ❌

// Then:
// Same DbContext reused across requests
// → concurrency issues
// → data corruption







