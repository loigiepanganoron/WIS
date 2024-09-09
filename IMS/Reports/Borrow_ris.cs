namespace IMS.Reports
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using IMS.Models;
    using IMS.@class;
    

    /// <summary>
    /// Summary description for Borrow_ris.
    /// </summary>
    public partial class Borrow_ris : Telerik.Reporting.Report
    {
        public Borrow_ris(int officeid1,string officename1)
        {

            wis r = new wis();
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //

            officename.Value = officename1.ToString();
            var rd = r.get_borrow_ris_report(officeid1,officename1);
            objectDataSource1.DataSource = rd; 
        }
    }
}