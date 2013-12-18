$(function () {
    $("#colourPicker").spectrum({
        change: setColour,
        clickoutFiresChange: true
    })
    $('#size').change(function () {
        setSize($(this).val());
    });
    function setSize(size) {
        sketchpad.setLineSize(size);
    }
    function setColour(color) {
        // Select the new color.
        sketchpad.setLineColor('#' + color.toHex());
    }
})