using Polly;
using Polly.Timeout;
using Polly.Wrap;

namespace PollyResilienceTest
{
    public static class ResiliencePolicies
    {
        public static IAsyncPolicy<HttpResponseMessage> RetryExponencialPolicy()
        {
            return Policy
                .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .WaitAndRetryAsync(5, tentativa =>
                {
                    double delay = Math.Pow(2, tentativa); // 2s, 4s, 8s, etc.
                    return TimeSpan.FromSeconds(delay);
                },
                onRetry: (resultado, tempo, tentativa, contexto) =>
                {
                    Console.WriteLine($"[Polly] Retry {tentativa} em {tempo.TotalSeconds}s: {resultado.Result?.StatusCode}");
                });
        }
    }
}