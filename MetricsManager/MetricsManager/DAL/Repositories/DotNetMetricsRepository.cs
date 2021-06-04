using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MetricsManager.DAL.Interfaces;
using MetricsManager.DAL.Models;
using Microsoft.Extensions.Logging;

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
            throw new NotImplementedException();
        }

        public DateTimeOffset GetLastTimeFromAgent(int agent_id)
        {
            throw new NotImplementedException();
        }

        public IList<DotNetMetric> GetMetricsFromAgent(int agentId, DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            throw new NotImplementedException();
        }

        public IList<DotNetMetric> GetMetricsFromAllCluster(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            throw new NotImplementedException();
        }
    }
}
