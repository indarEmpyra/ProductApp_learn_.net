//Note:
//? Program.cs/class Program is just the default convention the dotnet new templates use, not something the compiler, MSBuild, or the CLR actually requires. I verified this concretely rather than assuming.

// What I checked
// OneFlowAPI.csproj has no <StartupObject> setting — nothing explicitly telling the build "use this class as the entry point."
// There's exactly one method matching the entry-point signature in the whole project: Start.cs:19 → static void Main(string[] args), inside class Start.
// That's it — no special wiring exists. It just works because of how entry-point resolution actually happens.

//# How .NET actually finds the entry point
// The rule isn't "look for a file named Program.cs" — it's: the Roslyn compiler scans the entire compiled assembly for a method matching one of the valid Main signatures (static void Main(string[]), static int Main(...), static async Task Main(...), etc.), completely independent of what file it's in or what the containing class is named.

// If it finds exactly one candidate → that's automatically the entry point. This is your case.
// If it finds more than one (e.g., multiple classes with a Main method, common in solutions where a test/console project references others) → build error CS5001 ("does not contain a static 'Main' method suitable for an entry point"), and you'd have to explicitly disambiguate with <StartupObject>OneFlowAPI.Start</StartupObject> in the .csproj.
// Since this project has zero ambiguity, no <StartupObject> was ever needed — class Start { static void Main(...) } is functionally 100% identical to class Program { static void Main(...) }. The name Start carries no special meaning to the build system at all; it could be named Foo, Bootstrap, Kickoff — doesn't matter.

//# Why does the file also matter so little?
// There are actually two distinct styles ASP.NET Core projects use for their entry point, and it's worth being clear on which one this is:

// Top-level statements (what modern dotnet new webapi templates scaffold by default since .NET 6): Program.cs has no visible class or Main at all — just bare statements at the top of the file (var builder = WebApplication.CreateBuilder(args); ...). The compiler auto-generates a hidden Program class and Main method wrapping that code. This style does have one filename-adjacent rule: only one file in a project may use top-level-statement syntax — but that's a constraint on the syntax style, not on the file being named Program.cs specifically.

// Explicit Main method (the older, pre-.NET 6 style, and what this project actually uses): a normal class with a normal static void Main(...) method, written out in full — exactly what you see in Start.cs. This style has always allowed any class/file name; Program.cs/class Program was just what Visual Studio's project templates happened to scaffold by default, and most people never had a reason to deviate from it.

// OneFlowAPI project uses style 2, just under a different, deliberately chosen name. Whoever set this up most likely either migrated from an older template (where public class Program with an explicit Main was standard before top-level statements existed) and renamed it, or simply preferred Start as a clearer/more intentional name than the generic scaffold default — either way, it's a pure naming choice with zero functional consequence. Tooling that needs to locate/invoke the app's entry point (Visual Studio's debugger, dotnet run, IIS's ANCM launching OneFlowAPI.exe, even dotnet ef for migrations) all resolve it via the compiled assembly's entry-point metadata, not by looking for a file called Program.cs — so nothing here is fighting convention or relying on a workaround.






// using directives to include necessary namespaces for the application.

using Microsoft.EntityFrameworkCore;
// This is required to use the UseSqlServer() extension method when configuring the 
// DbContext with SQL Server in the dependency injection container.

using ProductApp.Data;
// This is required to use AppDbContext when configuring the DbContext with SQL Server in the dependency injection container.

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


// / ******************************************************** /

var builder = WebApplication.CreateBuilder(args);

//# var builder = WebApplication.CreateBuilder(args);
// This line initializes a new instance of the WebApplicationBuilder class, which is used to configure and build the web application.
// The builder provides access to services, configuration, and other settings needed to set up the application. 
// It takes command-line arguments (args) which can be used for configuration purposes 
// (e.g., setting environment variables, specifying URLs, etc.). 

//? Where does the WebApplicationBuilder class come from? 
// It is part of the Microsoft.AspNetCore.Builder namespace, which is included in the Microsoft.AspNetCore.App framework.

