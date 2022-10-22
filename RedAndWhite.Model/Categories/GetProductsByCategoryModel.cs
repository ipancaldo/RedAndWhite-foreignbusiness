using RedAndWhite.Domain.ValueObjects.Category;
using RedAndWhite.Infrastructure.Mapping;

namespace RedAndWhite.Model.Categories
{
    public class GetProductsByCategoryModel : IMapFrom<CategoryToGet>
    {
        public string CategoryName { get; set; }
    }
}
