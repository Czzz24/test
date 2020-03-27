/*
*
*BigMap地图封装操作JS
*/
var bigmap = bigmap || {};
/*
*
*封装配置
*/
var myOption = {
    /*地图最大级别*/
    maxZoom: 16,
    /*地图最小级别*/
    minZoom: 4,
    /*地图名称*/
    name: "杭州市个性化电子地图",
    /*地图服务地址*/
    url: 'http://60.12.250.45:8083/Tiles_BIGEMAP/',
    /*地图服务后缀类型.png.wtms.tms*/
    _suffx: '.png',
};
/*
*
*地图配置
*/
var mapOption = {
    /*地图中心点*/
    center: new google.maps.LatLng(30.254536111199418, 119.79416274683877),
    /*默认显示级别*/
    zoom: 6,
    /*缩放控件*/
    zoomControl: false,
    /*是否显示全屏控件*/
    fullscreenControl: false,
    /*是否显示街道控件*/
    streetViewControl: false,
    /*是否显示切换地图类型控件*/
    mapTypeControl: false,
    /*ctrl控制地图缩放*/
    gestureHandling: 'greedy',
}

/*点配置*/
var markerOption = {
    /*唯一属性*/
    id:null,
    /*标注的原点坐标*/
    anchorPoint: google.maps.Point(0, 0),
    /*标注的动画数据google.maps.Animation. DROP和google.maps.Animation.BOUNCE*/
    animationbounce: google.maps.Animation.BOUNCE,
    animationdrop: google.maps.Animation.DROP,
    animation:null,
    /*boolean是否接收鼠标点击事件*/
    clickable: true,
    /*boolean是否允许鼠标移动过程时拖动*/
    crossOnDrag: false,
    /*鼠标样式string*/
    cursor: 'pointer',
    /*boolean是否可拖动*/
    draggable: false,
    /*icon图标*/
    icon: 'Img/location/temp.png',
    /*标注的文本标注string|MarkerLabel*/
    label: null,
    /*标注的存放地图对象*/
    map: null,
    /*透明度 0.0到1.0*/
    opacity: 1,
    /*boolean是否渲染标注是：将所有的标注一直渲染（默认）否：标注将单独渲染，当图标是GIF或PNG格式时，可设置为false*/
    optimized: false,
    /*标注的位置 必须*/
    position:null,
    /*标注的标题，当鼠标在标注上时显示*/
    title: null,
    /*是否可见boolean*/
    visible: true,
    /*设置标注的层级，所有的标注都以层级的关键进行上下层级显示*/
    zIndex: 1,
}

/*线配置*/
var polylineOption = {
    /*是否允许单击*/
    clickable: false,
    /*是否可拖动*/
    draggable: false,
    /*是否可编辑*/
    editable: false,
    /*是否根据当前投影变换*/
    geodesic: false,
    /*上线段上显示的图标*/
    icons: [
             {
                 fixedRotation: false,
                 icon: {
                     scale: 2,
                     path: google.maps.SymbolPath.FORWARD_OPEN_ARROW
                 },
                 offset: 0,
                 repeat: '50%'
             }
    ],
    /*多线段展示的Map 对象*/
    map:null,
    /*多线段路径*/
    path: [],
    /*边颜色*/
    strokeColor: "#FAD949",
    /*透明度0.0-1.0*/
    strokeOpacity: 1,
    /*边宽度单位px*/
    strokeWeight: 2,
    /*是否可见*/
    visible: true,
    /*多线段层级*/
    zIndex: 1,
}

