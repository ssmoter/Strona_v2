using Blazored.LocalStorage;
using Strona_v2.Shared.User;

namespace Strona_v2.Client.Data.API
{
    public class AddTokenHttpClient
    {
        private ILocalStorageService localStorage { get; set; }

        private string? Token { get; set; }
        private string Auth = "Authorization";
        public UserLocalStorage UserLocal { get;}
        public AddTokenHttpClient(ILocalStorageService LocalStorageService)
        {
            localStorage = LocalStorageService;
        }

        public async Task<UserLocalStorage> GetUserLocal()
        {
            UserLocalStorage UserLocal= await localStorage.GetItemAsync<UserLocalStorage>(nameof(UserLocalStorage)); 

            return UserLocal;
        }


        public async Task<HttpClient> AddHeadersAuthorization()
        {

            var user = await GetUserLocal();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add(Auth, user.Token);
            return httpClient;
        }

    }
}
