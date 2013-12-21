﻿$(function () {
    var diceroller = $.connection.diceHub;
    diceroller.client.broadcastMessage = function (label, message) {
        rollerHub.getMessage(label, message, $('#log'));
    };

    diceroller.client.broadcastImg = function (img) {
        sketchHub.getImg(img);
    };

    diceroller.client.updateUsers = function (users) {
        userHub.setNames(users, $('#userList').get(0))
    };

    diceroller.client.userIsDrawing = function (user) {
        sketchHub.startDraw($('#notify'),user);
    };

    diceroller.client.userStoppedDrawing = function (user) {
        sketchHub.EndDraw(user);
    };
    // Start the connection.
    $.connection.hub.start().done(function () {
        canvas = $("#sketchPad").get(0);
        context = canvas.getContext('2d');
        sketchHub.init(diceroller, context, canvas);
        rollerHub.init(diceroller);
        userHub.init(diceroller);
        // Get the user name and store it to prepend to messages.
        //user = prompt("Please enter your name");
        user = "anon";
        diceroller.state.userName = user;
        $('#user').empty().append("not logged in");
        $('#displayname').val(user);
        // Set initial focus to message input box.  
        $('#message').focus();
        diceroller.server.setName(user);
        diceroller.server.getLog();

        $('#message').val('');
        $('#sketchPad').bind("mousedown",(function () {
            sketchHub.notifyDraw();
        }));
        $('#sketchPad').bind("mouseup",(function () {
            sketchHub.sendImg();
            sketchHub.notifyEndDraw();
        }));

        $('#clear').bind("click", function () {
            sketchHub.clearImg();
        });

        $('#setInit').click(function () {
            diceroller.server.setInit($('#diceroll').val());
        });

        $('#userlogin').blur(function () {
            user = $('#userlogin').val();
            diceroller.state.userName = user;
            $('#user').empty().append("Logged in as " + user);
            $('#displayname').val(user);
            // Set initial focus to message input box.  
            $('#message').focus();
            diceroller.server.setName(user);
            diceroller.server.getLog();
            $('#userlogin').hide();
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
        $('#makeRoom').click(function () {
            diceroller.server.createRoom("dave", "");
        });
    });    
});