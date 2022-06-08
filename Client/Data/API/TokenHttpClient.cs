using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Strona_v2.Client.Data.Toast;
using Strona_v2.Shared.User;

namespace Strona_v2.Client.Data.API
{
    public interface ITokenHttpClient
    {
        Task<HttpClient> AddHeadersAuthorization();
        Task UnAuthorizationUser(HttpResponseMessage result);
        Task<UserLocalStorage> GetUserLocal();

    }

    public class TokenHttpClient : ITokenHttpClient
    {
        private ILocalStorageService localStorage { get; set; }

        private string? Token { get; set; }
        private string Auth = "Authorization";
        private UserLocalStorage UserLocal { get; }

        private NavigationManager _navigation;
        private ToastService _toast;

        public TokenHttpClient(ILocalStorageService LocalStorageService,
            ToastService toast = null, NavigationManager navigation = null)
        {
            localStorage = LocalStorageService;
            _toast = toast;
            _navigation = navigation;
        }

        public async Task<UserLocalStorage> GetUserLocal()
        {
            UserLocalStorage UserLocal = await localStorage.GetItemAsync<UserLocalStorage>(nameof(UserLocalStorage));

            return UserLocal;
        }

        //wylogowanie przy braku autoryzacji
        public async Task UnAuthorizationUser(HttpResponseMessage result)
        {
            bool User = false;
            bool Admin = false;


            if (!result.IsSuccessStatusCode)
            {
                var uploadResult = await result.Content.ReadAsStringAsync();

                var stringResult = uploadResult.Split('"', '"');

                for (int i = 0; i < stringResult.Count() - 1; i++)
                {
                    if (stringResult[i] == "You are not Authorized.")
                    {
                        User = true;
                    }
                    if (false)
                    {
                        Admin = true;
                    }

                }

                if (User)
                {//zmienić na zmienną
                    _toast.ShowToast("Zostałeś wylogowany", ToastLevel.Info);
                    _navigation.NavigateTo("Logout");
                }
                if (Admin)
                {//zmienić na zmienną
                    _toast.ShowToast("Nie masz odpowiednich uprawnień", ToastLevel.Info);
                }
            }

        }

        //dodanie tokena do nagłówka
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
