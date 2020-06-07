using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace deliSHAs.v2.api.restaurant.Models
{
    public class Restaurant
    {
        public long id { get; set; }
        public string name { get; set; }
        public string place { get; set; }
        public string contact { get; set; }

        public string breakfastTime { get; set; }
        public string lunchTime { get; set; }
        public string dinnerTime { get; set; }

        public DateTime date { get; set; }

        public Meal breakfast { get; set; }
        public Meal lunch { get; set; }
        public Meal dinner { get; set; }

        public double latitude { get; set; }
        public double longitude { get; set; }
    }
}
