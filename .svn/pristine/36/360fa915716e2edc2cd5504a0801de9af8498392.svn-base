namespace IMS.Reports
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using IMS.@class;
    using IMS.Classess;
    using IMS.Controllers;
    using System.Data;
    using IMS.Models;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    


    /// <summary>
    /// Summary description for office_util.
    /// </summary>
    public partial class office_util : Telerik.Reporting.Report
    {
        public office_util(string date_from, string date_to,int? officeid,string officename)
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            int? finalOfficeId = null;

            if (officename != null)
            {
                textBox5.Value = officename;
                finalOfficeId = officeid;
            }
            else
            {
                textBox5.Value = USER.C_Office.ToString();
                finalOfficeId = USER.C_officeID;
            }
           
            textBox8.Value = date_from.ToString();
            textBox9.Value = date_to.ToString();
             
            DataTable dt = (@"select item_id,itemname ,SUM(quantity) as consume,unit FROM [IMS].[dbo].[transaction]
   where officeid = '" + finalOfficeId + "' and in_out='OUT' and cast(stock_date as date)  between '" + date_from + "' and '" + date_to + "' group by item_id,itemname,unit order by itemname").DataSet();
             
            //DataTable preparedby = (@"select eid,EmpName,position FROM  [pmis].[dbo].[vwMergeAllEmployee]  where eid = '" +USER.C_eID+ "'").DataSet();

            DataTable preparedby = new DataTable();
            using (SqlConnection con = new SqlConnection(common.livecon()))
            {
                SqlCommand com = new SqlCommand(@"select eid,EmpName,position FROM  [pmis].[dbo].[m_vwGetAllEmployee_Minified]  where eid = '" + USER.C_eID + "'", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                preparedby.Load(reader);
                con.Close();
            }






            textBox19.Value = preparedby.Rows[0]["EmpName"].ToString();
            userposition.Value = preparedby.Rows[0]["position"].ToString();


            DataTable notedby = (@"select a.eid,b.EmpName,b.position FROM [IMS].[dbo].[tbl_t_notedby] as a
  inner join [pmis].[dbo].[m_vwGetAllEmployee_Minified] as b on a.eid = b.eid where a.officeid = '" + finalOfficeId + "'").DataSet();
             
            try 
            {

                notedname.Value = notedby.Rows[0]["EmpName"].ToString();
                notedposition.Value = notedby.Rows[0]["position"].ToString();
            }
            catch(Exception)
            {

                notedname.Value = "Not set";
                notedposition.Value = "Not set";
            }

            //var i = 0;
            //List<item> item = new List<item>();
            //foreach (DataRow row in dt.Rows)
            //{
                
            //    item x = new item();
            //    x.itemname = dt.Rows[i]["itemname"].ToString();
            //    x.quantity = Convert.ToInt32(dt.Rows[i]["consume"].ToString());
            //    x.unit = dt.Rows[i]["unit"].ToString(); 
            //    item.Add(x);
            //} 
            objectDataSource1.DataSource = dt;
    
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
    }
}