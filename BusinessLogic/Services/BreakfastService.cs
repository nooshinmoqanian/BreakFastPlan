using AutoMapper;
using BusinessLogic.DTO;
using BusinessLogic.Interfaces;
using DataAccess.DBContexts;
using DataAccess.Models;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if (breakfast != null)
            {
                var mapBreakfast = _mapper.Map<Breakfast>(breakfast);

                var breakfastTags = _mapper.Map<List<Tag>>(breakfast.TagNames);

                mapBreakfast.Name = breakfast.Name;

                mapBreakfast.Description = breakfast.Description;

                mapBreakfast.Price = breakfast.Price;

                mapBreakfast.BreakfastTags = new List<BreakfastTag>();

                var tagNames = new List<string> { breakfast.Name };

                foreach (var tagName in tagNames)
                {
                    var tag = await _breakfastTagRepository.GetByNameAsync(tagName);

                    if (tag.Id == 0)
                    {
                       await _breakfastTagRepository.CreateAsync(tag);
                    }

                    var breakfastTag = new BreakfastTag
                    {
                        Tag = tag 
                    };

                    mapBreakfast.BreakfastTags.Add(breakfastTag);

                    var addBreakfastResult = await _breakfastRepository.CreateAsync(mapBreakfast);

                    if (addBreakfastResult.Success)
                    {
                        return addBreakfastResult.Message; 
                    }

                    return addBreakfastResult.Message;
                }
            }
            return "There was a problem registering breakfast";
        }
        public async Task<IEnumerable<Breakfast>> GetBreakfastList() 
        {
            return await _breakfastRepository.GetAllAsync();
        }
    }
}
