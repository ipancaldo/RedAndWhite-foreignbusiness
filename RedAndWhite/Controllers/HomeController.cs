using Microsoft.AspNetCore.Mvc;
using RedAndWhite.Models;
using RedAndWhite.Service.Products;
using System.Diagnostics;

namespace RedAndWhite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductsService _productsService;

        public HomeController(ILogger<HomeController> logger,
                              IProductsService productsService)
        {
            this._logger = logger;
            this._productsService = productsService;
        }

        public IActionResult Index()
        {
            return View(this._productsService.GetAll().ToList());

            //return View(); // Estaba esto sólo
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}