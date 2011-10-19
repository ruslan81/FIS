using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Configuration;

/// <summary>
/// Summary description for AnyChartStockXml
/// </summary>
public class AnyChartStockXml
{   

    public static string AnyChartMultiYChart(List<PLFUnit.PLFRecord> records, List<string> graphsSelected)
    {

        //считаем Time_step
        int TimeStep = 30;
        if (records[0] != null && records[1] != null)
        {
            DateTime DATETIMETEMP = new DateTime();
            DATETIMETEMP = DATETIMETEMP.Add(records[1].SYSTEM_TIME.GetSystemTime().Subtract(records[0].SYSTEM_TIME.GetSystemTime()));
            TimeStep = DATETIMETEMP.Minute*60 + DATETIMETEMP.Second;
        }
        if (TimeStep >= 60)
            TimeStep = 2;
        else
            if (TimeStep >= 30)
                TimeStep = 1;
            else
                TimeStep = 0;

        System.Text.StringBuilder xmlSpeedForAnyChart = new System.Text.StringBuilder();
        System.Text.StringBuilder xmlRPMForAnyChart = new System.Text.StringBuilder();
        System.Text.StringBuilder xmlVoltageForAnyChart = new System.Text.StringBuilder();
        System.Text.StringBuilder xmlFuelVolumeForAnyChart = new System.Text.StringBuilder();
        System.Text.StringBuilder xmlFuelConsumptionForAnyChart = new System.Text.StringBuilder();
        System.Text.StringBuilder xmlTemperature1ForAnyChart = new System.Text.StringBuilder();
        System.Text.StringBuilder xmlWeight1ForAnyChart = new System.Text.StringBuilder();
        System.Text.StringBuilder xmlFuelCounterForAnyChart = new System.Text.StringBuilder();


        System.Text.StringBuilder xmlScrollerData = new System.Text.StringBuilder();
        PLFUnit.PLFRecord rec;

        bool Engine_rpm = false;
        bool FuelVol = false;
        bool Voltage = false;
        bool Speed = false;
        bool Temperature1 = false;
        bool FuelCons = false;
        bool FuelCounter = false;
        bool Weight1 = false;

        if (graphsSelected.Contains("ENGINE_RPM"))
            Engine_rpm = true;
        if (graphsSelected.Contains("FuelVolume"))
            FuelVol = true;
        if (graphsSelected.Contains("Voltage"))
            Voltage = true;
        if (graphsSelected.Contains("SPEED"))
            Speed = true;
        if (graphsSelected.Contains("Temperature1"))
            Temperature1 = true;
        if (graphsSelected.Contains("FuelConsumption"))
            FuelCons = true;
        if (graphsSelected.Contains("FuelCounter"))
            FuelCounter = true;
        if (graphsSelected.Contains("Weight1"))
            Weight1 = true;

        int recordsCount = records.Count;
        for (int r = 0; r < recordsCount; r++)
        {
            rec = records[r];
            if (Speed)
                xmlSpeedForAnyChart.Append(rec.SYSTEM_TIME.GetRoundedSystemTime(TimeStep) + "," + rec.SPEED + ";");
            if (Engine_rpm)
                xmlRPMForAnyChart.Append(rec.SYSTEM_TIME.GetRoundedSystemTime(TimeStep) + "," + rec.ENGINE_RPM + ";");
            if (Voltage)
                xmlVoltageForAnyChart.Append(rec.SYSTEM_TIME.GetRoundedSystemTime(TimeStep) + "," + rec.VOLTAGE + ";");
            if (FuelVol)
                xmlFuelVolumeForAnyChart.Append(rec.SYSTEM_TIME.GetRoundedSystemTime(TimeStep) + "," + rec.FUEL_VOLUME1 + ";");
            if (Temperature1)
                xmlTemperature1ForAnyChart.Append(rec.SYSTEM_TIME.GetRoundedSystemTime(TimeStep) + "," + rec.TEMPERATURE1 + ";");
            if (FuelCons)
                xmlFuelConsumptionForAnyChart.Append(rec.SYSTEM_TIME.GetRoundedSystemTime(TimeStep) + "," + rec.FUEL_CONSUMPTION + ";");
            if(FuelCounter)
                xmlFuelCounterForAnyChart.Append(rec.SYSTEM_TIME.GetRoundedSystemTime(TimeStep) + "," + rec.FUEL_COUNTER + ";");
            if (Weight1)
                xmlWeight1ForAnyChart.Append(rec.SYSTEM_TIME.GetRoundedSystemTime(TimeStep) + "," + rec.WEIGHT1 + ";");
        }
        /* Расчитываем скроллер с шагом */
        int step = 1;
        int maxAnyChartVisiblePoints = Convert.ToInt32(ConfigurationSettings.AppSettings["AnyChartMaxVisiblePoints"]);
        if (records.Count > maxAnyChartVisiblePoints)
            step = records.Count / maxAnyChartVisiblePoints;
        else
            step = 1;

        for (int r = 0; r < records.Count; r += step)
        {
            if (Speed)
                xmlScrollerData.Append(records[r].SYSTEM_TIME.GetRoundedSystemTime(TimeStep) + "," + records[r].SPEED + ";");
            else
                if (Engine_rpm)
                    xmlScrollerData.Append(records[r].SYSTEM_TIME.GetRoundedSystemTime(TimeStep) + "," + records[r].ENGINE_RPM + ";");
                else
                    if (Voltage)
                        xmlScrollerData.Append(records[r].SYSTEM_TIME.GetRoundedSystemTime(TimeStep) + "," + records[r].VOLTAGE + ";");
                    else
                        if (FuelVol)
                            xmlScrollerData.Append(records[r].SYSTEM_TIME.GetRoundedSystemTime(TimeStep) + "," + records[r].FUEL_VOLUME1 + ";");
                        else
                            if (Temperature1)
                                xmlScrollerData.Append(records[r].SYSTEM_TIME.GetRoundedSystemTime(TimeStep) + "," + records[r].TEMPERATURE1 + ";");
                            else
                                if (FuelCons)
                                    xmlScrollerData.Append(records[r].SYSTEM_TIME.GetRoundedSystemTime(TimeStep) + "," + records[r].FUEL_CONSUMPTION + ";");
                                else
                                    if (FuelCounter)
                                        xmlScrollerData.Append(records[r].SYSTEM_TIME.GetRoundedSystemTime(TimeStep) + "," + records[r].FUEL_COUNTER + ";");
                                    else
                                        if (Weight1)
                                            xmlScrollerData.Append(records[r].SYSTEM_TIME.GetRoundedSystemTime(TimeStep) + "," + records[r].WEIGHT1 + ";");

        }
        /*Конец расчета скроллера */

        List<string> YAxisNames = new List<string>();
        List<string> multyYChartData = new List<string>();
        List<string> colors = new List<string>();
        ChartColors col = new ChartColors();

        if (Speed)
        {
            multyYChartData.Add(xmlSpeedForAnyChart.ToString());
            YAxisNames.Add(@"Скорость (КМ/Ч)");
            colors.Add("#" + col.getFCColor_Iteration());
        }
        if (Engine_rpm)
        {
            multyYChartData.Add(xmlRPMForAnyChart.ToString());
            YAxisNames.Add("RPM");
            colors.Add("#" + col.getFCColor_Iteration());
        }
        if (Voltage)
        {
            multyYChartData.Add(xmlVoltageForAnyChart.ToString());
            YAxisNames.Add("Вольты");
            colors.Add("#" + col.getFCColor_Iteration());
        }
        if (FuelVol)
        {
            multyYChartData.Add(xmlFuelVolumeForAnyChart.ToString());
            YAxisNames.Add("Уровень топлива");
            colors.Add("#" + col.getFCColor_Iteration());
        }
        if (Temperature1)
        {
            multyYChartData.Add(xmlTemperature1ForAnyChart.ToString());
            YAxisNames.Add("Температура 1");
            colors.Add("#" + col.getFCColor_Iteration());
        }
        if (FuelCons)
        {
            multyYChartData.Add(xmlFuelConsumptionForAnyChart.ToString());
            YAxisNames.Add("Расход топлива");
            colors.Add("#" + col.getFCColor_Iteration());
        }
        if (FuelCounter)
        {
            multyYChartData.Add(xmlFuelCounterForAnyChart.ToString());
            YAxisNames.Add("Счетчик топлива");
            colors.Add("#" + col.getFCColor_Iteration());
        }
        if (Weight1)
        {
            multyYChartData.Add(xmlWeight1ForAnyChart.ToString());
            YAxisNames.Add("Вес 1");
            colors.Add("#" + col.getFCColor_Iteration());
        }
                                           

        return DoAnyMultyYChart(multyYChartData, YAxisNames, colors, xmlScrollerData.ToString());
    }

