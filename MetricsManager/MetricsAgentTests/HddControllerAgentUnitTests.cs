using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace MetricsAgentTests
{
    public class HddControllerAgentUnitTests
    {
        private HddMetricsAgentController controller;
        public HddControllerAgentUnitTests()
        {
            controller = new HddMetricsAgentController();
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
