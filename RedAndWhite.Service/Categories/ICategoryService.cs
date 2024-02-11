using RedAndWhite.Domain;
using RedAndWhite.Model.Categories;

namespace RedAndWhite.Service.Categories
{
    public interface ICategoryService : IServiceBase<Category>
    {
        Task Create(CategoryModel newCategoryModel);

        List<Category> OrderBy();
    }
}