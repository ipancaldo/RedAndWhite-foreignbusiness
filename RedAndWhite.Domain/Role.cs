using System.ComponentModel.DataAnnotations;

namespace RedAndWhite.Domain
{
    public class Role : BaseDomain
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual List<User> Users { get; set; }

    }
}
