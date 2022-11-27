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
        private readonly IProductService _productsService;
        private readonly IBrandService _brandService;
        private readonly ICategoryService _categoryService;
        private readonly ICategoryDomainService _categoryDomainService;

        public HomeController(IProductService productsService,
                              IBrandService brandService,
                              ICategoryService categoryService)
        {
            _productsService = productsService;
            _brandService = brandService;
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            try
            {
                var test = TestGetBrandsByCategory(2);

                return View(_productsService.GetAll().ToList());
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
            _productsService.Create(newProductModel);
        }

        private void TestModifyProduct(int id, string name, string? description)
        {
            ModifyPropertiesProduct modifyPropertiesProduct = new ModifyPropertiesProduct()
            {
                Id = id,
                Name = name,
                Description = description
            };
            _productsService.ModifyProperties(modifyPropertiesProduct);
        }

        private void TestDeleteProduct(int id)
        {
            _productsService.Delete(id);
        }

        private void TestCreateNewBrand(string brandName)
        {
            NewBrandModel newBrandModel = new NewBrandModel()
            {
                BrandName = brandName
            };
            _brandService.Create(newBrandModel);
        }

        private void TestDeleteBrand(int id)
        {
            _brandService.Delete(id);
        }

        private void TestModifyBrand(int id, string name)
        {
            ModifyPropertiesBrand modifyPropertiesBrand = new ModifyPropertiesBrand()
            {
                Id = id,
                Name = name
            };
            _brandService.Modify(modifyPropertiesBrand);
        }

        private List<Product> TestOrderByProduct()
        {
            return _productsService.OrderBy();
        }

        private List<Brand> TestOrderByBrand()
        {
            return _brandService.OrderBy();
        }

        private void TestAssignBrandToProduct(int productId, int brandId)
        {
            AddOrRemoveProductBrandModel addProductBrandModel = new AddOrRemoveProductBrandModel()
            {
                ProductId = productId,
                BrandId = brandId
            };
            _productsService.AssignBrand(addProductBrandModel);
        }

        private void TestGetBrandById(int id)
        {
            _brandService.GetById(id);
        }

        private void TestDomainGetBrandById(int id)
        {
            _brandService.GetById(id);
        }

        private void TestRemoveBrand(int productId, int brandId)
        {
            AddOrRemoveProductBrandModel removeProductBrandModel = new AddOrRemoveProductBrandModel()
            {
                ProductId = productId,
                BrandId = brandId
            };
            _productsService.RemoveBrand(removeProductBrandModel);
        }

        private void TestAssignCategory(int categoryId, int productId)
        {
            AssignCategoryModel assignCategoryModel = new AssignCategoryModel()
            {
                CategoryId = categoryId,
                ProductId = productId,
            };
            _productsService.AssignCategory(assignCategoryModel);
        }

        private void TestRemoveCategory(int productId, int categoryId)
        {
            RemoveCategoryFromProductModel removeCategoryFromProductModel = new RemoveCategoryFromProductModel()
            {
                ProductId = productId,
                CategoryId = categoryId
            };
            _productsService.RemoveCategory(removeCategoryFromProductModel);
        }

        private List<Category> TestOrderByCategory()
        {
            return _categoryService.OrderBy();
        }

        private void TestCreateCategory(string categoryName)
        {
            CategoryModel newCategoryModel = new CategoryModel()
            {
                CategoryName = categoryName
            };

            _categoryService.Create(newCategoryModel);
        }

        private List<Product> TestGetProductsByCategory(string categoryName)
        {
            GetProductsByCategoryModel getProductsByCategoryModel = new GetProductsByCategoryModel()
            {
                CategoryName = categoryName
            };

            return _productsService.GetByCategory(getProductsByCategoryModel);
        }

        private List<Brand> TestGetBrandsByCategory(int categoryId)
        {
            GetCategoryByIdModel categoryModel = new GetCategoryByIdModel()
            {
                CategoryId = categoryId
            };

            return _brandService.GetByCategory(categoryModel);
        }
    }
}