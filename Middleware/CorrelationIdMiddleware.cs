namespace TextIntelligenceApi.Middleware
{
    public sealed class CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
    {
        private const string HeaderName = "X-Correlation-Id";
        private const string ItemKey = "CorrelationId";

        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId = context.Request.Headers.TryGetValue(HeaderName, out var header) && !string.IsNullOrWhiteSpace(header)
                ? header.ToString()
                : Guid.NewGuid().ToString("N");

            context.Items[ItemKey] = correlationId;
            context.Response.Headers[HeaderName] = correlationId;

            using (logger.BeginScope(new Dictionary<string, object> { ["CorrelationId"] = correlationId }))
            {
                await next(context);
            }
        }
    }

    public static class CorrelationIdExtensions
    {
        public static string GetCorrelationId(this HttpContext context) =>
            context.Items.TryGetValue("CorrelationId", out var correlationId) ? correlationId?.ToString() ?? string.Empty : string.Empty;
    }
}
