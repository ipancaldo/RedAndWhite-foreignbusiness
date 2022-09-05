namespace RedAndWhite.Service
{
    public interface IServiceBase<TDomain>
    {
        IEnumerable<TDomain> GetAll();
    }
}
