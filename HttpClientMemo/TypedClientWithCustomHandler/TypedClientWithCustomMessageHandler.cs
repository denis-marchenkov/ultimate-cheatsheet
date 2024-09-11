namespace HttpClientMemo.TypedClientWithCustomHandler
{
    // Typed Client - wrapped in type and injected through constructor
    public class TypedClientWithCustomMessageHandler
    {
        private readonly HttpClient _httpClient;

        public TypedClientWithCustomMessageHandler(HttpClient httpClient) => _httpClient = httpClient;

        public async Task<string> GetAsync(string param)
        {
            var response = await _httpClient.GetAsync($"/test/{param}");

            var result = await response.Content.ReadAsStringAsync();

            return result;
        }
    }
}
