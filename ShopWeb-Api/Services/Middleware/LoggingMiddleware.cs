namespace ShopWeb_Api.Services.Middleware
{
    /// <summary>
    /// Middleware для логирования входящих HTTP-запросов и исходящих ответов
    /// </summary>
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        /// <summary>
        /// Инициализирует новый экземпляр middleware для логирования
        /// </summary>
        /// <param name="next">Следующий делегат в конвейере middleware</param>
        /// <param name="logger">Логгер для записи информации о запросах и ответах</param>
        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Обрабатывает HTTP-запрос, логируя информацию о входящем запросе и исходящем ответе
        /// </summary>
        /// <param name="context">Контекст HTTP-запроса</param>
        /// <returns>Задача, представляющая асинхронную операцию обработки запроса</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation($"Request: {context.Request.Method} {context.Request.Path}");
            await _next(context);
            _logger.LogInformation($"Response: {context.Response.StatusCode}");
        }
    }
}
