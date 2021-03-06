﻿var helper = {

    prettyPrint: function (data) {
        var html = "";
        if (typeof(data) === "object") {
            var total = 0;
            for (var i = 0; i < data[0].length; i++) {
                total = total + data[0][i];
                html = i == 0 ? html : html + (data[0][i] >= 0 ? "+ " : "- ");
                html = html + Math.abs(data[0][i]);
                if (data[1][i] > 0) {
                    html = html + "<sub>[d" + data[1][i] + "]</sub> ";
                }
            }
            html = "<em>" + total + "</em>" + "&nbsp;=&nbsp;" + html;
        }
        else {
            html = data
        }
        return html
    },


    getNames: function (obj) {
        var out = '';
        obj.sort(function (a, b) { return parseFloat(b.Init) - parseFloat(a.Init) });
        for (var i in obj) {
            out += "<li><p>" + obj[i].Name + ":" + obj[i].Init + "</p></li>";
        }
        return out;
    },

    prune: function (node) {
        chld = $(node).children();
        if (chld.length > 100) {
            $(node).slice(100).remove();
        }
    },

    getQueryString: function (sVar) {
        urlStr = window.location.search.substring(1);
        sv = urlStr.split("&");
        for (i=0;i< sv.length;i++) {
            ft = sv [i].split("=");
            if (ft[0] == sVar) {
                return ft[1];
            }
        }
    }

}