using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace MetricsAgentTests
{
    public class DotNetControllerAgentUnitTests
    {
        private DotNetMetricsAgentController controller;
        public DotNetControllerAgentUnitTests()
        {
            controller = new DotNetMetricsAgentController();
        }

        [Fact]
        public void GetMetricsFromAgent_Returns_Ok()
        {
            int errorsCount = 10;
            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(100);

            //Act
            var result = controller.GetMetricsFromAgent(errorsCount, fromTime, toTime);

            //Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
