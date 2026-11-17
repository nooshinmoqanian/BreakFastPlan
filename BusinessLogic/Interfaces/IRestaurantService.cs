using BusinessLogic.DTO;

namespace BusinessLogic.Interfaces
{
    public interface IRestaurantService
    {
        Task<IEnumerable<RestaurantDto>> GetRestaurants();
        Task AddRestaurant(RestaurantDto restaurant);
        Task<bool> UpdateRestaurant(int id, RestaurantDto restaurant);
        Task<bool> DeleteRestaurant(int id);
    }
}
