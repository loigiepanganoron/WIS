using System;
using System.Data;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using System.Configuration;
using System.IO;
using System.Net;
using Kendo.Mvc.UI;
using System.Web.Script.Serialization;
using System.Web.Mail;
using System.Net.Mail;
using System.Net.Security;
using System.Web.Security;
using IMS.@class;
using IMS.Models;
namespace IMS.Controllers
{
    [ExpireChecker]
    
    public class BaseController : Controller
    {
        //
        // GET: /Base/
        public class ExpireChecker : ActionFilterAttribute
        {
            private pmisEntities ifmisdb = new pmisEntities();

            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                try
                {

                    if (System.Web.HttpContext.Current.User != null && System.Web.HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        FormsIdentity id = (FormsIdentity)System.Web.HttpContext.Current.User.Identity;

                        FormsAuthenticationTicket ticket = id.Ticket;

                        if (ticket.Expired == true)
                        {
                            System.Web.HttpContext.Current.Response.Redirect("~/Login/New_index");
                        }
                        else
                        {
                            if (USER.C_Name == "")
                            {
                                FormsAuthentication.SignOut();
                                System.Web.HttpContext.Current.Response.Redirect("~/Login/New_index");
                            }
                        }
                    }
                    else
                    {
                        // the user is not yet authenticated and 
                        // there is no Forms Identity for current request
                        System.Web.HttpContext.Current.Response.Redirect("~/Login/New_index"); 
                         //System.Web.HttpContext.Current.Response.Redirect("", false);
                    }
                }
                catch
                {
                    System.Web.HttpContext.Current.Response.Redirect("~/Login/New_index"); 
                }
            }
        }
    }
}
