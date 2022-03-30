using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Strona_v2.Client.Data.API;
using Strona_v2.Shared.User;
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


            UserLocalStorage userLocal = new();
            userLocal = await _LocalStorage.GetItemAsync<UserLocalStorage>(nameof(UserLocalStorage));

            if (userLocal != null)
            {
                if (!string.IsNullOrEmpty(userLocal.Name) && !string.IsNullOrEmpty(userLocal.Token))
                {
                    var identity = new ClaimsIdentity(new[]
                    {
                    new Claim(ClaimTypes.Name, userLocal.Name,ClaimTypes.SerialNumber,userLocal.Token),
                }, "authentication type"); //bez tego nie działa ale po co to jest to nie wiem

                    state = new AuthenticationState(new ClaimsPrincipal(identity));

                    // state = await UpdateOnlineAndCheckToken(state);
                }
            }

            NotifyAuthenticationStateChanged(Task.FromResult(state));
            return state;
        }
        private async Task<AuthenticationState> UpdateOnlineAndCheckToken(AuthenticationState state)
        {
            bool time = await StorageDate();
            if (time)
            {
                var _anonymous = new AuthenticationState(new ClaimsPrincipal());
                bool TokenValid = false;
                TokenValid = await _apiUserWithToken.UpdateLastOnline();
                if (!TokenValid)
                {
                    await _LocalStorage.RemoveItemAsync(nameof(UserLocalStorage));

                    return _anonymous;
                }
            }
            return state;
        }
        private async Task<bool> StorageDate()
        {
            bool result = false;

            DateTimeOffset time = await _LocalStorage.GetItemAsync<DateTimeOffset>(nameof(time));
            if (time.Year == 1)
            {
                time = DateTimeOffset.Now;
            }

            long UnixSecond = time.ToUnixTimeSeconds() + 1;//300 = 5min
            if (UnixSecond < DateTimeOffset.Now.ToUnixTimeSeconds())//aktuazlizacja wykonuje się max co 5min
            {
                await _LocalStorage.SetItemAsync<DateTimeOffset>(nameof(time), time);

                result = true;
            }
            string timeExists = await _LocalStorage.GetItemAsync<string>(nameof(time));
            if (string.IsNullOrEmpty(timeExists))
            {
                await _LocalStorage.SetItemAsync<DateTimeOffset>(nameof(time), time);
            }
            return result;
        }

    }
}
