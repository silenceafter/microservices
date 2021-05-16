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
    [Route("api/metrics/hdd")]
    [ApiController]
    public class HddMetricsAgentController : ControllerBase
    {
        private readonly ILogger<HddMetricsAgentController> _logger;
        private readonly IHddMetricsRepository _repository;
        public HddMetricsAgentController(ILogger<HddMetricsAgentController> logger, IHddMetricsRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet("byPeriod/from/{fromTime}/to/{toTime}")]
        public IActionResult GetByTimePeriod([FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"Parameters passed to the method: fromTime = {fromTime}, toTime = {toTime}");
            var result = _repository.GetByTimePeriod(fromTime, toTime);
            var metrics = new AllHddMetricsResponse();
            metrics.Metrics = new List<HddMetricDto>();

            for (int i = 0; i < result.Count; i++)
            {
                metrics.Metrics.Add(new HddMetricDto()
                {
                    Id = result[i].Id,
                    Value = result[i].Value,
                    Time = DateTimeOffset.FromUnixTimeSeconds(result[i].Time)
                });
                _logger.LogInformation($"Results from repository method GetByTimePeriod(): Id = {metrics.Metrics[i].Id}, Value = {metrics.Metrics[i].Value}, Time = {metrics.Metrics[i].Time}");
            }
            return Ok(metrics);
        }

        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"fromTime = {fromTime}, toTime = {toTime}");
            return Ok();
        }
    }
}
