using Strona_v2.Shared.User;
using System.Net.Http.Json;

namespace Strona_v2.Client.Data.API
{
    public class ApiUser : IApiUser
    {
        private readonly HttpClient _httpClient;

        public ApiUser(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public string ApiStringName { get; set; } = "https://localhost:7249/api/User/";

        //pobieranie danych pod sprawdzenie czy user istnieje i daje możliwość zalogowania
        //poprzez pobranie tokena
        public async Task<UserLogin?> LogIn(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            string Url = ApiStringName + "login?email=" + email + "&password=" + password;
            try
            {
                var result = await _httpClient.GetFromJsonAsync<UserLogin>(Url);
                if (result != null)
                {
                    return result;
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        //pobranie profilu usera po Id
        public async Task<UserPublic> ProfileUserPublic(int Id)
        {
            try
            {
                var Url = ApiStringName + "profile?id=" + Id;
                var result = await _httpClient.GetFromJsonAsync<UserPublic>(Url);
                if (result != null)
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
        //pobranie profilu usera po nick
        public async Task<UserPublic> ProfileUserPublic(string UserName)
        {
            try
            {
                var Url = ApiStringName + "profile?userName=" + UserName;
                var result = await _httpClient.GetFromJsonAsync<UserPublic>(Url);
                if (result != null)
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





    }
}
