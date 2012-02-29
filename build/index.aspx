<%@ page language="C#" masterpagefile="~/MasterPage/MasterPage.master" autoeventwireup="true" inherits="index, App_Web_1ot4ot02" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content">
        <!-- !-->
        <div id="left">
            <h2>
                Сайт находится в разработке</h2>
            <asp:Button ID="showRep" OnClick="ShowCrystalReport" runat="server" />
        </div>
        <div id="right">
            <asp:Image ID="Image1" ImageUrl="~/images/icons/under_construction3.gif" runat="server"
                Width="190" />
            <asp:Label ID="Status" runat="server" />
        </div>
        <div id="center">
        </div>
    </div>
</asp:Content>
