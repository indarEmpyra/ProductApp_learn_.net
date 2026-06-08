namespace ProductApp.Middleware
{
  public class RequestLoggingMiddleware
  {
    private readonly RequestDelegate _next;
    // The next middleware in the pipeline is represented by the RequestDelegate type, which is a delegate that can process an HTTP request.
    // The _next field is used to store the reference to the next middleware, allowing the current middleware to call it and pass the HttpContext along the pipeline.

    // Why do we need _next here? 
    // Because we want to ensure that after logging the incoming request, we allow the request to proceed to the next middleware in the pipeline.
    // If we don't call _next(context), the request will not continue, and the response will not be generated, resulting in a stalled request.

    public RequestLoggingMiddleware(RequestDelegate next)
    {
      _next = next;
      // The constructor of the RequestLoggingMiddleware class takes a RequestDelegate as a parameter, which represents the next middleware in the pipeline.
      // This allows the middleware to call the next middleware in the pipeline after it has finished processing the current request.
      // This middleware logs incoming HTTP requests and outgoing HTTP responses. 
      // It uses the HttpContext object to access the request and response details, such as the HTTP method, request path, and response status code. 
      // next is the next middleware in the pipeline, which will be invoked after logging the request and before logging the response.
      // If we don't call _next(context), the request will not proceed to the next middleware, and the response will not be generated, resulting in a stalled request.

      //? Middleware being able to read request is fine because when request comes in, it goes through the middleware pipeline 
      //? but how does it get to know of the response?
      // The middleware can access the response through the HttpContext object, which is passed to the InvokeAsync method. 
      // After the request has been processed by the next middleware and the controller, the Response property of the HttpContext 
      // will contain the details of the outgoing response, such as the status code and any response body. 

      // In the RequestLoggingMiddleware, after calling await _next(context), the middleware can access context.Response to log the status code 
      // of the outgoing response.

      // RequestLoggingMiddleware flows like this:
      // Client → RequestLoggingMiddleware → Other Middleware → Controller → Other Middleware → RequestLoggingMiddleware

      //? How does the RequestLoggingMiddleware get called again after the controller processes the request?
      // The RequestLoggingMiddleware is part of the middleware pipeline, which means that after the controller processes the 
      // request and generates a response, the response travels back through the middleware pipeline in reverse order.
      // So, after the controller processes the request, the response goes back through the middleware pipeline, and the RequestLoggingMiddleware 
      // can log the outgoing response status code before the response is sent back to the client. 

      //? what actually ejects the response back to the client and who sends it?
      // The response is sent back to the client by the ASP.NET Core framework itself. 
      // After the controller generates a response, the ASP.NET Core framework takes care of sending that response back to the client. 
      // The middleware can modify the response or log details about it, but the actual sending of the response is handled by the framework.

      // Request <- Middleware 1 <-> Middleware 2 <-> Controller <-> Middleware 2 <-> Middleware 1 <- Response flows in both directions, 
      // but the actual sending of the response is handled by the ASP.NET Core framework after it has passed through 
      // all the middleware and the controller.

      //? What receives the request, processes it and sends the response back to the client in ASP.NET Core?
      // In ASP.NET Core, the Kestrel web server is responsible for receiving incoming HTTP requests from clients.
      // When a request is received, Kestrel passes it to the ASP.NET Core middleware pipeline for processing.
      // The middleware components in the pipeline can inspect and modify the request and response as needed.
      // After the request has been processed by the middleware and the controller, 
      // the ASP.NET Core framework takes care of sending the response back to the client through Kestrel. 

      //? What is the role of Kestrel in ASP.NET Core?
      // Kestrel is a cross-platform web server for ASP.NET Core applications.
      // It is designed to be fast and efficient, and it can handle a large number of concurrent connections.
      // Kestrel is responsible for receiving incoming HTTP requests from clients and passing them to the ASP.NET Core middleware pipeline for processing.
      // After the request has been processed by the middleware and the controller, Kestrel is responsible for sending the response back to the client.


      //? How to pass data from one middleware to another?
      // You can use the HttpContext.Items property to pass data between middleware components.
      // HttpContext.Items is a key-value collection that allows you to store and retrieve data during the processing of an HTTP request.
      // For example, in one middleware, you can set a value like this: 
      // context.Items["UserId"] = userId;
      // Then, in another middleware later in the pipeline, you can retrieve that value like this:
      // var userId = context.Items["UserId"];
      // This allows you to share data between middleware components without using global state or other external storage mechanisms.


      //? Can middleware modify the response body?
      // Yes, middleware can modify the response body by using the HttpContext.Response.Body property, which is a Stream that represents the response body.
      // To modify the response body, you can create a new MemoryStream, set it as the Response.Body, and write your modified content to that stream.
      // For example:
      // var originalBodyStream = context.Response.Body;
      // using (var modifiedBodyStream = new MemoryStream())
      // {
      //     context.Response.Body = modifiedBodyStream;
      //     await _next(context); // Call the next middleware to generate the response
      //     modifiedBodyStream.Seek(0, SeekOrigin.Begin);
      //     var modifiedContent = new StreamReader(modifiedBodyStream).ReadToEnd();
      //     // Modify the content as needed
      //     var modifiedBytes = Encoding.UTF8.GetBytes(modifiedContent);
      //     await originalBodyStream.WriteAsync(modifiedBytes, 0, modifiedBytes.Length);
      // }

      //? If authentication middle checks if the request is valid and wants to fetch user details and 
      //? pass it on to other further middlewares, how does it do that?
      // The authentication middleware can fetch user details and pass them to other middleware components using the HttpContext.Items property.
      // For example, after validating the token and fetching user details, the authentication middleware can set the user 
      // information in the HttpContext.Items collection like this:
      // context.Items["User"] = userDetails; 
      // Then, in other middleware components later in the pipeline, you can retrieve the user details like this:
      // var userDetails = context.Items["User"];
      // This allows the authentication middleware to share user information with other middleware 
      // components without using global state or other external storage mechanisms.  
    }

    public async Task InvokeAsync(HttpContext context)
    // The InvokeAsync method is called for each HTTP request that passes through the middleware. 
    // It takes an HttpContext object as a parameter, which contains information about the incoming request and the outgoing response.
    // context: The HttpContext object provides access to the request and response details, such as the HTTP method, request path, and response status code.
    // The method is asynchronous because it may involve awaiting the next middleware in the pipeline, which could be performing I/O operations or other asynchronous tasks.
    {
      Console.WriteLine($"Incoming request: {context.Request.Method} {context.Request.Path}");
      await _next(context);
      Console.WriteLine($"Outgoing response: {context.Response.StatusCode}");
    }

    //? Why only InvokeAsync method of the RequestLoggingMiddleware class get called? How does the framework know that this method needs to get called ?
    // The framework knows to call the InvokeAsync method because it is a convention used by ASP.NET Core middleware. 
    // When you add a middleware to the pipeline using app.UseMiddleware<RequestLoggingMiddleware>(), 
    // the framework automatically looks for a method named Invoke or InvokeAsync and calls it for each HTTP request.

    // This happens because the middleware is registered in the Startup.cs file, and the framework uses reflection to find the 
    // appropriate method to call when processing requests.

    //? Can I have multiple methods in the middleware class and how does the framework know which method to call?
    // Yes, you can have multiple methods in the middleware class, but the framework will only call the method named Invoke or InvokeAsync when processing HTTP requests.
    // The other methods in the class can be helper methods that are called from within the 
    // Invoke or InvokeAsync method, but they will not be called directly by the framework when processing requests. 

    //$ This happens for all middlewares.
  }
}


