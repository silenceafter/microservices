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
    public class DotNetMetricsRepository : IDotNetMetricsRepository
    {
        private readonly ILogger<DotNetMetricsRepository> _logger;
        private readonly ISqlConnectionProvider _provider;

        public DotNetMetricsRepository(ILogger<DotNetMetricsRepository> logger, ISqlConnectionProvider provider)
        {
            _logger = logger;
            _provider = provider;
        }

        public void Create(DotNetMetric item)
        {
            try
            {
                var ConnectionString = _provider.GetConnectionString();
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Execute("INSERT INTO dotnetmetrics(AgentId, value, time) VALUES(@agent_id,@value,@time)",
                    new
                    {
                        agent_id = item.AgentId,
                        value = item.Value,
                        time = item.Time
                    });
                }
            }
            catch (Exception myex)
            {
                _logger.LogError(myex.Message);
            }
            return;
        }

        public DateTimeOffset GetLastTimeFromAgent(int agent_id)
        {
            try
            {
                var ConnectionString = _provider.GetConnectionString();
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    var timeFromAgent = connection.QueryFirstOrDefault<DateTimeOffset>("SELECT time FROM dotnetmetrics WHERE AgentId = @agent_id ORDER BY id DESC",
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
            catch (Exception myEx)
            {
                _logger.LogError(myEx.Message);
            }
            return DateTimeOffset.UtcNow;
        }

        public IList<DotNetMetric> GetMetricsFromAgent(int agentId, DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            try
            {
                var ConnectionString = _provider.GetConnectionString();
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    return connection.Query<DotNetMetric>("SELECT * FROM dotnetmetrics WHERE AgentId = @agentId AND time >= @fromTime AND time <= @toTime",
                        new
                        {
                            fromTime = fromTime.ToUnixTimeSeconds(),
                            toTime = toTime.ToUnixTimeSeconds(),
                            agentId = agentId
                        }).ToList();
                }
            }
            catch (Exception myex)
            {
                _logger.LogError(myex.Message);
            }
            return null;
        }

        public IList<DotNetMetric> GetMetricsFromAllCluster(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            try
            {
                var ConnectionString = _provider.GetConnectionString();
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    return connection.Query<DotNetMetric>("SELECT * FROM dotnetmetrics WHERE (time>=@fromTime) AND (time<=@toTime)",
                        new
                        {
                            fromTime = fromTime.ToUnixTimeSeconds(),
                            toTime = toTime.ToUnixTimeSeconds(),
                        }).ToList();
                }
            }
            catch (Exception myex)
            {
                _logger.LogError(myex.Message);
            }
            return null;
        }
    }
}
