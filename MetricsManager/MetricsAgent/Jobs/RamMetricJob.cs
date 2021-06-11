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
    public class RamMetricJob : IJob
    {
        private readonly IRamMetricsRepository _repository;
        private readonly IServiceProvider _provider;
        private PerformanceCounter _ramCounter;

        public RamMetricJob(IRamMetricsRepository repository, IServiceProvider provider)
        {
            _repository = repository;
            _provider = provider;
            _ramCounter = new PerformanceCounter("Memory", "Available MBytes");
        }

        public Task Execute(IJobExecutionContext context)
        {
            var ramUsageInPercents = Convert.ToInt32(_ramCounter.NextValue());
            var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            _repository.Create(new RamMetric { Time = time, Value = ramUsageInPercents });
            return Task.CompletedTask;
        }
    }
}
