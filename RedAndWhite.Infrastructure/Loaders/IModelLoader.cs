namespace RedAndWhite.Infrastructure.Loaders
{
    public interface IModelLoader
    {
        TModel CreateModel<TModel>(params object[] args) where TModel : ModelBase;
        TModel CreateModel<TModel>() where TModel : ModelBase;
    }
}
