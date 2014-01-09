$(function () {
    var diceHub = $.connection.diceHub;
    diceHub.client.broadcastMessage = function (label, message) {
        diceClient.getMessage(label, message, $('#log'));
    };
    diceHub.client.broadcastDice = function (label, dice) {
        diceClient.getDice(label, dice, $('#log'));
    };

    diceHub.client.broadcastImg = function (img) {
        sketchClient.getImg(img);
    };

    diceHub.client.updateUsers = function (users) {
        userClient.setNames(users, $('#userList').get(0))
    };

    diceHub.client.userIsDrawing = function (user) {
        sketchClient.startDraw($('#notify'),user);
    };

    diceHub.client.userStoppedDrawing = function (user) {
        sketchClient.EndDraw(user);
    };
    // Start the connection.
    $.connection.hub.start().done(function () {
        var chatHub = $.connection.chatHub;
        var userHub = $.connection.userHub;
        var canvasHub = $.connection.canvasHub;
        canvas = $("#sketchPad").get(0);
        context = canvas.getContext('2d');
        sketchClient.init(canvasHub, context, canvas);
        diceClient.init(diceHub);
        userClient.init(diceClient);
        user = "anon";
        diceHub.state.userName = user;
        $('#user').empty().append("not logged in");
        $('#displayname').val(user);
        // Set initial focus to message input box.  
        $('#message').focus();
        userHub.server.setName(user);
        diceHub.server.getLog();

        $('#message').val('');
        $('#sketchPad').bind("mousedown",(function () {
            sketchClient.notifyDraw();
        }));
        $('#sketchPad').bind("mouseup",(function () {
            sketchClient.sendImg();
            sketchClient.notifyEndDraw();
        }));

        $('#clear').bind("click", function () {
            sketchClient.clearImg();
        });

        $('#setInit').click(function () {
            diceHub.server.setInit($('#diceroll').val());
        });

        $('#userlogin').blur(function () {
            user = $('#userlogin').val();
            diceHub.state.userName = user;
            $('#user').empty().append("Logged in as " + user);
            $('#displayname').val(user);
            // Set initial focus to message input box.  
            $('#message').focus();
            userHub.server.setName(user);
            diceHub.server.getLog();
            $('#userlogin').hide();
        });

        $('#sendmessage').click(function () {
            var request = {
                roll: $('#diceroll').val(),
                message: $('#message').val(),
                numrolls: $('#numdice').val(),
                user: $('#displayname').val()
            }
            diceClient.sendMessage(request);
        });
        $('#sendhit').click(function () {
            var request = {
                roll: $('#diceroll').val(),
                message: "To Hit: " + $('#message').val(),
                numrolls: $('#numdice').val(),
                user: $('#displayname').val()
            }
            diceClient.sendMessage(request);
        });
        $('#senddmg').click(function () {
            var request = {
                roll: $('#diceroll').val(),
                message: "Damage: " + $('#message').val(),
                numrolls: $('#numdice').val(),
                user: $('#displayname').val()
            }
            diceClient.sendMessage(request);
        });
        $('#makeRoom').click(function () {
            diceHub.server.createRoom("dave", "");
        });
    });    
});