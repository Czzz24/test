var partialAnyChart = echarts.init(document.getElementById('partialAnyChart'));

var option = {
    tooltip: {
        trigger: 'axis',
        axisPointer: {            // 坐标轴指示器，坐标轴触发有效
            type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
        }
    },
    legend: {
        data: ['状态评估']
    },
    grid: {
        top:20,
        left: '3%',
        right: '4%',
        bottom: 0,
        width: '100%',
        height:'100%',
        containLabel: true
    },
    color: ['#79F83A', '#3BE591', '#17BA7C', '#F7A45C', '#F92817'],
    xAxis: {
        type: 'value',
        show: false,
    },
    yAxis: {
        type: 'category',
        data: ['状态评估'],
        axisTick: {
            show: false,
        }
    },
    series: [
        {
            name: '无局放信号',
            type: 'bar',
            barMaxWidth:40,
            stack: '总量',
            label: {
                normal: {
                    show: true,
                    position: 'insideRight'
                }
            },
            data: [5]
        },
        {
            name: '偶发且低于设定值局放信号',
            type: 'bar',
            barMaxWidth: 40,
            stack: '总量',
            label: {
                normal: {
                    show: true,
                    position: 'insideRight'
                }
            },
            data: [10]
        },
        {
            name: '连续且低于设置值局放信号',
            type: 'bar',
            barMaxWidth: 40,
            stack: '总量',
            label: {
                normal: {
                    show: true,
                    position: 'insideRight'
                }
            },
            data: [15]
        },
        {
            name: '连续且上升趋势局放信号',
            type: 'bar',
            barMaxWidth: 40,
            stack: '总量',
            label: {
                normal: {
                    show: true,
                    position: 'insideRight'
                }
            },
            data: [20]
        },
        {
            name: '连续上升并高于设定值局放信号',
            type: 'bar',
            barMaxWidth: 40,
            stack: '总量',
            label: {
                normal: {
                    show: true,
                    position: 'insideRight'
                }
            },
            data: [25]
        }
    ]
};

// 使用刚指定的配置项和数据显示图表。
partialAnyChart.setOption(option);