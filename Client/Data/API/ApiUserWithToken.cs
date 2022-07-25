using Blazored.LocalStorage;
using Newtonsoft.Json;
using Strona_v2.Shared.User;
using System.Text;


namespace Strona_v2.Client.Data.API
{
    public interface IApiUserWithToken
    {
        HttpClient _httpClient { get; set; }
        Task<HttpResponseMessage> EditProfilPach(UserLogin userLogin, UserEditProfile usereditProfile);
        Task<bool> UpdateLastOnline();
    }

    public class ApiUserWithToken : IApiUserWithToken
    {
        public HttpClient _httpClient { get; set; }
        private readonly ILocalStorageService _localStorage;
        private readonly ITokenHttpClient _addTokenHttpClient;
        public string ApiStringName { get; set; }

        public ApiUserWithToken(HttpClient httpClient,
            ILocalStorageService localStorage,
            ITokenHttpClient addTokenHttpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Clear();
            UrlString _Url = new();
            ApiStringName = _Url.User;

            //_addTokenHttpClient = new(localStorage);
            _localStorage = localStorage;
            _addTokenHttpClient = addTokenHttpClient;
        }
        //Edycja profilu
        public async Task<HttpResponseMessage> EditProfilPach(UserLogin userLogin, UserEditProfile usereditProfile)
        {
            try
            {
                _httpClient = await _addTokenHttpClient.AddHeadersAuthorization();

                var Url = ApiStringName + "profile/patch?email=" +
                    userLogin.Email + "&password=" + userLogin.Password;

                var serializedUser = JsonConvert.SerializeObject(usereditProfile);

                var requestContent = new StringContent(serializedUser, Encoding.UTF8, "application/json");

                var result = await _httpClient.PatchAsync(Url, requestContent);

                return result;
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
                _httpClient = await _addTokenHttpClient.AddHeadersAuthorization();
                var Url = ApiStringName + "lastonline";
                var result = await _httpClient.GetAsync(Url);
                if (result.StatusCode.ToString() == "OK")
                {
                    Console.WriteLine("Zaktuzalizowano godzine");
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }



    }

}
