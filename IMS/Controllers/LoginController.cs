﻿using System;
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
using IMS.Classess;
using System.Data.SqlClient;



namespace IMS.Controllers
{
    public class LoginController : Controller
    {
        
        // GET: /Login/
        private pmisEntities pmidb = new pmisEntities();
        private IMSEntities imsdb = new IMSEntities();
        public ActionResult Index()
        {
            return View("New_index");
        }
        public ActionResult New_index()
        {
            return View("New_index");
        }
        public ActionResult CheckParameter(string emailaddress, string passcode)
        {
            string _status = "failed";
           
            if (ModelState.IsValid)
            {
                long eid;
                long roleId;
                var userValid = pmidb.vwLoginParameters.Any(user => user.emailaddress == emailaddress && user.passcode == passcode);

                if (userValid)
                {
                    var rec = pmidb.vwLoginParameters.SingleOrDefault(user => user.emailaddress == emailaddress && user.passcode == passcode);

                    bool userRegistered = imsdb.users.Any(a => a.eid == rec.eid);

                    if (userRegistered)
                    {
                        FormsAuthentication.SetAuthCookie(emailaddress, true);
                        System.Web.Security.FormsAuthentication.SetAuthCookie(emailaddress, false);

                        eid = (long)rec.eid;
                        roleId = (long)imsdb.users.SingleOrDefault(a => a.eid == rec.eid).role;
                        USER.Set(eid, roleId);
                        _status = "success";
                    }
                    else
                    {
                        _status = "unregistered";
                    }
                }
                else
                {
                    DataTable emp2 = new DataTable("Data"); 
                    using(SqlConnection con = new SqlConnection(common.livecon()))
                    {
                        SqlCommand com = new SqlCommand(@"select *  FROM [IMS].[dbo].[tbl_employee]  where username = '"+ emailaddress + "' and password = '"+passcode+"' ", con);
                        con.Open();
                        SqlDataReader reader = com.ExecuteReader();
                        emp2.Load(reader);
                        con.Close();
                    }

                    if (emp2.Rows.Count > 0)
                    {
                        FormsAuthentication.SetAuthCookie(emailaddress, true);
                        System.Web.Security.FormsAuthentication.SetAuthCookie(emailaddress, false);
                        USER.Set2(emailaddress, passcode);
                        _status = "success";
                    }
                    else
                    {
                        _status = "unregistered";
                    }
                }
            }
            else
            {
                ViewBag.loginerror = true;
            }
            return Content(_status);
        }
        public ActionResult Logout()
        {
            USER.Logout();
            return RedirectToAction("New_index", "Login");
        }
        public ActionResult _myInfo()
        {
            return Content(USER.C_picLink);
        }
        public ActionResult _myName()
        {
            return Content(USER.C_Name);
        }
        public ActionResult _officeName()
        {
            return Content(USER.C_Office);
            
        }
        public ActionResult _offid() 
        {
            return Content(Convert.ToString(USER.C_officeID));
        }
        public ActionResult _myeID()
        {
            return Content(Convert.ToString(USER.C_eID));
        }
        public ActionResult _myRoleID()
        {
            return Content(Convert.ToString(USER.C_role));
        }
        public ActionResult _piclink()
        {
            return Content(Convert.ToString(USER.C_picLink));
        }
        public ActionResult OfficeNew([DataSourceRequest] DataSourceRequest request)
        {
        //    string qry = "select * from [IMS].[dbo].[tbl_t_Office] order by OfficeName";
         //   DataTable dt = qry.DataSet();
            DataTable dt = new DataTable("Data");

            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand(@"select * from [IMS].[dbo].[tbl_t_Office] order by OfficeName", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                dt.Load(reader);
                con.Close();
            }
            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }
        public ActionResult Employees([DataSourceRequest] DataSourceRequest request)
        {
            //string qry = "select eid,EmpName from [pmis].[dbo].[vwMergeAllEmployee] order by EmpName";
            //DataTable dt = qry.DataSet(); 
            DataTable dt = new DataTable("Data");
       
            using (SqlConnection con = new SqlConnection(common.livecon()))
            {
                SqlCommand com = new SqlCommand(@"select eid,EmpName from [pmis].[dbo].[vwMergeAllEmployee]  UNION  select eid,[EmpName]  FROM [IMS].[dbo].[tbl_employee]  order by EmpName", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                dt.Load(reader);
                con.Close();
            } 
            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }
        public string create_account (int eid,int officeid, string name)
        {
            int check = ("select count(*) from [IMS].[dbo].[user] where eid = '"+eid+"'").Scalar();

            if (check > 0)
            {
                return "Already Exist!";
            }
            try
            {
                (@"insert into [IMS].[dbo].[user] values ('1','" + eid + "','" + name.ToUpper() + "','" + officeid + "','')").NonQuery();
                return "1";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
