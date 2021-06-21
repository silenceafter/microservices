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
    public class HddMetricJob : IJob
    { 
        private IHddMetricsRepository _repository;
        private readonly IAgentsRepository _agentsRepository;
        private readonly IMetricsAgentClient _metricsAgentClient;
        private readonly ILogger<HddMetricJob> _logger;

        public HddMetricJob(IHddMetricsRepository repository, IAgentsRepository agentsRepository, IMetricsAgentClient metricsAgentClient, ILogger<HddMetricJob> logger)
        {
            _repository = repository;
            _agentsRepository = agentsRepository;
            _metricsAgentClient = metricsAgentClient;
            _logger = logger;
        }
    
        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("HddMetricJob started");
            var allAgentsList = _agentsRepository.GetAgents();

            foreach (var agent in allAgentsList)
            {
                var fromTime = _repository.GetLastTimeFromAgent(agent.AgentId);
                var toTime = DateTimeOffset.UtcNow;

                try
                {
                    _logger.LogInformation($"HddMetricJob try GetHddMetrics() from {fromTime} to {toTime}, agentAddress {agent.AgentAddress}");
                    var outerMetrics = _metricsAgentClient.GetHddMetrics(new GetAllHddMetricsApiRequest
                    {
                        ClientBaseAddress = agent.AgentAddress,
                        fromTime = fromTime,
                        toTime = toTime
                    });

                    if (outerMetrics != null)
                    {
                        foreach (var oneMetric in outerMetrics.Metrics)
                        {
                            _logger.LogInformation($"HddMetricJob write hdd metric to DB from agentId {agent.AgentId}, time: {oneMetric.Time}, value: {oneMetric.Value}");
                            _repository.Create(new HddMetric
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
