namespace IMS.Reports
{
    using System.Data;
    using IMS.Models;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using IMS.Classess;
    using IMS.@class;
    using System.Data.SqlClient;
    /// <summary>
    /// Summary description for vale_report.
    /// </summary>
    public partial class vale_report : Telerik.Reporting.Report
    {
        public vale_report(int officeid,string from, string to,string offname,string empid)
        {
            revise_read r = new revise_read();
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            DataTable notedby = (@"select a.eid,b.EmpName,b.position FROM [IMS].[dbo].[tbl_t_notedby] as a inner join [pmis].[dbo].[vwMergeAllEmployee] as b on a.eid = b.eid where a.officeid = "+officeid+" ").DataSet();

            try
            {
                notedname.Value = notedby.Rows[0]["EmpName"].ToString();
                notedposition.Value = notedby.Rows[0]["position"].ToString();
            }
            catch (Exception)
            {

            }
            textBox42.Value = "NAOMI I. DIZA";
            textBox47.Value = "SUPPLY OFFICER IV";

            DataTable preparedby = new DataTable();
            using (SqlConnection con = new SqlConnection(common.livecon()))
            {
                SqlCommand com = new SqlCommand(@"select eid,EmpName,position FROM  [pmis].[dbo].[vwMergeAllEmployee]  where eid = '" + empid + "'", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                preparedby.Load(reader);
                con.Close();
            }

            try
            {
                  textBox41.Value = preparedby.Rows[0]["EmpName"].ToString();
                  textBox37.Value = preparedby.Rows[0]["position"].ToString();
                //sdf.Value = USER.C_Name.ToString();
                //   userposition.Value = USER.C_Position.ToString();
            }
            catch (Exception)
            {

            }




            officename_ni.Value = offname.ToString();
            var rd = r.get_office_items(officeid, from, to );
            objectDataSource1.DataSource = rd; 
        }
    }
}