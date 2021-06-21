using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Configuration;
using System.Collections.Specialized;
using MetricsManagerClient.ChartControls;
using Quartz.Impl;
using Quartz;
using MetricsManagerClient.Jobs;

namespace MetricsManagerClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int DotsOnChart { get; set; }
        public string[] UrlParts { get; set; }

        public MainWindow()
        {
            InitializeComponent();           
        }

        public async void Button_Click(object sender, RoutedEventArgs e)
        {
            buttonStart.IsEnabled = false;
            var chartCpu = CpuChart;
            var chartDotNet = DotNetChart;
            var chartHdd = HddChart;
            var chartNetwork = NetworkChart;
            var chartRam = RamChart;

            string address = "http://localhost:5001/api/metrics/cpu/agent/1/from/2021-06-19T00:00:00+03:00/to/2021-06-22T00:00:00+03:00";//urlValue.Text;
            StdSchedulerFactory factory = new StdSchedulerFactory();
            IScheduler scheduler = await factory.GetScheduler();
            await scheduler.Start();
        
            //cpu
            IJobDetail jobCPU = JobBuilder.Create<CpuMetricJob>()
                .WithIdentity("myJob", "group1")
                .Build();
            jobCPU.JobDataMap.Put("myChart", chartCpu);
            jobCPU.JobDataMap.Put("myUrl", address);
            ITrigger triggerCPU = TriggerBuilder.Create()
                .WithIdentity("myTrigger", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                .WithIntervalInSeconds(10)
                .RepeatForever())
                .Build();

            await scheduler.ScheduleJob(jobCPU, triggerCPU);

            address = "http://localhost:5001/api/metrics/dotnet/agent/1/from/2021-06-19T00:00:00+03:00/to/2021-06-22T00:00:00+03:00";
            //dotnet
            IJobDetail jobDotNet = JobBuilder.Create<DotNetMetricJob>()
                .WithIdentity("myJob", "group2")
                .Build();
            jobDotNet.JobDataMap.Put("myChart", chartDotNet);
            jobDotNet.JobDataMap.Put("myUrl", address);
            ITrigger triggerDotNet = TriggerBuilder.Create()
                .WithIdentity("myTrigger", "group2")
                .StartNow()
                .WithSimpleSchedule(x => x
                .WithIntervalInSeconds(10)
                .RepeatForever())
                .Build();

            await scheduler.ScheduleJob(jobDotNet, triggerDotNet);

            address = "http://localhost:5001/api/metrics/hdd/agent/1/from/2021-06-19T00:00:00+03:00/to/2021-06-22T00:00:00+03:00";
            //hdd
            IJobDetail jobHdd = JobBuilder.Create<HddMetricJob>()
                .WithIdentity("myJob", "group3")
                .Build();
            jobHdd.JobDataMap.Put("myChart", chartHdd);
            jobHdd.JobDataMap.Put("myUrl", address);
            ITrigger triggerHdd = TriggerBuilder.Create()
                .WithIdentity("myTrigger", "group3")
                .StartNow()
                .WithSimpleSchedule(x => x
                .WithIntervalInSeconds(10)
                .RepeatForever())
                .Build();

            await scheduler.ScheduleJob(jobHdd, triggerHdd);

            address = "http://localhost:5001/api/metrics/network/agent/1/from/2021-06-19T00:00:00+03:00/to/2021-06-22T00:00:00+03:00";
            //network
            IJobDetail jobNetwork = JobBuilder.Create<NetworkMetricJob>()
                .WithIdentity("myJob", "group4")
                .Build();
            jobNetwork.JobDataMap.Put("myChart", chartNetwork);
            jobNetwork.JobDataMap.Put("myUrl", address);
            ITrigger triggerNetwork = TriggerBuilder.Create()
                .WithIdentity("myTrigger", "group4")
                .StartNow()
                .WithSimpleSchedule(x => x
                .WithIntervalInSeconds(10)
                .RepeatForever())
                .Build();

            await scheduler.ScheduleJob(jobNetwork, triggerNetwork);

            address = "http://localhost:5001/api/metrics/ram/agent/1/from/2021-06-19T00:00:00+03:00/to/2021-06-22T00:00:00+03:00";
            //ram
            IJobDetail jobRam = JobBuilder.Create<RamMetricJob>()
                            .WithIdentity("myJob", "group5")
                            .Build();
            jobRam.JobDataMap.Put("myChart", chartRam);
            jobRam.JobDataMap.Put("myUrl", address);
            ITrigger triggerRam = TriggerBuilder.Create()
                .WithIdentity("myTrigger", "group5")
                .StartNow()
                .WithSimpleSchedule(x => x
                .WithIntervalInSeconds(10)
                .RepeatForever())
                .Build();

            await scheduler.ScheduleJob(jobRam, triggerRam);
        }
    } 
}
