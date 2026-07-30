
//? request lifecycle, including:

// object creation
// memory scope
// DI resolution
// middleware flow

// We’ll use your current app:

//# GET /tasks
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

// ----------------------------------

// ⚠️ Important:

// NO controllers, NO services, NO DbContext created yet

//# 🚀 1. Request Comes In
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

//# 🔁 2. Middleware Pipeline Starts
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


//# 🎯 3. Routing → Controller Selection
// Route: GET /tasks
// → TaskController.GetTasks()


//# 🧠 4. Controller Creation (DI kicks in)

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

//? 🧠 Memory Now Looks Like:
// [ REQUEST SCOPE MEMORY ]
// ----------------------------------

// HttpContext

// TaskController instance
// TaskService instance
// AppDbContext instance

// ----------------------------------

// 👉 All are SCOPED → same instance reused in this request


//# ⚡ 5. Controller Action Executes
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

//# 🔁 6. Response Creation

// Controller:

// return Ok(tasks);
// Framework does:
// Object → JSON serialization

// Memory:

// JSON string created

//# 🔁 7. Response Goes Back Through Middleware

// Pipeline reverses:

// Controller
//   ↑
// Authorization
//   ↑
// Authentication
//   ↑
// ExceptionMiddleware

// Question: When a request comes in, it goes through the middleware pipeline but how does the response go back through the middleware? Does it reverse the order?
// Answer: Yes, the response goes back through the middleware in reverse order. When a request is processed, it flows through the 
// middleware pipeline in the order they are registered. Once the controller action returns a response, that response then travels back 
// through the middleware in reverse order, allowing each middleware to modify or handle the response as needed before it is sent back to the client.   


//# 📤 8. Response Sent to Client
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



// ______________________________________________________________________________________________________ //

//? Concurrent Requests + Lifetimes Deep Dive

//# PART 1 — Two Concurrent Requests

// Suppose two users call:

// GET /tasks

// at the same time.

// 🧠 Global App Memory (Shared)

// This exists only once:

// [ GLOBAL APP MEMORY ]
// ----------------------------------

// DI Container
// Middleware Pipeline
// App Config
// Singleton services

// ----------------------------------

// Shared by ALL requests.

// 🚀 Request A Arrives
// User A → GET /tasks

// Creates:

// [ REQUEST A SCOPE ]
// ----------------------------------

// HttpContext A
// TaskController A
// TaskService A
// AppDbContext A

// ----------------------------------
// 🚀 Request B Arrives Simultaneously
// User B → GET /tasks

// Creates:

// [ REQUEST B SCOPE ]
// ----------------------------------

// HttpContext B
// TaskController B
// TaskService B
// AppDbContext B

// ----------------------------------
// 🔥 IMPORTANT

// Even though both requests hit:

// Same controller class
// Same service class

// They use:

// DIFFERENT OBJECT INSTANCES
// 🧠 Visualization
// GLOBAL MEMORY
// ────────────────────────────

// Middleware Pipeline
// DI Container

// ────────────────────────────

// REQUEST A
// ────────────────────────────

// TaskController #101
// TaskService #201
// DbContext #301

// ────────────────────────────

// REQUEST B
// ────────────────────────────

// TaskController #102
// TaskService #202
// DbContext #302
// 🔥 Why This Matters

// If User A updates data:

// DbContext A tracks ONLY A's changes

// User B:

// Has completely isolated memory

// This prevents:

// race conditions
// state leakage
// corruption
// 🚀 PART 2 — What if DbContext was Singleton? ❌

// Suppose:

// AddSingleton<AppDbContext>()

// Now memory becomes:

// GLOBAL MEMORY
// ────────────────────────────

// ONE shared DbContext

// ────────────────────────────

// REQUEST A
// uses DbContext #1

// REQUEST B
// uses SAME DbContext #1
// 💥 Disaster Scenario
// Request A
// task.Title = "Updated by A"

// EF starts tracking change.

// Before SaveChanges finishes...

// Request B:

// task.Title = "Updated by B"

// SAME DbContext. SAME tracked entity.

// 💥 Possible Problems
// ✔ Wrong data saved
// ✔ Entity tracking conflicts
// ✔ Random exceptions
// ✔ Thread safety issues
// ✔ Memory leaks
// 🔥 This is WHY DbContext = Scoped
// One request
// → one DbContext
// → isolated tracking
// 🚀 PART 3 — Lifetimes Deep Dive
// 🧠 1. Singleton
// AddSingleton<IMyService, MyService>()
// Behavior
// ONE instance for entire app lifetime
// Visualization
// APP START
//    ↓
// Create Singleton once
//    ↓
// ALL requests share it
// Good For

// ✔ config
// ✔ caching
// ✔ stateless utilities
// ✔ logging services

// Dangerous For

// ❌ DbContext
// ❌ request-specific data

