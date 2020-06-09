using deliSHAs.v2.api.restaurant.Models;
using deliSHAs.v2.api.restaurant.Models.exception;
using deliSHAs.v2.api.restaurant.Services.cache;
using deliSHAs.v2.api.restaurant.Services.store.restaurant;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace deliSHAs.v2.api.restaurant.Services.retaurant.service
{
    public class RestaurantService: IRestaurantService
    {
        private InMemoryCache _restaurantCache;
        private readonly IMySqlRestaurantDataService _mySqlRestaurantDataService; 

        public RestaurantService(IMySqlRestaurantDataService mySqlRestaurantDataService)
        {
            _restaurantCache = new InMemoryCache { };
            _mySqlRestaurantDataService = mySqlRestaurantDataService;
        }

        public async Task<List<Restaurant>> GetRestaurants()
        {
            var restaurants = await _restaurantCache.Get<List<Restaurant>>("");

            if (restaurants.Count <= 0 || restaurants == null)
            {
                restaurants = _mySqlRestaurantDataService.GetRestaurants();
                if (restaurants.Count <= 0 || restaurants == null)
                    throw new RestaurantNotFoundException("");
            }

            return restaurants;
        }
    }
}
