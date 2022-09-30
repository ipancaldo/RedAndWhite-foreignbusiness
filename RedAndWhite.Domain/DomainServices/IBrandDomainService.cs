using RedAndWhite.Domain.ValueObjects.Brand;

namespace RedAndWhite.Domain.DomainServices
{
    public interface IBrandDomainService : IDomainService
    {
        Brand GetOrCreateByName(NewBrand newBrand);

        Brand GetById(GetBrandById id);
        //Brand GetByIds(GetBrandsById id);
    }
}
