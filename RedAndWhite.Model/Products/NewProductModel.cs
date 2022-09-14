using RedAndWhite.Domain.ValueObjects;
using RedAndWhite.Infrastructure.Mapping;

namespace RedAndWhite.Model.Products
{
    public class NewProductModel : IMapFrom<NewProduct>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
