using RedAndWhite.Domain.ValueObjects.Brand;
using RedAndWhite.Domain.ValueObjects.Category;
using RedAndWhite.Infrastructure.Mapping;

namespace RedAndWhite.Model.Products
{
    public class AssignCategoryModel : IMapFrom<CategoryToGet>
    {
        public string CategoryName { get; set; }

        public int ProductId { get; set; }
    }
}
