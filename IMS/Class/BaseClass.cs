﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Script.Serialization;
using System.Web.Mvc;
using System.Configuration;
using System.Data;
using IMS.Models;
using System.Security.Cryptography;
using IMS.Classess;
namespace IMS.@class
{

    public class UserDataModelBinder<T> : IModelBinder
    {
        
        public object BindModel(ControllerContext controllerContext,
            ModelBindingContext bindingContext)
        {
            if (bindingContext.Model != null)
                throw new InvalidOperationException("Cannot update instances");
            if (controllerContext.RequestContext.HttpContext.Request.IsAuthenticated)
            {
                var cookie = controllerContext
                    .RequestContext
                    .HttpContext
                    .Request
                    .Cookies[FormsAuthentication.FormsCookieName];

                if (null == cookie)
                    return null;

                var decrypted = FormsAuthentication.Decrypt(cookie.Value);

                if (!string.IsNullOrEmpty(decrypted.UserData))
                    return new JavaScriptSerializer().Deserialize<T>(decrypted.UserData); // .Deserialize<T>(decrypted.UserData);
            }
            return null;
        }
    }

    public static class HttpResponseBaseExtensions
    {
        public static int SetAuthCookie<T>(this HttpResponseBase responseBase, string name, bool rememberMe, T userData)
        {
            /// In order to pickup the settings from config, we create a default cookie and use its values to create a 
            /// new one.
            var cookie = FormsAuthentication.GetAuthCookie(name, rememberMe);
            var ticket = FormsAuthentication.Decrypt(cookie.Value);
            var data = new JavaScriptSerializer().Serialize(userData);

            var newTicket = new FormsAuthenticationTicket(ticket.Version, ticket.Name, ticket.IssueDate, ticket.Expiration,
                ticket.IsPersistent, data, ticket.CookiePath);
            var encTicket = FormsAuthentication.Encrypt(newTicket);

            /// Use existing cookie. Could create new one but would have to copy settings over...
            cookie.Value = encTicket;
            //cookie.Expires = DateTime.Now.AddHours(1);
            
            responseBase.Cookies.Add(cookie);

            return encTicket.Length;
        }
    }
    public struct USER 
    {
        
        public static void Set(long eid, long roleID)
        {
            
            //pmisEntities db = new pmisEntities();
            
            
            //var check = db.vwMergeAllEmployees.Count(a => a.eid == eid);


            DataTable dt = (@"select * from [pmis].[dbo].[vwMergeAllEmployee]  where eid = " + eid+"").DataSet();



            //if (check > 0)
            //{
               // var rec = db.vwMergeAllEmployees.SingleOrDefault(a => a.eid == eid);
              
                var  off = (@"select a.OfficeID,b.OfficeName from [IMS].[dbo].[user] as a inner join IMS.dbo.tbl_t_Office as b on a.OfficeID = b.OfficeID where eid = '"+eid+"'").DataSet();

                HttpCookie cookie = new HttpCookie(".userinfo");
                cookie.Values["eid"] = dt.Rows[0]["eid"].ToString();
                cookie.Values["name"] = dt.Rows[0]["EmpName"].ToString();  
                cookie.Values["OfficeID"] = off.Rows[0]["OfficeId"].ToString();
            // cookie.Values["officename"] = (dt.Rows[0]["OfficeName"].ToString().Contains("National Office")  ?  "Register of Deeds" : dt.Rows[0]["OfficeName"].ToString())    ;   
            cookie.Values["officename"] = off.Rows[0]["OfficeName"].ToString();


            
                cookie.Values["pst"] = dt.Rows[0]["Position"].ToString();
            //cookie.Values["piclink"] = "http://192.168.2.104/idprod/PGAS/" + rec.SwipeId.ToString() + ".jpg";
            cookie.Values["piclink"] = "http://10.100.100.5/HRIS/Content/images/photos/" + dt.Rows[0]["eid"].ToString() + ".png";
            cookie.Values["role"] = roleID.ToString();


                //cookie.Values["eid"] = rec.eid.ToString();
                //cookie.Values["name"] = rec.EmpName.ToString();
                //cookie.Values["OfficeID"] = off.Rows[0]["OfficeID"].ToString();
                //cookie.Values["officename"] = off.Rows[0]["OfficeName"].ToString();
                //cookie.Values["pst"] = rec.Position.ToString();
                ////cookie.Values["piclink"] = "http://192.168.2.104/idprod/PGAS/" + rec.SwipeId.ToString() + ".jpg";
                //cookie.Values["piclink"] = "http://192.168.2.104/HRIS/Content/images/photos/"+eid.ToString() + ".png";
                //cookie.Values["role"] = roleID.ToString();
                cookie.Expires = DateTime.Now.AddDays(2);
                HttpContext.Current.Response.Cookies.Add(cookie);
            //}
            //else {
            //    DataTable dt = ("select * from [pmis].[dbo].[employee] where eid = '" + eid + "'").DataSet();
            //    var mi = dt.Rows[0]["MI"].ToString();
            //    var offname = ("select OfficeName from fmis.dbo.tblREF_AIS_Offices where pmisOfficeID='"+dt.Rows[0]["Office"].ToString()+"'").ScalarString();
            //    HttpCookie cookie = new HttpCookie(".userinfo");
            //    cookie.Values["eid"] =  dt.Rows[0]["eid"].ToString();
            //    cookie.Values["name"] = dt.Rows[0]["Firstname"].ToString() + ',' + dt.Rows[0]["Lastname"].ToString() + ',' + mi[0] ;
            //    cookie.Values["OfficeID"] = dt.Rows[0]["Office"].ToString();
            //    cookie.Values["officename"] = offname;
            //    cookie.Values["pst"] = dt.Rows[0]["Position"].ToString();
            //    cookie.Values["piclink"] = "http://192.168.2.104/idprod/PGAS/" + dt.Rows[0]["swipeid_new"].ToString()+".jpg";
            //    cookie.Values["role"] = roleID.ToString();
            //    cookie.Expires = DateTime.Now.AddDays(1);
            //    HttpContext.Current.Response.Cookies.Add(cookie);
            //}
        }

