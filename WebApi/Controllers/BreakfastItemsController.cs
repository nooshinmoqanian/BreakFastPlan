using BusinessLogic.DTO;
using BusinessLogic.Interfaces;
using DataAccess.Models;
using Microsoft.AspNetCore.Http;
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
        public async Task <IEnumerable<Breakfast>> GetAll() 
        {
            return await _breakfastService.GetBreakfastList();
        }
        [HttpPost("add-breakfast")]
        public async Task<IActionResult> AddBreakfast(BreakfastListDto breakfastList)
        {
            if(ModelState.IsValid)
              return Ok( await _breakfastService.AddBreakfast(breakfastList) );

            return BadRequest(ModelState);
        }
    }
}
