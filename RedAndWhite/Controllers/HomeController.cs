using Microsoft.AspNetCore.Mvc;
using RedAndWhite.Model.Products;
using RedAndWhite.Models;
using RedAndWhite.Service.Brands;
using RedAndWhite.Service.Products;
using System.Diagnostics;

namespace RedAndWhite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productsService;

        public HomeController(ILogger<HomeController> logger,
                              IProductService productsService)
        {
            this._logger = logger;
            this._productsService = productsService;
        }

        public IActionResult Index()
        {
            try
            {
                NewProductModel newProductModel = new NewProductModel()
                {
                    Name = "Test Name",
                    Description = "TestDescription"
                };
                this._productsService.Create(newProductModel);

                var testProduct = this._productsService.GetProductById(1);
                return View(this._productsService.GetAll().ToList());

                this._productsService.AssignBrand("Ani", 1);
            }
            catch (Exception ex)
            {
                throw ex;
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