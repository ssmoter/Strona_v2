﻿using Strona_v2.Client.Data.API;
using Strona_v2.Shared.File;
using System.Net.Http.Json;

namespace Strona_v2.Client.Data.File.Dowloand
{
    public interface IAPIFileDowloand
    {
        string GetFileImg(string UserId, string StoredFileName, string Type, ILogger logger);
        Task<FileModelPublic> GetFileModel(string Id, ILogger logger);
        Task<List<FileModelPublic>> GetFileModelFromTag(string tag, ILogger logger);
        Task<List<FileModelPublic>> GetFileModels(ILogger logger);
    }

    public class APIFileDowloand : IAPIFileDowloand
    {
        private HttpClient _httpClient;
        private UrlString urlString;
        private readonly string ImgError = "";

        public APIFileDowloand(HttpClient httpClient)
        {
            urlString = new();
            _httpClient = httpClient;
        }

        //pobieranie listy 
        public async Task<List<FileModelPublic>> GetFileModels(ILogger logger)
        {
            try
            {
                var Url = urlString.File;

                var result = await _httpClient.GetFromJsonAsync<List<FileModelPublic>>(Url);

                return result;
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex.Message);
                throw;
            }
        }
        //pobranie jednego elementu
        public async Task<FileModelPublic> GetFileModel(string Id, ILogger logger)
        {
            try
            {
                var Url = urlString.File + "single?id=" + Id;

                var resutl = await _httpClient.GetFromJsonAsync<FileModelPublic>(Url);

                return resutl;

            }
            catch (Exception ex)
            {
                logger.LogInformation(ex.Message);
                throw;
            }
        }

        public async Task<List<FileModelPublic>> GetFileModelFromTag(string tag, ILogger logger)
        {
            try
            {
                var Url = urlString.Tag + tag;

                var result = await _httpClient.GetFromJsonAsync<List<FileModelPublic>>(Url);

                return result;
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex.Message);
                throw;
            }

        }


        // przygotowanie adresu do wyświetlenia img
        public string GetFileImg(string UserId, string StoredFileName, string Type, ILogger logger)
        {
            try
            {
                var Url = urlString.File + "img?" +
                    nameof(UserId) + "=" + UserId +
                    "&" + nameof(StoredFileName) + "=" + StoredFileName +
                    "&" + nameof(Type) + "=" + Type;

                return Url;
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex.Message);
                return ImgError;
            }
        }




    }
}
