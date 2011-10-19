<%@ Application Language="C#"  Inherits="TestCacheTimeout.Global"%>
<%@ Import Namespace="System.Security.Principal" %>

<script runat="server">


    void Application_AuthenticateRequest(Object sender, EventArgs e)
    {        
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        HttpApplication app = (HttpApplication)sender;
        string rolе;
        BLL.UsersTables userCheck = new BLL.UsersTables(connectionString, "STRING_EN", new DB.SQL.SQLDB(connectionString));

        if (app.Request.IsAuthenticated && app.User.Identity is FormsIdentity)
        {
            FormsIdentity identity = (FormsIdentity)app.User.Identity;
            string tmp;
            userCheck.OpenConnection();
            rolе = userCheck.Get_UserRoleName(identity.Name);//определить роль пользователя, если она назначена
            userCheck.CloseConnection();
            tmp = rolе;
            if (rolе != null)//Создаем GenericPrincipal  с именем роли и назначаем текущему запросу
            {               
                string[] tempRoles = new string[1];
                tempRoles[0] = tmp;
                
                app.Context.User = new GenericPrincipal(identity, tempRoles);
            }
        }
    }
    
    
       
</script>
