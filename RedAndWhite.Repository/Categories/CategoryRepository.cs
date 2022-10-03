using RedAndWhite.Data;
using RedAndWhite.Domain;

namespace RedAndWhite.Repository.Categories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(RedAndWhiteContext redAndWhiteContext) 
            : base(redAndWhiteContext)
        {

        }
    }
}
