using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MetricsAgent.Controllers;
using MetricsAgent.DAL;
using MetricsAgent.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace MetricsAgentTests
{
    public class HddControllerAgentUnitTests
    {
        private HddMetricsAgentController _controller;
        private Mock<IHddMetricsRepository> _mock;
        private Mock<ILogger<HddMetricsAgentController>> _logger;
        private readonly IMapper _mapper;

        public HddControllerAgentUnitTests()
        {
            _mock = new Mock<IHddMetricsRepository>();
            _logger = new Mock<ILogger<HddMetricsAgentController>>();
            _controller = new HddMetricsAgentController(_logger.Object, _mock.Object, _mapper);
        }

        [Fact]
        public void GetByTimePeriod_ReturnsOk()
        {
            //Arrange
            DateTimeOffset fromTime = DateTimeOffset.FromUnixTimeSeconds(10);
            DateTimeOffset toTime = DateTimeOffset.FromUnixTimeSeconds(20);
            _mock.Setup(a => a.GetByTimePeriod(DateTimeOffset.FromUnixTimeSeconds(10), DateTimeOffset.FromUnixTimeSeconds(20))).Returns(new List<HddMetric>()).Verifiable();

            //Act
            var result = _controller.GetByTimePeriod(fromTime, toTime);

            //Assert
            _mock.Verify(r => r.GetByTimePeriod(DateTimeOffset.FromUnixTimeSeconds(10), DateTimeOffset.FromUnixTimeSeconds(20)), Times.AtMostOnce());
            _logger.Verify();
        }
    }
}
