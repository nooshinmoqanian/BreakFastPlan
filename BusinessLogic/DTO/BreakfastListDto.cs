using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTO
{
    public class BreakfastListDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public List<TagDto> TagNames { get; set; } 
    }
}
