using Microsoft.AspNetCore.Mvc;
using RedAndWhite.Service.Brands;
using RedAndWhite.Users.UI.Models;
using System.Diagnostics;

namespace RedAndWhite.Users.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBrandService _brandService;

        public HomeController(ILogger<HomeController> logger,
                              IBrandService brandService)
        {
            _logger = logger;
            _brandService = brandService;
        }

        public IActionResult Index()
        {
            try
            {
                ViewBag.Brands = _brandService.GetAllBrands();
                return View(_brandService.GetAllBrands());
            }
            catch (Exception ex)
            {
                throw;
            }
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