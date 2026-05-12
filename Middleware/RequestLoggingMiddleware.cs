namespace ProductApp.Middleware
{
  public class RequestLoggingMiddleware
  {
    private readonly RequestDelegate _next;
    // The next middleware in the pipeline
    // Why do we need _next here? 
    // Because we want to ensure that after logging the incoming request, we allow the request to proceed to the next middleware in the pipeline.
    // If we don't call _next(context), the request will not continue, and the response will not be generated, resulting in a stalled request.

    public RequestLoggingMiddleware(RequestDelegate next)
    {
      _next = next;
      // This middleware logs incoming HTTP requests and outgoing responses to the console.
      // next is the next middleware in the pipeline, which will be invoked after logging the request and before logging the response.
      // If we don't call _next(context), the request will not proceed to the next middleware, and the response will not be generated, resulting in a stalled request.
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
  }
}