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
            try
            {
                HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;
                using var responseStream = response.Content.ReadAsStreamAsync().Result;
                return JsonSerializer.DeserializeAsync<AllCpuMetricsResponse>(responseStream,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }).Result;
            }
            catch (Exception myex)
            {
                _logger.LogError(myex.Message);
            }
            return null;
        }

        public AllDotNetMetricsResponse GetDotNetMetrics(GetAllDotNetMetricsApiRequest request)
        {
            _logger.LogInformation("AllDotNetMetricsResponse GetDotNetMetrics starts");
            var fromParameter = request.fromTime.LocalDateTime.ToString("O");
            var toParameter = request.toTime.LocalDateTime.ToString("O");

            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{request.ClientBaseAddress}/api/metrics/dotnet/byPeriod/from/{fromParameter}/to/{toParameter}");
            try
            {
                HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;
                using var responseStream = response.Content.ReadAsStreamAsync().Result;

                return JsonSerializer.DeserializeAsync<AllDotNetMetricsResponse>(responseStream,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }).Result;
            }
            catch (Exception myex)
            {
                _logger.LogError(myex.Message);
            }
            return null;
        }

        public AllHddMetricsResponse GetHddMetrics(GetAllHddMetricsApiRequest request)
        {
            _logger.LogInformation("AllHddMetricsResponse GetHddMetrics starts");
            var fromParameter = request.fromTime.LocalDateTime.ToString("O");
            var toParameter = request.toTime.LocalDateTime.ToString("O");

            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{request.ClientBaseAddress}/api/metrics/hdd/byPeriod/from/{fromParameter}/to/{toParameter}");
            try
            {
                HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;
                using var responseStream = response.Content.ReadAsStreamAsync().Result;

                return JsonSerializer.DeserializeAsync<AllHddMetricsResponse>(responseStream,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }).Result;
            }
            catch (Exception myex)
            {
                _logger.LogError(myex.Message);
            }
            return null;
        }

        public AllNetworkMetricsResponse GetNetworkMetrics(GetAllNetworkMetricsApiRequest request)
        {
            _logger.LogInformation("AllNetworkMetricsResponse GetNetworkMetrics starts");
            var fromParameter = request.fromTime.LocalDateTime.ToString("O");
            var toParameter = request.toTime.LocalDateTime.ToString("O");

            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{request.ClientBaseAddress}/api/metrics/network/byPeriod/from/{fromParameter}/to/{toParameter}");
            try
            {
                HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;
                using var responseStream = response.Content.ReadAsStreamAsync().Result;

                return JsonSerializer.DeserializeAsync<AllNetworkMetricsResponse>(responseStream,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }).Result;
            }
            catch (Exception myex)
            {
                _logger.LogError(myex.Message);
            }
            return null;
        }

        public AllRamMetricsResponse GetRamMetrics(GetAllRamMetricsApiRequest request)
        {
            _logger.LogInformation("AllRamMetricsResponse GetRamMetrics starts");
            var fromParameter = request.fromTime.LocalDateTime.ToString("O");
            var toParameter = request.toTime.LocalDateTime.ToString("O");

            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{request.ClientBaseAddress}/api/metrics/ram/byPeriod/from/{fromParameter}/to/{toParameter}");
            try
            {
                HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;
                using var responseStream = response.Content.ReadAsStreamAsync().Result;

                return JsonSerializer.DeserializeAsync<AllRamMetricsResponse>(responseStream,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }).Result;
            }
            catch (Exception myex)
            {
                _logger.LogError(myex.Message);
            }
            return null;
        }
    }
}
