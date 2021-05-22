using Dapper;
using MetricsAgent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace MetricsAgent.DAL.Repositories
{
    public class DotNetMetricsRepository : IDotNetMetricsRepository
    {
        private readonly ISqlConnectionProvider _provider;
        public DotNetMetricsRepository(ISqlConnectionProvider provider)
        {
            _provider = provider;
        }

        public List<DotNetMetric> GetByTimePeriod(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            var ConnectionString = _provider.GetConnectionString();
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<DotNetMetric>("SELECT * FROM dotnetmetrics WHERE time >= @fromTime AND time <= @toTime",
                    new
                    {
                        fromTime = fromTime.ToUnixTimeSeconds(),
                        toTime = toTime.ToUnixTimeSeconds()
                    }).ToList();
            }            
        }
    }
}
