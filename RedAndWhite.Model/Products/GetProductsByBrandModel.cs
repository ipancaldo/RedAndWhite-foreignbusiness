using RedAndWhite.Domain.ValueObjects.Brand;
using RedAndWhite.Infrastructure.Loaders;
using RedAndWhite.Infrastructure.Mapping;

namespace RedAndWhite.Model.Products
{
    public class GetProductsByBrandModel : ModelBase, IMapFrom<BrandToGet>
    {
        public GetProductsByBrandModel() { }

        public GetProductsByBrandModel(string brandName)
        {
            BrandName = brandName;
        }

        public string BrandName { get; set; }
    }
}
