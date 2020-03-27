var dataCharts = echarts.init(document.getElementById('dataChart'));

var option = {
    tooltip: {
        trigger: 'axis'
    },
    color: ["#D50390", "#8F1EC2", "#66E638", "#2B98DC", "#04DD98", "#36A15D", "#F87F38", "#EEE8AB", "#E5B5B5", "#BCD3BB"],
    legend: {
        data: null,
    },
    grid: {
        left: '3%',
        right: '4%',
        bottom: '3%',
        containLabel: true
    },
    xAxis: {
        type: 'category',
        boundaryGap: false,
        data: null
    },
    yAxis: {
        type: 'value'
    },
    dataZoom: [
        {
            type: 'slider',
            xAxisIndex: 0,
            filterMode: 'empty'
        },
        {
            type: 'inside',
            xAxisIndex: 0,
            filterMode: 'empty'
        },
    ],
    series: [
        {
            name: '电镐方位角',
            type: 'line',
            data: null,
            smooth: true
        },
        {
            name: '电镐概率',
            type: 'line',
            data: null,
            smooth: true
        },
        {
            name: '挖掘机方位角',
            type: 'line',
            data: null,
            smooth: true
        },
        {
            name: '挖掘机概率',
            type: 'line',
            data: null,
            smooth: true
        },
        {
            name: '冲击锤方位角',
            type: 'line',
            data: null,
            smooth: true
        },
        {
            name: '冲击锤概率',
            type: 'line',
            data: null,
            smooth: true
        },
        {
            name: '切割机方位角',
            type: 'line',
            data: null,
            smooth: true
        },
        {
            name: '切割机概率',
            type: 'line',
            data: null,
            smooth: true
        },
        {
            name: '信号强度',
            type: 'line',
            data: null,
            smooth: true
        }
    ]
};

dataCharts.setOption(option);

/*重置Chart*/
function dataEchart(legendName, xAxisData, data1, data2, data3, data4, data5, data6, data7, data8, data9) {
    var options = dataCharts.getOption();
    if (dataCharts) {
        dataCharts.dispose();
    }
    dataCharts = echarts.init(document.getElementById('dataChart'));
    if (typeof options === 'object') {
        options.legend[0].data = legendName;
        options.xAxis[0].data = xAxisData;
        options.series[0].data = data1;
        options.series[1].data = data2;
        options.series[2].data = data3;
        options.series[3].data = data4;
        options.series[4].data = data5;
        options.series[5].data = data6;
        options.series[6].data = data7;
        options.series[7].data = data8;
        options.series[8].data = data9;
        dataCharts.setOption(options, true);
    }
}