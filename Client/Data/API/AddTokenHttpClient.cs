using Blazored.LocalStorage;

namespace Strona_v2.Client.Data.API
{
    public class AddTokenHttpClient
    {
        private HttpClient _httpClient { get; set; }
        private ILocalStorageService localStorage { get; set; }

        private string? Token { get; set; }
        private string Auth= "Authorization";

        public AddTokenHttpClient( )
        {
            Token = localStorage.GetItemAsync<string>("token").Result;
            _httpClient=new();
        }

        public HttpClient AddHeadersAuthorization()
        {
            _httpClient.DefaultRequestHeaders.Add(Auth,Token);
            return _httpClient;
        }

    }
}
