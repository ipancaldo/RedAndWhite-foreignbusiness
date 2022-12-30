namespace RedAndWhite.Domain.ValueObjects.Informations
{
    public class NewInformation : ValueObject
    {
        public string Title { get; set; }

        public string Text { get; set; }

        public string? Image { get; set; }
    }
}
