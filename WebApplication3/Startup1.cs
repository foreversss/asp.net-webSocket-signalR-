using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(WebApplication3.Startup1))]

namespace WebApplication3
{
    public class Startup1
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR<MyConnection1>("/myconnection");
            // 有关如何配置应用程序的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkID=316888
        }
    }
}
