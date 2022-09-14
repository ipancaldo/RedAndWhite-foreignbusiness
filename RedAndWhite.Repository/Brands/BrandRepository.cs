using RedAndWhite.Data;
using RedAndWhite.Domain;

namespace RedAndWhite.Repository.Brands
{
    public class BrandRepository : BaseRepository<Brand>, IBrandRepository
    {
        public BrandRepository(RedAndWhiteContext redAndWhiteContext) 
            : base(redAndWhiteContext)
        {
        }
    }
}
