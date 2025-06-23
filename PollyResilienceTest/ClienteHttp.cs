using Polly;

namespace PollyResilienceTest
{
    public class ClienteHttp(IAsyncPolicy policy)
    {
        private readonly HttpClient _httpClient = new()
        {
            Timeout = TimeSpan.FromSeconds(3)
        };

        private IAsyncPolicy Policy { get; } = policy;

        public async Task<string> ChamarEndpoint(string url)
        {
            try
            {
                Console.WriteLine($"Executando requisição HTTP...{url}");

                var response = await Policy.ExecuteAsync(
                    async () => await _httpClient.GetAsync(url));

                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();
                return content;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Erro ao chamar o endpoint: {ex.Message}");
                return "Erro na chamada HTTP";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro inesperado: {ex.Message}");
                return "Erro na chamada HTTP";
            }
        }
    }
}