//# why did we not have to add "using Microsoft.AspNetCore.Builder;" at the top of Program.cs to use WebApplicationBuilder?
// In .NET 6 and later, the Microsoft.AspNetCore.App framework is included by default in ASP.NET Core applications, 
// and it provides implicit global usings for commonly used namespaces, including Microsoft.AspNetCore.Builder.

//# how to enable or disable implicit global usings in a .NET project?
// You can enable or disable implicit global usings in a .NET project by editing the project file (.csproj) and adding the following property:
// <PropertyGroup>
//   <ImplicitUsings>enable</ImplicitUsings> <!-- Set to "disable" to turn off implicit global usings -->
// </PropertyGroup>

//# What all we don't have to explicitly import because of implicit global usings? 
// In .NET 6 and later, implicit global usings are enabled by default in ASP.NET Core projects. 
// This means you don't have to explicitly import commonly used namespaces like:
// - System
// - System.Collections.Generic
// - System.Linq
// - System.Threading.Tasks
// - Microsoft.AspNetCore.Builder
// - Microsoft.Extensions.DependencyInjection
// - Microsoft.Extensions.Hosting
// 




//? How to provide environment variables and command-line arguments when running the application?
// You can provide environment variables and command-line arguments when running the application using the dotnet run command in the terminal.
// For example, to set an environment variable and provide a command-line argument, you can run:
// dotnet run --environment "Development" --urls "http://localhost:5000"  

//? What all can be passed as command-line arguments?
// You can pass various command-line arguments to configure your ASP.NET Core application, such as:
// --environment: Sets the hosting environment (e.g., Development, Staging, Production).
// --urls: Specifies the URLs the application should listen on (e.g., http://localhost:5000).
// --launch-profile: Specifies the launch profile to use from launchSettings.json (e.g., dev, https).
// --configuration: Overrides configuration values (e.g., --configuration "MySetting=Value").

// You can also define custom command-line arguments and access them in your application using 
// builder.Configuration.GetCommandLineArgs() or builder.Configuration.GetValue<string>("MyCustomArg"). 

// And, how to access the configuration/args values in the builder? You can access configuration values using builder.Configuration, 
// and command-line arguments can be accessed through builder.Configuration.GetCommandLineArgs() or directly from the args parameter if needed.
// The builder is the starting point for configuring the application's services and middleware before building and running the app.


//? builder.services is the IServiceCollection where you register services for dependency injection.
// This is where you add services that your application will use, such as controllers, database contexts, custom services, etc.

// is AddEndpointsApiExplorer pre-defined method in services class? 
// Yes, AddEndpointsApiExplorer() is a predefined extension method provided by the Microsoft.AspNetCore.Mvc.ApiExplorer namespace.

builder.Services.AddEndpointsApiExplorer();
// This is required for minimal APIs to generate OpenAPI documentation. 
// It registers the services needed to discover and describe your API endpoints, which is essential for Swagger to 
// generate accurate documentation. Even if you're using controllers, 
// it's a good idea to include this to ensure all endpoints are documented correctly.

// Can we use AddEndpointsApiExplorer() without using minimal APIs? 
//Yes, you can use AddEndpointsApiExplorer() even if you're not using minimal APIs.
// This method is part of the ASP.NET Core framework and is used to enable API endpoint discovery and documentation generation,
// which is beneficial for both minimal APIs and controller-based APIs. It helps tools like Swagger to generate accurate API 
// documentation regardless of the approach you take to define your endpoints.

// Can we add swagger without using minimal APIs? Yes, you can add Swagger to a controller-based API without using minimal APIs. 
// Swagger is a tool for generating interactive API documentation, and it can be used with both minimal
// APIs and traditional controller-based APIs. To add Swagger to a controller-based API, you would typically use the Swashbuckle.AspNetCore package,
// which provides the AddSwaggerGen() and UseSwaggerUI() methods to set up Swagger in your application. 
// You can follow the same steps to configure Swagger in a controller-based API as you would with a minimal API, 
// and it will work seamlessly to generate documentation for your API endpoints defined in controllers.

// #How to add this only for development environment? 
// You can conditionally add Swagger services and middleware based on the environment by checking the environment in Program.cs.
// For example, you can wrap the Swagger configuration in an if statement that checks if the environment
// is Development:
// if (app.Environment.IsDevelopment())
// {
//    builder.Services.AddEndpointsApiExplorer();
//    builder.Services.AddSwaggerGen();
//    app.UseSwagger();
//    app.UseSwaggerUI();
// }
// This way, Swagger will only be available in the development environment, and it won't be exposed in production, 
// which is a common best practice for security reasons.

