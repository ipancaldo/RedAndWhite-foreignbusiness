using RedAndWhite.Domain.ValueObjects.Brand;
using RedAndWhite.Infrastructure.Mapping;

namespace RedAndWhite.Model.Products
{
    public class AddOrRemoveProductBrandModel : IMapFrom<GetBrandById>
    {
        public int ProductId { get; set; }
        public int BrandId { get; set; }
    }
}
