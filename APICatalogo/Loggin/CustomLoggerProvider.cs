using System.Collections.Concurrent;

namespace APICatalogo.Loggin;

/// <summary>
/// Provedor de logs customizado que cria e gerencia instâncias de CustomLogger
/// </summary>
/// <remarks>
/// Implementa o padrão de provedor de logs do .NET Core:
/// 1. Mantém um dicionário de loggers por categoria
/// 2. Reutiliza instâncias de logger para a mesma categoria
/// 3. Implementa IDisposable para limpeza adequada
/// </remarks>
public class CustomLoggerProvider : ILoggerProvider
{

    readonly CustomLoggerProviderConfiguration loggerConfig;
    readonly ConcurrentDictionary<string, CustomLogger> loggers = new ConcurrentDictionary<string, CustomLogger>();

    /// <summary>
    /// Inicializa uma nova instância do provedor de logs
    /// </summary>
    /// <param name="config">Configuração do provedor de logs</param>
    public CustomLoggerProvider(CustomLoggerProviderConfiguration config)
    {
        loggerConfig = config;
    }

    /// <summary>
    /// Cria uma nova instância de logger ou retorna uma existente
    /// </summary>
    /// <param name="categoryName">Nome da categoria do logger</param>
    /// <returns>Instância de ILogger configurada</returns>
    /// <remarks>
    /// Utiliza um ConcurrentDictionary para garantir thread-safety
    /// </remarks>
    public ILogger CreateLogger(string categoryName)
    {
        return loggers.GetOrAdd(categoryName, name => new CustomLogger(name, loggerConfig));
    }

    /// <summary>
    /// Libera os recursos do provedor de logs
    /// </summary>
    /// <remarks>
    /// Limpa o dicionário de loggers mantidos
    /// </remarks>
    public void Dispose()
    {
        loggers.Clear();
    }

}
