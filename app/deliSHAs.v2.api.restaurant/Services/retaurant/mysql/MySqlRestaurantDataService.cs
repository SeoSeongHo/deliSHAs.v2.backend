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

        public List<Restaurant> GetRestaurants()
        {
            var restaurantInfos = new List<RestaurantInfo>();
            var menus = new List<Menu>();
            var restaurants = new List<Restaurant>();

            using(MySqlConnection conn = new MySqlConnection(connectionStr))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;


                // 1. 식당 정보 가져오기
                cmd.CommandText = "get_restaurants_info_by_date";
                cmd.Parameters.Add("in_date", MySqlDbType.DateTime).Value = new DateTime(2020, 04, 21);

                using (MySqlDataReader reader = cmd.ExecuteReader()) 
                {
                    while (reader.Read())
                    {
                        restaurantInfos.Add(new RestaurantInfo { 
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
                

                // 2. 메뉴 가져오기
                cmd.CommandText = "get_restaurants_menu_by_date";

                using (MySqlDataReader reader = cmd.ExecuteReader()) 
                {
                    while (reader.Read()) 
                    {
                        menus.Add(new Menu
                        {
                            date = MySqlDBHelper.SafeRead<DateTime>(reader, "date"),
                            isValid = Convert.ToBoolean(MySqlDBHelper.SafeRead<UInt64>(reader, "is_valid")),
                            msg = MySqlDBHelper.SafeRead<string>(reader, "msg"),
                            name = MySqlDBHelper.SafeRead<string>(reader, "name"),
                            price = MySqlDBHelper.SafeRead<int>(reader, "price"),
                            restaurant_id = MySqlDBHelper.SafeRead<long>(reader, "restaurant_id")
                        });
                    }
                }
                

                // 3. 식당 정보, 메뉴를 합치기
                foreach(var restaurantInfo in restaurantInfos)
                {
                    var menu = menus?.Where(x => x.restaurant_id == restaurantInfo.id)?.ToList();

                    restaurants.Add(new Restaurant { 
                        id = restaurantInfo.id,
                        name = restaurantInfo.name,
                        place = restaurantInfo.place,
                        contact = restaurantInfo.contact,
                        breakfastTime = restaurantInfo.breakfastTime,
                        lunchTime = restaurantInfo.lunchTime,
                        dinnerTime = restaurantInfo.dinnerTime,
                        date = restaurantInfo.date,

                        breakfast = new Meal
                        {
                            menus = menu?.Where(x => x.mealTime == MealTime.BREAKFAST)?.ToList() ?? null,
                            message = menu?.FirstOrDefault()?.msg,
                            isValid = menu?.FirstOrDefault()?.isValid ?? true
                        },

                        lunch = new Meal
                        {
                            menus = menu?.Where(x => x.mealTime == MealTime.LUNCH)?.ToList() ?? null,
                            message = menu?.FirstOrDefault()?.msg,
                            isValid = menu?.FirstOrDefault()?.isValid ?? true
                        },

                        dinner = new Meal
                        {
                            menus = menu?.Where(x => x.mealTime == MealTime.DINNER)?.ToList() ?? null,
                            message = menu?.FirstOrDefault()?.msg,
                            isValid = menu?.FirstOrDefault()?.isValid ?? true
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
