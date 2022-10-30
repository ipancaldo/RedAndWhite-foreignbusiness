using RedAndWhite.Domain.ValueObjects.Category;

namespace RedAndWhite.Domain.DomainServices
{
    public interface ICategoryDomainService : IDomainService
    {
        Category GetById(GetCategoryById id);

        Category GetByName(CategoryToGet categoryToGet);
    }
}
