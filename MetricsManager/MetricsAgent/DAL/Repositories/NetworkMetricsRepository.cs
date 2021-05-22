using Dapper;
using MetricsAgent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace MetricsAgent.DAL.Repositories
{
    public class NetworkMetricsRepository : INetworkMetricsRepository
    {
        private readonly ISqlConnectionProvider _provider;
        public NetworkMetricsRepository(ISqlConnectionProvider provider)
        {
            _provider = provider;
        }

        public List<NetworkMetric> GetByTimePeriod(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            var ConnectionString = _provider.GetConnectionString();
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<NetworkMetric>("SELECT * FROM networkmetrics WHERE time >= @fromTime AND time <= @toTime",
                    new
                    {
                        fromTime = fromTime.ToUnixTimeSeconds(),
                        toTime = toTime.ToUnixTimeSeconds()
                    }).ToList();
            }            
        }
    }
}
