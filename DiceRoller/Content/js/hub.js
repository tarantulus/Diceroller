$(function () {
    var chat = $.connection.diceHub;
    chat.client.broadcastMessage = function (name, message) {
        var li = document.createElement('li');
        li.innerHTML = '<em>' + name
            + '</em>:&nbsp;&nbsp;' +
            prettyPrint(message)
        prune($('#log'));
        $('#log').prepend(li)
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

    chat.client.updateUsers = function (users) {
        $('#users').get(0).innerHTML = getNames(users);
    };

    // Get the user name and store it to prepend to messages.
    user = prompt("Please enter your name");
    $('#user').append(user);
    $('#displayname').val(user);
    // Set initial focus to message input box.  
    $('#message').focus();
    // Start the connection.
    $.connection.hub.start().done(function () {
        chat.server.setName(user);
        chat.server.getLog();
        $('#sketchPad').mouseup(function () {
            var img = canvas.toDataURL("image/png");
            chat.server.sendCanvas(img);
        });

        $('#clear').bind("click", function () {
            chat.server.clearImg();
        });

        $('#setInit').click(function () {
            chat.server.setInit($('#diceroll').val());
        });

        $('#sendmessage').click(function () {
            // Call the Send method on the hub.
            if (!$('#diceroll').val()) {
                roll = "";
                numdice = "";
                chat.server.send($('#displayname').val(), $('#message').val());
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
            $('#message').val('');
        });
    });

    function prettyPrint(data) {
        var html = "";
        if (data instanceof Array) {
            var total = 0;
            for (var i = 0; i < data[0].length; i++) {
                total = total + data[0][i];
                html = i == 0 ? html : html + (data[0][i] >= 0 ? "+ " : "- ");
                html = html + Math.abs(data[0][i]);
                if (data[1][i] > 0) {
                    html = html + "<sub>[d" + data[1][i] + "]</sub> ";
                }
            }
            html = "<em>" + total + "</em>" + "&nbsp;=&nbsp;" + html;
        }
        else {
            html = data
        }
        return html;
    }

    function getNames(obj) {
        var out = '';
        obj.sort(function (a, b) { return parseFloat(b.Init) - parseFloat(a.Init) });
        for (var i in obj) {
            out += "<li>" + obj[i].Name + ":"+obj[i].Init+"</li>";
        }
        return out;
    }

    function prune(node) {
        chld = $(node).children();
        if (chld.length > 100) {
            $(node).empty();
        }
    }
});