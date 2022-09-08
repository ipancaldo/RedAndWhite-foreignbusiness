using System.ComponentModel.DataAnnotations;
using System.Dynamic;

namespace RedAndWhite.Domain
{
    public class Brand : BaseDomain
    {
        public Brand()
        {
            this.Products = new List<Product>();
            this.Categories = new List<Category>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual List<Product> Products { get; set; }

        public virtual List<Category> Categories { get; set; }

        public void Create(string brandName)
        {
            this.Name = brandName;
        }
    }
}
