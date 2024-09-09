namespace IMS.Reports
{
    using Classess;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using ZXing;

    /// <summary>
    /// Summary description for transmittal_qr.
    /// </summary>
    public partial class transmittal_qr : Telerik.Reporting.Report
    {
        public transmittal_qr(string transnumber)
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
             
            transmittal_ws.serviceSoapClient ws = new transmittal_ws.serviceSoapClient();

            string check = ws.CheckSent(transnumber);
            if (check.Equals("0"))
            {
                   
            }


            DataSet dt = ws.ViewTransmittalInfo(transnumber);
             
            DataTable header = dt.Tables[0];
            DataTable table = dt.Tables[1];


            var code = "";
            var eid = "";
            var reid = "";
           
            try
            {
                textBox9.Value = transnumber.ToUpper();//header.Rows[0]["transid"].ToString();
                textBox26.Value = header.Rows[0]["datesent"].ToString();
                textBox19.Value = header.Rows[0]["sender"].ToString();
                textBox20.Value = header.Rows[0]["receiver"].ToString();

                textBox10.Value = header.Rows[0]["purpose"].ToString();
               
                code = header.Rows[0]["transactionno"].ToString();
                eid = header.Rows[0]["eid"].ToString();
                 
            }
            catch (Exception ex)
            {

            }

            string data = code;
            var barcodeWriter = new ZXing.BarcodeWriter();
            barcodeWriter.Format = BarcodeFormat.QR_CODE;
            barcodeWriter.Options = new ZXing.Common.EncodingOptions
            {
                Width = 500,
                Height = 500
            };
            var result = barcodeWriter.Write(data);
            var barcodeBitmap = new Bitmap(result);


            qr.Value = barcodeBitmap; 
            textBox24.Value = DateTime.Now.ToString("MMM dd, yyyy hh:mm tt"); 
            objectDataSource1.DataSource = table;
             

            var name = (@"select emailaddress from [pmis].[dbo].[vwLoginParameter]  where eid = "+eid+" ").ScalarString();

            textBox25.Value = name.ToString();


            if (reid.Length > 0)
            {
              //  textBox31.Value = (@"select emailaddress from [pmis].[dbo].[vwLoginParameter]  where eid = " + reid + " ").ScalarString();
            }
            textBox29.Value = "https://pgas.ph/pdts/Reports/print_ris.aspx?repID=12&transnumber="+ transnumber;

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
    }
}