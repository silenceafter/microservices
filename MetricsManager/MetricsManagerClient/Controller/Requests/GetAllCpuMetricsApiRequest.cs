using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsManagerClient.Controller.Requests
{
    public class GetAllCpuMetricsApiRequest
    {
        public DateTimeOffset fromTime { get; set; }
        public DateTimeOffset toTime { get; set; }
        public string ClientBaseAddress { get; set; }
    }
}
