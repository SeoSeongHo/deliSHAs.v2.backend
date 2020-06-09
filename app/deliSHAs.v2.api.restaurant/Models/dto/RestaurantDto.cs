using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace deliSHAs.v2.api.restaurant.Models
{
    public class RestaurantDto
    {
        public long id { get; set; }
        public string name { get; set; }
        public string place { get; set; }
        public string contact { get; set; }

        public string breakfastTime { get; set; }
        public string lunchTime { get; set; }
        public string dinnerTime { get; set; }

        public DateTime date { get; set; }

        public MealDto breakfast { get; set; }
        public MealDto lunch { get; set; }
        public MealDto dinner { get; set; }

        public double latitude { get; set; }
        public double longitude { get; set; }
    }
}
