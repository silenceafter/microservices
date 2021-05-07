using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace MetricsAgentTests
{
    public class NetworkControllerAgentUnitTests
    {
        private NetworkMetricsAgentController controller;
        public NetworkControllerAgentUnitTests()
        {
            controller = new NetworkMetricsAgentController();
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
