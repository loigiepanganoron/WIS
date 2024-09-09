namespace IMS.Reports
{
    using IMS.Models;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for statement_of_ending_inventory_new.
    /// </summary>
    public partial class statement_of_ending_inventory_new : Telerik.Reporting.Report
    {
        public statement_of_ending_inventory_new(string date)
        {
            var r = new revise_read();
            DateTime dt = Convert.ToDateTime(date);
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            var data = r.report_ending_inventory(date);
            objectDataSource1.DataSource = data;
            endDate.Value = dt.ToString("MMMM dd, yyyy");
        }
    }
}