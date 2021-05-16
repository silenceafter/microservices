using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
using MetricsAgent.DAL;
using MetricsAgent.Models;
using System;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace MetricsAgentTests
{
    public class HddControllerAgentUnitTests
    {
        private HddMetricsAgentController controller;
        private Mock<IHddMetricsRepository> mock;
        private Mock<ILogger<HddMetricsAgentController>> logger;

        public HddControllerAgentUnitTests()
        {
            mock = new Mock<IHddMetricsRepository>();
            logger = new Mock<ILogger<HddMetricsAgentController>>();
            controller = new HddMetricsAgentController(logger.Object, mock.Object);
        }

        [Fact]
        public void GetByTimePiriod_TestMethod()
        {
            //Arrange
            var valueList = new List<HddMetric>();
            mock.Setup(repository => repository.GetByTimePeriod(
                It.IsAny<DateTimeOffset>(),
                It.IsAny<DateTimeOffset>()))
                .Returns(valueList);

            //Act
            IActionResult result = controller.GetByTimePeriod(
                DateTimeOffset.FromUnixTimeSeconds(100),
                DateTimeOffset.FromUnixTimeSeconds(200));

            //Assert
            mock.Verify(repository => repository.GetByTimePeriod(DateTimeOffset.FromUnixTimeSeconds(100), DateTimeOffset.FromUnixTimeSeconds(200)), Times.AtLeastOnce());
        }
    }
}
