using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Reporting;

namespace IMS.Reports
{
    public partial class reportplc : System.Web.UI.Page
    {
        InstanceReportSource rs = new InstanceReportSource();
        protected void Page_Load(object sender, EventArgs e)
        {
            int filterid = Convert.ToInt32(Request["repID"].ToString());
          
            switch (filterid)
            {
                case 1:
                    string in_out = Request["in_out"].ToString() ?? "";
                    int itemid = Convert.ToInt32(Request["itemid"].ToString());
                    string from = Request["from"].ToString();
                    string to = Request["to"].ToString();

                    rs.ReportDocument = new rptplc(from,to,in_out,itemid);
                    ReportViewer1.ReportSource = rs;
                    break;
  
            }
        }
    }
}