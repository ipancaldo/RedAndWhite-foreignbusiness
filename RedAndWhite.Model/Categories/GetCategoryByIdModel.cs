using RedAndWhite.Domain.ValueObjects.Category;
using RedAndWhite.Infrastructure.Mapping;

namespace RedAndWhite.Model.Categories
{
    public class GetCategoryByIdModel : IMapFrom<GetCategoryById>
    {
        public int CategoryId { get; set; }
    }
}
