using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace HttpClientMemo.TypedClientWithCustomHandler
{
    // Example of a custom message handler for authentication
    public class CustomAuthMessageHandler(IOptions<AuthOptions> options) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authToken = await GetAccessTokenAsync(cancellationToken);

            request.Headers.Authorization = new AuthenticationHeaderValue(options.Value.AuthScheme, authToken.AccessToken);

            var httpResponseMessage = await base.SendAsync(request, cancellationToken);

            httpResponseMessage.EnsureSuccessStatusCode();

            return httpResponseMessage;
        }

        private async Task<AuthToken> GetAccessTokenAsync(CancellationToken cancellationToken)
        {
            var authParams = new KeyValuePair<string, string>[]
            {
                new("client_id", options.Value.ClientId),
                new("client_secret", options.Value.ClientSecret),
                new("scope", options.Value.Scope),
                new("grant_type", options.Value.GrantType)
            };

            var content = new FormUrlEncodedContent(authParams);

            var authRequest = new HttpRequestMessage(HttpMethod.Post, new Uri(options.Value.TokenUri))
            {
                Content = content
            };

            var response = await base.SendAsync(authRequest, cancellationToken);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<AuthToken>(cancellationToken: cancellationToken);
        }
    }

    public class AuthToken
    {
        public string AccessToken { get; set; }
    }

    public class AuthOptions
    {
        public string TokenUri { get; set; }
        public string AuthScheme { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scope { get; set; }
        public string GrantType { get; set; }
    }
}
