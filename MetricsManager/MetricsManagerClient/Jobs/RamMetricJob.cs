﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MetricsManagerClient.ChartControls;
using MetricsManagerClient.Controller.Responses;
using Newtonsoft.Json;
using Quartz;

namespace MetricsManagerClient.Jobs
{
    class RamMetricJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var dataMap = context.MergedJobDataMap;
            string myUrl = (string)dataMap["myUrl"];
            var myChart = (ChartRam)dataMap["myChart"];

            string fromTime = DateTimeOffset.UtcNow.ToString();
            if (myChart.LineSeriesValues[0].Values.Count > 0)
            {
                var lastTime = myChart.Labels.GetValue(myChart.LineSeriesValues[0].Values.Count - 1).ToString();
                var toDTO = DateTimeOffset.Parse(lastTime);
                fromTime = toDTO.AddSeconds(1).ToString("0");
            }
            _ = GetRequestAsync(myUrl, myChart, context);
        }

        private async Task GetRequestAsync(string requestAddress, ChartRam myChart, IJobExecutionContext context)
        {
            AllRamMetricsResponse ramMetrics = new AllRamMetricsResponse();
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    using (HttpResponseMessage response = await httpClient.GetAsync(requestAddress))
                    {
                        using (HttpContent content = response.Content)
                        {
                            string data = await content.ReadAsStringAsync();
                            ramMetrics = JsonConvert.DeserializeObject<AllRamMetricsResponse>(data);

                            foreach (RamMetricDto metric in ramMetrics.Metrics)
                            {
                                myChart.LineSeriesValues[0].Values.Add(metric.Value);

                                if (myChart.LineSeriesValues[0].Values.Count > myChart.valuesCount)
                                {
                                    myChart.LineSeriesValues[0].Values.RemoveAt(0);
                                    for (int i = 1; i < myChart.valuesCount; i++)
                                    {
                                        myChart.Labels.SetValue(myChart.Labels.GetValue(i), i - 1);
                                    }
                                }

                                myChart.Labels.SetValue(metric.Time.ToString(), myChart.LineSeriesValues[0].Values.Count - 1);
                            }
                        }
                    }
                }
            }
            catch (Exception myex)
            {
                Console.WriteLine("GetRequestAsync exception " + myex.Message);
                await context.Scheduler.PauseJob(context.JobDetail.Key);
            }
        }
    }
}
