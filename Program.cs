using ProductApp.Data;
// This is required to use AppDbContext when configuring the DbContext with SQL Server in the dependency injection container.
// using directives to include necessary namespaces for the application.


using Microsoft.EntityFrameworkCore;
// This is required to use the UseSqlServer() extension method when configuring the 
// DbContext with SQL Server in the dependency injection container.

using ProductApp.Services.Interfaces;
using ProductApp.Services.Implementations;


using ProductApp.Middleware;
// This is required to use the RequestLoggingMiddleware in the middleware pipeline configuration 
// (app.UseMiddleware<RequestLoggingMiddleware>()).

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
// Instead of a class creating its own dependencies, they are provided from outside. Compare:
// ASP.NET sees IProductService in the constructor and automatically resolves and injects a ProductService instance.
// Why use the Interface (IProductService) instead of ProductService directly?
// The controller depends on the abstraction, not the implementation. This means you can swap implementations without touching the controller:

// ? What does AddScoped mean?
// It controls the lifetime of the created instance:
// Method	Lifetime	When to use:

// AddSingleton	One instance for the entire app	Stateless shared services
// AddScoped	One instance per HTTP request	DB-related services (like yours)
// AddTransient	New instance every time it's requested	Lightweight, stateless utilities

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


//
var app = builder.Build();
// This builds the WebApplication based on the configuration defined in the builder.
// After this line, the app variable contains the configured application, and you can start adding middleware

// how to read environment variables in ASP.NET Core? Use builder.Configuration.GetValue<string>("MyVariable") or builder.
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


// Read launchSettings.json to see the configured URLs and launch profiles. 
// By default, ASP.NET Core apps have two profiles: "http" and "https".
// The app runs on different ports. Use these URLs:

// HTTP: http://localhost:5186
// HTTPS: https://localhost:7199
// And since this is a Web API (no UI), go directly to an endpoint, e.g.:

// http://localhost:5186/api/product

// Note: dotnet run uses the http profile by default. To use HTTPS, run dotnet run --launch-profile https.



// Migrations are a way to keep your database schema in sync with your EF Core model classes.
// When you create or modify your model classes (like Product or User), you need to create a migration to update the database schema accordingly.
// The migration captures the changes you made to your model and generates code to apply those changes to the database.
// To create a migration, you use the following command in the terminal:

// Run the EF Core migration commands to create the database schema based on your models:
//    - dotnet ef migrations add InitialCreate
//    - dotnet ef database update





// Environment Variables and User Secrets:
// In ASP.NET Core, you can manage sensitive configuration data (like API keys, connection strings, etc.) 
// using environment variables or the User Secrets feature during development.

// appsettings.json is great for non-sensitive configuration, 
// but you should avoid putting secrets there, especially if your code is in a public repository.

// appsettings.development.json is used for development-specific settings, but it’s still not ideal for secrets.

// dotnet user-secrets init
// dotnet user-secrets set "ApiKey" "secret-value"