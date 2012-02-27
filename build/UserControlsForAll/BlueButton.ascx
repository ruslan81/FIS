<%@ control language="C#" autoeventwireup="true" inherits="UserControlsForAll_BlueButton, App_Web_exggujnm" %>

<asp:Panel ID="BlueButtonPanel" runat="server" CssClass="enterbutton">
    <link href="../css/BlueButtonCSS.css" rel="stylesheet" type="text/css" />
    <asp:LinkButton ID="blueButtonLink" OnClick="BlueButtonClick" Text="Кнопка по-умолчанию" runat="server" CausesValidation="true"/>
</asp:Panel>