using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class WorkshopCardCalibrationRecord
    {
        public static int structureSize = 105;

        public CalibrationPurpose calibrationPurpose { get; set; }
        public VehicleIdentificationNumber vehicleIdentificationNumber { get; set; }
        public VehicleRegistrationIdentification vehicleRegistration { get; set; }
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
        public VuPartNumber vuPartNumber { get; set; }
        public ExtendedSerialNumber vuSerialNumber { get; set; }
        public ExtendedSerialNumber sensorSerialNumber { get; set; }


        public WorkshopCardCalibrationRecord()
        {
            calibrationPurpose = new CalibrationPurpose();
            vehicleIdentificationNumber = new VehicleIdentificationNumber();
            vehicleRegistration = new VehicleRegistrationIdentification();
            wVehicleCharacteristicConstant = new W_VehicleCharacteristicConstant();
            kConstantOfRecordingEquipment = new K_ConstantOfRecordingEquipment();
            lTyreCircumference = new L_TyreCircumference();
            tyreSize = new TyreSize();
            authorisedSpeed = new SpeedAuthorised();
            oldOdometerValue = new OdometerShort();
            newOdometerValue = new OdometerShort();
            oldTimeValue = new TimeReal();
            newTimeValue = new TimeReal();
            vuPartNumber = new VuPartNumber();
            vuSerialNumber = new ExtendedSerialNumber();
            sensorSerialNumber = new ExtendedSerialNumber();
        }

        public WorkshopCardCalibrationRecord(byte[] value)
        {
            calibrationPurpose = new CalibrationPurpose(value[0]);
            vehicleIdentificationNumber = new VehicleIdentificationNumber(ConvertionClass.arrayCopy(value, 1, 17));
            vehicleRegistration = new VehicleRegistrationIdentification(ConvertionClass.arrayCopy(value, 18, 15));
            wVehicleCharacteristicConstant = new W_VehicleCharacteristicConstant(ConvertionClass.arrayCopy(value, 33, 2));
            kConstantOfRecordingEquipment = new K_ConstantOfRecordingEquipment(ConvertionClass.arrayCopy(value, 35, 2));
            lTyreCircumference = new L_TyreCircumference(ConvertionClass.arrayCopy(value, 37, 2));
            tyreSize = new TyreSize(ConvertionClass.arrayCopy(value, 39, 15));
            authorisedSpeed = new SpeedAuthorised(value[54]);
            oldOdometerValue = new OdometerShort(ConvertionClass.arrayCopy(value, 55, 3));
            newOdometerValue = new OdometerShort(ConvertionClass.arrayCopy(value, 58, 3));
            oldTimeValue = new TimeReal(ConvertionClass.arrayCopy(value, 61, 4));
            newTimeValue = new TimeReal(ConvertionClass.arrayCopy(value, 65, 4));
            vuPartNumber = new VuPartNumber(ConvertionClass.arrayCopy(value, 69, 16));
            vuSerialNumber = new ExtendedSerialNumber(ConvertionClass.arrayCopy(value, 85, 8));
            sensorSerialNumber = new ExtendedSerialNumber(ConvertionClass.arrayCopy(value, 93, 8));
        }

    }
}
/*
	 * WorkshopCardCalibrationRecord ::= SEQUENCE {
	 * 	calibrationPurpose CalibrationPurpose, 1 byte
	 * 	vehicleIdentificationNumber VehicleIdentificationNumber, 17 bytes
	 * 	vehicleRegistration VehicleRegistrationIdentification, 15 bytes
	 * 	wVehicleCharacteristicConstant W-VehicleCharacteristicConstant, 2 bytes
	 * 	kConstantOfRecordingEquipment K-ConstantOfRecordingEquipment, 2 bytes
	 * 	lTyreCircumference L-TyreCircumference, 2 bytes
	 * 	tyreSize TyreSize, 15 bytes
	 * 	authorisedSpeed SpeedAuthorised, 1 byte
	 * 	oldOdometerValue OdometerShort, 3 bytes
	 * 	newOdometerValue OdometerShort, 3 bytes
	 * 	oldTimeValue TimeReal, 4 bytes
	 * 	newTimeValue TimeReal, 4 bytes
	 * 	nextCalibrationDate TimeReal, 4 bytes
	 * 	vuPartNumber VuPartNumber, 16 bytes
	 * 	vuSerialNumber VuSerialNumber, 8 bytes
	 * 	sensorSerialNumber SensorSerialNumber, 8 bytes
	 * }
	 * ---
	 * W-VehicleCharacteristicConstant ::= INTEGER(0..2^16-1)
	 * ---
	 * K-ConstantOfRecordingEquipment ::= INTEGER(0..2^16-1)
	 * ---
	 * L-TyreCircumference ::= INTEGER(0..2^16-1)
	 * ---
	 * Speed ::= INTEGER(0..255)
	 * ---
	 * SpeedAuthorized ::= Speed
	 * ---
	 * OdometerShort ::= INTEGER(0..2^24-1)
	 * ---
	 * VuSerialNumber ::= ExtendedSerialNumber
	 * ---
	 * SensorSerialNumber ::= ExtendedSerialNumber:
	 */