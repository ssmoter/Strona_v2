using Strona_v2.Client.Data.API;
using Strona_v2.Shared.File;
using System.Net.Http.Json;

namespace Strona_v2.Client.Data.File.Dowloand
{
    public interface IAPIFileDowloand
    {
        Task<List<FileModelPublic>> GetFileAsync();
    }

    public class APIFileDowloand : IAPIFileDowloand
    {
        private HttpClient _httpClient;
        private string ApiStringName;

        public APIFileDowloand(HttpClient httpClient)
        {
            UrlString urlString = new();
            ApiStringName = urlString.File;
            _httpClient = httpClient;
        }


        public async Task<List<FileModelPublic>> GetFileAsync()
        {
            try
            {
                var Url = ApiStringName ;

               var result = await _httpClient.GetFromJsonAsync<List<FileModelPublic>>(Url);

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        

    }
}
