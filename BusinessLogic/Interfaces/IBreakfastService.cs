using BusinessLogic.DTO;


namespace BusinessLogic.Interfaces
{
    public interface IBreakfastService
    {
        Task<IEnumerable<BreakfastDetailDto>> GetBreakfastList();
        Task<BreakfastDetailDto?> GetBreakfast(int id);
        Task<string> AddBreakfast(BreakfastListDto breakfast);
        Task<bool> UpdateBreakfast(int id, BreakfastListDto breakfast);
        Task<bool> DeleteBreakfast(int id);
    }
}
