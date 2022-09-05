using System.ComponentModel.DataAnnotations;

namespace RedAndWhite.Domain
{
    public class Brand : BaseDomain
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual List<Product> Products { get; set; }

        public virtual List<Category> Categories { get; set; }
    }
}
