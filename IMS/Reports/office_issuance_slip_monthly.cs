namespace IMS.Reports
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using IMS.@class;
    using IMS.Classess;

    /// <summary>
    /// Summary description for office_issuance_slip_monthly.
    /// </summary>
    public partial class office_issuance_slip_monthly : Telerik.Reporting.Report
    {
        public office_issuance_slip_monthly(string monthly, int eid, string qr)
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            DateTime yearmonth = Convert.ToDateTime(monthly);
            int month = yearmonth.Month;
            int year = yearmonth.Year;

            DataTable dt = (@"select a.item_id,a.itemname,a.unit, SUM(CASE WHEN a.in_out = 'OUT' THEN a.quantity ELSE 0 END) as quantity,a.reid,b.EmpName,cast(a.stock_date as date) as date from [IMS].[dbo].[transaction]  as a
  inner join [pmis].[dbo].[m_vwGetAllEmployee]  as b on a.reid = b.eid 
   where  a.officeid = '" + USER.C_officeID + "' and a.reid = '"+eid+"' and a.in_out = 'OUT' and YEAR(a.stock_date) = '" + year + "' and MONTH(a.stock_date) = '" + month + "'  group by a.item_id,a.itemname,a.unit,a.reid,b.EmpName,cast(a.stock_date as date) ").DataSet();


            textBox5.Value = USER.C_Office;
            textBox10.Value = USER.C_Office;


            string[] it = USER.C_Name.Split(',');
             

            textBox24.Value = it[1] + it[0].ToString().Replace(",", ""); ;
            textBox33.Value = it[1] + it[0].ToString().Replace(",", ""); ;

            textBox36.Value = monthly;
            textBox38.Value = monthly;




            textBox39.Value = dt.Rows[0]["EmpName"].ToString();
            textBox42.Value = dt.Rows[0]["EmpName"].ToString();

            barcode1.Value = qr;
            textBox14.Value = "System Generated Report " + DateTime.Now;

            objectDataSource1.DataSource = dt;



            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
    }
}