    <%@ Page Language="C#" AutoEventWireup="true" CodeFile="loginPage.aspx.cs" Inherits="loginPage"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="content-type" content="text/html" charset="utf-8" />
    <title>Авторизация</title>
    
    <script type="text/javascript" src="js/jquery-1.5.1.min.js"></script>
    <script type="text/javascript" src="js/jquery-ui-1.8.11.custom.min.js"></script>

    <link href="css/icons/favicon.ico" rel="icon" type="image/x-icon"/>
    <link type="text/css" href="css/form-enter.css" rel="stylesheet"/>
    <link type="text/css" href="css/custom-theme/jquery-ui-1.8.11.custom.css" rel="stylesheet" />

    <script type="text/javascript">

        var _gaq = _gaq || [];
        _gaq.push(['_setAccount', 'UA-37263349-1']);
        _gaq.push(['_trackPageview']);

        (function () {
            var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
            ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
            var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
        })();

    </script>

    <script type="text/javascript">
        $(function () {
            $("[name='profile']").focus();

            $(".login").button();

            $("#send-email").button();
            $("#send-email").click(function () {
                $("#errorStatus").hide();
                $("#errorStatus .error").empty();
                $.ajax({
                    type: "POST",
                    //Page Name (in which the method should be called) and method name
                    url: "loginPage.aspx/RecoverPassword",
                    data: "{'email':'" + $("#recover-email") + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        $("#errorStatus .error").text(response.d);
                        $("#errorStatus").show();
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        showErrorMessage("SmartFIS - Внимание!", jqXHR, errorThrown);
                    }
                });
            });
            $("#cancel").button();
            $("#cancel").click(function () {
                $('#dialog').dialog("close");
            });

            $(".recover-password").click(function () {
                $("#errorStatus").hide();
                $("#errorStatus .error").empty();
                $('#dialog').dialog({ autoOpen: true, draggable: false, resizable: false, modal: true });
            });
        });
    </script>
    
</head>
<body runat="server">
    
    <div id="all">
        <div id="contain">
            <div class="bt"><b>&nbsp;</b></div>
                <div>
                <div id="logo">
        	        <a href="http://smartfis.info">
                        <img alt="smartfis.info" title="Узнайте о нашем продукте больше!" src="images/EnterForm/logo.png"/>
                    </a>
                </div>
                <form id="form" action="loginPage.aspx" method="post">
                    <div>
                        <div id="profiles">
                            <p>Профиль</p>
                            <input name="profile" class="searchBox" tabindex="1"/>
                            &nbsp;
                            &nbsp;
                        </div>
                        <div id="username">
                            <p>Пользователь</p>
                            <input name="username" class="searchBox" tabindex="2"/>
                            &nbsp;
                            &nbsp;
                            <a href="http://smartfis.info/ru/contacts.html" target="_blank">Регистрация</a>
                        </div>
                        <div id="password">
                            <p>Пароль</p>
                            <input type="password" name="password" class="searchBox" tabindex="3"/>
                            &nbsp;
                            &nbsp;
                            <span class="recover-password">Забыли пароль?</span>
                        </div>
                        <div id="rem">
                            <input type="checkbox" name="persistent" class="remember"/><div class="remember-text">Запомнить меня</div>
                        </div>
                        <div id="enterButton" class="enterbutton" runat="server">
                            <input type="submit" value="Войти" class="login" tabindex="4"/>
                        </div>
                        
                    </div>
                </form>
                <%if (ErrorMessage != null)
                    { %>
                    <div id="errorBlock" class="error-block" style="display:none;" runat="server">
                        <div class="error">
                            <%=ErrorMessage %>
                        </div>
                    </div>
                <%} %>
                </div>
            <div class="bb"><b>&nbsp;</b></div>
        </div>
    </div>

    <div id="bottom">
        <form runat="server">
        Version 
        <b>
        <%System.Reflection.Assembly web = System.Reflection.Assembly.Load("App_Code");
            System.Reflection.AssemblyName webName = web.GetName();
            string myVersion = webName.Version.ToString(); 
        %>
        <%=myVersion %>
        (<%=System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location).ToShortDateString()%>)
        </b>
        <p>&copy 2009-2013 FIS Все права защищены.</p>
        </form>
    </div>

    <div id="dialog" title="Восстановление пароля" style="display:none;">
        Введите адрес вашей электронной почты, на нее будет выслан ваш пароль:
        <br /><br />
        <input id="recover-email" style="width:270px;"/>
        <br /><br />
        <button id="cancel" style="float:right">Отмена</button>
        <button id="send-email" style="float:right">Отправить</button>
        <br />
        <br />
        <div id="errorStatus" class="error-block-status" style="margin-top:5px;width:272px;display:none;">
            <div class="error">error
            </div>
        </div>
    </div>
    
</body>
</html>
