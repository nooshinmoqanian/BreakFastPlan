using BusinessLogic.DTO;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PlanController : ControllerBase
    {
        private readonly IPlanService _planService;

        public PlanController(IPlanService planService)
        {
            _planService = planService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _planService.GetPlan(User.Identity!.Name!));
        }

        [HttpPut("{day}")]
        public async Task<IActionResult> SetDay(int day, SetPlanDayDto dto)
        {
            if (day < 0 || day > 6) return BadRequest(new { message = "روز نامعتبر است" });

            if (await _planService.SetDay(User.Identity!.Name!, day, dto.BreakfastId)) return NoContent();

            return NotFound(new { message = "صبحانه پیدا نشد" });
        }

        [HttpDelete("{day}")]
        public async Task<IActionResult> ClearDay(int day)
        {
            await _planService.ClearDay(User.Identity!.Name!, day);

            return NoContent();
        }

        [HttpPost("order")]
        public async Task<IActionResult> Order(PlanOrderDto dto)
        {
            var order = await _planService.OrderShoppingList(User.Identity!.Name!, dto.Address);

            if (order == null) return BadRequest(new { message = "برنامه‌ی هفتگی شما خالی است" });

            return Ok(order);
        }
    }
}
