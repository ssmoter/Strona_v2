using HashidsNet;
using Microsoft.AspNetCore.Mvc;
using Strona_v2.Server.Data.FileData;
using Strona_v2.Server.Data.SqlData.File;
using Strona_v2.Server.Filtres;
using Strona_v2.Shared.File;

namespace Strona_v2.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<FileController> _logger;
        private ISaveFileToSQL _IsaveFileToSQL;
        private readonly IFileToSQL _IFileToSQL;
        private readonly IHashids _Ihashids;


        public FileController(IWebHostEnvironment webHostEnvironment, ILogger<FileController> logger, ISaveFileToSQL isaveFileToSQL, IFileToSQL fileToSQL, IHashids hashids)
        {
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
            _IsaveFileToSQL = isaveFileToSQL;
            _IFileToSQL = fileToSQL;
            _Ihashids = hashids;
        }

        //zwracanie listy modeli
        [HttpGet]
        //public async Task<IList<FileModelClient>> GetFile()
        public async Task<IActionResult> GetFile()
        {
            var server = await _IFileToSQL.GetFileModelsSimple();

            List<FileModelClient> client = new();

            for (int i = 0; i < server.Count; i++)
            {
                client.Add(server[i].CastToClient(server[i]));
                client[i].Id = _Ihashids.Encode(server[i].Id, 11);
                client[i].UserId = _Ihashids.Encode(server[i].UserId, 11);
            }
            return Ok(client);
        }
        //zwracanie konkretnego modelu
        [HttpGet]
        [Route("single")]
        public async Task<IActionResult> GetFile(string id)
        {
            FileModelServer server = new();
            var ravId = _Ihashids.Decode(id);
            if (ravId.Length == 0)
            {
                return NotFound();
            }
            server.Id = ravId[0];
            server = await _IFileToSQL.GetFileModel(server);

            var client = server.CastToClient(server);
            client.Id = _Ihashids.Encode(server.Id, 11);
            client.UserId = _Ihashids.Encode(server.UserId, 11);

            return Ok(client);
        }


        [HttpGet]
        [Route("img")]
        public async Task<IActionResult> ShowImg(string UserId, string StoredFileName, string Type)
        {
            var ravId = _Ihashids.Decode(UserId);
            if (ravId.Length == 0)
            {
                return NotFound();
            }

            var path = Path.Combine(_webHostEnvironment.ContentRootPath,
                _webHostEnvironment.EnvironmentName, "unsafe_uploads", ravId[0].ToString(), StoredFileName);

            try
            {
                using (var image = System.IO.File.OpenRead(path))
                {
                    return File(image, "image/" + Type);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest();
            }
        }


        [HttpPost]
        [Route("model")]
        [TokenAuthenticationFilter]
        //funkcja zapisywania danych w bazie danych
        public async Task<IActionResult> PostModel(FileModelClient client)
        {
            client.Created = DateTimeOffset.Now;
            FileModelServer server = new();
            server = client.CastToServer(client);
            var rawUserId = _Ihashids.Decode(client.UserId);
            if (rawUserId.Length == 0)
            {
                return NotFound();
            }
            server.UserId = rawUserId[0];

            bool result = await _IsaveFileToSQL.SaveAsync(server, _logger);
            //wszystko ok
            if (result)
            {
                return Ok(client.Created);
            }
            return BadRequest();
        }

        //funkcja do pobierania plików 
        [HttpPost]
        [Route("img")]
        [TokenAuthenticationFilter]
        public async Task<IActionResult> PostFile(
            [FromForm] IEnumerable<IFormFile> files, string UserId)
        {
            FileUpload fileUpload = new();
            FileModelServer fileM = new();

            var rawUserId = _Ihashids.Decode(UserId);
            if (rawUserId.Length <= 0)
            {
                return NotFound();
            }

            //zapisywanie plików na serwerze
            if (files.Count() > 0)
            {
                fileM = await fileUpload.UploadAsync(files, rawUserId[0], _logger, _webHostEnvironment);

                if (fileM == null)
                {
                    return BadRequest();
                }

                FileModelClient client = new();

                client = fileM.CastToClient(fileM);

                return Ok(client);
            }
            else
                return BadRequest();
        }



        //[HttpPost]
        //[TokenAuthenticationFilter]
        //[Route("PostFileReapit")]
        ////funkcja pomocnycza jeżeli jakiś plik nie udało się zapisać
        ////tutaj można próbować przesłać plik do tego samego posta
        ////jest aktywna do 15 min po wrzuceniu posta
        //public async Task<IActionResult> PostFileReapit(
        //    [FromForm] IEnumerable<IFormFile> formFiles, int id)
        //{
        //    FileUpload fileUpload = new FileUpload();

        //    FileModel file = await fileUpload.UploadAsync(formFiles, _logger, _webHostEnvironment);

        //    file.Id = id;
        //    await _IsaveFileToSQL.SaveAsyncReapit(file, _logger);

        //    return Ok(file);
        //}


    }
}
