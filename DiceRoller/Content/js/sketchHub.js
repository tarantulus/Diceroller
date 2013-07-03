var sketchHub = {

    init: function (hub) {
        this.hub = hub;
    },

    sendImg: function () {
        var img = canvas.toDataURL("image/jpeg",0.5);
        this.hub.server.sendCanvas(img);
    },

    clearImg: function () {
        this.hub.server.clearImg();
    },

    doClear: function () {
        context.fillStyle = "#fff";
        context.fillRect(0, 0, canvas.width, canvas.height);
    },

    getImg: function (img) {
        var imageObj = new Image();
        imageObj.src = img;
        imageObj.onload = function () {
            context.drawImage(this, 0, 0);
        };
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