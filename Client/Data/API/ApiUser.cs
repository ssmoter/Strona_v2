using Strona_v2.Shared.User;
using System.Net.Http.Json;

namespace Strona_v2.Client.Data.API
{
    public class ApiUser : IApiUser
    {
        private HttpClient _httpClient { get; set; }

        public ApiUser(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private readonly string ApiStringName = "https://localhost:7249/api/User/";

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
                var response = await _httpClient.GetAsync(Url);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<UserLogin?>();
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
        public async Task<UserPublic?> ProfileUserPublicID(string Id, ILogger logger)
        {
            try
            {
                var Url = ApiStringName + "profileID?ID=" + Id;
                var response = await _httpClient.GetAsync(Url);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<UserPublic?>();
                    return result;
                }
                return null;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }




        //pobranie profilu usera po nick
        public async Task<UserPublic?> ProfileUserPublicName(string UserName, ILogger logger)
        {
            try
            {
                var Url = ApiStringName + "profileName?userName=" + UserName;
                var response = await _httpClient.GetAsync(Url);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<UserPublic?>();
                    return result;
                }
                return null;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        //sprawdzenie istnienia email'a
        public async Task<int> IndividualEmail(ILogger logger, string Email)
        {
            try
            {
                var Url = ApiStringName + "emails?email=" + Email;
                var result = await _httpClient.GetAsync(Url);

                if (result.IsSuccessStatusCode)
                {
                    return 0;
                }
                if (!result.IsSuccessStatusCode)
                {
                    return 1;
                }
                return 2;
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex.Message);
                return 2;
            }
        }

        //Rejestracja
        public async Task<UserLogin?> Register(ILogger logger, UserRegisterClient client)
        {
            string Url = ApiStringName;

            try
            {
                var response = await _httpClient.PostAsJsonAsync<UserRegisterClient>(Url, client);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<UserLogin?>();

                    return result;
                }
                return null;
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex.Message);
                return null;
            }
        }


    }
}
