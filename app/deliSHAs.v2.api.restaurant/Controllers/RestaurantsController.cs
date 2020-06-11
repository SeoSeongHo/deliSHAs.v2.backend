using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using deliSHAs.v2.api.restaurant.Models;
using deliSHAs.v2.api.restaurant.Services.retaurant.service;
using deliSHAs.v2.api.restaurant.Services.store.restaurant;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace deliSHAs.v2.api.restaurant.Controllers
{
    [Route("api/v2/restaurants")]
    public class RestaurantsController : Controller
    {
        private readonly IRestaurantService restaurantService;

        public RestaurantsController(IRestaurantService restaurantService)
        {
            this.restaurantService = restaurantService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRestaurants()
        {
            var restaurants = await restaurantService.GetRestaurants();

            return Ok(restaurants);
        }
    }
}
