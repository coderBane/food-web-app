using AutoMapper;
using Foody.Entities.DTOs;
using Foody.Entities.Models;

namespace Foody.WebApi.Profiles
{
    public class ItemProfile : Profile
    {
        public ItemProfile()
        {
            CreateMap<Category, CategoryDto>()
                .ReverseMap();
                
            CreateMap<Category, CategoryDetailDto>();

        }
    }
}

