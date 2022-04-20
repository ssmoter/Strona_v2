using HashidsNet;
using Microsoft.AspNetCore.Mvc;
using Strona_v2.Server.Data.CommentData;
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
        public async Task<ActionResult> GetComment(string FileId)
        {
            var ravId = _hashids.Decode(FileId);
            if (ravId.Length == 0)
            {
                return NotFound();
            }

            var server = await _CommentSQL.GetCommentsAsync(ravId[0]);

            if (server != null)
            {
                List<CommentModelClient> clients=new();
                for (int i = 0; i < server.Count(); i++)
                {
                    clients.Add(server[i].CastToClient());
                    //clients[i] = server[i].CastToClient();

                    clients[i].Id = _hashids.Encode(server[i].Id,11);
                    clients[i].FileId = FileId;
                    clients[i].UserId=_hashids.Encode(server[i].UserId,11);
                }

                return Ok(clients);
            }
            return NoContent();
        }

        //zapisywanie komentarzy 
        [HttpPost]
        public async Task<ActionResult> SetComment(CommentModelClient comment)
        {
            var fileId=_hashids.Decode(comment.FileId);
            if(fileId.Length == 0)
            {
                return NotFound();
            }
            var userId = _hashids.Decode(comment.UserId);
            if (userId.Length == 0)
            {
                return NotFound();
            }

            CommentModelServer server = new();
            server = comment.CastToServer();
            server.FileId = fileId[0];
            server.UserId = userId[0];
            server.Created=DateTimeOffset.Now;

            await _CommentSQL.SetCommentsAsync(server);

            return Ok();
        }





    }
}
