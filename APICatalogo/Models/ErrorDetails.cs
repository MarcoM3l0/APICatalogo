using System.Text.Json;

namespace APICatalogo.Models;

/// <summary>
/// Classe para padronização de detalhes de erros na API
/// </summary>
/// <remarks>
/// Utilizada para retornar informações consistentes sobre erros,
/// incluindo código de status, mensagem e stack trace (em desenvolvimento).
/// </remarks>
public class ErrorDetails
{
    /// <summary>
    /// Código de status HTTP do erro
    /// </summary>
    /// <example>500</example>
    public int StatusCode { get; set; }

    /// <summary>
    /// Mensagem descritiva do erro
    /// </summary>
    /// <example>Ocorreu um erro ao processar sua requisição</example>
    public string? Message { get; set; }

    /// <summary>
    /// Stack trace do erro (normalmente apenas em ambiente de desenvolvimento)
    /// </summary>
    /// <remarks>
    /// Não deve ser exposto em produção por questões de segurança
    /// </remarks>
    public string? Trace { get; set; }

    /// <summary>
    /// Serializa o objeto para formato JSON
    /// </summary>
    /// <returns>
    /// String contendo a representação JSON dos detalhes do erro
    /// </returns>
    /// <example>
    /// {"StatusCode":500,"Message":"Erro interno","Trace":"System.Exception: ..."}
    /// </example>
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
