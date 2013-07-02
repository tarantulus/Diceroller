var rollerHub = {

    init: function (hub) {
        this.hub = hub;
        this.user = "";
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
        this.user = request.user;
    },

    getMessage: function (name, message, log) {
        var li = document.createElement('li');
        li.innerHTML ='<em>' + name
            + '</em>:&nbsp;&nbsp;' +
            helper.prettyPrint(message)
        helper.prune(log);
        log.prepend(li)
        if (name == this.user) {
            log.prepend('<hr />')
        }
        
    }

}