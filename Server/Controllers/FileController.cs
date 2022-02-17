using Microsoft.AspNetCore.Mvc;
using Strona_v2.Server.Filtres;

namespace Strona_v2.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        [HttpPost]
        [TokenAuthenticationFilter]
        public async Task get()
        {

        }


    }
}
