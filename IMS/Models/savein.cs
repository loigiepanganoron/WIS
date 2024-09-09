using IMS.@class;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace IMS.Models
{
    public class savein
    {
        public class samplemodel {
            public double reorder { get; set; }
            public string nt { get; set; }
            public string ny { get; set; }
            public int totalout { get; set; }
            public int unitcost { get; set; }
            public int eid { get; set; }
            public int itemid { get; set; }
            public int tobeout { get; set; }
            public DateTime timein { get; set; }
            public int quantity { get; set; }
            public int officeid { get; set; }
            public string in_out { get; set; }
            public string itemname { get; set; }
            public string EmpName { get; set; }
            public string descript { get; set; }
            public string qty { get; set; }
            public int total { get; set; }
            public string unit { get; set; }
            public string newtotal { get; set; }
            public string ntotal { get; set; }
            public string quit { get; set; }
            public int average { get; set; }
        }
        public string SaveNew(item it )
        {
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                var date = DateTime.Now;
                var eid = USER.C_eID;
                SqlCommand com = new SqlCommand("insert into [IMS].[dbo].[transact](eid,itemid,itemname,date,quantity,unitcost,officeid,in_out,descript,srcid,accountid,tid,unit,obj,reid) values('" + eid + "','" + it.itemid + "', '" + it.itemname + "' ,'" + date.ToString("MM-dd-yyyy") + "','" + it.quantity + "','" + it.unitcost + "','" + it.officeid + "','" + it.in_out + "','" + it.descript + "','" + it.srcid + "','" + it.accountid + "','" + it.tid + "','" + it.unit + "','" + it.obj + "','" + it.reid + "')", con);
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

        public string plcsaveout(item it)
        {
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                var date = DateTime.Now;
                var en = USER.C_Name;
                var eid = USER.C_eID;
                SqlCommand com = new SqlCommand("insert into [IMS].[dbo].[plc] values('" + en + "','" + date.ToString("MM-dd-yyyy") + "','" + it.itemid + "','" + it.itemname + "' ,'" + it.in_out + "', '" + it.quit + "','" + it.unit + "','" + it.srcid + "','" + it.unitcost + "','" + eid + "','" + it.reid + "')", con);
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
        public string SaveOut(samplemodel it)
         {
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                var date = DateTime.Now;
                SqlCommand com = new SqlCommand("insert into [IMS].[dbo].[transact] (eid,itemid,date,quantity,officeid,in_out,descript)  values('" + it.eid + "','" + it.itemid + "','" + date.ToString("MM-dd-yyyy") + "','" + it.quantity + "','" + it.officeid + "','" + it.in_out + "','" + it.descript + "')", con);
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
        public string ins(item it)
        {
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand("insert into [IMS].[dbo].[plcitems] (itemname,unit,unitcost)  values('" + it.itemname+ "','" + it.unit+ "','" +it.unitcost+ "')", con);
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

        public string b(item it)
        {
            var eid = USER.C_eID;
            var name = USER.C_Name;
            var date = DateTime.Now;
            var in_out = "IN";
            var reid = "";
            
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand("insert into [IMS].[dbo].[plc] values('" + name + "','" + date.ToString("MM-dd-yyyy") + "','" + it.itemid + "','" + it.itemname + "','" + in_out + "','" + it.quit + "','" + it.unit + "','" + it.srcid + "','" + it.unitcost + "','" + eid + "','" + reid + "')", con);
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

        public string byofficeout(item it)
        {
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                var date = DateTime.Now;
                var eid = USER.C_eID;
                var ename = USER.C_Name;
                var officeid = USER.C_officeID;
                SqlCommand com = new SqlCommand("insert into [IMS].[dbo].[transaction] (eid,item_id,stock_date,quantity,in_out,officeid,description,itemname,unit,reid,ename) values ('" + eid + "','" + it.itemid + "', '" +date.ToString("MM-dd-yyyy HH:mm:ss")+ "','" + it.quit + "','" + it.in_out + "','" +officeid + "','" + it.descript + "','" + it.itemname.Replace("'","''") + "' ,'" + it.unit + "','" +it.reid+"','"+ename+"')", con);
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
        public string update(item it)
        {
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand("UPDATE [IMS].[dbo].[plcitems] SET itemname = '"+it.itemname+"', unit = '"+it.unit+"' where itemid ='"+it.itemid+"'", con);
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
        public string destroy(item it)
        {
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand("delete from [IMS].[dbo].[plcitems] where itemid = '" + it.itemid + "'", con);
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
        public void apr_bb_entry(IEnumerable<item> Items)
        {
            var date = DateTime.Now;
            var eid = USER.C_eID;
            var IN = "IN";
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                con.Open();
                foreach (item item in Items)
                {
                    if (item.quantity > 0 && item.iquantity >= item.quantity)
                    {
                        SqlCommand com = new SqlCommand(@"insert into [IMS].[dbo].[tbl_wis_transaction](eid,itemid,itemname,date,quantity,unitcost,officeid,in_out,unit) values('" + eid + "','" + item.itemid + "', '" + item.itemname + "' ,'" + date.ToString("MM-dd-yyyy")+ "','" + item.quantity + "','" + item.unitcost + "','" + item.officeid + "','" + IN + "','" + item.unit + "')", con);
                        com.ExecuteNonQuery();
                    }
                }
            }
        }
        //public string insert_tbl_wis_allocated()
        //{ 
                
        //}
    }
}