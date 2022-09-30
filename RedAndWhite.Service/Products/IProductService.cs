using RedAndWhite.Domain;
using RedAndWhite.Domain.ValueObjects.Product;
using RedAndWhite.Model.Products;

namespace RedAndWhite.Service.Products
{
    public interface IProductService : IServiceBase<Product>
    {
        Product GetProductById(int id);

        void Create(NewProductModel newProductModel);

        void Modify(ModifyPropertiesProduct modifyPropertiesProduct);

        void Delete(int id);

        void AssignBrand(string brandName, int productId);

        List<Product> OrderBy();
    }
}
