//? What is appsettings.json?
// It's the default configuration file for ASP.NET Core apps — a plain JSON file, checked into source control,
// holding settings the app needs at runtime but that aren't compiled into the code: connection strings,
// logging levels, feature flags, third-party API base URLs, etc. It ships in this project at the root:
// appsettings.json (this project's copy has Logging.LogLevel, AllowedHosts, and ConnectionStrings.DefaultConnection).

// Why not just hardcode these values in C#? Because changing them would require a rebuild + redeploy.
// A JSON file can be edited (or replaced per-environment, or overridden by env vars) without touching code.

//# Where does the framework actually read appsettings.json from?
// WebApplication.CreateBuilder(args) (Program.cs:71 in this project) wires up a default configuration
// pipeline before your code ever runs. That pipeline loads, in this order (each later source can override
// keys set by an earlier one):
//   1. appsettings.json
//   2. appsettings.{Environment}.json   (Environment = ASPNETCORE_ENVIRONMENT, e.g. "Development")
//   3. User Secrets (Development environment only)
//   4. Environment variables
//   5. Command-line arguments
// This is why builder.Configuration (used at Program.cs:198 for the connection string) already has
// everything merged — you never manually tell it to "go read appsettings.json," it just does.


/*_______________________________________________________________________________________________________________________________ */

//? appsettings.json vs appsettings.{Environment}.json — how do they combine, not replace?

// They're merged key-by-key (a "layered" config), not swapped wholesale. Example from this project:

// appsettings.json:
// {
//   "Logging": { "LogLevel": { "Default": "Information", "Microsoft": "Warning", "System": "Warning" } },
//   "AllowedHosts": "*",
//   "ConnectionStrings": { "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ProductDb;..." }
// }

// appsettings.Development.json:
// {
//   "Logging": { "LogLevel": { "Default": "Debug", "Microsoft": "Information", "System": "Information" } },
//   "ConnectionStrings": { "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ProductDb;..." }
// }

// Running with ASPNETCORE_ENVIRONMENT=Development, the effective config is:
//   Logging.LogLevel.Default = "Debug"   <- overridden by appsettings.Development.json
//   AllowedHosts = "*"                    <- kept from appsettings.json (Development file doesn't mention it)
//   ConnectionStrings.DefaultConnection = the Development file's value (points at the same local DB here,
//                                          but could point somewhere else entirely per environment)

// appsettings.Production.json in this project only overrides ConnectionStrings.DefaultConnection
// (pointing at "Server=localhost\\SQLEXPRESS" instead of localdb) — everything else falls back to the
// base appsettings.json. That's the whole pattern: put shared defaults in the base file, put only the
// deltas in the per-environment file.

//# How does the app know which environment it's in?
// The ASPNETCORE_ENVIRONMENT environment variable. Common values: Development, Staging, Production.
// Set via Properties/launchSettings.json profiles when running locally (dotnet run --launch-profile ...),
// or via the actual OS/container environment variable in a real deployment (IIS app pool setting, Docker
// ENV, Azure App Service configuration, etc.). If unset, ASP.NET Core defaults to "Production".


/*_______________________________________________________________________________________________________________________________ */

//? How to read appsettings.json values in code

// 1. A single value:
//    builder.Configuration.GetValue<string>("AllowedHosts")

// 2. A nested value (section -> key):
//    builder.Configuration.GetSection("Logging").GetValue<string>("LogLevel:Default")
//    // or equivalently, colon-separated path in one call:
//    builder.Configuration.GetValue<string>("Logging:LogLevel:Default")

// 3. Connection strings specifically have their own shortcut method (used at Program.cs:198):
//    builder.Configuration.GetConnectionString("DefaultConnection")
//    // equivalent to: builder.Configuration.GetValue<string>("ConnectionStrings:DefaultConnection")

// 4. Binding a whole section to a strongly-typed class (better than scattering GetValue calls everywhere):
//    // appsettings.json:  "MySettings": { "ApiKey": "abc", "TimeoutSeconds": 30 }
//    public class MySettings
//    {
//        public string ApiKey { get; set; }
//        public int TimeoutSeconds { get; set; }
//    }
//    builder.Services.Configure<MySettings>(builder.Configuration.GetSection("MySettings"));
//    // then inject IOptions<MySettings> into any service's constructor instead of re-reading raw config.

