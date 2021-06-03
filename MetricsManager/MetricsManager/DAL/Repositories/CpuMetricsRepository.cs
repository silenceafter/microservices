using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Dapper;
using System.Linq;
using Microsoft.Extensions.Logging;
using MetricsManager.DAL.Interfaces;
using MetricsManager.DAL.Models;

namespace MetricsManager.DAL.Repositories
{
    public class CpuMetricsRepository : ICpuMetricsRepository
    {
        private readonly ILogger<CpuMetricsRepository> _logger;
        private readonly ISqlConnectionProvider _provider;

        public CpuMetricsRepository(ILogger<CpuMetricsRepository> logger, ISqlConnectionProvider provider)
        {
            _logger = logger;
            _provider = provider;
        }

        public void Create(CpuMetric item)
        {
            var ConnectionString = _provider.GetConnectionString();
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("INSERT INTO cpumetrics(AgentId, value, time) VALUES(@agent_id,@value,@time)",
                new
                {
                    agent_id = item.AgentId,
                    value = item.Value,
                    time = item.Time
                });
                var getAll = connection.Query<CpuMetric>("SELECT * FROM cpumetrics", null).ToList();
            }
        }

        public DateTimeOffset GetLastTimeFromAgent(int agent_id)
        {
            var ConnectionString = _provider.GetConnectionString();
            using (var connection = new SQLiteConnection(ConnectionString))
            {//connection.QueryFirstOrDefault<DateTimeOffset>("SELECT time FROM cpumetrics WHERE AgentId = @agentId ORDER BY id DESC"
                var timeFromAgent = connection.QueryFirstOrDefault<DateTimeOffset>("SELECT time FROM cpumetrics WHERE AgentId = @agent_id ORDER BY id DESC",
                new
                {
                    agent_id = agent_id
                });

                DateTimeOffset lastTime = DateTimeOffset.UtcNow;
                if (timeFromAgent.Year == 1)
                {
                    lastTime = DateTimeOffset.UnixEpoch;
                }
                else
                {
                    lastTime = timeFromAgent;
                }
                return lastTime;
            }
        }

        public IList<CpuMetric> GetMetricsFromAgent(int agentId, DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            var ConnectionString = _provider.GetConnectionString();
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<CpuMetric>("SELECT Id, AgentId, Value, Time FROM cpumetrics WHERE (AgentId=@agentId) and ((time>=@fromTime) and (time<=@toTime))",
                    new
                    {
                        fromTime = fromTime,
                        toTime = toTime,
                        agentId = agentId
                    }).ToList();
            }
        }

        public IList<CpuMetric> GetMetricsFromAllCluster(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            var ConnectionString = _provider.GetConnectionString();
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<CpuMetric>("SELECT * FROM cpumetrics WHERE (time>=@fromTime) AND (time<=@toTime)",
                    new
                    {
                        fromTime = fromTime,
                        toTime = toTime,
                    }).ToList();
            }
        }
    }
}
