$(function () {
    sketchHub = {
        init: function (hub, context, canvas) {
            this.hub = hub;
            this.context = context;
            this.canvas = canvas;
        },

        sendImg: function () {
            var img = this.canvas.toDataURL("image/jpeg", 0.5);
            var imgobj = {
                image: img,
                w: this.canvas.width,
                h: this.canvas.height
            }
            this.hub.server.sendCanvas(imgobj);
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
                imageObj.src = img.image;
                imageObj.onload = function () {
                    sketchHub.context.scale(sketchHub.canvas.width / img.w, sketchHub.canvas.height / img.h);
                    sketchHub.context.drawImage(imageObj, 0, 0);
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
}
)
