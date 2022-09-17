namespace RedAndWhite.Domain.ValueObjects.Brand
{
    public class NewBrand : ValueObject
    {
        public NewBrand(string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }
    }
}
