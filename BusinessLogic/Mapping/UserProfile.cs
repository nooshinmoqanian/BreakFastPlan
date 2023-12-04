using AutoMapper;
using BusinessLogic.DTO;
using DataAccess.Models;

namespace BusinessLogic.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
           CreateMap<Users, RegisterDto>().ReverseMap();
           CreateMap<Users, LoginDto>().ReverseMap();
        }
    }
}
