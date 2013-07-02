var rollerHub = {

    init: function (hub) {
        this.hub = hub;
    },

    sendMessage: function (request) {
        if (!request.roll) {
            roll = "";
            numdice = "";
            this.hub.server.send(request.user, request.message);
        }
        else if (!request.numrolls) {
            roll = request.roll
            numdice = 1;
        }
        else {
            roll = request.roll
            numdice = request.numrolls
        }
        this.hub.server.send(request.user, request.message, roll, numdice);        
    },

    getMessage: function (name, message, log) {
        var li = document.createElement('li');
        li.innerHTML = '<em>' + name
            + '</em>:&nbsp;&nbsp;' +
            prettyPrint(message)
        prune(log);
        log.prepend(li)
    }

}