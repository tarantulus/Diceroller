using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiceRoller.Helpers
{
    public class HTMLhelper
    {
        public string Roll2HTML(List<List<Int32>> data)
        {
            string html = "";
            int total = 0;
            for(var i=0; i < data[0].Count(); i++){
                total = total + (int)data[0][i];
                html = i==0?html:html + (data[0][i]>=0?"+ ":"- ");
                html = html + Math.Abs(data[0][i]);
                if (data[1][i] > 0)
                {
                    html = html + "<sub>[d" + data[1][i] + "]</sub> ";
                }                
            }
            html = "<strong>" + total + "</strong>" + "&nbsp;=&nbsp;" + html;
            return html;
        }
    }
}