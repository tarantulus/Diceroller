$(function () {
    var diceroller = $.connection.diceHub;
    diceroller.client.broadcastMessage = function (label, message) {
        rollerHub.getMessage(label, message, $('#log'));
    };

    diceroller.client.broadcastImg = function (img) {
        sketchHub.getImg(img);
    };

    diceroller.client.clearImg = function () {
        sketchHub.doClear();
    };

    diceroller.client.updateUsers = function (users) {
        userHub.setNames(users, $('#users').get(0))
    };

    diceroller.client.userIsDrawing = function (user) {
        sketchHub.startDraw($('#notify'),user);
    };

    diceroller.client.userStoppedDrawing = function (user) {
        sketchHub.EndDraw(user);
    };
    // Get the user name and store it to prepend to messages.
    user = prompt("Please enter your name");
    $('#user').append(user);
    $('#displayname').val(user);
    // Set initial focus to message input box.  
    $('#message').focus();
    // Start the connection.
    $.connection.hub.start().done(function () {
        sketchHub.init(diceroller);
        rollerHub.init(diceroller);
        userHub.init(diceroller);
        diceroller.server.setName(user);
        diceroller.server.getLog();

        $('#message').val('');
        $('#sketchPad').mousedown(function () {
            sketchHub.notifyDraw();
        });
        $('#sketchPad').mouseup(function () {
            sketchHub.sendImg();
            sketchHub.notifyEndDraw();
        });

        $('#clear').bind("click", function () {
            sketchHub.clearImg();
        });

        $('#setInit').click(function () {
            diceroller.server.setInit($('#diceroll').val());
        });

        $('#sendmessage').click(function () {
            var request = {
                roll: $('#diceroll').val(),
                message: $('#message').val(),
                numrolls: $('#numdice').val(),
                user: $('#displayname').val()
            }
            rollerHub.sendMessage(request);
        });
    });    
});