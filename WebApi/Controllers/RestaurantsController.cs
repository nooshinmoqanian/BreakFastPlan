using BusinessLogic.DTO;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantsController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpGet]
        public async Task<IEnumerable<RestaurantDto>> GetAll()
        {
            return await _restaurantService.GetRestaurants();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(RestaurantDto restaurant)
        {
            await _restaurantService.AddRestaurant(restaurant);

            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, RestaurantDto restaurant)
        {
            if (await _restaurantService.UpdateRestaurant(id, restaurant)) return NoContent();

            return NotFound();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _restaurantService.DeleteRestaurant(id)) return NoContent();

            return NotFound();
        }
    }
}
