using System.ComponentModel.DataAnnotations;

namespace RedAndWhite.Domain.ValueObjects.Product
{
    public class ModifyPropertiesProduct : ValueObject
    {
        [Required]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }
    }
}
