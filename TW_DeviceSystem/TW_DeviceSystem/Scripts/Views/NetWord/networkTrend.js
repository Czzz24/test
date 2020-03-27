var networkChart = echarts.init(document.getElementById('networkTrend'));

var data = [["2000-06-05", 1], ["2000-06-06", 1], ["2000-06-07", 1], ["2000-06-08", 2], ["2000-06-09", 2], ["2000-06-10", 2], ["2005-09-05", 3], ["2005-09-06", 3], ["2005-09-07", 4], ["2005-09-08", 5], ["2005-09-09", 5]]

// 指定图表的配置项和数据
var option = {
    title: {
        text: '设备通讯状态',

    },
    tooltip: {
        trigger: 'axis',
        axisPointer: {
            type: 'cross'
        }
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
    visualMap: {
        top: 10,
        right: 10,
        pieces: [{
            gt: 0,
            lte: 1,
            color: '#4CF129'
        }, {
            gt: 1,
            lte: 2,
            color: '#F7511E'
        }, {
            gt: 2,
            lte: 3,
            color: '#1AD5F0'
        }, {
            gt: 3,
            lte: 4,
            color: '#EBF214'
        }, {
            gt: 4,
            lte: 5,
            color: '#6214F2'
        }
        ],
        outOfRange: {
            color: '#6214F2'
        }
    },
    xAxis: {
        data: data.map(function (item) {
            return item[0];
        })
    },
    yAxis: {
        splitLine: {
            show: false
        }
    },
    //1.校验通过.2.校验不通过.3.没回复模块正常.4.网络无连接,5.网络已连接
    series: {
        name: '通讯状态',
        type: 'line',
        data: data.map(function (item) {
            return item[1];
        }),
        markLine: {
            silent: true,
            data: [{
                yAxis: 1
            }, {
                yAxis: 2
            }, {
                yAxis: 3
            }, {
                yAxis: 4
            },
            {
                yAxis: 5
            }]
        }
    }
};

// 使用刚指定的配置项和数据显示图表。
networkChart.setOption(option);

/*重置echarts*/
function resetnetworkChart(data) {
    var options = networkChart.getOption();
    if (networkChart) {
        networkChart.dispose();
    }
    networkChart = echarts.init(document.getElementById('networkTrend'));
    if (typeof options === 'object') {
        options.xAxis[0].data = data.map(function (item) {
            return item[0];
        })
        options.series[0].data = data.map(function (item) {
            return item[1];
        })
        networkChart.setOption(options, true);
    }
}