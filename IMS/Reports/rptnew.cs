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
    /// Summary description for Report1.
    /// </summary>
    public partial class rptnew : Telerik.Reporting.Report
    {
        private pmisEntities pmis = new pmisEntities();
        private IMSEntities db = new IMSEntities();
        public rptnew( string in_out,int itemid, DateTime start, DateTime end, int offid, String off)
        {
            
            // Required for telerik Reporting designer support

            walayclass a = new walayclass();
            InitializeComponent();

            
            // TODO: Add any constructor code after InitializeComponent call

            epsws.serviceSoapClient r = new epsws.serviceSoapClient();
            String offname = off;
            //String iName;
            //if (itemid != 0)
            //    iName = r.POItems(offid).AsEnumerable().Where(b => b.Field<int>("itemid") == itemid).CopyToDataTable().Rows[0]["itemname"].ToString();
            //else
               // iName = "All";
               // itemNames.Value = iName;
                off1.Value = offname;
                from.Value = start.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                to.Value = end.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                var myread = a.readwis( in_out,start, end, itemid,offid);
                objectDataSource2.DataSource = myread; 
        }
    }
}
  
