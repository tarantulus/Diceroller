$(function () {
    function tiggle(elem) {
        if (elem.siblings().length > 0) {
            elem.toggle(200);
        }
        else {
            elem.parent().toggle(200);
        }
    }

    $(".close").click(function () {
        $(this).closest('.sidebar').toggle(200);
    })


    $("#dice").click(function () {
        tiggle($(".dicewell"));
    })


    $("#spreadsheet").click(function () {
        tiggle($(".xpwell"));
    })


    $("#users").click(function () {
        tiggle($(".userwell"));
    })

    $("#dice").click(function () {
        tiggle($("#dialog"));
    });

    $("#search").keydown(function (e) {
            if (e.which == 27) {
                $("#dialog").dialog("close");
            }
    });

    $(".gsc-input").blur(function () {
        $("#dialog").focus();

    })
})

