﻿using RedAndWhite.Domain.ValueObjects.Brand;
using RedAndWhite.Infrastructure.Mapping;

namespace RedAndWhite.Model.Brands
{
    public class NewBrandModel : IMapFrom<NewBrand>
    {
        public string BrandName { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Website { get; set; }
    }
}
