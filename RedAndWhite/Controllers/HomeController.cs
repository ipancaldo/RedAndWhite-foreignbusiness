using Microsoft.AspNetCore.Mvc;
using RedAndWhite.Domain;
using RedAndWhite.Domain.DomainServices;
using RedAndWhite.Domain.ValueObjects.Brand;
using RedAndWhite.Domain.ValueObjects.Product;
using RedAndWhite.Model.Brands;
using RedAndWhite.Model.Categories;
using RedAndWhite.Model.Products;
using RedAndWhite.Models;
using RedAndWhite.Service.Brands;
using RedAndWhite.Service.Categories;
using RedAndWhite.Service.Products;
using System.Diagnostics;

namespace RedAndWhite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productsService;
        private readonly IBrandService _brandService;
        private readonly ICategoryService _categoryService;
        private readonly ICategoryDomainService _categoryDomainService;

        public HomeController(ILogger<HomeController> logger,
                              IProductService productsService,
                              IBrandService brandService,
                              ICategoryService categoryService)
        {
            this._logger = logger;
            this._productsService = productsService;
            this._brandService = brandService;
            this._categoryService = categoryService;
        }

        public IActionResult Index()
        {
            try
            {

                return View(this._productsService.GetAll().ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult AdminPanel()
        {
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

        private void TestCreateNewBrand(string brandName)
        {
            NewBrandModel newBrandModel = new NewBrandModel()
            {
                BrandName = brandName
            };
            this._brandService.Create(newBrandModel);
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
            this._productsService.AssignBrand(addProductBrandModel);
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

        private void TestAssignCategory(string categoryName, int productId)
        {
            AssignCategoryModel assignCategoryModel = new AssignCategoryModel()
            {
                CategoryName = categoryName,
                ProductId = productId,
            };
            this._productsService.AssignCategory(assignCategoryModel);
        }

        private void TestRemoveCategory(int productId, int categoryId)
        {
            RemoveCategoryFromProductModel removeCategoryFromProductModel = new RemoveCategoryFromProductModel()
            {
                ProductId = productId,
                CategoryId = categoryId
            };
            this._productsService.RemoveCategory(removeCategoryFromProductModel);
        }

        private List<Category> TestOrderByCategory()
        {
            return this._categoryService.OrderBy();
        }

        private void TestCreateCategory(string categoryName)
        {
            NewCategoryModel newCategoryModel = new NewCategoryModel()
            {
                CategoryName = categoryName
            };

            this._categoryService.Create(newCategoryModel);
        }


    }
}