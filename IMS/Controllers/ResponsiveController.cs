using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using System.Data.Entity;
using Kendo.Mvc.UI;
using System.Web.Services;
using IMS.Models;
using System.Collections;
using System.Data;
using IMS.@class;
using System.Data.SqlClient;
using System.Configuration;
using IMS.Classess;
using System.Web.Script.Serialization;
using System.IO;
using System.Web.Security;
using System.Threading;
using System.Data.Common;
using System.Data.EntityModel;
using System.Data.Mapping;
using System.Data.OleDb;
using IMS.SMS;
using Newtonsoft.Json;


namespace IMS.Controllers
{
    public class ResponsiveController : BaseController
    {

        private pmisEntities pmis = new pmisEntities();
        private IMSEntities db = new IMSEntities();
        private SMS.SMSWebServiceSoapClient sms = new SMS.SMSWebServiceSoapClient();
        


        // GET: Responsive
        

        #region
        
        public ActionResult landingview()
        {
            return  View();
        }
        [ChildActionOnly]
        public ActionResult landing()
        { 
            return new FilePathResult("~/Content/landing.html", "text/html");
        }
        public ActionResult release()
        {
            return View();
        }
        public ActionResult Monitoring()
        {
            return View();
        }
        public ActionResult Borrow()
        {
            return PartialView("returned_item_view");
        }
        
        public ActionResult User()
        {
            return View();
        }
        public ActionResult allocated_page()
        {
            return View();
        }
        public ActionResult print_ris()
        {
            return View();
        }
        public ActionResult manage_ris()
        {
            return View();
        }
        public ActionResult entry_barcode()
        {
            return View();
        }
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
          [Authorize]
        public ActionResult Wispage()
        {
            return View();
        }
          [Authorize]
        public ActionResult Rispage()
        {
            return View();
        }
          [Authorize]
          