// Can't we add builder.Services.AddEndpointsApiExplorer(); too inside the if statement for development environment?
// Yes, you can add builder.Services.AddEndpointsApiExplorer() inside the if statement for the development environment 
// if you only want to enable API endpoint discovery and documentation generation in development.

builder.Services.AddSwaggerGen();
// Add swagger for API documentation and testing
// The AddSwaggerGen() method registers the services required to generate Swagger documentation for your API.
// dotnet add package Swashbuckle.AspNetCore
// Swashbuckle.AspNetCore package (which provides AddSwaggerGen() and UseSwaggerUI()) is missing — 
// only Microsoft.AspNetCore.OpenApi is installed.


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Add dbContext to the service container
// This allows us to inject AppDbContext into our controllers and services using dependency injection.
// The UseSqlServer() method configures the DbContext to use SQL Server as the database provider, 
// and it retrieves the connection string from the app's configuration (e.g., appsettings.json) using the key "DefaultConnection".

//# how to read appsettings.json value. i.e
//1.  ConnectionString: builder.Configuration.GetConnectionString("DefaultConnection") looks for a connection string named "DefaultConnection"
// in the configuration sources (like appsettings.json, environment variables, etc.) and returns its value.

//2. builder.Configuration.GetValue<string>("MySetting") looks for a configuration value with the key "MySetting" and returns it as a string.

//3. builder.Configuration.GetSection("MySection").GetValue<string>("MySetting") looks for a section named "MySection"
//  in the configuration and then retrieves the value of "MySetting" within that section as a string.


// Add Controllers
builder.Services.AddControllers();
// This registers the services required for using controllers in the application.
// It allows you to define API endpoints using controller classes decorated with attributes like [ApiController], [Route], [HttpGet], etc.
// This is essential for building a Web API using the controller-based approach in ASP.NET Core.


// Add Services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUserService, UserService>();

// ? What this line does:
// It tells ASP.NET's built-in IoC container: "Whenever something asks for IProductService, create and give it a ProductService."
// IOC container will manage the lifecycle of ProductService instances based on the specified lifetime (scoped in this case),
// and it will automatically resolve and inject the dependencies (like AppDbContext) when creating a ProductService instance.

// AddScoped → A new instance of the service will be created for each HTTP request and shared within that request.
// This is the most common lifetime for services that interact with the database (like your ProductService and UserService) 
// because it ensures that each request gets its own instance of the service and its dependencies (like DbContext), 
// which helps to manage resources and avoid issues with shared state across requests.

//# Why is this important for each request to have their own instance of the service and its dependencies?
// This is important because it prevents issues with shared state and concurrency that can arise when using singleton services for things like database contexts.

//# If request is GET user(id) and we create all user services for that request, won't that be a waste of resources?
// Not necessarily, because the scoped lifetime ensures that each request gets its own instance of the service and its dependencies.
// This means that the resources are only used for the duration of the request and are released afterwards, which is efficient and safe.

//# Is this a pre-defined way to register services in ASP.NET Core? 
// Yes, AddScoped is one of the built-in methods provided by ASP.NET Core's dependency injection system to register services with a specific lifetime.
// And, is this constructor injection? 
// Yes, this is an example of constructor injection, where the dependencies (IProductService and IUserService) are injected into the controllers
// or other services that require them through their constructors.

// If we were to inject a service without registering it in the DI container, we would get an error at runtime when the application tries to resolve that service.

// ? What is Dependency Injection (DI)?

// How to understand classes dependencies in lay man language?
// Dependencies are the other classes or services that a class relies on to perform its functions.
// For example, a ProductService might depend on an AppDbContext to access the database and perform CRUD operations on products, 
// and a ProductController might depend on IProductService to handle business logic related to products.
// Much like how a car depends on an engine to run, a class depends on its dependencies to function properly.  

