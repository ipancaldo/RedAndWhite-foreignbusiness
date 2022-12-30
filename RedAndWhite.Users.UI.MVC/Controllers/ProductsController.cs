using Microsoft.AspNetCore.Mvc;
using RedAndWhite.Infrastructure.Enums;
using RedAndWhite.Infrastructure.Loaders;
using RedAndWhite.Model.Categories;
using RedAndWhite.Service.Products;

namespace RedAndWhite.Users.UI.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IModelLoader _modelLoader;
        private readonly IProductService _productService;

        private List<string> _resultVerifier = new() { "There were no coincidences", "don't exist", "already exist" };

        public ProductsController(IModelLoader modelLoader, 
                                  IProductService productService)
        {
            _modelLoader = modelLoader;
            _productService = productService;

            ViewBag.Categories = Categories.Cookies;
        }

        public IActionResult Index(string? category)
        {
            try
            {
                if (!string.IsNullOrEmpty(category))
                    return View(_productService.GetByCategory(_modelLoader.CreateModel<GetProductsByCategoryModel>(new object[] { category })));
                else
                    return View(_productService.GetAllProducts());
            }
            catch (Exception ex)
            {
                if (_resultVerifier.Any(rv => rv == ex.Message))
                {
                    TempData["AlertMessage"] = ex.Message;
                    return RedirectToAction("Index");
                }
                    
                return RedirectToAction("Index", "Error", ex);
            }
        }
    }
}
