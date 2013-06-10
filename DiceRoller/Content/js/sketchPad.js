if (window.addEventListener) {
    window.addEventListener('load', function () { init(); });
}

var started = false;
var canvas, context;
var lastColor = 'black';
var penDown = false;

function init() {

    //set canvas vars
    canvas = $("#sketchPad").get(0);
    context = canvas.getContext('2d');
    canvas.addEventListener('mousemove', onMouseMove, false);
    canvas.addEventListener('mousedown', function (e) { penDown = true; }, false);
    canvas.addEventListener('mouseup', function (e) { penDown = false; started = false; }, false);

}

function onMouseMove(e) {
    var x, y;

    // Get the mouse position.
    if (e.offsetX) {
        x = e.offsetX;
        y = e.offsetY;
    }
    else if (e.layerX) {
        if (e.layerX >= 0) {
            x = e.layerX;
            y = e.layerY;
        }
    }
    if (penDown) {

        if (!started) {
            started = true;

            context.beginPath();
            context.moveTo(x, y);
        }
        else {
            context.lineTo(x, y);
            context.stroke();
        }

    }
}

function onColorClick(color) {
    // Start a new path to begin drawing in a new color.
    context.closePath();
    context.beginPath();

    // Select the new color.
    context.strokeStyle = color.toHex();
}


