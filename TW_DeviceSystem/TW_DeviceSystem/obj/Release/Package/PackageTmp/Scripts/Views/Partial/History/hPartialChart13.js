var hmyChart13 = echarts.init(document.getElementById('hform13'));

option = {
    title: {
        text: 'A相3通道放电波形(单位mV)',
        textStyle: {
            color: '#8F3A2F',
            fontSize: 12,
        },
        top: '5%',
        left: 'center',
    },
    color: ['#EFEF3F'],
    tooltip: {
        trigger: 'axis',
        axisPointer: {
            type: 'cross',
            lineStyle: {
                color: '#333333',
            },
            crossStyle: {
                color: '#333333',
            }
        }
    },
    xAxis: {
        type: 'category',
        data: null,
    },
    yAxis: {
        type: 'value',
        splitNumber: 4,
        splitLine: {
            lineStyle: {
                color: '#898C95',
            }
        },
        min: -500,
        max: 500
    },
    grid: {
        top: '25%',
        left: '5%',
        right: '5%',
        bottom: '5%',
        containLabel: true
    },
    series: [{
        symbol: "none",
        name: "波形",
        data: null,
        type: 'line',
        smooth: true
    }]
};

hmyChart13.setOption(option);

/*重置Chart13*/
function hresetEchart13(xAxisData, seriesData, titleText, color, Yvalue) {
    var options = hmyChart13.getOption();
    if (hmyChart13) {
        hmyChart13.dispose();
    }
    hmyChart13 = echarts.init(document.getElementById('hform13'));
    if (typeof options === 'object') {
        options.color = color;
        options.yAxis[0].min = Yvalue - (Yvalue * 2);
        options.yAxis[0].max = Yvalue;
        options.xAxis[0].data = xAxisData;
        options.series[0].data = seriesData;
        options.title[0].text = titleText;
        hmyChart13.setOption(options, true);
    }
}