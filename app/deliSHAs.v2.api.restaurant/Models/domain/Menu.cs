using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace deliSHAs.v2.api.restaurant.Models
{
    public class Menu
    {
        public long id { get; set; }
        public DateTime date { get; set; }
        public bool isValid { get; set; }
        public MealTime mealTime { get; set; }
        public string msg { get; set; }
        public string name { get; set; }
        public int price { get; set; }
        public long restaurant_id { get; set; }

        public static MenuDto ToDto(Menu menu)
        {
            return new MenuDto
            {
                id = menu.id,
                name = menu.name,
                price = menu.price
            };
        }
    }

    public enum MealTime
    {
        BREAKFAST,
        LUNCH,
        DINNER
    }
}
