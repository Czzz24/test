/**
 * @license
 *
 * Copyright 2011 Google Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

/**
 * @fileoverview Map Label.
 *
 * @author Luke Mahe (lukem@google.com),
 *         Chris Broadfoot (cbro@google.com)
 */

/**
 * Creates a new Map Label
 * @constructor
 * @extends google.maps.OverlayView
 * @param {Object.<string, *>=} opt_options Optional properties to set.
 */

var MapText = function (opt_options) {
    this.set('zIndex', 1e3);
    this.setValues(opt_options);
    this.iniDiv();
    this.iniEvent();
};

MapText.prototype = new google.maps.OverlayView;

MapText.prototype.getValue=function(){
    return this.value;
};

MapText.prototype.iniDiv = function () {
    this.container = this.getDom('div');
    this.container.className = 'text_test';
    this.setStyle(this.container, {position: 'absolute'});
    this.delBtn = this.getDom('a');
    var  del_img= new Image();
    this.setStyle(del_img,{width:'25px',height:'25px'});
    del_img.src=this.del_src?this.del_src:'./images/del.png';
    this.delBtn.appendChild(del_img);
    this.editBtn = this.getDom('a');
    var edit_img=new Image();
    this.setStyle(edit_img,{width:'25px',height:'25px'});
    edit_img.src=this.edit_src?this.edit_src:'./images/edit.png';
    this.setStyle(this.editBtn, {display: 'inline-block'});
    this.editBtn.appendChild(edit_img);
    this.saveBtn = this.getDom('a');
    var save_img=new Image();
    this.setStyle(save_img,{width:'25px',height:'25px'});
    save_img.src=this.save_src?this.save_src:'./images/save.png';
    this.setStyle(this.saveBtn, {display: 'none'});
    this.saveBtn.appendChild(save_img);
    this.textareap=this.getDom('div');
    this.setStyle(this.textareap,{width:'200px',display: 'none',height:'100px',overflow:'hidden'});
    this.textarea = document.createElement('textarea');
    this.showDiv = this.getDom('div');
    this.setStyle(this.showDiv, {
        display: 'inline-block',
        'backgroundColor': '#fff',
        'vertical-align': 'bottom',
        width: '200px',
        wordWrap: 'break-word',
        'min-height':'25px',
    });

    this.textareap.appendChild(this.textarea);
    this.showDiv.innerHTML=this.value?this.value:'';
    this.textarea.value=this.value?this.value:'';
    this.textarea.addEventListener('mousewheel',function(e){
            e.stopPropagation();
    },false);
    this.setStyle(this.textarea, {resize: 'none',border:'none','vertical-align': "bottom", width: '200px', height: '100px'});
    this.container.appendChild(this.textareap);
    this.container.appendChild(this.showDiv);
    this.container.appendChild(this.editBtn);
    this.container.appendChild(this.saveBtn);
    this.container.appendChild(this.delBtn);
};

MapText.prototype.iniEvent = function () {
    var _this = this;
    this.saveBtn.addEventListener('click', function () {
        _this.setEditable(false);
    });

    this.editBtn.addEventListener('click', function () {
        _this.setEditable(true);
    });

    this.delBtn.addEventListener('click', function () {
        _this.setMap(null);
        _this.fireEvent(_this.container, 'delete');
    });
};

MapText.prototype.fireEvent = function (element, event) {
    if (document.createEventObject) {
        // IE浏览器支持fireEvent方法
        var evt = document.createEventObject();
        return element.fireEvent('on' + event, evt);
    } else {
        // 其他标准浏览器使用dispatchEvent方法
        var evt = document.createEvent('HTMLEvents');
        evt.obj=this;
        evt.initEvent(event, true, true);
        return !element.dispatchEvent(evt);
    }
};

MapText.prototype.setEditable = function (t) {
    if (t) {
        this.textareap.style.display = 'inline-block';
        this.showDiv.style.display = 'none';
        this.editBtn.style.display = 'none';
        this.saveBtn.style.display = 'inline-block';
    } else {
        var al = this.textarea.value;
        this.showDiv.innerHTML = al;
        this.value = al;
        this.textareap.style.display = 'none';
        this.showDiv.style.display = 'inline-block';
        this.saveBtn.style.display = 'none';
        this.editBtn.style.display = 'inline-block';
        this.fireEvent(this.container, 'update');
    }
};

MapText.prototype.addListener = function (type, fn) {
    fn=fn.bind(this);
    this.container.addEventListener(type, fn);
};
MapText.prototype.setStyle = function (dom, style) {
    for (var k in style) {
        dom.style[k] = style[k];
    }
};

MapText.prototype.getDom = function (type) {
    var dom = document.createElement(type);
    return dom;
};
/** @inheritDoc */
MapText.prototype.changed = function (prop) {
    switch (prop) {
        case 'maxZoom':
        case 'minZoom':
        case 'position':
            return this.draw();
    }
};

/**
 * Draws the label to the canvas 2d context.
 * @private
 */


/**
 * @inheritDoc
 */
MapText.prototype.onAdd = function () {
    var panes = this.getPanes();
    if (panes) {
        panes.floatPane.appendChild(this.container);
    }
};

MapText.prototype['onAdd'] = MapText.prototype.onAdd;
/**
 * @inheritDoc
 */
MapText.prototype.draw = function () {
    var projection = this.getProjection();
    if (!projection) {
        // The map projection is not ready yet so do nothing
        return;
    }

    if (!this.container) {
        // onAdd has not been called yet.
        return;
    }

    var latLng = /** @type {google.maps.LatLng} */ (this.get('position'));
    if (!latLng) {
        return;
    }
    var pos = projection.fromLatLngToDivPixel(latLng);
    var style = this.container.style;
    style['top'] = pos.y + 'px';
    style['left'] = pos.x + 'px';

    style['visibility'] = this.getVisible_();
};
MapText.prototype['draw'] = MapText.prototype.draw;

/**
 * Get the visibility of the label.
 * @private
 * @return {string} blank string if visible, 'hidden' if invisible.
 */
MapText.prototype.getVisible_ = function () {
    var minZoom = /** @type number */(this.get('minZoom'));
    var maxZoom = /** @type number */(this.get('maxZoom'));

    if (minZoom === undefined && maxZoom === undefined) {
        return '';
    }

    var map = this.getMap();
    if (!map) {
        return '';
    }

    var mapZoom = map.getZoom();
    if (mapZoom < minZoom || mapZoom > maxZoom) {
        return 'hidden';
    }
    return '';
};

/**
 * @inheritDoc
 */
MapText.prototype.onRemove = function () {
    var canvas = this.container;
    if (canvas && canvas.parentNode) {
        canvas.parentNode.removeChild(canvas);
    }
};
MapText.prototype['onRemove'] = MapText.prototype.onRemove;
