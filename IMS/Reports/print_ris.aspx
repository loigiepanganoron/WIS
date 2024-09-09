<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="print_ris.aspx.cs" Inherits="IMS.Reports.print_ris" %>
<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=9.0.15.324, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
     <div>
      <telerik:ReportViewer ID="RV" runat="server" ShowExportGroup="true" Style="width: auto; min-height: 2000px; height: auto; margin: auto"></telerik:ReportViewer>
    </div>
    </form>

    <script type="text/javascript">

        RV.prototype.PrintReport = function ()
            { 
                switch (this.defaultPrintFormat)
                {
                    case "Default": 
                        this.DefaultPrint(); 
                        break;

                    case "PDF": 
                        this.PrintAs("PDF"); 
                        previewFrame = document.getElementById(this.previewFrameID); 
                        previewFrame.onload = function () { previewFrame.contentDocument.execCommand("print", true, null); } 
                         break; 
                } 
            };

     </script>

</body>
</html>
