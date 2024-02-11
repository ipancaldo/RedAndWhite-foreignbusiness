using RedAndWhite.Domain.Domains;
using RedAndWhite.Domain.ValueObjects.Informations;
using System.ComponentModel.DataAnnotations;

namespace RedAndWhite.Domain
{
    public class Information : BaseDomain
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Title { get; set; }

        [Required]
        public string Text { get; set; }

        public virtual List<Image>? Images { get; set; } = new();

        public void Create(NewInformation newInformation)
        {
            Title = newInformation.Title;
            Text = newInformation.Text;
            if (newInformation.Images.Any())
                newInformation.Images.ForEach(img => Images.Add(new Image(img.Image)));
        }

        public void UpdateProperties(NewInformation newInformation)
        {
            if (newInformation.Title.Count() < 2)
                throw new Exception($"The title cannot be less than 2 characters.");
            if (Title.ToLower() != newInformation.Title.ToLower())
                Title = newInformation.Title;

            if (newInformation.Text.Count() < 2)
                throw new Exception($"The text cannot contain less than 10 characters.");
            if (Text.ToLower() != newInformation.Text.ToLower())
                Text = newInformation.Text;

            //TODO: Make the proper modification of the Images for add/remove
        }
    }
}
