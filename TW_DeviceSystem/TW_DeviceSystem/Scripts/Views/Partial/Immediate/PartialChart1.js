var myChart1 = echarts.init(document.getElementById('form1'));

option = {
    color: ['#EFEF3F'],
    title: {
        text: 'A相1通道放电量(单位PC)',
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

myChart1.setOption(option);

/*重置Chart1*/
function resetEchart1(xAxisData, seriesData, titleText, yMax, color) {
    var options = myChart1.getOption();
    if (myChart1) {
        myChart1.dispose();
    }
    myChart1 = echarts.init(document.getElementById('form1'));
    if (typeof options === 'object') {
        options.color = color;
        options.xAxis[0].data = xAxisData;
        options.series[0].data = seriesData;
        options.title[0].text = titleText;
        options.yAxis[0].max = yMax;
        myChart1.setOption(options, true);
    }
}