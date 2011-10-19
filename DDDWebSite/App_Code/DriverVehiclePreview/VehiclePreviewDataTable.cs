using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using VehichleUnit;
/// <summary>
/// Summary description for VehiclePreviewDataTable
/// </summary>
public class VehiclePreviewDataTable
{
	public VehiclePreviewDataTable()
	{		
	}

    public static DataTable VehiclePreview_FileContents(VehicleUnitClass card)
    {
        DataTable activityTable = new DataTable(Guid.NewGuid().ToString());
        DataRow dr;
        string Col_1 = "Name";
        string Col_3 = "noOfRecords";
        string Col_2 = "Length";

        activityTable.Columns.Add(new DataColumn(Col_1, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_3, typeof(int)));
        activityTable.Columns.Add(new DataColumn(Col_2, typeof(int)));

        dr = activityTable.NewRow();
        dr[Col_1] = "MemberStateCertificate";
        dr[Col_3] = 1;
        dr[Col_2] = 194;
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "VUCertificate";
        dr[Col_3] = 1;
        dr[Col_2] = 194;
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "VehicleIdentification";
        dr[Col_3] = 1;
        dr[Col_2] = 32;
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "CurrentDateTime";
        dr[Col_3] = 1;
        dr[Col_2] = 4;
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "VUDownloadablePeriod";
        dr[Col_3] = 1;
        dr[Col_2] = 8;
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "CardSlotsStatus";
        dr[Col_3] = 1;
        dr[Col_2] = 1;
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "VUDownloadActivityData";
        dr[Col_3] = 1;
        dr[Col_2] = 40;
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "VUCompanyLocksData";
        dr[Col_3] = card.vehicleOverview.vuCompanyLocksData.noOfLocks;
        dr[Col_2] = card.vehicleOverview.vuCompanyLocksData.structureSize;
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "VUControlActivityData";
        dr[Col_3] = card.vehicleOverview.vuControlActivityData.noOfControls;
        dr[Col_2] = card.vehicleOverview.vuControlActivityData.structureSize;
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "Signature";
        dr[Col_3] = 1;
        dr[Col_2] = 128;
        activityTable.Rows.Add(dr);

        foreach(Vehicle_Activities act in card.vehicleActivities)
        {
            dr = activityTable.NewRow();
            dr[Col_1] = "VUCardIWData";
            dr[Col_3] = act.vuCardIWData.noOfIWRecords;
            dr[Col_2] = act.vuCardIWData.structureSize;
            activityTable.Rows.Add(dr);

            dr = activityTable.NewRow();
            dr[Col_1] = "VUActivityDailyData";
            dr[Col_3] = act.vuActivityDailyData.noOfActivityChanges;
            dr[Col_2] = act.vuActivityDailyData.structureSize;
            activityTable.Rows.Add(dr);

            dr = activityTable.NewRow();
            dr[Col_1] = "VUPlaceDailyWorkPeriodData";
            dr[Col_3] = act.vuPlaceDailyWorkPeriodData.noOfPlaceRecords;
            dr[Col_2] = act.vuPlaceDailyWorkPeriodData.structureSize;
            activityTable.Rows.Add(dr);

            dr = activityTable.NewRow();
            dr[Col_1] = "VUSpecificConditionData";
            dr[Col_3] = act.vuSpecificConditionData.noOfSpecificConditionRecords;
            dr[Col_2] = act.vuSpecificConditionData.structureSize;
            activityTable.Rows.Add(dr);

            dr = activityTable.NewRow();
            dr[Col_1] = "Signature";
            dr[Col_3] = 1;
            dr[Col_2] = 128;
            activityTable.Rows.Add(dr);
        }

        //----------------------------------------Дни активности ТС закончились

        dr = activityTable.NewRow();
        dr[Col_1] = "VUIdentification";
        dr[Col_3] = 1;
        dr[Col_2] = 116;
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "SensorPaired";
        dr[Col_3] = 1;
        dr[Col_2] = 20;
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "VUCalibrationData";
        dr[Col_3] = card.vehicleTechnicalData.vuCalibrationData.noOfVuCalibrationRecords;
        dr[Col_2] = card.vehicleTechnicalData.vuCalibrationData.structureSize;
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "Signature";
        dr[Col_3] = 1;
        dr[Col_2] = 128;
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "VUFaultData";
        dr[Col_3] = card.vehicleEventsAndFaults.vuFaultData.noOfVuFaults;
        dr[Col_2] = card.vehicleEventsAndFaults.vuFaultData.structureSize;
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "VUEventData";
        dr[Col_3] = card.vehicleEventsAndFaults.vuEventData.noOfVuEvents;
        dr[Col_2] = card.vehicleEventsAndFaults.vuEventData.structureSize;
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "VUOverSpeedingControlData";
        dr[Col_3] = 1;
        dr[Col_2] = 9;
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "VUOverSpeedingEventData";
        dr[Col_3] = card.vehicleEventsAndFaults.vuOverSpeedingEventData.noOfOverSpeedingEvents;
        dr[Col_2] = card.vehicleEventsAndFaults.vuOverSpeedingEventData.structureSize;
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "VUTimeAdjustmentData";
        dr[Col_3] = card.vehicleEventsAndFaults.vuTimeAdjustmentData.noOfVuTimeAdjRecords;
        dr[Col_2] = card.vehicleEventsAndFaults.vuTimeAdjustmentData.size;
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "Signature";
        dr[Col_3] = 1;
        dr[Col_2] = 128;
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "VUDetailedSpeedData";
        dr[Col_3] = card.vehicleDetailedSpeed.vuDetailedSpeedData.noOfSpeedBlocks;
        dr[Col_2] = card.vehicleDetailedSpeed.size;
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "Signature";
        dr[Col_3] = 1;
        dr[Col_2] = 128;
        activityTable.Rows.Add(dr);

        return activityTable;
    }

