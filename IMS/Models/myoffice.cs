using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IMS.Models;
using System.Data.SqlClient;


namespace IMS.Models
{
    public class office {
        public int OfficeId { get; set; }
        public string OfficeName { get; set; }
    }
    public class myoffice
    {
        public IEnumerable<office> Offices()
        {

            List<office> offices = new List<office>();
            using (SqlConnection con = new SqlConnection(common.MyConnection()))
             {
                SqlCommand com = new SqlCommand("select * from [pmis].[dbo].[m_vwOffices] ", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    office off = new office();
                    off.OfficeId = reader.GetInt32(0);
                    off.OfficeName = reader.GetString(1);

                    offices.Add(off);
                }
            }
            return offices;
        }

    } 
}