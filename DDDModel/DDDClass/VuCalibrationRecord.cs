using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class VuCalibrationRecord//167bytes
    {
        public static int structureSize = 167;

        public CalibrationPurpose calibrationPurpose { get; set; }
        public Name workshopName { get; set; }
        public Address workshopAddress { get; set; }
        public FullCardNumber workshopCardNumber { get; set; }
        public TimeReal workshopCardExpiryDate { get; set; }
        public VehicleIdentificationNumber vehicleIdentificationNumber { get; set; }
        public VehicleRegistrationIdentification vehicleRegistrationIdentification { get; set; }
        public W_VehicleCharacteristicConstant wVehicleCharacteristicConstant { get; set; }
        public K_ConstantOfRecordingEquipment kConstantOfRecordingEquipment { get; set; }
        public L_TyreCircumference lTyreCircumference { get; set; }
        public TyreSize tyreSize { get; set; }
        public SpeedAuthorised authorisedSpeed { get; set; }
        public OdometerShort oldOdometerValue { get; set; }
        public OdometerShort newOdometerValue { get; set; }
        public TimeReal oldTimeValue { get; set; }
        public TimeReal newTimeValue { get; set; }
        public TimeReal nextCalibrationDate { get; set; }

        public VuCalibrationRecord()
        {
            calibrationPurpose = new CalibrationPurpose();
            workshopName = new Name();
            workshopAddress = new Address();
            workshopCardNumber = new FullCardNumber();
            workshopCardExpiryDate = new TimeReal();
            vehicleIdentificationNumber = new VehicleIdentificationNumber();
            vehicleRegistrationIdentification = new VehicleRegistrationIdentification();
            wVehicleCharacteristicConstant = new W_VehicleCharacteristicConstant();
            kConstantOfRecordingEquipment = new K_ConstantOfRecordingEquipment();
            lTyreCircumference = new L_TyreCircumference();
            tyreSize = new TyreSize();
            authorisedSpeed = new SpeedAuthorised();
            oldOdometerValue = new OdometerShort();
            newOdometerValue = new OdometerShort();
            oldTimeValue = new TimeReal();
            newTimeValue = new TimeReal();
            nextCalibrationDate = new TimeReal();
        }

        public VuCalibrationRecord(byte[] value)
        {
            calibrationPurpose = new CalibrationPurpose(value[0]);
            workshopName = new Name(ConvertionClass.arrayCopy(value, 1, 36));
            workshopAddress = new Address(ConvertionClass.arrayCopy(value, 37, 36));
            workshopCardNumber = new FullCardNumber(ConvertionClass.arrayCopy(value, 73, 18));
            workshopCardExpiryDate = new TimeReal(ConvertionClass.arrayCopy(value, 91, 4));
            vehicleIdentificationNumber = new VehicleIdentificationNumber(ConvertionClass.arrayCopy(value, 95, 17));
            vehicleRegistrationIdentification = new VehicleRegistrationIdentification(ConvertionClass.arrayCopy(value, 112, 15));
            wVehicleCharacteristicConstant = new W_VehicleCharacteristicConstant(ConvertionClass.arrayCopy(value, 127, 2));
            kConstantOfRecordingEquipment = new K_ConstantOfRecordingEquipment(ConvertionClass.arrayCopy(value, 129, 2));
            lTyreCircumference = new L_TyreCircumference(ConvertionClass.arrayCopy(value, 131, 2));
            tyreSize = new TyreSize(ConvertionClass.arrayCopy(value, 133, 15));
            authorisedSpeed = new SpeedAuthorised(value[148]);
            oldOdometerValue = new OdometerShort(ConvertionClass.arrayCopy(value, 149, 3));
            newOdometerValue = new OdometerShort(ConvertionClass.arrayCopy(value, 152, 3));
            oldTimeValue = new TimeReal(ConvertionClass.arrayCopy(value, 155, 4));
            newTimeValue = new TimeReal(ConvertionClass.arrayCopy(value, 159, 4));
            nextCalibrationDate = new TimeReal(ConvertionClass.arrayCopy(value, 163, 4));
        }
    }
}
