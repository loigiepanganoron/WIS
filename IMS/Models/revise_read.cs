using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using IMS.Controllers;
using IMS.@class;
using System.Configuration;

namespace IMS.Models
{
    public class revise_read
    {
        //epsws.serviceSoapClient r = new epsws.serviceSoapClient();
        public IEnumerable<item> read_in(string transaction_code)
        {
            List<item> item = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand(@"select itemid,itemname,sum(quantity) as total_in,unit   FROM [IMS].[dbo].[tbl_wis_transaction]
  where transcode = '" + transaction_code + "'and in_out = 'IN' group by itemid,unit,itemname", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item c = new item();
                    c.itemid = (Convert.IsDBNull(reader["itemid"]) ? 0 : (int)(reader["itemid"]));
                    c.itemname = reader["itemname"] == DBNull.Value ? (string)null : (string)reader["itemname"];
                    c.quantity = (Convert.IsDBNull(reader["total_in"]) ? 0 : (int)(reader["total_in"]));
                    c.unit = reader["unit"] == DBNull.Value ? (string)null : (string)reader["unit"];
                    item.Add(c);
                }
            }
            return item;
        }

        public string revise_in(IEnumerable<item> items,string transcode , int from)
        {
            var eid = USER.C_eID;
            string query = "";
            foreach (var i in items)
            {
                query += "insert into [IMS].[dbo].[tbl_wis_transaction] (eid,itemid,itemname,date,quantity,in_out,unit,unitcost,transcode,starting_balance,year,quarter,mooe_no,dbm_bb) values ('" + eid + "','" + i.itemid + "','" + i.itemname.Replace("'", "''") + "','" + DateTime.Now.ToString("MM-dd-yyyy") + "','" + i.quantity + "','IN','" + i.unit + "','" + i.rprice + "','"+transcode+"','"+from+"','"+i.year+"','"+i.quarter+"','"+i.mooe_no+"','"+i.dbm_bb+"');" + System.Environment.NewLine;
            }
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand(query, con);
                con.Open();
                try
                {
                    com.ExecuteNonQuery();
                    return "1";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }
        public IEnumerable<item> get_sai(int year,int quarter,int mooe_no , int dbm_bb, int accountid)
        {
            List<item> item = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                var qry = "";

                if (USER.C_officeID == 13)
                {
                    qry = @"select tbl_1.*,tbl_6.itemname,tbl_3.*,tbl_4.*,tbl_5.*,isnull(tbl_3.total_in,0) - isnull(tbl_4.total_out,0) as available,isnull(tbl_1.total_quantity,0) - isnull(tbl_5.office_out,0) as remaining_office_quantity,tbl_2.year,tbl_2.quarter,tbl_2.mooe_no,tbl_2.dbm_bb  from ( 
 select officeid,itemid,sum(quantity) as total_quantity,unit,transaction_code,accountid  
 from [IMS].[dbo].[tbl_items_per_transaction_code]  
 where officeid = '" + USER.C_officeID + "' group by officeid,itemid,unit,transaction_code,accountid   ) tbl_1 left join  (select distinct(transcode),year,quarter,mooe_no,dbm_bb from [IMS].[dbo].[tbl_wis_transaction] where year is not null and quarter is not null and mooe_no is not null and dbm_bb is not null ) tbl_2 on tbl_1.transaction_code = tbl_2.transcode left join ( select itemid,SUM(CASE WHEN in_out = 'IN' THEN quantity ELSE 0 END) as total_in,transcode from [IMS].[dbo].[tbl_wis_transaction] group by itemid,transcode ) tbl_3 on tbl_1.transaction_code = tbl_3.transcode and tbl_1.itemid =tbl_3.itemid left join ( select itemid,sum(quantity) as total_out,trans_code from [IMS].[dbo].[wis_ris_transactions] group by itemid,trans_code ) tbl_4 on tbl_1.transaction_code = tbl_4.trans_code and tbl_1.itemid = tbl_4.itemid  left join ( select  itemid,SUM(quantity) as office_out,trans_code from [IMS].[dbo].[wis_ris_transactions]  where officeid =  '" + USER.C_officeID + "' group by itemid,trans_code ) tbl_5 on tbl_1.itemid = tbl_5.itemid and tbl_1.transaction_code = tbl_5.trans_code left join (SELECT  t.itemid,t.itemname FROM(SELECT  DISTINCT itemid FROM    [IMS].[dbo].[tbl_items_per_transaction_code]) mo CROSS APPLY ( SELECT  TOP 1 * FROM    [IMS].[dbo].[tbl_items_per_transaction_code] mi WHERE   mi.itemid = mo.itemid ) t) tbl_6 on tbl_1.itemid = tbl_6.itemid where total_quantity != isnull(office_out,0) and  year = '" + year + "' and quarter = '" + quarter + "'  and mooe_no = '" + mooe_no + "' and dbm_bb =  '" + dbm_bb + "' and accountid = '" + accountid + "'";
                }
                else
                {
                    qry = @"select tbl_1.*,tbl_6.itemname,tbl_3.*,tbl_4.*,tbl_5.*,isnull(tbl_3.total_in,0) - isnull(tbl_4.total_out,0) as available,isnull(tbl_1.total_quantity,0) - isnull(tbl_5.office_out,0) as remaining_office_quantity,tbl_2.year,tbl_2.quarter,tbl_2.mooe_no,tbl_2.dbm_bb  from ( select officeid,itemid,sum(quantity) as total_quantity,unit,transaction_code from [IMS].[dbo].[tbl_items_per_transaction_code]  where officeid = '" + USER.C_officeID + "' group by officeid,itemid,unit,transaction_code  ) tbl_1 left join  (select distinct(transcode),year,quarter,mooe_no,dbm_bb from [IMS].[dbo].[tbl_wis_transaction] where year is not null and quarter is not null and mooe_no is not null and dbm_bb is not null ) tbl_2 on tbl_1.transaction_code = tbl_2.transcode left join ( select itemid,SUM(CASE WHEN in_out = 'IN' THEN quantity ELSE 0 END) as total_in,transcode from [IMS].[dbo].[tbl_wis_transaction] group by itemid,transcode ) tbl_3 on tbl_1.transaction_code = tbl_3.transcode and tbl_1.itemid =tbl_3.itemid left join ( select itemid,sum(quantity) as total_out,trans_code from [IMS].[dbo].[wis_ris_transactions] group by itemid,trans_code ) tbl_4 on tbl_1.transaction_code = tbl_4.trans_code and tbl_1.itemid = tbl_4.itemid  left join ( select  itemid,SUM(quantity) as office_out,trans_code from [IMS].[dbo].[wis_ris_transactions]  where officeid =  '" + USER.C_officeID + "' group by itemid,trans_code ) tbl_5 on tbl_1.itemid = tbl_5.itemid and tbl_1.transaction_code = tbl_5.trans_code left join (SELECT  t.itemid,t.itemname FROM(SELECT  DISTINCT itemid FROM    [IMS].[dbo].[tbl_items_per_transaction_code]) mo CROSS APPLY ( SELECT  TOP 1 * FROM    [IMS].[dbo].[tbl_items_per_transaction_code] mi WHERE   mi.itemid = mo.itemid ) t) tbl_6 on tbl_1.itemid = tbl_6.itemid where total_quantity != isnull(office_out,0) and  year = '"+year+"' and quarter = '"+quarter+"'  and mooe_no = '"+mooe_no+"' and dbm_bb =  '"+dbm_bb+"'";
                }




                SqlCommand com = new SqlCommand(qry, con);
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
                    c.request_quantity = 0;
                    c.transcode = reader["transaction_code"] == DBNull.Value ? (string)null : (string)reader["transaction_code"];
                    item.Add(c);
                }
            }
            return item;
        }
        public IEnumerable<item> items_received_per_office(string transno, int officeid)
        {
            List<item> item = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand(@" select * from (
 select itemid,itemname,unit,sum(total_in) as total,sum(available) as available 
 FROM [IMS].[dbo].[vw_where_to_get_existing_item]
	where  transcode = '"+transno+"' group by itemid,itemname,unit ) as a  left join (select itemid,unit,sum(quantity) as allocated from [IMS].[dbo].[tbl_items_per_transaction_code] where transaction_code = '"+transno+"' and officeid = '"+officeid+"' group by itemid,unit ) as b on a.itemid = b.itemid left join (select itemid,unit,officeid,sum(quantity) as received from [IMS].[dbo].[tbl_wis_transaction]where transcode = '"+transno+"' and in_out = 'OUT' and officeid = '"+officeid+"' group by itemid,unit,officeid) as c on a.itemid = c.itemid", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item c = new item();
                    c.itemid = (Convert.IsDBNull(reader["itemid"]) ? 0 : (int)(reader["itemid"]));
                    c.itemname = reader["itemname"] == DBNull.Value ? (string)null : (string)reader["itemname"];
                    c.unit = reader["unit"] == DBNull.Value ? (string)null : (string)reader["unit"];
                    c.available = (Convert.IsDBNull(reader["available"]) ? 0 : (int)(reader["available"]));
                    c.total_quantity = (Convert.IsDBNull(reader["total"]) ? 0 : (int)(reader["total"]));
                    c.allocated = (Convert.IsDBNull(reader["allocated"]) ? 0 : (int)(reader["allocated"]));
                    c.recieve_quantity = (Convert.IsDBNull(reader["received"]) ? 0 : (int)(reader["received"])); 
                    item.Add(c);
                }
            }
            return item;
        }
         
        public string submit_request(IEnumerable<item> items)
        {
            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));

            String dy = datevalue.Day.ToString();
            String mn = datevalue.Month.ToString();
            String yy = datevalue.Year.ToString();

            string query1 = "exec Proc_GetNewTranNo_ris '[IMS].[dbo].[wis_ris_transactions]','RIS','trans_code','" + yy + "','" + mn + "'";
            string strcon = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();
            string control_number = OleDbHelper.ExecuteScalar(strcon, System.Data.CommandType.Text, query1).ToString();


            string query2 = "insert into [IMS].[dbo].[tbl_t_ris_preparation] (trans_code,date_time,officeid,officename) values ('" + control_number + "','" + DateTime.Now + "','"+USER.C_officeID+"','"+USER.C_Office+"')";
            string strcon2 = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();
            string result = OleDbHelper.ExecuteNonQuery(strcon, System.Data.CommandType.Text, query2).ToString();
          

            var eid = USER.C_eID;
            string query = "";
            foreach (var i in items)
            {
                query += "insert into [IMS].[dbo].[wis_ris_transactions] (officeid,officename,itemid,itemname,unit,quantity,date_submited,trans_code) values ('" + USER.C_officeID + "','" + USER.C_Office + "','" + i.itemid + "','" + i.itemname + "','" + i.unit + "','" + i.request_quantity + "','"+DateTime.Now+"','" + control_number + "')" + System.Environment.NewLine;
            }
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand(query, con);
                con.Open();
                try
                {
                    com.ExecuteNonQuery();
                    return "1";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }

        public IEnumerable<item> get_item_by_ris(string controlno)
        {
            List<item> item = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {

                var query = @" select  *  from (select * FROM [IMS].[dbo].[wis_ris_transactions] where controlno = 'RIS-18-4-000001') as a 
 left join (select officeid,itemid,sum(quantity) as total_quantity,unit,transaction_code from [IMS].[dbo].[tbl_items_per_transaction_code] where officeid = 72 group by officeid,itemid,unit,transaction_code) as b on a.itemid = b.itemid and a.trans_code = b.transaction_code
";

                SqlCommand com = new SqlCommand(@"select  a.* ,b.total_quantity - isnull(c.quantity,0) as remaining_office_quantity from (select *,LTRIM(RTRIM(itemname)) as itemnames FROM [IMS].[dbo].[wis_ris_transactions] where controlno = '" + controlno + "') as a  left join (select officeid,itemid,sum(quantity) as total_quantity,unit,transaction_code from [IMS].[dbo].[tbl_items_per_transaction_code] where officeid = (select distinct(officeid) from [IMS].[dbo].[wis_ris_transactions]where controlno = '" + controlno + "') group by officeid,itemid,unit,transaction_code) as b on a.itemid = b.itemid and a.trans_code = b.transaction_code left join (select itemid,sum(quantity) as quantity from [IMS].[dbo].[tbl_wis_transaction] where in_out = 'OUT' and transcode in (select distinct(trans_code) FROM [IMS].[dbo].[wis_ris_transactions] where controlno = '" + controlno + "') group by itemid) as c on a.itemid = c.itemid order by itemnames", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item c = new item();
                    c.id = (Convert.IsDBNull(reader["id"]) ? 0 : (int)(reader["id"]));
                    c.officeid = (Convert.IsDBNull(reader["officeid"]) ? 0 : (int)(reader["officeid"]));
                    c.itemid = (Convert.IsDBNull(reader["itemid"]) ? 0 : (int)(reader["itemid"]));
                    c.itemname = reader["itemnames"] == DBNull.Value ? (string)null : (string)reader["itemnames"];
                    c.unit = reader["unit"] == DBNull.Value ? (string)null : (string)reader["unit"];
                    c.quantity = (Convert.IsDBNull(reader["quantity"]) ? 0 : (double)(reader["quantity"]));
                    c.rprice = (Convert.IsDBNull(reader["price"]) ? 0 : (decimal)(reader["price"]));
                    c.transcode = reader["trans_code"] == DBNull.Value ? (string)null : (string)reader["trans_code"];
                    c.status = (Convert.IsDBNull(reader["status"]) ? 0 : (int)(reader["status"]));
                    c.total = (Convert.IsDBNull(reader["remaining_office_quantity"]) ? 0 : (double)(reader["remaining_office_quantity"]));
                    c.controlno = reader["controlno"] == DBNull.Value ? (string)null : (string)reader["controlno"];
                    c.to_be_deleted = reader["to_be_deleted"] as bool? ?? false;

                    c.comment = reader["comment"] == DBNull.Value ? (string)null : (string)reader["comment"];

                    item.Add(c);
                    // + (Convert.IsDBNull(reader["quantity"]) ? 0 : (int)(reader["quantity"]));
                }
            }
            return item;
        }
        public IEnumerable<item> get_listview_ris()
        {
            List<item> item = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand(@"select * from [IMS].[dbo].[tbl_t_ris_preparation] where officeid = '"+USER.C_officeID+"' and status != 3 order by date_time desc ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item c = new item();
                    c.controlno = reader["controlno"] == DBNull.Value ? (string)null : (string)reader["controlno"];
                    c.date_submitted = Convert.ToDateTime(reader["date_time"]); 
                    c.status = (Convert.IsDBNull(reader["status"]) ? 0 : (int)(reader["status"]));
                    c.agree = reader["agree"] as bool? ?? false;
                    c.edited = reader["edited"] as bool? ?? false;
                    c.deleted = reader["deleted"] as bool? ?? false;
                    c.receive = reader["receive"] as bool? ?? false;
                    item.Add(c);
                }
            }
            return item;
        }
        public IEnumerable<item> report_ris(string controlno)
        {
            List<item> item = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
               // SqlCommand com = new SqlCommand(@"select a.*,b.id FROM [IMS].[dbo].[wis_ris_transactions] as a  inner join [IMS].[dbo].[tbl_wis_transaction] as b on a.trans_code = b.transcode and a.itemid = b.itemid and a.price = b.unitcost where a.controlno = '"+controlno+"'and b.in_out = 'IN' order by itemname asc", con);
                SqlCommand com = new SqlCommand(@"select *,LTRIM(RTRIM(itemname)) as itemnames FROM [IMS].[dbo].[wis_ris_transactions]  where controlno = '" + controlno.Trim() + "' and (status = 1 or status IS NULL or status = 0) order by itemnames", con); 
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item c = new item();
                    c.id = (Convert.IsDBNull(reader["id"]) ? 0 : (int)(reader["id"]));
                    c.itemid = (Convert.IsDBNull(reader["itemid"]) ? 0 : (int)(reader["itemid"]));
                    c.itemname = reader["itemnames"] == DBNull.Value ? (string)null : (string)reader["itemnames"];
                    c.unit = reader["unit"] == DBNull.Value ? (string)null : (string)reader["unit"];
                    c.quantity = (Convert.IsDBNull(reader["quantity"]) ? 0 : (double)(reader["quantity"]));
                    c.rprice = (Convert.IsDBNull(reader["price"]) ? 0 : (decimal)(reader["price"]));
                    c.transcode = reader["trans_code"] == DBNull.Value ? (string)null : (string)reader["trans_code"];
                    c.status = (Convert.IsDBNull(reader["status"]) ? 0 : (int)(reader["status"]));
                    c.totalamount = Convert.ToDecimal(c.quantity) * c.rprice;
                    item.Add(c);
                }
            }
            return item;
        }

        public IEnumerable<item> get_office_available(int officeid)
        {
            List<item> item = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand(@"select * from [IMS].[dbo].[vwbyofficeout] where officeid='" + officeid + "' and total > 0", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item c = new item();
                    c.itemid = (Convert.IsDBNull(reader["item_id"]) ? 0 : (int)(reader["item_id"]));
                    c.itemname = reader["itemname"] == DBNull.Value ? (string)null : (string)reader["itemname"];
                    c.quantity = (Convert.IsDBNull(reader["total"]) ? 0 : (int)(reader["total"]));
                    c.unit = reader["unit"] == DBNull.Value ? (string)null : (string)reader["unit"];
                    item.Add(c);
                }
            }
            return item;
        }

        public IEnumerable<item> report_ending_inventory(string date)
        {
            List<item> item = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand(@"exec Ending_inventory '" + date + "'", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item c = new item();
                    c.itemid = (Convert.IsDBNull(reader["itemid"]) ? 0 : (int)(reader["itemid"]));
                    c.itemname = reader["itemname"] == DBNull.Value ? (string)null : (string)reader["itemname"];
                    c.quantity = (Convert.IsDBNull(reader["total_remaining"]) ? 0 : (double)(reader["total_remaining"]));
                    c.unit = reader["unit"] == DBNull.Value ? (string)null : (string)reader["unit"];
                    c.rprice = (Convert.IsDBNull(reader["unitcost"]) ? 0 : (decimal)(reader["unitcost"]));
                    c.totalamount = Convert.ToDecimal(c.quantity) * c.rprice;
                     
                    item.Add(c);
                }
            }
            return item;
        }

        public IEnumerable<item> get_gen_transcode(int year,int quarter,int mooe_no , int dbm_bb)
        {
            List<item> item = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand(@"select itemid,itemname,sum(quantity) as quantity,unit,unitcost,transcode FROM [IMS].[dbo].[tbl_wis_transaction] 
	   where year = '" + year + "' and quarter = '" + quarter + "' and mooe_no = '" + mooe_no + "' and dbm_bb = '" + dbm_bb + "' and  transcode like '%MAN%' group by itemid,itemname,unit,unitcost,transcode ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item c = new item();
                   // c.id = (Convert.IsDBNull(reader["id"]) ? 0 : (int)(reader["id"]));
                    c.itemid = (Convert.IsDBNull(reader["itemid"]) ? 0 : (int)(reader["itemid"]));
                    c.itemname = reader["itemname"] == DBNull.Value ? (string)null : (string)reader["itemname"];
                    c.quantity = (Convert.IsDBNull(reader["quantity"]) ? 0 : (int)(reader["quantity"]));
                    c.unit = reader["unit"] == DBNull.Value ? (string)null : (string)reader["unit"];
                    c.rprice = (Convert.IsDBNull(reader["unitcost"]) ? 0 : (decimal)(reader["unitcost"]));
                    c.transcode = reader["transcode"] == DBNull.Value ? (string)null : (string)reader["transcode"] ;
                    //c.item_description = c.transcode + "(" + c.itemname + ")";
                    item.Add(c);
                }
            }
            return item;
        }

        public IEnumerable<item> get_office_items_gen_transcode(string transcode)
        {
            List<item> item = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand(@"select * from [IMS].[dbo].[tbl_items_per_transaction_code] where transaction_code = '" + transcode + "'", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item c = new item();
                    c.id = (Convert.IsDBNull(reader["id"]) ? 0 : (int)(reader["id"]));
                    c.itemid = (Convert.IsDBNull(reader["itemid"]) ? 0 : (int)(reader["itemid"]));
                    c.itemname = reader["itemname"] == DBNull.Value ? (string)null : (string)reader["itemname"];
                    c.officename = reader["officename"] == DBNull.Value ? (string)null : (string)reader["officename"];
                    c.quantity = (Convert.IsDBNull(reader["quantity"]) ? 0 : (int)(reader["quantity"]));
                    c.unit = reader["unit"] == DBNull.Value ? (string)null : (string)reader["unit"];
                    c.rprice = (Convert.IsDBNull(reader["price"]) ? 0 : (decimal)(reader["price"]));
                    c.transcode = reader["transaction_code"] == DBNull.Value ? (string)null : (string)reader["transaction_code"];
                    item.Add(c);
                }
            }
            return item;
        }

        public IEnumerable<item> get_office_received(string transcode , int itemid )
        {
            List<item> item = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand(@"select  id,itemid,itemname,officeid , officename , sum(quantity) as quantity , unit  FROM [IMS].[dbo].[wis_ris_transactions] 
	  where itemid = '" + itemid + "' and trans_code = '" + transcode + "'  and controlno = '' group by id,itemid,itemname ,officeid , officename , unit", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item c = new item();
                    c.id = (Convert.IsDBNull(reader["id"]) ? 0 : (int)(reader["id"]));
                    c.itemid = (Convert.IsDBNull(reader["itemid"]) ? 0 : (int)(reader["itemid"]));
                    c.itemname = reader["itemname"] == DBNull.Value ? (string)null : (string)reader["itemname"];
                    c.officeid = (Convert.IsDBNull(reader["officeid"]) ? 0 : (int)(reader["officeid"]));
                    c.officename = reader["officename"] == DBNull.Value ? (string)null : (string)reader["officename"];
                    c.quantity = (Convert.IsDBNull(reader["quantity"]) ? 0 : (int)(reader["quantity"]));
                    c.unit = reader["unit"] == DBNull.Value ? (string)null : (string)reader["unit"];  
                    item.Add(c);
                }
            }
            return item;
        }

        public IEnumerable<item> get_agree_edited(string controlno)
        {
            List<item> item = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand(@"select edited,agree,deleted from [IMS].[dbo].[tbl_t_ris_preparation]  where controlno = '" + controlno + "'", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item c = new item(); 
                    c.agree = reader["agree"] as bool? ?? false;
                    c.edited = reader["edited"] as bool? ?? false;
                    c.deleted = reader["deleted"] as bool? ?? false;
                    item.Add(c);
                }
            }
            return item;
        }


        public IEnumerable<item> emergency_sai(int year, int quarter, int mooe_no, int dbm_bb, int accountid)
        {
            List<item> item = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                var qry = @"select wis_trans.*,isnull(ris_trans.total_out,0) as total_out,items.itemname  from (select  itemid,sum(quantity)as total_quantity,unit,transcode,year,quarter,mooe_no,dbm_bb,accountid from [IMS].[dbo].[tbl_wis_transaction] where in_out = 'IN' and year = '"+year+"' and quarter = '"+quarter+"' and mooe_no = '"+mooe_no+"' and dbm_bb = '"+dbm_bb+"' and accountid = '"+accountid+"'  group by  itemid,unit,transcode,accountid,year,quarter,mooe_no,dbm_bb ) wis_trans left join (select itemid,sum(quantity) as total_out,trans_code from [IMS].[dbo].[wis_ris_transactions] group by itemid,trans_code) ris_trans on wis_trans.transcode = ris_trans.trans_code and wis_trans.itemid = ris_trans.itemid left join (SELECT  t.itemid,t.itemname FROM(SELECT  DISTINCT itemid FROM [IMS].[dbo].[tbl_wis_transaction]  ) mo CROSS APPLY ( SELECT  TOP 1 * FROM    [IMS].[dbo].[tbl_wis_transaction] mi WHERE   mi.itemid = mo.itemid ) t) items on wis_trans.itemid = items.itemid";
                 
                SqlCommand com = new SqlCommand(qry, con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item c = new item();
                    c.itemid = (Convert.IsDBNull(reader["itemid"]) ? 0 : (int)(reader["itemid"]));
                    c.itemname = reader["itemname"] == DBNull.Value ? (string)null : (string)reader["itemname"];
                    c.quantity = (Convert.IsDBNull(reader["total_quantity"]) ? 0 : (double)(reader["total_quantity"]));
                    c.unit = reader["unit"] == DBNull.Value ? (string)null : (string)reader["unit"];
                    c.totalout = (Convert.IsDBNull(reader["total_out"]) ? 0 : (double)(reader["total_out"]));
                    c.available = c.quantity - c.totalout;
                    c.transcode = reader["transcode"] == DBNull.Value ? (string)null : (string)reader["transcode"];
                    item.Add(c);
                }
            }
            return item;
        }

        public IEnumerable<item> get_office_vale(string date )
        {
            if (date == null)
            {
                date = DateTime.Now.ToString();
            }
            DateTime yearmonth = Convert.ToDateTime(date);
            int month = yearmonth.Month;
            int year = yearmonth.Year;

            List<item> item = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                var qry = @"select distinct(officeid),officename FROM [IMS].[dbo].[tbl_t_ris_preparation] where transaction_type like '%BORROW%' and MONTH(date_time) = " +month + " and YEAR(date_time) = " + year + "";
                 
                SqlCommand com = new SqlCommand(qry, con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item c = new item();
                    c.officeid = (Convert.IsDBNull(reader["officeid"]) ? 0 : (int)(reader["officeid"]));
                    c.officename = reader["officename"] == DBNull.Value ? (string)null : (string)reader["officename"]; 
                    item.Add(c);
                }
            }
            return item;
        }

        public IEnumerable<item> get_office_items(int officeid,string from ,string to)
        {
            DateTime f = Convert.ToDateTime(from);
            DateTime t = Convert.ToDateTime(to);
            var qry = "";
           
                // qry = @"select tbl1.*,c.paid  from ( select b.itemid,b.itemname,b.unit,sum(b.quantity) as quantity,b.price   from (select controlno,cast(  (cast( date_approve as date)) as nvarchar ) as date_approve,officeid  from [IMS].[dbo].[tbl_t_ris_preparation]  where transaction_type like '%BORROW%' and officeid = "+officeid+" ) as a inner join  [IMS].[dbo].[wis_ris_transactions] as b on a.controlno = b.controlno group by b.itemid,b.itemname,b.unit,b.price) as tbl1 left join (select itemid,officeid,sum(quantity) as paid  from [IMS].[dbo].[tbl_t_paid] where officeid = "+officeid+" group by itemid,officeid )  as c on tbl1.itemid = c.itemid order by itemname asc";


            qry = @"select a.*,b.paid,t.itemname from ( select itemid,price,unit,sum(quantity) as total  from [IMS].[dbo].[wis_ris_transactions] where controlno IN ( select controlno from [IMS].[dbo].[tbl_t_ris_preparation]  where transaction_type like '%BORROW%' and officeid = " + officeid + " and status = 1 ) group by itemid,price,unit ) as a left join (select itemid,unit_cost,unit,sum(quantity) as paid  from [IMS].[dbo].[tbl_t_paid] where officeid = " + officeid + " group by itemid,unit_cost,unit ) as b on a.itemid = b.itemid and a.price = b.unit_cost and a.unit = b.unit CROSS APPLY (SELECT TOP 1 * FROM ( select distinct(itemid),itemname,unit from  [IMS].[dbo].[tbl_items_per_transaction_code] union select distinct(id) as itemid,itemname,unit from  [IMS].[dbo].[tbl_manual_items] union select distinct(itemid)  ,itemname,unit from  [IMS].[dbo].[tbl_wis_transaction] ) mi  WHERE   mi.itemid = a.itemid ) t  order by itemname asc";
         
            //else
            //{
            //    qry = @"select * from (select controlno,cast(  (cast( date_approve as date)) as nvarchar ) as date_approve,officeid  from [IMS].[dbo].[tbl_t_ris_preparation]  where transaction_type like '%BORROW%' and officeid = " + officeid + " and cast( date_approve as date) between '" + f.ToString("yyyy-MM-dd") + "' and '" + t.ToString("yyyy-MM-dd") + "' ) as a inner join  [IMS].[dbo].[wis_ris_transactions] as b on a.controlno = b.controlno";
            //}

            List<item> item = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
              
                SqlCommand com = new SqlCommand(qry, con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    
                    item c = new item();
                    c.officeid = officeid;
                    c.itemid = (Convert.IsDBNull(reader["itemid"]) ? 0 : (int)(reader["itemid"]));
                  //  c.officename = reader["officename"] == DBNull.Value ? (string)null : (string)reader["officename"];
                    c.itemname = reader["itemname"] == DBNull.Value ? (string)null : (string)reader["itemname"];
                    c.rprice = (Convert.IsDBNull(reader["price"]) ? 0 : (decimal)(reader["price"]));
                    c.unit = reader["unit"] == DBNull.Value ? (string)null : (string)reader["unit"];
                    c.quantity = (Convert.IsDBNull(reader["total"]) ? 0.0 : (double)(reader["total"]));
                    //c.date_approve = reader["date_approve"] == DBNull.Value ? (string)null : (string)reader["date_approve"];
                    c.totalout = (Convert.IsDBNull(reader["paid"]) ? 0.0 : (double)(reader["paid"]));
                    c.remaining = c.quantity - c.totalout;
                    c.totalamount = Convert.ToDecimal(c.remaining) * c.rprice;


                    if (c.remaining == 0)
                    {

                    }
                    else
                    {
                        item.Add(c);
                    }

                 
                }
            }
            return item;
        }

        public IEnumerable<item> readItemsBorrowed(int officeid, int itemid)
        { 
            List<item> item = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                var qry = @"";
                SqlCommand com = new SqlCommand(qry, con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {

                    item c = new item();
                    c.officeid = (Convert.IsDBNull(reader["officeid"]) ? 0 : (int)(reader["officeid"]));
                   
                    item.Add(c);
                }
            }
            return item;
        }

        public IEnumerable<item> readAllBorrowedControl(int officeid)
        { 
            List<item> item = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                var qry = @"select * from [IMS].[dbo].[tbl_t_ris_preparation] where officeid = " + officeid + " and transaction_type = 'BORROW' ";
                SqlCommand com = new SqlCommand(qry, con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item c = new item();
                    c.officeid = (Convert.IsDBNull(reader["officeid"]) ? 0 : (int)(reader["officeid"]));
                    c.controlno = reader["controlno"] == DBNull.Value ? (string)null : (string)reader["controlno"];
                    c.officename = reader["officename"] == DBNull.Value ? (string)null : (string)reader["officename"];
                    item.Add(c);
                }
            }
            return item;
        }

        public IEnumerable<item> getItemsPerControlNo(int officeid,string controlno)
        { 
            List<item> item = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                var qry = @"select * from [IMS].[dbo].[wis_ris_transactions] where officeid = "+officeid+" and controlno = '"+controlno+"'";
                SqlCommand com = new SqlCommand(qry, con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item c = new item();
                    c.id = (Convert.IsDBNull(reader["id"]) ? 0 : (int)(reader["id"]));
                    c.officeid = (Convert.IsDBNull(reader["officeid"]) ? 0 : (int)(reader["officeid"]));
                    c.itemid = (Convert.IsDBNull(reader["itemid"]) ? 0 : (int)(reader["itemid"]));
                    c.itemname = reader["itemname"] == DBNull.Value ? (string)null : (string)reader["itemname"];
                    c.unit = reader["unit"] == DBNull.Value ? (string)null : (string)reader["unit"];
                    c.quantity = (Convert.IsDBNull(reader["quantity"]) ? 0 : (int)(reader["quantity"]));
                    c.transcode = reader["trans_code"] == DBNull.Value ? (string)null : (string)reader["trans_code"];
                    c.rprice = (Convert.IsDBNull(reader["price"]) ? 0 : (decimal)(reader["price"]));
                    c.controlno = reader["controlno"] == DBNull.Value ? (string)null : (string)reader["controlno"];
                    item.Add(c);
                }
            }
            return item;
        }

        public IEnumerable<item> getItems(int officeid)
        { 
            List<item> item = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {

                //select tbl1.*,c.paid  from ( select b.itemid,b.itemname,b.unit,sum(b.quantity) as quantity,b.price  from (select controlno,cast(  (cast( date_approve as date)) as nvarchar ) as date_approve,officeid  from [IMS].[dbo].[tbl_t_ris_preparation]  where transaction_type like '%BORROW%' and officeid = "+officeid+" ) as a inner join  [IMS].[dbo].[wis_ris_transactions] as b on a.controlno = b.controlno group by b.itemid,b.itemname,b.unit,b.price ) as tbl1 left join ( select itemid,officeid,sum(quantity) as paid  from [IMS].[dbo].[tbl_t_paid] where officeid = "+officeid+" group by itemid,officeid )  as c on tbl1.itemid = c.itemid

                var qry = @"select a.*,b.paid,t.itemname from ( select itemid,price,unit,sum(quantity) as total  from [IMS].[dbo].[wis_ris_transactions] where controlno IN ( select controlno from [IMS].[dbo].[tbl_t_ris_preparation]  where transaction_type like '%BORROW%' and officeid = " + officeid + " and status = 1 ) group by itemid,price,unit ) as a left join (select itemid,unit_cost,unit,sum(quantity) as paid  from [IMS].[dbo].[tbl_t_paid] where officeid = " + officeid + " group by itemid,unit_cost,unit ) as b on a.itemid = b.itemid and a.price = b.unit_cost and a.unit = b.unit CROSS APPLY (SELECT TOP 1 * FROM ( select distinct(itemid),itemname,unit from  [IMS].[dbo].[tbl_items_per_transaction_code] union select distinct(id) as itemid,itemname,unit from  [IMS].[dbo].[tbl_manual_items] union select distinct(itemid)  ,itemname,unit from  [IMS].[dbo].[tbl_wis_transaction] ) mi  WHERE   mi.itemid = a.itemid ) t  order by itemname asc";
                SqlCommand com = new SqlCommand(qry, con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item c = new item();  
                    c.itemid = (Convert.IsDBNull(reader["itemid"]) ? 0 : (int)(reader["itemid"]));
                    var name = reader["itemname"] == DBNull.Value ? (string)null : (string)reader["itemname"];
                    c.itemname = name.Replace("\"", "\\\"");
                    c.unit = reader["unit"] == DBNull.Value ? (string)null : (string)reader["unit"];
                    c.quantity = (Convert.IsDBNull(reader["total"]) ? 0 : (int)(reader["total"])); 
                    c.rprice = (Convert.IsDBNull(reader["price"]) ? 0 : (decimal)(reader["price"]));
                    c.recieve_quantity = (Convert.IsDBNull(reader["paid"]) ? 0 : (int)(reader["paid"])); 
                    item.Add(c);
                }
            }
            return item;
        }

        public IEnumerable<item> get_warehouse()
        { 
            List<item> item = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                var qry = @"select tbl1.itemid,tbl1.items_in,tbl1.items_out,tbl1.remaining , t.itemname,t.unit from(select a.*,b.items_out,a.items_in - CASE when b.items_out is null then 0 else b.items_out end as remaining  from (SELECT itemid , SUM(CASE WHEN in_out = 'IN' THEN quantity ELSE 0 END)  as items_in FROM [IMS].[dbo].[tbl_wis_transaction] GROUP BY itemid) as a left join (SELECT itemid,   SUM(quantity)  as items_out FROM [IMS].[dbo].[wis_ris_transactions] where to_be_deleted  = 0 GROUP BY itemid   ) as b on a.itemid = b.itemid ) tbl1 CROSS APPLY ( SELECT  TOP 1 * FROM (select distinct(itemid),itemname,unit from  [IMS].[dbo].[tbl_items_per_transaction_code] union select id as itemid,itemname,unit from  [IMS].[dbo].[tbl_manual_items]) mi WHERE   mi.itemid = tbl1.itemid ) t where  remaining > 0 order by t.itemname asc ";
                SqlCommand com = new SqlCommand(qry, con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item c = new item();  
                    c.itemid = (Convert.IsDBNull(reader["itemid"]) ? 0 : (int)(reader["itemid"]));
                    c.itemname = reader["itemname"] == DBNull.Value ? (string)null : (string)reader["itemname"];
                    c.unit = reader["unit"] == DBNull.Value ? (string)null : (string)reader["unit"];
                    c.quantity = (Convert.IsDBNull(reader["remaining"]) ? 0 : (int)(reader["remaining"]));   
                    item.Add(c);
                }
            }
            return item;
        }

        public IEnumerable<item> ris_warehouse()
        {
            var date = DateTime.Now.ToString("yyyy-MM-dd");
            List<item> item = new List<item>();

            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand(@"exec ris_warehouse  ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item c = new item();
                    c.itemid = (Convert.IsDBNull(reader["itemid"]) ? 0 : (int)(reader["itemid"]));
                    c.itemname = reader["itemname"] == DBNull.Value ? (string)null : (string)reader["itemname"];
                    c.quantity = Math.Truncate((Convert.IsDBNull(reader["total_remaining"]) ? 0 : (double)(reader["total_remaining"])) * 100) / 100;
                    c.unit = reader["unit"] == DBNull.Value ? (string)null : (string)reader["unit"];
                    c.rprice = (Convert.IsDBNull(reader["unitcost"]) ? 0 : (decimal)(reader["unitcost"]));
                    c.totalamount = Convert.ToDecimal(c.quantity)  * c.rprice;

                    item.Add(c);
                }
            }
            return item;
        }

        public IEnumerable<item> ris_warehouse_tires()
        {
            var date = DateTime.Now.ToString("yyyy-MM-dd");
            List<item> item = new List<item>();

            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand(@"exec ris_warehouse  ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item c = new item();
                    c.itemid = (Convert.IsDBNull(reader["itemid"]) ? 0 : (int)(reader["itemid"]));
                    c.itemname = reader["itemname"] == DBNull.Value ? (string)null : (string)reader["itemname"];
                    c.quantity = Math.Truncate((Convert.IsDBNull(reader["total_remaining"]) ? 0 : (double)(reader["total_remaining"])) * 100) / 100;
                    c.unit = reader["unit"] == DBNull.Value ? (string)null : (string)reader["unit"];
                    c.rprice = (Convert.IsDBNull(reader["unitcost"]) ? 0 : (decimal)(reader["unitcost"]));
                    c.totalamount = Convert.ToDecimal(c.quantity) * c.rprice;

                    var istire = (Convert.IsDBNull(reader["istire"]) ? 0 : (int)(reader["istire"]));
                    if (istire == 1)
                    {
                        item.Add(c);
                    } 
                }
            }
            return item;
        }

        
        public IEnumerable<item> getRisWithAmountObr(int officeid)
        {
            List<item> item = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                var qry = @" select * from ( select  preparation_id, controlno,  date_time, officeid, officename, OBR   from [IMS].[dbo].[tbl_t_ris_preparation]   where officeid = '"+officeid+"' and status = 1 ) as a left join ( select cast(sum(price*quantity) as money) as amount ,controlno as asd from [IMS].[dbo].[wis_ris_transactions]  group by controlno ) as b on a.controlno = b.asd where    LEN(a.OBR) > 0  order by a.controlno desc";
                SqlCommand com = new SqlCommand(qry, con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item c = new item();
                    c.preparation_id = (Convert.IsDBNull(reader["preparation_id"]) ? 0 : (int)(reader["preparation_id"]));
                    c.controlno = reader["controlno"] == DBNull.Value ? (string)null : (string)reader["controlno"]; 
                    c.officeid = (Convert.IsDBNull(reader["officeid"]) ? 0 : (int)(reader["officeid"]));
                    c.rprice = (Convert.IsDBNull(reader["amount"]) ? 0 : (decimal)(reader["amount"]));
                    c.obr = reader["OBR"] == DBNull.Value ? (string)null : (string)reader["OBR"];
                    
                    item.Add(c);
                }
            }
            return item;
        }
        public IEnumerable<item> getRisWithAmountObr1(int officeid)
        {
            List<item> item = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                var qry = @" select * from  ( select  preparation_id, controlno,  date_time, officeid, officename, OBR   from [IMS].[dbo].[tbl_t_ris_preparation]   where officeid = '"+officeid+"' and status = 1 ) as a left join (select cast(sum(price*quantity) as money) as amount ,controlno as asd from [IMS].[dbo].[wis_ris_transactions]  group by controlno ) as b on a.controlno = b.asd where  LEN(a.OBR) is null or LEN(a.OBR) = 0";
                SqlCommand com = new SqlCommand(qry, con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item c = new item();
                    c.preparation_id = (Convert.IsDBNull(reader["preparation_id"]) ? 0 : (int)(reader["preparation_id"]));
                    c.controlno = reader["controlno"] == DBNull.Value ? (string)null : (string)reader["controlno"]; 
                    c.officeid = (Convert.IsDBNull(reader["officeid"]) ? 0 : (int)(reader["officeid"]));
                    c.rprice = (Convert.IsDBNull(reader["amount"]) ? 0 : (decimal)(reader["amount"]));
                    c.obr = reader["OBR"] == DBNull.Value ? (string)null : (string)reader["OBR"];
                    
                    item.Add(c);
                }
            }
            return item;
        }
         
        public IEnumerable<dynamic> edit_quantity()
        {
            var offid = USER.C_officeID;
            List<item> itms = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand(@"SELECT  a.id,a.date,a.itemid, a.itemname, a.unitcost, a.unit, a.total_in as quantity, a.transcode, b.total_out as recieve_quantity, a.total_in - ISNULL(b.total_out, 0) AS available, a.year, a.quarter, a.mooe_no, a.dbm_bb FROM  ( SELECT   id,date,itemid, itemname, SUM(CASE WHEN in_out = 'IN' THEN quantity ELSE 0 END) AS total_in, unit, unitcost, transcode, year, quarter, mooe_no, dbm_bb FROM            dbo.tbl_wis_transaction WHERE        (in_out = 'IN')  GROUP BY id,date,itemid, itemname, transcode, unitcost, unit, year, quarter, mooe_no, dbm_bb  ) AS a LEFT OUTER JOIN (SELECT itemid, SUM(quantity) AS total_out, price, trans_code FROM            dbo.wis_ris_transactions AS tbl_wis_transaction_1 where    to_be_deleted = 0 GROUP BY itemid, trans_code, price) AS b ON a.itemid = b.itemid AND a.transcode = b.trans_code", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item off = new item();
                    off.id = (Convert.IsDBNull(reader["id"]) ? 0 : (int)(reader["id"]));
                    off.itemid = (Convert.IsDBNull(reader["itemid"]) ? 0 : (int)(reader["itemid"]));
                    off.itemname = reader["itemname"] == DBNull.Value ? (string)null : (string)reader["itemname"];
                    off.quantity = (Convert.IsDBNull(reader["quantity"]) ? 0 : (double)(reader["quantity"]));
                    off.recieve_quantity = (Convert.IsDBNull(reader["recieve_quantity"]) ? 0 : (double)(reader["recieve_quantity"]));
                    off.unit = reader["unit"] == DBNull.Value ? (string)null : (string)reader["unit"];
                    off.date = (DateTime)reader["date"];
                    itms.Add(off);
                }
                return itms;
            }
        }
    }
}