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
using System.Data;
using IMS.@class;
using System.Data.SqlClient;
using System.Configuration;
using IMS.Classess;

namespace IMS.Models
{
    public class wis
    {
        private pmisEntities pmis = new pmisEntities();
        private IMSEntities db = new IMSEntities();
        string strcon = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();
        public class employee
        {
            public int req_eid { get; set; }
            public DateTime birthday { get; set; }
            public Int64 Swipe_id { get; set; }
            public string reid { get; set; }
            public string Status { get; set; }
            public string BirthD { get; set; }
            public string Position { get; set; }
            public string EmailAdd { get; set; }
            public long eid { get; set; }

            public string EmpName { get; set; }
            public string SwipeId { get; set; }

            public int Id { get; set; }
            public long EmployeeId { get; set; }

            public int OfficeId { get; set; }

            public string EmployeeFname { get; set; }
            public string EmployeeLname { get; set; }

        }

        public IEnumerable<employee> Employee(int req_eid)
        {
            List<employee> empinfo = new List<employee>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand("select * from [pmis].[dbo].[m_vwGetAllEmployee] where isactive=1 and eid ='" + req_eid + "'", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    employee emp = new employee();
                    // emp.Id = reader.GetInt32(0);
                    
                   // c.status = (Convert.IsDBNull(reader["status"]) ? 0 : (int)(reader["status"]));
                    emp.SwipeId = reader["SwipeID"] == DBNull.Value ? (string)null : (string)reader["SwipeID"];
                    emp.eid = (Convert.IsDBNull(reader["eid"]) ? 0 : (Int64)(reader["eid"]));
                    emp.EmpName = reader["EmpFullName"] == DBNull.Value ? (string)null : (string)reader["EmpFullName"];
                   // emp.birthday = (DateTime)reader["BirthD"];
                    //emp.Position = reader["Position"] == DBNull.Value ? (string)null : (string)reader["Position"];
                    //emp.EmailAdd = reader["EmailAdd"] == DBNull.Value ? (string)null : (string)reader["EmailAdd"];
                    //emp.Status = reader["Status"] == DBNull.Value ? (string)null : (string)reader["Status"];

                    empinfo.Add(emp);
                }
            }
            return empinfo;
        }
        public IEnumerable<employee> Employee2(int reid)
        {
            List<employee> empinfo = new List<employee>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand("select * from [pmis].[dbo].[m_vwGetAllEmployee] where isactive=1 and eid ='" + reid + "'", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    employee emp = new employee();
                    // emp.Id = reader.GetInt32(0);
                    emp.SwipeId = reader.GetString(0);
                    emp.eid = reader.GetInt64(1);
                    emp.EmpName = reader.GetString(4);
                    emp.BirthD = reader.GetValue(20).ToString();
                    emp.Position = reader.GetString(5);

                    emp.EmailAdd = reader.GetValue(9).ToString();
                    emp.Status = reader.GetString(10);

                    empinfo.Add(emp);
                }
            }
            return empinfo;
        }
        public IEnumerable<item> Getitems()
        {
            List<item> itms = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand("select * from [IMS].[dbo].[vw_stockviewing_wis] where total > '0'", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item off = new item();
                    off.itemid = reader.GetInt32(1);
                    off.itemname = reader.GetString(2);
                    off.unitcost = reader.GetValue(3).ToString();
                    off.total = reader.GetInt32(4);
                    off.unit = reader.GetString(5);

                    itms.Add(off);
                }
                return itms;
            }
        }
        public void submit_ris(IEnumerable<item> Items)
        {
            var status = 0;
            var date = DateTime.Now;
            var eid = USER.C_eID;
            var officeid = USER.C_officeID;
            var officename = USER.C_Office;
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                con.Open();
                foreach (item item in Items)
                {
                    if (item.quantity > 0 && item.total >= item.quantity)
                    {
                        SqlCommand com = new SqlCommand(@"insert into [IMS].[dbo].[wis_ris_transactions](officeid,officename,itemid,itemname,price,unit,quantity,status,date_submited) values('" + officeid + "','" + officename + "','" + item.itemid + "','" + item.itemname + "','" + item.unitcost + "','" + item.unit + "','" + item.quantity + "','" + status + "','" + date + "')", con);

                        com.ExecuteNonQuery();
                    }
                }
            }
        }
        //public string submit_ris(IEnumerable<item> Items)
        //{
        //    var date = DateTime.Now;
        //    var eid = USER.C_eID;
        //    var IN = "IN";
        //    using (SqlConnection con = new SqlConnection(common.MyConnection()))
        //    {

        //            SqlCommand com = new SqlCommand(@"insert into [IMS].[dbo].[tbl_wis_transaction](eid,itemid,itemname,date,quantity,unitcost,officeid,in_out,descript,srcid,accountid,tid,unit,obj,reid) values('" + eid + "','" + item.itemid + "', '" + item.itemname + "' ,'" + date + "','" + item.quantity + "','" + item.unitcost + "','" + item.officeid + "','" + IN + "','" + item.descript + "','" + item.srcid + "','" + item.accountid + "','" + item.tid + "','" + item.unit + "','" + item.obj + "','" + item.reid + "')", con);
        //            con.Open();
        //            try
        //            {
        //                foreach (item item in Items)
        //                {
        //                    com.ExecuteNonQuery();
        //                    return "1";
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                return ex.Message;
        //            }

        //    }
        //}
        public IEnumerable<item> get_aprroved_items()
        {
            List<item> itms = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand("select * from [IMS].[dbo].[wis_ris_transactions] where status= 1", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item off = new item();
                    off.officeid = reader.GetInt32(1);
                    off.itemid = reader.GetInt32(2);
                    off.itemname = reader.GetString(3);
                    off.unitcost = reader.GetValue(4).ToString();
                    off.unit = reader.GetString(5);
                    off.quantity = reader.GetInt32(6);
                    off.status = reader.GetInt32(7);
                    off.date_approved = reader.GetDateTime(8);
                    off.date_submitted = reader.GetDateTime(9);
                    itms.Add(off);
                }
                return itms;
            }
        }
        public IEnumerable<item> get_submitted_ris()
        {
            List<item> itms = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand("select * from [IMS].[dbo].[wis_ris_transactions] where status= 0", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item off = new item();
                    off.officeid = reader.GetInt32(1);
                    off.officename = reader.GetValue(2).ToString();
                    off.itemid = reader.GetInt32(3);
                    off.itemname = reader.GetString(4);
                    off.unitcost = reader.GetValue(5).ToString();
                    off.unit = reader.GetString(6);
                    off.ris_quantity = reader.GetValue(7).ToString();
                    off.status = reader.GetInt32(8);
                    off.date_approve = reader.GetValue(9).ToString();
                    off.date_submitted = reader.GetDateTime(11);
                    off.risid = reader.GetValue(12).ToString();
                    itms.Add(off);
                }
                return itms;
            }
        }

        public string approve_submited_ris(int[] items)
        {
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                var approve_date = DateTime.Now;
                var eid = USER.C_eID;
                var officeid = USER.C_officeID;
                var OUT = "OUT";
                string query = "";
                foreach (int i in items)
                {
                    query += "UPDATE [IMS].[dbo].[wis_ris_transactions] SET status = 1 ,date_approved ='" + approve_date + "' where risid = " + i.ToString() + ";  insert into [IMS].[dbo].[tbl_wis_transaction] ([eid],[itemid],[itemname],[date],[quantity],[unitcost],[officeid],[unit],[in_out],[ris_id])" +
                                 "  ( select '" + eid + "',itemid,itemname,date_approved,quantity,price,officeid,unit,'" + OUT + "',risid from [IMS].[dbo].[wis_ris_transactions] where risid =" + i.ToString() + " )";
                }

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

        public IEnumerable<item> get_approved_ris()
        {
            List<item> itms = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand("select * from [IMS].[dbo].[wis_ris_transactions] where status= 1", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item off = new item();
                    off.officeid = reader.GetInt32(1);
                    off.officename = reader.GetValue(2).ToString();
                    off.itemid = reader.GetInt32(3);
                    off.itemname = reader.GetString(4);
                    off.unitcost = reader.GetValue(5).ToString();
                    off.unit = reader.GetString(6);
                    off.ris_quantity = reader.GetValue(7).ToString();
                    off.status = reader.GetInt32(8);
                    off.date_approved = reader.GetDateTime(9);
                    off.date_submitted = reader.GetDateTime(11);
                    off.risid = reader.GetValue(12).ToString();
                    itms.Add(off);
                }
                return itms;
            }
        }
        public string update_ris(item items)
        {
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {

                SqlCommand com = new SqlCommand("UPDATE [IMS].[dbo].[wis_ris_transactions] SET quantity = '" + items.ris_quantity + "' where risid ='" + items.risid + "' ", con);
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

        public string delete_ris(item items)
        {
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {

                SqlCommand com = new SqlCommand("delete from  [IMS].[dbo].[wis_ris_transactions]  where risid ='" + items.risid + "' ", con);
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
        public string return_ris(int[] items)
        {
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                var returned_date = DateTime.Now;
                string query = "";
                foreach (int i in items)
                {
                    query += "UPDATE [IMS].[dbo].[wis_ris_transactions] SET status = 0 ,date_returned ='" + returned_date + "' where risid = " + i.ToString() + " ; delete from [IMS].[dbo].[tbl_wis_transaction] where ris_id = '" + i.ToString() + "'";
                }

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

        public IEnumerable<item> get_item_description(string code)
        {
            List<item> itms = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand("select * from [IMS].[dbo].[tbl_itemcode] where itemcode='" + code + "' ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item off = new item();
                    off.itemcode = reader.GetString(1);
                    off.item_description = reader.GetString(2);
                    off.price = reader.GetValue(3).ToString();
                    off.unit = reader.GetString(4);
                    off.itemid = reader.GetInt32(5);
                    itms.Add(off);
                }
                return itms;
            }
        }
        public string instock_readed_code(item items)
        {
            var eid = USER.C_eID;
            var dt = DateTime.Now;
            var inout = "IN";


            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {

               string strcon = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();
                string total_sum = db.totalsum(items.itemid, items.transcode).ToString();
                string query = "";

                DataTable dtr = OleDbHelper.ExecuteDataset(strcon, System.Data.CommandType.Text, "select  a.id,a.officeid,cast(cast(a.quantity as numeric) as int) as quantity,a.ris,a.transaction_code,a.unit,a.itemid,a.price from  [IMS].[dbo].[tbl_items_per_transaction_code] as a where itemid='" + items.itemid + "' and transaction_code='" + items.transcode + "'  group by a.id,a.officeid,a.quantity,a.ris,a.transaction_code,a.unit,a.itemid,a.price order by a.ris asc  ").Tables[0];
                
             

                var countITEMS = items.quantity;

                for (var i = 0; i < items.quantity; )
                {
                    foreach (DataRow row in dtr.Rows)
                    {
                        if (i < countITEMS)
                        {
                            if (row[2].Equals(row[3]))
                            {

                            }
                            else
                            {
                                var num = row[3].ToString();
                                row[3] = Int32.Parse(num) + 1;
                                query += " update  [IMS].[dbo].[tbl_items_per_transaction_code] set ris = ris + 1 where id= '" + row[0].ToString() + "'";
                                i++;
                            }
                        }
                    }
                }

                SqlCommand com = new SqlCommand(query, con);
                SqlCommand coc = new SqlCommand("insert into [IMS].[dbo].[tbl_wis_transaction] (eid,itemcode,itemid,itemname,date,quantity,unitcost,in_out,unit,transcode) values ('" + eid + "','" + items.itemcode + "','" + items.itemid + "','" + items.itemname + "','" + dt + "','" + items.quantity + "','" + items.price + "','" + inout + "','" + items.unit + "','" + items.transcode + "')", con);

                con.Open();
                try
                {
                    com.ExecuteNonQuery();
                    coc.ExecuteNonQuery();
                    return "1";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }

        public IEnumerable<item> get_added_recent()
        {
            List<item> itms = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand("select * from [IMS].[dbo].[tbl_wis_transaction] where in_out = 'IN' ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item off = new item();
                    off.id = reader.GetInt32(0);
                    off.itemname = reader.GetString(4);
                    off.date = reader.GetDateTime(5);
                    off.totalin = reader.GetValue(6).ToString();
                    off.unit = reader.GetString(14);
                    off.price = reader.GetValue(7).ToString();
                    off.in_out = reader.GetString(9);

                    itms.Add(off);
                }
                return itms;
            }
        }

        public string insert_items_percode(string transaction_code)
        {
            DataTable dt;
            string qry = "";
            epsws.serviceSoapClient r = new epsws.serviceSoapClient();

            dt = r.GetItems(transaction_code);

            foreach (DataRow row in dt.Rows)
            {
                qry += @" insert into [IMS].[dbo].[tbl_items_per_transaction_code] (officeid,officename,quantity,itemid,unit,price,itemname,transaction_code,ris,released,officecode,mainoffice,accountid) values ('" + row["officeid"] + "','" + row["office"].ToString().Replace("'","''")+ "','" + Convert.ToInt32(row["quantity"]) + "','" + row["itemid"] + "','" + row["unit"] + "','" + row["unitcost"] + "','" + row["itemname"].ToString() .Replace("'", "''") + "','" + transaction_code + "','0','0','" + row["officecode"] + "','" + row["mainofficeid"] + "','"+row["accountid"]+"')";
            }
            try
            {
                (qry).NonQuery();
                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            #region lambda
            
            
            //var rec = r.GetItems(transaction_code)
            //    .AsEnumerable()
            //    .Select(b => new
            //    {
            //        itemid = b.Field<int>("itemid"),
            //        officeid = b.Field<int>("officeid"),
            //        officecode = b.Field<string>("officecode"),
            //        itemname = b.Field<string>("itemname"),
            //        unit = b.Field<string>("unit"),
            //        unitcost = b.Field<decimal>("unitcost"),
            //        quantity = b.Field<decimal>("quantity")
            //    })
            //     .GroupBy(ac => new
            //     {

            //         ac.itemid,
            //         ac.officecode,
            //         ac.officeid,// required by your view model. should be omited
            //         // in most cases because group by primary key
            //         // makes no sense.
            //         ac.unit,
            //         ac.itemname,
            //         ac.unitcost,
            //     })
            //      .Select(ac => new
            //      {
            //          officeid = ac.Key.officeid,
            //          officecode = ac.Key.officecode,
            //          iquantity = ac.Sum(acs => acs.quantity),

            //          itemname = ac.Key.itemname,
            //          itemid = ac.Key.itemid,
            //          unit = ac.Key.unit,
            //          unitcost = ac.Key.unitcost,
            //      })
            //    .ToList();



            //using (SqlConnection con = new SqlConnection(common.MyConnection()))
            //{
            //    //'" + i.officename.Replace("'","''''") + "'
            //    string query = "";
            //    var start = 0;
            //    foreach (var i in rec)
            //    {
            //        query += " insert into [IMS].[dbo].[tbl_items_per_transaction_code] (officeid,officename,quantity,itemid,unit,price,itemname,transaction_code,ris,released) values ('" + i.officeid + "','" + i.officecode + "','" + Convert.ToInt32(i.iquantity) + "','" + i.itemid + "','" + i.unit + "','" + i.unitcost + "','" + i.itemname.Replace("'", "`") + "','" + transaction_code + "','" + start + "','" + start + "')" + System.Environment.NewLine;
            //    }
            //    SqlCommand com = new SqlCommand(query, con);
            //    con.Open();

            //    try
            //    {
            //        com.ExecuteNonQuery();
            //        return "1";
            //    }
            //    catch (Exception ex)
            //    {
            //        return ex.Message;
            //    }
            //}
            #endregion   
        }
        public string save_new_itemcode(item items)
        {

            string strcon = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();

            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand(" insert into [IMS].[dbo].[tbl_itemcode] (itemcode,item_description,unit,itemid) values ('" + items.itemcode + "','" + items.itemname + "','" + items.unit + "','" + items.itemid + "')", con);
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
        public IEnumerable<item> get_ris_print_method(string transcode)
        {
            List<item> itms = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand("select a.officeid,a.transaction_code,a.officename from  [IMS].[dbo].[tbl_items_per_transaction_code] as a where transaction_code='" + transcode + "' and ris != released group by a.officeid,a.transaction_code,a.officename", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item off = new item();
                    off.officeid = reader.GetInt32(0);
                    off.transcode = reader.GetString(1);
                    off.officename = reader.GetString(2);
                    itms.Add(off);
                }
                return itms;
            }
        }
        public IEnumerable<item> get_ris_report(string transcode, int officeid, string officename)
        {
            List<item> itms = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                //SqlCommand com = new SqlCommand("select officeid,officename,CAST(quantity as numeric) as quantity,transaction_code,itemname,unit,itemid,price,ris,released,ris - released as toprint FROM [IMS].[dbo].[tbl_items_per_transaction_code] where transaction_code='"+transcode+"' and officeid='"+officeid+"' and ris !=0", con);
                //  SqlCommand com = new SqlCommand("select id,transaction_code,itemname,unit,price,quantity,released,officename,officeid,itemid,ris FROM [IMS].[dbo].[tbl_items_per_transaction_code] where officeid= '" + officeid + "' and transaction_code= '" + transcode + "' and  ris != released", con);
                //  SqlCommand com = new SqlCommand(@"select a.id,a.transaction_code,a.itemname,a.unit,a.price,a.quantity,a.released,a.officename,a.officeid,a.itemid,a.ris,b.bid,b.quantity as borrowed_quantity,b.transaction_code as borrowed_source FROM [IMS].[dbo].[tbl_items_per_transaction_code] as  a  LEFT JOIN  [IMS].[dbo].[wis_borrowed_items] as b on a.itemid = b.itemid and a.officeid = b.officeid  where a.officeid= '" + officeid + "' and a.transaction_code= '" + transcode + "'  and  a.ris != a.released ", con);
                SqlCommand com = new SqlCommand(@"
 SELECT 
* 
FROM
(select a.id,a.transaction_code,a.itemname,a.unit,a.price,a.quantity,a.released,a.officename,a.officeid,a.itemid,a.ris 
FROM [IMS].[dbo].[tbl_items_per_transaction_code] as a) t1 
LEFT JOIN
(select b.itemid,b.itemname,b.officeid, sum(b.quantity) as total_borrow  FROM [IMS].[dbo].[wis_borrowed_items] as b where b.status = 1 and b.paid = 0 
group by  b.itemid,b.itemname,b.officeid) t2
ON t1.officeid = t2.officeid and t1.itemid = t2.itemid 
where t1.officeid= '" + officeid + "'  and t1.transaction_code= '" + transcode + "' and  t1.ris != t1.released", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item off = new item();

                    off.id = reader.GetInt32(0);
                    off.transcode = reader.GetString(1);
                    off.itemname = reader.GetString(2);
                    off.unit = reader.GetString(3);
                    off.rprice = reader.GetDecimal(4);
                    off.qty = reader.GetValue(5).ToString();
                    off.released = reader.GetInt32(6);
                    off.officename = reader.GetString(7);
                    off.officeid = reader.GetInt32(8);
                    off.itemid = reader.GetInt32(9);
                    off.ris = reader.GetInt32(10);
                    //off.str_bid = reader.GetValue(11).ToString();
                    // off.str_borrowed_qty = reader.GetValue(12).ToString();
                    off.borrowed_quantity = reader.GetValue(14) is DBNull ? 0 : reader.GetInt32(14);
                    // off.borrowed_source = reader.GetString(13);
                    off.deducted = off.ris - off.released;
                    off.toprint = off.deducted - off.borrowed_quantity;
                    off.totalamount = off.rprice * off.toprint;

                    itms.Add(off);
                }
                return itms;
            }
        }
        public IEnumerable<item> get_item_by_tnum_offid(string transcode, int officeid)
        {
            List<item> itms = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                // SqlCommand com = new SqlCommand("select id,transaction_code,itemname,unit,price,quantity,released,officename,officeid,itemid,ris FROM [IMS].[dbo].[tbl_items_per_transaction_code] where officeid= '" + officeid + "' and transaction_code= '" + transcode + "' and  ris != released", con);
                SqlCommand com = new SqlCommand(@"select * from(select id,transaction_code,itemname,unit,price,quantity,released,officename,officeid,itemid,ris 
FROM [IMS].[dbo].[tbl_items_per_transaction_code]
 where officeid= '" + officeid + "' and transaction_code= '" + transcode + "' and  ris != released) t1  LEFT JOIN ( select b.itemid,b.officeid, sum(b.quantity) as total_borrow FROM [IMS].[dbo].[wis_borrowed_items] as b where b.status = 1 and b.paid = 0 group by  b.itemid,b.itemname,b.officeid) t2 ON t1.officeid = t2.officeid and t1.itemid = t2.itemid ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item off = new item();
                    off.id = reader.GetInt32(0);
                    off.transcode = reader.GetString(1);
                    off.itemname = reader.GetString(2);
                    off.unit = reader.GetString(3);
                    off.rprice = reader.GetDecimal(4);
                    off.qty = reader.GetValue(5).ToString();
                    off.released = reader.GetInt32(6);
                    off.officename = reader.GetString(7);
                    off.officeid = reader.GetInt32(8);
                    off.itemid = reader.GetInt32(9);
                    off.ris = reader.GetInt32(10);
                    off.borrowed_quantity = reader.GetValue(13) is DBNull ? 0 : reader.GetInt32(13);
                    off.toprint = off.ris - off.released;
                    off.available = off.ris - off.released;

                    itms.Add(off);
                }
                return itms;
            }
        }
        public string confirm_out_na(item items)
        {
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                var in_out = "IN";
                var date = DateTime.Today;
                var eid = USER.C_eID;
                var ename = USER.C_Name;
                SqlCommand com = new SqlCommand(@"insert into [IMS].[dbo].[tbl_wis_transaction] (eid,itemid,itemname,date,quantity,unitcost,officeid,in_out,unit,transcode,officename) values('" + eid + "','" + items.itemid + "','" + items.itemname + "','" + date + "','" + items.toprint + "','" + items.rprice + "','" + items.officeid + "','OUT','" + items.unit + "','" + items.transcode + "','" + items.officename + "')  "
                 + " update [IMS].[dbo].[tbl_items_per_transaction_code] set released = released + '" + items.toprint + "' where id = '" + items.id + "' ; insert into [IMS].[dbo].[tbl_wis_transaction] (eid,itemid,itemname,date,quantity,unitcost,officeid,in_out,unit,ris_id,transcode,officename) (select '" + eid + "',itemid,itemname,'" + date + "',quantity,price,officeid,'IN',unit,bid,transaction_code,officename from  [IMS].[dbo].[wis_borrowed_items] where itemid='" + items.itemid + "' and officeid ='" + items.officeid + "' and status = 1 and paid = 0) ; Update  [IMS].[dbo].[wis_borrowed_items] set paid = 1 where officeid='" + items.officeid + "' and itemid = '" + items.itemid + "' and status = 1 and paid = 0 ;"
                 + " insert into [IMS].[dbo].[transaction] (eid,item_id,stock_date,quantity,in_out,officeid,description,itemname,unit,ename) values ('" + eid + "','" + items.itemid + "', '" + date + "','" + items.toprint+ "','"+in_out+"','" +items.officeid+ "','" + items.descript + "','" + items.itemname + "' ,'" + items.unit + "','"+ename+"')" , con);
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
        public IEnumerable<item> history(string transcode)
        {
            List<item> itms = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand("select officename,officeid,itemname,quantity,unit,date FROM [IMS].[dbo].[tbl_wis_transaction] where transcode = '" + transcode + "' and in_out = 'OUT' order by date asc", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item off = new item();
                    off.officename = reader.GetValue(0).ToString();
                    off.officeid = reader.GetInt32(1);
                    off.itemname = reader.GetString(2);
                    off.quantity = reader.GetInt32(3);
                    off.unit = reader.GetString(4);
                    off.date = reader.GetDateTime(5);
                    itms.Add(off);
                }
                return itms;
            }
        }
        public IEnumerable<item> remaining_wis(string transcode)
        {
            List<item> itms = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand(@"SELECT   isnull((row_number() OVER (ORDER BY [itemid])), 0) AS trnid,transcode, itemname, unit,unitcost, [itemid], SUM(CASE WHEN in_out = 'IN' THEN quantity ELSE 0 END) - SUM(CASE WHEN in_out = 'OUT' THEN quantity ELSE 0 END) 
                                AS total FROM            [IMS].[dbo].[tbl_wis_transaction] 
                                where transcode = '" + transcode + "' GROUP BY transcode,itemid, itemname, unit,unitcost", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item off = new item();
                    //off.id = reader.GetInt32(0);
                    off.transcode = reader.GetString(1);
                    off.itemname = reader.GetString(2);
                    off.unit = reader.GetString(3);
                    off.rprice = reader.GetDecimal(4);
                    off.itemid = reader.GetInt32(5);
                    off.total = reader.GetInt32(6);
                    itms.Add(off);
                }
                return itms;
            }
        }
        public string save_borrow(item items)
        {
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                string qry = "";
                var zero = 0;
                var dt = DateTime.Today;
                if (items.quantity > items.total)
                {
                    qry = "";
                }
                else if (items.quantity < 0)
                {
                    qry = "";
                }
                else if (items.quantity <= items.total && items.quantity > 0)
                {
                    qry = "insert into [IMS].[dbo].[wis_borrowed_items] (itemid,itemname,price,quantity,unit,date,officename,officeid,transaction_code,status,paid) values(" + items.itemid + ",'" + items.itemname + "','" + items.price + "','" + items.quantity + "','" + items.unit + "','" + dt + "','" + items.officename.Replace("'", "`") + "','" + items.officeid + "','" + items.transcode + "'," + zero + "," + zero + ")";
                }

                SqlCommand com = new SqlCommand(qry, con);
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
        public IEnumerable<item> get_submited_req(string transcode)
        {
            List<item> itms = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand(@"select * from [IMS].[dbo].[wis_borrowed_items] where transaction_code = '" + transcode + "' and status = 0", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item off = new item();
                    off.id = reader.GetInt32(0);
                    off.itemid = reader.GetInt32(1);
                    off.itemname = reader.GetString(2);
                    off.rprice = reader.GetDecimal(3);
                    off.quantity = reader.GetInt32(4);
                    off.unit = reader.GetString(5);
                    off.date = reader.GetDateTime(6);
                    off.officename = reader.GetString(7);
                    off.officeid = reader.GetInt32(8);
                    off.transcode = reader.GetString(9);
                    off.status = reader.GetInt32(10);

                    itms.Add(off);
                }
                return itms;
            }
        }
        public IEnumerable<item> get_approve_req(string transcode)
        {
            List<item> itms = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand(@"select * from [IMS].[dbo].[wis_borrowed_items] where transaction_code = '" + transcode + "' and status = 1", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item off = new item();
                    off.id = reader.GetInt32(0);
                    off.itemid = reader.GetInt32(1);
                    off.itemname = reader.GetString(2);
                    off.rprice = reader.GetDecimal(3);
                    off.quantity = reader.GetInt32(4);
                    off.unit = reader.GetString(5);
                    off.date = reader.GetDateTime(6);
                    off.officename = reader.GetString(7);
                    off.officeid = reader.GetInt32(8);
                    off.transcode = reader.GetString(9);
                    off.status = reader.GetInt32(10);

                    itms.Add(off);
                }
                return itms;
            }
        }
        public string approve_borrow(int[] items)
        {
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                var approve_date = DateTime.Now;
                var eid = USER.C_eID;
                var officeid = USER.C_officeID;
                var OUT = "OUT";
                string query = "";
                foreach (int i in items)
                {
                    query += "UPDATE [IMS].[dbo].[wis_borrowed_items] SET status = 1 where bid =" + i.ToString() + " ; insert into [IMS].[dbo].[tbl_wis_transaction] (eid,officeid,ris_id,itemid,itemname,quantity,unitcost,in_out,unit,transcode,date,officename) " +
                        "(select '" + eid + "',officeid,bid,itemid,itemname,quantity,price,'" + OUT + "',unit,transaction_code,'" + approve_date + "',officename from [IMS].[dbo].[wis_borrowed_items] where bid = " + i.ToString() + ")";
                }

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
        public IEnumerable<item> get_for_approve_req()
        {
            List<item> itms = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand("select * from [IMS].[dbo].[wis_borrowed_items] where status = 0", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item off = new item();
                    off.id = reader.GetInt32(0);
                    off.itemid = reader.GetInt32(1);
                    off.itemname = reader.GetString(2);
                    off.rprice = reader.GetDecimal(3);
                    off.quantity = reader.GetInt32(4);
                    off.unit = reader.GetString(5);
                    off.date = reader.GetDateTime(6);
                    off.officename = reader.GetString(7);
                    off.officeid = reader.GetInt32(8);
                    off.transcode = reader.GetString(9);
                    off.status = reader.GetInt32(10);

                    itms.Add(off);
                }
                return itms;
            }
        }
        public IEnumerable<item> get_borrow_groupby_office()
        {
            List<item> itms = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand("  select officeid,officename from [IMS].[dbo].[wis_borrowed_items] where status = 0 group by officeid,officename ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item off = new item();
                    off.officeid = reader.GetInt32(0);
                    off.officename = reader.GetString(1);
                    itms.Add(off);
                }
                return itms;
            }
        }
        public IEnumerable<borrow_item> get_borrow_ris_report(int officeid1, string officename1)
        {
            List<borrow_item> itms = new List<borrow_item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand(@"select itemid,itemname,price,SUM(quantity) as quantity,unit,officename,officeid FROM [IMS].[dbo].[wis_borrowed_items]
   where officeid = '" + officeid1 + "' and status = 0  group by itemid,itemname,price,unit,officename,officeid", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    borrow_item off = new borrow_item();
                    off.itemid = reader.GetInt32(0);
                    off.itemname = reader.GetString(1);
                    off.price = reader.GetDecimal(2);
                    off.quantity = reader.GetInt32(3);
                    off.unit = reader.GetString(4);
                    off.officename = reader.GetString(5);
                    off.officeid = reader.GetInt32(6);
                    off.totalamount = off.quantity * off.price;
                    itms.Add(off);
                }
                return itms;
            }
        }
        public IEnumerable<item> get_monitoring(string transcode)
        {
            List<item> itms = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand(@"select itemname,itemid, sum(ris) - SUM(released) as total,unit FROM [IMS].[dbo].[tbl_items_per_transaction_code] 
    where transaction_code= '" +transcode+"'  group by  itemname,unit,itemid", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item it = new item();
                    it.itemname = reader.GetString(0);
                    it.itemid = reader.GetInt32(1);
                    it.toprint = reader.GetInt32(2);
                    it.unit = reader.GetString(3);
                    itms.Add(it);
                }
            }
            return itms;
        }
        public IEnumerable<item> get_monitoring_all()
        {
            List<item> itms = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand(@"select itemname,itemid, sum(ris) - SUM(released) as total,unit FROM [IMS].[dbo].[tbl_items_per_transaction_code] 
                                                 group by  itemname,unit,itemid", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item it = new item();
                    it.itemname = reader.GetString(0);
                    it.itemid = reader.GetInt32(1);
                    it.toprint = reader.GetInt32(2);
                    it.unit = reader.GetString(3);
                    itms.Add(it);
                }
            }
            return itms;
        }
        public IEnumerable<item> get_availability(int officeid)
         {
            List<item> itms = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand(@"select transaction_code FROM [IMS].[dbo].[tbl_items_per_transaction_code]
 where released != cast(quantity as numeric) and ris!=released and officeid = '"+officeid+"' group by transaction_code", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    item it = new item();
                    it.transcode = reader.GetString(0);
                    itms.Add(it);
                }
            }
            return itms;
        }
        public IEnumerable<item> get_off()
        {
            List<item> itms = new List<item>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand(@"select officeid,officename FROM [IMS].[dbo].[tbl_items_per_transaction_code] group by officeid,officename", con);
                con.Open();
                 SqlDataReader reader = com.ExecuteReader();
                     while(reader.Read())
                     {
                         item it = new item();
                         it.officeid = reader.GetInt32(0);
                         it.officename = reader.GetString(1);
                         itms.Add(it);
                     }
            }
            return itms;
        }
         
    }
}