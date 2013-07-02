$(function () {
    var diceroller = $.connection.diceHub;
    diceroller.client.broadcastMessage = function (label, message) {
        rollerHub.getMessage(label, message, $('#log'));
    };

    diceroller.client.broadcastImg = function (img) {
        sketchHub.getImg(img);
    };

    diceroller.client.clearImg = function () {
        context.clearRect(0, 0, canvas.width, canvas.height);
    };

    diceroller.client.updateUsers = function (users) {
        userHub.setNames(users, $('#users').get(0))
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
        $('#sketchPad').mouseup(function () {
            sketchHub.sendImg();
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