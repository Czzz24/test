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
        data: null,
    },
    yAxis: {
        type: 'value'
    },
    dataZoom: [
        {
            type: 'slider',
            xAxisIndex: 0,
            filterMode: 'empty',
            handleIcon: 'M10.7,11.9H9.3c-4.9,0.3-8.8,4.4-8.8,9.4c0,5,3.9,9.1,8.8,9.4h1.3c4.9-0.3,8.8-4.4,8.8-9.4C19.5,16.3,15.6,12.2,10.7,11.9z M13.3,24.4H6.7V23h6.6V24.4z M13.3,19.6H6.7v-1.4h6.6V19.6z',
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
            name: 'A相电流(A)',
            type: 'line',
            data: null,
            smooth: true
        },
        {
            name: 'B相电流(A)',
            type: 'line',
            data: null,
            smooth: true
        },
        {
            name: 'C相电流(A)',
            type: 'line',
            data: null,
            smooth: true
        },
        {
            name: 'A相电压(V)',
            type: 'line',
            data: null,
            smooth: true
        },
        {
            name: 'B相电压(V)',
            type: 'line',
            data: null,
            smooth: true
        },
        {
            name: 'C相电压(V)',
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
            name: '蓄电池电压(V)',
            type: 'line',
            data: null,
            smooth: true
        }
    ]
};

dataCharts.setOption(option);

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