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
    /// Summary description for rptall.
    /// </summary>
    public partial class rptall : Telerik.Reporting.Report
    {
        public rptall()
        {
            //
            // Required for telerik Reporting designer support
            //
            mymodel a = new mymodel();
            InitializeComponent();
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            var myread = a.readall();
            objectDataSource1.DataSource = myread; 
        }
    }
}