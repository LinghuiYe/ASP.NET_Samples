using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SignalRStockTicker.Startup))]

namespace SignalRStockTicker
{
    //The server needs to know which URL to intercept and direct to SignalR. To do that, add an OWIN startup class
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888

            //Any connection or hub wire up and configuration should go here
            app.MapSignalR();
        }
    }
}
