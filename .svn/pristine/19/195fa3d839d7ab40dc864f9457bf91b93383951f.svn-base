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
    /// Summary description for acknowlege_receipt.
    /// </summary>
    public partial class acknowlege_receipt : Telerik.Reporting.Report
    {
        public acknowlege_receipt(string transnumber)
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            transmittal_ws.serviceSoapClient ws = new transmittal_ws.serviceSoapClient();
            DataSet dt = ws.ViewTransmittalInfo(transnumber);


            textBox9.Value = transnumber.ToUpper();
             
            DataTable header = dt.Tables[0];
            DataTable table  = dt.Tables[1];


            var reid = "";
            try
            {
                textBox12.Value = header.Rows[0]["transactionno"].ToString();

                textBox13.Value = header.Rows[0]["datesent"].ToString();

                textBox16.Value = header.Rows[0]["datereceived"].ToString();

                textBox19.Value = header.Rows[0]["sender"].ToString();

                textBox20.Value = header.Rows[0]["receiver"].ToString();

                textBox10.Value = header.Rows[0]["purpose"].ToString();

                reid = header.Rows[0]["receivedby"].ToString();

            }
            catch (Exception ex)
            {

            }
            




            string data = transnumber;
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


            var name = (@"select emailaddress from [pmis].[dbo].[vwLoginParameter]  where eid = " + reid + " ").ScalarString();

            textBox25.Value = name.ToString();

  

            textBox29.Value = "https://pgas.ph/pdts/Reports/print_ris.aspx?repID=12&transnumber=" + transnumber;


            objectDataSource1.DataSource = table;

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
    }
}