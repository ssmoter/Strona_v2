using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Strona_v2.Client.Data.API;
using System.Security.Claims;

namespace Strona_v2.Client.Data
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _LocalStorage;
        private readonly AuthenticationState _anonymous;
        private ApiUserWithToken _apiUserWithToken;

        public CustomAuthStateProvider(ILocalStorageService localStorage, HttpClient httpClient)
        {
            _LocalStorage = localStorage;
            _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            _apiUserWithToken = new(httpClient, localStorage);
        }

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var state = new AuthenticationState(new ClaimsPrincipal());

            string username = await _LocalStorage.GetItemAsStringAsync("name");
            string token = await _LocalStorage.GetItemAsStringAsync("token");
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(token))
            {
                var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username,ClaimTypes.SerialNumber,token),
                }, "authentication type"); //bez tego nie działa ale po co to jest to nie wiem

                state = new AuthenticationState(new ClaimsPrincipal(identity));
            }
            NotifyAuthenticationStateChanged(Task.FromResult(state));

            //bool TokenValid = false;
            //TokenValid = await _apiUserWithToken.UpdateLastOnline();
            //if (!TokenValid)
            //{
            //    state = _anonymous;
            //}

            return state;
        }
    }
}
