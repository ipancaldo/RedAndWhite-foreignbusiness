using RedAndWhite.Data;
using RedAndWhite.Domain;

namespace RedAndWhite.Repository.Products
{
    public class ProductsRepository : BaseRepository<Product>, IProductsRepository
    {
        public ProductsRepository(RedAndWhiteContext redAndWhiteContext) 
            : base(redAndWhiteContext)
        {

        }
    }
}
