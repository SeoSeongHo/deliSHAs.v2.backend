using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace deliSHAs.v2.api.restaurant.Utils
{
    public static class MySqlDBHelper
    {
        public static T SafeRead<T>(MySqlDataReader reader, string column)
        {
            var index = reader.GetOrdinal(column);
            if (!reader.IsDBNull(index))
                return reader.GetFieldValue<T>(index);
            else
                return default(T);
        }
    }
}

