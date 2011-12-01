<%@ control language="C#" autoeventwireup="true" inherits="Administrator_Reports_UserControls_NavigationReportControl, App_Web_2y2thjum" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Reference Control="~/MasterPage/MasterPage.master"%>

<%@ Register assembly="Stimulsoft.Report.Web, Version=2010.3.900.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a" namespace="Stimulsoft.Report.Web" tagprefix="cc1" %>


<asp:Panel ID="NavigationReportsPanel" runat="server" Visible="false">
    <asp:Table runat="server" ID="NavigationTable" Width="740px">
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Center">
                <asp:ImageButton ID="PrintButton" runat="server" ImageUrl="~/images/icons/StiWebReportIcons/print.png" Height="20px"/>
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Center">
                <asp:ImageButton ID="SaveButton" runat="server" ImageUrl="~/images/icons/StiWebReportIcons/save.png" Height="20px" />
            </asp:TableCell>
            <asp:TableCell  HorizontalAlign="Center">
                <asp:DropDownList ID="SavePrintFormatDropDownList" runat="server" Width="95px"/>
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Center">
                <asp:ImageButton ID="FirstPage" runat="server" OnClick="FirstButtonClick" ImageUrl="~/images/icons/StiWebReportIcons/doubleLeftArrow.png" Height="20px" />
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Center">
                <asp:ImageButton ID="PreviousPage" runat="server" OnClick="PrevButtonClick" ImageUrl="~/images/icons/StiWebReportIcons/leftArrow.png"  Height="20px"/>
            </asp:TableCell>
             <asp:TableCell HorizontalAlign="Right">
                <asp:Label ID="PageLabel" runat="server" Text="Страница"/> 
            </asp:TableCell><asp:TableCell HorizontalAlign="Center">
                <asp:FilteredTextBoxExtender ID="FilteredPageNumberTextBoxExtender" FilterType="Numbers" TargetControlID="PageNumberTextBox" runat="server"/>
                <asp:TextBox ID="PageNumberTextBox"  runat="server" OnTextChanged="PageTextChanged" MaxLength="3" Width="35px" style="text-align:right;"/>
            </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                <asp:Label ID="ofPagesLabel" Text="из 256" runat="server"/>
            </asp:TableCell><asp:TableCell HorizontalAlign="Center">
                <asp:ImageButton ID="NextPage" runat="server" OnClick="NextButtonClick" ImageUrl="~/images/icons/StiWebReportIcons/rightArrow.png"  Height="20px"/>
            </asp:TableCell><asp:TableCell HorizontalAlign="Center">
                <asp:ImageButton ID="LastPage" runat="server" OnClick="LastButtonClick" ImageUrl="~/images/icons/StiWebReportIcons/doubleRightArrow.png"  Height="20px"/>
            </asp:TableCell><asp:TableCell HorizontalAlign="Center">
                <asp:ImageButton ID="ZoomOut" runat="server" OnClick="ZoomOutClick" ImageUrl="~/images/icons/StiWebReportIcons/Zoom Out_24x24.png"  Height="20px"/>
            </asp:TableCell><asp:TableCell HorizontalAlign="Center">
                <asp:FilteredTextBoxExtender ID="ZoomPercentsFilteredTextBoxExtender" FilterType="Numbers" TargetControlID="ZoomPercentsTextBox" runat="server"/>
                <asp:TextBox ID="ZoomPercentsTextBox" runat="server" AutoPostBack="true" OnTextChanged="ZoomTextChanged" MaxLength="3"  Width="23px" />
                %
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Center">
                <asp:ImageButton ID="ZoomIn" runat="server" OnClick="ZoomInClick" ImageUrl="~/images/icons/StiWebReportIcons/Zoom In_24x24.png"  Height="20px"/>
            </asp:TableCell>
            <asp:TableCell>
                <asp:DropDownList ID="ViewModeDropDownList" OnSelectedIndexChanged="ViewModeChanged" style="text-align:left;" runat="server" Width="120px" AutoPostBack="true"/>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Panel>
            
<cc1:StiWebViewer ID="StiWebViewer1" RenderMode="Standard" runat="server" Visible="true" ShowToolBar="false" Width="100%" />
                    
