namespace IMS.Reports
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using IMS.Classess;
    using System.Data.SqlClient;
    using IMS.Models;
    using IMS.@class;

    /// <summary>
    /// Summary description for office_issuance_slip.
    /// </summary>
    public partial class office_issuance_slip : Telerik.Reporting.Report
    {
        public office_issuance_slip(string code, string qr)
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();



            int count = (@"select  count(distinct(reid))  from [IMS].[dbo].[transaction] where officeid  =  '"+USER.C_officeID+"' and controlno = '"+code+"' ").Scalar();

            var issued_to = "";
            if (count > 1)
            {
                issued_to = ("select EmpName from [pmis].[dbo].[m_vwGetAllEmployee_Minified] where eid = (select distinct(eid) FROM [IMS].[dbo].[transaction] where officeid  =  '" + USER.C_officeID + "' and controlno = '" + code + "')").ScalarString();
            }
            else 
            {
                issued_to = ("select EmpName from [pmis].[dbo].[m_vwGetAllEmployee_Minified] where eid = (select distinct(reid) FROM [IMS].[dbo].[transaction] where officeid  =  '" + USER.C_officeID + "' and controlno = '" + code + "')").ScalarString();
            }



            var usname = ("select EmpName from [pmis].[dbo].[m_vwGetAllEmployee_Minified] where eid = '"+USER.C_eID+"'").ScalarString();

             
            textBox5.Value = USER.C_Office;
            textBox10.Value = USER.C_Office;

            textBox23.Value = issued_to;
            textBox24.Value = usname;

            barcode1.Value = qr;
            textBox39.Value = "System Generated Report " + DateTime.Now;

            textBox34.Value = issued_to;
            textBox33.Value = usname;
            DataTable dt = (@"select item_id,itemname,unit,sum(quantity) as quantity from [IMS].[dbo].[transaction] where controlno = '" + code + "' and in_out = 'OUT' group by item_id,itemname,unit").DataSet();


            string dxxd = ("select   distinct(CONVERT(VARCHAR(20), cast(stock_date as date), 100))    from [IMS].[dbo].[transaction] where controlno = '"+code+"'").ScalarString();


            date.Value = dxxd ;
            date1.Value = dxxd ;

            objectDataSource1.DataSource = dt;
             
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
    }
}