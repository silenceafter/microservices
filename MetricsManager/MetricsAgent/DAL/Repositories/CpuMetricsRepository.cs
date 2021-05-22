using Dapper;
using MetricsAgent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace MetricsAgent.DAL.Repositories
{
    public class CpuMetricsRepository : ICpuMetricsRepository
    {
        private readonly ISqlConnectionProvider _provider;                
        //инжектируем соединение с бд в наш репозиторий через конструктор
        public CpuMetricsRepository(ISqlConnectionProvider provider)
        {
            _provider = provider;
        }

        public void Create(CpuMetric item)
        {
            var ConnectionString = _provider.GetConnectionString();
            using var connection = new SQLiteConnection(ConnectionString);
            connection.Open();
            //создаем команду
            using var cmd = new SQLiteCommand(connection);
            //прописываем в команду sql запрос на вставку данных
            cmd.CommandText = "INSERT INTO cpumetrics(value, time) VALUES(@value,@time)";
            //добавляем параметры в запрос из нашего объекта
            cmd.Parameters.AddWithValue("@value", item.Value);
            //в таблице будем хранить время в секундах, потому преобразуем перед записью в секунды
            //через свойство
            cmd.Parameters.AddWithValue("@time", item.Time);
            //подготовка команды к выполнению
            cmd.Prepare();
            //выполнение команды
            cmd.ExecuteNonQuery();
        }

        public List<CpuMetric> GetByTimePeriod(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            var ConnectionString = _provider.GetConnectionString();
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<CpuMetric>("SELECT * FROM cpumetrics WHERE time >= @fromTime AND time <= @toTime",
                    new
                    {
                        fromTime = fromTime.ToUnixTimeSeconds(),
                        toTime = toTime.ToUnixTimeSeconds()
                    }).ToList();
            }         
        }

        /*public void GetByTimePeriod()
        {
            throw new NotImplementedException();
        }*/
    }
}
