using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace MetricsAgent.DAL
{
    public interface IRepository<T> where T : class
    {
        //void Create(T item);
        List<T> GetByTimePeriod(DateTimeOffset fromTime, DateTimeOffset toTime);

    }
}
