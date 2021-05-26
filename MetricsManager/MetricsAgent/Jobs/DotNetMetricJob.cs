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
    public class DotNetMetricJob : IJob
    {
        private readonly IDotNetMetricsRepository _repository;
        private readonly IServiceProvider _provider;
        private PerformanceCounter _dotnetCounter;

        public DotNetMetricJob(IDotNetMetricsRepository repository, IServiceProvider provider)
        {
            _repository = repository;
            _provider = provider;
            _dotnetCounter = new PerformanceCounter(".NET CLR Memory", "# Bytes in all heaps", "_Global_");
        }

        public Task Execute(IJobExecutionContext context)
        {
            var dotnetUsageInPercents = Convert.ToInt32(_dotnetCounter.NextValue());
            var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            _repository.Create(new DotNetMetric { Time = time, Value = dotnetUsageInPercents });
            return Task.CompletedTask;
        }
    }
}