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
    using System.Collections.Generic;
    using System.Linq;
    using System.Globalization;
    /// <summary>
    /// Summary description for rptemployee.
    /// </summary>
    public partial class rptemployee : Telerik.Reporting.Report
    {
        private pmisEntities pmis = new pmisEntities();
        private IMSEntities db = new IMSEntities();
        public rptemployee(string status, int eid, int itemid,DateTime start, DateTime end)
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            officeName.Value = USER.C_Office;
            empName.Value = pmis.vwMergeAllEmployees.SingleOrDefault(a => a.eid == eid).EmpName;
            startd.Value = start.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            endd.Value = end.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);

            mymodel classname = new mymodel();
            empDataSource.DataSource =  classname.reademployee(status, eid, itemid, start, end);
        }
    }
}