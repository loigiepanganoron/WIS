
using IMS.@class;
using IMS.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Reporting;
using IMS.Classess;

namespace IMS.Reports
{
    public partial class print_ris : System.Web.UI.Page
    {
        InstanceReportSource rs = new InstanceReportSource();
        ReportBook rb = new ReportBook();

        protected void Page_Load(object sender, EventArgs e)
        {
            int filterid = Convert.ToInt32(Request["repID"].ToString());
           
            switch (filterid)
            {
                case 1:
                    String transcode = Request["transcode"].ToString();
                    int officeid = Convert.ToInt32(Request["officeid"].ToString());
                    String officename = Request["officename"].ToString();
                    rs.ReportDocument = new ris_report(transcode, officeid,officename);
                    RV.ReportSource = rs;
                    break;
                case 2:
                    int officeid1 = Convert.ToInt32(Request["officeid"].ToString());
                    String officename1 = Request["officename"].ToString();
                    rs.ReportDocument = new Borrow_ris(officeid1,officename1);
                    RV.ReportSource = rs;
                    break;
                case 3:
                    String controlno = Request["controlno"].ToString();
                    //rs.ReportDocument = new ris_revise_report(controlno);
                    rs.ReportDocument = new new_ris_report(controlno);
                    RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                    RV.ReportSource = rs;
                    break;
                case 4:
                    string date =  Request["date"].ToString();
                    rs.ReportDocument = new statement_of_ending_inventory_new(date);
                    RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                    RV.ReportSource = rs;
                    break;
                case 5:
                    string tdate = Request["date"].ToString();
                    rs.ReportDocument = new statement_of_ending_inventory(tdate);
                    RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                    RV.ReportSource = rs;
                    break;
               case 6: 
                    string date_from =  Request["date_from"].ToString();
                    string date_to = Request["date_to"].ToString();
                    string iswithdate = "1";
                    int? util_offficeid = null;
                    string util_officename = null; 
                    try 
                    {
                        util_offficeid = Convert.ToInt32(Request["officeid"].ToString());
                        util_officename = Request["officename"].ToString();
                    }
                    catch(Exception)
                    {

                    }

                    if(iswithdate == "1")
                    {
                        rs.ReportDocument = new office_util_with_date(date_from, date_to, util_offficeid, util_officename); 
                    }
                    else
                    {
                        rs.ReportDocument = new office_util(date_from, date_to, util_offficeid, util_officename); 
                    }


                    RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                    RV.ReportSource = rs;
                    break;

                case 7:
                    string code = Request["code"].ToString();

                    var qr1 = (@" DECLARE @OutputTbl TABLE (qrcode nvarchar(100)) insert into [IMS].[dbo].[report_log] OUTPUT INSERTED.qrcode INTO @OutputTbl(qrcode)  values ('" + DateTime.Now.ToString("yyyyMMddHHmmssf") + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.f") + "','" + USER.C_Name + "','individual_ris')  select top 1 qrcode from  @OutputTbl ").ScalarString();

                    rs.ReportDocument = new office_issuance_slip(code,qr1);
                    RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                    RV.ReportSource = rs;
                    break;
                case 8: 
                    string running_date = Request["running_date"].ToString();
                    rs.ReportDocument = new OfficeItemsRemaining(running_date);
                    RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                    RV.ReportSource = rs;
                    break;
                case 9:
                    string monthly = Request["monthly"].ToString();

                    var qr = (@" DECLARE @OutputTbl TABLE (qrcode nvarchar(100))   
                                 insert into [IMS].[dbo].[report_log] 
                                OUTPUT INSERTED.qrcode INTO @OutputTbl(qrcode)  
                                values ('" + DateTime.Now.ToString("yyyyMMddHHmmssf") + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.f") + "','" + USER.C_Name + "','monthly_ris')  select top 1 qrcode from  @OutputTbl ").ScalarString();
                            
                     
                            DataTable eid = new DataTable();
                            using (SqlConnection con = new SqlConnection(common.MyConnection()))
                            {
                                DateTime yearmonth = Convert.ToDateTime(monthly);
                                int month = yearmonth.Month;
                                int year = yearmonth.Year;
                                 
                                SqlCommand com = new SqlCommand(@"select distinct reid FROM [IMS].[dbo].[transaction] where officeid = '"+USER.C_officeID+"' and in_out = 'OUT' and year(stock_date) = '"+year+"' and MONTH(stock_date) = '"+month+"' ", con);
                                con.Open();
                                eid.Load(com.ExecuteReader());
                            }
                     
                            for (int i = 0; i < eid.Rows.Count ; i++)
                            {
                                var risreport = new office_issuance_slip_monthly(monthly,Convert.ToInt32(eid.Rows[i][0]),qr);
                                rb.Reports.Add(risreport);
                            }

                            rs.ReportDocument = rb;
                            RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                            RV.ReportSource = rb;

                           // var risreport = new LBP3New(Convert.ToInt32(OfficeIDList.Rows[i][0]), year);
                            break;
                case 10:
                     int vale_office_id = Convert.ToInt32(Request["officeid"].ToString());
                     string vale_from = Request["from"].ToString();
                     string vale_to = Request["to"].ToString();
                     string vale_offname = Request["offname"].ToString();
                     string empid = Request["empid"].ToString();
                     rs.ReportDocument = new vale_report(vale_office_id,vale_from,vale_to,vale_offname,empid);
                    RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                    RV.ReportSource = rs;
                    break;
                case 11: 
                    rs.ReportDocument = new pimo();
                    RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                    RV.ReportSource = rs;
                    break;
                case 12:
                    string transnumber = Request["transnumber"].ToString();

                    if (transnumber.ToUpper().Contains("TR"))
                    {
                        rs.ReportDocument = new acknowlege_receipt(transnumber);
                    }
                    else
                    { 
                        rs.ReportDocument = new transmittal_qr(transnumber);
                    }
                      
                    RV.ViewMode = Telerik.ReportViewer.WebForms.ViewMode.PrintPreview;
                    RV.ReportSource = rs;
                    break;
                     
            }
        }
    }
}