/*多边形配置*/
var polygonOption = {
    /*boolean是否允许单击*/
    clickable: true,
    /*是否可拖动*/
    draggable: false,
    /*是否可编辑*/
    editable: false,
    /*填充色支持所以 CSS3 颜色*/
    fillColor: '#FF0000',
    /*填充透明度，从0.0到1.0*/
    fillOpacity: 0.3,
    /*是否允许多边形根据投影相应的就换*/
    geodesic: true,
    /*多边形的Map对象*/
    map: null,
    /*多边形的路径*/
    path: [],
    /*描边颜色，支持所以 CSS3 颜色*/
    strokeColor: "#FAD949",
    /*描边透明度 0.0到1.0*/
    strokeOpacity: 1,
    /*描边的位置，可用的值：google.maps.StrokePosition.INSIDE,google.maps.StrokePosition.CENTER,google.maps.StrokePosition.OUTSIDE*/
    strokePosition: google.maps.StrokePosition.OUTSIDE,
    /*边宽度单位px*/
    strokeWeight: 2,
    /*是否可见*/
    visible: true,
    /*多边形的层级*/
    zIndex:1,
}

/*矩形配置*/
var rectangleOption = {
    /*边界*/
    bounds: null,
    /*是否允许单击*/
    clickable:true,
    /*是否允许拖拽*/
    draggable: false,
    /*是否允许编辑*/
    editable: false,
    /*填充颜色 所有 CSS3颜色都可使用*/
    fillColor: '#FF0000',
    /*填充透明度，从0.0到1.0*/
    fillOpacity:0.3,
    /*矩形的Map对象*/
    map:null,
    /*描边颜色，所有 CSS3 颜色都可用*/
    strokeColor: "#FAD949",
    /*描边透明度，从0.0到1.0*/
    strokeOpacity: 1.0,
    /*描边的位置，可用的值：google.maps.StrokePosition.INSIDE,google.maps.StrokePosition.CENTER,google.maps.StrokePosition.OUTSIDE*/
    strokePosition: google.maps.StrokePosition.OUTSIDE,
    /*描边大小，以像素为单位*/
    strokeWeight: 2,
    /*是否可见*/
    visible:true,
    /*层级*/
    zIndex:1,
}

/*圆形配置*/
var circleOption = {
    /*圆心*/
    center:null,
    /*设置圆是否允许单击*/
    clickable: true,
    /*设置圆是否可拖拽*/
    draggable: false,
    /*设置圆是否可编辑*/
    editable: false,
    /*填充颜色。所以 CSS3 颜色都可用*/
    fillColor: '#FF0000',
    /*填充透明度，从0.0到1.0*/
    fillOpacity: 0.3,
    /*圆形的Map对象*/
    map:null,
    /*圆半径*/
    radius: 0,
    /*描边颜色，所有CSS3 颜色都可用*/
    strokeColor: "#FAD949",
    /*描边透明度，从0.0到1.0*/
    strokeOpacity: 1,
    /*描边的位置，可用的值：google.maps.StrokePosition.INSIDE,google.maps.StrokePosition.CENTER,google.maps.StrokePosition.OUTSIDE*/
    strokePosition: google.maps.StrokePosition.OUTSIDE,
    /*描边大小，单位:像素*/
    strokeWeight: 2,
    /*是否可见状态*/
    visible: true,
    /*层级*/
    zIndex:1,
}

/*绘制配置*/
var drawOption = {
    /*要绘制的地图对象*/
    map: null,
    /*控制选项*/
    drawingControlOptions: {
        /*当前运行的模式，可用值：'marker', 'polygon', 'polyline','rectangle', 'circle'，'null';*/
        drawingModes: ['marker', 'polygon', 'polyline', 'rectangle', 'circle', 'null'],
    },
    /*是否启用绘画控件*/
    drawingControl:false,
}

/*轨迹配置*/
var trackOption = {
    /*边颜色16进制或rgba*/
    strokeColor: "#FAD949",
    /*边宽度单位px*/
    strokeWeight: 2,
    /*轨迹回放速度单位毫秒*/
    speed: 100,
    /*轨迹*/
    path:[],
}

/*绘制对象数组*/
var arrayOption = {
    markerArray: [],
    polylineArray: [],
    polygonArray: [],
    rectangleArray: [],
    circleArray: [],
}

/*infoWindow配置项*/
var infoWindowOption = {
    //倒三角配置
    arrowColor: 'rgba(3,120,168,0.7)',
    //面板样式配置
    infoClass: 'info',
    //信息窗口dom资源Id
    infoId: 'info',
    //信息窗口数量
    infoArray:[],
}

