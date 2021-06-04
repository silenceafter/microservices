using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MetricsManager.DAL.Interfaces;
using MetricsManager.DAL.Models;
using Microsoft.Extensions.Logging;

namespace MetricsManager.DAL.Repositories
{
    public class HddMetricsRepository : IHddMetricsRepository
    {
        private readonly ILogger<HddMetricsRepository> _logger;
        private readonly ISqlConnectionProvider _provider;
        public HddMetricsRepository(ILogger<HddMetricsRepository> logger, ISqlConnectionProvider provider)
        {
            _logger = logger;
            _provider = provider;
        }

        public void Create(HddMetric item)
        {
            throw new NotImplementedException();
        }

        public DateTimeOffset GetLastTimeFromAgent(int agent_id)
        {
            throw new NotImplementedException();
        }

        public IList<HddMetric> GetMetricsFromAgent(int agentId, DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            throw new NotImplementedException();
        }

        public IList<HddMetric> GetMetricsFromAllCluster(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            throw new NotImplementedException();
        }
    }
}
