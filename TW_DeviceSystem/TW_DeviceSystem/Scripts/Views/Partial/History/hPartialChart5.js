var hmyChart5 = echarts.init(document.getElementById('hform5'));

option = {
    color: ['#EFEF3F'],
    title: {
        text: 'A相5通道放电量(单位PC)',
        textStyle: {
            color: '#8F3A2F',
            fontSize: 12,
        },
        top: '5%',
        left: 'center',
    },
    tooltip: {
        trigger: 'axis',
        axisPointer: {
            type: 'line',
            lineStyle: {
                color: '#333333',
            }
        }
    },
    grid: {
        top: '25%',
        left: '5%',
        right: '5%',
        bottom: '5%',
        containLabel: true
    },
    xAxis: {
        type: 'category',
        data: null,
        silent: false,
        axisLabel: {
            color: '#333333',
        },
        axisLine: {
            lineStyle: {
                color: '#333333',
            }
        },
        splitLine: {
            show: false
        }
    },
    yAxis: [{
        type: 'value',
        axisLabel: {
            color: '#333333',
        },
        axisLine: {
            lineStyle: {
                color: '#333333',
            }
        },
        splitLine: {
            lineStyle: {
                color: '#898C95',
            }
        },
    }],
    series: [{
        name: '放电量',
        type: 'bar',
        data: null,
        barWidth: '40%'
    }]
};

hmyChart5.setOption(option);


/*重置Chart5*/
function hresetEchart5(xAxisData, seriesData, titleText, yMax, color) {
    var options = hmyChart5.getOption();
    if (hmyChart5) {
        hmyChart5.dispose();
    }
    hmyChart5 = echarts.init(document.getElementById('hform5'));
    if (typeof options === 'object') {
        options.color = color;
        options.xAxis[0].data = xAxisData;
        options.series[0].data = seriesData;
        options.title[0].text = titleText;
        options.yAxis[0].max = yMax;
        hmyChart5.setOption(options, true);
    }
}