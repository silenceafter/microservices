using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsManagerClient.Controller.Responses
{
    public class AllRamMetricsResponse
    {
        public List<RamMetricDto> Metrics { get; set; }
    }

    public class RamMetricDto
    {
        public int Id { get; set; }
        public int AgentId { get; set; }
        public int Value { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}
