//? What are .dll files?
// .dll files are Dynamic Link Libraries, which are files that contain code and data that can be used by multiple programs simultaneously. 
// They allow for code reuse and modular programming, as they can be shared across different applications without the need for duplication. 
// .dll files can contain functions, classes, and resources that can be accessed by other programs, making them an 
// essential part of the Windows operating system and many software applications.  

//? How to deploy a .dll file?
// To deploy a .dll file, you can follow these steps:
// 1. Build the .dll file: First, you need to compile your code into a .dll file using a development environment like Visual Studio or a command-line compiler.
// 2. Copy the .dll file: Once you have the .dll file, you need to copy it to the appropriate location on the target machine. 
// This could be the same directory as the executable that will use the .dll, or a system directory like C:\Windows\System32 for global access.
// 3. Register the .dll file (if necessary): If the .dll file contains COM components, you may need to register it using the regsvr32 
// command in the command prompt. For example, you can run "regsvr32 yourfile.dll" to register the .dll file.
// 4. Update the application configuration: If your application relies on the .dll file, you may need to update the application's configuration file 
// (e.g., app.config or web.config) to reference the .dll file correctly.
// 5. Test the deployment: Finally, you should test the application to ensure that it can successfully load and use the .dll file without any issues. 
// This may involve running the application and verifying that the functionality provided by the .dll is working as expected.  

//? What are the advantages of using .dll files?
// The advantages of using .dll files include:  
// 1. Code Reusability: .dll files allow developers to reuse code across multiple applications, reducing redundancy and improving efficiency.
// 2. Modular Programming: .dll files enable developers to break down complex applications into smaller, more manageable modules, making it easier 
// to maintain and update the codebase.
// 3. Memory Efficiency: Since .dll files can be shared among multiple applications, they can help reduce memory usage by allowing multiple 
// programs to use the same code and resources without duplication.
// 4. Faster Load Times: .dll files can be loaded into memory only when needed, which can improve the startup time of applications that rely on them.
// 5. Easier Updates: When a .dll file is updated, all applications that use that .dll can benefit from the update without needing to be recompiled, 
// as long as the interface remains consistent. This allows for easier maintenance and bug fixes. 
// 6. Language Interoperability: .dll files can be created using different programming languages, allowing for interoperability between applications 
// written in different languages. This can facilitate integration and collaboration between teams using different technologies.  
// 7. Security: .dll files can be signed with a digital signature to ensure their authenticity and integrity, providing an additional layer of 
// security for applications that rely on them. This helps prevent unauthorized modifications and ensures that the .dll file has not been tampered with.  

//? What are the disadvantages of using .dll files?
// The disadvantages of using .dll files include:
// 1. Dependency Issues: If a .dll file is missing or not properly registered, applications that rely on it may fail to run, leading to errors and crashes.
// 2. Versioning Problems: If multiple applications use different versions of the same .dll file, it can lead to compatibility issues 
// and conflicts, often referred to as "DLL Hell."
// 3. Security Risks: If a .dll file is compromised or contains malicious code, it can pose a security risk to applications that use it, 
// potentially leading to data breaches or system vulnerabilities.
// 4. Performance Overhead: Loading and using .dll files can introduce performance overhead, especially if the .dll file is large or 
// contains complex code, which can slow down the application that relies on it.
// 5. Complexity in Deployment: Deploying .dll files can be complex, especially when dealing with multiple applications and different 
// versions of the same .dll file, requiring careful management and testing to ensure compatibility and stability. 
// 6. Limited Debugging: Debugging issues related to .dll files can be challenging, especially if the source code for the .dll is not available, 
// making it difficult to identify and resolve problems that may arise from using the .dll file.  

//? What are some common use cases for .dll files?
// Some common use cases for .dll files include:  
// 1. Shared Libraries: .dll files are often used to create shared libraries that can be used by multiple applications, 
// allowing for code reuse and modular programming.
// 2. Plugin Systems: .dll files can be used to create plugin systems, where applications can dynamically load and use additional functionality 
// provided by .dll files without needing to be recompiled.
// 3. Device Drivers: .dll files are commonly used in the development of device drivers, allowing for communication between the operating system and hardware devices.
// 4. COM Components: .dll files can be used to create COM (Component Object Model) components, which can be used for inter-process communication 
// and integration between different applications.
// 5. Game Development: .dll files are often used in game development to create reusable libraries for graphics, physics, and other game-related 
// functionality, allowing for efficient development and maintenance of game code.
// 6. Web Development: .dll files can be used in web development to create reusable libraries for server-side functionality, such as data access, 
// authentication, and business logic, allowing for modular and maintainable web applications.   