    private static string DoAnyMultyYChart(List<string> xmlDatasForAnyChart, List<string> YAxisNames, List<string> colors, string xmlScrollerData)
    {
        int rightMargin = xmlDatasForAnyChart.Count*40 - 40;


        string maxAnyChartVisiblePoints = ConfigurationSettings.AppSettings["AnyChartMaxVisiblePoints"];
        string returnString = "";
        returnString += @"<stock xmlns=""http://anychart.com/products/stock/schemas/1.0.0/schema.xsd"">";
        returnString += @"<data><data_sets>";
        for (int i = 0; i < xmlDatasForAnyChart.Count; i++)
        {
            returnString += @"<data_set id=""dataSet" + i.ToString() + @""" source_mode=""InternalData"" date_time_column=""0"">";
            returnString += "<csv_data><![CDATA[";
            returnString += xmlDatasForAnyChart[i];
            returnString += @"]]></csv_data>";
            returnString += @"<csv_settings ignore_first_row=""false"" rows_separator="";"" columns_separator="","" ignore_trailing_spaces=""true"" />"
            + "<locale><date_time><format><![CDATA[%yy:%MM:%dd %hh:%mm:%ss]]></format></date_time></locale></data_set>";
        }
        //scroller data provider(with steps)
        returnString += @"<data_set id=""dataSetScroller"" source_mode=""InternalData"" date_time_column=""0"">";
        returnString += "<csv_data><![CDATA[";
        returnString += xmlScrollerData;
        returnString += @"]]></csv_data>";
        returnString += @"<csv_settings ignore_first_row=""false"" rows_separator="";"" columns_separator="","" ignore_trailing_spaces=""true"" />"
        + "<locale><date_time><format><![CDATA[%yy:%MM:%dd %hh:%mm%:%ss]]></format></date_time></locale></data_set>";
        //end

        returnString += @"</data_sets>";
        returnString += @"<data_providers check_missing_points=""false""><general_data_providers>";
        for (int i = 0; i < xmlDatasForAnyChart.Count; i++)
        {
            returnString += @"<data_provider data_set=""dataSet" + i.ToString() + @""" id=""s" + i.ToString() + @""">"
                + @"<fields><field type=""Close"" column=""1"" approximation_type=""Close"" /></fields></data_provider>";
        }

        returnString += @"</general_data_providers>"
        + @"<scroller_data_providers><data_provider data_set=""dataSetScroller"" column=""1"" /></scroller_data_providers>"
        + @"</data_providers></data>"
        + @"<settings><inside_margin  left=""33"" right=""" + rightMargin + @"""  /><data_grouping enabled=""true"" max_visible_points=""" + maxAnyChartVisiblePoints + @""" />"
        + @"<charts><chart>";

        returnString += @"<legend><date_time enabled=""false""></date_time></legend>";

        returnString += "<value_axes>";
        returnString += @"<primary position=""Left"">"
        + @"<line enabled=""true"" color=""" + colors[0] + @""" thickness=""2"" />"
        + @"<labels position=""Outside"" padding=""3"" show_first=""true"" show_last=""true"" valign=""Center"">"
        + @"<format><![CDATA[{%Value}{numDecimals:0}]]></format></labels>"
        + @"<tickmarks enabled=""true"" color=""" + colors[0] + @""" opacity=""1"" size=""3"" inside=""false"" outside=""true"" thickness=""2""/>"
        + @"<scale mode=""Values""/>"
        + @"<grid enabled=""false"" />"
        + @"</primary>";
        returnString += "<extra>";
        string offset = "";
        for (int i = 1; i < xmlDatasForAnyChart.Count; i++)
        {
            offset = (5 + 40 * (i - 1)).ToString();
            returnString += @"<axis id=""extra" + i.ToString() + @""" position=""Right"" offset=""" + offset + @""">"
                + @"<line enabled=""true"" color=""" + colors[i] + @""" thickness=""2"" />"
                + @"<labels position=""Outside"" padding=""3"" show_first=""true"" show_last=""true"" valign=""Center"">"
                + @"<format><![CDATA[{%Value}{numDecimals:0}]]></format></labels>"
                + @"<tickmarks enabled=""true"" color=""" + colors[i] + @""" opacity=""1"" size=""3"" inside=""false"" outside=""true"" thickness=""2"" />"
                + @"<scale mode=""Values"" />"
                + @"<grid enabled=""false"" />"
                + @"</axis>";
        }
        returnString += "</extra></value_axes>";
        returnString += @"<x_axis><minor_grid><line color=""#A0A0A0"" dashed=""true"" dash_length=""3"" dash_space=""3""/>"
            + @"</minor_grid><major_grid><line color=""#B5B5B5"" dashed=""false"" /></major_grid></x_axis>";
        returnString += @"<series_list>";
        string axis = "";
        for (int i = 0; i < xmlDatasForAnyChart.Count; i++)
        {
            if (i == 0)
                axis = @"""Primary""";
            else
                axis = @"""extra" + i.ToString() + @"""";
            returnString += @"<series thickness=""2"" type=""Line"" data_provider=""s" + i.ToString() + @""" color=""" + colors[i] + @""" axis=" + axis + ">"
                + @"<name><![CDATA[" + YAxisNames[i] + @"]]></name><line_series connect_missing_points=""false"" /></series>";
        }
        returnString += "</series_list></chart></charts>"
            + @"<time_scale is_ordinal=""true""></time_scale>";
        returnString += @"<scroller connect_missing_points=""false""></scroller>";
        returnString+= @"</settings></stock>";
        return returnString;
    }

     
    public static string WriteToUsersTempFile(string stringFile)
    {
        // Writes text to a temporary file and returns path
        string strFilename = Path.GetTempFileName();
        FileStream objFS = new FileStream(strFilename, FileMode.Append, FileAccess.Write);
        StreamWriter Writer = new StreamWriter(objFS);
        Writer.BaseStream.Seek(0, SeekOrigin.End);
        Writer.WriteLine(stringFile);
        Writer.Flush();
        Writer.Close();
        return strFilename;
    }

    public static string WriteToServersTempFile(string stringFile, string serverPath)
    {
        StreamWriter fp;
        try
        {
            string fileName = Guid.NewGuid().ToString() + ".xml";
            fp = File.CreateText(serverPath+ "\\" + fileName);
            fp.WriteLine(stringFile);
            fp.Close();
            return fileName;
        }
        catch(Exception err)
        {
           throw new Exception("File Creation failed. Reason is as follows: " + err.ToString());
        }
    }
}