//# Instead of a class creating its own dependencies, they are provided from outside. 
// Compared to tightly coupled code where a class creates its own dependencies, DI promotes loose coupling and makes
// it easier to test your application by allowing you to inject mock implementations of your services during testing.  
// ASP.NET sees IProductService in the constructor and automatically resolves and injects a ProductService instance.
// Why use the Interface (IProductService) instead of ProductService directly?
// The controller depends on the abstraction, not the implementation. This means you can swap implementations without touching the controller:

//# If we were to inject a service as a singleton instead of scoped, we would have a single instance of that service shared across all requests, 
// which could lead to issues with shared state and concurrency, especially if the service 
// interacts with a database context (like AppDbContext) that is also scoped. 
// builder.Services.AddSingleton<IProductService, ProductService>(); 
// Example of a singleton service: a configuration service that reads settings once and provides them throughout the app.

//# If we were to inject a service as transient, a new instance would be created every time it is requested, 
// which could lead to performance issues if the service is expensive to create or if it maintains state that should be shared within a request. 
// builder.Services.AddTransient<IProductService, ProductService>();
// Example of a transient service: a service that generates random values or performs simple calculations without maintaining any state.

//# Isn't Transient and Scoped same then?
// No, they are different in terms of their lifetimes and how they manage instances:
// - Scoped: A new instance of the service is created for each HTTP request and shared within that request.
// - Transient: A new instance of the service is created every time it is requested, regardless of the HTTP request.

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
// Can you explain this line in more detail?  
// This means that for each HTTP request, a new instance of ProductService and AppDbContext is created.
// They are disposed of at the end of the request, ensuring that each request has its own isolated set of services.
// This is important because it prevents issues with shared state and concurrency that can arise when using singleton services for things like database contexts.
// 


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

//# how to read environment variables in ASP.NET Core? Use builder.Configuration.GetValue<string>("MyVariable") 
// or builder.Configuration.GetSection("MySection").GetValue<string>("MyVariable")
// Example: var myVariable = builder.Configuration.GetValue<string>("MyVariable");
// You can also use Environment.GetEnvironmentVariable("MyVariable") to read environment variables directly from the system, 
// but using builder.Configuration is recommended as it integrates with the app's configuration system and supports various configuration sources 
// (like appsettings.json, environment variables, user secrets, etc.).
// Environment.IsEnvironment("MyEnvironment") to conditionally enable features based on environment settings.

// app.Environment provides information about the hosting environment (Development, Staging, Production).
// This allows you to conditionally enable features (like Swagger) only in development, which is a common best practice for security reasons.

if (app.Environment.IsDevelopment())
{
  app.UseSwagger(); // Generate swagger.json at runtime
  app.UseSwaggerUI(); // Serve the Swagger UI at /swagger, which reads swagger.json and renders the interactive API docs
}
// Only enable Swagger in development environment for security reasons
// In production, you typically don't want to expose detailed API documentation publicly.
// You can configure this further by using environment variables or appsettings.json to control when Swagger is enabled.

// Middleware configuration
// is "Use" as a prefix used for middleware as a standard/convention? 
// Yes, in ASP.NET Core, middleware components are typically added to the request pipeline using the "Use" prefix.

app.UseHttpsRedirection();
// Redirect HTTP requests to HTTPS for better security

app.UseStaticFiles();

//# Enable CORS middleware to allow cross-origin requests (if needed)
// app.UseCors("AllowAll"); // Use the CORS policy defined in Program.cs
// This allows your API to be accessed from different origins (like a frontend app running on a different domain) by applying the specified CORS policy.
// Custom CORS policies can be defined in Program.cs using builder.Services.AddCors() and then applied here with app.UseCors().
// app.UseCors(policy =>
//     policy.AllowAnyOrigin()
//           .AllowAnyMethod()
//           .AllowAnyHeader());


//# How to enable only certain Ip addresses to access the API using cors?
// You can create a custom CORS policy that allows requests only from specific IP addresses.
// Example:
// builder.Services.AddCors(options =>  
// {
//     options.AddPolicy("AllowSpecificIPs", policy =>
//     {
//        policy.WithOrigins("http://allowed-ip1", "http://allowed-ip2") // Replace with actual allowed IPs or domains
//              .AllowAnyMethod()
//              .AllowAnyHeader();
//     });
// });

