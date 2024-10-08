﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using System.Data.Entity;
using Kendo.Mvc.UI;
using System.Web.Services;
using IMS.Models;
using System.Data;
using IMS.@class;
using IMS.Classess;
using System.Data.SqlClient;
using System.Configuration;
namespace IMS.Controllers
{
    public class HomeController : BaseController
    {
        private pmisEntities pmis = new pmisEntities();
        private IMSEntities db = new IMSEntities();

        public ActionResult Welcome()
        {
            return View();
        }
        public PartialViewResult wc()
        {
            return PartialView("pwelcome");
        }
        public ActionResult ByOffice()
        {
            return View("ByOffice");
        }
        public ActionResult addemp()
        {
            return PartialView("addemp");
        }
        public ActionResult Index()
        {
            ViewBag.a = Convert.ToString(USER.C_officeID);
            epsws.serviceSoapClient r = new epsws.serviceSoapClient();

            return View();
        }
        public ActionResult in_out()
        {

            return PartialView("partial_in_out");
        } 
        public ActionResult Users()
        {
            return View();
        }
        public ActionResult pgso()
        {
            return PartialView("pgso");
        }
        public ActionResult partial_in()
        {
            return PartialView("partial_in");
        }
        //datasource for stockin
        public ActionResult InList()
        {
            
            epsws.serviceSoapClient r = new epsws.serviceSoapClient();
            var rec = r.POItems(USER.C_officeID).AsEnumerable().Select(b => new { itemid = b.Field<int>("itemid"), itemName = b.Field<string>("itemname"), unit = b.Field<string>("unit") }).GroupBy(c => c.itemid).Select(group => group.FirstOrDefault()).ToList();
            //var rec = r.POItems(72);        
            return Json(rec, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Stocks()
        {
            return View();
        }
        public ActionResult employeeList()
        {
            if (USER.C_officeID == 72)
            {
                var dt = pmis.vwMergeAllEmployees.Where(b => b.Department == USER.C_officeID || b.Department == 8) .Select(a => new { eid = a.eid, empName = a.EmpName }).OrderBy(b => b.empName).ToList();
                return Json(dt, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var dt = pmis.vwMergeAllEmployees.Where(b => b.Department == USER.C_officeID).Select(a => new { eid = a.eid, empName = a.EmpName }).OrderBy(b => b.empName).ToList();
                return Json(dt, JsonRequestBehavior.AllowGet);
            } 
        }
        public ActionResult Items()
        {
            epsws.serviceSoapClient r = new epsws.serviceSoapClient();
            //DataTable rec = r.POItems(USER.C_officeID);
            DataTable rec = r.POItems(USER.C_officeID);
            List<vwQtyOnHand> listitems = db.vwQtyOnHands.Where(c => c.officeid == USER.C_officeID).ToList();
            var combine = listitems
                .Join(rec.AsEnumerable(),
                        listItem => listItem.item_id, row => row.Field<int>("itemid"),
                        (nnn, li) => new
                        {
                            officeid = nnn.officeid,
                            itemid = nnn.item_id,
                            itemName = li.Field<string>("itemname"),
                            qty = nnn.IN_STOCK,
                            unit = li.Field<string>("unit"),
                        }).GroupBy(c => c.itemid).Select(group => group.FirstOrDefault()).ToList();
            return Json(combine, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ItemQtyList([DataSourceRequest]DataSourceRequest request, string itemName)
        {
            epsws.serviceSoapClient r = new epsws.serviceSoapClient();    
            //DataTable rec = r.POItems(USER.C_officeID);
            DataTable rec = r.POItems(USER.C_officeID);
            List<vwQtyOnHand> listitems = db.vwQtyOnHands.Where(c => c.officeid == USER.C_officeID).ToList();
            var combine = listitems
                .Join(rec.AsEnumerable(),
                        listItem => listItem.item_id, row => row.Field<int>("itemid"),
                        (nnn, li) => new
                        {
                            officeid = nnn.officeid,
                            itemid = nnn.item_id,
                            itemName = li.Field<string>("itemname"),
                            qty = nnn.IN_STOCK,
                            unit = li.Field<string>("unit"),
                        }).GroupBy(c => c.itemid).Select(group => group.FirstOrDefault()).ToList();
            return Json(combine.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
         
        public ActionResult ItemQtySelectList([DataSourceRequest]DataSourceRequest request, int itemid)
        {
            List<vwTransaction> listitems = db.vwTransactions.Where(a => a.item_id == itemid && a.officeid == USER.C_officeID).ToList();
            epsws.serviceSoapClient r = new epsws.serviceSoapClient();
            var rec = r.POItems(USER.C_officeID).AsEnumerable().Select(b => new { itemid = b.Field<int>("itemid"), itemName = b.Field<string>("itemname") }).GroupBy(c => c.itemid).Select(group => group.FirstOrDefault()).ToList();

            var combine = listitems
                .Join(rec.AsEnumerable(),
                        listItem => listItem.item_id,
                        row => row.itemid,
                        (nnn, li) => new
                        {
                            stockdate = Convert.ToDateTime(nnn.stock_date),
                            eName = nnn.eName,
                            in_out = nnn.in_out,
                            ID = nnn.ID,
                            itemid = nnn.item_id,
                            itemName = li.itemName,
                            qty = nnn.quantity,
                            int_out = nnn.in_out,
                            desc = nnn.description
                        });
            return Json(combine.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult dropdownVal()
        {
            epsws.serviceSoapClient r = new epsws.serviceSoapClient();
            //DataTable rec = r.POItems(USER.C_officeID);
            DataTable rec = r.POItems(USER.C_officeID);
            List<vwQtyOnHand> listitems = db.vwQtyOnHands.Where(b => b.IN_STOCK > 0).ToList();
            var combine = listitems
                .Join(rec.AsEnumerable(),
                        listItem => listItem.item_id, row => row.Field<int>("itemid"),
                        (nnn, li) => new
                        {
                            itemid = nnn.item_id,
                            itemName = li.Field<string>("itemname"),
                            qty = nnn.IN_STOCK
                        }).GroupBy(c => c.itemid).Select(group => group.FirstOrDefault()).ToList();
            return Json(combine, JsonRequestBehavior.AllowGet);
        }
        //sa main grid 
        public ActionResult menuList([DataSourceRequest]DataSourceRequest request)
        {
            List<vwTransaction> listitems = db.vwTransactions.Where(a => a.officeid == USER.C_officeID).ToList();
            epsws.serviceSoapClient r = new epsws.serviceSoapClient();
            var rec = r.POItems(USER.C_officeID).AsEnumerable().Select(b => new
            {
                itemid = b.Field<int>("itemid"),
                itemName = b.Field<string>("itemname"),
                unit = b.Field<string>("unit")
            }).GroupBy(c => c.itemid).Select(group => group.FirstOrDefault()).ToList();

            var combine = listitems
                .Join(rec.AsEnumerable(),
                        listItem => listItem.item_id,
                        row => row.itemid,
                        (nnn, li) => new
                        {
                            stockdate = Convert.ToDateTime(nnn.stock_date),
                            eName = nnn.eName,
                            in_out = nnn.in_out,
                            ID = nnn.ID,
                            itemid = nnn.item_id,
                            itemName = li.itemName,
                            qty = nnn.quantity + " " + li.unit + "/s",
                            int_out = nnn.in_out
                        });


            return Json(combine.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public ActionResult itemList([DataSourceRequest]DataSourceRequest request)
        {
            var vAll = db.transactions.ToList();
            return Json(vAll, JsonRequestBehavior.AllowGet);
        }
        public ActionResult stockOut(String desc, int itemID, int qty, int eid)
        {
            transaction entity = new transaction();
            if (qty != 0)
            {
                if (eid == 0)
                    entity.eid = USER.C_eID;
                else
                    entity.eid = eid;

                entity.item_id = itemID;
                entity.stock_date = DateTime.Now;
                entity.quantity = qty;
                entity.in_out = "OUT";
                entity.officeid = USER.C_officeID;
                entity.description = desc;
                db.transactions.AddObject(entity);
                db.SaveChanges();
            }
            return Content("");
        }
        public ActionResult stockIn(int itemID, double qty , string itemname , string unit, double unitcost )
        {
            if (qty != 0)
            {
                transaction entity = new transaction();
                entity.eid = USER.C_eID;
                entity.item_id = itemID;
                entity.stock_date = DateTime.Now;
                entity.quantity = qty;
                entity.in_out = "IN";
                entity.description = "";
                entity.itemname = itemname;
                entity.unit = unit;
                entity.ename = USER.C_Name; 
                entity.officeid = USER.C_officeID;
                entity.unitcost = unitcost;
                db.transactions.AddObject(entity);
                db.SaveChanges();
            }

            return Content("");
        }

        public int filterNum(int itemid)
        {
            int count = db.vwQtyOnHands.SingleOrDefault(a => a.item_id == itemid).IN_STOCK ?? 0;
            return count;
        }

        public ActionResult userList()
        {

            var dt = pmis.vwLoginParameters.Select(a => new { eid = a.eid, emailaddress = a.emailaddress }).OrderBy(b => b.emailaddress).ToList();
            return Json(dt, JsonRequestBehavior.AllowGet);
        }
        public ActionResult usertype()
        {
            var us = db.userRoles.Select(a => new { usertype = a.usertype, roleid = a.roleid }).ToList();
            return Json(us, JsonRequestBehavior.AllowGet);
        }
        public String Adduser(int roleid, int eid , string name , int officeid)
        {
            bool c = db.vwUsers.Where(a => a.eid == eid).Any();
            if (c)
            {
                return "Already Exist";
            }
            else
            {
                try
                {

                    user entity = new user();
                    entity.role = roleid;
                    entity.eid = eid;
                    entity.EmpName = name;
                    entity.OfficeID = officeid;
                    db.users.AddObject(entity);
                    db.SaveChanges();

                    return "1";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }


        }
        public JsonResult SavedUsers([DataSourceRequest] DataSourceRequest request)
        {
            var rec = db.vwUsers.Select(a => new { SwipEmployeeID = a.SwipEmployeeID, emailaddress = a.emailaddress, passcode = a.passcode, eid = a.eid, ID = a.ID, usertype = a.usertype }).OrderBy(a=> new {emailaddress = a.emailaddress });
            return Json(rec.ToDataSourceResult(request), JsonRequestBehavior.AllowGet); 
        }
        public String Deleteuser(int id)
        {

            try
            {
                using (var a = new IMSEntities())
                {
                    user entity = new user();
                    entity.ID = id;
                    a.users.Attach(entity);
                    a.users.DeleteObject(entity);
                    a.SaveChanges();

                    return "1";
                }
            }

            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public ActionResult _reportView()
        {
            return PartialView();
        }
        public ActionResult _desc()
        {
            return PartialView();
        }

        public ActionResult GetOffices([DataSourceRequest] DataSourceRequest request)
        {
            myoffice Ol = new myoffice();
            var list = Ol.Offices();

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult sampleitem([DataSourceRequest] DataSourceRequest request) 
        //{
        //    loigie it = new loigie();
        //    var item = it.Items();
        //    ViewData["item"] = item;
        //    return Json(item, JsonRequestBehavior.AllowGet);
        //}
        public string SaveNew(item it)
        {

            savein el = new savein();
            return el.SaveNew(it);
        }
        public JsonResult readtransact([DataSourceRequest] DataSourceRequest request, int officeid)
        {
            ViewBag.id = officeid;
            read rd = new read();
            var data = rd.Transaction(officeid);
            return Json(data.ToDataSourceResult(request));
        }
        public JsonResult readOut([DataSourceRequest] DataSourceRequest request, int officeid)
        {
            ViewBag.id = officeid;
            epsws.serviceSoapClient r = new epsws.serviceSoapClient();
            read rd = new read();
            var data = rd.Stockout(officeid);
            return Json(data.ToDataSourceResult(request));
        }
        public JsonResult released([DataSourceRequest] DataSourceRequest request, int officeid)
        {
            ViewBag.id = officeid;
            read rd = new read(); 
            var data = rd.Released(officeid);
            return Json(data.ToDataSourceResult(request));
        }
        public string SaveOut([DataSourceRequest] DataSourceRequest request, item it)
        {
            savein el = new savein();
           return el.SaveNew(it);
           //return Json(new[] { it }.ToDataSourceResult(request, ModelState));
        }
        public ActionResult itembyoffice([DataSourceRequest] DataSourceRequest request,int officeid)
        {
             ViewBag.id = officeid;
            epsws.serviceSoapClient r = new epsws.serviceSoapClient();
            int x = officeid == null ? 0 : officeid;
            var rec = r.POItems(x).AsEnumerable().Select(b => new { itemid = b.Field<int>("itemid"), itemName = b.Field<string>("itemname"), unit = b.Field<string>("unit") }).GroupBy(c => c.itemid).Select(group => group.FirstOrDefault()).ToList();
            return Json(rec, JsonRequestBehavior.AllowGet);
        }
        public ActionResult byofficeout([DataSourceRequest] DataSourceRequest request)
        {
           // ViewBag.id = officeid;
            epsws.serviceSoapClient r = new epsws.serviceSoapClient();
            read rd = new read();
            var data = rd.readbyofficeout();
            return Json(data.ToDataSourceResult(request));
        }
        public ActionResult byofficechart([DataSourceRequest] DataSourceRequest request, int itemid)
        {
            read r = new read();
            var d = r.byoffchart(itemid);
            return Json(d, JsonRequestBehavior.AllowGet);
        }
        public String byofficesaveout(item it)
        {
            savein r = new savein();
            return r.byofficeout(it);
        }
        public ActionResult byofftransaction([DataSourceRequest] DataSourceRequest request)
        {
            read r = new read();
            var d = r.byofftrans();
            return Json(d.ToDataSourceResult(request));
        }
        public ActionResult edit_grid_quantity([DataSourceRequest] DataSourceRequest request)
        {
            read r = new read();
            var d = r.edit_quantity();
            return Json(d.ToDataSourceResult(request));
        }
        public string  update_grid_quantity(item items)
        {
            //this is out quantity
            double out_quantity = items.recieve_quantity;  
            //this is old quantity
            double actual_quantity = (@"select quantity from [IMS].[dbo].[transaction]  where ID = '" + items.id + "'").Scalar();
          
            
            //this is new quantity
            double quantity = items.quantity;

            if (quantity < out_quantity)
            {
                return null;
            }
            else 
            {
                (@"update [IMS].[dbo].[transaction] set quantity = '"+quantity+"' where ID = '"+items.id+"' ").NonQuery();
                return "1";
            }
              
        }

        public string delete_grid_quantity(int id)
        {
            (@"delete from [IMS].[dbo].[transaction] where ID = '"+id+"'").NonQuery();
            return "1";
        }
         
        public string submit_selected(IEnumerable<item> items)
        {
            var error_items = "";
            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));

            String dy = datevalue.Day.ToString();
            String mn = datevalue.Month.ToString();
            String yy = datevalue.Year.ToString();

            string query1 = "exec Proc_GetNewOfficeRis '[IMS].[dbo].[transaction]','RIS','controlno','" + yy + "','" + mn + "'";
            string strcon = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();
            string control_number = OleDbHelper.ExecuteScalar(strcon, System.Data.CommandType.Text, query1).ToString();

             
            try
            {
                #region foreach
                foreach (var i in items)
                {
                var date = DateTime.Now;
                var eid = USER.C_eID;
                var ename = USER.C_Name;
                var officeid = USER.C_officeID;
                var itemid = i.itemid;
                var unit = i.unit;
                var itemname = i.itemname.ToString();
                var quantity = i.request_quantity;
                var reid = i.reid;
                var row_number = 0;


                DataTable chck_total_avail = (@"select a.*,b.total_out,a.total_in - isnull(b.total_out,0) as total_available from (select item_id,itemname,SUM(CASE WHEN in_out = 'IN' THEN quantity ELSE 0 END) AS total_in 
						FROM [IMS].[dbo].[transaction] where in_out = 'IN' and item_id = '" + itemid + "' and officeid = '" + USER.C_officeID + "' group by item_id, itemname) as a left join (select  item_id,itemname,SUM(CASE WHEN in_out = 'OUT' THEN quantity ELSE 0 END) AS total_out FROM [IMS].[dbo].[transaction] where in_out = 'OUT' and ref_id != null and item_id = '" + itemid + "' and officeid = '" + USER.C_officeID + "' group by item_id, itemname )  as b on a.item_id = b.item_id").DataSet();

                var total_avail = Convert.ToDouble(chck_total_avail.Rows[0]["total_available"].ToString());

                if (total_avail < quantity)
                {
                    return "Ops Remaining Quantity for'" + chck_total_avail.Rows[0]["itemname"].ToString() + "' is only '" + total_avail + "' ";
                }


                DataTable item_source = (@"select id,item_id,itemname,unit,total_in,total_out,available from (
  select a.*,b.total_out,a.total_in - isnull(total_out,0) as available from (select id,item_id,itemname,unit,sum(CASE WHEN in_out = 'IN' THEN quantity ELSE 0 END) AS total_in 
   FROM [IMS].[dbo].[transaction] where in_out = 'IN' and item_id = '" + itemid + "' and officeid = '" + USER.C_officeID + "' group by item_id, itemname, id,unit) as a left join (select  item_id,itemname,SUM(CASE WHEN in_out = 'OUT' THEN quantity ELSE 0 END) AS total_out, ref_id  FROM [IMS].[dbo].[transaction] where in_out = 'OUT' and ref_id is not null and item_id = '" + itemid + "' and officeid = '" + USER.C_officeID + "' group by item_id, itemname,ref_id) as b on a.ID= b.ref_id ) as z where available != 0 and available !<0   order by id ").DataSet();

                var initial_available = item_source.Rows[row_number]["available"].ToString();
                var int_available = Convert.ToDouble(initial_available);



                if (quantity > int_available)
                {
                    var total_pass = 0.0;
                    var y = quantity;
                    #region for loop
                    for (var x = 0; quantity != total_pass; x++)
                    {
                        var dt_itemid = item_source.Rows[row_number]["item_id"].ToString();
                        var dt_itemname = item_source.Rows[row_number]["itemname"].ToString(); 
                        var dt_unit = item_source.Rows[row_number]["unit"].ToString(); 
                        var dt_available = item_source.Rows[row_number]["available"].ToString();
                        var _available = Convert.ToDouble(dt_available);
                        var ref_id = item_source.Rows[row_number]["id"].ToString();

                        if (_available > y)
                        {
                            try
                            {
                                string qry = @"insert into [IMS].[dbo].[transaction](eid,item_id,stock_date,quantity,in_out,officeid,description,itemname,unit,reid,ename,controlno,ref_id) values ('" + eid + "','" + dt_itemid + "', '" + date.ToString("MM-dd-yyyy HH:mm:ss") + "','" + y + "','OUT','" + officeid + "','','" + dt_itemname.Replace("'", "''") + "' ,'" + dt_unit + "','" + reid + "','" + ename + "','" + control_number + "','" + ref_id + "')";
                                string con = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();
                                string result = OleDbHelper.ExecuteNonQuery(con, System.Data.CommandType.Text, qry).ToString();

                            }
                            catch
                            {
                                error_items += " '" + dt_itemname + "' ";
                            }
                            total_pass += y;
                        }
                        else 
                        {
                            try
                            {
                                string qry = @"insert into [IMS].[dbo].[transaction](eid,item_id,stock_date,quantity,in_out,officeid,description,itemname,unit,reid,ename,controlno,ref_id) values ('" + eid + "','" + dt_itemid + "', '" + date.ToString("MM-dd-yyyy HH:mm:ss") + "','" + _available + "','OUT','" + officeid + "','','" + dt_itemname.Replace("'", "''") + "' ,'" + dt_unit + "','" + reid + "','" + ename + "','" + control_number + "','" + ref_id + "')";
                                string con = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();
                                string result = OleDbHelper.ExecuteNonQuery(con, System.Data.CommandType.Text, qry).ToString(); 
                            }
                            catch
                            {
                                error_items += " '" + dt_itemname + "' ";
                            }
                            
                            y -= _available;
                            total_pass += _available;
                            row_number += 1;
                        }
                    }
                    #endregion
                }

                else if (quantity <= int_available)
                {
                    var dt_itemid = item_source.Rows[0]["item_id"].ToString();
                    var dt_itemname = item_source.Rows[0]["itemname"].ToString();
                    var dt_unit = item_source.Rows[0]["unit"].ToString();
                    var dt_available = item_source.Rows[0]["available"].ToString();
                    var _available = Convert.ToInt32(0);
                    var ref_id = item_source.Rows[0]["id"].ToString();
                    try
                    {
                        string qry = @"insert into [IMS].[dbo].[transaction](eid,item_id,stock_date,quantity,in_out,officeid,description,itemname,unit,reid,ename,controlno,ref_id) values ('" + eid + "','" + dt_itemid + "', '" + date.ToString("MM-dd-yyyy HH:mm:ss") + "','" + quantity + "','OUT','" + officeid + "','','" + dt_itemname.Replace("'", "''") + "' ,'" + dt_unit + "','" + reid + "','" + ename + "','" + control_number + "','" + ref_id + "')";
                        string con = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();
                        string result = OleDbHelper.ExecuteNonQuery(con, System.Data.CommandType.Text, qry).ToString();

                    }
                    catch
                    {
                        error_items += " '" + item_source.Rows[0]["itemname"].ToString() + "' ";
                    }
                }
                }
                #endregion
                return control_number;
            }
            catch (Exception ex)
            {
                return "" + ex;
            }
        }

        public ActionResult get_office_ris([DataSourceRequest] DataSourceRequest request)
        {
            read r = new read();
            var d = r.get_ris_code();

            var jsonResult = Json(d, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
           // return Json(d , JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
         
        public string getOutItems()
        {
            read r = new read();
            try
            {
                submit_selected(r.getOutItems());
               (@"update [IMS].[dbo].[transaction] set description = 'done' where officeid = '"+USER.C_officeID+"' and  in_out = 'OUT' and ref_id is null ").NonQuery();
                return "1";
            }
            catch (Exception ex)
            {
                return "" + ex;
            }
        }

        public ActionResult readItemsWithReorder([DataSourceRequest] DataSourceRequest request)
        { 
            read r = new read();
            var d = r.readItemsWithReorder(); 
            return Json(d.ToDataSourceResult(request));
        } 
        public string  updateItemsWithReorder(item items)
        {

            string ret = "";

            var exist = (@"select count(*) from  [IMS].[dbo].[tbl_l_reorder] where officeid = '"+USER.C_officeID+"' and itemid = '"+items.itemid+"'").Scalar();

            if (exist > 0)
            {
                (@"update [IMS].[dbo].[tbl_l_reorder] set reorder_point = '" + items.quantity + "' where itemid = '" + items.itemid + "' ").NonQuery();
                 ret = "1";
            }
            else
            {
                (@"insert into [IMS].[dbo].[tbl_l_reorder]  values ('"+items.itemid+"','"+items.quantity+"','"+USER.C_officeID+"') ").NonQuery();
                ret = "1";
            }


            return ret;
        } 
    }
}





   ////Added Function to auto fill out dtr time
   //         foreach (DataRow row in dt.Rows)
   //         {
   //             var date = row["date_entry"].ToString();
   //             var date_mnt = row["dte"].ToString();
   //             var Time_in = row["am_time_in"].ToString();
   //             var Time_out = row["am_time_out"].ToString();
   //             var Time_in2 = row["pm_time_in"].ToString();
   //             var Time_out2 = row["pm_time_out"].ToString();
   //             var Over_in = row["Overtime_in"].ToString();
   //             var Over_out = row["Overtime_out"].ToString();
   //             var Remarks = row["remarks"].ToString();
   //             var member_id = Convert.ToInt32(row["eid"].ToString());

   //             var new_amin = (@" select  top 1 * from  [TransOrient].[dbo].[temp_time]  where SwipeID = 11 and tdate = '2018-07-17' order by ABS(DATEDIFF(minute, ttime , '20:00'))").ScalarString();




   //         }
