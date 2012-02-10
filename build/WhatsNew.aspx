<%@ page language="C#" autoeventwireup="true" inherits="WhatsNew, App_Web_l2rfijt4" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript"> 
        function fixform() { 
            if (opener.document.getElementById("aspnetForm").target != "_blank") return; 
            opener.document.getElementById("aspnetForm").target = ""; 
            opener.document.getElementById("aspnetForm").action = opener.location.href; 
            } 
    </script> 
</head>
<body onload="fixform()">
    <form id="form123" runat="server">
    <div>
     <div style="float:left; width:100%; padding: 5px;">
	            <asp:Table ID="Table1" runat="server" Width="99%">
	                <asp:TableRow>
	                    <asp:TableCell HorizontalAlign="Left">
	                        <asp:TextBox TextMode="MultiLine" Width="100%" Height="600px" runat="server" id="ReportLabel" ForeColor="Black" />
	                    </asp:TableCell>
	                    </asp:TableRow>
	            </asp:Table>
	        </div>
    </div>
    </form>
</body>
</html>
