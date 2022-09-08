using RedAndWhite.Data;
using RedAndWhite.Domain;

namespace RedAndWhite.Repository.Brands
{
    public class BrandsRepository : BaseRepository<Brand>, IBrandsRepository
    {
        public BrandsRepository(RedAndWhiteContext redAndWhiteContext) 
            : base(redAndWhiteContext)
        {
        }
    }
}
