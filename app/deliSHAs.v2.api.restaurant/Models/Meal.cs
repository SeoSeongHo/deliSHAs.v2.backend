using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace deliSHAs.v2.api.restaurant.Models
{
    public class Meal
    {
        public List<Menu>? menus { get; set; }
        public string? message { get; set; }
        public bool isValid { get; set; }
    }
}
