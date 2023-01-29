using RedAndWhite.Domain.ValueObjects.Image;

namespace RedAndWhite.Domain.ValueObjects.Informations
{
    public class NewInformation : ValueObject
    {
        public string Title { get; set; }

        public string Text { get; set; }

        public List<CreateImage>? Images { get; set; }
    }
}
