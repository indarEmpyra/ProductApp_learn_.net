using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ProductApp.Middleware
{
  public class RequestAuthenticationMiddleware
  {
    private readonly RequestDelegate _next;

    public RequestAuthenticationMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
      if (!context.Request.Headers.TryGetValue("Authorization", out var authorizationHeader)
        || string.IsNullOrWhiteSpace(authorizationHeader))
      {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        await context.Response.WriteAsync("Unauthorized");
        return;
      }

      var token = authorizationHeader.ToString().StartsWith("Bearer ")
        ? authorizationHeader.ToString().Substring("Bearer ".Length).Trim()
        : authorizationHeader.ToString().Trim();

      if (!IsValidToken(token))
      {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        await context.Response.WriteAsync("Unauthorized");
        return;
      }

      await _next(context);
    }

    private bool IsValidToken(string token)
    {
      // Replace this with your actual token validation logic.
      return !string.IsNullOrWhiteSpace(token) && token == "your-valid-token";
    }
  }

  public static class RequestAuthenticationMiddlewareExtensions
  {
    public static IApplicationBuilder UseRequestAuthentication(this IApplicationBuilder builder)
    {
      return builder.UseMiddleware<RequestAuthenticationMiddleware>();
    }
  }
}