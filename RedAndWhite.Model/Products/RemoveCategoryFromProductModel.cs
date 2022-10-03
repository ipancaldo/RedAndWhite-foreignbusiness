using RedAndWhite.Domain.ValueObjects.Category;
using RedAndWhite.Infrastructure.Mapping;

namespace RedAndWhite.Model.Products
{
    public class RemoveCategoryFromProductModel : IMapFrom<GetCategoryById>
    {
        public int ProductId { get; set; }

        public int CategoryId { get; set; }
    }
}
