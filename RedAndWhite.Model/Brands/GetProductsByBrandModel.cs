using RedAndWhite.Domain.ValueObjects.Brand;
using RedAndWhite.Domain.ValueObjects.Category;
using RedAndWhite.Infrastructure.Loaders;
using RedAndWhite.Infrastructure.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedAndWhite.Model.Brands
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
