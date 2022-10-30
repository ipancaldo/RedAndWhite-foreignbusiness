using RedAndWhite.Domain.ValueObjects.Category;
using RedAndWhite.Infrastructure.Mapping;

namespace RedAndWhite.Model.Products
{
    public class AssignCategoryModel : IMapFrom<GetCategoryById>
    {
        public int CategoryId { get; set; }

        public int ProductId { get; set; }
    }
}
