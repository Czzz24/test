var dataCharts = echarts.init(document.getElementById('dataChart'));

var lineoptions = {
    tooltip: {
        trigger: 'axis'
    },
    color: ["#D50390", "#8F1EC2", "#66E638", "#2B98DC", "#04DD98", "#36A15D", "#F87F38", "#EEE8AB", "#E5B5B5"],
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
        data:null,
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
            name: '箱內溫度(°C)',
            type: 'line',
            data: null,
            smooth: true
        },
        {
            name: '箱内湿度(%)',
            type: 'line',
            data: null,
            smooth: true
        },
        {
            name: '取电电压(V)',
            type: 'line',
            data: null,
            smooth: true
        },
        {
            name: '太阳能电压(V)',
            type: 'line',
            data: null,
            smooth: true
        },
        {
            name: '蓄电池电压(V)',
            type: 'line',
            data: null,
            smooth: true
        },
        {
            name: 'A相温度',
            type: 'line',
            stack: '总量',
            data: null,
            smooth: true
        },
        {
            name: 'B相温度',
            type: 'line',
            data: null,
            smooth: true
        },
        {
            name: 'C相温度',
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

dataCharts.setOption(lineoptions);


/*重置Chart*/
function dataEchart(option) {
    var options = dataCharts.getOption();
    if (dataCharts) {
        dataCharts.dispose();
    }
    dataCharts = echarts.init(document.getElementById('dataChart'));
    if (typeof options === 'object') {
        dataCharts.setOption(option, true);
    }
}