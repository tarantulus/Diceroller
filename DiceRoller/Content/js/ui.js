$(function () {
    $(".close").click(function () {
        $(this).closest('.sidebar').toggle(200);
    })
})

$(function () {
    $("#dice").click(function () {
        $(".dicewell").toggle(200);
        $(".dicewell").parent().toggle(200);
    })
})

$(function () {
    $("#spreadsheet").click(function () {
        $(".xpwell").toggle(200);
    })
})

$(function () {
    $("#users").click(function () {
        $(".userwell").toggle(200);
    })
})

