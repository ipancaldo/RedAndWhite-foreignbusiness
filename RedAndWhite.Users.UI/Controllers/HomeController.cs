using Microsoft.AspNetCore.Mvc;
using RedAndWhite.Service.Products;
using RedAndWhite.Users.UI.Models;
using System.Diagnostics;

namespace RedAndWhite.Users.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;

        public HomeController(ILogger<HomeController> logger,
                              IProductService productService
            )
        {
            _logger = logger;
            _productService = productService;
        }

        public IActionResult Index()
        {
            var test = _productService.GetAllProducts();
            return View();
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