using Blazored.LocalStorage;
using Newtonsoft.Json;
using Strona_v2.Shared.User;
using System.Text;


namespace Strona_v2.Client.Data.API
{
    public interface IApiUserWithToken
    {
        Task<HttpResponseMessage> EditProfilPach(UserLogin userLogin, UserEditProfile usereditProfile);
        Task<bool> UpdateLastOnline();
    }

    public class ApiUserWithToken : IApiUserWithToken
    {
        private HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly AddTokenHttpClient _addTokenHttpClient;
        private string ApiStringName { get; set; }

        public ApiUserWithToken(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Clear();
            UrlString _Url = new();
            ApiStringName = _Url.Url;

            _addTokenHttpClient = new(localStorage);
            _localStorage = localStorage;
        }
        //Edycja profilu
        public async Task<HttpResponseMessage> EditProfilPach(UserLogin userLogin, UserEditProfile usereditProfile)
        {
            try
            {
                _httpClient = await _addTokenHttpClient.AddHeadersAuthorization(_httpClient);

                var Url = ApiStringName + "profile/patch?email=" +
                    userLogin.Email + "&password=" + userLogin.Password;

                var serializedUser = JsonConvert.SerializeObject(usereditProfile);

                var requestContent = new StringContent(serializedUser, Encoding.UTF8, "application/json");

                var result = await _httpClient.PatchAsync(Url, requestContent);
                if (result.IsSuccessStatusCode)
                {
                    return result;
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        //Aktualizacja ostatnie wizyty
        public async Task<bool> UpdateLastOnline()
        {
            try
            {
                _httpClient = await _addTokenHttpClient.AddHeadersAuthorization(_httpClient);
                var Url = ApiStringName + "lastonline";
                var result = await _httpClient.GetAsync(Url);
                if (result.StatusCode.ToString() == "OK")
                {
                    Console.WriteLine("Zaktuzalizowano godzine");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }



    }

}
