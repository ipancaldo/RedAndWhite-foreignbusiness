using RedAndWhite.Domain.ValueObjects.Image;
using System.ComponentModel.DataAnnotations;

namespace RedAndWhite.Domain.Domains
{
    public class Image
    {
        public Image() { }

        public Image(string image)
        {
            Img = image;
        }

        [Key]
        public int Id { get; set; }

        public string Img { get; set; }

        public virtual List<Information>? Informations { get; set; }
    }
}
