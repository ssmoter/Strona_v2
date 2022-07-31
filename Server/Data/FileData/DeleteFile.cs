using Microsoft.AspNetCore.Hosting;
using Strona_v2.Shared.File;

namespace Strona_v2.Server.Data.FileData
{
    public class DeleteFile
    {
        ILogger _logger;
        IWebHostEnvironment _IwebHost;
        public DeleteFile(ILogger logger, IWebHostEnvironment iwebHost)
        {
            _logger = logger;
            _IwebHost = iwebHost;
        }

        //Usuwanie rekordu z bazy danych przy błędnym przesyłaniu danych
        public async Task<bool> Delete(FileModelServer server)
        {
            bool result = false;


            for (int i = 0; i < server.Files.Count() - 1; i++)
            {
                try
                {
                    System.IO.File.Delete(Path.Combine(_IwebHost.ContentRootPath, _IwebHost.EnvironmentName) + server.Files[i].StoredFileName);
                    result = true;
                }
                catch (IOException ex)
                {
                    _logger.LogError(ex.Message + "\n Błąd przy pliku: " + i);
                    return result;
                }
            }
            return result;
        }


    }
}