// 🧠 2. Scoped (MOST COMMON)
// AddScoped<IMyService, MyService>()
// Behavior
// One instance per request
// Visualization
// REQUEST A → Service A
// REQUEST B → Service B
// Good For

// ✔ business services
// ✔ DbContext
// ✔ repositories

// 🧠 3. Transient
// AddTransient<IMyService, MyService>()
// Behavior
// NEW object every time requested
// Example
// public class A
// {
//    public A(IMyService s1, IMyService s2)
// }

// With transient:

// s1 ≠ s2

// Two separate instances.

// 🧠 Visualization Comparison
// Singleton
// APP
//  └── ONE shared instance
// Scoped
// REQUEST A → one instance
// REQUEST B → another instance
// Transient
// Every injection → new object
// 🚀 PART 4 — Real Request Timeline
// 🧠 Request A Timeline
// t=0ms  Request arrives
// t=1ms  Create scope
// t=2ms  Create Controller
// t=3ms  Create Service
// t=4ms  Create DbContext
// t=5ms  Execute query
// t=20ms Return response
// t=21ms Dispose scoped objects
// 🧠 Request B Timeline

// Runs independently:

// t=0ms  Request arrives
// t=1ms  Create separate scope
// ...
// 🔥 Key Insight
// ASP.NET Core is highly concurrent

// Many requests execute simultaneously.

// 🚀 PART 5 — Async + Thread Pool (Simplified)

// Suppose:

// await _context.Tasks.ToListAsync();
// 🧠 What happens internally
// 1. Thread sends DB query
// 2. Thread RETURNS to pool
// 3. Request waits asynchronously
// 4. DB responds
// 5. Another thread resumes execution
// 🔥 Huge Advantage
// Thread is NOT blocked waiting for DB

// This is why async scales well.

// 🧠 Node Comparison

// Node:

// Single-threaded event loop

// .NET:

// Multi-threaded thread pool

// Different internals, similar async goal.

// 🚀 PART 6 — MOST IMPORTANT ARCHITECT TAKEAWAYS
// 🔥 Rule 1
// DbContext MUST be Scoped
// 🔥 Rule 2
// Singletons must be stateless or thread-safe
// 🔥 Rule 3
// Never store request/user data in Singleton
// 🔥 Rule 4
// Middleware should usually be stateless
// 🔥 Rule 5
// Scoped services are safest default for business logic
// 🧠 Final Mental Model
// APP START
//    ↓
// Build shared infrastructure
//    ↓
// Each request gets:
//    - separate scope
//    - separate services
//    - separate DbContext
//    ↓
// Request ends
//    ↓
// Scoped memory destroyed
// 🚀 You Now Understand

// ✔ Request lifecycle
// ✔ DI object creation
// ✔ Concurrent requests
// ✔ Scoped vs Singleton vs Transient
// ✔ Why EF uses Scoped
// ✔ Async request handling



//$ Questions to Go Deeper
// 1. When concurrent requests come to the app, how do they all get treated at the same time and how does the db respond, especially 
// when upsert request come on the exact time. How does the db decide which record will go first and which one next. 
// Can't imagine a system with thousands request coming at the same timestamp.

// 2. Does each request cycle get it's own object of all dependencies, dbContext, controller and service? Or these objects get 
// created just once and all reuests use the same and share state?


//# Question 1: How do thousands of concurrent requests work? What happens when two upserts happen at the same time?

// Let's separate this into:

// Application Server (.NET)
//         +
// Database Server (SQL Server)

// Many developers accidentally think they're one thing.

// Part A: How ASP.NET Handles 1000 Requests

// Imagine:

// 1000 users hit GET /tasks
// at nearly the same moment

// What happens?

// Kestrel receives requests
// Request 1
// Request 2
// Request 3
// ...
// Request 1000

// Kestrel doesn't say:

// Wait for Request 1 to finish,
// then start Request 2

// Instead:

// Queue incoming requests
// Assign available thread-pool workers
// Run them concurrently

// Think of a restaurant:

// Customers = requests
// Waiters = threads
// Kitchen = database

// One waiter doesn't serve all tables.

// Part B: What Happens During DB Calls?

// Suppose 1000 requests execute:

// await _context.Tasks.ToListAsync();

// Important:

// The thread does NOT sit idle waiting.

// Flow:

// Thread sends SQL query
// ↓
// Thread returns to thread pool
// ↓
// DB works on query
// ↓
// DB responds
// ↓
// Another thread resumes request

// This is why async scales.

// Part C: Two Users Update Same Record

// Now the interesting part.

// Suppose table:

// Task
// ----------------
// Id=1
// Title="Old Title"
// User A
// Update Task 1
// Title = "From A"
// User B
// Update Task 1
// Title = "From B"

// Both arrive nearly simultaneously.

// What Does SQL Server Do?

