namespace RedAndWhite.Domain.DomainServices
{
    public interface IBrandDomainService : IDomainService
    {
        Brand GetOrCreateBrandByName(string brandName);
    }
}