/*告警动画配置自定义infowindow*/
var overViewOption = {
    className: "pulse",
    alarmArray:[],
}

/*创建文字Lable*/
var lableOption = {
    text: null,
    lat: null,
    lng: null,
}

bigmap.prototype = {
    /*加载地图*/
    init: function (mapOption, myOption, obj) {
        /*自定义地图*/
        function BigeMap() { }
        /*地图下属瓦片大小*/
        BigeMap.prototype.tileSize = new google.maps.Size(256, 256);
        /*地图最大级别*/
        BigeMap.prototype.maxZoom = myOption.maxZoom;
        /*地图最小级别*/
        BigeMap.prototype.minZoom = myOption.minZoom;
        /*地图名称*/
        BigeMap.prototype.name = myOption.name;
        /*自动获取瓦片地图服务*/
        BigeMap.prototype.getTile = function (coord, zoom, ownerDocument) {
            var img = ownerDocument.createElement("img");
            img.style.width = this.tileSize.width + "px";
            img.style.height = this.tileSize.height + "px";
            var strURL = myOption.url + zoom + '/' + coord.x + '/' + coord.y + myOption._suffx;
            img.src = strURL;
            img.onerror = function () {
                this.src = '/images/error.png';
            };
            return img;
        };
        /*实例化地图*/
        var google_map = new BigeMap();
        /*初始化map对象*/
        var map = new google.maps.Map(obj, mapOption);
        /*向map对象中添加两种种地图*/
        map.mapTypes.set('google', google_map);
        /*设置map对象默认显示 google 地图 也就是上面的google_map对象*/
        map.setMapTypeId('google');
        return map;
    },
    /*加点*/
    addMarker: function (markerOption, map) {
        markerOption.map = map;
        var marker = new google.maps.Marker({
            anchorPoint:markerOption.anchorPoint,
            animation: markerOption.animation,
            clickable: markerOption.clickable,
            crossOnDrag: markerOption.crossOnDrag,
            draggable: markerOption.draggable,
            position: markerOption.position,
            map: markerOption.map,
            icon: markerOption.icon,
            label: markerOption.label,
            opacity: markerOption.opacity,
            optimized: markerOption.optimized,
            title: markerOption.title,
            visible: markerOption.visible,
            zIndex: markerOption.zIndex,
        });
        marker.id = markerOption.id;
        return marker;
    },
    /*加线*/
    addPolyline: function (polylineOption, map) {
        polylineOption.map = map;
        var polyline = new google.maps.Polyline({
            clickable: polylineOption.clickable,
            draggable: polylineOption.draggable,
            editable: polylineOption.editable,
            geodesic: polylineOption.geodesic,
            icons: polylineOption.icons,
            map: polylineOption.map,
            path: polylineOption.path,
            strokeColor: polylineOption.strokeColor,
            strokeOpacity: polylineOption.strokeOpacity,
            strokeWeight: polylineOption.strokeWeight,
            visible: polylineOption.visible,
            zIndex: polylineOption.zIndex,
        });
        return polyline;
    },
    /*加多边形*/
    addPolygon: function (polygonOption, map) {
        polygonOption.map = map;
        var polygon = new google.maps.Polygon({
            clickable: polygonOption.clickable,
            draggable: polygonOption.draggable,
            editable: polygonOption.editable,
            fillColor: polygonOption.fillColor,
            fillOpacity: polygonOption.fillOpacity,
            geodesic: polygonOption.geodesic,
            map: polygonOption.map,
            paths: polygonOption.path,
            strokeColor: polygonOption.strokeColor,
            strokeOpacity: polygonOption.strokeOpacity,
            strokePosition: polygonOption.strokePosition,
            strokeWeight: polygonOption.strokeWeight,
            visible: polygonOption.visible,
            zIndex: polygonOption.zIndex,
        });
        return polygon;
    },
    /*加矩形*/
    addRectangle: function (rectangleOption, map) {
        rectangleOption.map=map;
        var rectangle = new google.maps.Rectangle({
            bounds: rectangleOption.bounds,
            clickable: rectangleOption.clickable,
            draggable: rectangleOption.draggable,
            editable: rectangleOption.editable,
            fillColor: rectangleOption.fillColor,
            fillOpacity:rectangleOption.fillOpacity,
            map: rectangleOption.map,
            strokeColor: rectangleOption.strokeColor,
            strokeOpacity: rectangleOption.strokeOpacity,
            strokePosition:rectangleOption.strokePosition,
            strokeWeight: rectangleOption.strokeWeight,
            visible: rectangleOption.visible,
            zIndex: rectangleOption.zIndex,
        });
        return rectangle;
    },
    /*加圆*/
    addCircle: function (circleOption, map) {
        circleOption.map = map;
        var circle = new google.maps.Circle({
            center: circleOption.center,
            clickable: circleOption.clickable,
            draggable: circleOption.draggable,
            editable: circleOption.editable,
            fillColor: circleOption.fillColor,
            fillOpacity:circleOption.fillOpacity,
            map: circleOption.map,
            radius: circleOption.radius,
            strokeColor: circleOption.strokeColor,
            strokeOpacity: circleOption.strokeOpacity,
            strokePosition: circleOption.strokePosition,
            strokeWeight: circleOption.strokeWeight,
            visible: circleOption.visible,
            zIndex: circleOption.zIndex,
        });
        return circle;
    },
    addLable: function (map, lableOption) {
        var lable = new MapLabel({
            map: map,
            text: lableOption.text,
            position: new google.maps.LatLng(lableOption.lat, lableOption.lng),
        });
        return lable;
    },
    //创建info自定义窗体
    addInfo: function (infoWindowOption) {
        var infoWindow = new google.maps.InfoWindow({
            content: document.getElementById(infoWindowOption.infoId),
        });
        return infoWindow;
    },
    //关闭info自定义窗体
    closeInfo: function (infoArray) {
        for (var i = 0; i < infoArray.length; i++) {
            infoArray[i].close();
        }
    },
    //设置点单击监听
    setMarkerClick: function (infoWindow, map, marker, infoWindowOtion) {
        // 监听点点击事件
        google.maps.event.addListener(marker, 'click', function () {
            bigmap.prototype.closeInfo(infoWindowOtion.infoArray);
            infoWindow.open(map, marker);
            var dom = document.getElementById(infoWindowOtion.infoId);
            dom.parentNode.parentNode.parentNode.children[0].children[1].className = infoWindowOtion.infoClass;
            dom.parentNode.parentNode.parentNode.children[0].children[3].style.background = "none";
            dom.parentNode.parentNode.parentNode.children[0].children[2].children[0].children[0].style.background = infoWindowOtion.arrowColor;
            dom.parentNode.parentNode.parentNode.children[0].children[2].children[1].children[0].style.background = infoWindowOtion.arrowColor;
        });
    },
    //自定义OverView告警动画
    setAlarm: function (map, marker) {
        var alarm = new contextMenu({
            map: map,
            position: marker.position,
        });
        alarm.addMenu(marker);
        return alarm;
    },
    //清除告警动画
    clearAlarm: function (overViewOption, index) {
        var dom=document.getElementsByClassName(overViewOption.className);
        for (var i = dom.length; i >= 0; i--) {
            dom[i - 1].remove();
        }
    },
    /*创建绘制*/
    addDraw: function (markerOption, polylineOption, polygonOption, rectangleOption, circleOption, drawOption, map) {
        drawOption.map = map;
        var draw_manager = new google.maps.drawing.DrawingManager({
            map: drawOption.map,
            markerOptions: {
                anchorPoint: markerOption.anchorPoint,
                animation: markerOption.animation,
                clickable: markerOption.clickable,
                crossOnDrag: markerOption.crossOnDrag,
                draggable: markerOption.draggable,
                icon: markerOption.icon,
                label: markerOption.label,
                opacity: markerOption.opacity,
                optimized: markerOption.optimized,
                title: markerOption.title,
                visible: markerOption.visible,
                zIndex: markerOption.zIndex,
            },
            polylineOptions: {
                clickable: polylineOption.clickable,
                draggable: polylineOption.draggable,
                editable: polylineOption.editable,
                geodesic: polylineOption.geodesic,
                icons: polylineOption.icons,
                strokeColor: polylineOption.strokeColor,
                strokeOpacity: polylineOption.strokeOpacity,
                strokeWeight: polylineOption.strokeWeight,
                visible: polylineOption.visible,
                zIndex: polylineOption.zIndex,
            },
            polygonOptions: {
                clickable: polygonOption.clickable,
                draggable: polygonOption.draggable,
                editable: polygonOption.editable,
                fillColor: polygonOption.fillColor,
                fillOpacity: polygonOption.fillOpacity,
                geodesic: polygonOption.geodesic,
                strokeColor: polygonOption.strokeColor,
                strokeOpacity: polygonOption.strokeOpacity,
                strokePosition: polygonOption.strokePosition,
                strokeWeight: polygonOption.strokeWeight,
                visible: polygonOption.visible,
                zIndex: polygonOption.zIndex,
            },
            rectangleOptions: {
                clickable: rectangleOption.clickable,
                draggable: rectangleOption.draggable,
                editable: rectangleOption.editable,
                fillColor: rectangleOption.fillColor,
                fillOpacity: rectangleOption.fillOpacity,
                strokeColor: rectangleOption.strokeColor,
                strokeOpacity: rectangleOption.strokeOpacity,
                strokePosition: rectangleOption.strokePosition,
                strokeWeight: rectangleOption.strokeWeight,
                visible: rectangleOption.visible,
                zIndex: rectangleOption.zIndex,
            },
            circleOptions: {
                clickable: circleOption.clickable,
                draggable: circleOption.draggable,
                editable: circleOption.editable,
                fillColor: circleOption.fillColor,
                fillOpacity: circleOption.fillOpacity,
                strokeColor: circleOption.strokeColor,
                strokeOpacity: circleOption.strokeOpacity,
                strokePosition: circleOption.strokePosition,
                strokeWeight: circleOption.strokeWeight,
                visible: circleOption.visible,
                zIndex: circleOption.zIndex,
            },
            drawingControlOptions:drawOption.drawingControlOptions,
            drawingControl: drawOption.drawingControl,
        });
        return draw_manager;
    },
    //设置绘点
    setDrawMarker: function (draw_manager) {
        draw_manager.setDrawingMode('marker');
    },
    //设置绘线
    setDrawPolyline:function(draw_manager){
        draw_manager.setDrawingMode('polyline');
    },
    //设置绘多边形
    setDrawPolygon:function(draw_manager){
        draw_manager.setDrawingMode('polygon');
    },
    //设置绘矩形
    setDrawRectangle:function(draw_manager){
        draw_manager.setDrawingMode('rectangle');
    },
    //设置绘圆形
    setDrawCricle:function(draw_manager){
        draw_manager.setDrawingMode('circle');
    },
    //设置停止绘制
    setStopDraw:function(draw_manager){
        draw_manager.setDrawingMode(null);
    },
    //设置绘点监听
    setlistenDrawMarker: function (draw_manager, latobj, lngobj) {
        draw_manager.addListener('markercomplete', function (args) {
            bigmap.prototype.clearDraw(arrayOption.markerArray);
            arrayOption.markerArray.push(args);
            var position = args.position;
            var result = {
                position: position,
            };
            latobj.value = result.position.lat();
            lngobj.value = result.position.lng();
        });
    },
    //设置绘线监听
    setlistenDrawPolyline: function (draw_manager, obj) {
        draw_manager.addListener('polylinecomplete', function (args) {
            arrayOption.polylineArray.push(args);
            var result = {
                path: args.getPath().getArray()
            };
            obj.value = JSON.stringify(result);
        });
    },
    //设置绘多边形监听
    setlistenDrawPolygon: function (draw_manager, obj) {
        //获取绘多边形坐标
        draw_manager.addListener('polygoncomplete', function (args) {
            arrayOption.polygonArray.push(args);
            var result = {
                path: args.getPath().getArray()
            };
            obj.value = JSON.stringify(result);
        });
    },
    //设置绘矩形监听
    setlistenDrawRectangle: function (draw_manager, obj) {
        draw_manager.addListener('rectanglecomplete', function (args) {
            arrayOption.rectangleArray.push(args);
            var result = {
                bounds: args.bounds
            };
            obj.value = JSON.stringify(result);
        });
    },
    //设置绘圆监听
    setlistenDrawCircle:function(draw_manager,obj){
        //获取绘圆形坐标
        draw_manager.addListener('circlecomplete', function (args) {
            arrayOption.circleArray.push(args);
            var result = {
                center: args.center,
                radius: args.radius,
            };
            obj.value = JSON.stringify(result);
        });
    },
    /*添加地图点击监听获取经纬度*/
    setMapClick: function (obj, map) {
        map.addListener('click', function (e) {
            var lat = e.latLng.lat();
            var lng = e.latLng.lng();
            var result = {
                position: { lat: lat, lng: lng },
            };
            obj.value = JSON.stringify(result);
        });
    },
    /*轨迹回放*/
    trackPlay: function (trackOption, map) {
        var trackLine = new google.maps.Polyline({
            map: map,
            icons: [
                {
                    icon: {
                        path: 'M -2,0 0,-2 2,0 0,2 z',
                        strokeColor: '#F00',
                        fillColor: '#F00',
                        fillOpacity: 1,
                        scale: 5
                    },
                    offset: '0%'
                }
            ],
            strokeColor: trackOption.strokeColor,
            strokeWeight: trackOption.strokeWeight,
            path: trackOption.path,
        });
        var s = 0;
        var trackInterval = setInterval(function () {
            s = (s + 1) % 100;
            var t = trackLine.get('icons');
            t[0].offset = s + '%';
            trackLine.set('icons', t);
        }, trackOption.speed);
        var result = {
            trackLine: trackLine,
            trackInterval: trackInterval
        }
        return result;
    },
    /*停止轨迹回放*/
    trackStop: function (trackLine, trackInterval) {
        trackLine.setMap(null);
        window.clearInterval(trackInterval);
    },
    /*清除指定对象*/
    clearMap: function (obj) {
        obj.setMap(null);
    },
    /*清除绘制*/
    clearDraw: function (objarray, index) {
        if (index) {
            objarray[index].setMap(null);
        } else {
            for (var i = 0; i < objarray.length; i++) {
                objarray[i].setMap(null);
            }
            objarray.splice(0, objarray.length);
        }
    },
    /*指定地图移动至某个坐标*/
    moveTo: function (map, lng, lat) {
        var gm = new google.maps.LatLng(parseFloat(lat), parseFloat(lng));
        map.panTo(gm, { animate: true, duration: 0.5 });
    },
}

/*自定义infowindow*/
function contextMenu(position) {
    this.div = window.document.createElement('div');
    this.div.className = overViewOption.className;
    this.div.style.display = 'none';
    this.setEvent(position.map);
    this.setValues(position);
}
contextMenu.prototype = new google.maps.OverlayView;
contextMenu.prototype.setEvent = function (map) {
    var _this = this;
};
contextMenu.prototype.onAdd = function () {
    var div = this.div;
    div.style.position = 'absolute';
    var panels = this.getPanes();
    //因为菜单要接收点击事件，所以这里使用floatPane
    panels.floatPane.appendChild(div);
};
contextMenu.prototype.draw = function () {
    var projection = this.getProjection();
    var x_y = projection.fromLatLngToDivPixel(this.position);
    this.div.style.left = (x_y.x - 24) + 'px';
    this.div.style.top = (x_y.y - 24) + 'px';
};
contextMenu.prototype.addMenu = function (overlay) {
    var _this = this;
    _this.show();
};
contextMenu.prototype.show = function () {
    this.div.style.display = '';
};
contextMenu.prototype.hide = function () {
    this.div.style.display = 'none';
};
