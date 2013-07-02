var sketchHub = {

    init: function (hub) {
        this.hub = hub;
    },

    sendImg: function () {
        var img = canvas.toDataURL("image/jpeg",0.2);
        this.hub.server.sendCanvas(img);
    },

    clearImg: function () {
        this.hub.server.clearImg();
    },

    getImg: function (img) {
        var imageObj = new Image();
        imageObj.src = img;
        imageObj.onload = function () {
            context.drawImage(this, 0, 0);
        };
    }



}