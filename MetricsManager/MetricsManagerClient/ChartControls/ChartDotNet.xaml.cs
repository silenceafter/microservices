using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using LiveCharts;
using LiveCharts.Wpf;

namespace MetricsManagerClient.ChartControls
{
    /// <summary>
    /// Interaction logic for ChartDotNet.xaml
    /// </summary>
    public partial class ChartDotNet : UserControl, INotifyPropertyChanged
    {
        public string[] Labels { get; set; }
        public int valuesCount = 30;
        public SeriesCollection LineSeriesValues { get; set; }

        public ChartDotNet()
        {
            InitializeComponent();

            LineSeriesValues = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "DotNetMetric",
                    Values = new ChartValues<int> {}
                }
            };

            Labels = new string[valuesCount];
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
