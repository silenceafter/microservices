using MetricsAgent.DAL;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MetricsAgent.Models;

namespace MetricsAgent.Jobs
{
    [DisallowConcurrentExecution]
    public class CpuMetricJob : IJob
    {
        private readonly ICpuMetricsRepository _repository;
        private readonly IServiceProvider _provider; 
        private PerformanceCounter _cpuCounter;

        public CpuMetricJob(ICpuMetricsRepository repository, IServiceProvider provider)
        {
            _repository = repository;
            _provider = provider;
            _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        }

        public Task Execute(IJobExecutionContext context)
        {
            var cpuUsageInPercents = Convert.ToInt32(_cpuCounter.NextValue());
            var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            _repository.Create(new CpuMetric { Time = time, Value = cpuUsageInPercents });
            return Task.CompletedTask;
        }
    }
}