//# Why prefer the IOptions<T> pattern over calling GetValue everywhere?
// Typos in a raw string key ("Logging:LogLevel" typed as "Loging:LogLevel") fail silently (you just get null/default), whereas a bound
// class fails to compile if you typo a property name. It also means a service that needs 5 related settings
// declares one dependency (IOptions<MySettings>) instead of holding onto IConfiguration and hoping it reads
// the right keys everywhere it's used.


/*_______________________________________________________________________________________________________________________________ */

//? Maintaining appsettings across a real dev workflow

//# What actually belongs in appsettings.json (checked into git)?
// Non-sensitive, environment-shareable structure: logging categories, feature-flag names/defaults,
// third-party base URLs (not their keys), CORS-allowed-origins lists, pagination sizes, etc.

//# What should NEVER go in appsettings.json?
// Secrets: DB passwords embedded in a connection string, API keys, JWT signing keys, anything that would
// be bad if it leaked via git history. This project's DefaultConnection uses Trusted_Connection=true
// (Windows integrated auth, no password in the string) specifically so it's safe to commit — a
// SQL-auth connection string with a literal password would not be.

//# So where do secrets actually go, per environment?
// - Local development: `dotnet user-secrets init` then `dotnet user-secrets set "Key" "value"` — this
//   writes to a JSON file OUTSIDE the project folder (in your user profile), so it can never accidentally
//   get committed, and it's automatically merged into IConfiguration in the Development environment only.
// - Staging/Production: environment variables set by the hosting platform (IIS app pool env vars, Azure
//   App Service "Configuration" blade, Docker/K8s secrets, AWS Secrets Manager, Azure Key Vault). None of
//   these live in a file inside the repo at all.
// Environment variables map onto the same nested-key shape using double underscores instead of colons:
//   ConnectionStrings__DefaultConnection=... is the env-var equivalent of ConnectionStrings:DefaultConnection.

//# Why does appsettings.Development.json duplicate ConnectionStrings.DefaultConnection here if it's
//   pointing at the same localdb instance as the base file?
// In this project it's redundant right now (both point at the same localdb DB) — it was likely scaffolded
// by the template and never trimmed. In a team setting you'd normally either delete the duplicate (letting
// it fall through to the base file) or intentionally diverge it (e.g. Development uses a lightweight local
// DB, Production uses a real server) — a duplicate that's identical to the base file is dead weight worth
// cleaning up once noticed.

//# Practical workflow for adding a new setting
// 1. Add the key with a sensible default to appsettings.json (the base/shared file).
// 2. Override it in appsettings.{Environment}.json only if that environment genuinely needs a different
//    value.
// 3. If it's sensitive, don't put a real value in either file — use user-secrets locally and an env var /
//    secrets manager in deployed environments, and leave a placeholder or omit the key entirely in git.
// 4. Read it via a bound options class (IOptions<T>) if more than one place in the code needs it, or via
//    GetValue/GetConnectionString for a one-off read at startup (like the DbContext wiring).


/*_______________________________________________________________________________________________________________________________ */

//? What is Web.config, and how does it relate to appsettings.json?

// Web.config is an XML configuration file used by IIS (Internet Information Services) and the older
// ASP.NET (Framework, pre-.NET-Core) pipeline. It is NOT the same layer as appsettings.json — they solve
// different problems and, in a modern ASP.NET Core app, usually coexist for different reasons:
//
// - appsettings.json  -> application-level settings YOUR CODE reads (connection strings, log levels, etc.)
// - web.config         -> HOSTING-level settings IIS reads to know how to launch/manage your app process
//                         (it does not contain your app's business configuration in a Core app)

//# Do I have to hand-write Web.config for an ASP.NET Core app?
// No. When you run `dotnet publish` on an ASP.NET Core project targeting IIS hosting, the SDK
// auto-generates a web.config in the publish output folder. You don't author it by hand for Core apps —
// it's a build artifact, similar to how appsettings.json is a source file but the compiled DLLs in bin/
// are generated artifacts.

