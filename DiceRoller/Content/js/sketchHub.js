$(function () {
    sketchHub = {
        init: function (hub, context, canvas) {
            this.hub = hub;
            this.context = context;
            this.canvas = canvas;
        },

        sendImg: function () {
            this.hub.server.sendCanvas(sketchpad.json());
        },

        clearImg: function () {
            this.hub.server.clearImg();
        },

        doClear: function () {
            context.fillStyle = "#fff";
            context.fillRect(0, 0, canvas.width, canvas.height);
        },

        getImg: function (img) {            
            sketchpad.jsonLoad(img);
        },

        notifyDraw: function () {
            this.hub.server.userIsDrawing()
        },

        startDraw: function (elem, user) {
            var li = document.createElement('li');
            li.innerHTML = user + " is drawing";
            li.className = user;
            elem.append(li)
        },

        notifyEndDraw: function () {
            this.hub.server.userStoppedDrawing()
        },

        EndDraw: function (user) {
            $('.' + user).remove();
        }

    }
}
)
