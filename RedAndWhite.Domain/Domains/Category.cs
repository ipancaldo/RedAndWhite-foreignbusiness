﻿using System.ComponentModel.DataAnnotations;

namespace RedAndWhite.Domain
{
    public class Category : BaseDomain
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual List<Product> Products { get; set; }

        public virtual List<Brand> Brands { get; set; }
    }
}