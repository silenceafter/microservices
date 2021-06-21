using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MetricsManager.Controllers;
using MetricsManager.Controllers.Requests;
using MetricsManager.Responses;

namespace MetricsManager.Client
{
    public interface IMetricsAgentClient
    {
        AllCpuMetricsResponse GetCpuMetrics(GetAllCpuMetricsApiRequest request);
        AllDotNetMetricsResponse GetDotNetMetrics(GetAllDotNetMetricsApiRequest request);
        AllHddMetricsResponse GetHddMetrics(GetAllHddMetricsApiRequest request);
        AllNetworkMetricsResponse GetNetworkMetrics(GetAllNetworkMetricsApiRequest request);
        AllRamMetricsResponse GetRamMetrics(GetAllRamMetricsApiRequest request);
    }
}
