using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Application.From.Model.Echarts
{
    public class CurveRoot
    {
        /// <summary>
        /// 工具提示
        /// </summary>
        public Tooltip tooltip { get; set; }

        /// <summary>
        /// 颜色数组
        /// </summary>
        public List<string> color { get; set; }

        /// <summary>
        /// 图例对象
        /// </summary>
        public Legend legend { get; set; }

        /// <summary>
        /// 网格
        /// </summary>
        public Grid grid { get; set; }

        /// <summary>
        /// x轴对象
        /// </summary>
        public XAxis xAxis { get; set; }

        /// <summary>
        /// Y轴对象
        /// </summary>
        public YAxis yAxis { get; set; }

        /// <summary>
        /// 数据仓库工具条
        /// </summary>
        public List<DataZoom> dataZoom { get; set; }

        /// <summary>
        /// 整个series
        /// </summary>
        public List<Series> series { get; set; }
    }

    /// <summary>
    /// 工具提示
    /// </summary>
    public class Tooltip
    {
        /// <summary>
        /// 触发
        /// </summary>
        public string trigger { get; set; }
    }


    public class Legend
    {
        /// <summary>
        /// 图例名称
        /// </summary>
        public List<string> data { get; set; }
    }

    /// <summary>
    /// 网格
    /// </summary>
    public class Grid
    {
        /// <summary>
        /// 3%
        /// </summary>
        public string left { get; set; }
        /// <summary>
        /// 4%
        /// </summary>
        public string right { get; set; }
        /// <summary>
        /// 3%
        /// </summary>
        public string bottom { get; set; }
        /// <summary>
        /// 包含标签
        /// </summary>
        public bool containLabel { get; set; }
    }

    public class XAxis
    {
        /// <summary>
        /// category
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// BoundaryGap
        /// </summary>
        public bool boundaryGap { get; set; }

        /// <summary>
        /// x抽数据
        /// </summary>
        public List<string> data { get; set; }
    }

    public class YAxis
    {
        /// <summary>
        /// value
        /// </summary>
        public string type { get; set; }
    }

    public class DataZoom
    {
        /// <summary>
        /// slider
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// XAxisIndex
        /// </summary>
        public int xAxisIndex { get; set; }
        /// <summary>
        /// empty
        /// </summary>
        public string filterMode { get; set; }
        /// <summary>
        /// M10.7,11.9H9.3c-4.9,0.3-8.8,4.4-8.8,9.4c0,5,3.9,9.1,8.8,9.4h1.3c4.9-0.3,8.8-4.4,8.8-9.4C19.5,16.3,15.6,12.2,10.7,11.9z M13.3,24.4H6.7V23h6.6V24.4z M13.3,19.6H6.7v-1.4h6.6V19.6z
        /// </summary>
        public string handleIcon { get; set; }
    }

    public class Series
    {
        /// <summary>
        /// 对应图例名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 图表类型
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 图表数值
        /// </summary>
        public List<decimal?> data { get; set; }

        /// <summary>
        /// (true)曲线(false)折线
        /// </summary>
        public bool smooth { get; set; }
    }

    /// <summary>
    /// Y轴数据
    /// </summary>
    public class YValue
    {
        [Key]
        public decimal? value { get; set; }
    }

    /// <summary>
    /// x抽名称
    /// </summary>
    public class XValue
    {
        public DateTime? Time { get; set; }
    }

    public class HYValue
    {
        public int value { get; set; }
    }
}
