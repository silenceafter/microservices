using Dapper;
using MetricsAgent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace MetricsAgent.DAL.Repositories
{
    public class CpuMetricsRepository : ICpuMetricsRepository
    {
        private readonly ISqlConnectionProvider _provider;                
        //инжектируем соединение с бд в наш репозиторий через конструктор
        public CpuMetricsRepository(ISqlConnectionProvider provider)
        {
            _provider = provider;
        }

        public void Create(CpuMetric item)
        {
            var ConnectionString = _provider.GetConnectionString();
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("INSERT INTO cpumetrics(value, time) VALUES(@value,@time)",
                new
                {
                    value = item.Value, 
                    time = item.Time
                });
            }
        }

        public List<CpuMetric> GetByTimePeriod(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            var ConnectionString = _provider.GetConnectionString();
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<CpuMetric>("SELECT * FROM cpumetrics WHERE time >= @fromTime AND time <= @toTime",
                    new
                    {
                        fromTime = fromTime.ToUnixTimeSeconds(),
                        toTime = toTime.ToUnixTimeSeconds()
                    }).ToList();
            }         
        }
    }
}