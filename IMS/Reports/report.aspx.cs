using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Reporting;

namespace IMS.Reports
{
    public partial class report : System.Web.UI.Page
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

                    rs.ReportDocument = new rptTransaction(status, itemid, start, end);
                    RV.ReportSource = rs;
                    break;
                case 2:
                    String status2 = Request["in_out"].ToString() ?? "";
                    int itemid2 = Convert.ToInt32(Request["itemid"].ToString());
                    DateTime start2 = Convert.ToDateTime(Request["start"].ToString());
                    DateTime end2 = Convert.ToDateTime(Request["end"].ToString());

                    int eid = Convert.ToInt32(Request["eid"].ToString());
                    rs.ReportDocument = new rptemployee(status2, eid, itemid2, start2, end2);
                    RV.ReportSource = rs;
                    break;
                case 3:
                     rs.ReportDocument = new Users();
                    RV.ReportSource = rs;
                    break;
            }
        }
    }
}