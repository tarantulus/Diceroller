using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace DiceRoller.Hubs
{
    public class CanvasHub:Hub
    {
        public void SendCanvas(object img)
        {   if (img != null)         
            Clients.All.broadcastImg(img);
        }

        public void ClearImg()
        {
            Clients.All.clearImg();
        }

        public void userIsDrawing(string userName)
        {
            Clients.All.userIsDrawing(userName);
        }

        public void userStoppedDrawing(string userName)
        {            
            Clients.All.userStoppedDrawing(userName);
        }
    }
}