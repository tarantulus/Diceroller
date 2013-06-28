if (window.addEventListener) {
    window.addEventListener('load', function () { init(); });
}

var started = false;
var canvas, context;
var lastColor = 'black';
var penDown = false;
var currentTool = "pen";
var reader = new FileReader();

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
    canvas.addEventListener('mouseout', function (e) {
        context.closePath();
        context.moveTo(0, 0);
        context.lineTo(0,0);
        context.rect(0, 0, 0, 0);
    }, false);
    canvas.addEventListener('dragover', function (e) { e.preventDefault(); });
    canvas.addEventListener('drop', function (e) { dropImage(e); }, false);

}

function onMouseMove(e) {
    var x, y;

    // Get the mouse position.
    x = typeof e.offsetX !== 'undefined' ? e.offsetX : e.layerX - e.currentTarget.offsetLeft;
    y = typeof e.offsetY !== 'undefined' ? e.offsetY : e.layerY - e.currentTarget.offsetTop;
    switch(currentTool)
    {
        case "pen":
            if (penDown) {

                if (!started) {
                    started = true;
                    context.closePath();
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
                context.closePath();
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
                context.closePath();
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
    context.strokeStyle = "#"+color.toHex();
}

function setTool(tool) {
    currentTool = tool;
}

function setSize(size) {
    context.lineWidth = size;
}

function dropImage(e) {
    if (e.preventDefault) e.preventDefault();
    e.stopPropagation();
    var files = e.dataTransfer.files; // FileList object
    var reader = new FileReader();
    var f = files[0];
    // Closure to capture the file information.
    reader.onload = (function (theFile) {
        return function (evt) {
            // Render thumbnail.
            var imageObj = new Image();
            imageObj.src = evt.target.result;
            imageObj.onload = function () {
                context.drawImage(this, 0, 0, canvas.width, canvas.height);
            };
        };
    })(f);

    // Read in the image file as a data URL.
    reader.readAsDataURL(f);
}


