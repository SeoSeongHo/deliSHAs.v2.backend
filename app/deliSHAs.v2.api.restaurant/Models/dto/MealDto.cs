using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace deliSHAs.v2.api.restaurant.Models
{
    public class MealDto
    {
        public List<MenuDto> menus { get; set; }
        public string message { get; set; }
        public bool isValid { get; set; }
    }
}
