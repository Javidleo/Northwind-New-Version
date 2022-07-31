using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Mapping;
using AutoMapper;
using DomainModel.Entities;

namespace Application.Categories.Queries.GetAll
{
    public class CategoryViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public byte[] Picture { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Category, CategoryViewModel>()
                .ForMember(i => i.Name, option => option.MapFrom(i => i.CategoryName));
        }
    }
}
