var diceClient = {

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
        var em = document.createElement('em');
        em.innerText = name;
        li.appendChild(em);
        li.innerHTML = li.innerHTML + ": " + message;
        log.prepend(li);

        helper.prune(log);


        if (name == this.user) {
            log.prepend('<hr />')
        }

    },

    getDice: function (name, message, log) {
        var li = document.createElement('li');
        var em = document.createElement('em');        
        em.innerText = name;
        li.appendChild(em);
        li.innerHTML = helper.prettyPrint(message);        
        log.prepend(li);
        
        helper.prune(log);

        
        if (name == this.user) {
            log.prepend('<hr />')
        }

    }

}