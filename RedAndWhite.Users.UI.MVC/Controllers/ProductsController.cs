using Microsoft.AspNetCore.Mvc;
using RedAndWhite.Infrastructure.Enums;
using RedAndWhite.Infrastructure.Loaders;
using RedAndWhite.Model.Products;
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

        public async Task<IActionResult> Index(int? categoryId)
        {
            try
            {
                if (categoryId.HasValue)
                    return View(await _productService.GetByCategoryId(_modelLoader.CreateModel<GetProductByIdModel>(new object[] { categoryId })));
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
