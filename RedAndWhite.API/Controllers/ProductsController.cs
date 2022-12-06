using Microsoft.AspNetCore.Mvc;
using RedAndWhite.Infrastructure.Loaders;
using RedAndWhite.Model.Categories;
using RedAndWhite.Model.Products;
using RedAndWhite.Service.Products;

namespace RedAndWhite.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IModelLoader _modelLoader;
        private readonly IProductService _productService;

        public ProductsController(IModelLoader modelLoader,
                                  IProductService productService)
        {
            _modelLoader = modelLoader;
            _productService = productService;
        }

        [ProducesDefaultResponseType(typeof(ProductModel))]
        [HttpGet]
        [Route("getallproducts")]
        public List<ProductModel> GetAll()
        {
            try
            {
                return _productService.GetAllProducts();
            }
            catch (Exception ex)
            {
                throw ex; //Test if we can only send the throw without the ex
            }
        }

        [ProducesDefaultResponseType(typeof(ProductModel))]
        [HttpGet]
        [Route("getproductsbycategory")]
        public List<ProductModel> GetByCategory(string category)
        {
            try
            {
                return _productService.GetByCategory(_modelLoader.CreateModel<GetProductsByCategoryModel>(new object[] { category }));
            }
            catch (Exception ex)
            {
                throw ex; //Test if we can only send the throw without the ex
            }
        }
    }
}
