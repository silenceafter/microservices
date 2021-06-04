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
using MetricsManager.Controllers.Requests;

namespace MetricsManager.Jobs
{
    [DisallowConcurrentExecution]
    public class RamMetricJob : IJob
    {
        private IRamMetricsRepository _repository;
        private readonly IAgentsRepository _agentsRepository;
        private readonly IMetricsAgentClient _metricsAgentClient;
        private readonly ILogger<RamMetricJob> _logger;

        public RamMetricJob(IRamMetricsRepository repository, IAgentsRepository agentsRepository, IMetricsAgentClient metricsAgentClient, ILogger<RamMetricJob> logger)
        {
            _repository = repository;
            _agentsRepository = agentsRepository;
            _metricsAgentClient = metricsAgentClient;
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("RamMetricJob started");
            var allAgentsList = _agentsRepository.GetAgents();

            foreach (var agent in allAgentsList)
            {
                var fromTime = _repository.GetLastTimeFromAgent(agent.AgentId);
                var toTime = DateTimeOffset.UtcNow;

                try
                {
                    _logger.LogInformation($"RamMetricJob try GetRamMetrics() from {fromTime} to {toTime}, agentAddress {agent.AgentAddress}");
                    var outerMetrics = _metricsAgentClient.GetRamMetrics(new GetAllRamMetricsApiRequest
                    {
                        ClientBaseAddress = agent.AgentAddress,
                        fromTime = fromTime,
                        toTime = toTime
                    });

                    if (outerMetrics != null)
                    {
                        foreach (var oneMetric in outerMetrics.Metrics)
                        {
                            _logger.LogInformation($"RamMetricJob write ram metric to DB from agentId {agent.AgentId}, time: {oneMetric.Time}, value: {oneMetric.Value}");
                            _repository.Create(new RamMetric
                            {
                                AgentId = agent.AgentId,
                                Time = oneMetric.Time.ToUnixTimeSeconds(),
                                Value = oneMetric.Value
                            });
                        }
                    }
                }
                catch (Exception myex)
                {
                    _logger.LogError(myex.Message);
                }
            }
            return Task.CompletedTask;
        }
    }
}
