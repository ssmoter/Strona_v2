using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Forms;
using Strona_v2.Client.Data.API;
using Strona_v2.Shared.File;
using System.Net.Http.Json;

namespace Strona_v2.Client.Data.File.Upload
{
    public interface IUploadFileApi
    {
        Task<FileModel> PostFile(InputFileChangeEventArgs file, int UserId, ILogger logger);
    }

    public class UploadFileApi : IUploadFileApi
    {
        private AddTokenHttpClient _apiToken;
        private HttpClient _httpClient;
        private string ApiStringName { get; set; }
        public UploadFileApi(ILocalStorageService apiToken, HttpClient httpClient)
        {
            _apiToken = new(apiToken);
            UrlString urlString = new UrlString();
            ApiStringName = urlString.File;
            _httpClient = httpClient;
        }


        private async Task<MultipartFormDataContent> CastToMultiPart(InputFileChangeEventArgs file, ILogger logger)
        {
            var content = new MultipartFormDataContent();
            int maxFileSize = 1024 * 1024 * 10;

            try
            {
                foreach (var item in file.GetMultipleFiles())
                {
                    var fileContent = new StreamContent(item.OpenReadStream(maxFileSize));

                    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(item.ContentType);

                    content.Add(
                        content: fileContent,
                        name: "\"files\"",
                        fileName: item.Name);
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex.Message);
            }

            return content;
        }

        //wysłanie nowego obiektu
        public async Task<FileModel> PostFile(InputFileChangeEventArgs file, int UserId, ILogger logger)
        {
            MultipartFormDataContent content = new();
            if (file.GetMultipleFiles().Count > 0)
                content = await CastToMultiPart(file, logger);
            try
            {

                var Url = ApiStringName + "img?UserId=" + UserId;

                _httpClient = await _apiToken.AddHeadersAuthorization();

                var response = await _httpClient.PostAsync(Url, content);
                if (response.IsSuccessStatusCode)
                {
                    var UploadResult = await response.Content.ReadFromJsonAsync<FileModel>();

                    return UploadResult;
                }
                return null;
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex.Message);
                //Console.WriteLine(ex.Message);
                throw;
            }
        }






    }
}
