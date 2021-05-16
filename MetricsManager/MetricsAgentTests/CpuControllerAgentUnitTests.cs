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
    public class CpuControllerAgentUnitTests
    {        
        private CpuMetricsAgentController controller;
        private Mock<ICpuMetricsRepository> mock;
        private Mock<ILogger<CpuMetricsAgentController>> logger;

        public CpuControllerAgentUnitTests()
        {
            mock = new Mock<ICpuMetricsRepository>();
            logger = new Mock<ILogger<CpuMetricsAgentController>>();
            controller = new CpuMetricsAgentController(logger.Object, mock.Object);
        }

        [Fact]
        public void GetByTimePiriod_TestMethod()
        {
            //Arrange
            var valueList = new List<CpuMetric>();
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
