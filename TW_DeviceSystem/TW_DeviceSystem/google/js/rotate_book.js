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

function RotateMarker(opt_options) {
  this.set('zIndex', 1e3);
    var canvas = this.canvas_ = document.createElement('div');

    var style = canvas.style;
    this.style=style;
    style.position = 'absolute';
  this.setValues(opt_options);
    canvas.appendChild(this.img);
}

RotateMarker.prototype = new google.maps.OverlayView;

window['RotateMarker'] = RotateMarker;
/** @inheritDoc */
RotateMarker.prototype.changed = function(prop) {
  switch (prop) {
    // case 'fontFamily':
    // case 'fontSize':
    // case 'fontColor':
    // case 'strokeWeight':
    // case 'strokeColor':
    // case 'align':
    // case 'text':
    //   return this.drawCanvas_();
    case 'maxZoom':
    case 'minZoom':
    case 'position':
      return this.draw();
  }
};

RotateMarker.prototype.setRotate=function (rotate) {
    this.style.transform='rotate('+rotate+'deg)';
};

/**
 * Draws the label to the canvas 2d context.
 * @private
 */
RotateMarker.prototype.drawCanvas_ = function() {
  var canvas = this.canvas_;
  if (!canvas) return;
  // var style = canvas.style;
  // style.zIndex = /** @type number */(this.get('zIndex'));
  //   this.style=style;
  //   var ctx = canvas.getContext('2d');
  // ctx.clearRect(0, 0, canvas.width, canvas.height);
  // ctx.strokeStyle = this.get('strokeColor');
  // ctx.fillStyle = this.get('fontColor');
  // ctx.font = this.get('fontSize') + 'px ' + this.get('fontFamily');
  //
  // var strokeWeight = Number(this.get('strokeWeight'));
  //
  // var text = this.get('text');
  // if (text) {
  //   if (strokeWeight) {
  //     ctx.lineWidth = strokeWeight;
  //     ctx.strokeText(text, strokeWeight, strokeWeight);
  //   }
  //
  //   ctx.fillText(text, strokeWeight, strokeWeight);
  //
  //   var textMeasure = ctx.measureText(text);
  //   var textWidth = textMeasure.width + strokeWeight;
  //   style.marginLeft = this.getMarginLeft_(textWidth) + 'px';
  //   // Bring actual text top in line with desired latitude.
  //   // Cheaper than calculating height of text.
  //   style.marginTop = '-0.4em';
  // }
};

/**
 * @inheritDoc
 */
RotateMarker.prototype.onAdd = function() {
  this.drawCanvas_();
  var panes = this.getPanes();
  if (panes) {
    panes.floatPane.appendChild(this.canvas_);
  }
};
RotateMarker.prototype['onAdd'] = RotateMarker.prototype.onAdd;

/**
 * Gets the appropriate margin-left for the canvas.
 * @private
 * @param {number} textWidth  the width of the text, in pixels.
 * @return {number} the margin-left, in pixels.
 */
RotateMarker.prototype.getMarginLeft_ = function(textWidth) {
  switch (this.get('align')) {
    case 'left':
      return 0;
    case 'right':
      return -textWidth;
  }
  return textWidth / -2;
};

/**
 * @inheritDoc
 */
RotateMarker.prototype.draw = function() {
  var projection = this.getProjection();

  if (!projection) {
    // The map projection is not ready yet so do nothing
    return;
  }

  if (!this.canvas_) {
    // onAdd has not been called yet.
    return;
  }

  var latLng = /** @type {google.maps.LatLng} */ (this.get('position'));
  if (!latLng) {
    return;
  }
  var pos = projection.fromLatLngToDivPixel(latLng);

  var style = this.canvas_.style;
  if (!this.offset){
    this.offset={x:0,y:0};
  }

  style['top'] = pos.y -this.offset.x+ 'px';
  style['left'] = pos.x -this.offset.y + 'px';

  style['visibility'] = this.getVisible_();
};
RotateMarker.prototype['draw'] = RotateMarker.prototype.draw;

/**
 * Get the visibility of the label.
 * @private
 * @return {string} blank string if visible, 'hidden' if invisible.
 */
RotateMarker.prototype.getVisible_ = function() {
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
RotateMarker.prototype.onRemove = function() {
  var canvas = this.canvas_;
  if (canvas && canvas.parentNode) {
    canvas.parentNode.removeChild(canvas);
  }
};
RotateMarker.prototype['onRemove'] = RotateMarker.prototype.onRemove;
