using RedAndWhite.Domain.ValueObjects.Category;
using RedAndWhite.Infrastructure.Loaders;
using RedAndWhite.Infrastructure.Mapping;

namespace RedAndWhite.Model.Products
{
    public class GetProductByIdModel : ModelBase, IMapFrom<GetCategoryById>
    {
        public GetProductByIdModel() { }

        public GetProductByIdModel(int categoryId) 
        {
            CategoryId = categoryId;
        }

        public int CategoryId { get; set; }
    }
}
