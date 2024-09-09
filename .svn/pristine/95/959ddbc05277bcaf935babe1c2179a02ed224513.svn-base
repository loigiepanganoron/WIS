using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace IMS.Models
{
    public class walayclass
    {
        public  string itemname {get;set;}
        public  string totalin {get;set;}
        public  string totalout {get;set;}
        public string unit {get;set;}
        public string unitcost {get;set;}
        public string totalcostout {get;set;}
        public string totalcostin {get;set;}

        public IEnumerable<dynamic> readplc(string from, string to, string in_out, int itemid)
        {

            List<walayclass> itms = new List<walayclass>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand("exec plcrep '" + from + "','" + to + "'", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    walayclass off3 = new walayclass();

                    off3.itemname = reader.GetString(0);
                    off3.totalin = reader.GetValue(1).ToString();
                    off3.totalout = reader.GetValue(2).ToString();
                    off3.unit = reader.GetString(3);
                    off3.unitcost = reader.GetValue(4).ToString();
                    off3.totalcostout = reader.GetValue(5).ToString();
                    off3.totalcostin = reader.GetValue(6).ToString();

                    itms.Add(off3);
                }
                return itms;
            }
        }
        public IEnumerable<dynamic> readwis(string in_out,DateTime from, DateTime to, int itemid,int offid)
        {

            List<walayclass> itms = new List<walayclass>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
            {
                SqlCommand com = new SqlCommand("exec wisrep '" + from + "','" + to + "','"+offid+"'", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    walayclass off3 = new walayclass();

                    off3.itemname = reader.GetString(0);
                    off3.totalin = reader.GetValue(1).ToString();
                    off3.totalout = reader.GetValue(2).ToString();
                    off3.unit = reader.GetString(3);
                    off3.unitcost = reader.GetValue(4).ToString();
                    off3.totalcostout = reader.GetValue(5).ToString();
                    off3.totalcostin = reader.GetValue(6).ToString();

                    itms.Add(off3);
                }
                return itms;
            }
        }
    }

}