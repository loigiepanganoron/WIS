using IMS.@class;
using IMS.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace IMS.Controllers
{
    public class MenuController : Controller
    { 
        public ActionResult LoadMenu()
        {
            List<MenuModel> mnu = new List<MenuModel>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            { 
                string qry = "";

                if (USER.C_role == 7)
                {
                    qry = @"select MenuID,ParentID,MenuLevel,Description,MenuTitle,Action,Controller,Icon from [IMS].[dbo].[menu] where ActionCode = 1  order by MenuLevel,MenuOrder,ParentID ";
                }
                else
                {
                    qry = @"select MenuID,ParentID,MenuLevel,Description,MenuTitle,Action,Controller,Icon from [IMS].[dbo].[menu] where ActionCode = 1 and is_for = '" + USER.C_role + "'  order by MenuLevel,MenuOrder,ParentID ";
                }

                SqlCommand com = new SqlCommand(qry, con);




                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    MenuModel item = new MenuModel();
                    item.MenuID = Convert.ToInt32(reader.GetValue(0));
                    item.MenuParent = Convert.ToInt32(reader.GetValue(1));
                    item.MenuLevel = Convert.ToInt32(reader.GetValue(2));
                    item.Description = reader.GetValue(3).ToString();
                    item.Tittle = reader.GetValue(4).ToString();
                    item.Action = reader.GetValue(5).ToString();
                    item.Controller = reader.GetValue(6).ToString();
                    item.Icon = reader.GetValue(7).ToString();
                    mnu.Add(item);
                }
            }
            return PartialView("_sideMenu", mnu);
        }
    }
}
