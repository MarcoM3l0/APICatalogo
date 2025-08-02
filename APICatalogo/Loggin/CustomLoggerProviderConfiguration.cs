namespace APICatalogo.Loggin;

/// <summary>
/// Classe de configuração para o provedor de logger customizado
/// </summary>
/// <remarks>
/// Define parâmetros de configuração para o comportamento do logger:
/// - Nível mínimo de log a ser registrado
/// - Identificador do provedor (para cenários com múltiplos provedores)
/// </remarks>
public class CustomLoggerProviderConfiguration
{
    /// <summary>
    /// Nível mínimo de log a ser registrado
    /// </summary>
    public LogLevel LogLevel { get; set; } = LogLevel.Warning;

    /// <summary>
    /// Identificador do provedor de logs
    /// </summary>
    public int Id { get; set; } = 0;
}
