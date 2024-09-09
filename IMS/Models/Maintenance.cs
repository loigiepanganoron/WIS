using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace IMS.Models
{
    public class Maintenance
    {
        public class unit {
            public string unitname { get; set; }
            public int ID { get; set;}
        }
        public string deleteunit(int id)
        {
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand("delete [IMS].[dbo].[units] where ID = '"+id+"'", con);
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
        public string addunit(string unit)
        {
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand("insert into [IMS].[dbo].[units](unitname) values('"+unit+"')", con);
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
        public IEnumerable<dynamic> addCategory()
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

                    itms.Add(off);
                }
                return itms;
            }
        }
    }
}