// SQL Server doesn't execute both updates on the exact same row at the exact same CPU instruction.

// Internally:

// Transaction A
// Transaction B

// The database uses:

// Locks
// Transactions
// Isolation Levels
// Simplified

// Suppose:

// UPDATE Tasks
// SET Title='From A'
// WHERE Id=1

// starts first.

// SQL Server may place a lock on that row.

// Then:

// UPDATE Tasks
// SET Title='From B'
// WHERE Id=1

// must wait.

// Result:

// A finishes
// ↓
// Lock released
// ↓
// B executes

// Final value:

// From B

// (last writer wins)

// How Does DB Decide Who Goes First?

// The simplest answer:

// Whoever acquires the lock first

// That is determined by:

// scheduler
// timing
// CPU
// network
// query execution

// You should never assume order.

// Real-World Protection: Optimistic Concurrency

// Enterprise apps often use a version column.

// Example:

// Id
// Title
// Version

// Initial:

// 1
// Old Title
// Version=5

// User A reads:

// Version=5

// User B reads:

// Version=5

// User A updates:

// Version=6

// User B tries updating:

// Expected Version=5
// Actual Version=6

// SQL Server/EF rejects update.

// This prevents lost updates.

// You'll often see this implemented using:

// [Timestamp]
// public byte[] RowVersion { get; set; }

// in EF.

// "What about 10,000 requests at same timestamp?"

// This is the key realization:

// They don't actually happen at the exact same nanosecond.

// Think:

// 10:00:00.001
// 10:00:00.002
// 10:00:00.003

// The differences are tiny, but the OS scheduler, web server, and database serialize work internally when needed.

//# Question 2: Does every request get new objects?

// The answer depends on lifetime.

// Controller

// By default:

// NEW controller per request

// Request A:
// TaskController #101

// Request B:
// TaskController #102

// Different objects.

// Scoped Service
// AddScoped<ITaskService, TaskService>()

// Request A:

// TaskService #201

// Request B:

// TaskService #202

// Different objects.

// DbContext
// AddDbContext<AppDbContext>()

// (which internally uses Scoped)

// Request A:
// DbContext #301

// Request B:
// DbContext #302

// Different objects.

//# Singleton Example
// AddSingleton<ICacheService, CacheService>()

// Request A:

// CacheService #1

// Request B:

// CacheService #1

// Same object.

// Visual Comparison
// Scoped
// Request A
//  ├─ Controller A
//  ├─ Service A
//  └─ DbContext A

// Request B
//  ├─ Controller B
//  ├─ Service B
//  └─ DbContext B
// Singleton
// Request A
//  └─ CacheService #1

// Request B
//  └─ CacheService #1

// Request C
//  └─ CacheService #1
// One More Important Detail

// Within the SAME request:

// public class TaskController
// {
//     public TaskController(
//         ITaskService service1,
//         ITaskService service2)
// }

// If ITaskService is Scoped:

// service1 === service2

// Same instance.

// Because the request scope already contains one.

// Architect Mental Model

// Think of a request like this:

// Request arrives
//     ↓
// Create request scope
//     ↓
// Create controller
//     ↓
// Create scoped services
//     ↓
// Create DbContext
//     ↓
// Execute logic
//     ↓
// Dispose everything

// while

// Singletons stay alive
// for entire application lifetime

// That's why:

// Controllers = usually transient/per-request
// Services = usually scoped
// DbContext = scoped
// Cache = singleton
// Configuration = singleton

// The biggest thing to remember is:

// A .NET app is not "one controller talking to many users".

// It's thousands of short-lived object graphs
// being created and destroyed continuously,
// one graph per request.


//# Who creates these short-lived object graphs? 
// The DI container. It creates a new graph for each request, ensuring isolation and thread safety. 
// Each graph includes the controller, its dependencies, and the DbContext, all scoped to that request. 
// Once the request is complete, the entire graph is disposed, preventing memory leaks and ensuring that resources are released properly.


//# So, when we run the app and multiple requests come in, the DI container creates separate instances of controllers, services, and DbContexts for each request. 
// This means that each request is isolated and can safely interact with the database without worrying about concurrency issues. 
// The DI container manages the lifetimes of these objects, ensuring that they are created and disposed correctly based on their 
// registered lifetimes (scoped, singleton, transient).


//# Who creates the DI container? Does the DI container create itself when we start the application or build the app?
// The DI container is created by the framework when you call builder.Build() in Program.cs. 
// When you call builder.Build(), the framework sets up the DI container based on the services you've registered in builder.Services. 
// The container is then used throughout the application to resolve dependencies and manage object lifetimes. 

// So, the DI container is essentially created and configured during the application startup phase, and it manages itself from there on out, 
// creating and disposing objects as needed based on the registered services and their lifetimes.

// Once DI container gets created, We register dependencies in it and then it creates objects for each request based on the registered dependencies and their lifetimes.
