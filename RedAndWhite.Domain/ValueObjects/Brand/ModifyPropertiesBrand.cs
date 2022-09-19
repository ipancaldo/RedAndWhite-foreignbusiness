using System.ComponentModel.DataAnnotations;

namespace RedAndWhite.Domain.ValueObjects.Brand
{
    public class ModifyPropertiesBrand : ValueObject
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; }
    }
}
