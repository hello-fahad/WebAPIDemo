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
                try
                {
                    var response = await WebApiExecuter.InvokePost("shirts", shirt);
                    if (response != null) // Better to check for success explicitly if possible
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch(WebApiException ex) 
                {
                    HandleWebApiException(ex);
                }
                
                

                //ModelState.AddModelError("", "Failed to create shirt.");
            }

            return View(shirt);
        }


        public async Task<IActionResult> UpdateShirt(int shirtId)
        {

            try
            {
                var shirt = await WebApiExecuter.InvokeGet<Shirt>($"shirts/{shirtId}");
                if (shirt != null)
                {
                    return View(shirt);
                }
            }
            catch (WebApiException ex)
            {
                HandleWebApiException(ex);
                return View();
            }
            
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateShirt(Shirt shirt)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    await WebApiExecuter.InvokePut($"shirts/{shirt.ShirtId}", shirt);
                    return RedirectToAction(nameof(Index));
                }
                catch(WebApiException ex)
                {
                    HandleWebApiException(ex);
                }
                
            }

            return View(shirt);
        }

        public async Task<IActionResult> DeleteShirt(int shirtId)
        {
            try
            {
                await WebApiExecuter.InvokeDelete($"shirts/{shirtId}");
                return RedirectToAction(nameof(Index));
            }
            catch(WebApiException ex)
            {
                HandleWebApiException(ex);
                return View(nameof(Index),
                    await WebApiExecuter.InvokeGet<List<Shirt>>("shirts"));
            }
            
        }

        private void HandleWebApiException(WebApiException ex)
        {
            if (ex.ErrorResponse != null &&
                        ex.ErrorResponse.Errors != null &&
                        ex.ErrorResponse.Errors.Count > 0)
            {
                foreach (var error in ex.ErrorResponse.Errors)
                {
                    ModelState.AddModelError(error.Key, string.Join("; ", error.Value));
                }
            }
        }
    }
}
