using RedAndWhite.Domain;
using RedAndWhite.Domain.ValueObjects.Brand;

namespace RedAndWhite.Service.Brands
{
    public interface IBrandService : IServiceBase<Brand>
    {
        void Create(NewBrand newBrand);

        Brand GetById(int id);

        void Modify(ModifyPropertiesBrand modifyPropertiesBrand);

        void Delete(int id);

        List<Brand> OrderBy();
    }
}
