using RedAndWhite.Domain.ValueObjects.Category;
using RedAndWhite.Infrastructure.Loaders;
using RedAndWhite.Infrastructure.Mapping;

namespace RedAndWhite.Model.Products
{
    public class GetProductsByCategoryModel : ModelBase, IMapFrom<CategoryToGet>
    {
        public GetProductsByCategoryModel() { }

        public GetProductsByCategoryModel(string categoryName)
        {
            CategoryName = categoryName;
        }

        public string CategoryName { get; set; }
    }
}
