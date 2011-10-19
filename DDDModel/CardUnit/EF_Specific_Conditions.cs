using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDClass;

namespace CardUnit
{
    /// <summary>
    /// особые условия и данные
    /// </summary>
    public class EF_Specific_Conditions
    {
        public readonly int structureSize;

        public List<SpecificConditionRecord> specificConditionRecords { get; set; }

        public EF_Specific_Conditions()
        {
            specificConditionRecords = new List<SpecificConditionRecord>();
        }

        public EF_Specific_Conditions(byte[] value, short cardType)
        {
            int noOfSpecificConditionRecords;
            int noOfValidSpecificConditionRecords = 0;

            if (cardType == EquipmentType.DRIVER_CARD)
            {
                // driver card
                noOfSpecificConditionRecords = 56;
            }
            else
            {
                // workshop card
                noOfSpecificConditionRecords = 2;
            }

            specificConditionRecords = new List<SpecificConditionRecord>(noOfSpecificConditionRecords);

            for (int i = 0; i < noOfSpecificConditionRecords; i++)
            {
                SpecificConditionRecord scr = new SpecificConditionRecord(ConvertionClass.arrayCopy(value, i * SpecificConditionRecord.structureSize, SpecificConditionRecord.structureSize));

                if (scr.entryTime.timereal != 0)
                {
                    specificConditionRecords.Add(scr);
                    noOfValidSpecificConditionRecords += 1;
                }
            }
            structureSize = noOfValidSpecificConditionRecords * SpecificConditionRecord.structureSize;
        }
    }
}
