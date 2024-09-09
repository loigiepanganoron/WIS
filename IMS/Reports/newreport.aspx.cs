using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Reporting;

namespace IMS.Reports
{
    public partial class newreport : System.Web.UI.Page
    {
        InstanceReportSource rs = new InstanceReportSource();
        protected void Page_Load(object sender, EventArgs e)
        {
            int filterid = Convert.ToInt32(Request["repID"].ToString());
             
            switch (filterid)
            {
                case 1:
                    String status = Request["in_out"].ToString() ?? "";
                    int itemid = Convert.ToInt32(Request["itemid"].ToString());
                    DateTime start = Convert.ToDateTime(Request["start"].ToString());
                    DateTime end = Convert.ToDateTime(Request["end"].ToString());
                    int offid = Convert.ToInt32(Request["offid"].ToString());
                    String off = Request["off"].ToString();

                    rs.ReportDocument = new rptnew(status, itemid, start, end,offid,off);
                    RV.ReportSource = rs;
                    break;
                


                case 3:
                    rs.ReportDocument = new rptall();
                    RV.ReportSource = rs;
                    break;
            }
        }
    }
}