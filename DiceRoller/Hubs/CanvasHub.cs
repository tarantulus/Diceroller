using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace DiceRoller.Hubs
{
    public class CanvasHub
    {
        IHubContext _context = GlobalHost.ConnectionManager.GetHubContext<DiceHub>();

        public void SendCanvas(string img)
        {            
            _context.Clients.All.broadcastImg(img);
        }

        public void ClearImg()
        {
            _context.Clients.All.clearImg();
        }

        public void userIsDrawing(string userName)
        {
            _context.Clients.All.userIsDrawing(userName);
        }

        public void userStoppedDrawing(string userName)
        {            
            _context.Clients.All.userStoppedDrawing(userName);
        }
    }
}