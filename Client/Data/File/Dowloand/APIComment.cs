using Blazored.LocalStorage;
using Strona_v2.Client.Data.API;
using Strona_v2.Shared.File;
using System.Net.Http.Json;

namespace Strona_v2.Client.Data.File.Dowloand
{
    public interface IAPIComment
    {
        Task<List<CommentModelClient>> GetCommentAsync(string FileId, ILogger logger);
        Task<bool> SendComment(CommentModelClient comment, ILogger logger);
    }

    public class APIComment : IAPIComment
    {
        private HttpClient _httpClient;
        private string ApiStringName;
        private ITokenHttpClient _apiToken;


        public APIComment(HttpClient http,
            ILocalStorageService LocalStorage,
            ITokenHttpClient apiToken)
        {
            _httpClient = http;
            UrlString urlString = new();
            ApiStringName = urlString.Comment;
            _apiToken = apiToken;
            //_apiToken = new(LocalStorage);
        }

        //pobieranie komentarzy z jednego posta
        public async Task<List<CommentModelClient>> GetCommentAsync(string FileId, ILogger logger)
        {
            try
            {
                var Url = ApiStringName + "?ObjectId=" + FileId;

                var result = await _httpClient.GetAsync(Url);

                if (result.IsSuccessStatusCode)
                {
                    var response = await result.Content.ReadFromJsonAsync<List<CommentModelClient>>();
                    return response;
                }
                return null;
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex.Message);
                throw;
            }

        }

        //Wysyłanie komentarza do posta
        public async Task<bool> SendComment(CommentModelClient comment, ILogger logger)
        {
            try
            {
                var Url = ApiStringName;

                var UserLocal = await _apiToken.GetUserLocal();
                comment.UserId = UserLocal.Id;

                _httpClient = await _apiToken.AddHeadersAuthorization();

                var response = await _httpClient.PostAsJsonAsync(Url, comment);

                await _apiToken.UnAuthorizationUser(response);

                if(response.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex.Message);
                return false;
            }
        }




    }
}
