using AutoMapper;

namespace Application.Common.Mapping
{
    public interface IMapFrom<T> where T : class
    {
        void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
    }
}
