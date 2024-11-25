namespace ApiGateway.Middleware
{
    public class RateLimitMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _scopeFactory;

        public RateLimitMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
        {
            _next = next;
            _scopeFactory = scopeFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var rateLimitService = scope.ServiceProvider.GetService<RateLimitService>();
                context.Request.Headers.TryGetValue("Api-key", out var apiKey);
                if (string.IsNullOrEmpty(apiKey))
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest; 
                    await context.Response.WriteAsync("Bad Request: Missing API Key.");
                    return; 
                }
                else
                {
                    if (rateLimitService.IsLimitReached(apiKey))
                    {
                        context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                        await context.Response.WriteAsync("Too Many Request: Limit Exceeded");
                        return;
                    };

                    await _next(context);
                }
            }
        }
    }
}
