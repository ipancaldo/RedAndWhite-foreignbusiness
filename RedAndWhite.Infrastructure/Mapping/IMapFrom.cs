using AutoMapper;

namespace RedAndWhite.Infrastructure.Mapping
{
    public interface IMapFrom<T>
    {
        public virtual void Mapping(Profile profile) => profile.CreateMap(typeof(T), this.GetType()).ReverseMap();
    }
}
