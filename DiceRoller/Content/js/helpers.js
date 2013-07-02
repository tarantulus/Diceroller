var helper = {

    prettyPrint: function (data) {
        var html = "";
        if (data instanceof Array) {
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
            out += "<li>" + obj[i].Name + ":" + obj[i].Init + "</li>";
        }
        return out;
    },

    prune: function (node) {
        chld = $(node).children();
        if (chld.length > 100) {
            $(node).empty();
        }
    }

}