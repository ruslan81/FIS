using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Configuration;
using BLL;
using System.Net.Mail;
using DB.SQL;
using System.Net;

public partial class loginPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Так как не работает в Файрфокс валидация панели по умолчанию, приходится отслеживать нажатие кнопки энтер.
            UserNameTextBox.Attributes.Add("onKeyPress", "javascript:if (event.keyCode == 13) __doPostBack('" + LinkButton1.ClientID + "','')");
            PasswordTextBox.Attributes.Add("onKeyPress", "javascript:if (event.keyCode == 13) __doPostBack('" + LinkButton1.ClientID + "','')");

            ProfilesTextBox.Focus();
            LastUpdate.Text = "Последнее обновление 03.03.2012 00:00";
        }
        PassRecoverStatus.Text = "";
    }
    /// <summary>
    /// Нажатии на кнопку вход
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void OnLogin_Click(object sender, EventArgs e)
    {        
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            dataBlock.OpenConnection();
            int orgId = dataBlock.organizationTable.GetOrgId_byOrgName(ProfilesTextBox.Text);
            if (dataBlock.usersTable.CustomAuthenticate(UserNameTextBox.Text, PasswordTextBox.Text, orgId))
            {
                string url = FormsAuthentication.GetRedirectUrl(UserNameTextBox.Text, Persistent.Checked);
                //Кнопка запомнить меня создает куки на 24 часа.
                if (Persistent.Checked)
                {
                    HttpCookie cookie = Response.Cookies[FormsAuthentication.FormsCookieName];
                    cookie.Expires = DateTime.Now.AddHours(24);//куки живут сутки
                    FormsAuthentication.SetAuthCookie(UserNameTextBox.Text, Persistent.Checked);//Раньше эта строка была перед ИФ
                }
                else
                {
                    HttpCookie cookie = Response.Cookies[FormsAuthentication.FormsCookieName];
                    cookie.Expires = DateTime.Now.AddHours(2);//куки живут два часа
                    FormsAuthentication.SetAuthCookie(UserNameTextBox.Text, Persistent.Checked);//Раньше эта строка была перед ИФ
                }
                int userId = dataBlock.usersTable.Get_UserID_byName(Page.User.Identity.Name);
                dataBlock.usersTable.Set_TimeConnect(userId);
                Response.Redirect(url);
            }
            else
            {
                errorBlock.Style.Add("display","block");
                result.Text = "Введите корректные логин и пароль!";
                throw new Exception("Введите корректные логин и пароль!");
            }
        }
        catch (Exception ex)
        {
            errorBlock.Style.Add("display", "block");
            result.Text = "Ошибка: " + ex.Message;
        }
        finally
        {
            dataBlock.CloseConnection();
        }
    }
    /// <summary>
    /// переадресует на страницу с описанием изменений на сайте
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ShowChangesClick(object sender, EventArgs e)
    {
       // Response.Write("<script>window.open('WhatsNew.aspx', 'WhatsNewWindow', 'width=400; height=300')</script>");
        Response.Redirect("WhatsNew.aspx");
    }
    /// <summary>
    /// нажатие на кнопку восстановления пароля
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void PassRecoverButtonClick(object sender, EventArgs e)
    {
        Exception noPassword = new Exception("Такой e-mail не найден");
        string language = "STRING_EN";
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        BLL.DataBlock dataBlock = new BLL.DataBlock(connectionString, language);

        try
        {
            dataBlock.OpenConnection();
            string mailTO = RecoveryEmailHiddenField.Value;
            string userPassword = dataBlock.usersTable.Get_UserPassword(mailTO);
            if (userPassword == "")
                throw noPassword;

            SQLDB sqlDb = new SQLDB(connectionString);
            sqlDb.OpenConnection();
            int stringId = sqlDb.GetStringId(mailTO);
            int userId = sqlDb.GetUserInfoUserId(stringId);
            string name=sqlDb.GetUserName(userId);
            /*int orgId=sqlDb.GetUserOrgId(userId);
            int orgNameId = sqlDb.GetOrgNameId(orgId);
            string orgName=sqlDb.GetString(orgNameId, language);            */

            string mailSubject = "Напоминание пароля SmartFis.ru";
            string mailBody = name + ",\n\nВы запросили напоминание Вашего пароля на сайте SmartFis.ru.\n" +
                "Если Вы этого не делали, проигнорируйте это письмо.\n\nВаш пароль: " + userPassword+"\n\n"+
                "Желаем удачи!\n\nС уважением,\nАдминистрация SmartFis.ru.";
            //sending mail
            MailMessage Message = new MailMessage();
            Message.Subject = mailSubject;
            Message.Body = mailBody;
            Message.To.Add(new MailAddress(mailTO));
            Message.From = new MailAddress(ConfigurationManager.AppSettings["DefaultEmailAddress"]);

            SmtpClient smtpClient = new SmtpClient();
            NetworkCredential basicCredential =
                new NetworkCredential("u274550", "67cd6ab5");
            MailMessage message = new MailMessage();

            smtpClient.Host = "smtp-19.1gb.ru";
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = basicCredential;

            smtpClient.Send(Message);

            sqlDb.CloseConnection();
            //
            errorStatus.Style.Add("display", "block");
            PassRecoverStatus.Text = "Пароль был выслан на указанный адрес";
        }
        catch (Exception ex)
        {
            errorStatus.Style.Add("display", "block");
            PassRecoverStatus.Text = ex.Message;
        }
        finally
        {
            dataBlock.CloseConnection();
            PassRecoverUpdatePanel.Update();
        }
    }

}
