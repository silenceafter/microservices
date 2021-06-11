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
    public class HddMetricJob : IJob
    {
        private readonly IHddMetricsRepository _repository;
        private readonly IServiceProvider _provider;
        private PerformanceCounter _hddCounter;

        public HddMetricJob(IHddMetricsRepository repository, IServiceProvider provider)
        {
            _repository = repository;
            _provider = provider;
            _hddCounter = new PerformanceCounter("PhysicalDisk", "% Disk Time", "0 D: C:");
        }

        public Task Execute(IJobExecutionContext context)
        {
            var hddUsageInPercents = Convert.ToInt32(_hddCounter.NextValue());
            var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            _repository.Create(new HddMetric { Time = time, Value = hddUsageInPercents });
            return Task.CompletedTask;
        }
    }
}
