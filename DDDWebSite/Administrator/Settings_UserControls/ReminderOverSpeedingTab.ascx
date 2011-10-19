<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ReminderOverSpeedingTab.ascx.cs" Inherits="Administrator_Settings_UserControls_ReminderOverSpeedingTab" %>

<asp:Panel ID="OverSpeedingPanel" runat="server">
    <asp:Table ID="AditionalEditTable" runat="server" Width="100%" CellPadding="10">
        <asp:TableRow>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell HorizontalAlign="Left">
                <asp:Label ID="Var1Label" runat="server" Text="Вариант 1"/>
            </asp:TableCell>
             <asp:TableCell HorizontalAlign="Left">
                <asp:Label ID="Varц2abel" runat="server" Text="Вариант 2"/>
            </asp:TableCell>
             <asp:TableCell HorizontalAlign="Left">
                <asp:Label ID="Var3Label" runat="server" Text="Вариант 3"/>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right">
                <asp:Label ID="Var0_KomuLabel" runat="server" Text="Как часто"/>
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Left">
                <asp:DropDownList ID="KomuVar1DropDown" runat="server" Width="100%"/>
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Left">
                <asp:DropDownList ID="KomuVar2DropDown" runat="server" Width="100%"/>
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Left">
                <asp:DropDownList ID="KomuVar3DropDown" runat="server" Width="100%"/>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right">
                <asp:Label ID="Var0_KakChastoLabel" runat="server" Text="Кому"/>
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Left">
                <asp:DropDownList ID="KakChastoVar1DropDown" runat="server" Width="100%"/>
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Left">
                <asp:DropDownList ID="KakChastoVar2DropDown" runat="server" Width="100%"/>
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Left">
                <asp:DropDownList ID="KakChastoVar3DropDown" runat="server" Width="100%"/>
            </asp:TableCell>
        </asp:TableRow>
         <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right">
                <asp:Label ID="Var0_InterfaceLabel" runat="server" Text="Интерфейс"/>
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Left">
                <asp:DropDownList ID="InterfaceVar1DropDown" runat="server" Width="100%"/>
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Left">
                <asp:DropDownList ID="InterfaceVar2DropDown" runat="server" Width="100%"/>
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Left">
                <asp:DropDownList ID="InterfaceVar3DropDown" runat="server" Width="100%"/>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Panel>