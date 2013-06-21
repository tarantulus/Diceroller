if (window.addEventListener) {
    window.addEventListener('load', function () { init(); });
}

var started = false;
var canvas, context;
var lastColor = 'black';
var penDown = false;
var currentTool = "pen";

function init() {

    //set canvas vars
    canvas = $("#sketchPad").get(0);
    context = canvas.getContext('2d');    
    $('#line').bind("click", function () { setTool('line') });
    $('#rectangle').bind("click", function () { setTool('rectangle') });
    $('#eraser').bind("click", function () { setTool('pen'); setColour('#fff') });
    $('#pen').bind("click", function () { setTool('pen') });
    $('#size').change(function () {
        setSize($(this).attr('value'));
    });
    canvas.addEventListener('mousedown', function (e) {
        penDown = true;
        startx = typeof e.offsetX !== 'undefined' ? e.offsetX : e.layerX;
        starty = typeof e.offsetY !== 'undefined' ? e.offsetY : e.layerY;
        canvas.addEventListener('mousemove', onMouseMove, false);
    }, false);
    canvas.addEventListener('mouseup', function (e) { penDown = false; started = false; }, false);

}

function onMouseMove(e) {
    var x, y;

    // Get the mouse position.
    x = typeof e.offsetX !== 'undefined' ? e.offsetX : e.layerX;
    y = typeof e.offsetY !== 'undefined' ? e.offsetY : e.layerY;
    switch(currentTool)
    {
        case "pen":
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
            break;
        case "line":
            if (penDown) {

                context.beginPath();
                context.moveTo(startx, starty);
                context.lineTo(x, y);
            }
            else {
                context.stroke();
                context.closePath();
            }
            break;
        case "rectangle":
            if (penDown) {

                context.beginPath();
                context.moveTo(startx, starty);
                var rectx = Math.min(x, startx);
                var recty = Math.min(y, starty);
                var width = Math.abs(x - startx);
                var height = Math.abs(y - starty);
                context.rect(rectx, recty, width, height);
            }
            else {
                context.stroke();
                context.closePath();
            }
            break;
    }
}

function setColour(color) {
    // Start a new path to begin drawing in a new color.
    context.closePath();
    context.beginPath();

    // Select the new color.
    context.strokeStyle = color.toHex();
}

function setTool(tool) {
    currentTool = tool;
}

function setSize(size) {
    context.lineWidth = size;
}


