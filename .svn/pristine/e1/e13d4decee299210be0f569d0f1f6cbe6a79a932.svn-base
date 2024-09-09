using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Transports;
using IMS.Models;
using Kendo.Mvc.UI;
using System.Data.SqlClient;
using IMS.@class;

namespace IMS
{
    public class MyHub1 : Hub
    {
        public void Hello()
        {
            Clients.All.hello("Hey");
        }
       
        public IEnumerable<item> read()
        {
            List<item> item = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand(@"
select *,b.total_in - d.total_out as available , a.total_quantity - c.office_out as remaining_office_quantity from 
(select officeid,itemid,itemname,sum(quantity) as total_quantity,unit from [IMS].[dbo].[tbl_items_per_transaction_code]
where officeid = '"+USER.C_officeID+"' group by officeid,itemid,itemname,unit) a left join (select itemid,SUM(CASE WHEN in_out = 'IN' THEN quantity ELSE 0 END) as total_in from [IMS].[dbo].[tbl_wis_transaction] group by itemid) b on a.itemid = b.itemid left join (select  itemid,sum(quantity) as total_out from [IMS].[dbo].[wis_ris_transactions] group by itemid) d on a.itemid = d.itemid left join (select itemid,SUM(quantity) as office_out from [IMS].[dbo].[wis_ris_transactions] where officeid = '"+USER.C_officeID+"' group by officeid,itemid,itemname) c on a.itemid = c.itemid", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item c = new item();
                    c.itemid = (Convert.IsDBNull(reader["itemid"]) ? 0 : (int)(reader["itemid"]));
                    c.itemname = reader["itemname"] == DBNull.Value ? (string)null : (string)reader["itemname"];
                    c.quantity = (Convert.IsDBNull(reader["total_quantity"]) ? 0 : (double)(reader["total_quantity"]));
                    c.unit = reader["unit"] == DBNull.Value ? (string)null : (string)reader["unit"];
                    c.available = (Convert.IsDBNull(reader["total_in"]) ? 0 : (double)(reader["total_in"])) - (Convert.IsDBNull(reader["total_out"]) ? 0 : (double)(reader["total_out"]));
                    c.totalout = (Convert.IsDBNull(reader["office_out"]) ? 0 : (double)(reader["office_out"]));
                    c.remaining = c.quantity - c.totalout;
                    c.request_quantity = c.remaining;
                    item.Add(c);
                }
            }
            return item;
        }
    }
}