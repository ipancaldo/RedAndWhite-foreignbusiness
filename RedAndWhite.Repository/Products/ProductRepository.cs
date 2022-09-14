using RedAndWhite.Data;
using RedAndWhite.Domain;

namespace RedAndWhite.Repository.Products
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(RedAndWhiteContext redAndWhiteContext) 
            : base(redAndWhiteContext)
        {

        }
    }
}
