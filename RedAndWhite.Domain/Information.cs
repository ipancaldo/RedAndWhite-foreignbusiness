using System.ComponentModel.DataAnnotations;

namespace RedAndWhite.Domain
{
    public class Information : BaseDomain
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Text { get; set; }

        public byte[]? Image { get; set; }
    }
}
