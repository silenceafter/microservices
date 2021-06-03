using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MetricsManager.DAL.Interfaces;
using Microsoft.Extensions.Logging;
using Quartz;
using MetricsManager.Client;
using MetricsManager.DAL.Models;
using MetricsManager.DAL;
using System.Data.SQLite;
using Dapper;

namespace MetricsManager.Jobs
{
    [DisallowConcurrentExecution]
    public class CpuMetricJob : IJob
    {
        private ICpuMetricsRepository _repository;
        private readonly IAgentsRepository _agentsRepository;
        private readonly IMetricsAgentClient _metricsAgentClient;
        private readonly ILogger<CpuMetricJob> _logger;
        private readonly ISqlConnectionProvider _provider;//upd

        public CpuMetricJob(ICpuMetricsRepository repository, IAgentsRepository agentsRepository, IMetricsAgentClient metricsAgentClient, ILogger<CpuMetricJob> logger, ISqlConnectionProvider provider)
        {
            _repository = repository;
            _agentsRepository = agentsRepository;
            _metricsAgentClient = metricsAgentClient;
            _logger = logger;

            _provider = provider;
            var ConnectionString = _provider.GetConnectionString();
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("DELETE FROM agents",
                        null);
            }
        }

        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("CpuMetricJob started");
            var allAgentsList = _agentsRepository.GetAgents();

            foreach (var agent in allAgentsList)
            {
                var fromTime = _repository.GetLastTimeFromAgent(agent.AgentId);
                var toTime = DateTimeOffset.UtcNow;                
                _logger.LogInformation($"CpuMetricJob try GetCpuMetrics() from {fromTime} to {toTime}, agentAddr {agent.AgentAddress}");
                var outerMetrics = _metricsAgentClient.GetCpuMetrics(new GetAllCpuMetricsApiRequest
                {
                    ClientBaseAddress = agent.AgentAddress,
                    fromTime = fromTime,
                    toTime = toTime
                });

                if (outerMetrics != null)
                {
                    foreach (var oneMetric in outerMetrics.Metrics)
                    {
                        _logger.LogInformation($"CpuMetricJob write cpu metric to DB from agentId {agent.AgentId}, time: {oneMetric.Time}, value: {oneMetric.Value}");
                        _repository.Create(new CpuMetric
                        {
                            AgentId = agent.AgentId,
                            Time = oneMetric.Time.ToUnixTimeSeconds(),
                            Value = oneMetric.Value
                        });
                    }
                }
            }
            return Task.CompletedTask;            
        }
    }
}
