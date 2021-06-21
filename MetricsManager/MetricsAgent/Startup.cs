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
using FluentMigrator;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using System.IO;
using Quartz.Spi;
using Quartz.Impl;
using Quartz;
using MetricsAgent.Jobs;
using Microsoft.OpenApi.Models;
using System.Reflection;

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
            //mapper
            var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new MapperProfile()));
            var mapper = mapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);
            //fluentMigrator
            services.AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddSQLite()
                    .WithGlobalConnectionString(new SqlConnection().GetConnectionString())
                    .ScanIn(typeof(Startup).Assembly).For.Migrations()
                ).AddLogging(lb => lb
                    .AddFluentMigratorConsole());

            //quartz
            services.AddSingleton<IJobFactory, SingleJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddSingleton<CpuMetricJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(CpuMetricJob),
                cronExpression: "0/5 * * * * ?"));
            services.AddSingleton<DotNetMetricJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(DotNetMetricJob),
                cronExpression: "0/5 * * * * ?"));
            services.AddSingleton<HddMetricJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(HddMetricJob),
                cronExpression: "0/5 * * * * ?"));
            services.AddSingleton<NetworkMetricJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(NetworkMetricJob),
                cronExpression: "0/5 * * * * ?"));
            services.AddSingleton<RamMetricJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(RamMetricJob),
                cronExpression: "0/5 * * * * ?"));

            services.AddHostedService<QuartzHostedService>();
        }

        public void ConfigureSqlLiteConnection(IServiceCollection services)
        {
            var connectionString = new SqlConnection().GetConnectionString();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                PrepareSchema(connection);
            }
        }

        private void PrepareSchema(SQLiteConnection connection)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMigrationRunner migrationRunner)
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

            migrationRunner.MigrateUp();
        }
    }
}
