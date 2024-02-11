using RedAndWhite.Model.Products;
using RedAndWhite.Service.Products;
using Microsoft.Extensions.DependencyInjection;
using RedAndWhite.Domain.ValueObjects.Product;

namespace RedAndWhite.Test
{
    public class EeditProductTest : BaseTest
    {
        private readonly IProductService _productService;

        public EeditProductTest(ServiceProviderFixture serviceProviderFixture) : base(serviceProviderFixture)
        {
            _productService = _serviceProvider.GetRequiredService<IProductService>();
        }

        private const string _productName = "Test product name";
        private const string _productDescription = "Test product description";
        private const string _productNameEdited = "Test product name edited";
        private const string _productDescriptionEdited = "Test product description edited";

        [Fact]
        public async Task Should_Edit_Product()
        {
            //Arrange
            NewProductModel newProductModel = new NewProductModel()
            {
                Name = _productName,
                Description = _productDescription
            };

            await _productService.Create(newProductModel);
            var productFound = _productService.GetAllProducts().FirstOrDefault(p => p.Name.Equals(_productName));
            Assert.NotNull(productFound);

            ModifyPropertiesProduct modifyPropertiesProduct = new ModifyPropertiesProduct
            {
                Id = productFound.Id,
                Name = _productNameEdited,
                Description = _productDescriptionEdited
            };

            //Act
            await _productService.Update(modifyPropertiesProduct);

            //Assert
            var productEditedFound = _productService.GetAllProducts().FirstOrDefault(p => p.Name.Equals(_productNameEdited));
            Assert.NotNull(productEditedFound);
            Assert.Equal(_productNameEdited, productEditedFound.Name);
            Assert.Equal(_productDescriptionEdited, productEditedFound.Description);

            //Clean up
            await _productService.Delete(productEditedFound.Id);
            productEditedFound = _productService.GetAllProducts().FirstOrDefault(p => p.Name.Equals(_productName));
            Assert.Null(productEditedFound);
        }
    }
}