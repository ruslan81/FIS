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
    public string ErrorMessage = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        ErrorMessage = null;

        if (Request.HttpMethod == "POST")
        {
            string profile = Request.Form.Get("profile");
            string username = Request.Form.Get("username");
            string password = Request.Form.Get("password");
            bool persistent =false;
            if (Request.Form.Get("persistent")!=null)
            {
                if (Request.Form.Get("persistent") == "on")
                {
                    persistent = true;
                }
            }
            if (profile != null && username != null && persistent != null)
            {
                Login(profile,username, password, persistent);
            }
        }
    }

    /// <summary>
    /// Login function
    /// </summary>
    /// <param name="profile"></param>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="persistent"></param>
    private void Login(string profile, string username, string password, bool persistent)
    {        
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
        try
        {
            dataBlock.OpenConnection();
            int orgId = dataBlock.organizationTable.GetOrgId_byOrgName(profile);
            if (dataBlock.usersTable.CustomAuthenticate(username, password, orgId))
            {
                string url = FormsAuthentication.GetRedirectUrl(username, persistent);
                //Кнопка запомнить меня создает куки на 24 часа.
                if (persistent)
                {
                    HttpCookie cookie = Response.Cookies[FormsAuthentication.FormsCookieName];
                    cookie.Expires = DateTime.Now.AddHours(24);//куки живут сутки
                    FormsAuthentication.SetAuthCookie(username, persistent);
                }
                else
                {
                    HttpCookie cookie = Response.Cookies[FormsAuthentication.FormsCookieName];
                    cookie.Expires = DateTime.Now.AddHours(2);//куки живут два часа
                    FormsAuthentication.SetAuthCookie(username, persistent);
                }
                int userId = dataBlock.usersTable.Get_UserID_byName(username);
                dataBlock.usersTable.Set_TimeConnect(userId);
                Response.Redirect(url);
            }
            else
            {
                errorBlock.Style.Add("display","block");
                string errorMessage = "Неверные логин и/или пароль";
                throw new Exception(errorMessage);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = "Ошибка: " + ex.Message;
        }
        finally
        {
            dataBlock.CloseConnection();
        }
    }
    
    /// <summary>
    /// Восстановление пароля
    /// </summary>
    /// <param name="email">mail</param>
    [System.Web.Services.WebMethod]
    public static String RecoverPassword(string email)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        BLL.DataBlock dataBlock = new BLL.DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);

        try
        {
            //check mail
            dataBlock.OpenConnection();
            string mailTO = email;
            string userPassword = dataBlock.usersTable.Get_UserPassword(mailTO);
            if (userPassword == "")
                throw new Exception("Такой e-mail не найден");

            //get user information
            SQLDB sqlDb = new SQLDB(connectionString);
            sqlDb.OpenConnection();
            int stringId = sqlDb.GetStringId(mailTO, SQLDB.userString);
            int userId = sqlDb.GetUserInfoUserId(stringId);
            string name=sqlDb.GetUserName(userId);
            
            //prepare mail
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

            //show message
            return "Пароль был выслан на указанный адрес";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        finally
        {
            dataBlock.CloseConnection();
        }
    }

}
