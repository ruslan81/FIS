<%@ control language="C#" autoeventwireup="true" inherits="Administrator_GeneralTab, App_Web_hjo1fni2" %>

<asp:Panel ID="GeneralTablePanel" runat="server">
                <asp:Table ID="GeneralTable" runat="server" Width="75%" CellPadding="10">
                    <asp:TableRow >
                        <asp:TableCell HorizontalAlign="Right" Width="35%">
                            <asp:Label ID="GeneralFullName" runat="server" Text="Полное название предприятия"/>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="User_General_FullNameTextBox" runat="server" Width="80%" Enabled="false"/>
                        </asp:TableCell></asp:TableRow><asp:TableRow>
                        <asp:TableCell HorizontalAlign="Right" Width="35%">
                            <asp:Label ID="GeneralCountryName" runat="server" Text="Страна"/>
                        </asp:TableCell><asp:TableCell>
                            <asp:TextBox ID="User_General_CountryTextBox" runat="server" Width="80%" Enabled="false"/>
                        </asp:TableCell></asp:TableRow>
                        <asp:TableRow>
                        <asp:TableCell HorizontalAlign="Right" Width="35%">
                            <asp:Label ID="GeneralRegionName" runat="server" Text="Регион"/>
                        </asp:TableCell><asp:TableCell>
                            <asp:TextBox ID="User_General_RegionTextBox" runat="server" Width="80%" Enabled="false"/>
                        </asp:TableCell></asp:TableRow>
                        </asp:Table>
                    <div style="width:75%;">	
                    <div style="float:right;">
                        <asp:Panel runat="server" ID="GeneralLogoPanel" GroupingText="Логотип" >
                            <asp:Image ID="Image1" ImageUrl="~/images/LogoSmartFIS_medium.png" runat="server" Width="200px"/>
                            <br />
                            <asp:Button ID="Button6" runat="server" Text="Открыть"/> 
                        </asp:Panel>                        
                    </div>
                    </div>
</asp:Panel>