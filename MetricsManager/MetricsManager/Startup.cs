using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using AutoMapper;
using FluentMigrator;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MetricsManager.DAL;
using System.IO;
using Quartz.Spi;
using Quartz.Impl;
using Quartz;
using MetricsManager.DAL.Interfaces;
using MetricsManager.DAL.Repositories;
using MetricsManager.Client;
using MetricsManager.Jobs.JobSchedule;
using MetricsManager.Jobs;
using Polly;
using Microsoft.OpenApi.Models;

namespace MetricsManager
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
            services.AddSingleton<IAgentsRepository, AgentsRepository>();
            services.AddSingleton<IMetricsAgentClient, MetricsAgentClient>();
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
            //
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
            //httpclient
            services.AddHttpClient<IMetricsAgentClient, MetricsAgentClient>()
                .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(1000)));
            //swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API сервиса агента сбора метрик",
                    Description = "Описание",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Pinayev",
                        Email = string.Empty,
                        Url = new Uri("https://yandex.ru"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "лицензия",
                        Url = new Uri("https://example.com/license"),
                    }
                });
            }
                );
        }

        public void ConfigureSqlLiteConnection(IServiceCollection services)
        {
            var connectionString = new SqlConnection().GetConnectionString();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                //PrepareSchema(connection);
            }
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

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API сервиса агента сбора метрик");
                c.RoutePrefix = string.Empty;
            });

            migrationRunner.MigrateUp();
        }
    }
}