//? Is the .dll files only way to deploy a Net app?
// No, .dll files are not the only way to deploy a .NET application. In addition to .dll files, you can also deploy a .NET application using 
// executable (.exe) files, which contain the main entry point of the application. Additionally, you can use other deployment methods such as 
// ClickOnce, which allows for easy installation and updates of .NET applications, or you can create a self-contained deployment that includes 
// all necessary dependencies within a single package. The choice of deployment method depends on the specific requirements of your application and the target environment.    

//? How to deploy a .NET application using .dll files?
// To deploy a .NET application using .dll files, you can follow these steps:
// 1. Build the application: First, you need to compile your .NET application into a .dll file using a development environment like Visual Studio or a command-line compiler.
// 2. Copy the .dll file: Once you have the .dll file, you need to copy it to the appropriate location on the target machine. This could be the same directory as the executable that will use the .dll, or a system directory like C:\Windows\System32 for global access.
// 3. Register the .dll file (if necessary): If the .dll file contains COM components, you may need to register it using the regsvr32 command in the command prompt. For example, you can run "regsvr32 yourfile.dll" to register the .dll file.
// 4. Update the application configuration: If your application relies on the .dll file, you may need to update the application's configuration file (e.g., app.config or web.config) to reference the .dll file correctly.
// 5. Test the deployment: Finally, you should test the application to ensure that it can successfully load and use the .dll file without any issues. This may involve running the application and verifying that the functionality provided by the .dll is working as expected.  

//# Can we see step-by-step code example of deploying a .NET application using .dll files on a local IIS/IIsExpress server?
// Sure! Here is a step-by-step code example of deploying a .NET application using .dll files on a local IIS/IIS Express server:
// Step 1: Create a .NET application and build it to generate the .dll file.
// For example, you can create a simple ASP.NET Core web application and build it to generate the .dll file. In Visual Studio, 
// you can right-click on the project and select "Build" to generate the .dll file in the output directory (e.g., bin\Debug\netcoreapp3.1\).

// Step 2: Copy the .dll file to the appropriate location on the target machine.
// For example, you can copy the generated .dll file to a directory on your local machine, such as C:\MyApp\bin\. 
// Why bin? Because the .dll file is typically placed in the same directory as the executable that will use it, and in this case, the 
// executable is the web application hosted on IIS/IIS Express.  

// Step 3: Set up IIS/IIS Express to host the application.
// You can create a new site in IIS/IIS Express and point it to the directory where you copied the .dll file. Make sure to configure the 
// application pool to use the correct .NET runtime version.  

// Step 4: Update the application configuration (if necessary).
// If your application relies on the .dll file, you may need to update the application's configuration file (e.g., appsettings.json or web.config) to 
// reference the .dll file correctly. For example, you may need to specify the path to the .dll file in the configuration settings.

// Step 5: Test the deployment. 
// Finally, you should test the application to ensure that it can successfully load and use the .dll file without any issues. You can do this by 
// navigating to the URL of your application in a web browser (e.g., http://localhost:5000/) and verifying that the functionality provided by the .dll is 
// working as expected. If there are any issues, you can check the IIS/IIS Express logs for error messages and troubleshoot accordingly. 


/*_______________________________________________________________________________________________________________________________ */



//? Mental Model

// Deploying a .NET app to IIS is similar to:

// Build your application.
// Copy the published files to the server.
// Create/configure an IIS website.
// Tell IIS how to run the application.
// Ensure the required .NET runtime exists on the server.

// Think of IIS as the front door/reverse proxy and your ASP.NET Core application as the actual application process.

// Browser
//    |
//    v
//  IIS
//    |
//    v
// ASP.NET Core App (Kestrel)
//    |
//    v
// Database


//# Step 1: Install .NET Runtime on Server

// On the IIS server, install:

// .NET Hosting Bundle (recommended)
// This includes:
// .NET Runtime
// ASP.NET Core Runtime
// IIS Integration Module

// For example, if your application targets:

// <TargetFramework>net8.0</TargetFramework>

// Install the .NET 8 Hosting Bundle on the server.

// Verify:
// dotnet --info


//# Step 2: Publish the Application

// From Visual Studio:

// Right Click Project
// Publish
// Folder
// Publish -> Folder

