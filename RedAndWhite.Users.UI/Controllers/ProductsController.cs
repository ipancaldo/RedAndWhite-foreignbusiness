using Microsoft.AspNetCore.Mvc;
using RedAndWhite.Infrastructure.Loaders;
using RedAndWhite.Model.Categories;
using RedAndWhite.Service.Products;

namespace RedAndWhite.Users.UI.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IModelLoader _modelLoader;
        private readonly IProductService _productService;

        public ProductsController(IModelLoader modelLoader, 
                                  IProductService productService)
        {
            _modelLoader = modelLoader;
            _productService = productService;
        }

        public IActionResult Index()
        {
            try
            {
                return View(_productService.GetAllProducts());
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IActionResult Test()
        {
            try
            {
                var test = _productService.GetByCategory(_modelLoader.CreateModel<GetProductsByCategoryModel>(new object[] { "newTestCategory" }));
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }

        }
    }
}
