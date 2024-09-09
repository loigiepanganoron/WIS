namespace IMS.Reports
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using IMS.Classess;
    using System.Data;
    using IMS.@class;

    /// <summary>
    /// Summary description for OfficeItemsRemaining.
    /// </summary>
    public partial class OfficeItemsRemaining : Telerik.Reporting.Report
    {
        public OfficeItemsRemaining(string date)
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
             
            office1.Value = USER.C_Office;
           // office2.Value = USER.C_Office;

            preparedby1.Value = USER.C_Name;
            //preparedby2.Value = USER.C_Name;

            preparedby_pos1.Value = USER.C_Position;
            //preparedby_pos2.Value = USER.C_Position;

            office1.Value = USER.C_Office;
            //office2.Value = USER.C_Office;


            date1.Value = date;
            //date2.Value = date;
            //DataTable dt = ("select * from [IMS].[dbo].[vwbyofficeout] where officeid='" + USER.C_officeID+ "' and total > 0").DataSet();
            //DataTable dt = ("select * from ( SELECT officeid, isnull((row_number() OVER (ORDER BY [item_id])), 0) AS trnid, [item_id], itemname, SUM(CASE WHEN in_out = 'IN' THEN quantity ELSE 0 END) - SUM(CASE WHEN in_out = 'OUT' THEN (CASE WHEN ref_id IS NULL THEN 0 ELSE quantity END) ELSE 0 END) AS total, unit FROM [IMS].[dbo].[transaction] where stock_date <= '" + date + "' and officeid = " + USER.C_officeID + "  GROUP BY item_id, officeid, itemname, unit) as a where a.total > 0").DataSet();

            DataTable dt = ("select * from ( SELECT officeid, isnull((row_number() OVER (ORDER BY [item_id])), 0) AS trnid, [item_id], itemname, SUM(CASE WHEN in_out = 'IN' THEN quantity ELSE 0 END) - SUM(CASE WHEN in_out = 'OUT' THEN (CASE WHEN ref_id IS NULL THEN 0 ELSE quantity END) ELSE 0 END) AS total,SUM(CASE WHEN in_out = 'IN' THEN quantity ELSE 0 END) as totalin,SUM(CASE WHEN in_out = 'OUT' THEN (CASE WHEN ref_id IS NULL THEN 0 ELSE quantity END) ELSE 0 END) as totalout, unit FROM [IMS].[dbo].[transaction] where cast(stock_date as date) <= '" + date + "' and officeid = " + USER.C_officeID + "  GROUP BY item_id, officeid, itemname, unit) as a where a.total > 0 order by itemname asc").DataSet();


            DataTable notedby = (@"select a.eid,b.EmpName,b.position FROM [IMS].[dbo].[tbl_t_notedby] as a
  inner join [pmis].[dbo].[vwMergeAllEmployee] as b on a.eid = b.eid where a.officeid = '" + dt.Rows[0]["officeid"].ToString() + "'").DataSet();

            try
            { 
                approvedby1.Value = notedby.Rows[0]["EmpName"].ToString();
                approvedby_pos1.Value = notedby.Rows[0]["position"].ToString();

                //approvedby2.Value = notedby.Rows[0]["EmpName"].ToString();
                //approvedby_pos2.Value = notedby.Rows[0]["position"].ToString();


            }
            catch (Exception)
            {

            }

            objectDataSource1.DataSource = dt;
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
    }
}