// Output example:

// bin\Release\net8.0\publish\

// or CLI:

// dotnet publish -c Release

// Output:

// bin/Release/net8.0/publish/

// You will see files like:

// MyApp.dll
// appsettings.json
// web.config
// wwwroot/


//# Step 3: Copy Files to IIS Server

// Create a folder:

// D:\Websites\MyApp

// Copy all published files:

// D:\Websites\MyApp
//     MyApp.dll
//     web.config
//     appsettings.json
//     wwwroot

//# Step 4: Create Application Pool
// An application pool is a container for your web applications in IIS. It provides isolation and manages the resources for your application.

// Can we use the DefaultAppPool? Yes, but it's best practice to create a separate application pool for each application for better isolation and security.

// Can we manage without an application pool? No, IIS requires an application pool to run web applications. Each website in IIS must be assigned to an application pool, 
// which provides the necessary environment for the application to run. 

// Open IIS Manager.

// Application Pools
// Add Application Pool

// Example:

// Name: MyAppPool
// .NET CLR Version: No Managed Code
// Managed Pipeline: Integrated
// Why "No Managed Code"?

// ASP.NET Core runs in its own process.

// IIS simply forwards requests.

//# Step 5: Create Website
// Now we create a website in IIS that points to our application files and uses the application pool we just created.
// In IIS:

// Sites
//    -> Add Website

// Example:

// Site Name:
// MyApp

// Physical Path:
// D:\Websites\MyApp

// Port:
// 8080

// What port to choose?
// Any unused port will work. Common choices are 8080, 5000, etc. Avoid ports below 1024 as they require admin privileges.

// Assign:

// Application Pool:
// MyAppPool

//# Step 6: Give Folder Permissions

// The IIS App Pool identity needs access.

// Example:

// D:\Websites\MyApp

// Grant:

// IIS AppPool\MyAppPool

// Permissions:

// Read
// Execute
// Write (if logs/uploads required)

//# Step 7: Start Website

// Browse:

// http://server-ip:8080

// or

// http://localhost:8080

// Application should load.

// Understanding web.config

// When you publish an ASP.NET Core app, a web.config is usually generated:

// <aspNetCore
//     processPath="dotnet"
//     arguments=".\MyApp.dll"
//     stdoutLogEnabled="false"
//     stdoutLogFile=".\logs\stdout" />

// This tells IIS:

// Run:
// dotnet MyApp.dll

// behind the scenes.

// Framework-Dependent vs Self-Contained Deployment

// Framework-Dependent

// Publish:

// dotnet publish -c Release

// Server needs:

// .NET Runtime Installed

// Deployment size:

// Small
// Self-Contained

// Publish:

// dotnet publish -c Release -r win-x64 --self-contained true

// Output contains:

// dotnet runtime
// your app
// all dependencies

// Server does NOT need .NET installed.

// Deployment size:

// Large

// Common for customer-managed servers.

//? Common IIS Errors
//#1 HTTP Error 500.30

// Meaning:

// App failed to start

// Check:

// dotnet MyApp.dll

// from command prompt.

// Usually:

// Missing config
// Missing runtime
// Startup exception

//#2 HTTP Error 500.31

// Meaning:

// Required runtime missing

// Install correct hosting bundle.

//#3 HTTP Error 502.5

// Meaning:

// Process crashed during startup

// Enable stdout logging.

// stdoutLogEnabled="true"

// Create:

// logs/

// folder and inspect logs.

// Typical Production Deployment Flow

// For the deployment system you're building with Jenkins:

// Developer
//     |
//     v
// Bitbucket
//     |
//     v
// Jenkins
//     |
//     v
// dotnet publish
//     |
//     v
// Copy publish folder
//     |
//     v
// IIS Server
//     |
//     v
// Recycle App Pool

// Example Jenkins commands:

// dotnet restore
// dotnet build -c Release
// dotnet publish -c Release -o PublishOutput

// Then:

// xcopy PublishOutput D:\Websites\MyApp /E /Y

// Finally:

// Restart-WebAppPool MyAppPool

// This is the deployment pattern you'll see in many enterprise IIS-hosted ASP.NET Core applications.



/*______________________________________________________________________________________________________________________________ */


//? Deploying an ASP.NET Core application to Azure is actually simpler than deploying to IIS because Azure manages much of the infrastructure for you.

// Mental Model

// Think of it like this:

