using BusinessLogic.DTO;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BreakfastItemsController : ControllerBase
    {
        private readonly IBreakfastService _breakfastService;

        public BreakfastItemsController(IBreakfastService breakfastService)
        {
            _breakfastService = breakfastService;
        }

        [HttpGet("all-breakfast")]
        public async Task<IEnumerable<BreakfastDetailDto>> GetAll()
        {
            return await _breakfastService.GetBreakfastList();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var breakfast = await _breakfastService.GetBreakfast(id);

            if (breakfast == null) return NotFound();

            return Ok(breakfast);
        }

        [HttpPost("add-breakfast")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddBreakfast(BreakfastListDto breakfastList)
        {
            return Ok(await _breakfastService.AddBreakfast(breakfastList));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, BreakfastListDto breakfastList)
        {
            if (await _breakfastService.UpdateBreakfast(id, breakfastList)) return NoContent();

            return NotFound();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _breakfastService.DeleteBreakfast(id)) return NoContent();

            return NotFound();
        }
    }
}
