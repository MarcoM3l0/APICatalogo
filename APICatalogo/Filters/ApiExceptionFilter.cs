using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace APICatalogo.Filters;

/// <summary>
/// Filtro para tratamento global de exceções na API
/// </summary>
/// <remarks>
/// Este filtro captura todas as exceções não tratadas nos controllers,
/// registra o erro em log e retorna uma resposta padronizada com status 500.
/// </remarks>
public class ApiExceptionFilter : IExceptionFilter
{
    private readonly ILogger<ApiExceptionFilter> _logger;

    /// <summary>
    /// Inicializa uma nova instância do filtro de exceção
    /// </summary>
    /// <param name="logger">Serviço de logging para registro de erros</param>
    public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Método invocado quando uma exceção ocorre
    /// </summary>
    /// <param name="context">Contexto da exceção contendo detalhes do erro</param>
    /// <remarks>
    /// Este método:
    /// 1. Registra a exceção no sistema de logs
    /// 2. Define uma resposta padronizada com status 500
    /// 3. Previne a propagação da exceção
    /// </remarks>
    public void OnException(ExceptionContext context)
    {
        _logger.LogError(context.Exception, "Ocorreu uma exceção não tratada: Status code 500");

        context.Result = new ObjectResult("Ocorreu um erro interno no servidor. Tente novamente mais tarde.")
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
    }
}
