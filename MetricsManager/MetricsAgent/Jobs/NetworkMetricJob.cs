using MetricsAgent.DAL;
using MetricsAgent.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.Jobs
{
    [DisallowConcurrentExecution]
    public class NetworkMetricJob : IJob
    {
        private readonly INetworkMetricsRepository _repository;
        private readonly IServiceProvider _provider;
        private PerformanceCounter _networkCounter;

        public NetworkMetricJob(INetworkMetricsRepository repository, IServiceProvider provider)
        {
            _repository = repository;
            _provider = provider;

            PerformanceCounterCategory netWorkCategory = new PerformanceCounterCategory("Network Interface");
            string[] networkInstNames = netWorkCategory.GetInstanceNames();
            _networkCounter = new PerformanceCounter("Network Interface", "Bytes Received/sec", networkInstNames[0]);
        }
        public Task Execute(IJobExecutionContext context)
        {
            var networkUsageInPercents = Convert.ToInt32(_networkCounter.NextValue());
            var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            _repository.Create(new NetworkMetric { Time = time, Value = networkUsageInPercents });
            return Task.CompletedTask;
        }
    }
}