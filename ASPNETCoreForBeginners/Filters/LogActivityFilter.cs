using Azure.Core.Serialization;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ASPNETCoreForBeginners.Filters
{
    public class LogActivityFilter : IActionFilter, IAsyncActionFilter
    {
        private readonly ILogger<LogActivityFilter> _logger;

        public LogActivityFilter(ILogger<LogActivityFilter> logger)
        {
            _logger = logger;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation($"Execting Action {context.ActionDescriptor.DisplayName} on controller {context.Controller} with argument {JsonSerializer.Serialize(context.ActionArguments)}");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation($"Action {context.ActionDescriptor.DisplayName} Finshing extecuted on controller {context.Controller}");
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _logger.LogInformation($"(Async)  Execting Action {context.ActionDescriptor.DisplayName} on controller {context.Controller} with argument {JsonSerializer.Serialize(context.ActionArguments)}");
            await next();
            _logger.LogInformation($"(Async)  Action {context.ActionDescriptor.DisplayName} Finshing extecuted on controller {context.Controller}");

        }
    }
}
