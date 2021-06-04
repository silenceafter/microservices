using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using MetricsManager.Controllers;
using MetricsManager.Controllers.Requests;
using MetricsManager.Responses;
using Microsoft.Extensions.Logging;

namespace MetricsManager.Client
{
    public class MetricsAgentClient : IMetricsAgentClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<MetricsAgentClient> _logger;

        public MetricsAgentClient(HttpClient httpClient, ILogger<MetricsAgentClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public AllCpuMetricsResponse GetCpuMetrics(GetAllCpuMetricsApiRequest request)
        {
            _logger.LogInformation("AllCpuMetricsResponse GetCpuMetrics starts");
            var fromParameter = request.fromTime.LocalDateTime.ToString("O");
            var toParameter = request.toTime.LocalDateTime.ToString("O");

            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{request.ClientBaseAddress}/api/metrics/cpu/byPeriod/from/{fromParameter}/to/{toParameter}");
            HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;
            using var responseStream = response.Content.ReadAsStreamAsync().Result;

            var gg = JsonSerializer.DeserializeAsync<AllCpuMetricsResponse>(responseStream,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }).Result;
            return gg;
        }

        public AllDotNetMetricsResponse GetDotNetMetrics(GetAllDotNetMetricsApiRequest request)
        {
            throw new NotImplementedException();
        }

        public AllHddMetricsResponse GetHddMetrics(GetAllHddMetricsApiRequest request)
        {
            throw new NotImplementedException();
        }

        public AllNetworkMetricsResponse GetNetworkMetrics(GetAllNetworkMetricsApiRequest request)
        {
            throw new NotImplementedException();
        }

        public AllRamMetricsResponse GetRamMetrics(GetAllRamMetricsApiRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
