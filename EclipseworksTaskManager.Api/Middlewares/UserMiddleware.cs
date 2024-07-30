using EclipseworksTaskManager.Domain.Exceptions;
using EclipseworksTaskManager.Domain.Interfaces.Service;
using Microsoft.Extensions.Primitives;

namespace EclipseworksTaskManager.Api.Middlewares
{
    public class UserMiddleware
    {
        private readonly RequestDelegate _next;

        public IUserService UserService { get; set; }

        public UserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUserService userService)
        {
            UserService = userService;

            if (context.Request.Headers.TryGetValue("User", out StringValues value))
                UserService.Set(value[0]);
            else
                throw new EtmBaseException("User from header is required.");

            await _next(context);
        }
    }

    public static class UserMiddlewareExtensions
    {
        public static IApplicationBuilder UseUserMiddleware(this IApplicationBuilder builder)
            => builder.UseMiddleware<UserMiddleware>();
    }
}