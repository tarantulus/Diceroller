var compressor = {

    compress: function (img) {
        c = document.createElement('canvas');
        c.width = img.naturalWidth;
        c.height = img.naturalHeight;
        var ctx = c.getContext("2d").drawImage(img, 0, 0);
        var newImageData = c.toDataURL("image/jpeg", 0.1);
        var result_image_obj = new Image();
        result_image_obj.src = newImageData;
        return result_image_obj;
    }
}