// 


//# How to enable only certain Ip addresses to access the API?
// You can create a custom middleware to check the incoming request's IP address against a list of allowed IPs and return a 403 Forbidden response if the IP is not allowed.  


// If you have a frontend application (like React or Angular) that needs to call your API, you would configure CORS to allow requests from the frontend's origin.
//   


app.UseAuthorization();
// Enable authorization middleware (if you have any [Authorize] attributes in your controllers)
// [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Staff, ExternalAPIClient")] 
// This means that the endpoint requires authentication using the "Bearer" scheme and is only accessible to users in the "Admin", "Staff", or "ExternalAPIClient" roles.
// If you have authentication middleware (like app.UseAuthentication()) configured before this, 
// it will handle the authentication process and set the User property on the HttpContext. 

//# what are schemes in ASP.NET Core authentication? And, how wre we adding them to Authorize attribute? Are there more parameters?
// Authentication schemes in ASP.NET Core are a way to specify which authentication handler should be used for a particular endpoint or controller.
// They allow you to have multiple authentication methods (like JWT, cookies, etc.) in the same application and control which one is used for each endpoint.
// You can specify the authentication scheme in the [Authorize] attribute using the AuthenticationSchemes property, like this:
// [Authorize(AuthenticationSchemes = "Bearer")]
// This tells ASP.NET Core to use the "Bearer" authentication scheme for that endpoint, which typically corresponds to JWT authentication.
// You can also specify roles and policies in the [Authorize] attribute to control access based on user roles or custom policies, like this:
// [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Staff")]
// This means that the endpoint requires authentication using the "Bearer" scheme and is only accessible to users in the "Admin" or "Staff" roles.
// Other parameters you can use in the [Authorize] attribute include Policy (for custom authorization policies), and you can also combine multiple schemes and roles as needed. 
// For example, you can specify multiple authentication schemes like this:
// [Authorize(AuthenticationSchemes = "Bearer, Cookie", Roles = "Admin, Staff")]
// 

// Add custom middleware to log requests and responses
app.UseMiddleware<RequestLoggingMiddleware>();
// We can inject multiple custom middlewares by stacking them one-by-one.

//# app.AnyCustomMethod();
//# Can we call any custom method from any class on "app"? I believe not, there must be some logic to this which is lettin us call this sutom method on app. And, does that custom method have to be in certain way or return middlewares ?

// You're right to sense there's a specific mechanism here — this isn't WebApplication having some open-ended "call anything" behavior. It's C#'s extension method feature, and the exact code confirms it precisely.

// The mechanism

// // SwaggerConfiguration.cs:9-11
// public static class SwaggerConfiguration
// {
//     public static WebApplication ConfigureSwagger(this WebApplication app)
// The this keyword before the first parameter (this WebApplication app) is what turns an ordinary static method into something callable with instance-call syntax (app.ConfigureSwagger()) even though ConfigureSwagger is not a member of WebApplication at all — it's a completely unrelated static method sitting in a static class in a different file/namespace.

// For this to compile and work, three specific conditions must all hold:

// The containing class must be public static class — SwaggerConfiguration is.
// The method must be public static, with its first parameter prefixed by this <Type> — here, this WebApplication app. The compiler treats <Type> as "the type this method extends."
// The namespace containing that static class must be in scope at the call site, via a using directive. That's exactly why Start.cs:3 has using OneFlowAPI.AppStartUp; — without that line, app.ConfigureSwagger() wouldn't compile; the compiler wouldn't know to look in that static class at all.
// At compile time, when the compiler sees app.ConfigureSwagger(), it first checks whether WebApplication (or its base types/interfaces) has an actual instance method called ConfigureSwagger — it doesn't — then it searches every static class brought into scope by a using directive for an extension method whose this-parameter type is compatible with app's type. It finds exactly one match here, and rewrites the call into what it really is under the hood: SwaggerConfiguration.ConfigureSwagger(app).

