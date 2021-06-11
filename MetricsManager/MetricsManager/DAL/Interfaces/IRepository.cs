using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace MetricsManager.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IList<T> GetMetricsFromAgent(int agentId, DateTimeOffset fromTime, DateTimeOffset toTime);
        IList<T> GetMetricsFromAllCluster(DateTimeOffset fromTime, DateTimeOffset toTime);
        DateTimeOffset GetLastTimeFromAgent(int agent_id);
        void Create(T item);
    }
}
