using System;
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
    class DotNetMetricJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var dataMap = context.MergedJobDataMap;
            string myUrl = (string)dataMap["myUrl"];
            var myChart = (ChartDotNet)dataMap["myChart"];

            string fromTime = DateTimeOffset.UtcNow.ToString();
            if (myChart.LineSeriesValues[0].Values.Count > 0)
            {
                var lastTime = myChart.Labels.GetValue(myChart.LineSeriesValues[0].Values.Count - 1).ToString();
                var toDTO = DateTimeOffset.Parse(lastTime);
                fromTime = toDTO.AddSeconds(1).ToString("0");
            }
            //string address = "http://localhost:5001/api/metrics/dotnet/agent/1/from/2021-06-19T00:00:00+03:00/to/2021-06-21T00:00:00+03:00";
            _ = GetRequestAsync(myUrl, myChart, context);
        }

        private async Task GetRequestAsync(string requestAddress, ChartDotNet myChart, IJobExecutionContext context)
        {
            AllDotNetMetricsResponse dotnetMetrics = new AllDotNetMetricsResponse();
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    using (HttpResponseMessage response = await httpClient.GetAsync(requestAddress))
                    {
                        using (HttpContent content = response.Content)
                        {
                            string data = await content.ReadAsStringAsync();
                            dotnetMetrics = JsonConvert.DeserializeObject<AllDotNetMetricsResponse>(data);

                            foreach (DotNetMetricDto metric in dotnetMetrics.Metrics)
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
