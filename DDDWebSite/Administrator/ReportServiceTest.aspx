<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReportServiceTest.aspx.cs"
    Inherits="Administrator_ReportServiceTest" %>

<%@ Register Assembly="Stimulsoft.Report.Web, Version=2010.3.900.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a"
    Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControlsForAll/progressBar.ascx" TagName="progressBar" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript" language="javascript" src="../anychart_files/js/AnyChart.js"></script>

</head>
<body>
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" AsyncPostBackTimeout="360000"
        EnableScriptGlobalization="true" EnableScriptLocalization="true" />
    <div id="content">
        <asp:Button ID="BackButton" runat="server" OnClick="GoBack" Text="Go Back" />
        <asp:Button ID="someBtn2" runat="server" OnClick="PostBack" Text="ПостБек!" />
        <asp:Button ID="someBtn" runat="server" OnClick="somethingtodo" Text="Сделать это!" />
        <asp:Label ID="Status" runat="server" Font-Size="XX-Large" Font-Bold="true" ForeColor="Blue" />
    </div>
    <!-- <uc1:progressBar ID="progressBar1" runat="server"/>-->
    </form>
</body>
</html>
