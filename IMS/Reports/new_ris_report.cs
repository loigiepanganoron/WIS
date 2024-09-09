namespace IMS.Reports
{
    using IMS.Models;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using IMS.Classess;
    using IMS.@class;
    using System.Data.SqlClient;

    /// <summary>
    /// Summary description for ris_revise.
    /// </summary>
    public partial class new_ris_report : Telerik.Reporting.Report
    {
        public new_ris_report(string controlno)
        {
            revise_read r = new revise_read();
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            var data = r.report_ris(controlno);
            objectDataSource1.DataSource = data;

            DataTable dt = (@"select * from [IMS].[dbo].[tbl_t_ris_preparation] where controlno = '"+controlno+"' ").DataSet();
            officename_ni.Value = dt.Rows[0]["officename"].ToString();
            transcode_ni.Value = dt.Rows[0]["controlno"].ToString();
            date_time.Value = dt.Rows[0]["date_time"].ToString();
            var eid = dt.Rows[0]["preparedby"].ToString();

         //   DataTable preparedby = (@"select eid,EmpName,position FROM  [pmis].[dbo].[vwMergeAllEmployee]  where eid = '"+eid+"'").DataSet();
             DataTable preparedby = new DataTable();
             using (SqlConnection con = new SqlConnection(common.livecon()))
             {
                 SqlCommand com = new SqlCommand(@"select eid,EmpName,position FROM  [pmis].[dbo].[m_vwGetAllEmployee_Minified] where eid = '" + eid + "'", con);
                 con.Open();
                 SqlDataReader reader = com.ExecuteReader();
                 preparedby.Load(reader);
                 con.Close();
             }

             try
             {
               // textBox19.Value = preparedby.Rows[0]["EmpName"].ToString();
               // userposition.Value = preparedby.Rows[0]["position"].ToString();
                //sdf.Value = USER.C_Name.ToString();
             //   userposition.Value = USER.C_Position.ToString();
             }
            catch(Exception )
             {
               
             }

            DataTable notedby = (@"select a.eid,b.EmpName,b.position FROM [IMS].[dbo].[tbl_t_notedby] as a
  inner join [pmis].[dbo].[m_vwGetAllEmployee_Minified] as b on a.eid = b.eid where a.officeid = '" + dt.Rows[0]["officeid"].ToString() + "'").DataSet();

            try
            {
                notedname.Value = notedby.Rows[0]["EmpName"].ToString();
                notedposition.Value = notedby.Rows[0]["position"].ToString();
            }
            catch (Exception)
            {

            }

            textBox41.Value = "NAOMI I. DIZA";
            textBox26.Value = "SUPPLY OFFICER IV";

            DataTable request = (@"select a.preparedby,b.EmpName,b.position FROM [IMS].[dbo].[tbl_t_ris_preparation] as a
  inner join [pmis].[dbo].[m_vwGetAllEmployee_Minified] as b on a.preparedby = b.eid where a.officeid = '" + dt.Rows[0]["officeid"].ToString() + "'").DataSet();
             
            try
            {
                textBox39.Value = preparedby.Rows[0]["EmpName"].ToString();
                textBox34.Value = preparedby.Rows[0]["position"].ToString();
            }
            catch (Exception)
            {

            }


           // textBox27.Value = "Purpose :                                       ";
            /*
            DataTable purpose = (@"select tbl_2.year ,case when quarter =1 then '1ST QUARTER'  when quarter = 2 then '2ND QUARTER' when quarter=3 then '3RD QUARTER' when quarter = 4 then '4TH QUARTER' end  as  quarter,case when  mooe_no = 2 then 'MOOE' else 'NON-OFFICE' end as mooe_no, case when dbm_bb = 1 then 'DBM' else 'BULK BIDDING' end as dbm_bb from (select distinct(trans_code) from  [IMS].[dbo].[wis_ris_transactions] where controlno like '%"+controlno+"%') tbl_1 left join (select transcode,year,quarter,mooe_no,dbm_bb from [IMS].[dbo].[tbl_wis_transaction] where in_out = 'IN') tbl_2 on tbl_1.trans_code = tbl_2.transcode").DataSet();

            var str_purpose = purpose.Rows[0]["year"].ToString() + "-" + purpose.Rows[0]["quarter"].ToString() +"-"+ purpose.Rows[0]["mooe_no"].ToString() +"-"+purpose.Rows[0]["dbm_bb"].ToString() ;

            textBox27.Value = "Purpose : " + str_purpose;
             * */
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

    }
}