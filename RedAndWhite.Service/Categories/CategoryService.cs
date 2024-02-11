using AutoMapper;
using RedAndWhite.Domain;
using RedAndWhite.Domain.DomainServices;
using RedAndWhite.Domain.ValueObjects.Category;
using RedAndWhite.Model.Categories;
using RedAndWhite.Repository.Categories;
using RedAndWhite.Service.Common;
using System.Linq.Expressions;

namespace RedAndWhite.Service.Categories
{
    public class CategoryService : ServiceBase<Category, ICategoryRepository>, ICategoryService, ICategoryDomainService
    {
        private readonly IResultVerifierService _nullVerifier;
        const string CategoryType = "Category";

        public CategoryService(ICategoryRepository repository,
                               IResultVerifierService nullVerifier,
                               IMapper mapper)
            : base(repository, mapper)
        {
            _nullVerifier = nullVerifier;
        }

        public Category GetById(GetCategoryById getCategoryById)
        {
            var category = base.Repository.GetEntityByCriteria(GetByIdEvaluator(getCategoryById.CategoryId));
            _nullVerifier.IfNullThrowException(category, CategoryType);

            return category;
        }

        private Expression<Func<Category, bool>> GetByIdEvaluator(int id) => category => category.Id.Equals(id);

        public Category GetByName(CategoryToGet categoryToGet)
        {
            var category = base.Repository.GetEntityByCriteria(GetByNameEvaluator(categoryToGet.CategoryName));
            _nullVerifier.IfNullThrowException(category, CategoryType);

            return category;
        }

        public async Task Create(CategoryModel newCategoryModel)
        {
            var category = base.Repository.GetEntityByCriteria(GetByNameEvaluator(newCategoryModel.CategoryName));
            //_nullVerifier.IfExistsThrowException(category, CategoryType);

            base.Aggregate.Create(base.Mapper.Map<NewCategory>(newCategoryModel));
            await base.Repository.Add(base.Aggregate);

            await base.Repository.SaveChanges();
        }

        private Expression<Func<Category, bool>> GetByNameEvaluator(string categoryName) => category => category.Name.ToLower() == categoryName.ToLower();

        public List<Category> OrderBy()
        {
            return base.Repository.OrderBy(OrderByNameEvaluator()).ToList();
        }
        private Expression<Func<Category, string>> OrderByNameEvaluator() => category => category.Name;
    }
}
