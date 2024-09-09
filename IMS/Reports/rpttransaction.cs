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
	/// Summary description for rptTransaction.
	/// </summary>
    public partial class rptTransaction : Telerik.Reporting.Report
	{
        private pmisEntities pmis = new pmisEntities();
        private IMSEntities db = new IMSEntities();
		public rptTransaction(string status, int itemid, DateTime start, DateTime end)
		{
			//
			// Required for telerik Reporting designer support
			//
			InitializeComponent();
            mymodel a = new mymodel();
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
            epsws.serviceSoapClient r = new epsws.serviceSoapClient();
            //var rec = r.POItems(USER.C_officeID).AsEnumerable().Where(b => b.Field<int>("itemid") == itemid).Field<int>("itemName");
            String iName;
            if (itemid != 0)
                iName = r.POItems(USER.C_officeID).AsEnumerable().Where(b => b.Field<int>("itemid") == itemid).CopyToDataTable().Rows[0]["itemname"].ToString();
            else
            iName = "All Items";
            officeName.Value = USER.C_Office;
            itemName.Value = iName;
            //MMMM dd, yyyy
            startd.Value = start.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            endd.Value = end.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);

            var vr = a.readtransaction(status, itemid, start, end);
            objectDataSource1.DataSource = vr;
		}
	}
}