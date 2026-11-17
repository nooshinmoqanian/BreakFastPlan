using AutoMapper;
using BusinessLogic.DTO;
using DataAccess.Models;

namespace BusinessLogic.Mapping
{
    public class BreakfastProfile : Profile
    {
        public BreakfastProfile()
        {
            CreateMap<BreakfastIngredient, IngredientDto>().ReverseMap();
            CreateMap<BreakfastHealthInfo, HealthInfoDto>().ReverseMap();
            CreateMap<Breakfast, BreakfastDetailDto>()
                .ForMember(d => d.TagNames, o => o.MapFrom(b => b.BreakfastTags.Select(bt => new TagDto { Name = bt.Tag.Name })))
                .ForMember(d => d.TotalCalories, o => o.MapFrom(b => b.Ingredients.Sum(i => i.Calories)))
                .ForMember(d => d.IngredientsPrice, o => o.MapFrom(b => b.Ingredients.Sum(i => i.Price)))
                .ForMember(d => d.Restaurants, o => o.MapFrom(b => b.RestaurantBreakfasts));
            CreateMap<RestaurantBreakfast, BreakfastRestaurantDto>()
                .ForMember(d => d.RestaurantName, o => o.MapFrom(rb => rb.Restaurant!.Name))
                .ForMember(d => d.Type, o => o.MapFrom(rb => rb.Restaurant!.Type))
                .ForMember(d => d.Address, o => o.MapFrom(rb => rb.Restaurant!.Address))
                .ForMember(d => d.Phone, o => o.MapFrom(rb => rb.Restaurant!.Phone));
            CreateMap<Restaurant, RestaurantDto>()
                .ForMember(d => d.Menu, o => o.MapFrom(r => r.RestaurantBreakfasts));
            CreateMap<RestaurantBreakfast, MenuItemDto>()
                .ForMember(d => d.BreakfastName, o => o.MapFrom(rb => rb.Breakfast!.Name));
        }

    }
}
