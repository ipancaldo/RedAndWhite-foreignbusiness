using RedAndWhite.Data;
using RedAndWhite.Domain;

namespace RedAndWhite.Repository.Informations
{
    public class InformationRepository : BaseRepository<Information>, IInformationRepository
    {
        public InformationRepository(RedAndWhiteContext redAndWhiteContext) 
            : base(redAndWhiteContext)
        {
        }
    }
}
