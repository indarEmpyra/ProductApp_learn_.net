
using System.Net;
using System.Text.Json;

public class ExceptionMiddleware
{
  private readonly RequestDelegate _next;

  public ExceptionMiddleware(RequestDelegate next)
  {
    _next = next;
  }
  //? Does this mean that the constructor will receive the next middleware in the pipeline as a parameter when the middleware is registered in the Startup.cs file?
  // Yes, when you register the ExceptionMiddleware in the Startup.cs file using app.UseMiddleware<ExceptionMiddleware>();


  //? We define HandleException after the Invoke method, how does the Invoke method know about the HandleException method?
  // In C#, the order of method definitions within a class does not affect their visibility to each other. 
  // The Invoke method can call the HandleException method regardless of their order in the class definition.

  //? when does the Invoke method get called?
  // The Invoke method is called for each HTTP request that passes through the middleware. When a request is made to the server, it enters the 
  // middleware pipeline, and each middleware component's Invoke method is executed in sequence.
  // The Invoke method is responsible for processing the incoming request and generating a response. 
  // If an exception occurs during the processing of the request, it is caught in the catch block, and the HandleException 
  // method is called to handle the error and generate an appropriate response for the client.  


  public async Task Invoke(HttpContext context)
  {
    try
    {
      await _next(context); // next middleware / controller
    }
    catch (Exception ex)
    {
      await HandleException(context, ex);
    }
  }

  private Task HandleException(HttpContext context, Exception ex)
  {
    context.Response.ContentType = "application/json";
    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

    var response = new
    {
      message = ex.Message,
      statusCode = context.Response.StatusCode
    };

    var json = JsonSerializer.Serialize(response);
    // JsonSerializer is a built-in class in .NET that provides functionality for serializing and deserializing objects to and from JSON format.
    // JsonSerializer.Serialize is used to convert the response object into a JSON string, which can be sent back to the client as part of the HTTP response.
    // JsonSerializer.Deserialize is used to convert a JSON string back into an object, while JsonSerializer.Serialize is used to convert an object into a JSON string.

    return context.Response.WriteAsync(json);
    // context.Response.WriteAsync is an asynchronous method that writes the JSON string to the response body.
    // This method is used to send the JSON response back to the client after an exception has been handled. 
    // The client will receive a JSON object containing the error message and status code, allowing it to understand what went wrong with the request.  
  }
}


// Node Equivalent
// app.use((req,res,next)=>{
//    console.log("middleware 1")
//    next()
// })

//# 🧠 Middleware Signature
// public async Task Invoke(HttpContext context)
// {
//     await _next(context);
// }

//# The ExceptionMiddleware is designed to catch any unhandled exceptions that occur during the processing of an HTTP request. 
// It uses a try-catch block to catch exceptions thrown by the next middleware or controller in the pipeline. 
// If an exception is caught, the HandleException method is called to log the error details and generate a JSON response with the error message and status code. 
// This middleware ensures that any unhandled exceptions are properly logged and that the client receives a consistent error response.

//# Explain code:
// The ExceptionMiddleware class is a custom middleware component that handles exceptions that occur during the processing of HTTP requests. 
// It has a constructor that takes a RequestDelegate, which represents the next middleware in the pipeline

//# The Invoke method is called for each HTTP request that passes through the middleware. 
// It uses a try-catch block to catch any exceptions that occur during the processing of the request. If an exception is caught, 
// it calls the HandleException method to handle the error and generate a response for the client.
// The HandleException method sets the response content type to "application/json" and the status code to 500 (Internal Server Error). 
// It then creates an anonymous object containing the error message and status code, serializes it to JSON, 
// and writes it to the response body. This ensures that the client receives a consistent error response in case of any unhandled exceptions.

