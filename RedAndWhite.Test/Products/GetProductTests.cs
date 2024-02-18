using Microsoft.Extensions.DependencyInjection;
using RedAndWhite.Infrastructure.Loaders;
using RedAndWhite.Model.Products;
using RedAndWhite.Service.Products;

namespace RedAndWhite.Test.Products
{
    public class GetProductTests : BaseTest
    {
        private readonly IProductService _productService;
        private readonly IModelLoader _modelLoader;

        private const int _categoryId = 1;

        public GetProductTests(ServiceProviderFixture serviceProviderFixture) : base(serviceProviderFixture)
        {
            _productService = _serviceProvider.GetRequiredService<IProductService>();
            _modelLoader = _serviceProvider.GetRequiredService<IModelLoader>();
        }

        [Fact]
        public async Task GetByCategoryId()
        {
            //Arrange
            var getProductByIdModel = _modelLoader.CreateModel<GetProductByIdModel>(new object[] { _categoryId });

            //Act
            var product = await _productService.GetByCategoryId(getProductByIdModel);

            //Assert
            Assert.NotNull(product);
        }
    }
}
