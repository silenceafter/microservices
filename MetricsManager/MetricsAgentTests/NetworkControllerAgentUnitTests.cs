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
        public void GetMetricsFromAgent_Returns_Ok()
        {
            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(100);

            //Act
            var result = controller.GetMetricsFromAgent(fromTime, toTime);

            //Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
