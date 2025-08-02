using Microsoft.AspNetCore.Mvc.Filters;

namespace APICatalogo.Filters;

/// <summary>
/// Filtro para log de requisições e respostas da API
/// </summary>
/// <remarks>
/// Registra informações detalhadas sobre:
/// - Requisições recebidas (antes da execução da action)
/// - Respostas geradas (após a execução da action)
/// Validação de modelos e códigos de status são incluídos nos logs.
/// </remarks>
public class ApiLoggingFilter : IActionFilter
{
    /// <summary>
    /// 
    /// </summary>
    public readonly ILogger<ApiLoggingFilter> _logger;

    /// <summary>
    /// Inicializa uma nova instância do filtro de log
    /// </summary>
    /// <param name="logger">Serviço de logging (injetado por DI)</param>
    public ApiLoggingFilter(ILogger<ApiLoggingFilter> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Executado APÓS a action processar a requisição
    /// </summary>
    /// <param name="context">Contexto após execução da action</param>
    /// <remarks>
    /// Registra:
    /// - Horário da resposta
    /// - Status code retornado
    /// </remarks>
    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Executa depois da action
        _logger.LogInformation("Executando 'OnActionExecuted'");
        _logger.LogInformation("----------------------------------------------------------");
        _logger.LogInformation($"{DateTime.Now.ToLongTimeString()}");
        _logger.LogInformation($"Status: {context.HttpContext.Response.StatusCode}");
        _logger.LogInformation("----------------------------------------------------------");
    }

    /// <summary>
    /// Executado ANTES da action processar a requisição
    /// </summary>
    /// <param name="context">Contexto de execução da action</param>
    /// <remarks>
    /// Registra:
    /// - Horário da requisição
    /// - Estado de validação do modelo
    /// </remarks>
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
