using AutoMapper;
using BusinessLogic.DTO;
using BusinessLogic.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories;

namespace BusinessLogic.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRepositories<Restaurant> _restaurantRepository;
        private readonly IMapper _mapper;

        public RestaurantService(IRepositories<Restaurant> repositories, IMapper mapper)
        {
            _restaurantRepository = repositories;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RestaurantDto>> GetRestaurants()
        {
            var restaurants = await _restaurantRepository.GetAllAsync();

            return _mapper.Map<List<RestaurantDto>>(restaurants);
        }

        public async Task AddRestaurant(RestaurantDto restaurant)
        {
            var newRestaurant = new Restaurant();

            CopyToRestaurant(restaurant, newRestaurant);

            await _restaurantRepository.CreateAsync(newRestaurant);
        }

        public async Task<bool> UpdateRestaurant(int id, RestaurantDto restaurant)
        {
            var findRestaurant = await _restaurantRepository.GetByIdAsync(id);

            if (findRestaurant.Id == 0) return false;

            CopyToRestaurant(restaurant, findRestaurant);

            var updateResult = await _restaurantRepository.UpdateAsync(findRestaurant);

            return updateResult.Success;
        }

        public async Task<bool> DeleteRestaurant(int id)
        {
            var deleteResult = await _restaurantRepository.DeleteAsync(id);

            return deleteResult.Success;
        }

        private static void CopyToRestaurant(RestaurantDto source, Restaurant target)
        {
            target.Name = source.Name;
            target.Type = source.Type;
            target.Address = source.Address;
            target.Phone = source.Phone;

            var menu = source.Menu.GroupBy(m => m.BreakfastId).Select(g => g.Last()).ToList();

            target.RestaurantBreakfasts.RemoveAll(rb => menu.All(m => m.BreakfastId != rb.BreakfastId));

            foreach (var item in menu)
            {
                var existing = target.RestaurantBreakfasts.FirstOrDefault(rb => rb.BreakfastId == item.BreakfastId);

                if (existing != null)
                    existing.Price = item.Price;
                else
                    target.RestaurantBreakfasts.Add(new RestaurantBreakfast { BreakfastId = item.BreakfastId, Price = item.Price });
            }
        }
    }
}
