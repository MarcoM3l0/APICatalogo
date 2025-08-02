namespace APICatalogo.RateLimitOptions;

/// <summary>
/// Opções de configuração para limitação de taxa (Rate Limiting)
/// </summary>
/// <remarks>
/// Define parâmetros para controle de requisições à API, prevenindo abuso
/// e garantindo justiça no uso dos recursos.
/// </remarks>
public class MyRateLimitOptions
{
    /// <summary>
    /// Nome da política de limitação de taxa
    /// </summary>
    public const string MyRateLimit = "MyRateLimit";

    /// <summary>
    /// Número máximo de requisições permitidas no período
    /// </summary>
    /// <remarks>
    /// Valor padrão: 5 requisições
    /// </remarks>
    public int PermitLimit { get; set; } = 5;

    /// <summary>
    /// Período de tempo no qual o limite se aplica
    /// </summary>
    /// <remarks>
    /// Valor padrão: 10
    /// </remarks>
    public int Window { get; set; } = 10;

    /// <summary>
    /// Intervalo para reposição de tokens
    /// </summary>
    /// <remarks>
    /// Valor padrão: 2 
    /// </remarks>
    public int ReplenishmentPeriod { get; set; } = 2;

    /// <summary>
    /// Número máximo de requisições que podem ficar na fila de espera
    /// </summary>
    /// <remarks>
    /// Valor padrão: 2 requisições
    /// </remarks>
    public int QueueLimit { get; set; } = 2;

    /// <summary>
    /// Número de segmentos em que a janela de tempo é dividida
    /// </summary>
    /// <remarks>
    /// Valor padrão: 8 segmentos
    /// </remarks>
    public int SegmentsPerWindow { get; set; } = 8;

    /// <summary>
    /// Limite total de tokens no algoritmo de token bucket
    /// </summary>
    /// <remarks>
    /// Valor padrão: 10 tokens
    /// </remarks>
    public int TokenLimit { get; set; } = 10;

    /// <summary>
    /// Limite secundário de tokens para casos específicos
    /// </summary>
    /// <remarks>
    /// Valor padrão: 20 tokens
    /// </remarks>
    public int TokenLimit2 { get; set; } = 20;

    /// <summary>
    /// Indica se a reposição de tokens é automática
    /// </summary>
    /// <remarks>
    /// Valor padrão: false (reposição manual)
    /// </remarks>
    public int TokensPerPeriod { get; set; } = 4;

    /// <summary>
    /// Indica se a reposição de tokens é automática
    /// </summary>
    /// <remarks>
    /// Valor padrão: false (reposição manual)
    /// </remarks>
    public bool AutoReplenishment { get; set; } = false;
}
