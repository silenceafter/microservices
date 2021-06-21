using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsManagerClient.DAL.Models
{
    public class NetworkMetric
    {
        public int Id { get; set; }
        public int AgentId { get; set; }
        public int Value { get; set; }
        public long Time { get; set; }
    }
}
