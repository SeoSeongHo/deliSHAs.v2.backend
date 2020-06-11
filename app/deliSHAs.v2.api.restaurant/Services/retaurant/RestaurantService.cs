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

        public async Task<List<RestaurantDto>> GetRestaurants()
        {
            var restaurants = _restaurantCache.Get<List<RestaurantDto>>(DateTime.Now.ToString("yyyy-MM-dd"));

            if (restaurants == null || restaurants.Count <= 0)
            {
                restaurants = _mySqlRestaurantDataService.GetRestaurants();
                // TODO Exception 처리하기
                if (restaurants.Count <= 0 || restaurants == null)
                    throw new RestaurantNotFoundException($"can not find any restaurants.");

                bool isCreated = await _restaurantCache.Create(restaurants.FirstOrDefault().date.ToString("yyyy-MM-dd"), restaurants);

                if (isCreated)
                    throw new CreateCacheException($"failed to create cache.");
            }

            return restaurants;
        }
    }
}
