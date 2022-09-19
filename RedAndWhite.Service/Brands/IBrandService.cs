using RedAndWhite.Domain;
using RedAndWhite.Domain.ValueObjects.Brand;

namespace RedAndWhite.Service.Brands
{
    public interface IBrandService : IServiceBase<Brand>
    {
        Brand GetBrandById(int id);

        void Create(NewBrand newBrand);

        void Modify(ModifyPropertiesBrand modifyPropertiesBrand);

        void Delete(int id);
    }
}
