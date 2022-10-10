﻿using RedAndWhite.Domain.ValueObjects.Category;
using RedAndWhite.Infrastructure.Mapping;

namespace RedAndWhite.Model.Categories
{
    public class NewCategoryModel : IMapFrom<NewCategory>
    {
        public string CategoryName { get; set; }
    }
}
