using deliSHAs.v2.api.restaurant.Models;
using deliSHAs.v2.api.restaurant.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace deliSHAs.v2.api.restaurant.Services.store.restaurant
{
    public class MySqlRestaurantDataService : IMySqlRestaurantDataService
    {
        private readonly string connectionStr;
        public MySqlRestaurantDataService(string connectionStr)
        {
            this.connectionStr = connectionStr;
        }

        public List<RestaurantDto> GetRestaurants()
        {
            var restaurantInfos = new List<Restaurant>();
            var menus = new List<Menu>();
            var restaurants = new List<RestaurantDto>();

            using(MySqlConnection conn = new MySqlConnection(connectionStr))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;


                // 1. 식당 정보 가져오기
                cmd.CommandText = "get_restaurants_info_by_date";
                // TEST 용
                Console.WriteLine($"DateTIme : {DateTime.UtcNow.AddHours(9).ToString("yyyy-MM-dd")}");
                cmd.Parameters.Add("in_date", MySqlDbType.DateTime).Value = DateTime.UtcNow.AddHours(9).ToString("yyyy-MM-dd");

                using (MySqlDataReader reader = cmd.ExecuteReader()) 
                {
                    while (reader.Read())
                    {
                        restaurantInfos.Add(new Restaurant { 
                            id = MySqlDBHelper.SafeRead<long>(reader, "id"),
                            name = MySqlDBHelper.SafeRead<string>(reader, "name"),
                            contact = MySqlDBHelper.SafeRead<string>(reader, "contact"),
                            place = MySqlDBHelper.SafeRead<string>(reader, "place"),
                            breakfastTime = MySqlDBHelper.SafeRead<string>(reader, "breakfast_time"),
                            lunchTime = MySqlDBHelper.SafeRead<string>(reader, "lunch_time"),
                            dinnerTime = MySqlDBHelper.SafeRead<string>(reader, "dinner_time"),
                            latitude = MySqlDBHelper.SafeRead<double>(reader, "latitude"),
                            longitude = MySqlDBHelper.SafeRead<double>(reader, "longitude"),
                            date = MySqlDBHelper.SafeRead<DateTime>(reader, "date")
                        });
                    }
                }

                var sortedRestaurantInfos = restaurantInfos.OrderBy(restaurant => restaurant.name).ToList();
                

                // 2. 메뉴 가져오기
                cmd.CommandText = "get_restaurants_menu_by_date";

                using (MySqlDataReader reader = cmd.ExecuteReader()) 
                {
                    while (reader.Read()) 
                    {
                        menus.Add(new Menu
                        {
                            id = MySqlDBHelper.SafeRead<long>(reader, "id"),
                            date = MySqlDBHelper.SafeRead<DateTime>(reader, "date"),
                            isValid = Convert.ToBoolean(MySqlDBHelper.SafeRead<UInt64>(reader, "is_valid")),
                            mealTime = (MealTime)Enum.Parse(typeof(MealTime), MySqlDBHelper.SafeRead<string>(reader, "meal_time")),
                            msg = MySqlDBHelper.SafeRead<string>(reader, "msg"),
                            name = MySqlDBHelper.SafeRead<string>(reader, "name"),
                            price = MySqlDBHelper.SafeRead<int>(reader, "price"),
                            restaurant_id = MySqlDBHelper.SafeRead<long>(reader, "restaurant_id")
                        });
                    }
                }
                

                // 3. 식당 정보, 메뉴를 합치기
                foreach(var restaurantInfo in sortedRestaurantInfos)
                {
                    var targetMenu = menus?.Where(x => x.restaurant_id == restaurantInfo.id)?.ToList();

                    var breakfasts = targetMenu?.Where(x => x.mealTime == MealTime.BREAKFAST)?.ToList() ?? null;
                    var lunches = targetMenu?.Where(x => x.mealTime == MealTime.LUNCH)?.ToList() ?? null;
                    var dinners = targetMenu?.Where(x => x.mealTime == MealTime.DINNER)?.ToList() ?? null;

                    restaurants.Add(new RestaurantDto { 
                        id = restaurantInfo.id,
                        name = restaurantInfo.name,
                        place = restaurantInfo.place,
                        contact = restaurantInfo.contact,
                        breakfastTime = restaurantInfo.breakfastTime,
                        lunchTime = restaurantInfo.lunchTime,
                        dinnerTime = restaurantInfo.dinnerTime,
                        date = restaurantInfo.date,

                        breakfast = new MealDto
                        {
                            menus = (breakfasts != null && breakfasts.Count > 0) ? Enumerable.Range(0, breakfasts.Count).Select(i => Menu.ToDto(breakfasts[i])).ToList() : null,
                            message = breakfasts?.FirstOrDefault()?.msg,
                            isValid = breakfasts?.FirstOrDefault()?.isValid ?? true
                        },

                        lunch = new MealDto
                        {
                            menus = (lunches != null && lunches.Count > 0) ? Enumerable.Range(0, lunches.Count).Select(i => Menu.ToDto(lunches[i])).ToList() : null,
                            message = lunches?.FirstOrDefault()?.msg,
                            isValid = lunches?.FirstOrDefault()?.isValid ?? true
                        },

                        dinner = new MealDto
                        {
                            menus = (dinners != null && dinners.Count > 0) ? Enumerable.Range(0, dinners.Count).Select(i => Menu.ToDto(dinners[i])).ToList() : null,
                            message = dinners?.FirstOrDefault()?.msg,
                            isValid = dinners?.FirstOrDefault()?.isValid ?? true
                        },

                        latitude = restaurantInfo.latitude,
                        longitude = restaurantInfo.longitude
                    });
                }
            }

            return restaurants;
        }
    }
}
