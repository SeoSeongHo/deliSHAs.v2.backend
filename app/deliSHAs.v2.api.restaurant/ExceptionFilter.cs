using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace deliSHAs.v2.api.restaurant.Models.exception
{
    public class ExceptionFilter
    {

    }

    public class RestaurantNotFoundException : Exception
    { 
        public RestaurantNotFoundException(string message) : base(message)
        {

        }
    }
}
