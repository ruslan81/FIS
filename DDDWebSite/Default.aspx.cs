using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

/// <summary>
/// Эта страница используется для редиректа, соответвенно типу пользователя.
/// </summary>
public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string url = " ";
        url = "~/loginPage.aspx";
        if (User.IsInRole("Administrator"))
        {
            url = "~/Administrator/Data.aspx";
        }
        if (User.IsInRole("SuperAdministrator"))
        {
            url = "~/AdministratorS/CreatingForms.aspx";
        }
        Response.Redirect(url);
    }
}