        public static void Set2(string emailaddress, string password)
        { 
            DataTable dt = (@"select *  FROM [IMS].[dbo].[tbl_employee]  where username = '" + emailaddress + "' and password = '" + password + "' ").DataSet();
              
            HttpCookie cookie = new HttpCookie(".userinfo");
            cookie.Values["eid"] = dt.Rows[0]["eid"].ToString();
            cookie.Values["name"] = dt.Rows[0]["EmpName"].ToString();
            cookie.Values["OfficeID"] = dt.Rows[0]["officeid"].ToString();
            cookie.Values["officename"] = (dt.Rows[0]["OfficeName"].ToString().Contains("National Office") ? "Register of Deeds" : dt.Rows[0]["OfficeName"].ToString());
            cookie.Values["pst"] = dt.Rows[0]["Position"].ToString();
            //cookie.Values["piclink"] = "http://192.168.2.104/idprod/PGAS/" + rec.SwipeId.ToString() + ".jpg";
            cookie.Values["piclink"] = "http://10.100.100.5/HRIS/Content/images/photos/" + dt.Rows[0]["eid"].ToString() + ".png";
            cookie.Values["role"] = 1.ToString();
             
            cookie.Expires = DateTime.Now.AddDays(2);
            HttpContext.Current.Response.Cookies.Add(cookie);
           
        }



        public static void Logout()
        {
            FormsAuthentication.SignOut();
            HttpCookie cookie = HttpContext.Current.Request.Cookies[".userinfo"];
            cookie.Expires = DateTime.Now.AddDays(-1);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        public static int C_role
        {
            get
            {
                HttpCookie ck = HttpContext.Current.Request.Cookies.Get(".userinfo");
                return ck.Value != null ? Convert.ToInt32(ck.Values["role"]) : -1;
            } 
        }
        public static int C_eID
        {
            get
            {
                HttpCookie ck = HttpContext.Current.Request.Cookies.Get(".userinfo");
                return ck.Value != null ? Convert.ToInt32(ck.Values["eid"]) : -1;
            }
        }
        public static int C_officeID
        {
            get
            {
                HttpCookie ck = HttpContext.Current.Request.Cookies.Get(".userinfo");
                return ck.Value != null ? Convert.ToInt32(ck.Values["OfficeID"]) : -1;
            }
        }
        public static string C_Name
        {
            get
            {
                HttpCookie ck = HttpContext.Current.Request.Cookies.Get(".userinfo");
                return ck.Value != null ? ck.Values["name"] : "";
            }
        }

       
        public static string C_Office
        {
            get
            {
                HttpCookie ck = HttpContext.Current.Request.Cookies.Get(".userinfo");
                return ck.Value != null ? ck.Values["officename"] : "";
            }
        }
        public static string C_Position
        {
            get
            {
                HttpCookie ck = HttpContext.Current.Request.Cookies.Get(".userinfo");
                return ck.Value != null ? ck.Values["pst"] : "";
            }
        }

        public static string C_picLink
        {
            get
            {
                HttpCookie ck = HttpContext.Current.Request.Cookies.Get(".userinfo");
                return ck.Value != null ? ck.Values["picLink"] : "";
            }
        }
    }
}