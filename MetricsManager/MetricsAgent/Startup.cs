using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
using MetricsAgent.DAL;
using MetricsAgent.DAL.Repositories;
using Dapper;
using AutoMapper;

namespace MetricsAgent
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            ConfigureSqlLiteConnection(services);
            services.AddSingleton<ICpuMetricsRepository, CpuMetricsRepository>();
            services.AddSingleton<IDotNetMetricsRepository, DotNetMetricsRepository>();
            services.AddSingleton<IHddMetricsRepository, HddMetricsRepository>();
            services.AddSingleton<INetworkMetricsRepository, NetworkMetricsRepository>();
            services.AddSingleton<IRamMetricsRepository, RamMetricsRepository>();
            services.AddSingleton<ISqlConnectionProvider, SqlConnection>();
            //
            var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new MapperProfile()));
            var mapper = mapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);
        }

        public void ConfigureSqlLiteConnection(IServiceCollection services)
        {
            //const string connectionString = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100;";
            var connectionString = new SqlConnection().GetConnectionString();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                PrepareSchema(connection);
            }
        }

        private void PrepareSchema(SQLiteConnection connection)
        {
            using (var command = new SQLiteCommand(connection))
            {
                string[] tabnameArray = new string[5] {
                    "cpumetrics",
                    "dotnetmetrics",
                    "hddmetrics",
                    "networkmetrics",
                    "rammetrics"
                };

                for (int i = 0; i < tabnameArray.GetLength(0); i++)
                {
                    command.CommandText = $@"DROP TABLE IF EXISTS {tabnameArray[i]};
                     CREATE TABLE {tabnameArray[i]}(id INTEGER PRIMARY KEY, value INT, time INT)";
                    command.ExecuteNonQuery();
                }
                
                //добавляем значения реквизита value
                int[] myValueArray = new int[5]
                {
                    100,
                    200,
                    300,
                    400,
                    500
                };
                //добавляем значения реквизита time для cpu
                string[] myCpuTimeArray = new string[5] {
                    "2021-05-05T20:00:00+00:00",
                    "2021-05-07T00:00:00+00:00",
                    "2021-05-10T05:30:00+00:00",
                    "2021-05-12T10:30:00+00:00",
                    "2021-05-15T11:00:00+00:00"
                };
               
                for (int i = 0; i < myCpuTimeArray.GetLength(0); i++)
                {
                    var intValue = myValueArray[i];
                    var longValue = DateTimeOffset.Parse(myCpuTimeArray[i]).ToUnixTimeSeconds();
                    //cpu
                    connection.Execute("INSERT INTO cpumetrics (value, time) VALUES (@intValue,@longValue)",
                        new 
                        {
                            intValue = intValue, 
                            longValue = longValue
                        });
                }

                //добавляем значения реквизита time для dotnet
                string[] myDotNetTimeArray = new string[5] {
                    "2021-05-01T20:00:00+00:00",
                    "2021-05-02T00:00:00+00:00",
                    "2021-05-03T05:30:00+00:00",
                    "2021-05-04T10:30:00+00:00",
                    "2021-05-05T11:00:00+00:00"
                };

                for (int i = 0; i < myDotNetTimeArray.GetLength(0); i++)
                {
                    var intValue = myValueArray[i];
                    var longValue = DateTimeOffset.Parse(myDotNetTimeArray[i]).ToUnixTimeSeconds();
                    //cpu
                    connection.Execute("INSERT INTO dotnetmetrics (value, time) VALUES (@intValue,@longValue)",
                        new
                        {
                            intValue = intValue,
                            longValue = longValue
                        });
                }

                //добавляем значения реквизита time для hdd
                string[] myHddTimeArray = new string[5] {
                    "2021-04-01T20:00:00+00:00",
                    "2021-04-02T00:00:00+00:00",
                    "2021-04-03T05:30:00+00:00",
                    "2021-05-04T10:30:00+00:00",
                    "2021-05-05T11:00:00+00:00"
                };

                for (int i = 0; i < myHddTimeArray.GetLength(0); i++)
                {
                    var intValue = myValueArray[i];
                    var longValue = DateTimeOffset.Parse(myHddTimeArray[i]).ToUnixTimeSeconds();
                    //cpu
                    connection.Execute("INSERT INTO hddmetrics (value, time) VALUES (@intValue,@longValue)",
                        new
                        {
                            intValue = intValue,
                            longValue = longValue
                        });
                }

                //добавляем значения реквизита time для network
                string[] myNetworkTimeArray = new string[5] {
                    "2021-03-01T20:00:00+00:00",
                    "2021-03-02T00:00:00+00:00",
                    "2021-03-03T05:30:00+00:00",
                    "2021-03-04T10:30:00+00:00",
                    "2021-03-05T11:00:00+00:00"
                };

                for (int i = 0; i < myNetworkTimeArray.GetLength(0); i++)
                {
                    var intValue = myValueArray[i];
                    var longValue = DateTimeOffset.Parse(myNetworkTimeArray[i]).ToUnixTimeSeconds();
                    //cpu
                    connection.Execute("INSERT INTO networkmetrics (value, time) VALUES (@intValue,@longValue)",
                        new
                        {
                            intValue = intValue,
                            longValue = longValue
                        });
                }

                //добавляем значения реквизита time для ram
                string[] myRamTimeArray = new string[5] {
                    "2021-02-01T20:00:00+00:00",
                    "2021-02-02T00:00:00+00:00",
                    "2021-02-03T05:30:00+00:00",
                    "2021-02-04T10:30:00+00:00",
                    "2021-02-05T11:00:00+00:00"
                };

                for (int i = 0; i < myRamTimeArray.GetLength(0); i++)
                {
                    var intValue = myValueArray[i];
                    var longValue = DateTimeOffset.Parse(myNetworkTimeArray[i]).ToUnixTimeSeconds();
                    //cpu
                    connection.Execute("INSERT INTO rammetrics (value, time) VALUES (@intValue,@longValue)",
                        new
                        {
                            intValue = intValue,
                            longValue = longValue
                        });
                }
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
