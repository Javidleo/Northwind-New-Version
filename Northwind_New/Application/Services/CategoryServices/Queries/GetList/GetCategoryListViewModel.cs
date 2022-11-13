using Application.Common.Mapping;
using AutoMapper;
using DomainModel.Entities;

namespace Application.Services.CategoryServices.Queries.GetList
{
    public class GetCategoryListViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Picture { get; set; }

        public int Count { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Category, GetCategoryListViewModel>()
                .ForMember(i => i.Name, c => c.MapFrom(i => i.CategoryName));
        }
    }
}
