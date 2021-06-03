using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MetricsManager.Responses;

namespace MetricsManager.DAL.Interfaces
{
    public interface IAgentsRepository
    {
        public void RegisterAgent(AgentInfo agent);
        public void RemoveAgent(AgentInfo agent);
        public IList<AgentInfo> GetAgents();
        public string GetAddressForAgentId(int id);
    }
}
