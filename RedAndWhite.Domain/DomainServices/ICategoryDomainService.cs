using RedAndWhite.Domain.ValueObjects.Category;

namespace RedAndWhite.Domain.DomainServices
{
    public interface ICategoryDomainService : IDomainService
    {
        Category GetByName(CategoryToGet categoryToGet);

        Category GetById(GetCategoryById id);
    }
}
