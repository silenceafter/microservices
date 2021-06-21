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
    public class NetworkControllerAgentUnitTests
    {
        private NetworkMetricsAgentController _controller;
        private Mock<INetworkMetricsRepository> _mock;
        private Mock<ILogger<NetworkMetricsAgentController>> _logger;
        private readonly IMapper _mapper;

        public NetworkControllerAgentUnitTests()
        {
            _mock = new Mock<INetworkMetricsRepository>();
            _logger = new Mock<ILogger<NetworkMetricsAgentController>>();
            _controller = new NetworkMetricsAgentController(_logger.Object, _mock.Object, _mapper);
        }

        [Fact]
        public void GetByTimePeriod_ReturnsOk()
        {
            //Arrange
            DateTimeOffset fromTime = DateTimeOffset.FromUnixTimeSeconds(10);
            DateTimeOffset toTime = DateTimeOffset.FromUnixTimeSeconds(20);
            _mock.Setup(a => a.GetByTimePeriod(DateTimeOffset.FromUnixTimeSeconds(10), DateTimeOffset.FromUnixTimeSeconds(20))).Returns(new List<NetworkMetric>()).Verifiable();

            //Act
            var result = _controller.GetByTimePeriod(fromTime, toTime);

            //Assert
            _mock.Verify(r => r.GetByTimePeriod(DateTimeOffset.FromUnixTimeSeconds(10), DateTimeOffset.FromUnixTimeSeconds(20)), Times.AtMostOnce());
            _logger.Verify();
        }
    }
}
