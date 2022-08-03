using HashidsNet;
using Microsoft.AspNetCore.Mvc;
using Strona_v2.Server.Data.ReactionData;
using Strona_v2.Shared.File;

namespace Strona_v2.Server.Controllers
{
    public class ReactionController : Controller
    {
        private readonly IHashids _Ihashids;
        private readonly IReactionSql _reactionSql;

        public ReactionController(IHashids ihashids)
        {
            _Ihashids = ihashids;
        }

        [HttpPost]
        public async Task<IActionResult> PostReaction(ReactionModelClient client)
        {
            if (client is null)
            {
                return NotFound();
            }

            var server = new ReactionModelServer(client, _Ihashids);

            await _reactionSql.InsertConcreteUserReaction(server);

            switch (server.TypeObject)
            {
                case ReactionType.UserData:

                    break;
                case ReactionType.FileModelC:

                    break;
                case ReactionType.CommentModel:

                    break;
                default:
                    return NotFound();
            }

            return Ok();
        }



    }
}
