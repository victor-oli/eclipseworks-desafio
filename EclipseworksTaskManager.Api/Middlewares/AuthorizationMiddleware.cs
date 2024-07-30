using Microsoft.Extensions.Primitives;

namespace EclipseworksTaskManager.Api.Middlewares
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.Headers.TryGetValue("User", out StringValues value);

            if (value[0] != "Admin")
            {
                WriteResponse(context);

                return;
            }

            await _next(context);
        }

        private void WriteResponse(HttpContext context)
        {
            context.Response.StatusCode = 403;
        }
    }

    public static class AuthorizationMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthorizationMiddleware(this IApplicationBuilder builder)
            => builder.UseMiddleware<AuthorizationMiddleware>();
    }
}