using AutoMapper;
using BusinessLogic.DTO;
using DataAccess.Models;

namespace BusinessLogic.Mapping
{
    public class BreakfastProfile : Profile
    {
        public BreakfastProfile() 
        {
            CreateMap<Breakfast, BreakfastListDto>().ReverseMap();
            CreateMap<Tag, TagDto>().ReverseMap();
            CreateMap<TagDto, BreakfastTag>().ReverseMap();
        }

    }
}
