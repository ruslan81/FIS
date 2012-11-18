<%@ Page Language="C#" AutoEventWireup="true" CodeFile="loginPage.aspx.cs" Inherits="loginPage"
    Title="Login" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="content-type" content="text/html" charset="utf-8" />
    <title>Form Enter</title>
    
    <script type="text/javascript">
        function pageLoad() {
            $('#dialog').dialog({
                autoOpen: false, draggable: false, resizable: false
            });
            $('#LinkButton1').click(function () {
                $('#enterButton').removeClass('enterbutton');
                $('#enterButton').addClass('disabledEnterbutton');
                $('#LinkButton1').addClass("disable");
                $('#errorBlock').fadeOut(200);
            });
            $('#PassRecoverOkButton').click(function () {
                $('#enterButton2').removeClass('enterbutton');
                $('#enterButton2').addClass('disabledEnterbutton');
                $('#PassRecoverOkButton').addClass("disable");
                $('#errorStatus').fadeOut(200);
            });
        };
    </script>
    
    <link rel="stylesheet" type="text/css" href="css/form-enter.css" />
    <link type="text/css" href="css/custom-theme/jquery-ui-1.8.11.custom.css" rel="stylesheet" />	
    <link type="text/css" href="css/custom-theme/ui.jqgrid.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery-1.5.1.min.js"></script><!--Для страниц в папках-->
    <script type="text/javascript" src="js/jquery-ui-1.8.11.custom.min.js"></script>
    
</head>
<body>
    <form id="form1" enctype="multipart/form-data" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" AsyncPostBackTimeout="9999999"
        EnableScriptGlobalization="true" EnableScriptLocalization="true" EnablePageMethods="true" />
    <div id="all">
        <div id="contain">
            <em class="bt"><b>&nbsp;</b></em>
            <asp:UpdatePanel ID="LoginUpdatePanel" runat="server">
                <ContentTemplate>
                    <div id="logo">
        	            <a href="http://smartfis.info">
                            <img alt="smartfis.info" title="Узнайте о нашем продукте больше!" src="images/EnterForm/logo.png"/>
                        </a>
                    </div>
                    <asp:Panel ID="enterFormPanel" runat="server">
                    <div id="form">
                        <div id="profiles">
                            <p>Профиль</p>
                            <asp:TextBox ID="ProfilesTextBox" runat="server" CssClass="searchBox" TabIndex="1"/>
                            &nbsp;&nbsp;
                        </div>
                        <div id="username">
                            <p>Пользователь</p>
                            <asp:TextBox ID="UserNameTextBox" runat="server" CssClass="searchBox" TabIndex="2"/>
                            &nbsp;&nbsp;
                            <asp:LinkButton ID="LinkButton2" Text="Регистрация" runat="server" CausesValidation="false"
                                href="http://smartfis.info/ru/contacts.html" target="_blank"/>
                        </div>
                        <div id="password">
                            <p>Пароль</p>
                            <asp:TextBox ID="PasswordTextBox" TextMode="Password" runat="server" CssClass="searchBox" TabIndex="3"/>
                            &nbsp;&nbsp;
                            <asp:LinkButton ID="forgetPassButton" Text="Забыли пароль?" runat="server" CausesValidation="false"
                                OnClientClick="$('#dialog').dialog({autoOpen: true, draggable: false, resizable: false, modal:true});"/>
                        </div>
                        <div id="rem"><asp:CheckBox ID="Persistent" CssClass="remember" runat="server"/><div class="remember-text">Запомнить меня</div>
                        </div>
                        <div id="enterButton" class="enterbutton" runat="server">
                            <asp:LinkButton ID="LinkButton1" Text="Войти" runat="server" CssClass="login"
                                OnClick="OnLogin_Click" CausesValidation="false"/>
                        </div>
                        
                    </div>
                    <div id="errorBlock" class="error-block" style="display:none;" runat="server">
                        <asp:Label ID="result" CssClass="error" runat="server" />
                    </div>
                    </asp:Panel>
                    <em class="bb"><b>&nbsp;</b></em>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
       
    </div>
    <div id="bottom">
        Version 
        <b>
        <%System.Reflection.Assembly web = System.Reflection.Assembly.Load("App_Code");
            System.Reflection.AssemblyName webName = web.GetName();
            string myVersion = webName.Version.ToString(); 
        %>
        <%=myVersion %>
        (<%=System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location).ToShortDateString()%>)
        </b>
        <p>&copy 2009-2012 FIS Все права защищены.</p>
    </div>

    <script type="text/javascript">
          function recoverTextBoxChanged() {
              var currentTextBoxVal = document.getElementById("<% =PassRecoverTextBox.ClientID %>").value;
              alert(currentTextBoxVal);
              document.getElementById("<% =RecoveryEmailHiddenField.ClientID %>").value = currentTextBoxVal;
          }
    </script>

        <asp:HiddenField runat="server" ID="RecoveryEmailHiddenField" />
    
    <div id="dialog" title="Восстановление пароля">
        <asp:Label runat="server" ID="PassRecoverLabel" Text="Введите адрес вашей электронной почты, на нее будет выслан ваш пароль:" />
        <br /><br />
        <asp:TextBox ID="PassRecoverTextBox" Width="270px" runat="server" onchange="recoverTextBoxChanged();"/>
        <br /><br />
        <asp:UpdatePanel ID="PassRecoverUpdatePanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Table ID="PassRecoverButtonsTable" runat="server" Width="100%" GridLines="None">
                    <asp:TableRow>
                        <asp:TableCell ID="enterButton2" CssClass="enterbutton" HorizontalAlign="Left" Width="50%">
                            <asp:LinkButton runat="server" ID="PassRecoverOkButton" text="Отправить" Width="80%" 
                                ForeColor="White" OnClick="PassRecoverButtonClick"/>
                        </asp:TableCell>
                        <asp:TableCell CssClass="enterbutton" HorizontalAlign="Right" Width="50%">
                            <asp:LinkButton runat="server" ID="PassRecoverCancelButton" text="Отмена" Width="80%" ForeColor="White"
                                CausesValidation="false" OnClientClick="$('#dialog').dialog('close');"/>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <br />
                <div id="errorStatus" class="error-block-status" style="display:none;" runat="server">
                    <asp:Label ID="PassRecoverStatus" runat="server" CssClass="error"/>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    
    </form>
</body>
</html>
