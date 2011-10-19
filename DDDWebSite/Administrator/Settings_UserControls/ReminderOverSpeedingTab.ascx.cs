using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Administrator_Settings_UserControls_ReminderOverSpeedingTab : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public bool Enabled
    {
        get
        {
            return OverSpeedingPanel.Enabled;
        }
        set
        {
            OverSpeedingPanel.Enabled = value;
        }
    }

    public override bool Visible
    {
        get
        {
            return base.Visible;
        }
        set
        {
            base.Visible = value;
            Enabled = false;
        }
    }
}
