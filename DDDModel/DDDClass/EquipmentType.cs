using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    /// <summary>
    /// Тип оборудования. Описывает тип карты, либо ТС
    /// </summary>
    public class EquipmentType//1byte
    {
        public short equipmentType { get; set; }

        public readonly static short RESERVED = 0;

        public readonly static short DRIVER_CARD = 1;

        public readonly static short WORKSHOP_CARD = 2;

        public readonly static short CONTROL_CARD = 3;

        public readonly static short COMPANY_CARD = 4;

        public readonly static short MANUFACTURING_CARD = 5;

        public readonly static short VEHICLE_UNIT = 6;

        public readonly static short MOTION_SENSOR = 7;

        public EquipmentType()
        {
            equipmentType = RESERVED;
        }

        public EquipmentType(byte value)
        {
            equipmentType = ConvertionClass.convertIntoUnsigned1ByteInt(value);
        }

        public EquipmentType(string value)
        {
            byte b = Convert.ToByte(value);
            equipmentType = ConvertionClass.convertIntoUnsigned1ByteInt(b);
        }
        /// <summary>
        /// перегруженная строка соответственно документации
        /// </summary>
        /// <returns>строка по документам</returns>
        public override string ToString()
        {
            if (equipmentType == RESERVED)
            {
                return "reserved";
            }
            if (equipmentType == DRIVER_CARD)
            {
                return "driver card";
            }
            if (equipmentType == WORKSHOP_CARD)
            {
                return "workshop card";
            }
            if (equipmentType == CONTROL_CARD)
            {
                return "control card";
            }
            if (equipmentType == COMPANY_CARD)
            {
                return "company card";
            }
            if (equipmentType == MANUFACTURING_CARD)
            {
                return "manufacturing card";
            }
            if (equipmentType == VEHICLE_UNIT)
            {
                return "vehicle unit";
            }
            if (equipmentType == MOTION_SENSOR)
            {
                return "motion sensor";
            }

            return "????";
        }
    }
}
/*
	 * EquipmentType ::= INTEGER(0..255), 1 byte
	 * ---
	 * - - Reserved (0),
	 * - - Driver Card (1),
	 * - - Workshop Card (2),
	 * - - Control Card (3),
	 * - - Company Card (4),
	 * - - Manufacturing Card (5),
	 * - - Vehicle Unit (6),
	 * - - Motion Sensor (7),
	 * - - RFU (8..255)
	 */
