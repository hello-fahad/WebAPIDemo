using Microsoft.AspNetCore.Mvc;
using WebApp.Data;
using WebApp.Models;
using WebApp.Models.Repositories;

namespace WebApp.Controllers
{
    public class ShirtsController : Controller
    {
        public IWebApiExecuter WebApiExecuter { get; }
        public ShirtsController(IWebApiExecuter webApiExecuter)
        {
            WebApiExecuter = webApiExecuter;
        }

        public async Task<IActionResult> Index()
        {
            return View(await WebApiExecuter.InvokeGet<List<Shirt>>("shirts"));
        }

        public IActionResult CreateShirt()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateShirt(Shirt shirt)
        {

            if (ModelState.IsValid)
            {
                var response = WebApiExecuter.InvokePost("shirts", shirt);
                if (response != null)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(shirt);
        }

        public async Task<IActionResult> UpdateShirt(int shirtId)
        {
            var shirt = await WebApiExecuter.InvokeGet<Shirt>($"shirts/{shirtId}");
            if(shirt != null)
            {
                return View(shirt);
            }

            return NotFound();
        }
    }
}