        public ActionResult Submited_ris()
        {
            return View();
        }
          [Authorize]
        public ActionResult Approved_ris()
        {
            return View();
        }
        public JsonResult apr_bb([DataSourceRequest]DataSourceRequest request,int year)
        {
            epsws.serviceSoapClient r = new epsws.serviceSoapClient();
             
            var rec = r.GetTransactions(year)
                .AsEnumerable()
                .Select(b => new 
                {  
                    cyear = b.Field<int>("cyear") , 
                    transno = b.Field<string>("transno")
                }).ToList();

            return Json(rec, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getitems([DataSourceRequest]DataSourceRequest request)
        {
            
            epsws.serviceSoapClient r = new epsws.serviceSoapClient();
            var rec = r.GetItems("ADS-APR-160034")
                .AsEnumerable()
                .Select(b => new
                {
                    itemid = b.Field<int>("itemid"),
                    officeid = b.Field<int>("officeid"),
                    officename = b.Field<string>("office"),
                    itemname = b.Field<string>("itemname"),
                    unit = b.Field<string>("unit"),
                    unitcost = b.Field<dynamic>("unitcost"),
                    quantity = b.Field<decimal>("quantity")
                })
                 .GroupBy(ac => new
                 {

                     ac.itemid,
                     ac.officename,
                     ac.officeid,// required by your view model. should be omited
                     // in most cases because group by primary key
                     // makes no sense.
                     ac.unit,
                     ac.itemname,
                     ac.unitcost,
                 })
                  .Select(ac => new
                   {
                       officename = ac.Key.officename,
                       officeid = ac.Key.officeid,
                       itemid = ac.Key.itemid,
                       itemname = ac.Key.itemname,
                       unitcost = ac.Key.unitcost,
                       unit = ac.Key.unit,
                       iquantity = ac.Sum(acs => acs.quantity)
                   })
                .ToList();

            return Json(rec.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public ActionResult _update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<item> Items)
        {
            savein el = new savein();
            try
            {
                el.apr_bb_entry(Items);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
                //return Content("<script language='javascript' type='text/javascript'>alert('Error');</script>");
            }
            return Json(Items.ToDataSourceResult(request, ModelState));
        }
        public ActionResult StockViewing()    //View for stock viewing wis sa pgso
        {
            return View();
        }
        public ActionResult get_available_item([DataSourceRequest]DataSourceRequest request)
        {
            wis r = new wis();
            var d = r.Getitems();
            return Json(d.ToDataSourceResult(request));
        }
        public ActionResult submit_ris([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<item> Items)
        {
            wis r = new wis();
            try
            {
                r.submit_ris(Items);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
                //return Content("<script language='javascript' type='text/javascript'>alert('Error');</script>");
            }
            return Json(Items.ToDataSourceResult(request, ModelState));
        }
        public ActionResult read_approved_items([DataSourceRequest]DataSourceRequest request)
        {
            wis r = new wis();
            var d = r.get_aprroved_items();
            return Json(d.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public ActionResult read_sumitted_ris([DataSourceRequest]DataSourceRequest request)
        {
            wis r = new wis();
            var d = r.get_submitted_ris();
            return Json(d.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public ActionResult read_approved_ris([DataSourceRequest]DataSourceRequest request)
        {
            wis r = new wis();
            var d = r.get_approved_ris();
            return Json(d.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public string approve(int[] items)
        {
            wis r = new wis();
            return r.approve_submited_ris(items);
        }
        public string update_request_ris(item items)
        {
            wis r = new wis();
            return r.update_ris(items);
        }
        public string delete_request_ris(item items)
        {
            wis r = new wis();
            return r.delete_ris(items);
        }
        public string return_approved_ris(int[] items)
        {
            wis r = new wis();
            return r.return_ris(items);
        }
        //public ActionResult read_code(string code)   //local
        //{
        //    wis r = new wis();
        //    var d = r.get_item_description(code);
        //    return Json(d, JsonRequestBehavior.AllowGet);
        //}
        public string entry_via_code(item items)
        {
            wis r = new wis();
            return r.instock_readed_code(items);
        }
        public int filterNum(int itemid, string transcode)
        {
            string query = "USE [IMS] exec totalsum " + itemid + " , '" + transcode + "'";
            string strcon = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();

            string dt = OleDbHelper.ExecuteScalar(strcon, System.Data.CommandType.Text, query).ToString();

            var total = Convert.ToInt32(dt);

            return total;

        }
        public int filterNum_release(int itemid, string transcode)
        {
            string query = "SELECT  SUM(CASE WHEN in_out = 'IN' THEN quantity ELSE 0 END) - SUM(CASE WHEN in_out = 'OUT' THEN quantity ELSE 0 END) AS total FROM   [IMS].[dbo].[tbl_wis_transaction] where transcode = '" + transcode + "'  and itemid = " + itemid + " ";
            string strcon = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();

            string dt = OleDbHelper.ExecuteScalar(strcon, System.Data.CommandType.Text, query).ToString();

            var total = Convert.ToInt32(dt);

            return total;
        }
        public ActionResult get_added_in([DataSourceRequest] DataSourceRequest request)
        {
            wis r = new wis();
            var d = r.get_added_recent();
            return Json(d.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public ActionResult register_item()
        {
            return View();
        }
        public ActionResult check_code(string transaction_code)
        {
            string _status = "failed";
            var tcode = db.tbl_items_per_transaction_code.Any(a => a.transaction_code == transaction_code);
             
            if (tcode)
            {
                _status = "wala";
            }
            else
            {
                wis r = new wis();
                var d = r.insert_items_percode(transaction_code);
                _status = "save";
            }
            return Content(_status);
        }
        public ActionResult all_items()
        { 
            try
            {
                //transmittal_ws.serviceSoapClient ws = new transmittal_ws.serviceSoapClient();

                epsws.serviceSoapClient r = new epsws.serviceSoapClient();

                DataTable rec = new DataTable();
                try
                {
                    rec = r.Items();
                }
                catch (Exception wezx)
                {

                }

                if (rec.Rows.Count > 0)
                {
                    DataRow[] results = rec.Select("itemname not like ''");

                    var dr = results.AsEnumerable().Select(a => new { itemid = a.Field<int>("itemid"), itemname = a.Field<string>("itemname"), unit = a.Field<string>("unit") }).ToList();

                    var it = db.tbl_manual_items.Select(a => new { itemid = a.id, itemname = a.itemname, unit = a.unit }).ToList();
                     
                    var result = dr.Concat(it).OrderBy(x => x.itemname).ToList();
                      
                    return Json(it, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var it = db.tbl_manual_items.Select(a => new { itemid = a.id, itemname = a.itemname, unit = a.unit }).ToList();
                     
                    return Json(it, JsonRequestBehavior.AllowGet);
                }

             


                // .AsEnumerable().Select(b => new { itemid = b.Field<Int32>("itemid"), itemname = b.Field<String>("itemname").Replace("\n", ""), unit = b.Field<String>("unit") }).ToList();

                //DataTable dt = r.Items();
                //dt.DefaultView.Sort = "itemid asc";
                //dt = dt.DefaultView.ToTable(); 

           
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message.ToString());
                return null;
            }
           
        }
        public string all_items_actualprice(int itemid)
        {
            epsws.serviceSoapClient r = new epsws.serviceSoapClient();
            //var sd = db.tbl_items_per_transaction_code.Where(a => a.itemid == itemid).ToList();
             var sd = (@"select distinct unit from [IMS].[dbo].[tbl_items_per_transaction_code] where itemid = '"+itemid+"'  ").ScalarString();
            return sd;
        }
        public string save_itemcode(item items)
        {
            wis r = new wis();
            return r.save_new_itemcode(items);
        }
        public ActionResult get_ris_print([DataSourceRequest] DataSourceRequest request, string transcode)
        {
            wis r = new wis();
            var d = r.get_ris_print_method(transcode);
            return Json(d.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public ActionResult get_item_by_tnum_offid([DataSourceRequest] DataSourceRequest request, string transcode, int officeid)
        {
            wis r = new wis();
            var d = r.get_item_by_tnum_offid(transcode, officeid);
            return Json(d.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public string confirm_out(item items)
        {

            wis r = new wis();
            try
            {
                r.confirm_out_na(items);
                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public ActionResult get_out_history([DataSourceRequest] DataSourceRequest request, string transcode)
        {
            wis r = new wis();
            var d = r.history(transcode);
            return Json(d.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public string update_barcodes(item items)
        {
            epsws.serviceSoapClient r = new epsws.serviceSoapClient();
            try
            {
                var rec = r.SaveBarcode(items.itemid, items.itemcode);
                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public ActionResult sample()
        {
            epsws.serviceSoapClient r = new epsws.serviceSoapClient();
            var rec = r.Items().AsEnumerable().Select(b => new { itemid = b.Field<int>("itemid"), itemname = b.Field<string>("itemname"), unit = b.Field<string>("unit"), code = b.Field<string>("code") }).ToList();
            return Json(rec, JsonRequestBehavior.AllowGet);
        }
        public ActionResult read_itemcode(string code, string transcode)
        {
            epsws.serviceSoapClient r = new epsws.serviceSoapClient();

            var data = r.GetItem(code).AsEnumerable().Select(s => new { itemid = s.Field<int>("itemid"), itemname = s.Field<string>("itemname"), unit = s.Field<string>("unit") }).ToList()
                        .Join(db.tbl_items_per_transaction_code.Where(a => a.transaction_code == transcode),
                          c => c.itemid,
                          o => o.itemid,
                          (c, o) => new
                                    {
                                        itemid = c.itemid,
                                        itemname = c.itemname,
                                        unit = c.unit,
                                        price = o.price
                                    }).FirstOrDefault();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult read_remaining([DataSourceRequest] DataSourceRequest request, string transcode)
        {
            wis r = new wis();
            var d = r.remaining_wis(transcode);
            return Json(d.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public string save_borrow_method(item items)
        {
            wis r = new wis();
            return r.save_borrow(items);
        }
        public ActionResult get_sub_req([DataSourceRequest] DataSourceRequest request, string transcode)
        {
            wis r = new wis();
            var d = r.get_submited_req(transcode);
            return Json(d.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public ActionResult get_app([DataSourceRequest] DataSourceRequest request, string transcode)
        {
            wis r = new wis();
            var d = r.get_approve_req(transcode);
            return Json(d.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public ActionResult get_for_app([DataSourceRequest] DataSourceRequest request)
        {
            wis r = new wis();
            var d = r.get_for_approve_req();
            return Json(d.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public string approve_borrow_req(int[] items)
        {
            wis r = new wis();
            return r.approve_borrow(items);
        }
        public ActionResult get_borrow_by_office([DataSourceRequest] DataSourceRequest request)
        {
            wis r = new wis();
            var d = r.get_borrow_groupby_office();
            return Json(d.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public ActionResult get_mon([DataSourceRequest] DataSourceRequest request, string transcode)
        {
            wis r = new wis();
            var d = r.get_monitoring(transcode);
            return Json(d.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public ActionResult get_mon_all([DataSourceRequest] DataSourceRequest request)
        {
            wis r = new wis();
            var d = r.get_monitoring_all();
            return Json(d.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public ActionResult get_avail([DataSourceRequest] DataSourceRequest request, int officeid)
        {
            wis r = new wis();
            var d = r.get_availability(officeid);
            return Json(d.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public ActionResult get_list_offices()
        {
            wis r = new wis();
            var d = r.get_off();
            return Json(d, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region acceptance_inspection page
        public ActionResult acceptance_inspection()
        {
            return View();
        }
        public ActionResult Ris_approval()
        {
            return View();
        }
        public ActionResult GetItems_by_transnumber([DataSourceRequest] DataSourceRequest request, string transaction_code)
        {

            List<item> item = new List<item>();
            if (transaction_code == null)
            {
                return Json(item.ToDataSourceResult(request)); 
            }
             
                epsws.serviceSoapClient r = new epsws.serviceSoapClient();
                DataTable dt = r.GetItems(transaction_code);

              //  DataRow[] results = dt.Select("itemid = '2183'");


               // DataTable selected = dt.Clone();

               // foreach (DataRow row in results)
               // {
               //     selected.ImportRow(row);
               // }

                #region Old using lambda
                //var rec = r.GetItems(transaction_code)
                //    .AsEnumerable()
                //    .Select(b => new
                //    {
                //        itemid = b.Field<int>("itemid"),
                //        //   officeid = b.Field<int>("officeid"),
                //        // officecode = b.Field<string>("officecode"),
                //        itemname = b.Field<string>("itemname"),
                //        unit = b.Field<string>("unit"),
                //        unitcost = b.Field<decimal>("unitcost"),
                //        quantity = b.Field<decimal>("quantity")
                //    })
                //     .GroupBy(ac => new
                //     {
                //         ac.itemid,

                //         ac.unit,
                //         ac.itemname,
                //         ac.unitcost,
                //     })
                //      .Select(ac => new
                //      {
                //          //  officeid = ac.Key.officeid,
                //          // officecode = ac.Key.officecode,
                //          iquantity = ac.Sum(acs => acs.quantity),
                //          itemname = ac.Key.itemname,
                //          itemid = ac.Key.itemid,
                //          unit = ac.Key.unit,
                //          unitcost = ac.Key.unitcost,
                //      })
                //    .ToList();

                //revise_read reader = new revise_read();
                //var items = reader.read_in(transaction_code);
                //var item_in = items.ToList();

                //var new_final_items = rec.GroupJoin(
                //      item_in,
                //      x => x.itemid,
                //      y => y.itemid,
                //      (x, g) => g
                //          .Select(y => new { itemid = x.itemid, itemname = x.itemname, unit = x.unit, total_quantity = x.iquantity, unitcost = x.unitcost, recieve_quantity = y.quantity })
                //          .DefaultIfEmpty(new { itemid = x.itemid, itemname = x.itemname, unit = x.unit, total_quantity = x.iquantity, unitcost = x.unitcost, recieve_quantity = 0 }))
                //          .SelectMany(g => g);


                //List<item> item = new List<item>();
                //foreach (var it in new_final_items)
                //{

                //    item x = new item();
                //    x.itemid = it.itemid;
                //    x.itemname = it.itemname;
                //    x.unit = it.unit;
                //    x.iquantity = it.total_quantity;
                //    x.recieve_quantity = it.recieve_quantity;
                //    x.dremaining = it.total_quantity - it.recieve_quantity;
                //    x.rprice = it.unitcost;
                //    x.accepted = Convert.ToInt32(it.total_quantity - it.recieve_quantity);
                //    item.Add(x);

                //}
                #endregion

                DataTable dtNew = new DataTable();
                dtNew.Columns.Add("itemid");
                dtNew.Columns.Add("itemname");
                dtNew.Columns.Add("unit");
                dtNew.Columns.Add("unitcost");
                dtNew.Columns.Add("quantity");
                dtNew.Columns.Add("cyear");
                dtNew.Columns.Add("quarterid"); 
                dtNew.Columns.Add("objid");
                dtNew.Columns.Add("dbmbb");
                dtNew.Columns.Add("accountid");
                
                
                foreach (DataRow row in dt.Rows)
                {
                    DataRow it = dtNew.NewRow();
                    it["itemid"] = row["itemid"];
                    it["itemname"] = row["itemname"];
                    it["quantity"] = row["quantity"];
                    it["unit"] = row["unit"];
                    it["unitcost"] = row["unitcost"];
                    it["cyear"] = row["cyear"];
                    it["quarterid"] = row["quarterid"];
                    it["objid"] = row["objid"];
                    it["dbmbb"] = row["dbmbb"];
                    it["accountid"] = "336";
                    dtNew.Rows.Add(it);
                }

             
                DataTable returnDt = new DataTable();

                using (SqlConnection con = new SqlConnection(common.MyConnection()))
                {
                    SqlCommand com = new SqlCommand("InsertValue", con);
                    com.CommandType = System.Data.CommandType.StoredProcedure;

                    SqlParameter parameter = new SqlParameter();
                    parameter.SqlDbType = System.Data.SqlDbType.Structured;
                    parameter.ParameterName = "@TempTable";
                    parameter.TypeName = "dbo.ItemsTbl";
                    parameter.Value = dtNew;
                    com.Parameters.Add(parameter);

                    SqlParameter parameter2 = new SqlParameter();
                    parameter2.ParameterName = "@transaction_code";
                    parameter2.SqlDbType = SqlDbType.NVarChar;
                    parameter2.Size = 8000;
                    parameter2.Value = transaction_code;
                    com.Parameters.Add(parameter2);

                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                   // returnDt.Load(reader);
                    while (reader.Read())
                    {
                        item x = new item();

                        x.itemid = (Convert.IsDBNull(reader["itemid"]) ? 0 : (int)(reader["itemid"]));
                        x.itemname = reader["itemname"] == DBNull.Value ? (string)null : (string)reader["itemname"];
                        x.unit = reader["unit"] == DBNull.Value ? (string)null : (string)reader["unit"];
                        x.iquantity = (Convert.IsDBNull(reader["quantity"]) ? 0 : (double)(reader["quantity"]));
                        x.recieve_quantity = (Convert.IsDBNull(reader["total_in"]) ? 0 : (double)(reader["total_in"]));
                        x.dremaining = (Convert.IsDBNull(reader["remaining"]) ? 0 : (double)(reader["remaining"]));
                        x.rprice = (Convert.IsDBNull(reader["unitcost"]) ? 0 : (decimal)(reader["unitcost"]));
                        x.accepted = (Convert.IsDBNull(reader["accepted"]) ? 0 : (double)(reader["accepted"]));
                        x.year = (Convert.IsDBNull(reader["year"]) ? 0 : (int)(reader["year"]));
                        x.quarter = (Convert.IsDBNull(reader["quarterid"]) ? 0 : (int)(reader["quarterid"])); 
                        x.mooe_no = (Convert.IsDBNull(reader["objid"]) ? 0 : (int)(reader["objid"]));
                        x.dbm_bb = (Convert.IsDBNull(reader["dbmbb"]) ? 0 : (int)(reader["dbmbb"]));
                        //x.accountid = (Convert.IsDBNull(reader["accountid"]) ? 0 : (int)(reader["accountid"]));
                        item.Add(x);
                    }
                }
                return Json(item.ToDataSourceResult(request)); 
            
        }
        public string entry_revise(IEnumerable<item> items, string transcode , int from )
        {
            revise_read r = new revise_read();
            return r.revise_in(items, transcode , from);
        }
        public string entry_existing(item items)
        {
            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));

            String dy = datevalue.Day.ToString();
            String mn = datevalue.Month.ToString();
            String yy = datevalue.Year.ToString();
            var eid = USER.C_eID;
            string query ="";
            var transno = "";

            //var cnt_transcode = (@"select count(transcode) from  [IMS].[dbo].[tbl_wis_transaction] where  year = '" + items.year + "' and quarter = '" + items.quarter + "' and mooe_no = '" + items.mooe_no + "' and dbm_bb = '" + items.dbm_bb + "' ").Scalar();


            //if (cnt_transcode > 0)
            //{
            //    transno = (@"select distinct(transcode) from  [IMS].[dbo].[tbl_wis_transaction] where  year = '" + items.year + "' and quarter = '" + items.quarter + "' and mooe_no = '" + items.mooe_no + "' and dbm_bb = '" + items.dbm_bb + "' ").ScalarString();
            //}
            //else
            //{
                var myquery = (@"exec Proc_ManualTransNo '[IMS].[dbo].[tbl_generate_trans_code]','MAN','new_transcode','" + yy + "','" + mn + "'").ScalarString();
                transno = myquery;
            //}



            query += "insert into [IMS].[dbo].[tbl_wis_transaction] (eid,itemid,itemname,date,quantity,in_out,unit,unitcost,transcode,year,quarter,mooe_no,dbm_bb,accountid,[istire],[po_id]) values ('" + eid + "','" + items.itemid + "','" + items.itemname.Replace("'", "''") + "','" + DateTime.Now.ToString("MM-dd-yyyy") + "','" + items.quantity + "','IN','" + items.unit + "','" + items.rprice + "','"+transno.Trim()+"','" + items.year + "','" + items.quarter + "','" + items.mooe_no + "','" + items.dbm_bb + "' , 336,'"+items.istire+"','"+items.prnumber+"') select SCOPE_IDENTITY() ;";

            //var check_count = (@"select count(itemid) from [IMS].[dbo].[tbl_wis_transaction] where transcode like '%MAN%' and itemid = '"+items.itemid+"' and  year = '"+items.year+"' and quarter = '"+items.quarter+"' and mooe_no = '"+items.mooe_no+"' and dbm_bb = '"+items.dbm_bb+"'").Scalar();

            //if (check_count > 0)
            //{
            //    return "Item is Already exist , Enter new Quantity";
            //}

            try
            {
                //if (cnt_transcode > 0)
                //{
                //    var id = (query).Scalar();
                 
                //    return transno;
                //}
                //else
                //{
                    var id = (query).Scalar();
                    (@"insert into [IMS].[dbo].[tbl_generate_trans_code] values ('','"+transno.Trim()+"','" + DateTime.Today.ToString("MM-dd-yyyy") + "')").NonQuery();
                    return transno;
                //}
            }
            catch(Exception)
            {
                return "0";
            }
        }

        public ActionResult get_gen_transcode(int year,int quarter,int mooe_no,int dbm_bb)
        {
            revise_read r = new revise_read();
            var data = r.get_gen_transcode(year,quarter,mooe_no,dbm_bb);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult get_office_items_gen_transcode(IEnumerable enumerable, [DataSourceRequest] DataSourceRequest request, string tcode)
        {
            revise_read r = new revise_read();
            var data = r.get_office_items_gen_transcode(tcode);
            return Json(data.ToDataSourceResult(request));

          //DataTable dt = (@"select * from [IMS].[dbo].[tbl_items_per_transaction_code] where transaction_code = '" + transcode + "'").DataSet();
          //  var serializer = new JavaScriptSerializer();
          //  var result = new ContentResult();
          //  serializer.MaxJsonLength = Int32.MaxValue;
          //  result.Content = serializer.Serialize(dt.ToDataSourceResult(request));
          //  result.ContentType = "application/json";  
          //  return result;
        }
        public ActionResult get_office_received([DataSourceRequest] DataSourceRequest request, string code , int itemid)
        {
            revise_read r = new revise_read();
            var data = r.get_office_received(code, itemid);
            return Json(data.ToDataSourceResult(request)); 
        }

        public ActionResult update_office_items_gen_transcode([DataSourceRequest]DataSourceRequest request,item items )
        {
            string check_max = "select quantity from [IMS].[dbo].[tbl_wis_transaction] where transcode = '"+items.transcode+"' and in_out = 'IN'";
            string check_total = "select sum(quantity) as total from [IMS].[dbo].[tbl_items_per_transaction_code] where transaction_code = '"+items.transcode+"'";
            int actual_quantity = (@"select quantity from [IMS].[dbo].[tbl_items_per_transaction_code] where id = '"+items.id+"'").Scalar();

            var max =  (check_max).Scalar();
            var total = (check_total).Scalar();
            var allow = max - total ;

            int posible_quantity =  actual_quantity + allow;

             
            if (posible_quantity < items.quantity )
            {
                return null;
            }
            else 
            {
                string query2 = "Update [IMS].[dbo].[tbl_items_per_transaction_code] set quantity = '" + items.quantity + "' where id = '" + items.id + "'";
                string strcon = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();
                string result1 = OleDbHelper.ExecuteNonQuery(strcon, System.Data.CommandType.Text, query2).ToString();
            }
            return Json(new[] { items }.ToDataSourceResult(request, ModelState)); 
        }
        public ActionResult destroy_office_items_gen_transcode([DataSourceRequest]DataSourceRequest request, item items)
        {
            string query2 = "delete from [IMS].[dbo].[tbl_items_per_transaction_code] where id = '"+items.id+"' ";
            string strcon = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();
            string result1 = OleDbHelper.ExecuteNonQuery(strcon, System.Data.CommandType.Text, query2).ToString();
            return Json(new[] { items }.ToDataSourceResult(request, ModelState));
        }
        public string submited_allocated_gen_trans(IEnumerable<item> items, int year, int quarter, int mooe_no, int dbm_bb)
        {
            var total_sum = 0;
            var total_allocate = 0;
            var itemid = 0 ; 
            var officeid = 0;
            var officename = "";
            var quantity = 0.0;
            var transcode = "";
            var itemname = "";
            var unit = ""; 
            decimal rprice = 00;
            foreach (var i in items)
            {
                itemid = i.itemid;
                 quantity = i.quantity;
                 officeid = i.officeid;
                 officename = i.officename;
                 transcode = i.transcode;
                 itemname = i.itemname;
                 unit = i.unit;
                 rprice = i.rprice;
                 transcode = i.transcode;
            }
             
            DataTable items_transcode = (@"select tbl_1.* , tbl_2.itemid , isnull(tbl_2.quantity_out,0) as quantity_out,tbl_2.transaction_code from (select itemid,itemname,quantity,transcode from  [IMS].[dbo].[tbl_wis_transaction] where transcode like '%MAN%' and itemid = '" + itemid + "' and year = '" + year + "' and quarter = '" + quarter + "' and mooe_no = '" + mooe_no + "' and dbm_bb = '" + dbm_bb + "' group by itemid,itemname,quantity,transcode) tbl_1 left join (select itemid,sum(quantity) as quantity_out,transaction_code from [IMS].[dbo].[tbl_items_per_transaction_code] group by itemid,transaction_code) tbl_2 on tbl_1.itemid = tbl_2.itemid and tbl_1.transcode = tbl_2.transaction_code").DataSet();

            foreach (DataRow r in items_transcode.Rows)
            {
                total_sum +=  Convert.ToInt32(r["quantity"].ToString());
                total_allocate += Convert.ToInt32(r["quantity_out"].ToString());
            }

            if (quantity > total_sum)
            {
                return "Only '" + total_sum + "' is Allowed !";
            }

            var remain_to_allocate = total_sum - total_allocate;

            if (quantity == 0 || quantity < 0)
            {
                return "Only '" + remain_to_allocate + "' is Allowed !";
            }
            if (quantity > remain_to_allocate)
            {
                return "Only '" + remain_to_allocate + "' is Allowed !";
            }
            string qry = "";
            qry = "insert into [IMS].[dbo].[tbl_items_per_transaction_code] values ('" +officeid + "','" +officename.Replace("'", "''") + "','" + quantity + "','" +transcode + "','" +itemname.Replace("'", "''") + "','" +unit + "','" +itemid + "','" + rprice + "',0,0,'','',336)";
            try
            {
                 (qry).NonQuery();
                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public ActionResult get_office_pmis()
        {
            string qry = @"select pmisOfficeID,CONCAT(RTRIM(LTRIM(OfficeName)),RTRIM(LTRIM('(')),RTRIM(LTRIM(Abbr)),RTRIM(LTRIM(')'))) as OfficeName from fmis.dbo.tblREF_AIS_Offices";
            DataTable dt = qry.DataSet();

            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }

        public ActionResult get_itemsByCode(string code)
        {
            string qry = @"select distinct a.itemid ,a.itemname , b.quantity,b.transcode,b.unit from [IMS].[dbo].[tbl_wis_transaction]  as a
  inner join ( select itemid , sum(quantity) as quantity, transcode , unit from [IMS].[dbo].[tbl_wis_transaction] where  in_out = 'IN' and transcode = '"+code+"' and starting_balance = 2 group by itemid, transcode , unit) as b on a.itemid = b.itemid ";
            DataTable dt = qry.DataSet();

            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }
        public ActionResult getAllocated(string t_code, string itemid, string officeid)
        {
            string query = @"select itemid,sum(quantity) as quantity FROM [IMS].[dbo].[tbl_items_per_transaction_code] 
  where transaction_code = '" + t_code + "' and itemid = '" + itemid + "' and officeid = '" + officeid + "' group by itemid";
 
            DataTable dt = query.DataSet();

            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }


        public string submit_office_received(string code , int received ,int officeid ,string  officename , int itemid , string unit ,double price , string itemname )
        {

            var error_items = "";

                #region loop
            try
            {
                var row_number = 0;
                var cn = ""; 
                var request_quantity = received;

                #region check_available
                DataTable chck_total_avail = (@"SELECT  t.itemid,mo.total_available,t.itemname
 FROM
(select  itemid,sum(available) as total_available from [IMS].[dbo].[view_items_where_to_get] where itemid = '" + itemid + "' group by itemid ) mo  CROSS APPLY ( SELECT  TOP 1 * FROM    [IMS].[dbo].[tbl_items_per_transaction_code] mi WHERE   mi.itemid = mo.itemid ) t").DataSet();
                var total_avail = Convert.ToInt32(chck_total_avail.Rows[row_number]["total_available"].ToString());

                if (total_avail < request_quantity)
                {
                    return "Ops Remaining Quantity for'" + chck_total_avail.Rows[row_number]["itemname"].ToString() + "' is only '" + total_avail + "' ";
                }

                DataTable dt = (@"select * from [IMS].[dbo].[view_items_where_to_get] where itemid = '" + itemid + "' and available != 0 and available !< 0  order by id").DataSet();

                var initial_available = dt.Rows[row_number]["available"].ToString();
                var int_available = Convert.ToInt32(initial_available);
                #endregion

                if (request_quantity > int_available)
                {
                    var total_pass = 0;
                    var y = request_quantity;
                    for (var x = 0; request_quantity != total_pass; x++)
                    {
                        #region data
                        DataTable dt1 = (@"select * from [IMS].[dbo].[view_items_where_to_get] where itemid = '" + itemid + "' and available != 0 order by id").DataSet();

                        var dt_itemid = dt.Rows[row_number]["itemid"].ToString();
                        var dt_itemname = dt.Rows[row_number]["itemname"].ToString();
                        var dt_unitcost = dt.Rows[row_number]["unitcost"].ToString();
                        var dt_unit = dt.Rows[row_number]["unit"].ToString();
                        var dt_transcode = dt.Rows[row_number]["transcode"].ToString();
                        var dt_available = dt.Rows[row_number]["available"].ToString();
                        var _available = Convert.ToInt32(dt_available);
                        var ref_id = dt.Rows[row_number]["id"].ToString();
                        #endregion

                        if (_available > y)
                        {
                            try
                            {
                                string qry = @"insert into [IMS].[dbo].[wis_ris_transactions] (officeid,officename,itemid,itemname,unit,quantity,price,date_submited,trans_code,controlno,ref_id) values ('" + officeid + "','" + officename + "','" + dt_itemid + "','" + dt_itemname + "','" + dt_unit + "','" + y + "','" + dt_unitcost + "','" + DateTime.Now.ToString("MM-dd-yyyy") + "','" + dt_transcode + "','','" + ref_id + "')";
                                string con = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();
                                string result = OleDbHelper.ExecuteNonQuery(con, System.Data.CommandType.Text, qry).ToString();

                            }
                            catch(Exception ex)
                            { 
                                error_items += " '" + dt_itemname + "' ";
                            } 
                            total_pass += y;
                        }
                        else
                        {
                            try
                            {
                                string qry = @"insert into [IMS].[dbo].[wis_ris_transactions] (officeid,officename,itemid,itemname,unit,quantity,price,date_submited,trans_code,controlno,ref_id) values ('" + officeid + "','" + officename + "','" + dt_itemid + "','" + dt_itemname + "','" + dt_unit + "','" + _available + "','" + dt_unitcost + "','" + DateTime.Now.ToString("MM-dd-yyyy") + "','" + dt_transcode + "','','" + ref_id + "')";
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
                }
                else if (request_quantity <= int_available)
                {
                    try
                    {
                        string qry = @"insert into [IMS].[dbo].[wis_ris_transactions] (officeid,officename,itemid,itemname,unit,quantity,price,date_submited,trans_code,controlno,ref_id) values ('" +officeid+ "','" + officename+ "','" + dt.Rows[0]["itemid"] + "','" + dt.Rows[0]["itemname"].ToString().Replace("'", "''") + "','" + dt.Rows[0]["unit"] + "','" + request_quantity + "','" + dt.Rows[0]["unitcost"] + "','" + DateTime.Now.ToString("MM-dd-yyyy") + "','" + dt.Rows[0]["transcode"] + "','','" + dt.Rows[0]["id"].ToString() + "')";
                        string con = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();
                        string result = OleDbHelper.ExecuteNonQuery(con, System.Data.CommandType.Text, qry).ToString();

                    }
                    catch
                    {
                        error_items += " '" + dt.Rows[0]["itemname"].ToString() + "' ";
                    } 
                }
            }
            catch (Exception ex)
            {
                return "" + ex;
            }
                 
                #endregion
            
            try
            {
                int val = 2;
                string query = "insert into  [IMS].[dbo].[tbl_wis_transaction] (eid,itemid,itemname,date,quantity,unitcost,officeid,in_out,unit ,transcode,officename,starting_balance) values ('" + USER.C_eID + "','" + itemid + "','" + itemname + "' , '" + DateTime.Now.ToString("MM-dd-yyyy") + "' , '" + received + "' , '" + price + "','" + officeid + "','OUT', '" + unit + "','" + code.Replace(" ", "") + "' ,'" + officename.Replace("'","''") + "' , '"+val+"' )";
                (query).NonQuery();
                return "1";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public ActionResult items_received_per_office([DataSourceRequest] DataSourceRequest request, string transno, int officeid)
        {
            revise_read r = new revise_read();
            var data = r.items_received_per_office(transno,officeid);
            return Json(data.ToDataSourceResult(request));
        }

        public string  office_received(IEnumerable<item> items, int officeid , string transno, string officename)
        {
            var error_items = "";

            DataTable ris_items = new DataTable();
            ris_items.Columns.Add("itemid");
            ris_items.Columns.Add("itemname");
            ris_items.Columns.Add("quantity");
            ris_items.Columns.Add("unit");
            ris_items.Columns.Add("unitcost");
            ris_items.Columns.Add("transcode");
            ris_items.Columns.Add("date");
            ris_items.Columns.Add("controlno");



            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));

            String dy = datevalue.Day.ToString();
            String mn = datevalue.Month.ToString();
            String yy = datevalue.Year.ToString();
            String control_number = "manual";
            /*
            string query1 = "exec Proc_GetNewTranNo_ris '[IMS].[dbo].[tbl_t_ris_preparation]','RIS','controlno','" + yy + "','" + mn + "'";
            string strcon = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();
            string control_number = OleDbHelper.ExecuteScalar(strcon, System.Data.CommandType.Text, query1).ToString();

            string query2 = "insert into [IMS].[dbo].[tbl_t_ris_preparation] (controlno,date_time,officeid,officename,preparedby) values ('" + control_number.Trim() + "','" + DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss") + "','" + USER.C_officeID + "','" + USER.C_Office + "','" + USER.C_eID + "')";
            string strcon2 = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();
            string result1 = OleDbHelper.ExecuteNonQuery(strcon2, System.Data.CommandType.Text, query2).ToString();
          */
             
            try
            {
                #region loop
                foreach (var i in items)
                {
                    var cn = control_number.Trim();
                    var row_number = 0;
                    var itemid = i.itemid;
                    var request_quantity = i.request_quantity;

                    #region check_available
                    DataTable chck_total_avail = (@"SELECT  itemid,SUM(CASE WHEN in_out = 'IN' THEN quantity ELSE 0 END) - SUM(CASE WHEN in_out = 'OUT' THEN quantity ELSE 0 END) 
AS total_available, itemname 
 FROM [IMS].[dbo].[tbl_wis_transaction]  where transcode = '" + transno + "' and itemid = '" + itemid + "' and starting_balance = 2 group by itemid,itemname").DataSet();
                    var total_avail = Convert.ToInt32(chck_total_avail.Rows[row_number]["total_available"].ToString());

                    if (total_avail < request_quantity)
                    {
                        return "Ops Remaining Quantity for'" + chck_total_avail.Rows[row_number]["itemname"].ToString() + "' is only '" + total_avail + "' ";
                    }

                    DataTable dt = (@"select * from [IMS].[dbo].[vw_where_to_get_existing_item] where transcode = '"+transno+"' and itemid = '" + itemid + "' and available != 0 and available !< 0  order by id").DataSet();

                    var initial_available = dt.Rows[row_number]["available"].ToString();
                    var int_available = Convert.ToInt32(initial_available);

                    #endregion

                    if (request_quantity > int_available)
                    {
                        var total_pass = 0.0;
                        var y = request_quantity;
                        for (var x = 0; request_quantity != total_pass; x++)
                        {
                            #region data
                            DataTable dt1 = (@"select * from [IMS].[dbo].[vw_where_to_get_existing_item] where transcode = '" + transno + "' and itemid = '" + itemid + "' and available != 0 and available !< 0  order by id").DataSet();

                            var dt_itemid = dt.Rows[row_number]["itemid"].ToString();
                            var dt_itemname = dt.Rows[row_number]["itemname"].ToString();
                            var dt_unitcost = dt.Rows[row_number]["unitcost"].ToString();
                            var dt_unit = dt.Rows[row_number]["unit"].ToString();
                            var dt_transcode = dt.Rows[row_number]["transcode"].ToString();
                            var dt_available = dt.Rows[row_number]["available"].ToString();
                            var _available = Convert.ToInt32(dt_available);
                            var ref_id = dt.Rows[row_number]["id"].ToString();
                            #endregion

                            if (_available > y)
                            {
                                try
                                {
                                string qry = @"insert into [IMS].[dbo].[wis_ris_transactions] (officeid,officename,itemid,itemname,unit,quantity,price,date_submited,trans_code,controlno,ref_id) values ('" + officeid + "','" + officename.Replace("'", "''") + "','" + dt_itemid + "','" + dt_itemname.Replace("'", "''") + "','" + dt_unit + "','" + y + "','" + dt_unitcost + "','" + DateTime.Now.ToString("MM-dd-yyyy") + "','" + dt_transcode + "','" + cn + "','" + ref_id + "') select SCOPE_IDENTITY()";
                                string con = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();
                                string result = OleDbHelper.ExecuteScalar(con, System.Data.CommandType.Text, qry).ToString();

                                /* Insert to WIS transaction table */
                                string trans_qry = @"insert into [IMS].[dbo].[tbl_wis_transaction] (eid,itemid,itemname,date,quantity,in_out,unit,unitcost,transcode,officeid,ref_id_wrt) values ('" + USER.C_eID + "','" + dt_itemid + "','" + dt_itemname.Replace("'", "''") + "','" + DateTime.Now.ToString("MM-dd-yyyy") + "','" + y + "','OUT','" + dt_unit + "','" + dt_unitcost + "','" + dt_transcode + "','" + officeid + "','" + result + "');update [IMS].[dbo].[wis_ris_transactions] set status = 1 where id = '" + result + "';";

                                 string gg = OleDbHelper.ExecuteNonQuery(con, System.Data.CommandType.Text, trans_qry).ToString();


                                }
                                catch
                                {
                                    error_items += " '" + dt_itemname + "' ";
                                }


                                DataRow it = ris_items.NewRow();
                                it["itemid"] = dt_itemid;
                                it["itemname"] = dt_itemname;
                                it["quantity"] = y;
                                it["unit"] = dt_unit;
                                it["unitcost"] = dt_unitcost;
                                it["transcode"] = dt_transcode;
                                it["date"] = DateTime.Now;
                                ris_items.Rows.Add(it);
                                total_pass += y;
                            }
                            else
                            {
                                try
                                {
                                    string qry = @"insert into [IMS].[dbo].[wis_ris_transactions] (officeid,officename,itemid,itemname,unit,quantity,price,date_submited,trans_code,controlno,ref_id) values ('" + officeid + "','" + officename.Replace("'", "''") + "','" + dt_itemid + "','" + dt_itemname.Replace("'", "''") + "','" + dt_unit + "','" + _available + "','" + dt_unitcost + "','" + DateTime.Now.ToString("MM-dd-yyyy") + "','" + dt_transcode + "','" + cn + "','" + ref_id + "') select SCOPE_IDENTITY()";
                                    string con = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();
                                    string result = OleDbHelper.ExecuteScalar(con, System.Data.CommandType.Text, qry).ToString();


                                    string trans_qry = @"insert into [IMS].[dbo].[tbl_wis_transaction] (eid,itemid,itemname,date,quantity,in_out,unit,unitcost,transcode,officeid,ref_id_wrt) values ('" + USER.C_eID + "','" + dt_itemid + "','" + dt_itemname.Replace("'", "''") + "','" + DateTime.Now.ToString("MM-dd-yyyy") + "','" + _available + "','OUT','" + dt_unit + "','" + dt_unitcost + "','" + dt_transcode + "','" + officeid + "','"+result+"');update [IMS].[dbo].[wis_ris_transactions] set status = 1 where id = '" + result + "';";

                                    string gg = OleDbHelper.ExecuteNonQuery(con, System.Data.CommandType.Text, trans_qry).ToString();
                                }
                                catch
                                {
                                    error_items += " '" + dt_itemname + "' ";
                                }
                                DataRow it = ris_items.NewRow();
                                it["itemid"] = dt_itemid;
                                it["itemname"] = dt_itemname;
                                it["quantity"] = _available;
                                it["unit"] = dt_unit;
                                it["unitcost"] = dt_unitcost;
                                it["transcode"] = dt_transcode;
                                it["date"] = DateTime.Now;
                                ris_items.Rows.Add(it);
                                y -= _available;
                                total_pass += _available;
                                row_number += 1;
                            }
                        }
                    }
                    else if (request_quantity <= int_available)
                    {
                        try
                        {
                            string qry = @"insert into [IMS].[dbo].[wis_ris_transactions] (officeid,officename,itemid,itemname,unit,quantity,price,date_submited,trans_code,controlno,ref_id) values ('" + officeid + "','" + officename.Replace("'", "''") + "','" + dt.Rows[0]["itemid"] + "','" + dt.Rows[0]["itemname"].ToString().Replace("'", "''") + "','" + dt.Rows[0]["unit"] + "','" + request_quantity + "','" + dt.Rows[0]["unitcost"] + "','" + DateTime.Now.ToString("MM-dd-yyyy") + "','" + dt.Rows[0]["transcode"] + "','" + cn + "','" + dt.Rows[0]["id"].ToString() + "')  select SCOPE_IDENTITY()";
                            string con = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();
                            string result = OleDbHelper.ExecuteScalar(con, System.Data.CommandType.Text, qry).ToString();


                            string trans_qry = @"insert into [IMS].[dbo].[tbl_wis_transaction] (eid,itemid,itemname,date,quantity,in_out,unit,unitcost,transcode,officeid,ref_id_wrt) values ('" + USER.C_eID + "','" + dt.Rows[0]["itemid"] + "','" + dt.Rows[0]["itemname"].ToString().Replace("'", "''") + "','" + DateTime.Now.ToString("MM-dd-yyyy") + "','" + request_quantity + "','OUT','" + dt.Rows[0]["unit"] + "','" + dt.Rows[0]["unitcost"] + "','" + dt.Rows[0]["transcode"] + "','" + officeid + "','" + result + "');update [IMS].[dbo].[wis_ris_transactions] set status = 1 where id = '" + result + "';";

                            string gg = OleDbHelper.ExecuteNonQuery(con, System.Data.CommandType.Text, trans_qry).ToString();

                        }
                        catch
                        {
                            error_items += " '" + dt.Rows[0]["itemname"].ToString() + "' ";
                        }

                        DataRow it1 = ris_items.NewRow();
                        it1["itemid"] = dt.Rows[0]["itemid"];
                        it1["itemname"] = dt.Rows[0]["itemname"];
                        it1["quantity"] = request_quantity;
                        it1["unit"] = dt.Rows[0]["unit"];
                        it1["unitcost"] = dt.Rows[0]["unitcost"];
                        it1["transcode"] = dt.Rows[0]["transcode"];
                        it1["date"] = DateTime.Now;
                        ris_items.Rows.Add(it1);
                    }
                }
                if (error_items == "")
                {
                    return "1";
                }
                else
                {
                    return "Transaction Finish with Error on items '" + error_items + "'";
                }
                #endregion
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            #region hide
            
            #endregion
        }

        public string update_quantity(int itemid, int year, int quarter, int mooe_no, int dbm_bb, int quantity)
        {
            string qry = "update [IMS].[dbo].[tbl_wis_transaction] set quantity = '" + quantity + "' where transcode like '%MAN%' and  itemid = '" + itemid + "' and year = '" + year + "' and quarter = '" + quarter + "' and mooe_no = '" + mooe_no + "' and dbm_bb = '" + dbm_bb + "' ";

            try
            {
                (qry).NonQuery();
                return "1";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }

        public ActionResult edit_grid_quantity([DataSourceRequest] DataSourceRequest request)
        {
            revise_read r = new revise_read();
            var d = r.edit_quantity();
            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            result.Content = serializer.Serialize(d.ToDataSourceResult(request));
            result.ContentType = "application/json";


            return result;
        }
        public string update_grid_quantity(item items)
        {
            //this is out quantity
            double out_quantity = items.recieve_quantity;
            //this is old quantity
            int actual_quantity = (@"select quantity from [IMS].[dbo].[tbl_wis_transaction]  where id = '" + items.id + "'").Scalar();


            //this is new quantity
            double quantity = items.quantity;

            if (quantity < out_quantity)
            {
                return null;
            }
            else
            {
                (@"update [IMS].[dbo].[tbl_wis_transaction] set quantity = '" + quantity + "' where id = '" + items.id + "' ").NonQuery();
                return "1";
            } 
        }
        public string delete_grid_quantity(int id)
        {
            (@"delete from [IMS].[dbo].[tbl_wis_transaction] where id = '" + id + "'").NonQuery();
            return "1";
        }

        public ActionResult readItemsWithReorder([DataSourceRequest] DataSourceRequest request)
        {
            read r = new read();
            var d = r.readItemsWithReorderPGSO();
            return Json(d.ToDataSourceResult(request));
        }
        public string updateItemsWithReorder(item items)
        {
            string ret = ""; 
            var exist = (@"select count(*) from  [IMS].[dbo].[tbl_l_reorder] where officeid = 0 and itemid = '" + items.itemid + "'").Scalar();

            if (exist > 0)
            {
                (@"update [IMS].[dbo].[tbl_l_reorder] set reorder_point = '" + items.quantity + "' where itemid = '" + items.itemid + "' ").NonQuery();
                ret = "1";
            }
            else
            {
                (@"insert into [IMS].[dbo].[tbl_l_reorder]  values ('" + items.itemid + "','" + items.quantity + "','0') ").NonQuery();
                ret = "1";
            } 
            return ret;
        } 

        #endregion

        #region SAI page
        public ActionResult Sai_view()
        {
            return  View();
        }
        public PartialViewResult partialSai_view()
        {
            return PartialView("Sai_view");
        }
        public ActionResult for_SAI([DataSourceRequest] DataSourceRequest request,int year,int quarter,int mooe_no, int dbm_bb, int accountid)
        {
            revise_read r = new revise_read();
            var data = r.get_sai(year,quarter,mooe_no,dbm_bb,accountid);
            return Json(data.ToDataSourceResult(request));
        }
        public ActionResult get_available_delivery([DataSourceRequest] DataSourceRequest request)
        {
            read r = new read();
            var data = r.available_delivery();
            return Json(data.ToDataSourceResult(request)); 
        }

        #region
        public string submit_request(IEnumerable<item> items,int year,int quarter,int mooe_no,int dbm_bb,string tc)
        {
            var error_items = "";

            DataTable ris_items = new DataTable();
            ris_items.Columns.Add("itemid");
            ris_items.Columns.Add("itemname");
            ris_items.Columns.Add("quantity");
            ris_items.Columns.Add("unit");
            ris_items.Columns.Add("unitcost");
            ris_items.Columns.Add("transcode");
            ris_items.Columns.Add("date");
            ris_items.Columns.Add("controlno");



            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));

            String dy = datevalue.Day.ToString();
            String mn = datevalue.Month.ToString();
            String yy = datevalue.Year.ToString();

            string query1 = "exec Proc_GetNewTranNo_ris '[IMS].[dbo].[tbl_t_ris_preparation]','RIS','controlno','" + yy + "','" + mn + "'";
            string strcon = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();
            string control_number = OleDbHelper.ExecuteScalar(strcon, System.Data.CommandType.Text, query1).ToString();

            string query2 = "insert into [IMS].[dbo].[tbl_t_ris_preparation] (controlno,date_time,officeid,officename,preparedby) values ('"+ control_number.Trim()+ "','" + DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss") + "','" + USER.C_officeID + "','" + USER.C_Office.Replace("'","''") + "','"+USER.C_eID+"')";
            string strcon2 = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();
            string result1 = OleDbHelper.ExecuteNonQuery(strcon2, System.Data.CommandType.Text, query2).ToString();
             
            try
            {
                #region loop
                foreach (var i in items)
                {
                    var cn = control_number.Trim();
                    var row_number = 0;
                    var itemid = i.itemid;
                    var request_quantity = i.request_quantity;

                    #region check_available
                    DataTable chck_total_avail = (@"SELECT  t.itemid,mo.total_available,t.itemname
 FROM (select  itemid,sum(available) as total_available,year,quarter,mooe_no,dbm_bb,transcode from [IMS].[dbo].[view_items_where_to_get] where itemid = '" + itemid + "' group by itemid,year,quarter,mooe_no,dbm_bb,transcode ) mo  CROSS APPLY ( SELECT  TOP 1 * FROM    [IMS].[dbo].[tbl_items_per_transaction_code] mi WHERE   mi.itemid = mo.itemid ) t where mo.year = '" + year + "'  and mo.quarter = '" + quarter + "' and mo.mooe_no = '" + mooe_no + "' and mo.dbm_bb = '" + dbm_bb + "' and mo.transcode = '" + tc + "' ").DataSet();
                    var total_avail = Convert.ToInt32(chck_total_avail.Rows[row_number]["total_available"].ToString());

                    if (total_avail < request_quantity)
                    {
                        return "Ops Remaining Quantity for'" + chck_total_avail.Rows[row_number]["itemname"].ToString() + "' is only '"+total_avail+"' ";
                    }

                    DataTable dt = (@"select * from [IMS].[dbo].[view_items_where_to_get] where itemid = '" + itemid + "' and available != 0 and available !< 0 and year ='" + year + "'  and quarter = '" + quarter + "' and mooe_no = '" + mooe_no + "' and dbm_bb = '" + dbm_bb + "' and transcode = '"+tc+"' ").DataSet();

                    var initial_available = dt.Rows[row_number]["available"].ToString();
                    var int_available = Convert.ToInt32(initial_available);
                    #endregion

                    if (request_quantity > int_available)
                    {
                        var total_pass = 0.0;
                        var y = request_quantity;
                        for (var x = 0; request_quantity != total_pass; x++)
                        {
                            #region data
                            DataTable dt1 = (@"select * from [IMS].[dbo].[view_items_where_to_get] where itemid = '" + itemid + "' and available != 0 and year ='" + year + "'  and quarter = '" + quarter + "' and mooe_no = '" + mooe_no + "' and dbm_bb = '" + dbm_bb + "' and  transcode = '"+tc+"'").DataSet();

                            var dt_itemid = dt.Rows[row_number]["itemid"].ToString();
                            var dt_itemname = dt.Rows[row_number]["itemname"].ToString();
                            var dt_unitcost = dt.Rows[row_number]["unitcost"].ToString();
                            var dt_unit = dt.Rows[row_number]["unit"].ToString();
                            var dt_transcode = dt.Rows[row_number]["transcode"].ToString();
                            var dt_available = dt.Rows[row_number]["available"].ToString();
                            var _available = Convert.ToInt32(dt_available);
                            //var ref_id = dt.Rows[row_number]["id"].ToString();
                            #endregion

                            if (_available > y)
                            {
                                try
                                {
                                    string qry = @"insert into [IMS].[dbo].[wis_ris_transactions](officeid,officename,itemid,itemname,unit,quantity,price,date_submited,trans_code,controlno) values ('" + USER.C_officeID + "','" + USER.C_Office.Replace("'","''")+ "','" + dt_itemid + "','" + dt_itemname.Replace("'","''") + "','" + dt_unit + "','" + y + "','" + dt_unitcost + "','" + DateTime.Now.ToString("MM-dd-yyyy") + "','" + dt_transcode + "','" + cn + "')";
                                    string con = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();
                                    string result = OleDbHelper.ExecuteNonQuery(con, System.Data.CommandType.Text, qry).ToString();

                                }
                                catch 
                                {
                                    error_items+= " '"+dt_itemname+"' ";
                                }
                                

                                DataRow it = ris_items.NewRow();
                                it["itemid"] = dt_itemid;
                                it["itemname"] = dt_itemname;
                                it["quantity"] = y;
                                it["unit"] = dt_unit;
                                it["unitcost"] = dt_unitcost;
                                it["transcode"] = dt_transcode;
                                it["date"] = DateTime.Now;
                                ris_items.Rows.Add(it);
                                total_pass += y;
                            }
                            else
                            {
                                try
                                {
                                    string qry = @"insert into [IMS].[dbo].[wis_ris_transactions] (officeid,officename,itemid,itemname,unit,quantity,price,date_submited,trans_code,controlno) values ('" + USER.C_officeID + "','" + USER.C_Office.Replace("'","''") + "','" + dt_itemid + "','" + dt_itemname.Replace("'","''") + "','" + dt_unit + "','" + _available + "','" + dt_unitcost + "','" + DateTime.Now.ToString("MM-dd-yyyy") + "','" + dt_transcode + "','"+cn+"')";
                                    string con = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();
                                    string result = OleDbHelper.ExecuteNonQuery(con, System.Data.CommandType.Text, qry).ToString();

                                }
                                catch
                                {
                                    error_items += " '" + dt_itemname + "' ";
                                }
                                DataRow it = ris_items.NewRow();
                                it["itemid"] = dt_itemid;
                                it["itemname"] = dt_itemname;
                                it["quantity"] = _available;
                                it["unit"] = dt_unit;
                                it["unitcost"] = dt_unitcost;
                                it["transcode"] = dt_transcode;
                                it["date"] = DateTime.Now;
                                ris_items.Rows.Add(it);
                                y -= _available;
                                total_pass += _available;
                                row_number += 1;
                            }
                        }
                    }
                    else if (request_quantity <= int_available)
                    {
                        try
                        {
                            string qry = @"insert into [IMS].[dbo].[wis_ris_transactions] (officeid,officename,itemid,itemname,unit,quantity,price,date_submited,trans_code,controlno) values ('" + USER.C_officeID + "','" + USER.C_Office.Replace("'","''") + "','" + dt.Rows[0]["itemid"] + "','" + dt.Rows[0]["itemname"].ToString().Replace("'","''") + "','" + dt.Rows[0]["unit"] + "','" + request_quantity + "','" + dt.Rows[0]["unitcost"] + "','" + DateTime.Now.ToString("MM-dd-yyyy") + "','" + dt.Rows[0]["transcode"] + "','" + cn + "')";
                            string con = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();
                            string result = OleDbHelper.ExecuteNonQuery(con, System.Data.CommandType.Text, qry).ToString();

                        }
                        catch
                        {
                            error_items += " '" + dt.Rows[0]["itemname"].ToString()+ "' ";
                        }

                        DataRow it1 = ris_items.NewRow();
                        it1["itemid"] = dt.Rows[0]["itemid"];
                        it1["itemname"] = dt.Rows[0]["itemname"];
                        it1["quantity"] = request_quantity;
                        it1["unit"] = dt.Rows[0]["unit"];
                        it1["unitcost"] = dt.Rows[0]["unitcost"];
                        it1["transcode"] = dt.Rows[0]["transcode"];
                        it1["date"] = DateTime.Now;
                        ris_items.Rows.Add(it1);
                    }
                }
                if (error_items == "")
                {
                    return "1";
                }
                else
                {
                    return "Transaction Finish with Error on items '" + error_items + "'";
                }

                #endregion
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            #region hide
            //try
            //{
            //    //string query1 = "exec Proc_GetNewTranNo_ris '[IMS].[dbo].[tbl_t_ris_preparation]','RIS','controlno','" + yy + "','" + mn + "'";
            //    //string strcon = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();
            //    //string control_number = OleDbHelper.ExecuteScalar(strcon, System.Data.CommandType.Text, query1).ToString();

            //    string query2 = "insert into [IMS].[dbo].[tbl_t_ris_preparation] (controlno,date_time,officeid,officename) values ('" + control_number + "','" + DateTime.Now + "','" + USER.C_officeID + "','" + USER.C_Office + "')";
            //    string strcon2 = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();
            //    //  string result1 = OleDbHelper.ExecuteNonQuery(strcon2, System.Data.CommandType.Text, query2).ToString();
            //    string qry = "";

            //    foreach (DataRow row in ris_items.Rows)
            //    {
            //        qry += "insert into [IMS].[dbo].[wis_ris_transactions] (officeid,officename,itemid,itemname,unit,quantity,price,date_submited,trans_code,controlno) values ('" + USER.C_officeID + "','" + USER.C_Office + "','" + row["itemid"] + "','" + row["itemname"] + "','" + row["unit"] + "','" + row["quantity"] + "','" + row["unitcost"] + "','" + DateTime.Now + "','" + row["transcode"] + "','" + control_number + "')";
            //    }


            //    string con = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();
            //    //   string result = OleDbHelper.ExecuteNonQuery(con, System.Data.CommandType.Text, qry).ToString();
            //    string result = OleDbHelper.ExecuteNonQuery(con, System.Data.CommandType.Text, "exec submit_request '" + qry.Replace("'", "''") + "','" + control_number + "','" + query2.Replace("'", "''") + "'").ToString();
            //    return result;
            //}
            //catch (Exception ex)
            //{
            //    return ex.Message;
            //}
            #endregion
        }
        #endregion
        public ActionResult get_ris([DataSourceRequest] DataSourceRequest request)
        {
            DataTable dt = (@"select trans_code,status FROM [IMS].[dbo].[wis_ris_transactions] where officeid = '" + USER.C_officeID + "' group by trans_code,status ").DataSet();
            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            result.Content = serializer.Serialize(dt.ToDataSourceResult(request));
            result.ContentType = "application/json";
            return result;
        }

        
        public ActionResult listview_ris([DataSourceRequest] DataSourceRequest request)
        {
            revise_read r = new revise_read();
            var data = r.get_listview_ris();
            return Json(data.ToDataSourceResult(request));  
        }
        public ActionResult saigridview([DataSourceRequest] DataSourceRequest request,string controlno)
        {
            DataTable dt = (@"select a.* ,b.* from  
( select a.*,b.* FROM [IMS].[dbo].[wis_ris_transactions] as a 
 left join  (select distinct ris_transaction_id, max(id) as log_id  from [IMS].[dbo].[tbl_t_edited_logs] group by ris_transaction_id ) as b on a.id = b.ris_transaction_id where controlno = '"+controlno+"') as a left join  [IMS].[dbo].[tbl_t_edited_logs] as b on a.log_id = b.id ").DataSet();
            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            result.Content = serializer.Serialize(dt.ToDataSourceResult(request));
            result.ContentType = "application/json";
            return result;
        }

        public ActionResult notedby([DataSourceRequest] DataSourceRequest request)
        {
            //string qry = "select eid,EmpName from [pmis].[dbo].[vwMergeAllEmployee]";
            //DataTable dt = qry.DataSet();

            //var result = new ContentResult();
            //result.Content = SerializeDT.DataTableToJSON(dt);
            //result.ContentType = "application/json";
            //return result;
            DataTable dt = new DataTable("Data");

            using (SqlConnection con = new SqlConnection(common.livecon()))
            {
                SqlCommand com = new SqlCommand(@"select eid,EmpName from [pmis].[dbo].[vwMergeAllEmployee] order by EmpName", con);
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
        public ActionResult byoffice([DataSourceRequest] DataSourceRequest request)
        {
            //string qry = "select eid,EmpName from [pmis].[dbo].[vwMergeAllEmployee]";
            //DataTable dt = qry.DataSet();

            //var result = new ContentResult();
            //result.Content = SerializeDT.DataTableToJSON(dt);
            //result.ContentType = "application/json";
            //return result;
            var officeid = USER.C_officeID.ToString();
            var count = (@"select count(*) from [pmis].[dbo].[vwMergeAllEmployee] where Department = " + USER.C_officeID + " ").Scalar();
            if (count > 0)
            {
                //do nothing
            }
            else
            {
                officeid = (@"select mainofficeid   FROM [IMS].[dbo].[tbl_t_Office] where OfficeID = " + USER.C_officeID + "").ScalarString();
            }

            if (officeid == "11" || officeid == "67" || officeid == "68")
            {
                officeid = "11,67,68";

            }
            if (officeid == "1")
            {
                officeid = "1,8";
            }


            DataTable dt = new DataTable("Data");

            using (SqlConnection con = new SqlConnection(common.livecon()))
            {
                SqlCommand com = new SqlCommand(@"select eid,EmpName from [pmis].[dbo].[vwMergeAllEmployee]  where Department in( "+officeid+") order by EmpName", con);
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
        public string  save_notedby(int eid)
        {
           
            var id = (@"select count(eid) from  [IMS].[dbo].[tbl_t_notedby] where officeid = '"+USER.C_officeID+"' ").Scalar();
            if (id > 0)
            {
                ( "delete from  [IMS].[dbo].[tbl_t_notedby] where officeid = '" + USER.C_officeID + "' ").NonQuery();
            }
            else {
              
            }
            var qry = "insert into [IMS].[dbo].[tbl_t_notedby] values ('" + eid + "','" + USER.C_officeID + "') ";
            try
            {
                (qry).NonQuery();
                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string get_notedby()
        {
            var nn = (@"select b.EmpName FROM [IMS].[dbo].[tbl_t_notedby] as a
  inner join [pmis].[dbo].[vwMergeAllEmployee] as b on a.eid = b.eid where officeid = '"+USER.C_officeID+"'").ScalarString();
            return nn;
        }
        public string agree(string controlno)
        {
            string qry_delete_log = @"insert into [IMS].[dbo].[tbl_t_deleted_logs] select officeid,itemid,itemname,price,unit,quantity,trans_code,controlno,'"+DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss")+"' from  [IMS].[dbo].[wis_ris_transactions] where controlno = '" + controlno + "'  and to_be_deleted = 1"; 
            try 
            {
                (qry_delete_log).NonQuery();
                (@"delete from  [IMS].[dbo].[wis_ris_transactions] where controlno = '" + controlno + "'  and to_be_deleted = 1").NonQuery();
                (@"Update  [IMS].[dbo].[tbl_t_ris_preparation]  set agree = 1 where controlno = '"+controlno+"'").NonQuery();
                return "1";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public string save_phone_number(string phone_number)
        {
            try
            {
              var check = (@"select count(phone_number) from [IMS].[dbo].[tbl_t_sento_admin] where office_id= '"+USER.C_officeID+"' ").Scalar();

              if (check > 0)
              {
                  ("update [IMS].[dbo].[tbl_t_sento_admin] set phone_number = '" + phone_number + "' where office_id = '" + USER.C_officeID + "'").NonQuery();
              }
              else
              {
                  ("insert into [IMS].[dbo].[tbl_t_sento_admin] values ('" + USER.C_officeID + "','" + phone_number + "')").NonQuery();
              }
                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string get_phone_number()
        {
            var number = (@"select phone_number from [IMS].[dbo].[tbl_t_sento_admin] where office_id = '" + USER.C_officeID + "'").ScalarString();
            return number;
        }
        #endregion 
        
        #region ris_approval

        public ActionResult check_if_edited(string controlno, int officeid)
        {
            //DataTable dt = (@"select edited,agree from [IMS].[dbo].[tbl_t_ris_preparation]  where controlno = '" +controlno+"'").DataSet();
            revise_read r = new revise_read();
            var data = r.get_agree_edited(controlno);
            return Json(data ,JsonRequestBehavior.AllowGet);
        }

        public ActionResult update_ris([DataSourceRequest]DataSourceRequest request, item items)
        {
             #region edited_consent 
            var check_edited = (@"select edited from [IMS].[dbo].[tbl_t_ris_preparation]  where controlno = '" + items.controlno + "'").Scalar();
            
            if (check_edited == 1)
            {
                (@"update [IMS].[dbo].[tbl_t_ris_preparation] set agree = 0 where controlno = '" + items.controlno + "'").NonQuery();
            }
            else
            {
                string edited = "update [IMS].[dbo].[tbl_t_ris_preparation] set edited = 1 where controlno = '" + items.controlno + "'";
                (edited).NonQuery();
                (@"update [IMS].[dbo].[tbl_t_ris_preparation] set agree = 0 where controlno = '" + items.controlno + "'").NonQuery();
            }
             
            #endregion

             #region  edit_quantity
             //check available 
            //DataTable chck_total_avail = (@"select tbl_1.total_quantity - isnull(tbl_2.quantity,0) as total_available from (select itemid,sum(quantity) as total_quantity,unit,transaction_code from [IMS].[dbo].[tbl_items_per_transaction_code]  where transaction_code = '" + items.transcode + "' and itemid = "+items.itemid+" group by   itemid,unit,transaction_code ) as tbl_1  left join (select itemid,sum(quantity) as quantity,transcode from  [IMS].[dbo].[tbl_wis_transaction] where in_out = 'OUT' and transcode = '" + items.transcode + "' and itemid = "+items.itemid+" group by itemid,transcode) as tbl_2 on tbl_1.itemid = tbl_1.itemid and tbl_1.transaction_code = tbl_2.transcode").DataSet();

            DataTable chck_total_avail = (@" select tbl1.itemid,tbl1.items_in,tbl1.items_out,tbl1.remaining , t.itemname from( select a.*,b.items_out,a.items_in - CASE when b.items_out is null then 0 else b.items_out end as remaining  from ( SELECT itemid , SUM(CASE WHEN in_out = 'IN' THEN quantity ELSE 0 END)  as items_in  FROM [IMS].[dbo].[tbl_wis_transaction] where itemid = " + items.itemid + " GROUP BY itemid ) as a left join ( SELECT itemid,   SUM(quantity)  as items_out FROM [IMS].[dbo].[wis_ris_transactions] where to_be_deleted  = 0 and itemid = " + items.itemid + "  GROUP BY itemid) as b on a.itemid = b.itemid ) tbl1  CROSS APPLY ( SELECT   * FROM (select distinct(itemid),itemname from  [IMS].[dbo].[tbl_items_per_transaction_code] union select distinct(id) as itemid,itemname from  [IMS].[dbo].[tbl_manual_items] union select distinct(itemid)  ,itemname from  [IMS].[dbo].[tbl_wis_transaction] ) mi  WHERE   mi.itemid = tbl1.itemid ) t where tbl1.itemid = " + items.itemid + "").DataSet();
            var total_avail = Convert.ToInt32(chck_total_avail.Rows[0]["remaining"].ToString());
             
            //actual request
            int actual_request = (@"select quantity from [IMS].[dbo].[wis_ris_transactions]  where id = '"+items.id+"'").Scalar();

            //if (items.quantity > items.total)
            //{
            //    return null;
            //}
            if (items.quantity > actual_request)
            { 
                //quantity increased
                var increase_quantity = items.quantity - actual_request;
                if (increase_quantity > total_avail)
                {
                    return  null ;
                }
                else
                {
                    var prev_quantity = (@"select quantity from [IMS].[dbo].[wis_ris_transactions] where id = '" + items.id + "'").Scalar();
                    (@"insert into [IMS].[dbo].[tbl_t_edited_logs] values ('" + items.id + "','" + prev_quantity + "','" + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff") + "')").NonQuery();

                    string query2 = "Update  [IMS].[dbo].[wis_ris_transactions] set quantity = '" + items.quantity + "' ,comment = '"+items.comment+"'  where id = '" + items.id + "'";
                    string strcon = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();
                    string result1 = OleDbHelper.ExecuteNonQuery(strcon, System.Data.CommandType.Text, query2).ToString();
                }
            }
            if (items.quantity == actual_request)
            { 
              //Nothing's change

            }
            if(items.quantity < actual_request)
            {
              //Quantity Decrease
                var prev_quantity = (@"select quantity from [IMS].[dbo].[wis_ris_transactions] where id = '" + items.id + "'").Scalar();
                (@"insert into [IMS].[dbo].[tbl_t_edited_logs] values ('" + items.id + "','" + prev_quantity + "','" + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff") + "')").NonQuery();

                 
             var increase_quantity = items.quantity - actual_request;
             string query2 = "Update  [IMS].[dbo].[wis_ris_transactions] set quantity = '" + items.quantity + "' ,comment = '" + items.comment + "'  where id = '" + items.id + "'";
             string strcon = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();
             string result1 = OleDbHelper.ExecuteNonQuery(strcon, System.Data.CommandType.Text, query2).ToString();
            }
            #endregion

            return Json(new[] { items }.ToDataSourceResult(request, ModelState)); 
        }
        public ActionResult destroy_ris([DataSourceRequest]DataSourceRequest request, item items)
        {
            var cn = (@"select controlno from [IMS].[dbo].[wis_ris_transactions] where id =  '" + items.id + "'").DataSet();

            string query2 = "update [IMS].[dbo].[wis_ris_transactions] set to_be_deleted = 1 where id = '" + items.id + "';update [IMS].[dbo].[tbl_t_ris_preparation] set deleted = 1 ,agree = 0 where controlno = '" +cn.Rows[0]["controlno"] + "'";
            string strcon = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();
            string result1 = OleDbHelper.ExecuteNonQuery(strcon, System.Data.CommandType.Text, query2).ToString(); 

            int cnt = (@"select count(id) from [IMS].[dbo].[wis_ris_transactions] where controlno = '" +cn.Rows[0]["controlno"]+ "' and status is null ").Scalar();
            if (cnt > 0)
            {

            }
            else
            {
                (@"update [IMS].[dbo].[tbl_t_ris_preparation] set status = 1 where controlno = '" + cn.Rows[0]["controlno"] + "'").NonQuery();
            }
            return Json(new[] { items }.ToDataSourceResult(request, ModelState));
        }

        public string undo(string controlno, int id)
        {
           
            try
            {
                (@"update [IMS].[dbo].[wis_ris_transactions] set to_be_deleted = 0 where id = '" + id + "'").NonQuery();

                 
                var count_deleted = (@"select count(*) from [IMS].[dbo].[wis_ris_transactions] where to_be_deleted = 1").Scalar();
                if (count_deleted > 0)
                {
                   
                }
                else
                {
                    (@"update [IMS].[dbo].[tbl_t_ris_preparation] set deleted = 0 where controlno = '" + controlno + "'").NonQuery();
                  
                }
                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string remove_ris(string controlno, int officeid)
        {
            try 
            {
                (@"update [IMS].[dbo].[tbl_t_ris_preparation] set status = 3 where controlno = '"+controlno+"'").NonQuery();
                (@"update [IMS].[dbo].[wis_ris_transactions] set status = 3 ,to_be_deleted = 1 where controlno = '" + controlno + "'").NonQuery();
                return "1";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }
        public string approve_request(IEnumerable<item> items, string controlno)
        {
            string qry = "";
            foreach (var i in items)
            {
                qry += "insert into [IMS].[dbo].[tbl_wis_transaction] (eid,itemid,itemname,date,quantity,in_out,unit,unitcost,transcode,officeid,ris_id) values ('" + USER.C_eID + "','" + i.itemid + "','" + i.itemname.Replace("'", "''") + "','" + DateTime.Now.ToString("MM-dd-yyyy") + "','" + i.quantity + "','OUT','" + i.unit + "','" + i.rprice + "','" + i.transcode + "','" + i.officeid + "','"+i.id+"');update [IMS].[dbo].[wis_ris_transactions] set status = 1 where id = '" + i.id + "';";
            }

            try {
                string strcon = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();
                string result1 = OleDbHelper.ExecuteNonQuery(strcon, System.Data.CommandType.Text, qry).ToString();

                int cnt = (@"select count(id) from [IMS].[dbo].[wis_ris_transactions] where controlno = '" + controlno + "' and status is null ").Scalar();
                if (cnt > 0)
                {

                }
                else
                {
                    (@"update [IMS].[dbo].[tbl_t_ris_preparation] set status = 1 , date_approve = '" + DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss") + "' where controlno = '" + controlno + "'").NonQuery();
                }
                return "1";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public ActionResult get_items_office([DataSourceRequest]DataSourceRequest request, int officeid)
        {
            revise_read r = new revise_read();
            var data = r.get_office_available(officeid);
            return Json(data.ToDataSourceResult(request));
        }

        public ActionResult listview_approved_ris([DataSourceRequest]DataSourceRequest request)
        {
            DataTable dt = (@"select * from [IMS].[dbo].[tbl_t_ris_preparation] where status = 1" ).DataSet();
            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            result.Content = serializer.Serialize(dt.ToDataSourceResult(request));
            result.ContentType = "application/json";
            return result;
        }
        public ActionResult get_approved_items([DataSourceRequest]DataSourceRequest request, string controlno)
        {
            DataTable dt = (@"select * FROM [IMS].[dbo].[wis_ris_transactions]  where controlno = '" + controlno + "' and status = 1").DataSet();
            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            result.Content = serializer.Serialize(dt.ToDataSourceResult(request));
            result.ContentType = "application/json";
            return result;
        }
        public ActionResult get_items_by_ris_code([DataSourceRequest] DataSourceRequest request, string controlno)
        {
            revise_read r = new revise_read();
            var data = r.get_item_by_ris(controlno);
            return Json(data.ToDataSourceResult(request));
        }
        public ActionResult read_submited_ris_office([DataSourceRequest] DataSourceRequest request)
        {
            DataTable dt = (@"select controlno,date_time,officeid,officename,status FROM [IMS].[dbo].[tbl_t_ris_preparation] where status = 0").DataSet();
            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            result.Content = serializer.Serialize(dt.ToDataSourceResult(request));
            result.ContentType = "application/json";
            return result;
        }
        public string mark_released(string e)
        {
            string qry = "";
            string insert_qry = "";
            string check = "select release from [IMS].[dbo].[tbl_t_ris_preparation]  where controlno = '" + e + "'";
            int result_check = (check).Scalar();
            if (result_check == 0)
            {
                 qry = @"Update [IMS].[dbo].[tbl_t_ris_preparation] set release = 1 where controlno = '" + e + "'";

                 insert_qry = @"insert into [IMS].[dbo].[transaction] (eid,item_id,stock_date,quantity,in_out,officeid,itemname,unit,controlno)  select '" + USER.C_eID + "',itemid,'" + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff") + "',quantity,'IN',officeid,itemname,unit,'" + e + "' from [IMS].[dbo].[wis_ris_transactions] where controlno = '" + e + "'";
                   try
                   {
                       (qry).NonQuery();
                       (insert_qry).NonQuery();
                       return "1";
                   }
                   catch (Exception ex)
                   {
                       return ex.Message;
                   }
            }
            else
            {
                 qry = "Update [IMS].[dbo].[tbl_t_ris_preparation] set release = 0 where controlno = '" + e + "'";

                 insert_qry = @"delete from [IMS].[dbo].[transaction]  where controlno = '" + e + "'";
                 try
                 {
                     (qry).NonQuery();
                     (insert_qry).NonQuery();
                     return "1";
                 }
                 catch (Exception ex)
                 {
                     return ex.Message;
                 } 
            } 
        }
        public string receive(string e)
        {
            string qry = "";
            string insert_qry = "";
          
                qry = @"Update [IMS].[dbo].[tbl_t_ris_preparation] set receive = 1 where controlno = '" + e + "'";

                insert_qry = @"insert into [IMS].[dbo].[transaction] (eid,item_id,stock_date,quantity,in_out,officeid,itemname,unit,controlno)  select '" + USER.C_eID + "',itemid,'" + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff") + "',quantity,'IN',officeid,itemname,unit,'" + e + "' from [IMS].[dbo].[wis_ris_transactions] where controlno = '" + e + "'";
                try
                {
                    (qry).NonQuery();
                    (insert_qry).NonQuery();
                    return "1";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
        }
        public int count_approval()
        {
            var count = (@"select count(controlno) FROM [IMS].[dbo].[tbl_t_ris_preparation] where status = 0").Scalar();
            return count;
        }


        #endregion

        #region utilization_per_office
        public ActionResult utilization()
        {
            return View();
        }

        public ActionResult grid_utilization ([DataSourceRequest] DataSourceRequest request,int officeid,string  from ,string to)
        {

            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;

            DataTable dt = (@"select item_id,itemname ,SUM(quantity) as consume,unit FROM [IMS].[dbo].[transaction] where officeid = '" + officeid + "' and in_out='OUT' and cast(stock_date as date)  between '" + from   + "' and '" +to + "' group by item_id,itemname,unit").DataSet();

                result.Content = serializer.Serialize(dt.ToDataSourceResult(request));
                result.ContentType = "application/json";
                return result;
             
        }

        public ActionResult grid_consume([DataSourceRequest] DataSourceRequest request, int officeid, string from, string to)
        {
//            DataTable dt = (@"select item_id,itemname ,SUM(CASE WHEN in_out = 'OUT' THEN (case when ref_id is null then 0 else quantity end) ELSE 0 END) as consume,unit FROM [IMS].[dbo].[transaction]
//   where officeid = '" + (Convert.IsDBNull(officeid) ? 0 : (int)(officeid)) + "' and in_out='OUT' and cast(stock_date as date) between '" + from == null ? "01/01/1990" : ((DateTime)from).ToString("MM/dd/yyyy") + "' and '" + to == null ? "01/01/1990" : ((DateTime)from).ToString("MM/dd/yyyy") + "' group by item_id,itemname,unit").DataSet();


            read r = new read();
            var data = r.gridConsume(officeid,from,to);
            //var serializer = new JavaScriptSerializer();
            //var result = new ContentResult();
            //serializer.MaxJsonLength = Int32.MaxValue;
            //result.Content = serializer.Serialize(dt.ToDataSourceResult(request));
            //result.ContentType = "application/json";
            return Json(data.ToDataSourceResult(request)); 
        }
        public ActionResult consume([DataSourceRequest] DataSourceRequest request, int officeid, string from, string to)
        {
            read r = new read();
            var data = r.gridConsume(officeid, from, to);
            return Json(data.ToDataSourceResult(request)); 
        }
        public ActionResult running_balance([DataSourceRequest] DataSourceRequest request)
        {
//             DataTable dt = (@"select * from (SELECT itemid, itemname, unit , SUM(CASE WHEN in_out = 'IN' THEN quantity ELSE 0 END) - SUM(CASE WHEN in_out = 'OUT' THEN quantity ELSE 0 END) as running_balance
//FROM  [IMS].[dbo].[tbl_wis_transaction] GROUP BY itemid, itemname, unit ) as   tbl1 
//where running_balance > 0
// ORDER BY itemname").DataSet();


            DataTable dt = (@"select a.itemid,c.itemname ,a.unit, a.quantity as Qin,ISNULL(b.quantity,0) as Qout ,  ISNULL(a.quantity,0) - ISNULL(b.quantity,0)  as total_remaining,ISNULL(d.reorder_point,0) as reorder_point   from 
( 
SELECT itemid , in_out, unit, SUM(quantity) AS quantity FROM   dbo.tbl_wis_transaction WHERE        (in_out = 'IN' ) GROUP BY itemid,in_out, unit  
) as a 
left join 
( 
SELECT itemid, unit , SUM(quantity) AS quantity FROM      dbo.wis_ris_transactions WHERE        (to_be_deleted = 0) GROUP BY itemid , unit 
) as b on a.itemid = b.itemid 
 left join  
( 
 SELECT  [itemid] ,[reorder_point] ,[officeid] FROM [IMS].[dbo].[tbl_l_reorder] where officeid = 0 
) as d on a.itemid = d.itemid 
 CROSS APPLY 
( 
 SELECT TOP 1 * FROM 
 ( 
 select distinct(itemid),LTRIM(RTRIM(itemname)) as itemname  from  [IMS].[dbo].[tbl_items_per_transaction_code]  
 union 
 select distinct(id) as itemid,LTRIM(RTRIM(itemname)) as itemname from  [IMS].[dbo].[tbl_manual_items] 
  union 
 select distinct(itemid)  ,LTRIM(RTRIM(itemname)) as itemname from  [IMS].[dbo].[tbl_wis_transaction] 
 ) item where a.itemid = item.itemid
 ) as c  order by c.itemname").DataSet();

            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            result.Content = serializer.Serialize(dt.ToDataSourceResult(request));
            result.ContentType = "application/json";
            return result;
        }
         
        public ActionResult utilize_available([DataSourceRequest] DataSourceRequest request, int officeid)
        {
            read r = new read();
            var a = r.gridOfficeRemaining(officeid); 
            return Json(a.ToDataSourceResult(request));  
        }

        #endregion

        #region printing
        public ActionResult Printing()
        {
            return View();
        }
        public ActionResult get_office_vale([DataSourceRequest] DataSourceRequest request, string  date    )
        {
            revise_read r = new revise_read();
            var data = r.get_office_vale(date);
            return Json(data.ToDataSourceResult(request));
        }
       

        #endregion

        #region MyRegion serialize dropdown
        public ActionResult OfficeNew([DataSourceRequest] DataSourceRequest request)
        {
            string qry = "select * from [IMS].[dbo].[tbl_t_Office]";
            DataTable dt = qry.DataSet();

            var result = new ContentResult();
            result.Content = SerializeDT.DataTableToJSON(dt);
            result.ContentType = "application/json";
            return result;
        }
        #endregion

        #region skin
        public string save_skin(string skin)
        {
            string qry = "update [IMS].[dbo].[user] set skin='" + skin + "' where eid = '"+USER.C_eID+"'";
            try
            {
                (qry).NonQuery();
                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            } 
        }

        public string get_skin()
        {
            string qry = "select skin from [IMS].[dbo].[user] where eid = '"+USER.C_eID+"'";
            
            var data = (qry).ScalarString();

            return data;
        }
        #endregion

        #region Payment /attach OBR
        public ActionResult ReturnItemsBorrowed()
        {
            return View("ReturnItemsBorrowed");
        }
        public ActionResult returned_item_view()
        {
            return PartialView();
        }
        public ActionResult returned_items([DataSourceRequest] DataSourceRequest request)
        { 
            DataTable dt = (@"select itemid,itemname,unit,available as running_balance from [IMS].[dbo].[view_items_where_to_get] order by itemname").DataSet();
            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            result.Content = serializer.Serialize(dt.ToDataSourceResult(request));
            result.ContentType = "application/json";
            return result;
        }

        public string return_request(string ris_number)
        {
            try
            {
                ("update [IMS].[dbo].[wis_ris_transactions] set status = 0 where controlno = '"+ris_number+"'").NonQuery();

                ("update [IMS].[dbo].[tbl_t_ris_preparation] set status = 0  where controlno = '" + ris_number + "'").NonQuery();

                ("delete [IMS].[dbo].[tbl_wis_transaction] where ris_id IN ( select id from [IMS].[dbo].[wis_ris_transactions] where controlno = '"+ris_number+"' )").NonQuery();

                ("delete [IMS].[dbo].[transaction] where controlno = '"+ris_number+"'").NonQuery();

                return "1";
            }
            catch (Exception ex)
            {
                return "0";
            }
        }


        //PAID or With OBR
        public ActionResult GetRisListAndAmountWithObr([DataSourceRequest] DataSourceRequest request,int officeid)
        {
            revise_read r = new revise_read();
            var data = r.getRisWithAmountObr(officeid);
            return Json(data.ToDataSourceResult(request));
        }

        //UNPAID or without obr
        public ActionResult GetRisListAndAmountWithObr1([DataSourceRequest] DataSourceRequest request, int officeid)
        {
            revise_read r = new revise_read();
            var data = r.getRisWithAmountObr1(officeid);
            return Json(data.ToDataSourceResult(request));
        }
        public ActionResult paymenyGetItems([DataSourceRequest] DataSourceRequest request, string controlno)
        {
            DataTable dt = (@"select * FROM [IMS].[dbo].[wis_ris_transactions]  where controlno = '" + controlno + "' and (status = 1 or status is null or status = 0)").DataSet();
            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            result.Content = serializer.Serialize(dt.ToDataSourceResult(request));
            result.ContentType = "application/json";
            return result;
        }
        public string gettotalamount([DataSourceRequest] DataSourceRequest request, string controlno)
        {
            var amount = (@"select   CONVERT(varchar, CAST(sum(price*quantity)  AS money), 1) as amount   FROM [IMS].[dbo].[wis_ris_transactions]  where controlno = '" + controlno + "' and (status = 1 or status is null or status = 0)").ScalarString();
          return amount.ToString();
        }

        public string UpdateOBR([DataSourceRequest] DataSourceRequest request, item items)
        {
            string query = @"update [IMS].[dbo].[tbl_t_ris_preparation] set OBR = '" + items.obr + "'  where preparation_id = " + items.preparation_id + "";
            try
            {
                (query).NonQuery();
                return "1"; 
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        #endregion

        #region Maintenance
       
        public string new_item(item items)
        { 
            var name = items.itemname.First().ToString().ToUpper() + items.itemname.Substring(1);
             var n = items.unit.First().ToString().ToUpper() + items.unit.Substring(1);

             int maxid = (@"select max(id) FROM [IMS].[dbo].[tbl_manual_items]").Scalar();
             var id = maxid + 1;

             (@"insert into [IMS].[dbo].[tbl_manual_items]  values ('" + id + "','" + name.Replace("'","''") + "','" + n.Replace("'", "''") + "')").NonQuery(); 

            return "1";
        }

        public ActionResult get_manual_items([DataSourceRequest] DataSourceRequest request )
        {
            read r = new read();
            var data = r.get_manual_items();
            return Json(data.ToDataSourceResult(request)); 
        }

        public string update_item([DataSourceRequest] DataSourceRequest request, item items)
        {
            string query = @"update  [IMS].[dbo].[tbl_manual_items] set itemname = '"+items.itemname+"' , unit = '"+items.unit+"' where id = '"+items.id+"'" ;
            try
            {
                (query).NonQuery();
                return "1";
                 
            }
            catch (Exception ex)
            {
                return ex.ToString();
            } 
        }

        public string delete_item([DataSourceRequest] DataSourceRequest request, item items)
        {
            string query = @"delete from   [IMS].[dbo].[tbl_manual_items]   where id = '" + items.id + "'";
            try
            {
                (query).NonQuery();
                return "1"; 
            }
            catch (Exception ex)
            {
                return ex.ToString();
            } 
        }
        #endregion

        #region Emergency RIS
        public ActionResult emergency_ris()
        {
            return PartialView();
        }
        public ActionResult warehouse()
        {
            return  View();
        }
        public ActionResult ris_view_warehouse()
        {
            return View("ris_warehouse");
        }
        public ActionResult ris_view_warehouse_tires()
        {
            return View("ris_warehouse_tires");
        }
        public ActionResult emergency_sai([DataSourceRequest] DataSourceRequest request, int year, int quarter, int mooe_no, int dbm_bb, int accountid)
        {
            revise_read r = new revise_read();
            var data = r.emergency_sai(year, quarter, mooe_no, dbm_bb, accountid);
            return Json(data.ToDataSourceResult(request));
        }


        public ActionResult search_items([DataSourceRequest] DataSourceRequest request, string itemname)
        {
            read r = new read();
            var data = r.searchItem(itemname);
            return Json(data.ToDataSourceResult(request));
        }

        public string submit_emergency(IEnumerable<item> items, int year, int quarter, int mooe_no, int dbm_bb, string tc,int officeid, string officename,int eid)
        {
            var error_items = "";

            DataTable ris_items = new DataTable();
            ris_items.Columns.Add("itemid");
            ris_items.Columns.Add("itemname");
            ris_items.Columns.Add("quantity");
            ris_items.Columns.Add("unit");
            ris_items.Columns.Add("unitcost");
            ris_items.Columns.Add("transcode");
            ris_items.Columns.Add("date");
            ris_items.Columns.Add("controlno");



            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));

            String dy = datevalue.Day.ToString();
            String mn = datevalue.Month.ToString();
            String yy = datevalue.Year.ToString();

            string query1 = "exec Proc_GetNewTranNo_ris '[IMS].[dbo].[tbl_t_ris_preparation]','RIS','controlno','" + yy + "','" + mn + "'";
            string strcon = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();
            string control_number = OleDbHelper.ExecuteScalar(strcon, System.Data.CommandType.Text, query1).ToString();

            string query2 = "insert into [IMS].[dbo].[tbl_t_ris_preparation] (controlno,date_time,officeid,officename,preparedby,transaction_type) values ('" + control_number.Trim() + "','" + DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss") + "','" + officeid + "','" + officename.Replace("'", "''") + "','"+eid+"','BORROW')";
            string strcon2 = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();
            string result1 = OleDbHelper.ExecuteNonQuery(strcon2, System.Data.CommandType.Text, query2).ToString();

            try
            {
                #region loop
                foreach (var i in items)
                {
                    var cn = control_number.Trim();
                    var row_number = 0;
                    var itemid = i.itemid;
                    var request_quantity = i.request_quantity;

                    #region check_available
                    DataTable chck_total_avail = (@"SELECT  t.itemid,mo.total_available,t.itemname
 FROM (select  itemid,sum(available) as total_available,year,quarter,mooe_no,dbm_bb,transcode from [IMS].[dbo].[view_items_where_to_get] where itemid = '" + itemid + "' group by itemid,year,quarter,mooe_no,dbm_bb,transcode ) mo  CROSS APPLY ( SELECT  TOP 1 * FROM    [IMS].[dbo].[tbl_wis_transaction] mi WHERE   mi.itemid = mo.itemid ) t where mo.year = '" + year + "'  and mo.quarter = '" + quarter + "' and mo.mooe_no = '" + mooe_no + "' and mo.dbm_bb = '" + dbm_bb + "' and mo.transcode = '" + tc + "' ").DataSet();
                    var total_avail = Convert.ToInt32(chck_total_avail.Rows[row_number]["total_available"].ToString());

                    if (total_avail < request_quantity)
                    {
                        return "Ops Remaining Quantity for'" + chck_total_avail.Rows[row_number]["itemname"].ToString() + "' is only '" + total_avail + "' ";
                    }

                    DataTable dt = (@"select * from [IMS].[dbo].[view_items_where_to_get] where itemid = '" + itemid + "' and available != 0 and available !< 0 and year ='" + year + "'  and quarter = '" + quarter + "' and mooe_no = '" + mooe_no + "' and dbm_bb = '" + dbm_bb + "' and transcode = '" + tc + "' ").DataSet();

                    var initial_available = dt.Rows[row_number]["available"].ToString();
                    var int_available = Convert.ToInt32(initial_available);
                    #endregion

                    if (request_quantity > int_available)
                    {
                        var total_pass = 0.0;
                        var y = request_quantity;
                        for (var x = 0; request_quantity != total_pass; x++)
                        {
                            #region data
                            DataTable dt1 = (@"select * from [IMS].[dbo].[view_items_where_to_get] where itemid = '" + itemid + "' and available != 0 and year ='" + year + "'  and quarter = '" + quarter + "' and mooe_no = '" + mooe_no + "' and dbm_bb = '" + dbm_bb + "' and  transcode = '" + tc + "'").DataSet();

                            var dt_itemid = dt.Rows[row_number]["itemid"].ToString();
                            var dt_itemname = dt.Rows[row_number]["itemname"].ToString();
                            var dt_unitcost = dt.Rows[row_number]["unitcost"].ToString();
                            var dt_unit = dt.Rows[row_number]["unit"].ToString();
                            var dt_transcode = dt.Rows[row_number]["transcode"].ToString();
                            var dt_available = dt.Rows[row_number]["available"].ToString();
                            var _available = Convert.ToInt32(dt_available);
                            //var ref_id = dt.Rows[row_number]["id"].ToString();
                            #endregion

                            if (_available > y)
                            {
                                try
                                {
                                    string qry = @"insert into [IMS].[dbo].[wis_ris_transactions](officeid,officename,itemid,itemname,unit,quantity,price,date_submited,trans_code,controlno) values ('" +officeid + "','"+officename.Replace("'","''")+ "','" + dt_itemid + "','" + dt_itemname + "','" + dt_unit + "','" + y + "','" + dt_unitcost + "','" + DateTime.Now.ToString("MM-dd-yyyy") + "','" + dt_transcode + "','" + cn + "')";
                                    string con = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();
                                    string result = OleDbHelper.ExecuteNonQuery(con, System.Data.CommandType.Text, qry).ToString();

                                }
                                catch
                                {
                                    error_items += " '" + dt_itemname + "' ";
                                }


                                DataRow it = ris_items.NewRow();
                                it["itemid"] = dt_itemid;
                                it["itemname"] = dt_itemname;
                                it["quantity"] = y;
                                it["unit"] = dt_unit;
                                it["unitcost"] = dt_unitcost;
                                it["transcode"] = dt_transcode;
                                it["date"] = DateTime.Now;
                                ris_items.Rows.Add(it);
                                total_pass += y;
                            }
                            else
                            {
                                try
                                {
                                    string qry = @"insert into [IMS].[dbo].[wis_ris_transactions] (officeid,officename,itemid,itemname,unit,quantity,price,date_submited,trans_code,controlno) values ('" + officeid + "','"+officename.Replace("'","''")+ "','" + dt_itemid + "','" + dt_itemname + "','" + dt_unit + "','" + _available + "','" + dt_unitcost + "','" + DateTime.Now.ToString("MM-dd-yyyy") + "','" + dt_transcode + "','" + cn + "')";
                                    string con = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();
                                    string result = OleDbHelper.ExecuteNonQuery(con, System.Data.CommandType.Text, qry).ToString();

                                }
                                catch
                                {
                                    error_items += " '" + dt_itemname + "' ";
                                }
                                DataRow it = ris_items.NewRow();
                                it["itemid"] = dt_itemid;
                                it["itemname"] = dt_itemname;
                                it["quantity"] = _available;
                                it["unit"] = dt_unit;
                                it["unitcost"] = dt_unitcost;
                                it["transcode"] = dt_transcode;
                                it["date"] = DateTime.Now;
                                ris_items.Rows.Add(it);
                                y -= _available;
                                total_pass += _available;
                                row_number += 1;
                            }
                        }
                    }
                    else if (request_quantity <= int_available)
                    {
                        try
                        {
                            string qry = @"insert into [IMS].[dbo].[wis_ris_transactions] (officeid,officename,itemid,itemname,unit,quantity,price,date_submited,trans_code,controlno) values ('" + officeid + "','"+officename.Replace("'","''")+"','" + dt.Rows[0]["itemid"] + "','" + dt.Rows[0]["itemname"].ToString().Replace("'", "''") + "','" + dt.Rows[0]["unit"] + "','" + request_quantity + "','" + dt.Rows[0]["unitcost"] + "','" + DateTime.Now.ToString("MM-dd-yyyy") + "','" + dt.Rows[0]["transcode"] + "','" + cn + "')";
                            string con = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();
                            string result = OleDbHelper.ExecuteNonQuery(con, System.Data.CommandType.Text, qry).ToString();

                        }
                        catch
                        {
                            error_items += " '" + dt.Rows[0]["itemname"].ToString() + "' ";
                        }

                        DataRow it1 = ris_items.NewRow();
                        it1["itemid"] = dt.Rows[0]["itemid"];
                        it1["itemname"] = dt.Rows[0]["itemname"];
                        it1["quantity"] = request_quantity;
                        it1["unit"] = dt.Rows[0]["unit"];
                        it1["unitcost"] = dt.Rows[0]["unitcost"];
                        it1["transcode"] = dt.Rows[0]["transcode"];
                        it1["date"] = DateTime.Now;
                        ris_items.Rows.Add(it1);
                    }
                }
                if (error_items == "")
                {
                    return "1";
                }
                else
                {
                    return "Transaction Finish with Error on items '" + error_items + "'";
                }
                #endregion
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public ActionResult getEmp([DataSourceRequest] DataSourceRequest request, int officeid)
        {
            //Support to education program
            if (officeid == 1109)
            {
                officeid = 1261;
            }
            //pgo mbcd
           
            //nbi
            if (officeid == 1112)
            {
                officeid = 1;
            }
            //office of the provincial auditor
            if (officeid == 1116)
            {
                officeid = 1;
            }
            //sure
            if (officeid == 1124)
            {
                officeid = 1290;
            }


            DataTable dt = new DataTable("Data");

            var count = (@"select count(*) from [pmis].[dbo].[vwMergeAllEmployee] where Department = " + officeid + " ").Scalar();
            if (count > 0)
            {
                //do nothing
            }
            else
            {
                officeid = (@"select mainofficeid   FROM [IMS].[dbo].[tbl_t_Office] where OfficeID = " + officeid + "").Scalar();
            }


            using(SqlConnection con = new SqlConnection(common.livecon()))
            {
                SqlCommand com = new SqlCommand(@"select eid,EmpName from [pmis].[dbo].[vwMergeAllEmployee] where Department = '" + officeid + "' order by EmpName", con);
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

        public ActionResult readBorrowedItems([DataSourceRequest] DataSourceRequest request, int itemid, int officeid)
        {
            revise_read r = new revise_read();
            var data = r.readItemsBorrowed(itemid,officeid);
            return Json(data.ToDataSourceResult(request));
        }
        public ActionResult readControl([DataSourceRequest] DataSourceRequest request,int officeid)
        {
            revise_read r = new revise_read();
            var data = r.readAllBorrowedControl(officeid);
            return Json(data.ToDataSourceResult(request));
        }

        public ActionResult getItemsPerControlNo([DataSourceRequest] DataSourceRequest request, int officeid,string controlno)
        {
            revise_read r = new revise_read();
            var data = r.getItemsPerControlNo(officeid, controlno);
            return Json(data.ToDataSourceResult(request));
        }

        public ActionResult getBorrowedItems([DataSourceRequest] DataSourceRequest request, int officeid)
        {
            revise_read r = new revise_read();
            var data = r.getItems(officeid);
            return Json(data.ToDataSourceResult(request));
        }

        public string pay(int itemid, string itemname, int quantity, string transcode, string unit, int officeid, int id, decimal price, string controlno, int borrower_eid)
        {
            string query = @"insert into [IMS].[dbo].[tbl_t_paid] values ('" + itemid + "','" + itemname.Replace("'","''") + "','" + officeid + "','" + quantity + "','" + unit + "','" + borrower_eid + "','" + USER.C_eID + "','" + id + "','" + price + "')";
         
            try
            {
                query.NonQuery();
                return "1";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        #endregion

        public ActionResult allunit([DataSourceRequest] DataSourceRequest request)
        {  
            DataTable dt = new DataTable("Data");

            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand(@"select  distinct(unit) FROM [IMS].[dbo].[tbl_items_per_transaction_code]", con);
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

        public string check_auth()
        {
            string result = "0";
            try
            {

                if (System.Web.HttpContext.Current.User != null && System.Web.HttpContext.Current.User.Identity is FormsIdentity)
                {
                    FormsIdentity id = (FormsIdentity)System.Web.HttpContext.Current.User.Identity;

                    FormsAuthenticationTicket ticket = id.Ticket;

                    if (ticket.Expired == true)
                    {
                       // System.Web.HttpContext.Current.Response.Redirect("~/Login/New_index");
                        result = "1";
                    }
                    else
                    {
                        if (USER.C_Name == "")
                        {
                            FormsAuthentication.SignOut();
                          //  System.Web.HttpContext.Current.Response.Redirect("~/Login/New_index");
                            result = "1";
                        }
                    }
                }
                else
                {
                    // the user is not yet authenticated and 
                    // there is no Forms Identity for current request
                    //System.Web.HttpContext.Current.Response.Redirect("~/Login/New_index");
                    //System.Web.HttpContext.Current.Response.Redirect("", false);
                    result = "1";
                }

            }
            catch
            {
                result = "0";
                //System.Web.HttpContext.Current.Response.Redirect("~/Login/New_index");
            }
            return result;
        }

        public string Notify_SMS(string transaction_code,string noti_code,int? officeid, string control_number)
        {
            int isid = 1;
            try
            {
                using (SqlConnection con = new SqlConnection(common.MyConnection()))
                {
                    SqlCommand com = new SqlCommand("Notify_Office_Admin", con);
                    com.CommandType = System.Data.CommandType.StoredProcedure;

                    SqlParameter parameter1 = new SqlParameter();
                    parameter1.ParameterName = "@transaction_code";
                    parameter1.SqlDbType = SqlDbType.NVarChar;
                    parameter1.Size = 8000;
                    parameter1.Value = transaction_code;
                    com.Parameters.Add(parameter1);

                    SqlParameter parameter2 = new SqlParameter();
                    parameter2.ParameterName = "@notification_code";
                    parameter2.SqlDbType = SqlDbType.NVarChar;
                    parameter2.Size = 8000;
                    parameter2.Value = noti_code;
                    com.Parameters.Add(parameter2);

                    SqlParameter parameter3 = new SqlParameter();
                    parameter3.ParameterName = "@ris_code";
                    parameter3.SqlDbType = SqlDbType.NVarChar;
                    parameter3.Size = 8000;
                    parameter3.Value = control_number;
                    com.Parameters.Add(parameter3);

                    con.Open();
                    var msgd = com.ExecuteScalar();

                    string msg = msgd.ToString();
                    
                    con.Close();
                     
                    if (noti_code == "Entry")
                    {
                        SqlConnection cons = new SqlConnection(common.MyConnection());
                        SqlCommand comm = new SqlCommand(@"select phone_number from [IMS].[dbo].[tbl_t_sento_admin]  ", cons);
                        cons.Open();
                        SqlDataReader reader = comm.ExecuteReader();

                        List<string> myCollection = new List<string>();
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        cons.Close();


                        foreach (DataRow row in dt.Rows)
                        {
                            var num = row["phone_number"].ToString();
                            myCollection.Add(num); 
                        }

                        var arr = myCollection.ToArray();
                        var str_arr = string.Join(",", arr);
                        var edited_arr = str_arr.Replace("/", ",");

                        char[] delimiterChars = { ' ', ',', '.', ':', '\t' };

                        string[] Send_to = edited_arr.Split(delimiterChars);


                        foreach (string sendTo in Send_to)
                        {
                          //  sms.SEND_SMS(sendTo, msg, isid);
                           // r.SEND_SMSAsync(s, msg, 1);
                        }
                    }
                    if (noti_code == "Approved")
                    {
                        List<string> office_number = new List<string>();
                      DataTable dt_office = (@"select phone_number from [IMS].[dbo].[tbl_t_sento_admin] where office_id = '" + officeid + "'").DataSet();

                      foreach (DataRow row in dt_office.Rows)
                      {
                          var num = row["phone_number"].ToString();
                          office_number.Add(num);
                      }

                      var arr = office_number.ToArray();
                      var str_arr = string.Join(",", arr);
                      var edited_arr = str_arr.Replace("/", ",");

                      char[] delimiterChars = { ' ', ',', '.', ':', '\t' };

                      string[] Send_to = edited_arr.Split(delimiterChars);


                      foreach (string sendTo in Send_to)
                      {
                            sms.SEND_SMS(sendTo, msg, isid);
                         //  r.SEND_SMSAsync(s, msg, 1);
                      }
                    }
                    return "1";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        #region Warehouse
        public ActionResult for_warehouse([DataSourceRequest] DataSourceRequest request)
        {
            revise_read r = new revise_read();
            var data = r.get_warehouse();
            return Json(data.ToDataSourceResult(request));
        }
        public ActionResult ris_warehouse([DataSourceRequest] DataSourceRequest request)
        {
            revise_read r = new revise_read();
            var data = r.ris_warehouse();
            return Json(data.ToDataSourceResult(request));
        }
        public ActionResult ris_warehouse_tires([DataSourceRequest] DataSourceRequest request)
        {
            revise_read r = new revise_read();
            var data = r.ris_warehouse_tires();
            return Json(data.ToDataSourceResult(request));
        }
        public PartialViewResult view_running_balance()
        {
            return PartialView("view_running_balance");
        } 
        public string submitRequestNew(IEnumerable<item> items,int officeid,int empid,string officename)
        { 
            var error_items = ""; 
            
            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));

            String dy = datevalue.Day.ToString();
            String mn = datevalue.Month.ToString();
            String yy = datevalue.Year.ToString();

            string query1 = "exec Proc_GetNewTranNo_ris '[IMS].[dbo].[tbl_t_ris_preparation]','RIS','controlno','" + yy + "','" + mn + "'";
            string strcon = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();
            string control_number = OleDbHelper.ExecuteScalar(strcon, System.Data.CommandType.Text, query1).ToString();

            string query2 = "insert into [IMS].[dbo].[tbl_t_ris_preparation] (controlno,date_time,officeid,officename,preparedby,transaction_type) values ('" + control_number.Trim() + "','" + DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss") + "','" + officeid + "','" + officename.Replace("'", "''") + "','" + empid + "','BORROW')";
            string strcon2 = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();
            string result1 = OleDbHelper.ExecuteNonQuery(strcon2, System.Data.CommandType.Text, query2).ToString();

            try
            {
                #region loop
                foreach (var i in items)
                {
                    var cn = control_number.Trim();
                    var row_number = 0;
                    var itemid = i.itemid;
                    var request_quantity = i.request_quantity;
                  
                    #region check_available
                    DataTable chck_total_avail = (@"select tbl1.itemid,tbl1.items_in,tbl1.items_out,tbl1.remaining , c.itemname from( select a.*,b.items_out,a.items_in - CASE when b.items_out is null then 0 else b.items_out end as remaining  from ( SELECT itemid , SUM(CASE WHEN in_out = 'IN' THEN quantity ELSE 0 END)  as items_in ,unitcost FROM [IMS].[dbo].[tbl_wis_transaction] where itemid = " + itemid + " and unitcost = " + i.rprice + " GROUP BY itemid,unitcost ) as a left join ( SELECT itemid,   SUM(quantity)  as items_out FROM [IMS].[dbo].[wis_ris_transactions] where to_be_deleted  = 0 and itemid = " + itemid + " and price = " + i.rprice + "   GROUP BY itemid,price   ) as b on a.itemid = b.itemid ) tbl1 inner join ( SELECT TOP 1 * FROM ( select distinct(itemid),itemname from  [IMS].[dbo].[tbl_items_per_transaction_code]  union select distinct(id) as itemid,itemname from  [IMS].[dbo].[tbl_manual_items] union select distinct(itemid)  ,itemname from  [IMS].[dbo].[tbl_wis_transaction] ) as d where d.itemid = " + itemid + ") as c  on tbl1.itemid = c.itemid where tbl1.itemid = " + itemid + " and remaining > 0 ").DataSet();
                    var total_avail = Convert.ToInt32(chck_total_avail.Rows[row_number]["remaining"].ToString());
                  
                    if (total_avail < request_quantity)
                    {
                        return "Ops Remaining Quantity for'" + chck_total_avail.Rows[row_number]["itemname"].ToString() + "' is only '" + total_avail + "' ";
                         
                    }

                    DataTable dt = (@"select tbl1.itemid,tbl1.items_in,tbl1.items_out,tbl1.transcode,tbl1.remaining,tbl1.unitcost, t.itemname,t.unit from ( select a.*,b.items_out,a.items_in - CASE when b.items_out is null then 0 else b.items_out end as remaining  from ( SELECT itemid ,transcode,unitcost, SUM(CASE WHEN in_out = 'IN' THEN quantity ELSE 0 END)  as items_in FROM [IMS].[dbo].[tbl_wis_transaction] GROUP BY itemid,transcode,unitcost ) as a left join ( SELECT itemid,trans_code,  SUM(quantity)  as items_out FROM [IMS].[dbo].[wis_ris_transactions] where to_be_deleted  = 0 GROUP BY itemid,trans_code ) as b on a.itemid = b.itemid and a.transcode = b.trans_code) tbl1 CROSS APPLY (SELECT   * FROM ( select distinct(itemid),itemname,unit from  [IMS].[dbo].[tbl_items_per_transaction_code] union select distinct(id) as itemid,itemname,unit from  [IMS].[dbo].[tbl_manual_items] union select distinct(itemid)  ,itemname,unit from  [IMS].[dbo].[tbl_wis_transaction] ) mi  WHERE   mi.itemid = tbl1.itemid ) t where tbl1.itemid = " + itemid + " and unitcost = " + i.rprice + " and remaining > 0 ").DataSet();

                    var initial_available = dt.Rows[row_number]["remaining"].ToString();
                    var int_available = Convert.ToInt32(initial_available);
                    #endregion

                    if (request_quantity > int_available)
                    {
                        //y -= _available;
                        //total_pass += _available;
                        //row_number += 1;

                        var total_pass = 0.0;
                        var y = request_quantity;
                        for (var x = 0; request_quantity != total_pass; x++)
                        {
                            #region data 
                            var dt_itemid = dt.Rows[row_number]["itemid"].ToString();
                            var dt_itemname = dt.Rows[row_number]["itemname"].ToString();
                            var dt_unitcost = dt.Rows[row_number]["unitcost"].ToString();
                            var dt_unit = dt.Rows[row_number]["unit"].ToString();
                            var dt_transcode = dt.Rows[row_number]["transcode"].ToString();
                            var dt_available = dt.Rows[row_number]["remaining"].ToString();
                            var _available = Convert.ToInt32(dt_available);
                            //var ref_id = dt.Rows[row_number]["id"].ToString();
                            #endregion

                            if (_available >= y)
                            {
                                try
                                { 
                                    string qry = @"insert into [IMS].[dbo].[wis_ris_transactions](officeid,officename,itemid,itemname,unit,quantity,price,date_submited,trans_code,controlno) values ('" +officeid + "','" + officename.Replace("'", "''") + "','" + dt_itemid + "','" + dt_itemname.Replace("'", "''") + "','" + dt_unit + "','" + y + "','" + dt_unitcost + "','" + DateTime.Now.ToString("MM-dd-yyyy") + "','" + dt_transcode + "','" + cn + "')";
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
                                    string qry = @"insert into [IMS].[dbo].[wis_ris_transactions] (officeid,officename,itemid,itemname,unit,quantity,price,date_submited,trans_code,controlno) values ('" + officeid+ "','" + officename.Replace("'", "''") + "','" + dt_itemid + "','" + dt_itemname.Replace("'", "''") + "','" + dt_unit + "','" + _available + "','" + dt_unitcost + "','" + DateTime.Now.ToString("MM-dd-yyyy") + "','" + dt_transcode + "','" + cn + "')";
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
                    }
                    else if (request_quantity <= int_available)
                    {
                        try
                        {
                            string qry = @"insert into [IMS].[dbo].[wis_ris_transactions] (officeid,officename,itemid,itemname,unit,quantity,price,date_submited,trans_code,controlno) values ('" + officeid + "','" + officename.Replace("'", "''") + "','" + dt.Rows[0]["itemid"] + "','" + dt.Rows[0]["itemname"].ToString().Replace("'", "''") + "','" + dt.Rows[0]["unit"] + "','" + request_quantity + "','" + dt.Rows[0]["unitcost"] + "','" + DateTime.Now.ToString("MM-dd-yyyy") + "','" + dt.Rows[0]["transcode"] + "','" + cn + "')";
                            string con = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();
                            string result = OleDbHelper.ExecuteNonQuery(con, System.Data.CommandType.Text, qry).ToString();

                        }
                        catch
                        {
                            error_items += " '" + dt.Rows[0]["itemname"].ToString() + "' ";
                        } 
                    }
                }
                if (error_items == "")
                {
                    return "1";
                }
                else
                {
                    return "Transaction Finish with Error on items '" + error_items + "'";
                }

                #endregion
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string submit_ris_Warehouse(IEnumerable<item> items)
        {
            var error_items = "";

            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));

            String dy = datevalue.Day.ToString();
            String mn = datevalue.Month.ToString();
            String yy = datevalue.Year.ToString();

            string query1 = "exec Proc_GetNewTranNo_ris '[IMS].[dbo].[tbl_t_ris_preparation]','RIS','controlno','" + yy + "','" + mn + "'";
            string strcon = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();
            string control_number = OleDbHelper.ExecuteScalar(strcon, System.Data.CommandType.Text, query1).ToString();

            string query2 = "insert into [IMS].[dbo].[tbl_t_ris_preparation] (controlno,date_time,officeid,officename,preparedby,transaction_type) values ('" + control_number.Trim() + "','" + DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss") + "','" + USER.C_officeID + "','" + USER.C_Office.Replace("'", "''") + "','" + USER.C_eID + "','BORROW')";
            string strcon2 = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();
            string result1 = OleDbHelper.ExecuteNonQuery(strcon2, System.Data.CommandType.Text, query2).ToString();

            try
            {
                #region loop
                foreach (var i in items)
                {
                    var cn = control_number.Trim();
                    var row_number = 0;
                    var itemid = i.itemid;
                    var request_quantity = i.request_quantity;

                    #region check_available
                    DataTable chck_total_avail = (@"select tbl1.itemid,tbl1.items_in,tbl1.items_out,tbl1.remaining , c.itemname from( select a.*,b.items_out,a.items_in - CASE when b.items_out is null then 0 else b.items_out end as remaining  from ( SELECT itemid , SUM(CASE WHEN in_out = 'IN' THEN quantity ELSE 0 END)  as items_in ,unitcost FROM [IMS].[dbo].[tbl_wis_transaction] where itemid = " + itemid + " and unitcost = " + i.rprice + " GROUP BY itemid,unitcost ) as a left join ( SELECT itemid,   SUM(quantity)  as items_out FROM [IMS].[dbo].[wis_ris_transactions] where to_be_deleted  = 0 and itemid = " + itemid + " and price = " + i.rprice + "   GROUP BY itemid,price   ) as b on a.itemid = b.itemid ) tbl1 inner join ( SELECT TOP 1 * FROM ( select distinct(itemid),itemname from  [IMS].[dbo].[tbl_items_per_transaction_code]  union select distinct(id) as itemid,itemname from  [IMS].[dbo].[tbl_manual_items] union select distinct(itemid)  ,itemname from  [IMS].[dbo].[tbl_wis_transaction] ) as d where d.itemid = " + itemid + ") as c  on tbl1.itemid = c.itemid where tbl1.itemid = " + itemid + " and remaining > 0 ").DataSet();

                    var total_avail = 0.0;
                    try
                    {
                        total_avail = Convert.ToDouble(chck_total_avail.Rows[row_number]["remaining"].ToString());
                    }
                    catch (Exception ex)
                    {
                        error_items += " '" + i.itemname + "' ";
                        continue;
                    }

                    if (total_avail < request_quantity)
                    {
                        return "Ops Remaining Quantity for'" + chck_total_avail.Rows[row_number]["itemname"].ToString() + "' is only '" + total_avail + "' "; 
                    }

                    DataTable dt = (@"select tbl1.itemid,tbl1.items_in,tbl1.items_out,tbl1.transcode,tbl1.remaining,tbl1.unitcost, t.itemname,t.unit from ( select a.*,b.items_out,a.items_in - CASE when b.items_out is null then 0 else b.items_out end as remaining  from ( SELECT itemid ,transcode,unitcost, SUM(CASE WHEN in_out = 'IN' THEN quantity ELSE 0 END)  as items_in FROM [IMS].[dbo].[tbl_wis_transaction] GROUP BY itemid,transcode,unitcost ) as a left join ( SELECT itemid,trans_code,  SUM(quantity)  as items_out FROM [IMS].[dbo].[wis_ris_transactions] where to_be_deleted  = 0 GROUP BY itemid,trans_code ) as b on a.itemid = b.itemid and a.transcode = b.trans_code) tbl1 CROSS APPLY (SELECT   * FROM ( select distinct(itemid),itemname,unit from  [IMS].[dbo].[tbl_items_per_transaction_code] union select distinct(id) as itemid,itemname,unit from  [IMS].[dbo].[tbl_manual_items] union select distinct(itemid)  ,itemname,unit from  [IMS].[dbo].[tbl_wis_transaction] ) mi  WHERE   mi.itemid = tbl1.itemid ) t where tbl1.itemid = " + itemid + " and unitcost = " + i.rprice + " and remaining > 0 ").DataSet();


                    var initial_available = 0.0;
                    try
                    {
                        initial_available = Convert.ToDouble(dt.Rows[row_number]["remaining"].ToString());  
                    }
                    catch (Exception ex)
                    {
                        error_items += " '" + i.itemname + "' ";
                        continue;
                    }
                  
                    var int_available = Convert.ToDouble(initial_available);
                    #endregion

                    if (request_quantity > int_available)
                    {
                        //y -= _available;
                        //total_pass += _available;
                        //row_number += 1;

                        var total_pass = 0.0;
                        var y = request_quantity;
                        for (var x = 0; request_quantity != total_pass; x++)
                        {
                            #region data
                            var dt_itemid = dt.Rows[row_number]["itemid"].ToString();
                            var dt_itemname = dt.Rows[row_number]["itemname"].ToString();
                            var dt_unitcost = dt.Rows[row_number]["unitcost"].ToString();
                            var dt_unit = dt.Rows[row_number]["unit"].ToString();
                            var dt_transcode = dt.Rows[row_number]["transcode"].ToString();
                            var dt_available = dt.Rows[row_number]["remaining"].ToString();
                            var _available = Convert.ToInt32(dt_available);
                            //var ref_id = dt.Rows[row_number]["id"].ToString();
                            #endregion

                            if (_available >= y)
                            {
                                try
                                {
                                    string qry = @"insert into [IMS].[dbo].[wis_ris_transactions](officeid,officename,itemid,itemname,unit,quantity,price,date_submited,trans_code,controlno) values ('" + USER.C_officeID + "','" + USER.C_Office.Replace("'", "''") + "','" + dt_itemid + "','" + dt_itemname.Replace("'", "''") + "','" + dt_unit + "','" + y + "','" + dt_unitcost + "','" + DateTime.Now.ToString("MM-dd-yyyy") + "','" + dt_transcode + "','" + cn + "')";
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
                                    string qry = @"insert into [IMS].[dbo].[wis_ris_transactions] (officeid,officename,itemid,itemname,unit,quantity,price,date_submited,trans_code,controlno) values ('" + USER.C_officeID + "','" + USER.C_Office.Replace("'", "''") + "','" + dt_itemid + "','" + dt_itemname.Replace("'", "''") + "','" + dt_unit + "','" + _available + "','" + dt_unitcost + "','" + DateTime.Now.ToString("MM-dd-yyyy") + "','" + dt_transcode + "','" + cn + "')";
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
                    }
                    else if (request_quantity <= int_available)
                    {
                        try
                        {
                            string qry = @"insert into [IMS].[dbo].[wis_ris_transactions] (officeid,officename,itemid,itemname,unit,quantity,price,date_submited,trans_code,controlno) values ('" + USER.C_officeID + "','" +  USER.C_Office.Replace("'", "''") + "','" + dt.Rows[0]["itemid"] + "','" + dt.Rows[0]["itemname"].ToString().Replace("'", "''") + "','" + dt.Rows[0]["unit"] + "','" + request_quantity + "','" + dt.Rows[0]["unitcost"] + "','" + DateTime.Now.ToString("MM-dd-yyyy") + "','" + dt.Rows[0]["transcode"] + "','" + cn + "')";
                            string con = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();
                            string result = OleDbHelper.ExecuteNonQuery(con, System.Data.CommandType.Text, qry).ToString();

                        }
                        catch
                        {
                            error_items += " '" + dt.Rows[0]["itemname"].ToString() + "' ";
                        }
                    }
                }
                if (error_items == "")
                {
                    return "1";
                }
                else
                {
                    return "Transaction Finish with Error on items '" + error_items + "'";
                }

                #endregion
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string submit_added_items(IEnumerable<item> items, string control_number)
        {
            var error_items = "";

            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));

            String dy = datevalue.Day.ToString();
            String mn = datevalue.Month.ToString();
            String yy = datevalue.Year.ToString();

            //string query1 = "exec Proc_GetNewTranNo_ris '[IMS].[dbo].[tbl_t_ris_preparation]','RIS','controlno','" + yy + "','" + mn + "'";
            //string strcon = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();
            //string control_number = OleDbHelper.ExecuteScalar(strcon, System.Data.CommandType.Text, query1).ToString();

            //string query2 = "insert into [IMS].[dbo].[tbl_t_ris_preparation] (controlno,date_time,officeid,officename,preparedby,transaction_type) values ('" + control_number.Trim() + "','" + DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss") + "','" + USER.C_officeID + "','" + USER.C_Office.Replace("'", "''") + "','" + USER.C_eID + "','BORROW')";
            //string strcon2 = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();
            //string result1 = OleDbHelper.ExecuteNonQuery(strcon2, System.Data.CommandType.Text, query2).ToString();

            try
            {
                #region loop
                foreach (var i in items)
                {
                    var cn = control_number.Trim();
                    var row_number = 0;
                    var itemid = i.itemid;
                    var request_quantity = i.request_quantity;

                    #region check_available
                    DataTable chck_total_avail = (@"select tbl1.itemid,tbl1.items_in,tbl1.items_out,tbl1.remaining , c.itemname from( select a.*,b.items_out,a.items_in - CASE when b.items_out is null then 0 else b.items_out end as remaining  from ( SELECT itemid , SUM(CASE WHEN in_out = 'IN' THEN quantity ELSE 0 END)  as items_in ,unitcost FROM [IMS].[dbo].[tbl_wis_transaction] where itemid = " + itemid + " and unitcost = " + i.rprice + " GROUP BY itemid,unitcost ) as a left join ( SELECT itemid,   SUM(quantity)  as items_out FROM [IMS].[dbo].[wis_ris_transactions] where to_be_deleted  = 0 and itemid = " + itemid + " and price = " + i.rprice + "   GROUP BY itemid,price   ) as b on a.itemid = b.itemid ) tbl1 inner join ( SELECT TOP 1 * FROM ( select distinct(itemid),itemname from  [IMS].[dbo].[tbl_items_per_transaction_code]  union select distinct(id) as itemid,itemname from  [IMS].[dbo].[tbl_manual_items] union select distinct(itemid)  ,itemname from  [IMS].[dbo].[tbl_wis_transaction] ) as d where d.itemid = " + itemid + ") as c  on tbl1.itemid = c.itemid where tbl1.itemid = " + itemid + " and remaining > 0 ").DataSet();
                    var total_avail = Convert.ToDouble(chck_total_avail.Rows[row_number]["remaining"].ToString());

                    if (total_avail < request_quantity)
                    {
                        return "Ops Remaining Quantity for'" + chck_total_avail.Rows[row_number]["itemname"].ToString() + "' is only '" + total_avail + "' ";

                    }

                    DataTable dt = (@"select tbl1.itemid,tbl1.items_in,tbl1.items_out,tbl1.transcode,tbl1.remaining,tbl1.unitcost, t.itemname,t.unit from ( select a.*,b.items_out,a.items_in - CASE when b.items_out is null then 0 else b.items_out end as remaining  from ( SELECT itemid ,transcode,unitcost, SUM(CASE WHEN in_out = 'IN' THEN quantity ELSE 0 END)  as items_in FROM [IMS].[dbo].[tbl_wis_transaction] GROUP BY itemid,transcode,unitcost ) as a left join ( SELECT itemid,trans_code,  SUM(quantity)  as items_out FROM [IMS].[dbo].[wis_ris_transactions] where to_be_deleted  = 0 GROUP BY itemid,trans_code ) as b on a.itemid = b.itemid and a.transcode = b.trans_code) tbl1 CROSS APPLY (SELECT   * FROM ( select distinct(itemid),itemname,unit from  [IMS].[dbo].[tbl_items_per_transaction_code] union select distinct(id) as itemid,itemname,unit from  [IMS].[dbo].[tbl_manual_items] union select distinct(itemid)  ,itemname,unit from  [IMS].[dbo].[tbl_wis_transaction] ) mi  WHERE   mi.itemid = tbl1.itemid ) t where tbl1.itemid = " + itemid + " and unitcost = " + i.rprice + " and remaining > 0 ").DataSet();

                    var initial_available = dt.Rows[row_number]["remaining"].ToString();
                    var int_available = Convert.ToDouble(initial_available);
                    #endregion

                    if (request_quantity > int_available)
                    {
                        //y -= _available;
                        //total_pass += _available;
                        //row_number += 1;

                        var total_pass = 0.0;
                        var y = request_quantity;
                        for (var x = 0; request_quantity != total_pass; x++)
                        {
                            #region data
                            var dt_itemid = dt.Rows[row_number]["itemid"].ToString();
                            var dt_itemname = dt.Rows[row_number]["itemname"].ToString();
                            var dt_unitcost = dt.Rows[row_number]["unitcost"].ToString();
                            var dt_unit = dt.Rows[row_number]["unit"].ToString();
                            var dt_transcode = dt.Rows[row_number]["transcode"].ToString();
                            var dt_available = dt.Rows[row_number]["remaining"].ToString();
                            var _available = Convert.ToDouble(dt_available);
                            //var ref_id = dt.Rows[row_number]["id"].ToString();
                            #endregion

                            if (_available >= y)
                            {
                                try
                                {
                                    string qry = @"insert into [IMS].[dbo].[wis_ris_transactions](officeid,officename,itemid,itemname,unit,quantity,price,date_submited,trans_code,controlno) values ('" + USER.C_officeID + "','" + USER.C_Office.Replace("'", "''") + "','" + dt_itemid + "','" + dt_itemname.Replace("'", "''") + "','" + dt_unit + "','" + y + "','" + dt_unitcost + "','" + DateTime.Now.ToString("MM-dd-yyyy") + "','" + dt_transcode + "','" + cn + "')";
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
                                    string qry = @"insert into [IMS].[dbo].[wis_ris_transactions] (officeid,officename,itemid,itemname,unit,quantity,price,date_submited,trans_code,controlno) values ('" + USER.C_officeID + "','" + USER.C_Office.Replace("'", "''") + "','" + dt_itemid + "','" + dt_itemname.Replace("'", "''") + "','" + dt_unit + "','" + _available + "','" + dt_unitcost + "','" + DateTime.Now.ToString("MM-dd-yyyy") + "','" + dt_transcode + "','" + cn + "')";
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
                    }
                    else if (request_quantity <= int_available)
                    {
                        try
                        {
                            string qry = @"insert into [IMS].[dbo].[wis_ris_transactions] (officeid,officename,itemid,itemname,unit,quantity,price,date_submited,trans_code,controlno) values ('" + USER.C_officeID + "','" +  USER.C_Office.Replace("'", "''") + "','" + dt.Rows[0]["itemid"] + "','" + dt.Rows[0]["itemname"].ToString().Replace("'", "''") + "','" + dt.Rows[0]["unit"] + "','" + request_quantity + "','" + dt.Rows[0]["unitcost"] + "','" + DateTime.Now.ToString("MM-dd-yyyy") + "','" + dt.Rows[0]["transcode"] + "','" + cn + "')";
                            string con = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();
                            string result = OleDbHelper.ExecuteNonQuery(con, System.Data.CommandType.Text, qry).ToString();

                        }
                        catch
                        {
                            error_items += " '" + dt.Rows[0]["itemname"].ToString() + "' ";
                        }
                    }
                }
                if (error_items == "")
                {
                    return "1";
                }
                else
                {
                    return "Transaction Finish with Error on items '" + error_items + "'";
                }

                #endregion
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

         
        #endregion
         
    }
}
