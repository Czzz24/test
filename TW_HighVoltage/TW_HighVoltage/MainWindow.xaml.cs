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
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Panuon.UI.Silver;

namespace TW_HighVoltage
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string temp = "28.2,29.6,27.3,27.5,26.7,30,31.1,31,27.3,28.2,28.4,29.3,29.5,29.3,28.4,28.8,29.4,31.1,30.7,29.9,27.4,29.5,26.7,28,26,28.8,27.5,27.8,28.6,29.8,29.3,25.6,26.5,26.9,27.4,27.9,29,27.1,28.6,29.7,29.2,28.7,28.8,29.2,29.9,28.6,28.8,29.4,28.5,28.2,29.1,29.1,29.6,28.9,27.4,26.2,28.2,27.1,28.4,28.8,57.6,59.3,57.9,59.1,60.4,58,59.4,58.2,57.4,58.8,57.5,58.6,59.6,60.5,59,59.1,58.4,56.8,58.7,59,59.3,59.9,57.1,58.5,58.9,58.9,29.4,29.6,28.6,28.3,28.3,28,28.8,27.1,27.4,27.1,27.8,26.6,28.3,29,29.5,28.2,27.5,26,28.9,28.2,27.1,26.5,28.7,28.5";
                string[] temps = temp.Split(',');
                double[] doutemps = new double[temps.Length];
                for(int i = 0; i < temps.Length; i++)
                {
                    doutemps[i] = Convert.ToDouble(temps[i]);
                }
                //初始化数据序列集合
                SeriesCollection collection = new SeriesCollection();
                //初始化数据序列
                ScatterSeries series = new ScatterSeries()
                {
                    Values = new ChartValues<ScatterPoint>()
                };
                for (int i = 1; i < temps.Count()+1; i++)
                {
                    double x = i;
                    double y =  Convert.ToDouble(temps[i-1]);
                    ScatterPoint point = new ScatterPoint(x, y);
                    series.Values.Add(point);
                }
                //collection.Add(series);
                //实例化一条折线图
                LineSeries mylineseries = new LineSeries();
                //设置折线的标题
                mylineseries.Title = "Temp";
                //折线图直线形式
                mylineseries.LineSmoothness = 0;
                //折线图的无点样式
                mylineseries.PointGeometry = null;
                mylineseries.Values = new ChartValues<double>(doutemps);
                collection.Add(mylineseries);
                this.lvcChart.Series = collection;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

