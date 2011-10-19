using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Data;
using BLL;
using System.Configuration;
using System.Threading;

public partial class Administrator_ReportServiceTest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["WhatIsThat"] = null;
        }
    }

    protected void somethingtodo(object sender, EventArgs e)
    {
        string connectionString = System.Configuration.ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DB.SQL.SQLDB sqlDb;
        sqlDb = new DB.SQL.SQLDB(connectionString);

    }

    protected void PostBack(object sender, EventArgs e)
    {
    }

    protected void GoBack(object sender, EventArgs e)
    {
        Response.Redirect("Data.aspx");
    }
}
