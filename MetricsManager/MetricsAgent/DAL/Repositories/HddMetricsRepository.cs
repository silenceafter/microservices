using Dapper;
using MetricsAgent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace MetricsAgent.DAL.Repositories
{
    public class HddMetricsRepository : IHddMetricsRepository
    {
        private readonly ISqlConnectionProvider _provider;
        public HddMetricsRepository(ISqlConnectionProvider provider)
        {
            _provider = provider;
        }

        public List<HddMetric> GetByTimePeriod(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            var ConnectionString = _provider.GetConnectionString();
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<HddMetric>("SELECT * FROM hddmetrics WHERE time >= @fromTime AND time <= @toTime",
                    new
                    {
                        fromTime = fromTime.ToUnixTimeSeconds(),
                        toTime = toTime.ToUnixTimeSeconds()
                    }).ToList();
            }          
        }
    }
}
