using EclipseworksTaskManager.Api.ViewModels;
using EclipseworksTaskManager.Domain.Exceptions;
using Newtonsoft.Json;

namespace EclipseworksTaskManager.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (EtmBaseException ex)
            {
                await ResponseWithError(ex.Message, ex, context);
            }
            catch (Exception ex)
            {
                await ResponseWithError("Internal server error.", ex, context);
            }
        }

        private async Task ResponseWithError(string message, Exception ex, HttpContext context)
        {
            Console.WriteLine(JsonConvert.SerializeObject(ex));

            var responseBody = BaseRespose<bool>
                .GetFailure(message);

            context.Response.StatusCode = 500;

            await context.Response
                .WriteAsJsonAsync(responseBody);
        }
    }

    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
            => builder.UseMiddleware<ExceptionMiddleware>();
    }
}