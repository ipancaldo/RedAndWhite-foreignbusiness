using RedAndWhite.Domain;
using RedAndWhite.Model.Informations;

namespace RedAndWhite.Service.Informations
{
    public interface IInformationService : IServiceBase<Information>
    {
        InformationModel GetLastInformation();
        void Create(NewInformationModel newInformationModel);
        void Delete(int id);
        void ModifyProperties(ModifyInformationModel modifyInformationModel);
    }
}
