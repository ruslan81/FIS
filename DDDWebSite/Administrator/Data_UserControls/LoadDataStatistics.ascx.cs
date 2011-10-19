using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using BLL;

public partial class Administrator_Data_UserControls_LoadDataStatistics : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["StatisticsDataBlockIds"] = null;
        }
    }

    protected void RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            /*e.Row.Attributes["onclick"] = 
            ClientScript.GetPostBackClientHyperlink
                (this.FilesPreviewDataGrid, "Select$" + e.Row.RowIndex);*/

            e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackClientHyperlink(e.Row.NamingContainer, "Select$" + e.Row.RowIndex));
        }
    }

    protected void ReloadStats_Click(object s, EventArgs e)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");

        try
        {
            bool isDDD = IsDDDFile();
            List<int> selectedIds = new List<int>();
            selectedIds = (List<int>)Session["StatisticsDataBlockIds"];
            int yearVal = Convert.ToInt32(YearsGrid.SelectedRow.Cells[0].Text);
            int monthNumber = MonthGrid.SelectedIndex+1;
            dataBlock.OpenConnection();
           
            if (isDDD)
            {
                if (dataBlock.GetDataBlock_CardType(selectedIds[0]) == 0)//dddDriverCard
                {
                    MonthGrid_Values.DataSource = GetDDDMonthValuesDataTable(dataBlock, selectedIds, yearVal);
                    DaysGrid.DataSource = GetDDDDayValuesDataTable(dataBlock, selectedIds, yearVal, monthNumber);
                }
                else//DDDVehicleCard
                {
                    MonthGrid_Values.DataSource = GetVehicleMonthValuesDataTable(dataBlock, selectedIds, yearVal);
                    DaysGrid.DataSource = GetVehicleDayValuesDataTable(dataBlock, selectedIds, yearVal, monthNumber);
                }
            }
            else
            {
                MonthGrid_Values.DataSource = GetPLFMonthValuesDataTable(dataBlock, selectedIds, yearVal);
                DaysGrid.DataSource = GetPLFDayValuesDataTable(dataBlock, selectedIds, yearVal, monthNumber);
            }
            MonthGrid_Values.DataBind();
            DaysGrid.DataBind();

            dataBlock.CloseConnection();
            DateUpdate.Update();
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
            DateUpdate.Update();
        }
        finally
        {
            dataBlock.CloseConnection();
        }
    }

    public void CheckPlfFiles(bool YES)
    {
        Session["isDDDField"] = (!YES).ToString();
        DateUpdate.Update();
    }
    private bool IsDDDFile()
    {
        bool isDDD = Convert.ToBoolean(Session["isDDDField"].ToString());
        return isDDD;
    }

    public void LoadAllDriversDateStatistics(List<int> dataBlockIds)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");

        try
        {
            DateTime mindate = DateTime.Now;
            DateTime maxdate = new DateTime();
            List<DateTime> startEndPeriod = new List<DateTime>();
            List<int> selectedIds = new List<int>();
            bool isDDD = IsDDDFile();
            dataBlock.OpenConnection();

            Session["AllDataBlockIds"] = dataBlockIds;
            int fileType = 0;
            foreach (int id in dataBlockIds)
            {
                fileType = dataBlock.GetDataBlock_CardType(id);
                if (isDDD == true)//DDD
                    if (fileType == 2 || fileType == 1)//2-PLF, 1-vehicle
                        continue;
                    else
                    {
                        selectedIds.Add(id);
                        startEndPeriod = dataBlock.cardUnitInfo.Get_StartEndPeriod(id);
                    }
                else//PLF
                    if (fileType == 0 || fileType == 1)//0 - drivers DDD; 1 for vehicle
                        continue;
                    else
                    {
                        selectedIds.Add(id);
                        startEndPeriod = new List<DateTime>();
                        startEndPeriod.Add(dataBlock.plfUnitInfo.Get_START_PERIOD(id));
                        startEndPeriod.Add(dataBlock.plfUnitInfo.Get_END_PERIOD(id));
                    }

                if (mindate > startEndPeriod[0])
                    mindate = startEndPeriod[0];
                if (maxdate < startEndPeriod[1])
                    maxdate = startEndPeriod[1];
            }

            Session["StatisticsDataBlockIds"] = selectedIds;
            List<int> yearsList = GetYearList(mindate, maxdate);

            if (isDDD)
            {
                LoadAllForDDDDriver(dataBlock, selectedIds, yearsList);
            }
            else
            {
                LoadAllForPLFFile(dataBlock, selectedIds, yearsList);
            }
            Status.Text = "";
            YearsGrid.SelectedIndex = 0;
            MonthGrid.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
        finally
        {
            dataBlock.CloseConnection();
            DateUpdate.Update();
        }
    }
    public void LoadAllVehiclesDateStatistics(List<int> dataBlockIds)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");

        try
        {
            DateTime mindate = DateTime.Now;
            DateTime maxdate = new DateTime();
            List<DateTime> startEndPeriod = new List<DateTime>();
            List<int> selectedIds = new List<int>();
            dataBlock.OpenConnection();
            Session["AllDataBlockIds"] = dataBlockIds;
            int fileType = 0;
            foreach (int id in dataBlockIds)
            {
                fileType = dataBlock.GetDataBlock_CardType(id);
                    if (fileType == 0 || fileType == 2)//0 - drivers DDD; 1 for vehicle
                        continue;
                    else
                    {
                        selectedIds.Add(id);
                        startEndPeriod = new List<DateTime>();
                        startEndPeriod = dataBlock.vehicleUnitInfo.Get_StartEndPeriod(id);
                    }

                if (mindate > startEndPeriod[0])
                    mindate = startEndPeriod[0];
                if (maxdate < startEndPeriod[1])
                    maxdate = startEndPeriod[1];
            }

            Session["StatisticsDataBlockIds"] = selectedIds;
            List<int> yearsList = GetYearList(mindate, maxdate);

            LoadAllForVehicleFile(dataBlock, selectedIds, yearsList);

            Status.Text = "";
            YearsGrid.SelectedIndex = 0;
            MonthGrid.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
        finally
        {
            dataBlock.CloseConnection();
            DateUpdate.Update();
        }
    }
    public void ClearAllData()
    {
        YearsGrid.DataSource = null;
        YearsGrid.DataBind();
        YearsGrid_Values.DataSource = null;
        YearsGrid_Values.DataBind();
        MonthGrid.DataSource = null;
        MonthGrid.DataBind();
        MonthGrid_Values.DataSource = null;
        MonthGrid_Values.DataBind();
        DaysGrid.DataSource = null;
        DaysGrid.DataBind();
        DateUpdate.Update();
    }

    private void LoadAllForDDDDriver(DataBlock dataBlock, List<int> selectedIds, List<int> yearsList)
    {
        YearsGrid.DataSource = GetYearsDataTable(yearsList);
        YearsGrid.DataBind();
        YearsGrid_Values.DataSource = GetDDDYearsValuesDataTable(dataBlock, selectedIds, yearsList);
        YearsGrid_Values.DataBind();

        MonthGrid.DataSource = GetMonthDataTable();
        MonthGrid.DataBind();
        if (yearsList.Count > 0)
        {
            MonthGrid_Values.DataSource = GetDDDMonthValuesDataTable(dataBlock, selectedIds, yearsList[0]);
            MonthGrid_Values.DataBind();
        }

        if (yearsList.Count > 0)
        {
            DaysGrid.DataSource = GetDDDDayValuesDataTable(dataBlock, selectedIds, yearsList[0], 1);
            DaysGrid.DataBind();
        }
    }
    private void LoadAllForPLFFile(DataBlock dataBlock, List<int> selectedIds, List<int> yearsList)
    {
        YearsGrid.DataSource = GetYearsDataTable(yearsList);
        YearsGrid.DataBind();
        YearsGrid_Values.DataSource = GetPLFYearsValuesDataTable(dataBlock, selectedIds, yearsList);
        YearsGrid_Values.DataBind();

        MonthGrid.DataSource = GetMonthDataTable();
        MonthGrid.DataBind();
        if (yearsList.Count > 0)
        {
            MonthGrid_Values.DataSource = GetPLFMonthValuesDataTable(dataBlock, selectedIds, yearsList[0]);
            MonthGrid_Values.DataBind();
        }

        if (yearsList.Count > 0)
        {
            DaysGrid.DataSource = GetPLFDayValuesDataTable(dataBlock, selectedIds, yearsList[0], 1);
            DaysGrid.DataBind();
        }
    }
    private void LoadAllForVehicleFile(DataBlock dataBlock, List<int> selectedIds, List<int> yearsList)
    {
        YearsGrid.DataSource = GetYearsDataTable(yearsList);
        YearsGrid.DataBind();
        YearsGrid_Values.DataSource = GetVehicleYearsValuesDataTable(dataBlock, selectedIds, yearsList);
        YearsGrid_Values.DataBind();

        MonthGrid.DataSource = GetMonthDataTable();
        MonthGrid.DataBind();
        if (yearsList.Count > 0)
        {
            MonthGrid_Values.DataSource = GetVehicleMonthValuesDataTable(dataBlock, selectedIds, yearsList[0]);
            MonthGrid_Values.DataBind();
        }

        if (yearsList.Count > 0)
        {
            DaysGrid.DataSource = GetVehicleDayValuesDataTable(dataBlock, selectedIds, yearsList[0], 1);
            DaysGrid.DataBind();
        }
    }

    private DataTable GetYearsDataTable(List<int> yearsList)
    {
        DataTable yearsTable = new DataTable(Guid.NewGuid().ToString());
        DataRow dr;
        string Col_1 = "Год";

        yearsTable.Columns.Add(new DataColumn(Col_1, typeof(string)));

        foreach(int year in yearsList)
        {
            dr = yearsTable.NewRow();
            dr[Col_1] = year.ToString();
            yearsTable.Rows.Add(dr);
        }
        return yearsTable;
    }
    private DataTable GetMonthDataTable()
    {
        DataTable monthTable = new DataTable(Guid.NewGuid().ToString());
        DataRow dr;
        string Col_1 = "Месяц";

        monthTable.Columns.Add(new DataColumn(Col_1, typeof(string)));
        DateTime date = new DateTime();

        for (int i = 0; i < 12; i++)
        {
            dr = monthTable.NewRow();
            dr[Col_1] = date.ToString("MMMM");
            monthTable.Rows.Add(dr);
            date = date.AddMonths(1);
        }

        return monthTable;
    }   

    private DataTable GetDDDYearsValuesDataTable(DataBlock dataBlock, List<int> dataBlockIds, List<int> yearsList)
    {
        DataTable yearsTable = new DataTable(Guid.NewGuid().ToString());
        DataRow dr;
        string Col_1 = "Процент данных";

        yearsTable.Columns.Add(new DataColumn(Col_1, typeof(string)));

        List<double> yearProcents = new List<double>();
        int procCount = 0;
        double procSum = 0;
        foreach (int year in yearsList)
        {
            foreach (int id in dataBlockIds)
            {
                procSum += dataBlock.cardUnitInfo.Statistics_GetYearStatistics(new DateTime(year, 1, 1), id);
                procCount++;
            }
            yearProcents.Add(procSum / procCount);
            procSum = 0;
            procCount = 0;            
        }

        foreach (double proc in yearProcents)
        {
            dr = yearsTable.NewRow();
            dr[Col_1] = String.Format("{0:0.00}", proc);
            yearsTable.Rows.Add(dr);
        }
        return yearsTable;
    }
    private DataTable GetDDDMonthValuesDataTable(DataBlock dataBlock, List<int> dataBlockIds, int year)
    {
        DataTable monthTable = new DataTable(Guid.NewGuid().ToString());
        DataRow dr;
        string Col_1 = "Процент данных";

        monthTable.Columns.Add(new DataColumn(Col_1, typeof(string)));

        List<double> MonthProcents = new List<double>();
        int procCount = 0;
        double procSum = 0;
        for(int month=1;month<=12; month++)
        {
            foreach (int id in dataBlockIds)
            {
                procSum += dataBlock.cardUnitInfo.Statistics_GetMonthStatistics(new DateTime(year, month, 1), id);
                procCount++;
            }
            MonthProcents.Add(procSum / procCount);
            procSum = 0;
            procCount = 0;
        }

        foreach (double proc in MonthProcents)
        {
            dr = monthTable.NewRow();
            dr[Col_1] = String.Format("{0:0.00}", proc);
            monthTable.Rows.Add(dr);
        }
        return monthTable;
    }
    private DataTable GetDDDDayValuesDataTable(DataBlock dataBlock, List<int> dataBlockIds, int year, int month)
    {
        DataTable daysTable = new DataTable(Guid.NewGuid().ToString());
        DataRow dr;
        string Col_1 = "Число";
        string Col_2 = "Процент данных";
        string Col_3 = "Число ";
        string Col_4 = "Процент данных ";

        daysTable.Columns.Add(new DataColumn(Col_1, typeof(int)));
        daysTable.Columns.Add(new DataColumn(Col_2, typeof(string)));
        daysTable.Columns.Add(new DataColumn(Col_3, typeof(string)));
        daysTable.Columns.Add(new DataColumn(Col_4, typeof(string)));

        int daysInMonth = DateTime.DaysInMonth(year, month);
        List<double> dayProcents = new List<double>();
        int procCount = 0;
        double procSum = 0;
        for (int day = 1; day <= daysInMonth; day++)
        {
            foreach (int id in dataBlockIds)
            {
                procSum += dataBlock.cardUnitInfo.Statistics_GetDayStatistics(new DateTime(year, month, day), id);
                procCount++;
            }
            dayProcents.Add(procSum / procCount);
            procSum = 0;
            procCount = 0;
        }

        int halfMontsDaysCount = 16;
        for (int i = 1; i <= halfMontsDaysCount; i++)
        {
            dr = daysTable.NewRow();
            dr[Col_1] = i;
            dr[Col_2] = String.Format("{0:0.00}", dayProcents[i-1]);
            if ((i + halfMontsDaysCount) <= daysInMonth)
            {
                dr[Col_3] = i + halfMontsDaysCount;
                dr[Col_4] = String.Format("{0:0.00}", dayProcents[i + halfMontsDaysCount-1]);
            }
            else
            {
                dr[Col_3] = "";
                dr[Col_4] = "";
            }
            daysTable.Rows.Add(dr);
        }
        return daysTable;
    }

    private DataTable GetPLFYearsValuesDataTable(DataBlock dataBlock, List<int> dataBlockIds, List<int> yearsList)
    {
        DataTable yearsTable = new DataTable(Guid.NewGuid().ToString());
        DataRow dr;
        string Col_1 = "Процент данных";

        yearsTable.Columns.Add(new DataColumn(Col_1, typeof(string)));

        List<double> yearProcents = new List<double>();
        int procCount = 0;
        double procSum = 0;
        foreach (int year in yearsList)
        {
            foreach (int id in dataBlockIds)
            {
                procSum += dataBlock.plfUnitInfo.Statistics_GetYearStatistics(new DateTime(year, 1, 1), id);
                procCount++;
            }
            yearProcents.Add(procSum / procCount);
            procSum = 0;
            procCount = 0;
        }

        foreach (double proc in yearProcents)
        {
            dr = yearsTable.NewRow();
            dr[Col_1] = String.Format("{0:0.00}", proc);
            yearsTable.Rows.Add(dr);
        }
        return yearsTable;
    }
    private DataTable GetPLFMonthValuesDataTable(DataBlock dataBlock, List<int> dataBlockIds, int year)
    {
        DataTable monthTable = new DataTable(Guid.NewGuid().ToString());
        DataRow dr;
        string Col_1 = "Процент данных";

        monthTable.Columns.Add(new DataColumn(Col_1, typeof(string)));

        List<double> MonthProcents = new List<double>();
        int procCount = 0;
        double procSum = 0;
        for (int month = 1; month <= 12; month++)
        {
            foreach (int id in dataBlockIds)
            {
                procSum += dataBlock.plfUnitInfo.Statistics_GetMonthStatistics(new DateTime(year, month, 1), id);
                procCount++;
            }
            MonthProcents.Add(procSum / procCount);
            procSum = 0;
            procCount = 0;
        }

        foreach (double proc in MonthProcents)
        {
            dr = monthTable.NewRow();
            dr[Col_1] = String.Format("{0:0.00}", proc);
            monthTable.Rows.Add(dr);
        }
        return monthTable;
    }
    private DataTable GetPLFDayValuesDataTable(DataBlock dataBlock, List<int> dataBlockIds, int year, int month)
    {
        DataTable daysTable = new DataTable(Guid.NewGuid().ToString());
        DataRow dr;
        string Col_1 = "Число";
        string Col_2 = "Процент данных";
        string Col_3 = "Число ";
        string Col_4 = "Процент данных ";

        daysTable.Columns.Add(new DataColumn(Col_1, typeof(int)));
        daysTable.Columns.Add(new DataColumn(Col_2, typeof(string)));
        daysTable.Columns.Add(new DataColumn(Col_3, typeof(string)));
        daysTable.Columns.Add(new DataColumn(Col_4, typeof(string)));

        int daysInMonth = DateTime.DaysInMonth(year, month);
        List<double> dayProcents = new List<double>();
        int procCount = 0;
        double procSum = 0;
        for (int day = 1; day <= daysInMonth; day++)
        {
            foreach (int id in dataBlockIds)
            {
                procSum += dataBlock.plfUnitInfo.Statistics_GetDayStatistics(new DateTime(year, month, day), id);
                procCount++;
            }
            dayProcents.Add(procSum / procCount);
            procSum = 0;
            procCount = 0;
        }

        int halfMontsDaysCount = 16;
        for (int i = 1; i <= halfMontsDaysCount; i++)
        {
            dr = daysTable.NewRow();
            dr[Col_1] = i;
            dr[Col_2] = String.Format("{0:0.00}", dayProcents[i - 1]);
            if ((i + halfMontsDaysCount) <= daysInMonth)
            {
                dr[Col_3] = i + halfMontsDaysCount;
                dr[Col_4] = String.Format("{0:0.00}", dayProcents[i + halfMontsDaysCount - 1]);
            }
            else
            {
                dr[Col_3] = "";
                dr[Col_4] = "";
            }
            daysTable.Rows.Add(dr);
        }
        return daysTable;
    }

    private DataTable GetVehicleYearsValuesDataTable(DataBlock dataBlock, List<int> dataBlockIds, List<int> yearsList)
    {
        DataTable yearsTable = new DataTable(Guid.NewGuid().ToString());
        DataRow dr;
        string Col_1 = "Процент данных";

        yearsTable.Columns.Add(new DataColumn(Col_1, typeof(string)));

        List<double> yearProcents = new List<double>();
        int procCount = 0;
        double procSum = 0;
        foreach (int year in yearsList)
        {
            foreach (int id in dataBlockIds)
            {
                procSum += dataBlock.vehicleUnitInfo.Statistics_GetYearStatistics(new DateTime(year, 1, 1), id);
                procCount++;
            }
            yearProcents.Add(procSum / procCount);
            procSum = 0;
            procCount = 0;
        }

        foreach (double proc in yearProcents)
        {
            dr = yearsTable.NewRow();
            dr[Col_1] = String.Format("{0:0.00}", proc);
            yearsTable.Rows.Add(dr);
        }
        return yearsTable;
    }
    private DataTable GetVehicleMonthValuesDataTable(DataBlock dataBlock, List<int> dataBlockIds, int year)
    {
        DataTable monthTable = new DataTable(Guid.NewGuid().ToString());
        DataRow dr;
        string Col_1 = "Процент данных";

        monthTable.Columns.Add(new DataColumn(Col_1, typeof(string)));

        List<double> MonthProcents = new List<double>();
        int procCount = 0;
        double procSum = 0;
        for (int month = 1; month <= 12; month++)
        {
            foreach (int id in dataBlockIds)
            {
                procSum += dataBlock.vehicleUnitInfo.Statistics_GetMonthStatistics(new DateTime(year, month, 1), id);
                procCount++;
            }
            MonthProcents.Add(procSum / procCount);
            procSum = 0;
            procCount = 0;
        }

        foreach (double proc in MonthProcents)
        {
            dr = monthTable.NewRow();
            dr[Col_1] = String.Format("{0:0.00}", proc);
            monthTable.Rows.Add(dr);
        }
        return monthTable;
    }
    private DataTable GetVehicleDayValuesDataTable(DataBlock dataBlock, List<int> dataBlockIds, int year, int month)
    {
        DataTable daysTable = new DataTable(Guid.NewGuid().ToString());
        DataRow dr;
        string Col_1 = "Число";
        string Col_2 = "Процент данных";
        string Col_3 = "Число ";
        string Col_4 = "Процент данных ";

        daysTable.Columns.Add(new DataColumn(Col_1, typeof(int)));
        daysTable.Columns.Add(new DataColumn(Col_2, typeof(string)));
        daysTable.Columns.Add(new DataColumn(Col_3, typeof(string)));
        daysTable.Columns.Add(new DataColumn(Col_4, typeof(string)));

        int daysInMonth = DateTime.DaysInMonth(year, month);
        List<double> dayProcents = new List<double>();
        int procCount = 0;
        double procSum = 0;
        for (int day = 1; day <= daysInMonth; day++)
        {
            foreach (int id in dataBlockIds)
            {
                procSum += dataBlock.vehicleUnitInfo.Statistics_GetDayStatistics(new DateTime(year, month, day), id);
                procCount++;
            }
            dayProcents.Add(procSum / procCount);
            procSum = 0;
            procCount = 0;
        }

        int halfMontsDaysCount = 16;
        for (int i = 1; i <= halfMontsDaysCount; i++)
        {
            dr = daysTable.NewRow();
            dr[Col_1] = i;
            dr[Col_2] = String.Format("{0:0.00}", dayProcents[i - 1]);
            if ((i + halfMontsDaysCount) <= daysInMonth)
            {
                dr[Col_3] = i + halfMontsDaysCount;
                dr[Col_4] = String.Format("{0:0.00}", dayProcents[i + halfMontsDaysCount - 1]);
            }
            else
            {
                dr[Col_3] = "";
                dr[Col_4] = "";
            }
            daysTable.Rows.Add(dr);
        }
        return daysTable;
    }


    private List<int> GetYearList(DateTime minDate, DateTime maxDate)
    {
        List<int> yearsList = new List<int>();
        for (int i = minDate.Year; i <= maxDate.Year; i++)
            yearsList.Add(i);
        return yearsList;
    }
}
