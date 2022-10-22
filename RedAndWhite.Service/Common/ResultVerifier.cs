namespace RedAndWhite.Service.Common
{
    public class ResultVerifier : IResultVerifier
    {
        public ResultVerifier() { }

        public void IfNullThrowException<T>(T entity, string type)
        {
            if (entity is null)
                throw new Exception($"{type} don't exist");
        }

        public void IfEmptyThrowException<T>(List<T> entity)
        {
            if (!entity.Any())
                throw new Exception("There were no coincidences");
        }

        public void IfExistsThrowException<T>(T entity, string type)
        {
            if (entity is not null)
                throw new Exception($"{type} already exist.");
        }
    }
}
