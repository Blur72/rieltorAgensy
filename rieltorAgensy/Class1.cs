using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rieltorAgensy
{
    internal class Class1
    {
        public static RieltorskoeAgentsvoEntities2 dbconnect = new RieltorskoeAgentsvoEntities2();
        public static Clients clients;
        public static Propertiezzz propertiezzz;
        public static Realtors realtors;
        public static Deals deals;
        public static PropertyImages propertyImages;
        public static PropertyFeatures propertyFeatures;
        public static PropertyTypes propertyTypes;
        public static PropertyStatuses propertyStatuses;
        public static DealStatuses dealStatuses;
        public static PropertyHistory propertyHistory;
        public static Contacts contacts;
        public static Payments payments;
    }
}
