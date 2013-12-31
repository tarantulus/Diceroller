$(function () {
    function tiggle(elem) {
        if (elem.siblings().length > 0) {
            elem.toggle(200);
            canvasResize(elem);
        }
        else {
            elem.parent().toggle(200);
        }
    }

    function canvasResize(e) {
        if ($(e).hasClass('right')) {
            $("#leftArea").toggleClass("span3")
            $("#leftArea").toggleClass("span4")
            $("#mainArea").toggleClass("span6")
            $("#mainArea").toggleClass("span8")
            sketchpad.resize();
        }
    }

    $(".close").click(function () {
        var sidebar = $(this).closest('.sidebar');
        sidebar.toggle(200);
        canvasResize(sidebar);
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

