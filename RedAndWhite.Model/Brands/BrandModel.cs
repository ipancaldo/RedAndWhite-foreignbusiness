using RedAndWhite.Domain;
using RedAndWhite.Infrastructure.Mapping;

namespace RedAndWhite.Model.Brands
{
    public class BrandModel : IMapFrom<Brand>
    {
        public string BrandName { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Website { get; set; }
    }
}
