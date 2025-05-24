using System.Security.Cryptography;

namespace APICatalogo.Loggin;

public class CustomLogger : ILogger
{
    private string loggerName;
    private CustomLoggerProviderConfiguration loggerConfig;

    public CustomLogger(string name, CustomLoggerProviderConfiguration loggerConfig)
    {
        this.loggerName = name;
        this.loggerConfig = loggerConfig;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel == loggerConfig.LogLevel;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return null;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        string message = $"{logLevel.ToString()}: {eventId.Id} - {formatter(state, exception)}";
        EscreverTextoNoArquivo(message);
    }

    private void EscreverTextoNoArquivo(string message)
    {
        string caminhoArquivo = @"D:\Faculdade\log\log_Api_Controle.txt";
        using (StreamWriter streamWriter = new StreamWriter(caminhoArquivo, true))
        {
            try
            {
                streamWriter.WriteLine(message);
                streamWriter.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
