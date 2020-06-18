using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace deliSHAs.v2.api.restaurant.Models
{
    public class MenuDto
    {
        public long id { get; set; }
        public string name { get; set; }
        public int price { get; set; }
    }
}
