using AutoMapper;
using BusinessLogic.DTO;
using BusinessLogic.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories;

namespace BusinessLogic.Services
{
    public class BreakfastService : IBreakfastService
    {
        private readonly IRepositories<Breakfast> _breakfastRepository;
        private readonly IRepositories<Tag> _breakfastTagRepository;
        private readonly IMapper _mapper;


        public BreakfastService(IRepositories<Breakfast> repositories, IMapper mapper, IRepositories<Tag> tagRepositories)
        {
            _breakfastRepository = repositories;
            _mapper = mapper;
            _breakfastTagRepository = tagRepositories;
        }

        public async Task<string> AddBreakfast(BreakfastListDto breakfast)
        {
            var mapBreakfast = new Breakfast();

            await CopyToBreakfast(breakfast, mapBreakfast);

            var addBreakfastResult = await _breakfastRepository.CreateAsync(mapBreakfast);

            return addBreakfastResult.Message;
        }

        public async Task<bool> UpdateBreakfast(int id, BreakfastListDto breakfast)
        {
            var findBreakfast = await _breakfastRepository.GetByIdAsync(id);

            if (findBreakfast.Id == 0) return false;

            await CopyToBreakfast(breakfast, findBreakfast);

            var updateResult = await _breakfastRepository.UpdateAsync(findBreakfast);

            return updateResult.Success;
        }

        public async Task<bool> DeleteBreakfast(int id)
        {
            var deleteResult = await _breakfastRepository.DeleteAsync(id);

            return deleteResult.Success;
        }

        public async Task<IEnumerable<BreakfastDetailDto>> GetBreakfastList()
        {
            var breakfasts = await _breakfastRepository.GetAllAsync();

            return _mapper.Map<List<BreakfastDetailDto>>(breakfasts);
        }

        public async Task<BreakfastDetailDto?> GetBreakfast(int id)
        {
            var breakfast = await _breakfastRepository.GetByIdAsync(id);

            if (breakfast.Id == 0) return null;

            return _mapper.Map<BreakfastDetailDto>(breakfast);
        }

        private async Task CopyToBreakfast(BreakfastListDto source, Breakfast target)
        {
            target.Name = source.Name;
            target.Description = source.Description;
            target.Price = source.Price;
            target.Recipe = source.Recipe;
            target.Ingredients = _mapper.Map<List<BreakfastIngredient>>(source.Ingredients);
            target.HealthInfos = _mapper.Map<List<BreakfastHealthInfo>>(source.HealthInfos);

            var tagNames = source.TagNames.Select(t => t.Name).Distinct().ToList();

            target.BreakfastTags.RemoveAll(bt => !tagNames.Contains(bt.Tag.Name));

            foreach (var tagName in tagNames.Where(n => target.BreakfastTags.All(bt => bt.Tag.Name != n)).ToList())
            {
                var tag = await _breakfastTagRepository.GetByNameAsync(tagName);

                if (tag.Id == 0)
                {
                    await _breakfastTagRepository.CreateAsync(tag);
                }

                target.BreakfastTags.Add(new BreakfastTag { Tag = tag });
            }
        }
    }
}
