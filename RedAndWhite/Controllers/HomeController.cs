using Microsoft.AspNetCore.Mvc;
using RedAndWhite.Domain;
using RedAndWhite.Domain.ValueObjects.Brand;
using RedAndWhite.Domain.ValueObjects.Product;
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
                TestRemoveBrand(3, 2);

                //return View(TestOrderByProduct());
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
        private void TestCreateProduct(string productName, string description)
        {
            NewProductModel newProductModel = new NewProductModel()
            {
                Name = productName,
                Description = description
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
            this._productsService.ModifyProperties(modifyPropertiesProduct);
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

        private List<Product> TestOrderByProduct()
        {
            return this._productsService.OrderBy();
        }
        
        private List<Brand> TestOrderByBrand()
        {
            return this._brandService.OrderBy();
        }

        private void TestAddBrand(int productId, int brandId)
        {
            AddOrRemoveProductBrandModel addProductBrandModel = new AddOrRemoveProductBrandModel()
            {
                ProductId = productId,
                BrandId = brandId
            };
            this._productsService.AddBrand(addProductBrandModel);
        }        
        
        private void TestRemoveBrand(int productId, int brandId)
        {
            AddOrRemoveProductBrandModel removeProductBrandModel = new AddOrRemoveProductBrandModel()
            {
                ProductId = productId,
                BrandId = brandId
            };
            this._productsService.RemoveBrand(removeProductBrandModel);
        }
    }
}