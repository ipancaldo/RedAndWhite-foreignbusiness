using AutoMapper;
using RedAndWhite.Domain;
using RedAndWhite.Domain.DomainServices;
using RedAndWhite.Domain.ValueObjects.Category;
using RedAndWhite.Model.Categories;
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

        public Category GetByName(CategoryToGet categoryToGet)
        {
            var category = base.Repository.GetEntityByCriteria(GetByNameEvaluator(categoryToGet.CategoryName));
            if (category != null) return category;

            throw new Exception("Category don't exist.");
        }

        public void Create(CategoryModel newCategoryModel)
        {
            var category = base.Repository.GetEntityByCriteria(GetByNameEvaluator(newCategoryModel.CategoryName));
            if (category is not null)
                throw new Exception("Category already exist.");

            this.Aggregate.Create(base.Mapper.Map<NewCategory>(newCategoryModel));
            base.Repository.Add(this.Aggregate);
            base.Repository.SaveChanges();
        }

        private Expression<Func<Category, bool>> GetByNameEvaluator(string categoryName) => category => category.Name.ToLower() == categoryName.ToLower();

        public List<Category> OrderBy()
        {
            return base.Repository.OrderBy(OrderByNameEvaluator()).ToList();
        }
        private Expression<Func<Category, string>> OrderByNameEvaluator() => category => category.Name;
    }
}
