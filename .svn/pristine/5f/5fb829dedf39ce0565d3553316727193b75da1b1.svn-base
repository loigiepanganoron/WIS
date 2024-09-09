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
    public class wis_read
    {
        public IEnumerable<dynamic> read_instock()
        {
            List<item> itms = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand(@"SELECT itemid, unit, itemname,SUM(CASE WHEN in_out = 'IN' THEN quantity ELSE 0 END) AS total
FROM  dbo.transact
GROUP BY itemid, itemname, unit", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item off = new item();
                    off.itemid = reader.GetInt32(0);
                    off.unit = reader.GetString(1);
                    off.itemname = reader.GetString(2);
                    off.total = reader.GetInt32(3);
                    off.totalin = off.total + " " + off.unit + "/s"; 
                    itms.Add(off);
                }
                return itms;
            }
        }

        public IEnumerable<dynamic> read_Owneroffice(int itemid)
        {
            List<item> itms = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand(@"SELECT b.OfficeName, a.itemname,a.quantity, a.unit from [IMS].[dbo].[vwtransact] as a
INNER JOIN [pmis].[dbo].[m_vwOffices] as b ON a.officeid = b.OfficeID 
  where itemid = '" + itemid + "' and in_out= 'IN'  GROUP BY a.itemid,a.officeid, a.itemname, a.unit,b.OfficeName,a.quantity", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item off = new item();
                    off.officename = reader.GetString(0);
                    off.itemname = reader.GetString(1);
                    off.quantity = reader.GetInt32(2);
                    off.unit = reader.GetString(3);
                    off.totalin = off.quantity + " " + off.unit + "/s ";
                    itms.Add(off);
                }
                return itms;
            }
        }

        public IEnumerable<dynamic> history_in()
        {
            List<item> itms = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand(@"select a.itemid,a.itemname,a.quantity,a.unit,a.date,b.OfficeName from [IMS].[dbo].[vwtransact] as a
INNER JOIN [pmis].[dbo].[m_vwOffices] as b ON a.officeid = b.OfficeID
where in_out = 'IN'", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item off = new item();
                    off.itemid = reader.GetInt32(0);
                    off.itemname = reader.GetString(1);
                    off.quantity = reader.GetInt32(2);
                    off.unit = reader.GetString(3);
                    off.date = reader.GetDateTime(4);
                    off.officename = reader.GetString(5);
                    off.totalin = off.quantity + " " + off.unit + "/s";
                    
                    itms.Add(off);
                }
                return itms;
            }
        }
        public IEnumerable<dynamic> history_out()
        {
            List<item> itms = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand(@"select a.itemid,a.itemname,a.quantity,a.unit,a.date,b.OfficeName from [IMS].[dbo].[vwtransact] as a
INNER JOIN [pmis].[dbo].[m_vwOffices] as b ON a.officeid = b.OfficeID
where in_out = 'OUT'", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item off = new item();
                    off.itemid = reader.GetInt32(0);
                    off.itemname = reader.GetString(1);
                    off.quantity = reader.GetInt32(2);
                    off.unit = reader.GetString(3);
                    off.date = reader.GetDateTime(4);
                    off.officename = reader.GetString(5);
                    off.totalin = off.quantity + " " + off.unit + "/s";

                    itms.Add(off);
                }
                return itms;
            }
        }
    
        public  IEnumerable<dynamic> new_ris()
        {
            List<item> itms = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                var offid = USER.C_officeID;

                SqlCommand com = new SqlCommand(@"select * from 
(SELECT itemid, unit, itemname,SUM(CASE WHEN in_out = 'IN' THEN quantity ELSE 0 END) 
- SUM(CASE WHEN in_out = 'OUT' THEN quantity ELSE 0 END) AS total
FROM  dbo.transact
GROUP BY itemid, itemname, unit) a 
left join 
(SELECT itemid, SUM(CASE WHEN in_out = 'IN' THEN quantity ELSE 0 END) 
- SUM(CASE WHEN in_out = 'OUT' THEN quantity ELSE 0 END) AS own,AVG(unitcost) as price
FROM            [IMS].[dbo].[transact] where officeid='"+offid+"' GROUP BY  itemid, unit,unitcost) b on a.itemid = b.itemid", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item off = new item();
                    off.itemid = reader.GetInt32(0);
                    off.unit = reader.GetString(1);
                    off.itemname = reader.GetString(2);
                    off.total = reader.GetInt32(3);
                    off.totalin = reader.GetValue(5).ToString();
                    off.ntotal = off.total + " " + off.unit + "/s";
                    off.navailable = off.totalin + " " + off.unit + "/s";
                    itms.Add(off);
                }
                return itms;
            }
        }
         
    }
}