//? when does the Invoke method get called?
// The Invoke method is called for each HTTP request that passes through the middleware. When a request is made to the server, it enters the 
// middleware pipeline, and each middleware component's Invoke method is executed in sequence.
// The Invoke method is responsible for processing the incoming request and generating a response. 
// If an exception occurs during the processing of the request, it is caught in the catch block, and the HandleException 
// method is called to handle the error and generate an appropriate response for the client.  


//? How does the middleware get to know about the incoming request and outgoing response or the errors occurring in controllers, services, entity or anywhere?
// The middleware gets this information through the HttpContext object, which is passed to the InvokeAsync method. 
// The HttpContext contains details about the request, response, and any exceptions that occur during the processing of the request.


// Full flow of a request through the middleware pipeline:
// 1. A client sends an HTTP request to the server.
// 2. The request enters the middleware pipeline, where each middleware component can inspect and modify the request and response.
// 3. The RequestLoggingMiddleware logs the incoming request details (HTTP method and path) and then calls the next middleware in the pipeline using _next(context).
// 4. The request continues through the pipeline, potentially passing through other middleware components (e.g., authentication, authorization, exception handling) and eventually reaches the controller that handles the request.
// 5. The controller processes the request and generates a response, which then travels back through the middleware pipeline in reverse order.
// 6. The RequestLoggingMiddleware logs the outgoing response status code before the response is sent back to the client.

//? How does the middleware get to know about the errors occurring in controllers, services, entity or anywhere?
// The middleware can get to know about errors through the HttpContext object, which contains information about any exceptions 
// that occur during the processing of the request. If an exception occurs in a controller, service, or any other part of the request processing 
// pipeline, it can be caught and handled by an exception handling middleware (like the ExceptionMiddleware shown earlier). 
// This middleware can log the error details and generate an appropriate response for the client. 
// The RequestLoggingMiddleware itself does not directly handle exceptions, but it can be used in conjunction with an exception 
// handling middleware to ensure that all aspects of the request and response are logged, including any errors that occur.

//? How does the middleware get to know about the outgoing response?
// The middleware gets to know about the outgoing response through the HttpContext object, which contains a Response property. 
// After the request has been processed by the next middleware and the controller, 
// the Response property of the HttpContext will contain the details of the outgoing response, such as the status code and any response body.   

// In the RequestLoggingMiddleware, after calling await _next(context), the middleware can access "context.Response" to log the status
//  code of the outgoing response.


//# How to write exception handling middleware?
// To write an exception handling middleware, you can create a new class that implements the middleware pattern. 
// This class will have a constructor that takes a RequestDelegate and an Invoke method that handles the incoming HTTP request. 
// Inside the Invoke method, you can use a try-catch block to catch any exceptions that occur during the processing of the request. 
// If an exception is caught, you can log the error details and generate an appropriate response for the client.



//# Explain this flow:
// Request <- Middleware 1 <-> Middleware 2 <-> Controller <-> Middleware 2 <-> Middleware 1 <- Response
// In this flow, the incoming HTTP request first passes through Middleware 1, which can inspect and modify the request as needed.
// Then, the request continues to Middleware 2, which can also inspect and modify the request. After passing through Middleware 2, the request reaches the Controller, which processes the request and generates a response.
// After the Controller generates the response, it travels back through Middleware 2, allowing it to inspect and modify the response if necessary. Finally, the response goes back through Middleware 1, which can also inspect and modify the response before it is sent back to the client.
// This flow demonstrates how middleware components can interact with both the incoming request and the outgoing response, allowing for a flexible and powerful way to handle various aspects of HTTP request processing in an ASP.NET Core application.  
