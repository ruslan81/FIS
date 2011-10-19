using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControlsForAll_progressBar : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            setToNull();
        }
    }

    public void setToNull()
    {
        ProgressTableCell_1.BackColor = System.Drawing.Color.Transparent;
        ProgressTableCell_2.BackColor = System.Drawing.Color.Transparent;
        ProgressTableCell_3.BackColor = System.Drawing.Color.Transparent;
        ProgressTableCell_4.BackColor = System.Drawing.Color.Transparent;
        ProgressTableCell_5.BackColor = System.Drawing.Color.Transparent;
        ProgressTableCell_6.BackColor = System.Drawing.Color.Transparent;
        ProgressTableCell_7.BackColor = System.Drawing.Color.Transparent;
        ProgressTableCell_8.BackColor = System.Drawing.Color.Transparent;
        ProgressTableCell_9.BackColor = System.Drawing.Color.Transparent;
        ProgressTableCell_10.BackColor = System.Drawing.Color.Transparent;
        ProgressTableCell_11.BackColor = System.Drawing.Color.Transparent;
    }

    public void LoadData(int current, int max)
    {
        Session["current"] = current.ToString();
        Session["max"] = max.ToString();
    }

    public void UpdateAsyncData(BLL.DataBlock dataBlock)
    {
        Session["current"] = dataBlock.plfUnitInfo.GetedPlfRecords.ToString();
        Session["max"] = dataBlock.plfUnitInfo.maxPlfRecords.ToString();
    }

    public void ReadyDone()
    {
        ProgressLabel.Text = 100 + "%";
        TextResultLabel.Text = "Выполнено!";
    }

    public void LoadData(BLL.DataBlock dataBlock)
    {
        LoadDataDelegate del = new LoadDataDelegate(LoadData);
        del.BeginInvoke(dataBlock, EndInvProc, "This is object");
    }

    static void EndInvProc(IAsyncResult result)
    {
    }

    delegate void LoadDataDelegate(BLL.DataBlock dataBlock);

    protected void TimerTick(object sender, EventArgs e)
    {
        int curr = Convert.ToInt32(Session["current"]);
        int maxx = Convert.ToInt32(Session["max"]);
        int proc = 0;
        if (maxx != 0)
        {
            int oneProc = (maxx / 100);
            if (oneProc != 0)
                proc = curr / oneProc;
        }

        int cellsCount = 0;

        if (proc > 0 && proc < 9)
            cellsCount = 1;
        if (proc > 9 && proc < 18)
            cellsCount = 2;
        if (proc > 18 && proc < 27)
            cellsCount = 3;
        if (proc > 27 && proc < 36)
            cellsCount = 4;
        if (proc > 36 && proc < 45)
            cellsCount = 5;
        if (proc > 45 && proc < 54)
            cellsCount = 6;
        if (proc > 54 && proc < 63)
            cellsCount = 7;
        if (proc > 63 && proc < 72)
            cellsCount = 8;
        if (proc > 72 && proc < 81)
            cellsCount = 9;
        if (proc > 81 && proc < 90)
            cellsCount = 10;
        if (proc > 90 && proc < 101)
        {
            cellsCount = 11;
            proc = 100;
        }
        if (maxx == curr && maxx != 0)
        {
            proc = 100;
        }
        if (curr > maxx)
        {
            proc = 100;
        }
        if (cellsCount >= 1)
            ProgressTableCell_1.BackColor = System.Drawing.Color.Goldenrod;
        if (cellsCount >= 2)
            ProgressTableCell_2.BackColor = System.Drawing.Color.Goldenrod;
        if (cellsCount >= 3)
            ProgressTableCell_3.BackColor = System.Drawing.Color.Goldenrod;
        if (cellsCount >= 4)
            ProgressTableCell_4.BackColor = System.Drawing.Color.Goldenrod;
        if (cellsCount >= 5)
            ProgressTableCell_5.BackColor = System.Drawing.Color.Goldenrod;
        if (cellsCount >= 6)
            ProgressTableCell_6.BackColor = System.Drawing.Color.Goldenrod;
        if (cellsCount >= 7)
            ProgressTableCell_7.BackColor = System.Drawing.Color.Goldenrod;
        if (cellsCount >= 8)
            ProgressTableCell_8.BackColor = System.Drawing.Color.Goldenrod;
        if (cellsCount >= 9)
            ProgressTableCell_9.BackColor = System.Drawing.Color.Goldenrod;
        if (cellsCount >= 10)
            ProgressTableCell_10.BackColor = System.Drawing.Color.Goldenrod;
        if (cellsCount >= 11)
            ProgressTableCell_11.BackColor = System.Drawing.Color.Goldenrod;
        TextResultLabel.Text = maxx + @"/" + curr;
        ProgressLabel.Text = proc.ToString() + "%";
    }
}
