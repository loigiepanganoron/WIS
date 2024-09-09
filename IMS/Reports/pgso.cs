namespace IMS.Reports
{
    using Classess;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for pgso.
    /// </summary>
    public partial class pgso : Telerik.Reporting.Report
    {
        public pgso()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            DataTable dt = ("select * from ( SELECT officeid, isnull((row_number() OVER (ORDER BY [item_id])), 0) AS trnid, [item_id], itemname, SUM(CASE WHEN in_out = 'IN' THEN quantity ELSE 0 END) - SUM(CASE WHEN in_out = 'OUT' THEN (CASE WHEN ref_id IS NULL THEN 0 ELSE quantity END) ELSE 0 END) AS total,SUM(CASE WHEN in_out = 'IN' THEN quantity ELSE 0 END) as totalin,SUM(CASE WHEN in_out = 'OUT' THEN (CASE WHEN ref_id IS NULL THEN 0 ELSE quantity END) ELSE 0 END) as totalout, unit FROM [IMS].[dbo].[transaction] where cast(stock_date as date) <= '2021-12-01' and officeid = 72  GROUP BY item_id, officeid, itemname, unit) as a where a.total > 0 order by itemname asc").DataSet();
             
            objectDataSource1.DataSource = dt;
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
    }
}