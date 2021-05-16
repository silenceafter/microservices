using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
using MetricsAgent.Models;

namespace MetricsAgent.DAL
{
    public interface INetworkMetricsRepository : IRepository<NetworkMetric>
    {
    }

    public class NetworkMetricsRepository : INetworkMetricsRepository
    {
        private const string ConnectionString = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100;";

        public List<NetworkMetric> GetByTimePeriod(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using var connection = new SQLiteConnection(ConnectionString);
            connection.Open();
            using var cmd = new SQLiteCommand(connection);
            cmd.CommandText = "SELECT * FROM networkmetrics WHERE time >= @fromTime AND time <= @toTime";
            cmd.Parameters.AddWithValue("@fromTime", fromTime.ToUnixTimeSeconds());
            cmd.Parameters.AddWithValue("@toTime", toTime.ToUnixTimeSeconds());

            List<NetworkMetric> myArray = new List<NetworkMetric>();
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    myArray.Add(new NetworkMetric
                    {
                        Id = reader.GetInt32(0),
                        Value = reader.GetInt32(1),
                        Time = reader.GetInt64(2)
                    });
                }
            }
            return myArray;
        }
    }
}
