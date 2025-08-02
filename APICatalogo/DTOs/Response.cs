namespace APICatalogo.DTOs;

/// <summary>
/// Modelo padrão para respostas da API
/// </summary>
/// <remarks>
/// Esta classe é utilizada para padronizar as respostas da API,
/// contendo um status e uma mensagem descritiva.
/// </remarks>
public class Response
{
    /// <summary>
    /// Status da operação
    /// </summary>
    /// <remarks>
    /// Valores comuns: "Success", "Error", "Warning"
    /// </remarks>
    /// <example>Success</example>
    public string? Status { get; set; }

    /// <summary>
    /// Mensagem detalhada sobre o resultado da operação
    /// </summary>
    /// <remarks>
    /// Deve fornecer informações claras sobre o resultado,
    /// especialmente em casos de erro.
    /// </remarks>
    /// <example>Operação concluída com sucesso</example>
    public string? Message { get; set; }
}
