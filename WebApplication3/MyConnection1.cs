using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace WebApplication3
{
    public class MyConnection1 : PersistentConnection
    {
        protected override Task OnConnected(IRequest request, string connectionId)
        {


            return Connection.Send(connectionId, "Welcome!");
        }

        protected override Task OnReceived(IRequest request, string connectionId, string data)
        {
            Debug.WriteLine(data);
            return Connection.Broadcast(data);
        }

        protected override Task OnDisconnected(IRequest request, string connectionId, bool stopCalled)
        {
            Debug.WriteLine("OnDisconnected");
            return base.OnDisconnected(request, connectionId, stopCalled);

        }
    }
}