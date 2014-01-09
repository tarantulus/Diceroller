using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DiceRoller.Annotations;
using Microsoft.AspNet.SignalR;

namespace DiceRoller.Hubs
{
    [UsedImplicitly]
    public class CanvasHub:Hub
    {
        private readonly static Lazy<CanvasHub> _instance = new Lazy<CanvasHub>(
            () => new CanvasHub(GlobalHost.ConnectionManager.GetHubContext<CanvasHub>()));

        private IHubContext _context;
        public static CanvasHub Instance { get { return _instance.Value; } }
        private CanvasHub(IHubContext context)
        {
            _context = context;
        }
        public CanvasHub()
        {
        }

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