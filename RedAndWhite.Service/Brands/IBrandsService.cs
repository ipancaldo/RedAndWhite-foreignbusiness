using RedAndWhite.Domain;

namespace RedAndWhite.Service.Brands
{
    public interface IBrandsService : IServiceBase<Brand>
    {
        Brand GetOrCreateBrandByName(string brandName);
    }
}
