﻿using RedAndWhite.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace RedAndWhite.Domain
{
    public class Product : BaseDomain
    {
        public Product()
        {
            Categories = new List<Category>();
            Brands = new List<Brand>();
        }

        [Key]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; private set; }

        [Required, MaxLength(200)]
        public string Description { get; private set; }

        public byte[]? Image { get; set; }

        public virtual List<Category> Categories { get; private set; }

        public virtual List<Brand> Brands { get; private set; }

        public void Create(NewProduct newProduct)
        {
            this.Name = newProduct.Name;
            this.Description = newProduct.Description;
        }

        public void AssignBrand(Brand brand)
        {
            var isBrandInProduct = Brands.Any(b => b.Name == brand.Name);
            if (isBrandInProduct) return;

            Brands.Add(brand);
        }
    }
}
