﻿$(function () {
    // Declare a proxy to reference the hub. 
    var chat = $.connection.diceHub;
    // Create a function that the hub can call to broadcast messages.
    chat.client.broadcastMessage = function (name, message) {
        // Add the message to the page. 
        $('#log').prepend('<li><strong>' + name
            + '</strong>:&nbsp;&nbsp;' + message + '</li>');
    };

    chat.client.broadcastImg = function (img) {
        var imageObj = new Image();
        imageObj.src = img;
        imageObj.onload = function () {
            context.drawImage(this, 0, 0);
        };
    };

    chat.client.clearImg = function () {
        context.clearRect(0, 0, canvas.width, canvas.height);
    };

    // Get the user name and store it to prepend to messages.
    $('#displayname').val(prompt("Please enter your name"));
    // Set initial focus to message input box.  
    $('#message').focus();
    // Start the connection.
    $.connection.hub.start().done(function () {
        chat.server.getLog();
        $('#sketchPad').mouseup(function () {
            var img = canvas.toDataURL("image/png");
            chat.server.sendCanvas(img);
        });
        $('#clear').bind("click", function () {
            chat.server.clearImg();
        });
        $('#sendmessage').click(function () {
            // Call the Send method on the hub.
            if (!$('#diceroll').val()) {
                roll = 0;
                numdice = 0
            }
            else if (!$('#numdice').val()) {
                roll = $('#diceroll').val()
                numdice = 1;
            }
            else {
                roll = $('#diceroll').val()
                numdice = $('#numdice').val()
            }
            chat.server.send($('#displayname').val(), $('#message').val(), roll, numdice);
            // Clear text box and reset focus for next comment. 
            $('#message').val('').focus();
        });
    });
});