// This is also why you can call things like UseRouting(), MapControllers(), and ConfigureSwagger() all on the same app variable despite them coming from three completely unrelated NuGet packages (Microsoft.AspNetCore.Routing, Microsoft.AspNetCore.Mvc, and this project's own OneFlowAPI.AppStartUp) — none of them are real members of WebApplication. WebApplication itself implements several interfaces (IApplicationBuilder, IEndpointRouteBuilder, IHost), and each library just ships extension methods targeting whichever of those interfaces (or WebApplication directly, as Swashbuckle/this custom method both do) it needs.

// Does it have to return middleware, or follow a specific shape?
// No — there's no compiler-enforced contract at all. An extension method's body can do absolutely anything, and its return type can be anything, including void. This would compile and run just as validly:


// public static class Foo
// {
//     public static void SeedCache(this WebApplication app)
//     {
//         Console.WriteLine("hi");   // does nothing pipeline-related
//     }
// }
// // called as: app.SeedCache();
// What you're seeing in ConfigureSwagger — returning the same WebApplication instance it was given — is a voluntary convention, not a requirement:


// public static WebApplication ConfigureSwagger(this WebApplication app)
// {
//     if (app.Environment.IsDevelopment())
//     {
//         app.UseSwagger();
//         app.UseSwaggerUI(c => { ... });
//     }
//     return app;
// }
// This mirrors exactly what ASP.NET Core's own built-in UseXxx extension methods do internally: they call app.Use(...)/app.UseMiddleware<T>() to actually register a delegate into the pipeline, then return app; so the calls could be chained fluently (app.UseRouting().UseAuthentication(), though this codebase writes them as separate statements instead — both are equally valid since the method just returns the same object).

// So to directly answer both parts: yes, this only works because of C# extension methods with a very specific, compiler-checked shape (static, in a static class, first param marked this, namespace in scope) — but no, there's no requirement that such a method register middleware or return anything in particular. ConfigureSwagger happens to register real middleware (UseSwagger/UseSwaggerUI) inside its body and return app purely because its author chose to follow the same pattern Microsoft's own middleware-registration methods use — not because C# or ASP.NET Core mandates it.



app.MapControllers();
// Map controller routes (e.g., /api/product) to the corresponding controller actions based on attributes like [HttpGet], [HttpPost], etc.


app.Lifetime.ApplicationStarted.Register(() =>
{
  var logger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger("Startup");
  var urls = app.Urls.Any() ? string.Join(", ", app.Urls) : "No URLs bound";

  // logger.LogInformation("Application is running on: {Urls}", urls);
  Console.WriteLine($"Application is running on: {urls}");
});
// This registers a callback to log the URLs the application is listening on when the application starts.
// app.Services.GetRequiredService<ILoggerFactory>() retrieves the ILoggerFactory from the dependency injection container,
// and CreateLogger("Startup") creates a logger instance with the category "Startup" for logging purposes.
// app.Urls contains the URLs that the application is configured to listen on, and we log them when the 
// application starts to confirm that it's running and to know which URLs are active.
// 

app.Run();
// Start the application and listen for incoming HTTP requests. 
// The app will run until you stop it (e.g., Ctrl+C in the terminal).

// Most useful app properties:

// app.Urls
// Type: ICollection<string>
// Contains listening addresses like http://localhost:5186, https://localhost:7199

// app.Environment
// Type: IWebHostEnvironment
// Use:
// app.Environment.EnvironmentName
// app.Environment.IsDevelopment()
// app.Environment.IsProduction()

// app.Configuration
// Type: IConfiguration
// Read config values and appsettings values

// app.Services
// Type: IServiceProvider
// Resolve DI services

// app.Logger
// Type: ILogger
// App-level logging

// app.Lifetime
// Type: IHostApplicationLifetime
// Hooks:
// ApplicationStarted
// ApplicationStopping
// ApplicationStopped
// How to get port:

// Parse from each entry in app.Urls
// Example concept: new Uri(url).Port
// So Node-like mapping is:

// Node app.get("env") -> ASP.NET app.Environment.EnvironmentName
// Node server.address().port -> ASP.NET parse port from app.Urls
// Node listen host/port -> ASP.NET app.Urls (or launch settings / --urls)



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

//? Should the migration be an automated process that runs with the build?
// It depends on your deployment strategy and the environment you're working in.
// 





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

//# Wht's the best practice to store db connection string as it always has login credentials in it?
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


