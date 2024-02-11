using RedAndWhite.Domain;
using RedAndWhite.Model.Informations;

namespace RedAndWhite.Service.Informations
{
    public interface IInformationService : IServiceBase<Information>
    {
        InformationModel GetLastInformation();
        Task Create(NewInformationModel newInformationModel);
        Task Delete(int id);
        Task Update(ModifyInformationModel modifyInformationModel);
    }
}
