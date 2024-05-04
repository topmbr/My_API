namespace Minimal_API.Middleware
{
    public class CustomHeadersMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Вызов следующего middleware в конвейере
            context.Response.OnStarting(() =>
            {
                // Добавляем заголовки к ответу
                context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
                context.Response.Headers["Pragma"] = "no-cache";
                context.Response.Headers["Expires"] = "0";
                return Task.CompletedTask;
            });

            await _next(context);
        }
    }
}
