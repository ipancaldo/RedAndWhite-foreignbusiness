using Microsoft.AspNetCore.Mvc;
using RedAndWhite.Infrastructure.Loaders;
using RedAndWhite.Model.Categories;
using RedAndWhite.Model.Products;
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

        public IActionResult Index(string? category)
        {
            try
            {
                List<ProductModel> products = new List<ProductModel>();

                if (!string.IsNullOrEmpty(category))
                    products = _productService.GetByCategory(_modelLoader.CreateModel<GetProductsByCategoryModel>(new object[] { category }));
                else
                    products = _productService.GetAllProducts();

                return View(products);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
