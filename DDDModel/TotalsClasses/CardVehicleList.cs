using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TotalsClasses
{
    public class CardVehicleList
    {
        List<DDDClass.CardVehicleRecord> vehicleUsed { get; set; }

        public CardVehicleList()
        {
            vehicleUsed = new List<DDDClass.CardVehicleRecord>();
        }

        public CardVehicleList(List<DDDClass.CardVehicleRecord> cardVehList)
        {
            vehicleUsed = new List<DDDClass.CardVehicleRecord>();
            vehicleUsed = cardVehList;
        }

        public List<DDDClass.CardVehicleRecord> FindVehicleNumberByDate(DateTime date)
        {
            return vehicleUsed.FindAll(
                delegate(DDDClass.CardVehicleRecord cvb)
                {
                    return cvb.vehicleFirstUse.getTimeRealDate().Date == date.Date;
                }
            );
        }
    }
}
