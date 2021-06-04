using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Controllers.Requests
{
    public class GetAllRamMetricsApiRequest
    {
        public DateTimeOffset fromTime { get; set; }
        public DateTimeOffset toTime { get; set; }
        public string ClientBaseAddress { get; set; }
    }
}
