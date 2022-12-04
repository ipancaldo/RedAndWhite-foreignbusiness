namespace RedAndWhite.Infrastructure.Loaders
{
    public class ModelLoader : IModelLoader
    {
        public TModel CreateModel<TModel>(params object[] args) where TModel : ModelBase
        {
            return (Activator.CreateInstance(typeof(TModel), args) as TModel)!;
        }

        public TModel CreateModel<TModel>() where TModel : ModelBase
        {
            return Activator.CreateInstance<TModel>();
        }
    }
}
