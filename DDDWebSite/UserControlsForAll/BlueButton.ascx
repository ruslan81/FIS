﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BlueButton.ascx.cs" Inherits="UserControlsForAll_BlueButton" %>

<asp:Panel ID="BlueButtonPanel" runat="server" CssClass="enterbutton">
    <link href="../css/BlueButtonCSS.css" rel="stylesheet" type="text/css" />
    <asp:LinkButton ID="blueButtonLink" OnClick="BlueButtonClick" Text="Кнопка по-умолчанию" runat="server" CausesValidation="true"/>
</asp:Panel>