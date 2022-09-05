using System.ComponentModel.DataAnnotations;

namespace RedAndWhite.Domain
{
    public class Product : BaseDomain
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public byte[]? Image { get; set; }

        public virtual List<Category> Categories { get; set; }

        public virtual List<Brand> Brands { get; set; }
    }
}
