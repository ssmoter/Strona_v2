using Blazored.LocalStorage;
using Strona_v2.Shared.User;
using System.Net.Http.Json;

namespace Strona_v2.Client.Data.API
{
    public class TokenHttpClient
    {
        private ILocalStorageService localStorage { get; set; }

        private string? Token { get; set; }
        private string Auth = "Authorization";
        public UserLocalStorage UserLocal { get;}
        public TokenHttpClient(ILocalStorageService LocalStorageService)
        {
            localStorage = LocalStorageService;
        }

        public async Task<UserLocalStorage> GetUserLocal()
        {
            UserLocalStorage UserLocal= await localStorage.GetItemAsync<UserLocalStorage>(nameof(UserLocalStorage)); 

            return UserLocal;
        }

        public async Task<string> GetTaskAsync(HttpResponseMessage result)
        {
            if (!result.IsSuccessStatusCode)
            {
               var uploadResult=await result.Content.ReadAsStringAsync();
                return uploadResult;
            }
            return null;
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
