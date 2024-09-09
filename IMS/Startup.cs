using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;
using IMS.Controllers;
using IMS;
using System.Configuration;
 

 
[assembly: OwinStartup(typeof(IMS.Startup))]
namespace IMS
{
    public class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            //Enables SignalR
            app.MapSignalR();

        }
    }
}