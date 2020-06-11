using deliSHAs.v2.api.restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace deliSHAs.v2.api.restaurant.Services.retaurant.service
{
    public interface IRestaurantService
    {
        Task<List<RestaurantDto>> GetRestaurants();
    }
}
