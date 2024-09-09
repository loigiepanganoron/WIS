namespace IMS.Reports
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using IMS.Models;
    using System.Data;
    using System.Collections;
    using IMS.@class;
    using System.Globalization;

    /// <summary>
    /// Summary description for ris_report.
    /// </summary>
    public partial class ris_report : Telerik.Reporting.Report
    {
        public ris_report(string transcode,int officeid, string officename)
        {
            wis r = new wis();
            
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            
            officename_ni.Value = officename.ToString();
            transcode_ni.Value = transcode.ToString();
            var rd = r.get_ris_report(transcode, officeid, officename);
            objectDataSource1.DataSource = rd; 
        }
    }
}