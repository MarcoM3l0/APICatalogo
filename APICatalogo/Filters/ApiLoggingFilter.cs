using Microsoft.AspNetCore.Mvc.Filters;

namespace APICatalogo.Filters;

public class ApiLoggingFilter : IActionFilter
{
    public readonly ILogger<ApiLoggingFilter> _logger;
    public ApiLoggingFilter(ILogger<ApiLoggingFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Executa depois da action
        _logger.LogInformation("Executando 'OnActionExecuting'");
        _logger.LogInformation("----------------------------------------------------------");
        _logger.LogInformation($"{DateTime.Now.ToLongTimeString()}");
        _logger.LogInformation($"Status: {context.HttpContext.Response.StatusCode}");
        _logger.LogInformation("----------------------------------------------------------");
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        // Executa antes da action
        _logger.LogInformation("Executando 'OnActionExecuting'");
        _logger.LogInformation("----------------------------------------------------------");
        _logger.LogInformation($"{DateTime.Now.ToLongTimeString()}");
        _logger.LogInformation($"Modelo: {context.ModelState.IsValid}");
        _logger.LogInformation("----------------------------------------------------------");
    }
}