//# What actually goes inside the generated web.config for an ASP.NET Core app?
// A small XML file whose main job is telling IIS's ASP.NET Core Module (ANCM) how to start your app:
//
// <configuration>
//   <location path="." inheritInChildApplications="false">
//     <system.webServer>
//       <handlers>
//         <add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModuleV2" resourceType="Unspecified" />
//       </handlers>
//       <aspNetCore processPath="dotnet" arguments=".\OneFlowAPI.dll" stdoutLogEnabled="false"
//                   stdoutLogFile=".\logs\stdout" hostingModel="InProcess">
//         <environmentVariables>
//           <environmentVariable name="ASPNETCORE_ENVIRONMENT" value="Production" />
//         </environmentVariables>
//       </aspNetCore>
//     </system.webServer>
//   </location>
// </configuration>
//
// Key pieces:
// - <handlers>/aspNetCore: registers ANCM as the module that intercepts all requests ("*") and hands them
//   to your app instead of IIS trying to serve them as static files.
// - processPath/arguments: literally the command IIS runs to start your app — same as typing
//   `dotnet OneFlowAPI.dll` yourself in a terminal.
// - hostingModel="InProcess": runs your app inside the IIS worker process (w3wp.exe) for better performance
//   (the alternative, OutOfProcess, runs Kestrel as a separate process and IIS just reverse-proxies to it).
// - <environmentVariables>: this is how ASPNETCORE_ENVIRONMENT gets set in a real IIS deployment — the
//   deployed equivalent of the --environment flag or launchSettings.json profile used locally.

//# Role Web.config plays: what would break without it?
// Without it, IIS has no idea your published folder full of DLLs is even an application it should run —
// it would just try to serve the .dll files as static file downloads. web.config is the bridge that says
// "route every request into the ASP.NET Core process via this module," which is why the deploy steps noted
// elsewhere in this project's notes (see Program.cs's "How to deploy .Net app on IIS server?" section) call
// out web.config as one of the files you must copy to the IIS server alongside the compiled DLLs.

//# Does Web.config ever hold application settings too (like appsettings.json does)?
// In classic ASP.NET (.NET Framework, pre-Core, e.g. WebForms or old-style MVC), yes — that's actually
// where <appSettings> and <connectionStrings> XML sections used to live, and ConfigurationManager.AppSettings
// / ConfigurationManager.ConnectionStrings read from them. ASP.NET Core replaced that whole model with the
// IConfiguration/appsettings.json system described above. This project is ASP.NET Core, so its generated
// web.config is hosting-only — if you ever see <appSettings> inside a web.config in a DIFFERENT (older)
// project, that's the tell it's a .NET Framework app, not .NET Core/5+.

//# Can I customize the generated web.config (e.g. add custom IIS rewrite rules)?
// Yes — you can hand-edit web.config transforms via the .csproj (e.g. a web.config template checked into
// source, or MSBuild properties like <AspNetCoreHostingModel>) so `dotnet publish` merges your customizations
// into the generated file rather than overwriting them outright. Not something this project currently does,
// but worth knowing it's not strictly "hands off" if a real IIS deployment needs custom rules (URL rewrite,
// custom headers, IP restrictions at the IIS level instead of in middleware).


/*_______________________________________________________________________________________________________________________________ */

//? launchSettings.json — the third config file, often confused with the other two

// Properties/launchSettings.json is DEVELOPMENT-ONLY and never gets published/deployed — it's read by
// `dotnet run` / Visual Studio's debugger to decide what URL and environment variables to use for a given
// launch profile (this project has "http" and "https" profiles, per the notes in Program.cs). It's the
// mechanism behind commands like `dotnet run --launch-profile https`.
//
// Quick contrast of the three config files this project actually has:
//   appsettings.json / appsettings.{Env}.json  -> ships with the app, read by IConfiguration at runtime,
//                                                 controls app behavior (logging, connection strings, ...)
//   web.config (generated on publish)           -> ships with the app, read by IIS/ANCM, controls how the
//                                                 process gets launched under IIS
//   launchSettings.json                         -> local dev only, never published, read by the dotnet CLI/
//                                                 IDE, controls how `dotnet run` launches the app on your
//                                                 own machine (ports, env vars for that run)


/*_______________________________________________________________________________________________________________________________ */

//? Quick mental model to tie it all together

// "What port does my app run on, and in what environment, when I hit F5 locally?"
//   -> launchSettings.json decides this.
//
// "What connection string / log level does my running app actually use?"
//   -> appsettings.json, layered with appsettings.{ASPNETCORE_ENVIRONMENT}.json, layered with any env vars
//      or user-secrets — merged into builder.Configuration.
//
// "How does IIS even know to start my app and route requests to it in production?"
//   -> web.config (auto-generated by `dotnet publish`), read by IIS's ASP.NET Core Module.

//TODO: Try setting an environment variable that overrides a nested appsettings key
// (ConnectionStrings__DefaultConnection=... before `dotnet run`) and confirm via a breakpoint/log that
// builder.Configuration.GetConnectionString("DefaultConnection") picks up the env var instead of the
// value in appsettings.Development.json — this is the layering rule ("later source wins") in action.
