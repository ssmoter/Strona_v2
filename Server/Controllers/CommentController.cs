using HashidsNet;
using Microsoft.AspNetCore.Mvc;
using Strona_v2.Server.Data.CommentData;
using Strona_v2.Server.Filtres;
using Strona_v2.Shared.File;

namespace Strona_v2.Server.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<CommentController> _logger;
        private readonly IHashids _hashids;
        private ISaveCommentToSQL _CommentSQL;


        public CommentController(IWebHostEnvironment webHostEnvironment,
            ILogger<CommentController> logger,
            IHashids hashids,
            ISaveCommentToSQL CommentSQL)
        {
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
            _hashids = hashids;
            _CommentSQL = CommentSQL;
        }


        //pobieranie komentarzy z jednego posta
        [HttpGet]
        public async Task<IActionResult> GetComment(string FileId)
        {
            var ravId = _hashids.Decode(FileId);
            if (ravId.Length == 0)
            {
                return NotFound();
            }

            var server = await _CommentSQL.GetCommentsAsync(ravId[0]);

            if (server != null)
            {
                List<CommentModelClient> clients = new();
                for (int i = 0; i < server.Count(); i++)
                {
                    clients.Add(new CommentModelClient(server[i], _hashids));
                }

                return Ok(clients);
            }
            return NoContent();
        }

        //zapisywanie komentarzy 
        [HttpPost]
        [TokenAuthenticationFilter]
        public async Task<IActionResult> SetComment(CommentModelClient comment)
        {
            var fileId = _hashids.Decode(comment.FileId);
            if (fileId.Length == 0)
            {
                return NotFound();
            }
            var userId = _hashids.Decode(comment.UserId);
            if (userId.Length == 0)
            {
                return NotFound();
            }

            CommentModelServer server = new(comment, _hashids);
            server.Created = DateTimeOffset.Now;

            await _CommentSQL.SetCommentsAsync(server);

            return Ok();
        }





    }
}
