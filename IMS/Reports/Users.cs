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
    /// Summary description for Users.
    /// </summary>
    public partial class Users : Telerik.Reporting.Report
    {
        IMSEntities db = new IMSEntities();

        public Users()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //

            objectDataSource1.DataSource = db.vwUsers;
        }
                   
    }
}