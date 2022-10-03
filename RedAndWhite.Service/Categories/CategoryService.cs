using AutoMapper;
using RedAndWhite.Domain;
using RedAndWhite.Domain.DomainServices;
using RedAndWhite.Domain.ValueObjects.Category;
using RedAndWhite.Repository.Categories;
using System.Linq.Expressions;

namespace RedAndWhite.Service.Categories
{
    public class CategoryService : ServiceBase<Category, ICategoryRepository>, ICategoryService, ICategoryDomainService
    {
        public CategoryService(ICategoryRepository repository, IMapper mapper) 
            : base(repository, mapper)
        {
        }

        public Category GetById(GetCategoryById getCategoryById)
        {
            return base.Repository.GetEntityByCriteria(GetByIdEvaluator(getCategoryById.CategoryId));
        }

        private Expression<Func<Category, bool>> GetByIdEvaluator(int id) => category => category.Id.Equals(id);


        public Category GetOrCreateByName(CategoryToGet categoryToGet)
        {
            var category = base.Repository.GetEntityByCriteria(GetByNameEvaluator(categoryToGet.CategoryName));
            if (category != null) return category;

            this.Aggregate.Create(categoryToGet);
            return this.Aggregate;
        }

        private Expression<Func<Category, bool>> GetByNameEvaluator(string categoryName) => category => category.Name.ToLower() == categoryName.ToLower();
    }
}
