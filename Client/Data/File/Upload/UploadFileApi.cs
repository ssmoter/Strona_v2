using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Forms;
using Strona_v2.Client.Data.API;
using Strona_v2.Shared.File;
using System.Net.Http.Json;

namespace Strona_v2.Client.Data.File.Upload
{
    public interface IUploadFileApi
    {
        Task<FileModelClient> PostFile(InputFileChangeEventArgs file, ILogger logger);
        Task<bool> PostModel(FileModelClient file, ILogger logger);
    }

    public class UploadFileApi : IUploadFileApi
    {
        private readonly ITokenHttpClient _apiToken;
        private HttpClient _httpClient;
        private readonly ILocalStorageService _LocalStorage;
        private readonly UrlString urlString;
        public UploadFileApi(ILocalStorageService LocalStorage,
            HttpClient httpClient,
            ITokenHttpClient apiToken)
        {
            _LocalStorage = LocalStorage;
            urlString = new UrlString();
            _httpClient = httpClient;
            _apiToken = apiToken;
        }

        //conwertowanie pliku
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

        //wysłanie nowego pliku
        public async Task<FileModelClient> PostFile(InputFileChangeEventArgs file, ILogger logger)
        {

            MultipartFormDataContent content = new();
            if (file.GetMultipleFiles().Count > 0)
                content = await CastToMultiPart(file, logger);
            try
            {
                var UserLocal = await _apiToken.GetUserLocal();

                var Url = urlString.File + "img?UserId=" + UserLocal.Id;

                _httpClient = await _apiToken.AddHeadersAuthorization();

                var response = await _httpClient.PostAsync(Url, content);

                if (response.IsSuccessStatusCode)
                {
                    var UploadResult = await response.Content.ReadFromJsonAsync<FileModelClient>();

                    return UploadResult;
                }
                logger.LogError("Falied");
                return null;
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex.Message);
                throw;
            }
        }
        //Wysyłanie modelu
        public async Task<bool> PostModel(FileModelClient file, ILogger logger)
        {
            try
            {
                var UserLocal = await _apiToken.GetUserLocal();

                file.UserId = UserLocal.Id;

                var Url = urlString.File + "model";

                _httpClient = await _apiToken.AddHeadersAuthorization();
                file.Id = UserLocal.Id;
                var responseFile = await _httpClient.PostAsJsonAsync(Url, file);

                //dopisać czas do local storage                

                if (responseFile.IsSuccessStatusCode)
                {
                    var UploadResult = await responseFile.Content.ReadFromJsonAsync<DateTimeOffset>();
                    return true;
                }
                logger.LogError("Falied");
                return false;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return false;
            }
        }




    }
}
