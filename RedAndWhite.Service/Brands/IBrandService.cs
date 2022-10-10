using RedAndWhite.Domain;
using RedAndWhite.Domain.ValueObjects.Brand;
using RedAndWhite.Model.Brands;

namespace RedAndWhite.Service.Brands
{
    public interface IBrandService : IServiceBase<Brand>
    {
        Brand GetById(int id);

        void Create(NewBrandModel newBrandModel);

        void Modify(ModifyPropertiesBrand modifyPropertiesBrand);

        void Delete(int id);

        List<Brand> OrderBy();
    }
}
