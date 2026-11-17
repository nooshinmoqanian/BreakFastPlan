using BusinessLogic.DTO;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderDto order)
        {
            var newOrder = await _orderService.CreateOrder(User.Identity!.Name!, order);

            if (newOrder == null) return BadRequest(new { message = "این سفارش در دسترس نیست" });

            return Ok(newOrder);
        }

        [HttpGet("mine")]
        public async Task<IActionResult> Mine()
        {
            return Ok(await _orderService.GetMyOrders(User.Identity!.Name!));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _orderService.GetAllOrders());
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus(int id, UpdateOrderStatusDto dto)
        {
            if (await _orderService.UpdateStatus(id, dto.Status)) return NoContent();

            return NotFound();
        }
    }
}
