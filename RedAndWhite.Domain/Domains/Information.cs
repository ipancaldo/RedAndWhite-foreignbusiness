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

        public string? Image { get; set; }

        public void Create(NewInformation newInformation)
        {
            Title = newInformation.Title;
            Text = newInformation.Text;
            if (!String.IsNullOrEmpty(newInformation.Image))
                Image = newInformation.Image;
        }

        public void ModifyProperties(NewInformation newInformation)
        {
            if (newInformation.Title.Count() < 2)
                throw new Exception($"The title cannot be less than 2 characters.");
            if (Title.ToLower() != newInformation.Title.ToLower())
                Title = newInformation.Title;

            if (newInformation.Text.Count() < 2)
                throw new Exception($"The text cannot contain less than 10 characters.");
            if (Text.ToLower() != newInformation.Text.ToLower())
                Text = newInformation.Text;

            if (!String.IsNullOrEmpty(newInformation.Image))
                Image = newInformation.Image;
        }
    }
}
