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
    canvas.addEventListener('click', onClick, false);
    canvas.width = window.innerWidth - 75;
    canvas.height = window.innerHeight - 75;

    canvas.addEventListener('mousedown', function (e) { penDown = true; }, false);
    canvas.addEventListener('mouseup', function (e) { penDown = false; started = false; }, false);

}

function onMouseMove(ev) {
    var x, y;

    // Get the mouse position.
    if (ev.layerX >= 0) {
        x = ev.layerX;
        y = ev.layerY;
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

    $('#stats').text(x + ', ' + y);
}

function onClick(e) {
}

function onColorClick(color) {
    // Start a new path to begin drawing in a new color.
    context.closePath();
    context.beginPath();

    // Select the new color.
    context.strokeStyle = color.toHex();
}


