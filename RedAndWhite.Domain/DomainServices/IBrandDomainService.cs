using RedAndWhite.Domain.ValueObjects.Brand;

namespace RedAndWhite.Domain.DomainServices
{
    public interface IBrandDomainService : IDomainService
    {
        Brand GetOrCreateBrandByName(NewBrand newBrand);
    }
}
