namespace Minimal_API.Middleware
{
    public class RequestOneLogging_Middleware
    {
        private readonly RequestDelegate _next;
        public RequestOneLogging_Middleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Логируем начало обработки запроса
            //Debug.WriteLine($"Запрос {context.Request.Method} {context.Request.Path} начал обрабатываться");
            Console.WriteLine($"Запрос 3 {context.Request.Method} {context.Request.Path} начал обрабатываться");
            // Передаем управление следующему компоненту в цепочке
            await _next(context);

            // Логируем завершение обработки запроса
            //Debug.WriteLine($"Запрос {context.Request.Method} {context.Request.Path} обработан с статус-кодом {context.Response.StatusCode}");

            Console.WriteLine($"Запрос 3 {context.Request.Method} {context.Request.Path} обрабтан ");
        }
    }
}
