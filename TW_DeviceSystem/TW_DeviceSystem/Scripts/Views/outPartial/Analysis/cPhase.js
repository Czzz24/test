
var cPhaseChart = echarts.init(document.getElementById('cPhase'));
var option = {
    title: {
        text: 'C相',
        textStyle: {
            color: '#8F3A2F',
            fontSize: 14
        }
    },
    tooltip: {
        trigger: 'axis',
        axisPointer: {
            type: 'cross',
            label: {
                backgroundColor: '#6a7985'
            }
        }
    },
    legend: {
        data: ['C相最大放电量', 'C相最大放电频次'],
        top: 25,
    },
    grid: {
        left: '3%',
        right: '4%',
        bottom: '3%',
        containLabel: true
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
    xAxis: [
        {
            type: 'category',
            boundaryGap: false,
            data: null
        }
    ],
    yAxis: [
        {
            type: 'value',
        }
    ],
    series: [
        {
            name: 'C相最大放电量',
            type: 'line',
            smooth: true,
            symbol: 'circle',
            showSymbol: false,
            areaStyle: {
                normal: {
                    color: new echarts.graphic.LinearGradient(0, 0, 0, 1, [{
                        offset: 0,
                        color: 'rgba(137, 189, 27, 0.3)'
                    }, {
                        offset: 0.8,
                        color: 'rgba(137, 189, 27, 0)'
                    }], false),
                    shadowColor: 'rgba(0, 0, 0, 0.1)',
                    shadowBlur: 10
                }
            },
            itemStyle: {
                normal: {
                    color: 'rgb(137,189,27)',
                    borderColor: 'rgba(137,189,2,0.27)',
                    borderWidth: 12

                }
            },
            data: null
        },
        {
            name: 'C相最大放电频次',
            type: 'line',
            smooth: true,
            symbol: 'circle',
            showSymbol: false,
            areaStyle: {
                normal: {
                    color: new echarts.graphic.LinearGradient(0, 0, 0, 1, [{
                        offset: 0,
                        color: 'rgba(219, 50, 51, 0.3)'
                    }, {
                        offset: 0.8,
                        color: 'rgba(219, 50, 51, 0)'
                    }], false),
                    shadowColor: 'rgba(0, 0, 0, 0.1)',
                    shadowBlur: 10
                }
            },
            itemStyle: {
                normal: {

                    color: 'rgb(219,50,51)',
                    borderColor: 'rgba(219,50,51,0.2)',
                    borderWidth: 12
                }
            },
            data: null
        }
    ]
};


// 使用刚指定的配置项和数据显示图表。
cPhaseChart.setOption(option);

/*重置Chart*/
function cPhaseEchart(xAxisData, electricData, rateData) {
    var options = cPhaseChart.getOption();
    if (cPhaseChart) {
        cPhaseChart.dispose();
    }
    cPhaseChart = echarts.init(document.getElementById('cPhase'));
    if (typeof options === 'object') {
        options.xAxis[0].data = xAxisData;
        options.series[0].data = electricData;
        options.series[1].data = rateData;
        cPhaseChart.setOption(options, true);
    }
}