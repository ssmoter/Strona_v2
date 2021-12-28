using Blazored.LocalStorage;

namespace Strona_v2.Client.Data.API
{
    public class AddTokenHttpClient
    {
        private ILocalStorageService localStorage { get; set; }

        private string? Token { get; set; }
        private string Auth = "Authorization";

        public AddTokenHttpClient(ILocalStorageService LocalStorageService)
        {
            localStorage = LocalStorageService;
        }

        public async Task<HttpClient> AddHeadersAuthorization(HttpClient httpClient)
        {
            Token = await localStorage.GetItemAsync<string>("token");
            httpClient= new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add(Auth, Token);
            return httpClient;
        }

    }
}
