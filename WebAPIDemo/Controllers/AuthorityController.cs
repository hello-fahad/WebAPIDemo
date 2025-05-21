using Microsoft.AspNetCore.Mvc;

namespace WebAPIDemo.Controllers
{
    public class AuthorityController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
