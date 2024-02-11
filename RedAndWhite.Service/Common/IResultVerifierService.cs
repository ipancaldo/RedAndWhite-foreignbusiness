using RedAndWhite.Model.Shared;

namespace RedAndWhite.Service.Common
{
    public interface IResultVerifierService
    {
        void IfNullThrowException<T>(T entity, string type);

        ResultDTO<T> IfEmptyReturnFailed<T>(List<T> entity);

        ResultDTO<T> IfExistsReturnFailed<T>(T entity);
    }
}