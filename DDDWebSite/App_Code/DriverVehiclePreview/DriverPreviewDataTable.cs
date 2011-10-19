using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CardUnit;
using System.Data;

/// <summary>
/// Summary description for DriverPreviewDataTable
/// </summary>
public class DriverPreviewDataTable
{
	public DriverPreviewDataTable()
	{
	}

    public static DataTable DriverPreview_FileContents(CardUnitClass card)
    {
        DataTable activityTable = new DataTable(Guid.NewGuid().ToString());
        DataRow dr;
        string Col_1 = "Name";
        string Col_2 = "Length";

        activityTable.Columns.Add(new DataColumn(Col_1, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_2, typeof(int)));

        dr = activityTable.NewRow();
        dr[Col_1] = "ICC";
        dr[Col_2] = 25;
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "IC";
        dr[Col_2] = 8;
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "Application Identification";
        dr[Col_2] = card.ef_application_identification.activityStructureLength;
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "Signature";
        dr[Col_2] = 128;
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "Events Data";
        dr[Col_2] = card.ef_events_data.cardEventData.structureSize;
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "Signature";
        dr[Col_2] = 128;
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "Faults Data";
        dr[Col_2] = card.ef_faults_data.cardFaultData.structureSize;
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "Signature";
        dr[Col_2] = 128;
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "Driver Activity Data";
        dr[Col_2] =card.ef_driver_activity_data.cardDriverActivity.structureSize;
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "Signature";
        dr[Col_2] = 128;
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "Vehicles Used";
        dr[Col_2] = card.ef_vehicles_used.structureSize;
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "Signature";
        dr[Col_2] = 128;
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "Places";
        dr[Col_2] = card.ef_places.cardPlaceDailyWorkPeriod.structureSize;
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "Signature";
        dr[Col_2] = 128;
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "Current Usage";
        dr[Col_2] = 19;
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "Signature";
        dr[Col_2] = 128;
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "Control Activity Data";
        dr[Col_2] = 46;
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "Card Download";
        dr[Col_2] = 4;
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "Identification";
        dr[Col_2] = card.ef_identification.structureSize;
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "Signature";
        dr[Col_2] = 128;
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "Driving Licence Info";
        dr[Col_2] = 53;
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "Signature";
        dr[Col_2] = 128;
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "Specific Conditions";
        dr[Col_2] = card.ef_specific_conditions.structureSize;
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "Signature";
        dr[Col_2] = 128;
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "Card Certificate";
        dr[Col_2] = 194;
        activityTable.Rows.Add(dr);

        dr = activityTable.NewRow();
        dr[Col_1] = "CA Certificate";
        dr[Col_2] = 194;
        activityTable.Rows.Add(dr);

        return activityTable;
    }
    public static DataTable DriverPreview_ICC(DDDClass.CardIccIdentification icc)
    {

        DataTable dataTable = new DataTable(Guid.NewGuid().ToString());
        DataRow dr;
        string Col_1 = "Description";
        string Col_2 = "Value";

        dataTable.Columns.Add(new DataColumn(Col_1, typeof(string)));
        dataTable.Columns.Add(new DataColumn(Col_2, typeof(string)));

        dr = dataTable.NewRow();
        dr[Col_1] = "clockstop";
        dr[Col_2] = icc.ClockStop_ToString();
        dataTable.Rows.Add(dr);

        dr = dataTable.NewRow();
        dr[Col_1] = "serialNumber";
        dr[Col_2] = icc.cardExtendedSerialNumber.serialNumber.ToString();
        dataTable.Rows.Add(dr);

        dr = dataTable.NewRow();
        dr[Col_1] = "monthYear";
        dr[Col_2] = icc.cardExtendedSerialNumber.monthYear.ToString();
        dataTable.Rows.Add(dr);

        dr = dataTable.NewRow();
        dr[Col_1] = "type";
        dr[Col_2] = icc.cardExtendedSerialNumber.type.ToString();
        dataTable.Rows.Add(dr);

        dr = dataTable.NewRow();
        dr[Col_1] = "manufacturerCode";
        dr[Col_2] = icc.cardExtendedSerialNumber.manufacturerCode.manufacturerCode.ToString() + " (" + icc.cardExtendedSerialNumber.manufacturerCode.ToString() + ")";
        dataTable.Rows.Add(dr);

        dr = dataTable.NewRow();
        dr[Col_1] = "cardApprovalNumber";
        dr[Col_2] = icc.cardApprovalNumber.ToString();
        dataTable.Rows.Add(dr);

        dr = dataTable.NewRow();
        dr[Col_1] = "cardPersonaliserID";
        dr[Col_2] = icc.CardPersonaliserID_ToString();
        dataTable.Rows.Add(dr);

        dr = dataTable.NewRow();
        dr[Col_1] = "embedderIcAssemblerID";
        dr[Col_2] = icc.EmbedderIcAssemblerId_ToString();
        dataTable.Rows.Add(dr);

        dr = dataTable.NewRow();
        dr[Col_1] = "icIdentifier";
        dr[Col_2] = icc.IcIdentifier_ToString();
        dataTable.Rows.Add(dr);

        return dataTable;
    }
    public static DataTable DriverPreview_IC(DDDClass.CardChipIdentification ic)
    {

        DataTable dataTable = new DataTable(Guid.NewGuid().ToString());
        DataRow dr;
        string Col_1 = "Description";
        string Col_2 = "Value";

        dataTable.Columns.Add(new DataColumn(Col_1, typeof(string)));
        dataTable.Columns.Add(new DataColumn(Col_2, typeof(string)));

        dr = dataTable.NewRow();
        dr[Col_1] = "icSerialNumber";
        dr[Col_2] = ic.icSerialNumber.ToString();
        dataTable.Rows.Add(dr);

        dr = dataTable.NewRow();
        dr[Col_1] = "icManufacturingReferences";
        dr[Col_2] = ic.icManufacturingReferences.ToString();
        dataTable.Rows.Add(dr);

        return dataTable;
    }
    public static DataTable DriverPreview_AplicationIdentification(object cardObject, DDDClass.EquipmentType cardType)
    {

        DataTable dataTable = new DataTable(Guid.NewGuid().ToString());
        DataRow dr;
        string Col_1 = "Description";
        string Col_2 = "Value";

        dataTable.Columns.Add(new DataColumn(Col_1, typeof(string)));
        dataTable.Columns.Add(new DataColumn(Col_2, typeof(string)));

        switch (cardType.equipmentType)
        {
            case 1:
                {
                    DDDClass.DriverCardApplicationIdentification driversCard = (DDDClass.DriverCardApplicationIdentification)cardObject;
                    dr = dataTable.NewRow();
                    dr[Col_1] = "typeOfTachographCardID";
                    dr[Col_2] = driversCard.typeOfTachographCardId.ToString();
                    dataTable.Rows.Add(dr);

                    dr = dataTable.NewRow();
                    dr[Col_1] = "cardStructureVersion";
                    dr[Col_2] = driversCard.cardStructureVersion.ToString();
                    dataTable.Rows.Add(dr);

                    dr = dataTable.NewRow();
                    dr[Col_1] = "noOfEventsPerType";
                    dr[Col_2] = driversCard.noOfEventsPerType.ToString();
                    dataTable.Rows.Add(dr);

                    dr = dataTable.NewRow();
                    dr[Col_1] = "noOfFaultsPerType";
                    dr[Col_2] = driversCard.noOfFaultsPerType.ToString();
                    dataTable.Rows.Add(dr);

                    dr = dataTable.NewRow();
                    dr[Col_1] = "activityStructureLength";
                    dr[Col_2] = driversCard.activityStructureLength.ToString();
                    dataTable.Rows.Add(dr);

                    dr = dataTable.NewRow();
                    dr[Col_1] = "noOfCardVehicleRecords";
                    dr[Col_2] = driversCard.noOfCardVehicleRecords.ToString();
                    dataTable.Rows.Add(dr);

                    dr = dataTable.NewRow();
                    dr[Col_1] = "noOfCardPlaceRecords";
                    dr[Col_2] = driversCard.noOfCardPlaceRecords.ToString();
                    dataTable.Rows.Add(dr);
                } break;
            case 2:
                {
                    DDDClass.WorkshopCardApplicationIdentification workshopCard = (DDDClass.WorkshopCardApplicationIdentification)cardObject;
                    dr = dataTable.NewRow();
                    dr[Col_1] = "typeOfTachographCardID";
                    dr[Col_2] = workshopCard.typeOfTachographCardId.ToString();
                    dataTable.Rows.Add(dr);

                    dr = dataTable.NewRow();
                    dr[Col_1] = "cardStructureVersion";
                    dr[Col_2] = workshopCard.cardStructureVersion.ToString();
                    dataTable.Rows.Add(dr);

                    dr = dataTable.NewRow();
                    dr[Col_1] = "noOfEventsPerType";
                    dr[Col_2] = workshopCard.noOfEventsPerType.ToString();
                    dataTable.Rows.Add(dr);

                    dr = dataTable.NewRow();
                    dr[Col_1] = "noOfFaultsPerType";
                    dr[Col_2] = workshopCard.noOfFaultsPerType.ToString();
                    dataTable.Rows.Add(dr);

                    dr = dataTable.NewRow();
                    dr[Col_1] = "activityStructureLength";
                    dr[Col_2] = workshopCard.activityStructureLength.ToString();
                    dataTable.Rows.Add(dr);

                    dr = dataTable.NewRow();
                    dr[Col_1] = "noOfCardVehicleRecords";
                    dr[Col_2] = workshopCard.noOfCardVehicleRecords.ToString();
                    dataTable.Rows.Add(dr);

                    dr = dataTable.NewRow();
                    dr[Col_1] = "noOfCardPlaceRecords";
                    dr[Col_2] = workshopCard.noOfCardPlaceRecords.ToString();
                    dataTable.Rows.Add(dr);
                } break;
            case 3:
                {
                } break;
            case 4:
                {
                } break;
        }

        return dataTable;
    }
    public static DataTable DriverPreview_CardIdentification(DDDClass.CardIdentification data, object cardObject, DDDClass.EquipmentType cardType)
    {

        DataTable dataTable = new DataTable(Guid.NewGuid().ToString());
        DataRow dr;
        string Col_1 = "Description";
        string Col_2 = "Value";

        dataTable.Columns.Add(new DataColumn(Col_1, typeof(string)));
        dataTable.Columns.Add(new DataColumn(Col_2, typeof(string)));

        dr = dataTable.NewRow();
        dr[Col_1] = "cardIssuingMemberState";
        dr[Col_2] = data.cardIssuingMemberState.ToString();
        dataTable.Rows.Add(dr);

        dr = dataTable.NewRow();
        dr[Col_1] = "cardNumber";
        dr[Col_2] = data.cardNumber.driverIdentificationNumber();
        dataTable.Rows.Add(dr);

        dr = dataTable.NewRow();
        dr[Col_1] = "cardIssuingAuthorityName";
        dr[Col_2] = data.cardIssuingAuthorityName.ToString();
        dataTable.Rows.Add(dr);

        dr = dataTable.NewRow();
        dr[Col_1] = "cardIssueDate";
        dr[Col_2] = data.cardIssueDate.getTimeRealDate().ToLongDateString();
        dataTable.Rows.Add(dr);

        dr = dataTable.NewRow();
        dr[Col_1] = "cardValidityBegin";
        dr[Col_2] = data.cardValidityBegin.getTimeRealDate().ToLongDateString();
        dataTable.Rows.Add(dr);

        dr = dataTable.NewRow();
        dr[Col_1] = "cardExpiryBegin";
        dr[Col_2] = data.cardExpiryDate.getTimeRealDate().ToLongDateString();
        dataTable.Rows.Add(dr);


        switch (cardType.equipmentType)
        {
            case 1:
                {
                    DDDClass.DriverCardHolderIdentification driversCard = (DDDClass.DriverCardHolderIdentification)cardObject;

                    dr = dataTable.NewRow();
                    dr[Col_1] = "cardHolderSurname";
                    dr[Col_2] = driversCard.cardHolderName.holderSurname.ToString();
                    dataTable.Rows.Add(dr);

                    dr = dataTable.NewRow();
                    dr[Col_1] = "cardHolderFirstNames";
                    dr[Col_2] = driversCard.cardHolderName.holderFirstNames.ToString();
                    dataTable.Rows.Add(dr);

                    dr = dataTable.NewRow();
                    dr[Col_1] = "cardHolderBirthDate";
                    dr[Col_2] = driversCard.cardHolderBirthDate.GetDateTime().ToLongDateString();
                    dataTable.Rows.Add(dr);

                    dr = dataTable.NewRow();
                    dr[Col_1] = "cardHolderPreferredLanguage";
                    dr[Col_2] = driversCard.cardHolderPreferredLanguage.ToString();
                    dataTable.Rows.Add(dr);
                } break;
            case 2:
                {
                    DDDClass.WorkshopCardHolderIdentification workshopCard = (DDDClass.WorkshopCardHolderIdentification)cardObject;

                    dr = dataTable.NewRow();
                    dr[Col_1] = "workshopName";
                    dr[Col_2] = workshopCard.workshopName.ToString();
                    dataTable.Rows.Add(dr);

                    dr = dataTable.NewRow();
                    dr[Col_1] = "workshopAddress";
                    dr[Col_2] = workshopCard.workshopAddress.ToString();
                    dataTable.Rows.Add(dr);

                    dr = dataTable.NewRow();
                    dr[Col_1] = "cardHolderName";
                    dr[Col_2] = workshopCard.cardHolderName.ToString();
                    dataTable.Rows.Add(dr);

                    dr = dataTable.NewRow();
                    dr[Col_1] = "cardHolderPreferredLanguage";
                    dr[Col_2] = workshopCard.cardHolderPreferredLanguage.ToString();
                    dataTable.Rows.Add(dr);
                } break;
            case 3:
                {
                } break;
            case 4:
                {
                } break;
        }

        return dataTable;
    }
    public static DataTable DriverPreview_DrivingLicenceInfo(CardUnit.EF_Driving_Licence_Info data)
    {

        DataTable dataTable = new DataTable(Guid.NewGuid().ToString());
        DataRow dr;
        string Col_1 = "Description";
        string Col_2 = "Value";

        dataTable.Columns.Add(new DataColumn(Col_1, typeof(string)));
        dataTable.Columns.Add(new DataColumn(Col_2, typeof(string)));

        dr = dataTable.NewRow();
        dr[Col_1] = "drivingLicenceIssuingAuthority";
        dr[Col_2] = data.cardDrivingLicenceInformation.drivingLicenceIssuingAuthority.ToString();
        dataTable.Rows.Add(dr);

        dr = dataTable.NewRow();
        dr[Col_1] = "drivingLicenceIssuingNation";
        dr[Col_2] = data.cardDrivingLicenceInformation.drivingLicenceIssuingNation.ToString();
        dataTable.Rows.Add(dr);

        dr = dataTable.NewRow();
        dr[Col_1] = "drivingLicenceNumber";
        dr[Col_2] = data.cardDrivingLicenceInformation.drivingLicenceNumber.ToString();
        dataTable.Rows.Add(dr);

        return dataTable;
    }
    public static DataTable DriverPreview_DrivingLicenceInfo(DDDClass.CardPlaceDailyWorkPeriod data)
    {
        DataTable dataTable = new DataTable(Guid.NewGuid().ToString());
        DataRow dr;
        string Col_1 = "entry Time";
        string Col_2 = "entryTypeDailyWorkPeriod";
        string Col_3 = "dailyWorkPeriodCountry";
        string Col_4 = "dailyWorkPeriodRegion";
        string Col_5 = "vehicleOdometerValue";

        dataTable.Columns.Add(new DataColumn(Col_1, typeof(string)));
        dataTable.Columns.Add(new DataColumn(Col_2, typeof(string)));
        dataTable.Columns.Add(new DataColumn(Col_3, typeof(string)));
        dataTable.Columns.Add(new DataColumn(Col_4, typeof(string)));
        dataTable.Columns.Add(new DataColumn(Col_5, typeof(string)));
        DateTime date = new DateTime();
        foreach (DDDClass.PlaceRecord record in data.placeRecords)
        {
            dr = dataTable.NewRow();
            date = record.entryTime.getTimeRealDate();
            dr[Col_1] = date.ToShortDateString()+" "+date.ToLongTimeString();
            dr[Col_2] = record.entryTypeDailyWorkPeriod.ToString();
            dr[Col_3] = record.dailyWorkPeriodCountry.ToString();
            dr[Col_4] = record.dailyWorkPeriodRegion.ToString();
            dr[Col_5] = record.vehicleOdometerValue.ToString();
            dataTable.Rows.Add(dr);
        }
        return dataTable;
    }
}
