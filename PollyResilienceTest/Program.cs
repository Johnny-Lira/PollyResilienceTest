using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using System.Net.Http;

namespace PollyResilienceTest
{
    public static class Program
    {
        public static async Task Main()
        {
            var policy = GetRetryPolicy();

            ClienteHttp clienteHttp = new(policy);

            string response = await clienteHttp.ChamarEndpoint("https://jsonplaceh.typicode.com/");

            Console.WriteLine($"Resposta do endpoint: {response}");
        }

        public static IAsyncPolicy GetRetryPolicy()
        {
            return Policy.Handle<Exception>()
                .WaitAndRetryAsync(5, retryAttempt =>
                {
                    double delay = Math.Pow(2, retryAttempt);
                    return TimeSpan.FromSeconds(delay);
                }, onRetry: (exception, timeSpan, retryCount, context) =>
                {
                    // Log a cada tentativa de retry
                    Console.WriteLine($"Tentativa {retryCount} falhou com exceção: {exception.Message}. " +
                                      $"Tentando novamente em {timeSpan.TotalSeconds} segundos.");
                });
        }
    }
}