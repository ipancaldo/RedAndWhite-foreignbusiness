using RedAndWhite.Domain.ValueObjects.Brand;

namespace RedAndWhite.Domain.DomainServices
{
    public interface IBrandDomainService : IDomainService
    {
        Brand GetByName(NewBrand newBrand);
        Brand GetById(GetBrandById id);
    }
}
