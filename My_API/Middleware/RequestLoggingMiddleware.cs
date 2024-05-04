using System.Diagnostics;

namespace Minimal_API.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Логируем начало обработки запроса
            //Debug.WriteLine($"Запрос {context.Request.Method} {context.Request.Path} начал обрабатываться");
            Console.WriteLine($"Запрос 2 {context.Request.Method} {context.Request.Path} начал обрабатываться");
            // Передаем управление следующему компоненту в цепочке
            await _next(context);

            // Логируем завершение обработки запроса
            //Debug.WriteLine($"Запрос {context.Request.Method} {context.Request.Path} обработан с статус-кодом {context.Response.StatusCode}");

            Console.WriteLine($"Запрос 2 {context.Request.Method} {context.Request.Path} обрабтан ");
        }
    }

    public static class RequestLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLoggingMiddleware>();
        }
    }
}
