using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace deliSHAs.v2.api.restaurant.Models.exception
{
    /// <summary>
    /// 로거가 추가되면 예외 처리를 담당하는 필터
    /// </summary>
    public class ExceptionFilter
    {

    }

    public class RestaurantNotFoundException : Exception
    { 
        public RestaurantNotFoundException(string message) : base(message)
        {

        }
    }

    #region Cache Exception
    public class GetCacheException : Exception
    {
        public GetCacheException(string message) : base(message)
        {
            Console.WriteLine(message);
        }
    }

    public class CreateCacheException : Exception
    {
        public CreateCacheException(string message) : base(message)
        {
            Console.WriteLine(message);
        }
    }

    public class UpdateCacheException : Exception
    {
        public UpdateCacheException(string message) : base(message)
        {
            Console.WriteLine(message);
        }
    }
    #endregion
}
