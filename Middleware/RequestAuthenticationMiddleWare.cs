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



// Client → Login API → JWT Token
//        ↓
// Client sends token in header
//        ↓
// Middleware validates token
//        ↓
// Controller runs (if valid)


//# Step 1: Install JWT Package
// dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer

//# 🧱 Step 2: Add JWT Settings

// In appsettings.json:

// "Jwt": {
//   "Key": "THIS_IS_SUPER_SECRET_KEY_123",
//   "Issuer": "TaskManagerAPI",
//   "Audience": "TaskManagerUsers"
// }

//# Step 3: Configure Authentication in Program.cs
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.IdentityModel.Tokens;
// using System.Text;


// Add this BEFORE builder.Build()
// var jwtSettings = builder.Configuration.GetSection("Jwt");
// var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

// builder.Services.AddAuthentication(options =>
// {
//     options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//     options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
// })
// .AddJwtBearer(options =>
// {
//     options.TokenValidationParameters = new TokenValidationParameters
//     {
//         ValidateIssuer = true,
//         ValidateAudience = true,
//         ValidateIssuerSigningKey = true,
//         ValidateLifetime = true,

//         ValidIssuer = jwtSettings["Issuer"],
//         ValidAudience = jwtSettings["Audience"],
//         IssuerSigningKey = new SymmetricSecurityKey(key)
//     };
// });


// Add middleware:
// app.UseAuthentication();
// app.UseAuthorization();

//# ⚠️ Order matters:

// UseAuthentication → UseAuthorization → MapControllers



//# Step 4: Create Login API

// Create AuthController:

// using Microsoft.AspNetCore.Mvc;
// using Microsoft.IdentityModel.Tokens;
// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using System.Text;

// [ApiController]
// [Route("auth")]
// public class AuthController : ControllerBase
// {
//   private readonly IConfiguration _config;

//   public AuthController(IConfiguration config)
//   {
//     _config = config;
//   }

//   [HttpPost("login")]
//   public IActionResult Login()
//   {
//     // Hardcoded user (for now)
//     var claims = new[]
//     {
//             new Claim(ClaimTypes.Name, "admin"),
//             new Claim(ClaimTypes.Role, "Admin")
//         };

//     var key = new SymmetricSecurityKey(
//         Encoding.UTF8.GetBytes(_config["Jwt:Key"])
//     );

//     var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//     var token = new JwtSecurityToken(
//         issuer: _config["Jwt:Issuer"],
//         audience: _config["Jwt:Audience"],
//         claims: claims,
//         expires: DateTime.Now.AddHours(1),
//         signingCredentials: creds
//     );

//     var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

//     return Ok(new { token = tokenString });
//   }
// }


//# 🧱 Step 5: Protect Your APIs

// Update controller:

// using Microsoft.AspNetCore.Authorization;

// [Authorize]
// [ApiController]
// [Route("tasks")]
// public class TaskController : ControllerBase


//NOTE:  Middleware Signature
// public async Task Invoke(HttpContext context)
// {
//   await _next(context);
// }