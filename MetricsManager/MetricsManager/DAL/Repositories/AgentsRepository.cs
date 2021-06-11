using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using System.Data.SQLite;
using MetricsManager.DAL.Interfaces;

namespace MetricsManager.DAL.Repositories
{
    public class AgentsRepository : IAgentsRepository
    {
        private readonly ILogger<AgentsRepository> _logger;
        private readonly ISqlConnectionProvider _provider;

        public AgentsRepository(ILogger<AgentsRepository> logger, ISqlConnectionProvider provider)
        {
            _logger = logger;
            _provider = provider;
        }

        public string GetAddressForAgentId(int id)
        {
            try
            {
                var ConnectionString = _provider.GetConnectionString();
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    var response = connection.QuerySingle<string>("SELECT AgentAddress FROM agents WHERE AgentId=@agent_id",
                            new
                            {
                                agent_id = id
                            }).ToList();

                    return response.ToString();
                }
            }
            catch (Exception myex)
            {
                _logger.LogError(myex.Message);
            }
            return null;
        }

        public IList<AgentInfo> GetAgents()
        {
            try
            {
                var ConnectionString = _provider.GetConnectionString();
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    return connection.Query<AgentInfo>("SELECT AgentId, AgentAddress FROM agents", null).ToList();
                }
            }
            catch (Exception myex)
            {
                _logger.LogError(myex.Message);
            }
            return null;
        }

        public void RegisterAgent(AgentInfo agent)
        {
            try
            {
                var ConnectionString = _provider.GetConnectionString();
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Execute("INSERT INTO agents(AgentId,AgentAddress) VALUES(@AgentId,@AgentAddress)",
                            new
                            {
                                AgentId = agent.AgentId,
                                AgentAddress = agent.AgentAddress.ToString()
                            });
                }
            }
            catch (Exception myex)
            {
                _logger.LogError(myex.Message);
            }
            return;
        }

        public void RemoveAgent(AgentInfo agent)
        {
            try
            {
                var ConnectionString = _provider.GetConnectionString();
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Execute($"DELETE FROM agents WHERE (AgentAddress=@AgentAddress)",
                            new
                            {
                                AgentAddress = agent.AgentAddress
                            });
                }
            }
            catch (Exception myex)
            {
                _logger.LogError(myex.Message);
            }
            return;
        }

        public void RemoveAllAgent()
        {
            try
            {
                var ConnectionString = _provider.GetConnectionString();
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Execute($"DELETE FROM agents", null);
                }
            }
            catch (Exception myex)
            {
                _logger.LogError(myex.Message);
            }
            return;
        }
    }
}
