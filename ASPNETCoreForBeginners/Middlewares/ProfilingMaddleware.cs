using System.Diagnostics;

namespace ASPNETCoreForBeginners.Middleware
{
    public class ProfilingMaddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ProfilingMaddleware> _logger;

        public ProfilingMaddleware(RequestDelegate next, ILogger<ProfilingMaddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            await _next(context);
            stopwatch.Stop();
            _logger.LogInformation($"Request `{context.Request.Path}` took `{stopwatch.ElapsedMilliseconds}ms ` to execut");
        }
    }
}
