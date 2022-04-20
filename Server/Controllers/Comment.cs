using Microsoft.AspNetCore.Mvc;

namespace Strona_v2.Server.Controllers
{
    public class Comment : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