    public static DataTable VehiclePreview_Identification(VehichleUnit.Vehicle_Overview card)
    {
        DataTable activityTable = new DataTable(Guid.NewGuid().ToString());
        DataRow dr;
        string Col_1 = "description";
        string Col_2 = "value";

        activityTable.Columns.Add(new DataColumn(Col_1, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_2, typeof(string)));

        dr = activityTable.NewRow();
        dr[Col_1] = "vehicleIdentificationNumber";
        dr[Col_2] = card.vehicleIdentificationNumber.ToString();
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "vehicleRegistrationNation";
        dr[Col_2] = card.vehicleRegistrationIdentification.vehicleRegistrationNation.ToString();
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "vehicleRegistrationNumber";
        dr[Col_2] = card.vehicleRegistrationIdentification.vehicleRegistrationNumber.ToString();
        activityTable.Rows.Add(dr);

        return activityTable;
    }
    public static DataTable VehiclePreview_CurrentDateTime(DDDClass.CurrentDateTime time)
    {
        DataTable activityTable = new DataTable(Guid.NewGuid().ToString());
        DataRow dr;
        string Col_1 = "description";
        string Col_2 = "value";

        activityTable.Columns.Add(new DataColumn(Col_1, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_2, typeof(string)));
        DateTime date = time.getTimeRealDate();
        dr = activityTable.NewRow();
        dr[Col_1] = "currentDateTime";
        dr[Col_2] = date.ToLongDateString() + " " + date.ToLongTimeString();
        activityTable.Rows.Add(dr);

        return activityTable;
    }
    public static DataTable VehiclePreview_DownloadablePeriod(DDDClass.VuDownloadablePeriod period)
    {
        DataTable activityTable = new DataTable(Guid.NewGuid().ToString());
        DataRow dr;
        string Col_1 = "description";
        string Col_2 = "value";

        activityTable.Columns.Add(new DataColumn(Col_1, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_2, typeof(string)));
        DateTime date = period.minDownloadableTime.getTimeRealDate();

        dr = activityTable.NewRow();
        dr[Col_1] = "minDownloadableTime";
        dr[Col_2] = date.ToLongDateString() + " " + date.ToLongTimeString();
        activityTable.Rows.Add(dr);

        date = period.maxDownloadableTime.getTimeRealDate();
        dr = activityTable.NewRow();
        dr[Col_1] = "maxDownloadableTime";
        dr[Col_2] = date.ToLongDateString() + " " + date.ToLongTimeString();
        activityTable.Rows.Add(dr);

        return activityTable;
    }
    public static DataTable VehiclePreview_InsertedCardType(DDDClass.CardSlotsStatus slotsStatus)
    {
        DataTable activityTable = new DataTable(Guid.NewGuid().ToString());
        DataRow dr;
        string Col_1 = "description";
        string Col_2 = "value";

        activityTable.Columns.Add(new DataColumn(Col_1, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_2, typeof(string)));

        dr = activityTable.NewRow();
        dr[Col_1] = "driverCardSlot";
        dr[Col_2] = slotsStatus.getDriverCardSlotsStatus_toString();
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "co-driverCardSlot";
        dr[Col_2] = slotsStatus.getCoDriverCardSlotsStatus_toString();
        activityTable.Rows.Add(dr);

        return activityTable;
    }
    public static DataTable VehiclePreview_DownloadActivityData(DDDClass.VuDownloadActivityData data)
    {
        DataTable activityTable = new DataTable(Guid.NewGuid().ToString());
        DataRow dr;
        string Col_1 = "description";
        string Col_2 = "value";

        DateTime date = new DateTime();
        activityTable.Columns.Add(new DataColumn(Col_1, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_2, typeof(string)));

        date = data.downloadingTime.getTimeRealDate();

        dr = activityTable.NewRow();
        dr[Col_1] = "downloadingTime";
        dr[Col_2] = date.ToLongDateString() + " " + date.ToLongTimeString();
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "cardType";
        dr[Col_2] = data.fullCardNumber.cardType.ToString();
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "cardIssuingMemberState";
        dr[Col_2] = data.fullCardNumber.cardIssuingMemberState.ToString();
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "driverCardNumber";
        dr[Col_2] = data.fullCardNumber.cardNumber.ToString();
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "companyOrWorkshopName";
        dr[Col_2] = data.companyOrWorkshopName.ToString();
        activityTable.Rows.Add(dr);

        return activityTable;
    }
    public static DataTable VehiclePreview_CompanyLocksData(List<DDDClass.VuCompanyLocksRecord> locks)
    {
        DataTable activityTable = new DataTable(Guid.NewGuid().ToString());
        DataRow dr;
        string Col_1 = "lockInTime";
        string Col_2 = "lockOutTime";
        string Col_3 = "companyName";
        string Col_4 = "companyAddress";
        string Col_5 = "cardType";
        string Col_6 = "cardIssuingMemberState";
        string Col_7 = "companyCardNumber";

        DateTime date = new DateTime();
        activityTable.Columns.Add(new DataColumn(Col_1, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_2, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_3, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_4, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_5, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_6, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_7, typeof(string)));

        foreach (DDDClass.VuCompanyLocksRecord rec in locks)
        {
            date = rec.lockInTime.getTimeRealDate();
            dr = activityTable.NewRow();
            dr[Col_1] = date.ToShortDateString() + " " + date.ToLongTimeString();
            date = rec.lockOutTime.getTimeRealDate();
            dr[Col_2] = date.ToShortDateString() + " " + date.ToLongTimeString();
            dr[Col_3] = rec.companyName.ToString();
            dr[Col_4] = rec.companyAddress.ToString();
            dr[Col_5] = rec.companyCardNumber.cardType.ToString();
            dr[Col_6] = rec.companyCardNumber.cardIssuingMemberState.ToString();
            dr[Col_7] = rec.companyCardNumber.cardNumber.ToString();
            activityTable.Rows.Add(dr);
        }
        return activityTable;
    }
    public static DataTable VehiclePreview_ControlActivityData(List<DDDClass.VuControlActivityRecord> records)
    {
        DataTable activityTable = new DataTable(Guid.NewGuid().ToString());
        DataRow dr;
        string Col_1 = "controlType";
        string Col_2 = "controlTime";
        string Col_3 = "cardType";
        string Col_4 = "cardIssuingMemberState";
        string Col_5 = "cardNumber";
        string Col_6 = "downloadPeriodBeginTime";
        string Col_7 = "downloadPeriodEndTime";

        DateTime date  = new DateTime();
        activityTable.Columns.Add(new DataColumn(Col_1, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_2, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_3, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_4, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_5, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_6, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_7, typeof(string)));

        foreach (DDDClass.VuControlActivityRecord rec in records)
        {
            dr = activityTable.NewRow();
            dr[Col_1] = rec.controlType.value.ToString();
            date = rec.controlTime.getTimeRealDate();
            dr[Col_2] = date.ToShortDateString()+" "+date.ToLongTimeString();
            dr[Col_3] = rec.controlCardNumber.cardType.ToString();
            dr[Col_4] = rec.controlCardNumber.cardIssuingMemberState.ToString();
            dr[Col_5] = rec.controlCardNumber.cardNumber.ToString();
            date = rec.downloadPeriodBeginTime.getTimeRealDate();
            dr[Col_6] = date.ToShortDateString() + " " + date.ToLongTimeString();
            date = rec.downloadPeriodEndTime.getTimeRealDate();
            dr[Col_7] = date.ToShortDateString() + " " + date.ToLongTimeString();
            activityTable.Rows.Add(dr);
        }
        return activityTable;
    }
    public static DataTable VehiclePreview_EventData(List<DDDClass.VuEventRecord> events)
    {
        DataTable activityTable = new DataTable(Guid.NewGuid().ToString());
        DataRow dr;
        string Col_1 = "eventType";
        string Col_2 = "eventRecordPurpose";
        string Col_3 = "eventBeginTime";
        string Col_4 = "eventEndTime";
        string Col_5 = "driverSlotBegin";
        string Col_6 = "coDriverSlotBegin";
        string Col_7 = "driverSlotEnd";
        string Col_8 = "coDriverSlotEnd";
        string Col_9 = "similarEventsNumber";

        DateTime date = new DateTime();
        activityTable.Columns.Add(new DataColumn(Col_1, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_2, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_3, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_4, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_5, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_6, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_7, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_8, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_9, typeof(string)));

        foreach (DDDClass.VuEventRecord rec in events)
        {
            dr = activityTable.NewRow();
            dr[Col_1] = rec.eventType.ToString();
            dr[Col_2] = rec.eventRecordPurpose.ToString();
            date = rec.eventBeginTime.getTimeRealDate();
            dr[Col_3] = date.ToShortDateString() + " " + date.ToLongTimeString();
            date = rec.eventEndTime.getTimeRealDate();
            dr[Col_4] = date.ToShortDateString() + " " + date.ToLongTimeString();
            dr[Col_5] = rec.cardNumberDriverSlotBegin.cardNumber.ToString().Replace("?","");
            dr[Col_6] = rec.cardNumberCodriverSlotBegin.cardNumber.ToString().Replace("?", "");
            dr[Col_7] = rec.cardNumberDriverSlotEnd.cardNumber.ToString().Replace("?", "");
            dr[Col_8] = rec.cardNumberCodriverSlotEnd.cardNumber.ToString().Replace("?", "");
            dr[Col_9] = rec.similarEventsNumber.similarEventsNumber.ToString();
            activityTable.Rows.Add(dr);
        }
        return activityTable;
    }
    public static DataTable VehiclePreview_FaultData(List<DDDClass.VuFaultRecord> faults)
    {
        DataTable activityTable = new DataTable(Guid.NewGuid().ToString());
        DataRow dr;
        string Col_1 = "faultType";
        string Col_2 = "faultRecordPurpose";
        string Col_3 = "faultBeginTime";
        string Col_4 = "faultEndTime";
        string Col_5 = "DriverSlotBegin";
        string Col_6 = "coDriverSlotBegin";
        string Col_7 = "driverSlotEnd";
        string Col_8 = "coDriverSlotEnd";

        DateTime date = new DateTime();
        activityTable.Columns.Add(new DataColumn(Col_1, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_2, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_3, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_4, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_5, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_6, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_7, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_8, typeof(string)));

        foreach (DDDClass.VuFaultRecord rec in faults)
        {
            dr = activityTable.NewRow();
            dr[Col_1] = rec.faultType.ToString();
            dr[Col_2] = rec.faultRecordPurpose.ToString();
            date = rec.faultBeginTime.getTimeRealDate();
            dr[Col_3] = date.ToShortDateString() + " " + date.ToLongTimeString();
            date = rec.faultEndTime.getTimeRealDate();
            dr[Col_4] = date.ToShortDateString() + " " + date.ToLongTimeString();
            dr[Col_5] = rec.cardNumberDriverSlotBegin.cardNumber.ToString();
            dr[Col_6] = rec.cardNumberCodriverSlotBegin.cardNumber.ToString();
            dr[Col_7] = rec.cardNumberDriverSlotEnd.cardNumber.ToString();
            dr[Col_8] = rec.cardNumberCodriverSlotEnd.cardNumber.ToString();
            activityTable.Rows.Add(dr);
        }
        return activityTable;
    }
    public static DataTable VehiclePreview_OverspeedingControlData(DDDClass.VuOverSpeedingControlData control)
    {
        DataTable activityTable = new DataTable(Guid.NewGuid().ToString());
        DataRow dr;
        string Col_1 = "lastOverspeedControlTime";
        string Col_2 = "firstOverspeedSince";
        string Col_3 = "numberOfOverspeedSince";

        DateTime date = new DateTime();
        activityTable.Columns.Add(new DataColumn(Col_1, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_2, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_3, typeof(string)));

            dr = activityTable.NewRow();

            date = control.lastOverspeedControlTime.getTimeRealDate();
            dr[Col_1] = date.ToShortDateString() + " " + date.ToLongTimeString();
            date =  control.firstOverspeedSince.getTimeRealDate();
            dr[Col_2] = date.ToShortDateString() + " " + date.ToLongTimeString();
            dr[Col_3] = control.numberOfOverspeedSince.ToString();

            activityTable.Rows.Add(dr);
        
        return activityTable;
    }
    public static DataTable VehiclePreview_OverspeedingEventData(List<DDDClass.VuOverSpeedingEventRecord> events)
    {
        DataTable activityTable = new DataTable(Guid.NewGuid().ToString());
        DataRow dr;
        string Col_1 = "eventType";
        string Col_2 = "eventRecordPurpose";
        string Col_3 = "eventBeginTime";
        string Col_4 = "eventEndTime";
        string Col_5 = "maxSpeedValue";
        string Col_6 = "averageSpeedValue";
        string Col_7 = "driverSlotBegin";
        string Col_8 = "similarEventsNumber";

        DateTime date = new DateTime();
        activityTable.Columns.Add(new DataColumn(Col_1, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_2, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_3, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_4, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_5, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_6, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_7, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_8, typeof(string)));

        foreach (DDDClass.VuOverSpeedingEventRecord rec in events)
        {
            dr = activityTable.NewRow();
            dr[Col_1] = rec.eventType.ToString();
            dr[Col_2] = rec.eventRecordPurpose.ToString();
            date = rec.eventBeginTime.getTimeRealDate();
            dr[Col_3] = date.ToShortDateString() + " " + date.ToLongTimeString();
            date = rec.eventEndTime.getTimeRealDate();
            dr[Col_4] = date.ToShortDateString() + " " + date.ToLongTimeString();
            dr[Col_5] = rec.maxSpeedValue.ToString();
            dr[Col_6] = rec.averageSpeedValue.ToString();
            dr[Col_7] = rec.cardNumberDriverSlotBegin.cardNumber.ToString();
            dr[Col_8] = rec.similarEventsNumber.ToString();
            activityTable.Rows.Add(dr);
        }
        return activityTable;
    }
    public static DataTable VehiclePreview_TimeAdjustmentData(List<DDDClass.VuTimeAdjustmentRecord> records)
    {
        DataTable activityTable = new DataTable(Guid.NewGuid().ToString());
        DataRow dr;
        string Col_1 = "oldTimeValue";
        string Col_2 = "newTimeValue";
        string Col_3 = "workshopName";
        string Col_4 = "workshopAddress";
        string Col_5 = "workshopCardNumber";

        DateTime date = new DateTime();
        activityTable.Columns.Add(new DataColumn(Col_1, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_2, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_3, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_4, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_5, typeof(string)));

        foreach (DDDClass.VuTimeAdjustmentRecord rec in records)
        {
            dr = activityTable.NewRow();
            date = rec.oldTimeValue.getTimeRealDate();
            dr[Col_1] = date.ToShortDateString() + " " + date.ToLongTimeString();
            date = rec.newTimeValue.getTimeRealDate();
            dr[Col_2] = date.ToShortDateString() + " " + date.ToLongTimeString();
            dr[Col_3] = rec.workshopName.ToString();
            dr[Col_4] = rec.workshopAddress.ToString();
            dr[Col_5] = rec.workshopCardNumber.cardNumber.ToString();
            activityTable.Rows.Add(dr);
        }
        return activityTable;
    }
    public static DataTable VehiclePreview_DetailedSpeedData(List<DDDClass.VuDetailedSpeedBlock> records)
    {
        DataTable activityTable = new DataTable(Guid.NewGuid().ToString());
        DataRow dr;
        string Col_1 = "speedBlockBeginDate";
        string Col_2 = "speedsPerSecond";

        DateTime date = new DateTime();
        activityTable.Columns.Add(new DataColumn(Col_1, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_2, typeof(string)));
        string speedString = "";
        foreach (DDDClass.VuDetailedSpeedBlock rec in records)
        {
            speedString = "";
            dr = activityTable.NewRow();
            date = rec.speedBlockBeginDate.getTimeRealDate();
            dr[Col_1] = date.ToShortDateString() + " " + date.ToLongTimeString();
            foreach(DDDClass.Speed speed in rec.speedsPerSecond)
            {
                speedString+= speed.ToString()+", ";
            }
            dr[Col_2] = speedString.TrimEnd(new char[]{' ', ','});
            activityTable.Rows.Add(dr);
        }
        return activityTable;
    }
    public static DataTable VehiclePreview_VuIdentification(DDDClass.VuIdentification ident)
    {
        DataTable activityTable = new DataTable(Guid.NewGuid().ToString());
        DataRow dr;
        string Col_1 = "description";
        string Col_2 = "value";

        DateTime date = new DateTime();
        activityTable.Columns.Add(new DataColumn(Col_1, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_2, typeof(string)));

        dr = activityTable.NewRow();
        dr[Col_1] = "vuManufacturerName";
        dr[Col_2] = ident.vuManufacturerName.ToString();
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "vuManufacturerAddress";
        dr[Col_2] = ident.vuManufacturerAddress.ToString();
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "vuPartNumber";
        dr[Col_2] = ident.vuPartNumber.ToString();
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "vuSerialNumber";
        dr[Col_2] = ident.vuSerialNumber.serialNumber.ToString();
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "monthYear";
        dr[Col_2] = ident.vuSerialNumber.monthYear;
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "type";
        dr[Col_2] = "Vehicle Unit";
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "manufacturerCode";
        dr[Col_2] = ident.vuSerialNumber.manufacturerCode.ToString();
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "vuSoftwareVersion";
        dr[Col_2] = ident.vuSoftwareIdentification.vuSoftwareVersion.ToString();
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "vuSoftInstallationDate";
        date = ident.vuSoftwareIdentification.vuSoftInstallationDate.getTimeRealDate();
        dr[Col_2] = date.ToShortDateString() + " " + date.ToLongTimeString();
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "vuManufacturingDate";
        date = ident.vuManufacturingDate.getTimeRealDate();
        dr[Col_2] = date.ToShortDateString();
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "vuApprovalNumber";
        dr[Col_2] = ident.vuApprovalNumber.ToString();
        activityTable.Rows.Add(dr);

        return activityTable;
    }
    public static DataTable VehiclePreview_SensorPaired(DDDClass.SensorPaired sensorPaired)
    {
        DataTable activityTable = new DataTable(Guid.NewGuid().ToString());
        DataRow dr;
        string Col_1 = "description";
        string Col_2 = "value";

        DateTime date = new DateTime();
        activityTable.Columns.Add(new DataColumn(Col_1, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_2, typeof(string)));

        dr = activityTable.NewRow();
        dr[Col_1] = "sensorSerialNumber";
        dr[Col_2] = sensorPaired.sensorSerialNumber.serialNumber.ToString();
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "monthYear";
        dr[Col_2] = sensorPaired.sensorSerialNumber.monthYear;
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "type";
        dr[Col_2] = "Motion Sensor";
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "sensorApprovalNumber";
        dr[Col_2] = sensorPaired.sensorApprovalNumber.ToString();
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "sensorPairingDateFirst";
        date = sensorPaired.sensorPairingDateFirst.getTimeRealDate();
        dr[Col_2] = date.ToShortDateString() + " " + date.ToLongTimeString();
        activityTable.Rows.Add(dr);

        return activityTable;
    }
    public static DataTable VehiclePreview_CalibrationData(List<DDDClass.VuCalibrationRecord> records)
    {
        DataTable activityTable = new DataTable(Guid.NewGuid().ToString());
        DataRow dr;
        string Col_1 = "calibrationPurpose";
        string Col_2 = "workshopName";
        string Col_3 = "workshopAddress";
        string Col_4 = "workshopCardNumber";
        string Col_5 = "workshopCardExpiryDate";
        string Col_6 = "vehicleIdentificationNumber";
        string Col_7 = "vehicleRegistrationNation";
        string Col_8 = "vehicleRegistrationNumber";
        string Col_9 = "wVehicleCharConstant";
        string Col_10 = "kConstantOfRecordingEquip";
        string Col_11 = "ITyreCircum.";
        string Col_12 = "tyreSize";
        string Col_13 = "authorSpeed";
        string Col_14 = "oldOdVal";
        string Col_15 = "newOdVal";
        string Col_16 = "oldTimeVal";
        string Col_17 = "newTimeVal";
        string Col_18 = "nextCalibDate";

        activityTable.Columns.Add(new DataColumn(Col_1, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_2, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_3, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_4, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_5, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_6, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_7, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_8, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_9, typeof(int)));
        activityTable.Columns.Add(new DataColumn(Col_10, typeof(int)));
        activityTable.Columns.Add(new DataColumn(Col_11, typeof(int)));
        activityTable.Columns.Add(new DataColumn(Col_12, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_13, typeof(int)));
        activityTable.Columns.Add(new DataColumn(Col_14, typeof(int)));
        activityTable.Columns.Add(new DataColumn(Col_15, typeof(int)));
        activityTable.Columns.Add(new DataColumn(Col_16, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_17, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_18, typeof(string)));

        DateTime date = new DateTime();
        foreach (DDDClass.VuCalibrationRecord rec in records)
        {

            dr = activityTable.NewRow();
            dr[Col_1] = rec.calibrationPurpose.ToString();
            dr[Col_2] = rec.workshopName.ToString();
            dr[Col_3] = rec.workshopAddress.ToString();
            dr[Col_4] = rec.workshopCardNumber.cardNumber.ToString();

            date = rec.workshopCardExpiryDate.getTimeRealDate();
            dr[Col_5] = date.ToShortDateString() + " " + date.ToLongTimeString();
            dr[Col_6] = rec.vehicleIdentificationNumber.ToString();
            dr[Col_7] = rec.vehicleRegistrationIdentification.vehicleRegistrationNation.ToString();
            dr[Col_8] = rec.vehicleRegistrationIdentification.vehicleRegistrationNumber.ToString();
            dr[Col_9] = rec.wVehicleCharacteristicConstant.wVehicleCharacteristicConstant;
            dr[Col_10] = rec.kConstantOfRecordingEquipment.kConstantOfRecordingEquipment;
            dr[Col_11] = rec.lTyreCircumference.lTyreCircumference;
            dr[Col_12] = rec.tyreSize.ToString();
            dr[Col_13] = rec.authorisedSpeed.speed;
            dr[Col_14] = rec.oldOdometerValue.odometerShort;
            dr[Col_15] = rec.newOdometerValue.odometerShort;

            date = rec.oldTimeValue.getTimeRealDate();
            dr[Col_16] = date.ToShortDateString() + " " + date.ToLongTimeString();
            date = rec.newTimeValue.getTimeRealDate();
            dr[Col_17] = date.ToShortDateString() + " " + date.ToLongTimeString();
            date = rec.nextCalibrationDate.getTimeRealDate();
            dr[Col_18] = date.ToShortDateString() + " " + date.ToLongTimeString();
            activityTable.Rows.Add(dr);
        }

        return activityTable;
    }

}
