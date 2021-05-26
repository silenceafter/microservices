using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
using MetricsAgent.DAL;
using MetricsAgent.Models;
using System;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using AutoMapper;

namespace MetricsAgentTests
{
    public class NetworkControllerAgentUnitTests
    {
        private NetworkMetricsAgentController _controller;
        private Mock<INetworkMetricsRepository> _mock;
        private Mock<ILogger<NetworkMetricsAgentController>> _logger;
        private IMapper _mapper;

        public NetworkControllerAgentUnitTests()
        {
            _mock = new Mock<INetworkMetricsRepository>();
            _logger = new Mock<ILogger<NetworkMetricsAgentController>>();
            _controller = new NetworkMetricsAgentController(_logger.Object, _mock.Object, _mapper);
        }

        [Fact]
        public void GetByTimePiriod_TestMethod()
        {
            //Arrange
            var valueList = new List<NetworkMetric>();
            _mock.Setup(repository => repository.GetByTimePeriod(
                It.IsAny<DateTimeOffset>(),
                It.IsAny<DateTimeOffset>()))
                .Returns(valueList);

            //Act
            IActionResult result = _controller.GetByTimePeriod(
                DateTimeOffset.FromUnixTimeSeconds(100),
                DateTimeOffset.FromUnixTimeSeconds(200));

            //Assert
            _mock.Verify(repository =>   
                repository
                    .GetByTimePeriod(
                    DateTimeOffset.FromUnixTimeSeconds(100), 
                    DateTimeOffset.FromUnixTimeSeconds(200)), 
                    Times.AtLeastOnce());
        }
    }
}
