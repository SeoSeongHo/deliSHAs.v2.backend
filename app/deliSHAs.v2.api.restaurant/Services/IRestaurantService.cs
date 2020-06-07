using deliSHAs.v2.api.restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace deliSHAs.v2.api.restaurant.Services.store.restaurant
{
    public interface IRestaurantService
    {
        List<Restaurant> GetRestaurants();
    }
}
