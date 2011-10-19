using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Кнопка синего цвета. Представляет собой ЛинкБаттон с задниками на ЦСС, при изменении Enabled меняется класс CSS, и она блокируется и меняет цвет.
/// </summary>
public partial class UserControlsForAll_BlueButton : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            enabled = true;
            //enabledOnCLientClick = "return false;";
        }
    }

    private bool enabled;

    public bool Enabled
    {
        get { return enabled; }
        set 
        {
            enabled = value;
            blueButtonLink.Enabled = value;
            BlueButtonPanel.Enabled = value;
            if (value)
            {
                BlueButtonPanel.CssClass = "enterbutton";
                OnClientClick = enabledOnCLientClick;
            }
            else
            {
                BlueButtonPanel.CssClass = "disabledEnterbutton";
                OnClientClick = "return false;";
            }
        }
    }
    public string OnClientClick
    {
        get
        { return blueButtonLink.OnClientClick; }
        set
        { 
            blueButtonLink.OnClientClick = value;
            if(value!="return false;")
                enabledOnCLientClick = value;
            if (blueButtonLink.Enabled == false && value!="return false;")
            {
                blueButtonLink.OnClientClick = "return false;";
            }
        }
    }
    private string enabledOnCLientClick { get; set; }
    public string Text
    {
        get { return blueButtonLink.Text; }
        set { blueButtonLink.Text = value; }
    }
    public double BtnWidth
    {
        get { return BlueButtonPanel.Width.Value; }
        set { BlueButtonPanel.Width = new Unit(value, UnitType.Pixel); }
    }
    public bool CausesValidation
    {
        get { return blueButtonLink.CausesValidation; }
        set { blueButtonLink.CausesValidation = value; }
    }

    public event EventHandler ButtOnClick;
    protected void BlueButtonClick(object s, EventArgs e)
    {
        try
        {
            ButtOnClick(s, e);
        }
        catch(Exception ex)
        {
        }
    }
}
