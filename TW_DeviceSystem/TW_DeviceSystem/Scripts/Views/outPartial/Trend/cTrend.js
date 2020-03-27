var tmyChart3 = echarts.init(document.getElementById('cTrend'));

var data = [
   ["2018-11-01\n00:00:09", "0", "0", "2", "0", "0", "0"],
   ["2018-11-01\n00:00:45", "0", "0", "2", "0", "0", "0"],
   ["2018-11-01\n00:01:21", "0", "0", "2", "0", "0", "0"],
   ["2018-11-01\n00:01:58", "0", "0", "2", "0", "0", "0"],
   ["2018-11-01\n00:02:34", "0", "0", "2", "0", "0", "0"],
   ["2018-11-01\n00:03:10", "0", "0", "2", "0", "0", "0"],
   ["2018-11-01\n00:03:47", "0", "0", "2", "0", "0", "0"],
   ["2018-11-01\n00:04:23", "0", "0", "2", "0", "0", "0"],
   ["2018-11-01\n00:04:59", "0", "0", "2", "0", "0", "0"],
   ["2018-11-01\n00:05:36", "0", "0", "2", "0", "0", "0"],
   ["2018-11-01\n00:06:12", "0", "0", "2", "0", "0", "0"],
   ["2018-11-01\n00:06:49", "0", "0", "2", "0", "0", "0"],
   ["2018-11-01\n00:07:25", "0", "0", "2", "0", "0", "0"],
   ["2018-11-01\n00:08:02", "0", "0", "2", "0", "0", "0"],
   ["2018-11-01\n00:08:38", "0", "0", "2", "0", "0", "0"],
   ["2018-11-01\n00:09:15", "0", "0", "2", "0", "0", "0"],
]

var option = {
    title: {
        text: 'C相放电量走势图'
    },
    tooltip: {
        trigger: 'axis'
    },
    xAxis: {
        data: data.map(function (item) {
            return item[0];
        }),
    },
    yAxis: {
        splitLine: {
            show: false
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
            lte: 50,
            color: '#66DE04'
        }, {
            gt: 50,
            lte: 200,
            color: '#F1F32A'
        }, {
            gt: 200,
            lte: 500,
            color: '#F04622'
        }],
        outOfRange: {
            color: '#FF0000'
        }
    },
    series: [
        {
            name: 'C相放电量走势',
            type: 'line',
            data: data.map(function (item) {
                return item[5];
            }),
            markLine: {
                silent: true,
                data: [{
                    yAxis: 50
                }, {
                    yAxis: 200
                }, {
                    yAxis: 300
                }]
            }
        }
    ]
}

tmyChart3.setOption(option);

/*重置echarts*/
function resettmyChart3(data) {
    var options = tmyChart3.getOption();
    if (tmyChart3) {
        tmyChart3.dispose();
    }
    tmyChart3 = echarts.init(document.getElementById('cTrend'));
    if (typeof options === 'object') {
        options.xAxis[0].data = data.map(function (item) {
            return item[0];
        })
        options.series[0].data = data.map(function (item) {
            return item[3];
        })
        tmyChart3.setOption(options, true);
    }
}