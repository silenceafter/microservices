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
    public class RamMetricsRepository : IRamMetricsRepository
    {
        private readonly ILogger<RamMetricsRepository> _logger;
        private readonly ISqlConnectionProvider _provider;
        public RamMetricsRepository(ILogger<RamMetricsRepository> logger, ISqlConnectionProvider provider)
        {
            _logger = logger;
            _provider = provider;
        }

        public void Create(RamMetric item)
        {
            throw new NotImplementedException();
        }

        public DateTimeOffset GetLastTimeFromAgent(int agent_id)
        {
            throw new NotImplementedException();
        }

        public IList<RamMetric> GetMetricsFromAgent(int agentId, DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            throw new NotImplementedException();
        }

        public IList<RamMetric> GetMetricsFromAllCluster(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            throw new NotImplementedException();
        }
    }
}
