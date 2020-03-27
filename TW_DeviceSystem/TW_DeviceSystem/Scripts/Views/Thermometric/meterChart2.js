var meterChart2 = echarts.init(document.getElementById('meter2'));

var option = {
    tooltip: {
        formatter: "{a}: {c} {b}",
        extraCssText: 'width:auto;height:auto;'
    },
    series: [{
        center: ['50%', '50%'],
        name: '温度',
        type: 'gauge',
        min: 0,
        max: 220,
        splitNumber: 11,
        radius: '85%',
        axisLine: {
            lineStyle: {
                color: [
                    [0.09, 'lime'],
                    [0.82, '#1e90ff'],
                    [1, '#ff4500']
                ],
                width: 3,
                shadowColor: '#fff',
                shadowBlur: 10
            }
        },
        axisLabel: {
            textStyle: {
                fontWeight: 'bolder',
                color: '#010000',
                shadowColor: '#fff',
                shadowBlur: 10
            }
        },
        axisTick: {
            length: 15,
            lineStyle: {
                color: 'auto',
                shadowColor: '#0DEAF1',
                shadowBlur: 10
            }
        },
        splitLine: {
            length: 25,
            lineStyle: {
                width: 3,
                color: '#fff',
                shadowColor: '#67A8E8',
                shadowBlur: 10
            }
        },
        pointer: {
            shadowColor: '#fff',
            shadowBlur: 5
        },
        title: {
            textStyle: {
                fontWeight: 'bolder',
                fontSize: 20,
                fontStyle: 'italic',
                color: '#010000',
                shadowColor: '#fff',
                shadowBlur: 10
            }
        },
        detail: {
            backgroundColor: 'rgba(30,144,255,0.8)',
            borderWidth: 1,
            borderColor: '#fff',
            shadowColor: '#fff',
            shadowBlur: 5,
            offsetCenter: ['0', '70%'],
            textStyle: {
                fontWeight: 'bolder',
                color: '#fff'
            }
        },
        data: [{
            value: 0,
            name: '℃'
        }]
    }]
};
meterChart2.setOption(option);

var oldValue2 = null;
function setmeterChart2(value) {
    if (oldValue2 != value) {
        var options = meterChart2.getOption();
        if (meterChart2) {
            meterChart2.dispose();
        }
        meterChart2 = echarts.init(document.getElementById('meter2'));
        options.series[0].data[0].value = value;
        meterChart2.setOption(options);
        oldValue2 = value;
    } else {
        return;
    }
   
}

window.onresize = function () {
    meterChart2.resize();
}