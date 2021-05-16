using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
using MetricsAgent.Models;

namespace MetricsAgent.DAL 
{
    //маркировочный интерфейс
    //необходим, чтобы проверить работу репозитория на тесте-заглушке
    public interface ICpuMetricsRepository : IRepository<CpuMetric>
    {
    }

    public class CpuMetricsRepository : ICpuMetricsRepository
    {
        private const string ConnectionString = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100;";
            //инжектируем соединение с бд в наш репозиторий через конструктор
        public void Create(CpuMetric item)
        {
            using var connection = new SQLiteConnection(ConnectionString);
            connection.Open();
            //создаем конманду
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
            using var connection = new SQLiteConnection(ConnectionString);
            connection.Open();
            using var cmd = new SQLiteCommand(connection);
            cmd.CommandText = "SELECT * FROM cpumetrics WHERE time >= @fromTime AND time <= @toTime";
            cmd.Parameters.AddWithValue("@fromTime", fromTime.ToUnixTimeSeconds());
            cmd.Parameters.AddWithValue("@toTime", toTime.ToUnixTimeSeconds());

            List<CpuMetric> myArray = new List<CpuMetric>();
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    myArray.Add(new CpuMetric
                    {
                        Id = reader.GetInt32(0),
                        Value = reader.GetInt32(1),
                        Time =  reader.GetInt64(2)
                    });
                }
            }
            return myArray;
        }

        public void GetByTimePeriod()
        {
            throw new NotImplementedException();
        }
    }
}
