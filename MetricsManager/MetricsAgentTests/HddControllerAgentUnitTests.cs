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
    public class HddControllerAgentUnitTests
    {
        private HddMetricsAgentController _controller;
        private Mock<IHddMetricsRepository> _mock;
        private Mock<ILogger<HddMetricsAgentController>> _logger;
        private IMapper _mapper;

        public HddControllerAgentUnitTests()
        {
            _mock = new Mock<IHddMetricsRepository>();
            _logger = new Mock<ILogger<HddMetricsAgentController>>();
            _controller = new HddMetricsAgentController(_logger.Object, _mock.Object, _mapper);
        }

        [Fact]
        public void GetByTimePiriod_TestMethod()
        {
            //Arrange
            var valueList = new List<HddMetric>();
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
