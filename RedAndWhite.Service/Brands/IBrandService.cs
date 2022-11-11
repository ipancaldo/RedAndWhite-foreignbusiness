using RedAndWhite.Domain;
using RedAndWhite.Domain.ValueObjects.Brand;
using RedAndWhite.Model.Brands;
using RedAndWhite.Model.Categories;

namespace RedAndWhite.Service.Brands
{
    public interface IBrandService : IServiceBase<Brand>
    {
        Brand GetById(int id);

        List<Brand> GetByCategory(GetCategoryByIdModel categoryModel);

        void Create(NewBrandModel newBrandModel);

        void Modify(ModifyPropertiesBrand modifyPropertiesBrand);

        void Delete(int id);

        List<Brand> OrderBy();
    }
}
