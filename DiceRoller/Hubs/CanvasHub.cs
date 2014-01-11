using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace DiceRoller.Hubs
{
    public class CanvasHub
    {
        private static CanvasHub _instance;
        public static CanvasHub Instance()
        {
            // Uses lazy initialization.
            // Note: this is not thread safe.
            if (_instance == null)
            {
                _instance = new CanvasHub();
            }

            return _instance;
        }

        IHubContext _context = GlobalHost.ConnectionManager.GetHubContext<DiceHub>();

        public void SendCanvas(object img, string connectionId)
        {   if (img != null)
            _context.Clients.AllExcept(connectionId).broadcastImg(img);
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