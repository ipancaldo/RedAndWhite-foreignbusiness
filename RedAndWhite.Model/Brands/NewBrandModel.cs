using RedAndWhite.Domain.ValueObjects.Brand;
using RedAndWhite.Infrastructure.Mapping;

namespace RedAndWhite.Model.Brands
{
    public class NewBrandModel : IMapFrom<NewBrand>
    {
        public string BrandName { get; set; }
    }
}
