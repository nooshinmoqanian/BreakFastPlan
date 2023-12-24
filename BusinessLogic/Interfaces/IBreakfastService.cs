using BusinessLogic.DTO;
using DataAccess.Models;


namespace BusinessLogic.Interfaces
{
    public interface IBreakfastService
    {
        Task<IEnumerable<Breakfast>> GetBreakfastList();
        Task<string> AddBreakfast(BreakfastListDto breakfast);
    }
}
