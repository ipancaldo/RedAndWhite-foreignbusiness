using RedAndWhite.Domain;

namespace RedAndWhite.Service.Brands
{
    public interface IBrandService : IServiceBase<Brand>
    {
        Brand GetOrCreateBrandByName(string brandName);
    }
}
