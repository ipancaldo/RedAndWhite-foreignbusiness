using RedAndWhite.Domain;
using RedAndWhite.Domain.ValueObjects.Brand;
using RedAndWhite.Model.Brands;
using RedAndWhite.Model.Categories;
using RedAndWhite.Model.Shared;

namespace RedAndWhite.Service.Brands
{
    public interface IBrandService : IServiceBase<Brand>
    {
        List<BrandModel> GetAllBrands();

        Brand GetById(int id);

        Task<List<Brand>> GetByCategory(GetCategoryByIdModel categoryModel);

        Task<ResultDTO<Brand>> Create(NewBrandModel newBrandModel);

        Task Update(ModifyPropertiesBrand modifyPropertiesBrand);

        Task Delete(int id);

        List<Brand> OrderBy();
    }
}
