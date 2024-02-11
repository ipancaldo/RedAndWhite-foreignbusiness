using RedAndWhite.Infrastructure.Enums;
using RedAndWhite.Model.Shared;

namespace RedAndWhite.Service.Common
{
    public class ResultVerifierService : IResultVerifierService
    {
        public ResultVerifierService() { }

        public void IfNullThrowException<T>(T entity, string type) //TODO: Modify
        {
            if (entity is null)
                throw new Exception($"{type} don't exist");
        }

        public ResultDTO<T> IfEmptyReturnFailed<T>(List<T> entities)
        {
            if (!entities.Any())
                return new ResultDTO<T>(ResultStatusEnum.Failed, "There were no coincidences");

            return new ResultDTO<T>();
        }

        public ResultDTO<T> IfExistsReturnFailed<T>(T entity)
        {
            if (entity is not null)
                 return new ResultDTO<T>(ResultStatusEnum.Failed, $"{typeof(T)} already exist");

            return new ResultDTO<T>();
        }
    }
}