//? How does a Class uses methods of other class? Does the class need to create an instance of the other class to use its methods?
// A class can use methods of another class by creating an instance of that class and calling its methods.
// For example, if you have a ProductService class that needs to use methods from AppDbContext,
// the ProductService would typically have a constructor that takes an AppDbContext as a parameter, and then it can call the methods of AppDbContext 
// to perform database operations.

//# Creating instance means, creating an object of that class using the new keyword, like this:
// var dbContext = new AppDbContext();

//# Let's use methods of AppDbContext in ProductService without using DI:
// public class ProductService : IProductService
// {
//    private readonly AppDbContext _context;
//    public ProductService()
//    {
//        _context = new AppDbContext(); // Manually creating an instance of AppDbContext
//    }
//     // Now you can use _context to access the database in your methods
// }

// However, this approach can lead to tightly coupled code and makes it difficult to manage dependencies, especially as the application grows in complexity.


//# But, 
// with the DI system in ASP.NET Core, you don't have to manually create instances of your dependencies (like AppDbContext) in your classes (like ProductService).
// Instead, you can register your services and their dependencies in the DI container (using builder.Services.AddScoped(), AddSingleton(), etc.),
// and then you can simply declare your dependencies in the constructor of your classes, and the framework will automatically 
// resolve and inject the correct instances when needed. 

//# How to declare your dependencies in the constructor of your classes?
// You can declare your dependencies in the constructor of your class by adding parameters that correspond to the services you want to inject.
// For example, in your ProductService class, you can declare a dependency on AppDbContext like this:
// public class ProductService : IProductService
// {
//     private readonly AppDbContext _context;

//     public ProductService(AppDbContext context)
//     {
//         _context = context;
//     }

//     // Now you can use _context to access the database in your methods
// }


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
// Do we need to copy the source code to the IIS server? 
// No, you only need to copy the published files (including the DLLs) to the IIS server.
// What files/folders do we need to copy to the IIS server?
// You need to copy the contents of the publish output directory, which typically includes:
// - The compiled DLLs (your application and its dependencies)
// - The web.config file (automatically generated during publish)
// - Any static assets (like images, CSS, JavaScript files) required by your application
// // You need to copy the files generated by the dotnet publish command, which include the compiled DLLs, configuration files (like web.config), 
// and any static assets required by your application. You do not need to copy the source code files (.cs) to the IIS server, 
// as the application will run based on the compiled assemblies (DLLs) and configuration files.


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





//? Program.cs
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





//? In a .NET web app:

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



//? I’ll walk you through a real request lifecycle, including:

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



//NOTE: Files of  .Net application:
// .csproj - project file that defines dependencies, target framework, build settings, etc. (similar to package.json but XML-based)
// appSettings.json - connection strings, API keys, logging config, feature flags, etc. (non-sensitive config)
// appSettings.Development.json - development-specific config (but still not ideal for secrets)
// Properties/launchSettings.json- Used only for local development i.e ports, environment


















// 1️⃣ Dependency Injection (DI container)
// Why .NET uses DI everywhere.
// 2️⃣ ASP.NET Request Pipeline
// This explains:
// app.UseAuthentication()
// app.UseAuthorization()
// 3️⃣ Entity Framework + DbContext
// How database connection works.

//TODO: Questions to go deep
// How DI container works internally
// Request lifecycle deep dive (thread, async, pipeline)
// Scoped vs Singleton pitfalls (very important in real apps)
// Threading model(async/await + thread pool)
// How Kestrel handles concurrent requests
// How EF manages connection pooling


//TODO: To understand
// ✔ How .NET handles requests
// ✔ How DI creates objects
// ✔ How middleware controls flow
// ✔ How EF translates LINQ → SQL
// ✔ How to optimize DB queries
// ✔ How the Dependency Injection container works internally and how objects get created automatically.
// How DI container works internally
// Request lifecycle deep dive (thread, async, pipeline)
// Scoped vs Singleton pitfalls (very important in real apps)
// Threading model(async/await + thread pool)
// How Kestrel handles concurrent requests
// CQRS + MediatR
// Background workers
// Caching
// Distributed systems patterns in .NET
// Minimal APIs vs Controllers
// Generic repositories (and why many architects avoid them)