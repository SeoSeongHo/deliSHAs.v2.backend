using deliSHAs.v2.api.restaurant.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace deliSHAs.v2.api.restaurant.Services.store.restaurant
{
    public class RestaurantService : IRestaurantService
    {
        private readonly string connectionStr;
        public RestaurantService(string connectionStr)
        {
            this.connectionStr = connectionStr;
        }

        public List<Restaurant> GetRestaurants()
        {
            var restaurants = new List<Restaurant>();

            using(MySqlConnection conn = new MySqlConnection(connectionStr))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "get_restaurants_by_date";

                cmd.Parameters.Add("in_date", MySqlDbType.Date).Value = new DateTime(2020,04,21);

                using (MySqlDataReader reader = cmd.ExecuteReader()) 
                {
                    while (reader.Read()) 
                    {
                        var restaurant = new Restaurant
                        {

                        };
                    }
                }
            }

            return new List<Restaurant>();
        }
    }
}
