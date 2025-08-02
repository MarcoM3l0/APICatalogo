using System.Security.Cryptography;

namespace APICatalogo.Loggin;

/// <summary>
/// Implementação customizada de ILogger para gravação de logs em arquivo
/// </summary>
/// <remarks>
/// Esta classe:
/// 1. Grava logs em um arquivo texto especificado
/// 2. Filtra logs baseado no nível configurado
/// 3. Formata mensagens com nível de log, eventId e mensagem
/// </remarks>
public class CustomLogger : ILogger
{
    private string loggerName;
    private CustomLoggerProviderConfiguration loggerConfig;

    /// <summary>
    /// Inicializa uma nova instância do logger customizado
    /// </summary>
    /// <param name="name">Nome do logger (categoria)</param>
    /// <param name="loggerConfig">Configuração do provedor de log</param>
    public CustomLogger(string name, CustomLoggerProviderConfiguration loggerConfig)
    {
        this.loggerName = name;
        this.loggerConfig = loggerConfig;
    }

    /// <summary>
    /// Verifica se o nível de log está habilitado
    /// </summary>
    /// <param name="logLevel">Nível de log a ser verificado</param>
    /// <returns>
    /// True se o nível de log estiver habilitado na configuração, False caso contrário
    /// </returns>
    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel == loggerConfig.LogLevel;
    }

    /// <summary>
    /// Inicia um escopo de log (não implementado)
    /// </summary>
    /// <typeparam name="TState">Tipo do estado</typeparam>
    /// <param name="state">O estado para o escopo</param>
    /// <returns>Um descartável que termina o escopo quando descartado</returns>
    public IDisposable? BeginScope<TState>(TState state)
    {
        return null;
    }

    /// <summary>
    /// Escreve uma entrada de log
    /// </summary>
    /// <typeparam name="TState">Tipo do estado</typeparam>
    /// <param name="logLevel">Nível do evento de log</param>
    /// <param name="eventId">ID do evento</param>
    /// <param name="state">O estado da entrada de log</param>
    /// <param name="exception">A exceção relacionada (opcional)</param>
    /// <param name="formatter">Formatador para criar a mensagem de texto</param>
    /// <remarks>
    /// Formato da mensagem: "NIVEL_LOG: EVENTO_ID - MENSAGEM [EXCECAO]"
    /// </remarks>
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        string message = $"{logLevel.ToString()}: {eventId.Id} - {formatter(state, exception)}";
        EscreverTextoNoArquivo(message);
    }

    private void EscreverTextoNoArquivo(string message)
    {
        string caminhoArquivo = @"D:\Api_controle\log\log_Api_Controle.txt";
        using (StreamWriter streamWriter = new StreamWriter(caminhoArquivo, true))
        {
            try
            {
                streamWriter.WriteLine(message);
                streamWriter.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
