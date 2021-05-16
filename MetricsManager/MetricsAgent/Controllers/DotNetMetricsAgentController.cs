using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MetricsAgent.DAL;
using MetricsAgent.Models;
using MetricsAgent.Responses;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/dotnet")]
    [ApiController]
    public class DotNetMetricsAgentController : ControllerBase
    {
        private readonly ILogger<DotNetMetricsAgentController> _logger;
        private readonly IDotNetMetricsRepository _repository;
        public DotNetMetricsAgentController(ILogger<DotNetMetricsAgentController> logger, IDotNetMetricsRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet("byPeriod/from/{fromTime}/to/{toTime}")]
        public IActionResult GetByTimePeriod([FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"Parameters passed to the method: fromTime = {fromTime}, toTime = {toTime}");
            var result = _repository.GetByTimePeriod(fromTime, toTime);
            var metrics = new AllDotNetMetricsResponse();
            metrics.Metrics = new List<DotNetMetricDto>();

            for (int i = 0; i < result.Count; i++)
            {
                metrics.Metrics.Add(new DotNetMetricDto()
                {
                    Id = result[i].Id,
                    Value = result[i].Value,
                    Time = DateTimeOffset.FromUnixTimeSeconds(result[i].Time)
                });
                _logger.LogInformation($"Results from repository method GetByTimePeriod(): Id = {metrics.Metrics[i].Id}, Value = {metrics.Metrics[i].Value}, Time = {metrics.Metrics[i].Time}");
            }
            return Ok(metrics);
        }

        [HttpGet("errorsCount/{errorsCount}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] int errorsCount, [FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"errorsCount = {errorsCount}, fromTime = {fromTime}, toTime = {toTime}");
            return Ok();
        }
    }
}
