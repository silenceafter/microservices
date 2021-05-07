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
        public void GetAllMetrics_Returns_Ok()
        {
            //Act
            var result = controller.GetAllMetrics();

            //Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