// Your Code
//     |
//     v
// dotnet publish
//     |
//     v
// Azure App Service
//     |
//     v
// Kestrel + IIS (managed by Azure)
//     |
//     v
// Users

// You don't need to:

// Install IIS
// Configure Application Pools
// Configure Hosting Bundle
// Manage Windows Server patches

// Azure App Service handles these for you.

// Option 1: Deploy from Visual Studio (Easiest)
// Step 1: Create Azure App Service

// In Azure Portal:

// Create Resource
// Web App
// Fill:
// Subscription: Your Subscription
// Resource Group: MyRG
// Name: myapp-prod
// Runtime Stack: .NET 8
// Region: Central India

// Create.

// Step 2: Publish from Visual Studio

// Right-click project:

// Publish

// Choose:

// Azure

// Select:

// Azure App Service

// Sign in and choose:

// myapp-prod

// Click:

// Publish

// Visual Studio will:

// Build
// Publish
// Zip
// Upload
// Deploy

// Automatically.

// Option 2: Deploy Using Azure CLI

// Publish locally:

// dotnet publish -c Release

// Create zip:

// zip -r app.zip publish/*

// Deploy:

// az webapp deployment source config-zip \
//   --resource-group MyRG \
//   --name myapp-prod \
//   --src app.zip
// Option 3: Deploy via GitHub Actions (Recommended)

// This is the modern approach.

// Developer
//     |
//     v
// GitHub
//     |
//     v
// GitHub Actions
//     |
//     v
// Azure App Service

// Every push to:

// main

// automatically deploys.

// Example workflow:

// name: Build and Deploy

// on:
//   push:
//     branches:
//       - main

// jobs:
//   build:
//     runs-on: ubuntu-latest

//     steps:
//     - uses: actions/checkout@v4

//     - uses: actions/setup-dotnet@v4
//       with:
//         dotnet-version: 8.0.x

//     - run: dotnet restore
//     - run: dotnet build --configuration Release
//     - run: dotnet publish -c Release -o publish

//     - uses: azure/webapps-deploy@v3
//       with:
//         app-name: myapp-prod
//         package: publish
// Option 4: Deploy via Azure DevOps

// Many enterprises use:

// Azure Repos
// Azure Pipelines
// Azure App Service

// Pipeline:

// Build
//     ↓
// Publish Artifacts
//     ↓
// Release Pipeline
//     ↓
// Azure App Service

// Very similar to Jenkins.

// App Settings

// Instead of editing:

// appsettings.Production.json

// on the server, use:

// Azure Portal
//   ↓
// App Service
//   ↓
// Environment Variables

// Example:

// ConnectionStrings__DefaultConnection
// Server=mydb.database.windows.net;
// Database=MyDb;
// User Id=...
// Password=...

// ASP.NET Core automatically reads these.

// Database Deployment

// Typical setup:

// ASP.NET Core App
//       |
//       v
// Azure SQL Database

// Connection string:

// {
//   "ConnectionStrings": {
//     "DefaultConnection": ""
//   }
// }

// Stored securely in App Service settings.

// Logs & Troubleshooting

// Enable:

// App Service
//     ↓
// Monitoring
//     ↓
// App Service Logs

// View logs:

// Log Stream

// in Azure Portal.

// Useful for startup errors.

// Deployment Slots (Very Important)

// Production deployments often use slots.

// Example:

// Production
// Staging

// Flow:

// Deploy → Staging
// Test
// Swap
// Production

// Benefits:

// Zero downtime
// Easy rollback
// Safe releases

// Example:

// myapp-staging.azurewebsites.net

// After testing:

// Swap

// Azure instantly switches traffic.

// Custom Domain

// Default URL:

// https://myapp-prod.azurewebsites.net

// Custom:

// https://api.mycompany.com

// Configure:

// Custom Domains

// inside App Service.

// Azure can automatically manage SSL certificates for many scenarios.

// CI/CD Flow Similar to Your Jenkins Experience

// Since you've worked with Jenkins, Bitbucket, and deployment automation, a typical Azure production flow would be:

// Developer
//     |
//     v
// Bitbucket/GitHub
//     |
//     v
// Pipeline (Jenkins/Azure DevOps/GitHub Actions)
//     |
//     v
// dotnet publish
//     |
//     v
// Package Artifact
//     |
//     v
// Azure App Service (Staging Slot)
//     |
//     v
// Smoke Test
//     |
//     v
// Swap to Production

// This is one of the most common enterprise deployments for ASP.NET Core applications on Azure.