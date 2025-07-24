using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Builder;
using Backend.Shared.Utils;
using Microsoft.Extensions.Logging;

namespace Backend.Middlewares
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JwtUtil _jwtUtil;
        private readonly ILogger<AuthenticationMiddleware> _logger;

        public AuthenticationMiddleware(RequestDelegate next, JwtUtil jwtUtil, ILogger<AuthenticationMiddleware> logger)
        {
            _next = next;
            _jwtUtil = jwtUtil;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower();

            if (path == "/api/user/login" || path == "/api/user/register" || path.StartsWith("/Images/")) {
                await _next(context);
                return;
            }
            
            var authHeader = context.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Missing or invalid Authorization header");
                return;
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();
            
            try
            {
                var userId = _jwtUtil.ValidateToken(token);
                context.Items["UserId"] = userId;
            }
            catch (Exception ex)
            {
                throw new SecurityTokenException("Invalid or expired token", ex);
                return;
            }

            await _next(context);
        }
    }

    public static class AuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthenticationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticationMiddleware>();
        }
    }
}
