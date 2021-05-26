using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.DAL
{
    public class SqlConnection : ISqlConnectionProvider
    {
        private const string _connectionString = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100;";
        public string GetConnectionString()
        {
            return _connectionString;
        }
    }
}
