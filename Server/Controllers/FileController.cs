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
        private readonly IFileToSQL _FileToSQL;
        public FileController(IWebHostEnvironment webHostEnvironment, ILogger<FileController> logger, ISaveFileToSQL isaveFileToSQL, IFileToSQL fileToSQL)
        {
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
            _IsaveFileToSQL = isaveFileToSQL;
            _FileToSQL = fileToSQL;
        }

        [HttpGet]
        public async Task<IList<FileModel>> GetFile()
        {
            IList<FileModel> files;

            files = await _FileToSQL.GetFileModels();

            return files;
        }

        [HttpGet]
        [Route("Single")]
        public async Task<ActionResult> GetFile(int id)
        {
            FileModel file = new();
            file.Id = id;
            file = await _FileToSQL.GetFileModel(file);

            return Ok(file);
        }




        [HttpPost]
        [Route("model")]
        [TokenAuthenticationFilter]
        //funkcja zapisywania danych w bazie danych
        public async Task<IActionResult> PostModel(FileModel fileModel)
        {


            return Ok();
        }

        [HttpPost]
        [Route("img")]
        [TokenAuthenticationFilter]
        //funkcja do pobierania plików 
        public async Task<ActionResult> PostFile(
         [FromForm] IEnumerable<IFormFile> files, int UserId)
        {

            FileUpload fileUpload = new FileUpload();
            FileModel fileM = new();

            //zapisywanie plików na serwerze
            if (files.Count() > 0)
            {
                fileM = await fileUpload.UploadAsync(files, UserId, _logger, _webHostEnvironment);

                return Ok(fileM);
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
