using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IMS.@class;
using System.Data.SqlClient;
using System.Data;
using System.Data.Objects;
using System.Globalization;
using System.Data.OleDb;
using System.Web.Mvc;
namespace IMS.Models
{
    public class mymodel
    {
        private pmisEntities pmis = new pmisEntities();
        private IMSEntities db = new IMSEntities();
        //sa ihatag na traw ni sir ;)
        public int ID { get; set; }
        public string itemName { get; set; }
        public int itemid { get; set; }
        public string unit { get; set; }

        public IEnumerable<dynamic> readtransaction(string status, int itemid, DateTime start, DateTime end)
        {
            List<vwTransaction> listitems = new List<vwTransaction>();
            List<vwTransaction> statusFilter = new List<vwTransaction>();
            if (itemid == 0)
                listitems = db.vwTransactions.Where(a => a.officeid == USER.C_officeID
                && EntityFunctions.TruncateTime(a.stock_date) >= start.Date && EntityFunctions.TruncateTime(a.stock_date) <= end.Date).ToList();
            else
                listitems = db.vwTransactions.Where(a => a.officeid == USER.C_officeID && a.item_id == itemid
                && EntityFunctions.TruncateTime(a.stock_date) >= start.Date && EntityFunctions.TruncateTime(a.stock_date) <= end.Date).ToList();

            if (status != "")
                statusFilter = listitems.Where(asdf => asdf.in_out == status).ToList();
            else
                statusFilter = listitems;

            epsws.serviceSoapClient r = new epsws.serviceSoapClient();
            var rec = r.POItems(USER.C_officeID).AsEnumerable().Select(b => new
            {
                itemid = b.Field<int>("itemid"),
                itemName = b.Field<string>("itemname"),
                unit = b.Field<string>("unit")
            }).GroupBy(c => c.itemid).Select(group => group.FirstOrDefault()).ToList();

            var combine = statusFilter
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
                            qty = nnn.quantity, // + " " + li.unit + "/s",
                            unit = li.unit
                        });
            return combine;
        }

        public IEnumerable<dynamic> reademployee(string status, int eid, int itemid, DateTime start, DateTime end)
        {
            List<vwTransaction> listitems = new List<vwTransaction>();
            List<vwTransaction> statusFilter = new List<vwTransaction>();
            if (itemid == 0)
                listitems = db.vwTransactions.Where(a => a.officeid == USER.C_officeID && a.eid == eid
                && EntityFunctions.TruncateTime(a.stock_date) >= start.Date && EntityFunctions.TruncateTime(a.stock_date) <= end.Date).ToList();
            else
                listitems = db.vwTransactions.Where(a => a.officeid == USER.C_officeID && a.eid == eid && a.item_id == itemid
                && EntityFunctions.TruncateTime(a.stock_date) >= start.Date && EntityFunctions.TruncateTime(a.stock_date) <= end.Date).ToList();

            if (status != "")
                statusFilter = listitems.Where(asdf => asdf.in_out == status).ToList();
            else
                statusFilter = listitems;

            epsws.serviceSoapClient r = new epsws.serviceSoapClient();
            var rec = r.POItems(USER.C_officeID).AsEnumerable().Select(b => new
            {
                itemid = b.Field<int>("itemid"),
                itemName = b.Field<string>("itemname"),
                unit = b.Field<string>("unit")
            }).GroupBy(c => c.itemid).Select(group => group.FirstOrDefault()).ToList();

            var combine = statusFilter
                .Join(rec.AsEnumerable(),
                        listItem => listItem.item_id,
                        row => row.itemid,
                        (nnn, li) => new
                        {
                            stockdate = Convert.ToDateTime(nnn.stock_date),
                            //eName = nnn.eName,
                            in_out = nnn.in_out,
                            //ID = nnn.ID,
                            //itemid = nnn.item_id,
                            itemName = li.itemName,
                            qty = nnn.quantity, // + " " + li.unit + "/s",
                            unit = li.unit
                        }).OrderBy(ord => ord.stockdate).ThenBy(ord2 => ord2.itemName);
            return combine;
        }

        public IEnumerable<dynamic> readnewreport(string status, int itemid, DateTime start, DateTime end, int offid, String off)
        {
            string qry = "";

            if (itemid == 0 && status == "")
            {
                qry = "select * from [IMS].[dbo].[vwtransact] where officeid = '" + offid + "' and date >= '" + start + "' and date <= '" + end + "'";
            }
            // if (itemid != 0 && status != "")
            else
            {
                qry = "select * from [IMS].[dbo].[vwtransact] where officeid = '" + offid + "' and itemid = '" + itemid + "'   and date >= '" + start + "' and date <= '" + end + "' and in_out = '" + status + "'";
            }
            if (status != "" && itemid == 0)
            {
                qry = "select * from [IMS].[dbo].[vwtransact] where officeid = '" + offid + "' and date >= '" + start + "' and date <= '" + end + "' and in_out = '" + status + "'";
            }
            if (status == "" && itemid != 0)
            {
                qry = "select * from [IMS].[dbo].[vwtransact] where officeid = '" + offid + "' and itemid = '" + itemid + "'  and date >= '" + start + "' and date <= '" + end + "'";
            }


            List<item> itms = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand(qry, con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item off3 = new item();
                    off3.itemid = reader.GetInt32(2);
                    off3.date = reader.GetDateTime(3);
                    off3.itemname = reader.GetString(4);
                    off3.quantity = reader.GetInt32(5);
                    off3.unit = reader.GetString(6);
                    off3.in_out = reader.GetString(7);
                    off3.eName = reader.GetString(8);
                    off3.officeid = reader.GetInt32(9);
                    off3.qty = off3.quantity + "" + off3.unit + "/s,".ToString();
                    itms.Add(off3);
                }
                return itms;
            }
        }

        public IEnumerable<dynamic> readall()
        {
            List<item> itms = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand("select * from [IMS].[dbo].[vwrptSummaryAll] ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item off3 = new item();
                    off3.itemid = reader.GetInt32(0);
                    off3.itemname = reader.GetString(1);
                    off3.unit = reader.GetString(2);
                    off3.available = reader.GetInt32(3);
                    off3.released = reader.GetInt32(4);
                    off3.navailable = off3.available + " " + off3.unit + "/s";
                    off3.nreleased = off3.released + " " + off3.unit + "/s";
                    itms.Add(off3);
                }
                return itms;
            }

            //List<vwrptSummaryAll> listItems = new List<vwrptSummaryAll>();
            //var listitems = db.vwrptSummaryAlls.ToList();

            //epsws.serviceSoapClient r = new epsws.serviceSoapClient();
            //var rec = r.POItems(USER.C_officeID).AsEnumerable().Select(b => new
            //{
            //    itemid = b.Field<int>("itemid"),
            //    itemName = b.Field<string>("itemname"),
            //    unit = b.Field<string>("unit")
            //}).GroupBy(c => c.itemid).Select(group => group.FirstOrDefault()).ToList();

            //var combine = listitems
            //  .Join(rec.AsEnumerable(),
            //          listItem => listItem.itemid,
            //          row => row.itemid,
            //          (nnn, li) => new
            //          {
            //              itemid = Convert.ToInt32(nnn.itemid),
            //              itemname = li.itemName,
            //              available = nnn.available + " " + li.unit + "/s",
            //              released = nnn.released + " " + li.unit + "/s"
            //          });
            //return combine;
        }

        //public IEnumerable<dynamic> byoffice()
        //{
        //    List<vwrptSummaryAll> listItems = new List<vwrptSummaryAll>();

        //    var listitems = db.vwrptSummaryAlls.ToList();

        //    epsws.serviceSoapClient r = new epsws.serviceSoapClient();
        //    var rec = r.POItems(USER.C_officeID).AsEnumerable().Select(b => new
        //    {
        //        itemid = b.Field<int>("itemid"),
        //        itemName = b.Field<string>("itemname"),
        //        unit = b.Field<string>("unit")
        //    }).GroupBy(c => c.itemid).Select(group => group.FirstOrDefault()).ToList();

        //    var combine = listitems
        //      .Join(rec.AsEnumerable(),
        //              listItem => listItem.itemid,
        //              row => row.itemid,
        //              (nnn, li) => new
        //              {
        //                  itemid = Convert.ToInt32(nnn.itemid),
        //                  itemname = li.itemName,
        //                  available = nnn.available + " " + li.unit + "/s",
        //                  released = nnn.released + " " + li.unit + "/s"
        //              });
        //    return combine;
        //}
      
    }
}

