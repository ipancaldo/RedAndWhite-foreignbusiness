using AutoMapper;
using RedAndWhite.Domain;
using RedAndWhite.Domain.ValueObjects.Informations;
using RedAndWhite.Model.Informations;
using RedAndWhite.Repository.Informations;
using RedAndWhite.Service.Common;
using System.Linq.Expressions;

namespace RedAndWhite.Service.Informations
{
    public class InformationService : ServiceBase<Information, IInformationRepository>, IInformationService
    {
        private readonly IResultVerifier _resultVerifier;

        const string ProductType = "Information";

        public InformationService(IInformationRepository informationRepository,
                                  IResultVerifier resultVerifier,
                                  IMapper mapper) 
            : base(informationRepository, mapper)
        {
            _resultVerifier = resultVerifier;
        }

        public InformationModel GetLastInformation()
        {
            var informationModel = base.Mapper.Map<InformationModel>(base.Repository.OrderByDescending(GetByIdToStringCriteria()).FirstOrDefault());
            _resultVerifier.IfNullThrowException(informationModel, ProductType);

            return informationModel;
        }
        private Expression<Func<Information, string>> GetByIdToStringCriteria() => information => information.Id.ToString();

        public void Create(NewInformationModel newInformationModel)
        {
            base.Aggregate.Create(base.Mapper.Map<NewInformation>(newInformationModel));
            base.Repository.Add(base.Aggregate);
            base.Repository.SaveChanges();
        }

        public void Delete(int id)
        {
            var information = base.Repository.GetEntityByCriteria(GetById(id));
            _resultVerifier.IfNullThrowException(information, ProductType);

            base.Repository.Delete(information);
            base.Repository.SaveChanges();
        }
        private Expression<Func<Information, bool>> GetById(int id) => information => information.Id == id;

        public void ModifyProperties(ModifyInformationModel modifyInformationModel)
        {
            var information = base.Repository.GetEntityByCriteria(GetById(modifyInformationModel.Id));
            _resultVerifier.IfNullThrowException(information, ProductType);

            base.Aggregate = information;
            base.Aggregate.ModifyProperties(base.Mapper.Map<NewInformation>(modifyInformationModel));
            base.Repository.SaveChanges();
        }
    }
}
