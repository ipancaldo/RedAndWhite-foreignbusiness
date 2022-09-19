using Microsoft.AspNetCore.Mvc;
using RedAndWhite.Domain.ValueObjects.Brand;
using RedAndWhite.Domain.ValueObjects.Product;
using RedAndWhite.Model.Products;
using RedAndWhite.Models;
using RedAndWhite.Service.Brands;
using RedAndWhite.Service.Products;
using System.Diagnostics;
using System.Xml.Linq;

namespace RedAndWhite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productsService;
        private readonly IBrandService _brandService;

        public HomeController(ILogger<HomeController> logger,
                              IProductService productsService,
                              IBrandService brandService)
        {
            this._logger = logger;
            this._productsService = productsService;
            this._brandService = brandService;
        }

        public IActionResult Index()
        {
            try
            {
                TestModifyBrand(3, "Test modify name");


                var testProduct = this._productsService.GetProductById(1);
                return View(this._productsService.GetAll().ToList());
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







        //---------------------------------------
        //Tests
        private void TestCreateProduct()
        {
            NewProductModel newProductModel = new NewProductModel()
            {
                Name = "Test Name",
                Description = "TestDescription"
            };
            this._productsService.Create(newProductModel);
        }

        private void TestModifyProduct()
        {
            ModifyPropertiesProduct modifyPropertiesProduct = new ModifyPropertiesProduct()
            {
                Id = 2,
                Name = "New test name",
                Description = null
            };
            this._productsService.Modify(modifyPropertiesProduct);
        }

       private void TestDeleteProduct(int id)
        {
            this._productsService.Delete(id);
        }

        private void TestAssignBrand(string brandName, int id)
        {
            this._productsService.AssignBrand(brandName, id);
        }

        private void TestCreateNewBrand(string brandName)
        {
            this._brandService.Create(new NewBrand(brandName));
        }

        private void TestDeleteBrand(int id)
        {
            this._brandService.Delete(id);
        }

        private void TestModifyBrand(int id, string name)
        {
            ModifyPropertiesBrand modifyPropertiesBrand = new ModifyPropertiesBrand()
            {
                Id = id,
                Name = name
            };
            this._brandService.Modify(modifyPropertiesBrand);
        }
    }
}