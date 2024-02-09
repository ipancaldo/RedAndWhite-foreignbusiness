using RedAndWhite.Model.Products;
using RedAndWhite.Service.Products;
using Microsoft.Extensions.DependencyInjection;

namespace RedAndWhite.Test
{
    public class CreateProductTest : BaseTest
    {
        private readonly IProductService _productService;

        public CreateProductTest(ServiceProviderFixture serviceProviderFixture) : base(serviceProviderFixture)
        {
            _productService = _serviceProvider.GetRequiredService<IProductService>();
        }

        private const string _productName = "Test product name";
        private const string _productDescription = "Test product description";

        [Fact]
        public async Task Should_Create_New_Product()
        {
            //Arrange
            NewProductModel newProductModel = new NewProductModel()
            {
                Name = _productName,
                Description = _productDescription
            };

            //Act
            await _productService.Create(newProductModel);

            //Assert
            var productFound = _productService.GetAllProducts().FirstOrDefault(p => p.Name.Equals(_productName));
            Assert.NotNull(productFound);
            Assert.Equal(_productName, productFound.Name);
            Assert.Equal(_productDescription, productFound.Description);

            //Clean up
            await _productService.Delete(productFound.Id);
            productFound = _productService.GetAllProducts().FirstOrDefault(p => p.Name.Equals(_productName));
            Assert.Null(productFound);
        }
    }
}