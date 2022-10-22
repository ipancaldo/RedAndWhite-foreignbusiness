namespace RedAndWhite.Service.Common
{
    public interface IResultVerifier
    {
        void IfNullThrowException<T>(T entity, string type);

        void IfEmptyThrowException<T>(List<T> entity);

        void IfExistsThrowException<T>(T entity, string type);
    }
}
