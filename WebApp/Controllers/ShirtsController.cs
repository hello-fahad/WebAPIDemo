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
                var response = await WebApiExecuter.InvokePost("shirts", shirt);
                if (response != null) // Better to check for success explicitly if possible
                {
                    return RedirectToAction(nameof(Index));
                }

                //ModelState.AddModelError("", "Failed to create shirt.");
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

        [HttpPost]
        public async Task<IActionResult> UpdateShirt(Shirt shirt)
        {

            if (ModelState.IsValid)
            {
                await WebApiExecuter.InvokePut($"shirts/{shirt.ShirtId}", shirt);
                return RedirectToAction(nameof(Index));
            }

            return View(shirt);
        }

        public async Task<IActionResult> DeleteShirt(int shirtId)
        {
            await WebApiExecuter.InvokeDelete($"shirts/{shirtId}");
            return RedirectToAction(nameof(Index));
        }
    }
}
