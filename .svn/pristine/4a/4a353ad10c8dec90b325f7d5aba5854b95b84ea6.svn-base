using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace IMS.Models
{

    public class read
    {


        private pmisEntities pmis = new pmisEntities();
        private IMSEntities db = new IMSEntities();
        public IEnumerable<dynamic> plcitems()
        {
            List<item> itms = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand("select * from [IMS].[dbo].[plcitems] ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item off = new item();
                    off.itemid = reader.GetInt32(0);
                    off.itemname = reader.GetString(1);
                    off.unit = reader.GetString(2);
                    off.unitcost = reader.GetString(3);
                    itms.Add(off);
                }
                return itms;
            }
        }
        public IEnumerable<dynamic> plcunits()
        {
            List<item> itms = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand("select * from [IMS].[dbo].[units] ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item off = new item();
                    off.id = reader.GetInt32(0);
                    off.unitname = reader.GetString(1);
                    itms.Add(off);
                }
                return itms;
            }
        } 
        public IEnumerable<dynamic> Transaction(int officeid)
        {
            List<item> itms = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand("select * from [IMS].[dbo].[vwtransact] where officeid='" + officeid + "' ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item off = new item();
                    off.eid = reader.GetInt32(1);
                    off.itemid = reader.GetInt32(2);
                    off.date = reader.GetDateTime(3);
                    off.itemname = reader.GetString(4);
                    off.quantity = reader.GetInt32(5);
                    off.unit = reader.GetString(6);
                    off.in_out = reader.GetString(7);
                    off.empname = reader.GetString(8);
                    off.officeid = reader.GetInt32(9);
                    off.descript = reader.GetValue(10).ToString();
                    off.reid = reader.GetValue(11).ToString();
                    off.ntotal = off.quantity + " " + off.unit + "/s".ToString();

                    itms.Add(off);
                }
                return itms;
            }
        }
        public IEnumerable<dynamic> Stockout(int officeid)
        {
            List<item> itms = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand("select * from [IMS].[dbo].[vwstockoutgrid] where officeid='" + officeid + "' and total > '0'", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item off = new item();
                    off.officeid = reader.GetInt32(0);
                    off.itemid = reader.GetInt32(2);
                    off.itemname = reader.GetString(3);
                    off.total = reader.GetInt32(4);
                    off.unit = reader.GetString(5);
                    off.ntotal = off.total + " " + off.unit + "/s".ToString();
                    off.unitcost = reader.GetValue(6).ToString();
                    off.srcid = reader.GetInt32(7);
                    off.accountid = reader.GetInt32(8);
                    off.tid = reader.GetInt32(9);
                    off.obj = reader.GetInt32(10);
                    itms.Add(off);
                }
                return itms;
            }

            ////List<vwstockoutgrid> fuck = db.vwstockoutgrids.Where(a => a.officeid == officeid).ToList();
            ////epsws.serviceSoapClient r = new epsws.serviceSoapClient();
            ////var rec = r.POItems(officeid).AsEnumerable().Select(b => new
            ////{
            ////    itemid = b.Field<int>("itemid"),
            ////    itemname = b.Field<string>("itemname"),
            ////    unit = b.Field<string>("unit")
            ////}).GroupBy(c => c.itemid).Select(group => group.FirstOrDefault()).ToList();

            ////var combe = fuck.Join(rec.AsEnumerable(), stockoutgrid => stockoutgrid.itemid,
            ////            row => row.itemid,
            ////            (nnn, li) => new
            ////            {
            ////                id = nnn.trnid,
            ////                itemid = nnn.itemid,
            ////                unit = li.unit,
            ////                itemname = li.itemname,
            ////                newtotal = nnn.total + " " + li.unit + "/s",
            ////                verynewtotal = nnn.total,
            ////                total = nnn.total + " " + li.unit + "/s",
            ////            }
            ////            );
            ////var filt = combe;
            ////var k = filt.Where(a => a.verynewtotal > 0).ToList();
            ////return k;
        }
        public IEnumerable<dynamic> Released(int officeid)
        {
            List<item> itms = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand("select * from [IMS].[dbo].[vwrecieve] where officeid='" + officeid + "' and total > '0'", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item off = new item();
                    off.officeid = reader.GetInt32(0);
                    off.itemid = reader.GetInt32(2);
                    off.itemname = reader.GetString(3);
                    off.total = reader.GetInt32(4);
                    off.unit = reader.GetString(5);
                    off.ntotal = off.total + " " + off.unit + "/s".ToString();
                    itms.Add(off);
                }
                return itms;
            }

            //List<vwrecieve> dawata = db.vwrecieves.Where(a => a.officeid == officeid).ToList();
            //epsws.serviceSoapClient r = new epsws.serviceSoapClient();
            //var rec = r.POItems(officeid).AsEnumerable().Select(b => new
            //{
            //    itemid = b.Field<int>("itemid"),
            //    itemname = b.Field<string>("itemname"),
            //    unit = b.Field<string>("unit")
            //}).GroupBy(c => c.itemid).Select(group => group.FirstOrDefault()).ToList();

            //var recieve = dawata.Join(rec.AsEnumerable(), stockoutgrid => stockoutgrid.itemid,
            //            row => row.itemid,
            //            (nnn, li) => new
            //            {
            //                id = nnn.trnid,
            //                tobeout = nnn.total,
            //                itemid = nnn.itemid,
            //                itemname = li.itemname,
            //                total = nnn.total + " " + li.unit + "/s",
            //            }
            //            );
            //var filtereceive = recieve;
            //var l = filtereceive.Where(a => a.tobeout > 0).ToList();
            //return l;
        }
        public IEnumerable<dynamic> TotalAll(int officeid, string all)
        {
            if (all == "All")
            {
                List<item> itms = new List<item>();
                using (SqlConnection con = new SqlConnection(common.MyConnection()))
                {
                    SqlCommand com = new SqlCommand("select * from [IMS].[dbo].[vwAll] ", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        item off = new item();
                        off.itemname = reader.GetString(1);
                        off.unit = reader.GetString(2);
                        off.total = reader.GetInt32(4);
                        off.ntotal = off.total + " " + off.unit + "/s".ToString();

                        itms.Add(off);
                    }
                    return itms;
                }
            }

            else
            {
                List<item> itms = new List<item>();
                using (SqlConnection con = new SqlConnection(common.MyConnection()))
                {
                    SqlCommand com = new SqlCommand("select * from [IMS].[dbo].[vwstockoutgrid]  where officeid='" + officeid + "' ", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        item off = new item();
                        off.itemname = reader.GetString(3);
                        off.total = reader.GetInt32(4);
                        off.unit = reader.GetString(5);
                        off.ntotal = off.total + " " + off.unit + "/s".ToString();

                        itms.Add(off);
                    }
                    return itms;
                }
            }
        }
        public IEnumerable<dynamic> TotalOut(int officeid, string all)
        {
            if (all == "All")
            {
                List<item> itms = new List<item>();
                using (SqlConnection con = new SqlConnection(common.MyConnection()))
                {
                    SqlCommand com = new SqlCommand("select * from [IMS].[dbo].[vwAllout] ", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        item off = new item();
                        off.itemname = reader.GetString(2);
                        off.total = reader.GetInt32(4);
                        off.unit = reader.GetString(3);
                        off.ntotal = off.total + " " + off.unit + "/s".ToString();


                        itms.Add(off);
                    }
                    return itms;
                }
            }
            else
            {
                List<item> itms = new List<item>();
                using (SqlConnection con = new SqlConnection(common.MyConnection()))
                {
                    SqlCommand com = new SqlCommand("select * from [IMS].[dbo].[vwrecieve] where officeid = '" + officeid + "' ", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        item off = new item();
                        off.itemname = reader.GetString(3);
                        off.total = reader.GetInt32(4);
                        off.unit = reader.GetString(5);
                        off.ntotal = off.total + " " + off.unit + "/s".ToString();


                        itms.Add(off);
                    }
                    return itms;
                }
            }
        }
        public IEnumerable<dynamic> readplcgrid()
        {
            List<item> itms = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand("select * from [IMS].[dbo].[vwplcgrid] where total > 0", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item off = new item();
                    off.itemid = reader.GetInt32(0);
                    off.unit = reader.GetString(1);
                    off.itemname = reader.GetString(2);
                    off.average = reader.GetValue(3).ToString();
                    off.quit = reader.GetValue(4).ToString();
                    
                    off.ntotal = off.quit + " " + off.unit + "/s".ToString();
                    itms.Add(off);
                }
                return itms;
            }
        }
        public IEnumerable<dynamic> plctrans()
        {
            List<item> itms = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand("select * from [IMS].[dbo].[plc] ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item off = new item();
                    off.eName = reader.GetString(1);
                    off.itemname = reader.GetString(4);
                    off.date = reader.GetDateTime(2);
                    off.quit = reader.GetValue(6).ToString();
                    off.unit = reader.GetString(7);
                    off.in_out = reader.GetString(5);
                    off.plceid = reader.GetValue(10).ToString();
                    off.reid = reader.GetValue(11).ToString();

                    itms.Add(off);
                }
                return itms;
            }
        }
        public IEnumerable<dynamic> chart(int itemid)
        {
            List<item> itms = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand("select * from [IMS].[dbo].[vwplcchart] where itemid='" + itemid + "' ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item off = new item();
                    off.itemid = reader.GetInt32(0);
                    off.unit = reader.GetString(1);
                    off.itemname = reader.GetString(2);
                    off.navailable = reader.GetValue(3).ToString();
                    off.nreleased = reader.GetValue(4).ToString();
                    //off.ntotal = off.total + " " + off.unit + "/s".ToString();
                    itms.Add(off);
                }
                return itms;
            }
        }
        public IEnumerable<dynamic> wischart( int officeid,int itemid)
        {
            List<item> itms = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand(@"select officeid,itemname,itemid,
SUM(CASE WHEN in_out = 'IN' THEN quantity ELSE 0 END)  AS totalin,
SUM(CASE WHEN in_out = 'OUT' THEN quantity ELSE 0 END)  AS totalout,
unit 
FROM [IMS].[dbo].[transact] where officeid='"+officeid+"' and itemid = '"+itemid+"' GROUP BY itemname,itemid,officeid,unit", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item off = new item();
                    
                    off.itemname = reader.GetString(1);
                    off.navailable = reader.GetValue(3).ToString();
                    off.nreleased = reader.GetValue(4).ToString();
                    off.unit = reader.GetString(5);
                    //off.ntotal = off.total + " " + off.unit + "/s".ToString();
                    itms.Add(off);
                }
                return itms;
            }
        }
        public IEnumerable<dynamic> readbyofficeout()
        {
            var officeid = USER.C_officeID;
            List<item> itms = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand(@"select a.*,b.reorder_point from ( select * from [IMS].[dbo].[vwbyofficeout] where officeid= " + officeid + " and total > 0   ) as a left join  [IMS].[dbo].[tbl_l_reorder] as b on a.item_id = b.itemid and b.officeid = " + officeid+"   order by itemname", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item off = new item();
                    off.itemid = (Convert.IsDBNull(reader["item_id"]) ? 0 : (int)(reader["item_id"]));
                    off.itemname = reader["itemname"] == DBNull.Value ? (string)null : (string)reader["itemname"];
                    double qq = (Convert.IsDBNull(reader["total"]) ? 0 : (double)(reader["total"])); 
                    off.quit = qq.ToString();
                    off.unit = reader["unit"] == DBNull.Value ? (string)null : (string)reader["unit"];
                    off.reorder = (Convert.IsDBNull(reader["reorder_point"]) ? 0 : (double)(reader["reorder_point"]));
                    off.ntotal = off.quit + " " + off.unit + "/s".ToString();
                    itms.Add(off);
                     
                }
                return itms;
            }
        }
        public IEnumerable<dynamic> byoffchart( int itemid)
        {
            var officeid = USER.C_officeID;
            List<item> itms = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand(@"select officeid,itemname,item_id,
SUM(CASE WHEN in_out = 'IN' THEN quantity ELSE 0 END)  AS totalin,
SUM(CASE WHEN in_out = 'OUT' THEN quantity ELSE 0 END)  AS totalout,
unit 
FROM [IMS].[dbo].[transaction] where officeid='" + officeid + "' and item_id = '" + itemid + "' GROUP BY itemname,item_id,officeid,unit", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item off = new item();

                    off.itemname = reader.GetString(1);
                    off.navailable = reader.GetValue(3).ToString();
                    off.nreleased = reader.GetValue(4).ToString();
                    off.unit = reader.GetString(5);
                    //off.ntotal = off.total + " " + off.unit + "/s".ToString();
                    itms.Add(off);
                }
                return itms;
            }
        }
        public IEnumerable<dynamic> byofftrans()
        {
            var offid = USER.C_officeID;
            List<item> itms = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand("select * from [IMS].[dbo].[transaction] where officeid ='"+offid+"' and ref_id is not null ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item off = new item();
                    off.eid = (Convert.IsDBNull(reader["eid"]) ? 0 : (int)(reader["eid"]));
                    off.req_eid = (Convert.IsDBNull(reader["reid"]) ? 0 : (int)(reader["reid"]));
                    off.itemid = (Convert.IsDBNull(reader["item_id"]) ? 0 : (int)(reader["item_id"])); ;
                    off.quantity = (Convert.IsDBNull(reader["quantity"]) ? 0 : (double)(reader["quantity"])); ;
                    off.unit = reader["unit"] == DBNull.Value ? (string)null : (string)reader["unit"];
                    off.in_out = reader["in_out"] == DBNull.Value ? (string)null : (string)reader["in_out"]; ;
                    off.itemname = reader["itemname"] == DBNull.Value ? (string)null : (string)reader["itemname"]; ;
                    off.date =  (DateTime) reader["stock_date"];
                    itms.Add(off);
                }
                return itms;
            }
        }
        public IEnumerable<dynamic> get_ris_code()
        {
            List<item> itms = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand("select distinct(controlno) as controlno  FROM [IMS].[dbo].[transaction] where officeid  =  '"+USER.C_officeID+"' and controlno is not null and in_out = 'OUT'", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item off = new item(); 
                    off.controlno = reader["controlno"] == DBNull.Value ? (string)null : (string)reader["controlno"]; ; 
                    itms.Add(off);
                }
                return itms;
            }
        }
        public IEnumerable<item> getOutItems()
        {
            List<item> itms = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand(@"select item_id,itemname,unit,reid,sum(quantity) as quantity  FROM [IMS].[dbo].[transaction]  where in_out = 'OUT' and officeid = '"+USER.C_officeID+"' and description != 'done' and ref_id is null group by item_id,itemname,unit,reid", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item off = new item(); 
                    off.itemid = (Convert.IsDBNull(reader["item_id"]) ? 0 : (int)(reader["item_id"]));
                    off.itemname = reader["itemname"] == DBNull.Value ? (string)null : (string)reader["itemname"];
                    off.unit = reader["unit"] == DBNull.Value ? (string)null : (string)reader["unit"];
                    off.request_quantity = (Convert.IsDBNull(reader["quantity"]) ? 0 : (int)(reader["quantity"]));
                    off.eid = (Convert.IsDBNull(reader["reid"]) ? 0 : (int)(reader["reid"]));
                    off.reid = off.eid.ToString();
                    itms.Add(off);
                }
                return itms;
            }
        }
        public IEnumerable<item> gridConsume(int officeid, string from , string to)
        {  
            List<item> itms = new List<item>();
           
                using (SqlConnection con = new SqlConnection(common.MyConnection()))
                {
                    SqlCommand com = new SqlCommand(@"select item_id,itemname ,SUM(CASE WHEN in_out = 'OUT' THEN (case when ref_id is null then 0 else quantity end) ELSE 0 END) as consume,unit FROM [IMS].[dbo].[transaction]
    where officeid = '" + officeid + "' and in_out='OUT' and cast(stock_date as date) between '" + from + "' and '" + to + "' group by item_id,itemname,unit order by itemname asc", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        item off = new item();
                        off.itemid = (Convert.IsDBNull(reader["item_id"]) ? 0 : (int)(reader["item_id"]));
                        off.itemname = reader["itemname"] == DBNull.Value ? (string)null : (string)reader["itemname"];
                        off.unit = reader["unit"] == DBNull.Value ? (string)null : (string)reader["unit"];
                        off.quantity = (Convert.IsDBNull(reader["consume"]) ? 0 : (double)(reader["consume"]));
                        itms.Add(off); 
                    }

                }
                return itms;
            
        }
        public IEnumerable<item> gridOfficeRemaining(int officeid)
        {
            List<item> itms = new List<item>();

            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand(@"select * from [IMS].[dbo].[vwbyofficeout] where officeid='" + officeid + "' and total > 0 order by itemname asc", con);
                con.Open();

                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item off = new item();
                    off.itemid = (Convert.IsDBNull(reader["item_id"]) ? 0 : (int)(reader["item_id"]));
                    off.itemname = reader["itemname"] == DBNull.Value ? (string)null : (string)reader["itemname"];
                    off.unit = reader["unit"] == DBNull.Value ? (string)null : (string)reader["unit"];
                    off.quantity = (Convert.IsDBNull(reader["total"]) ? 0 : (double)(reader["total"]));
                    itms.Add(off);
                }

            }
            return itms;

        }
        public IEnumerable<item> available_delivery()
        {  
            List<item> itms = new List<item>();
           
                using (SqlConnection con = new SqlConnection(common.MyConnection()))
                {
                    SqlCommand com = new SqlCommand(@"select year,quarter,mooe_no,dbm_bb from (select distinct(transaction_code)  FROM [IMS].[dbo].[tbl_items_per_transaction_code] where officeid = '" + USER.C_officeID + "') tbl_1 left join (select distinct(transcode),year, case when quarter =1 then '1ST QUARTER'  when quarter = 2 then '2ND QUARTER' when quarter=3 then '3RD QUARTER' when quarter = 4 then '4TH QUARTER' end  as  quarter,case when  mooe_no = 2 then 'MOOE' else 'NON-OFFICE' end as mooe_no, case when dbm_bb = 1 then 'DBM' else 'BULK BIDDING' end as dbm_bb from [IMS].[dbo].[tbl_wis_transaction] where in_out = 'IN'  group by transcode,year, quarter,mooe_no,dbm_bb) tbl_2 on tbl_1.transaction_code = tbl_2.transcode group by year,quarter,mooe_no,dbm_bb", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        item off = new item();
                        off.year = (Convert.IsDBNull(reader["year"]) ? 0 : (int)(reader["year"]));
                        off.quarter_str = reader["quarter"] == DBNull.Value ? (string)null : (string)reader["quarter"];
                        off.mooe_no_str = reader["mooe_no"] == DBNull.Value ? (string)null : (string)reader["mooe_no"];
                        off.dbm_bb_str = reader["dbm_bb"] == DBNull.Value ? (string)null : (string)reader["dbm_bb"]; 
                        itms.Add(off); 
                    }

                }
                return itms; 
        }

        public IEnumerable<dynamic> edit_quantity()
        {
            var offid = USER.C_officeID;
            List<item> itms = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand(@"select tbl_1.ID,tbl_1.item_id,tbl_1.itemname,tbl_1.unit,tbl_1.quantity,tbl_1.stock_date,isnull(tbl_2.quantity,0) as out_quantity from ( select  * from [IMS].[dbo].[transaction] where officeid = '" + offid + "' and in_out = 'IN' ) tbl_1 left join (select  item_id,sum(quantity) as quantity,ref_id from [IMS].[dbo].[transaction]  where in_out = 'OUT' and officeid = '" + offid + "' group by  item_id,ref_id )tbl_2 on tbl_1.item_id = tbl_2.item_id and tbl_1.ID = tbl_2.ref_id", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item off = new item();
                    off.id = (Convert.IsDBNull(reader["ID"]) ? 0 : (int)(reader["ID"])); 
                    off.itemid = (Convert.IsDBNull(reader["item_id"]) ? 0 : (int)(reader["item_id"])); 
                    off.itemname = reader["itemname"] == DBNull.Value ? (string)null : (string)reader["itemname"]; 
                    off.quantity = (Convert.IsDBNull(reader["quantity"]) ? 0 : (double)(reader["quantity"]));
                    off.recieve_quantity = (Convert.IsDBNull(reader["out_quantity"]) ? 0.0 : (double)(reader["out_quantity"])); 
                    off.unit = reader["unit"] == DBNull.Value ? (string)null : (string)reader["unit"];  
                    off.date = (DateTime)reader["stock_date"];
                    itms.Add(off);
                }
                return itms;
            }
        }

        public IEnumerable<item> get_manual_items()
        {  
            List<item> itms = new List<item>();
           
                using (SqlConnection con = new SqlConnection(common.MyConnection()))
                {
                    SqlCommand com = new SqlCommand(@"select * from [IMS].[dbo].[tbl_manual_items]", con);
                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        item off = new item();
                        off.id = (Convert.IsDBNull(reader["id"]) ? 0 : (int)(reader["id"])); 
                        off.itemname = reader["itemname"] == DBNull.Value ? (string)null : (string)reader["itemname"];
                        off.unit = reader["unit"] == DBNull.Value ? (string)null : (string)reader["unit"]; 
                        itms.Add(off); 
                    }
                }
                return itms; 
        }

        public IEnumerable<item> searchItem(string itemname)
        {
            List<item> itms = new List<item>();

            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand(@"select * from [IMS].[dbo].[vw_searchAvailable] where itemname like '%"+itemname+"%'", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item off = new item();
                    off.itemname = reader["itemname"] == DBNull.Value ? (string)null : (string)reader["itemname"];
                    off.year = (Convert.IsDBNull(reader["year"]) ? 0 : (int)(reader["year"]));
                    off.quarter_str = reader["quarter"] == DBNull.Value ? (string)null : (string)reader["quarter"];
                    off.mooe_no_str = reader["mooe_no"] == DBNull.Value ? (string)null : (string)reader["mooe_no"];
                    off.dbm_bb_str = reader["dbm_bb"] == DBNull.Value ? (string)null : (string)reader["dbm_bb"];
                    off.quantity = (Convert.IsDBNull(reader["remaining"]) ? 0 : (int)(reader["remaining"]));
                    off.account_str = reader["account"] == DBNull.Value ? (string)null : (string)reader["account"];
                    itms.Add(off);
                }
            }
            return itms;
        }


        public IEnumerable<dynamic> readItemsWithReorder()
        {
            var offid = USER.C_officeID;
            List<item> itms = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand(@"select a.*,b.itemname ,b.unit,c.reorder_point from ( select distinct(item_id) from  [IMS].[dbo].[transaction] where officeid = "+offid+" ) as a outer apply ( select top 1 item_id,itemname,unit from  [IMS].[dbo].[transaction] where item_id = a.item_id ) as b left join  [IMS].[dbo].[tbl_l_reorder]  as c on a.item_id = c.itemid and c.officeid = "+offid+"  ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item off = new item();
                    
                    off.itemid = (Convert.IsDBNull(reader["item_id"]) ? 0 : (int)(reader["item_id"]));
                    off.itemname = reader["itemname"] == DBNull.Value ? (string)null : (string)reader["itemname"];
                    off.quantity = (Convert.IsDBNull(reader["reorder_point"]) ? 0 : (double)(reader["reorder_point"]));
                    off.unit = reader["unit"] == DBNull.Value ? (string)null : (string)reader["unit"];
                    itms.Add(off);
                }
                return itms;
            }
        }
        public IEnumerable<dynamic> readItemsWithReorderPGSO()
        {
            var offid = USER.C_officeID;
            List<item> itms = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand(@"select a.*, b.itemname  ,b.unit,c.reorder_point from ( select distinct(itemid) from  [IMS].[dbo].[tbl_wis_transaction]   ) as a outer apply ( select top 1 itemid,LTRIM(RTRIM(itemname)) as itemname ,unit from [IMS].[dbo].[tbl_wis_transaction]    where itemid = a.itemid ) as b left join  [IMS].[dbo].[tbl_l_reorder]  as c on a.itemid = c.itemid and c.officeid = 0 order by b.itemname ASC", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item off = new item();

                    off.itemid = (Convert.IsDBNull(reader["itemid"]) ? 0 : (int)(reader["itemid"]));
                    off.itemname = reader["itemname"] == DBNull.Value ? (string)null : (string)reader["itemname"];
                    off.quantity = (Convert.IsDBNull(reader["reorder_point"]) ? 0 : (double)(reader["reorder_point"]));
                    off.unit = reader["unit"] == DBNull.Value ? (string)null : (string)reader["unit"];
                    itms.Add(off);
                }
                return itms;
            }
        }
    }
}


