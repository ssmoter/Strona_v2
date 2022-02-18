using Microsoft.AspNetCore.Mvc;
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
        public FileController(IWebHostEnvironment webHostEnvironment, ILogger<FileController> logger)
        {
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        




        [HttpPost]
        [TokenAuthenticationFilter]
        public async Task<ActionResult> PostFile(
            [FromForm]IEnumerable<IFormFile> files,FileModel fileMode)
        {




            return Ok();
        }


    }
}
