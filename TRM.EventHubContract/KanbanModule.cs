using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log4Pro.IS.TRM.EventHubContract
{
    /// <summary>
    /// A TRM modul EventHub Contractja
    /// </summary>
    public partial class TrackingContract
    {
        /// <summary>
        /// Kanban modul
        /// </summary>
        public class KanbanModule
        {
            /// <summary>
            /// EventHub szolgáltatáscsatorna modul prefixe
            /// </summary>
            public const string MODULE_PREFIX = nameof(KanbanModule);

            /// <summary>
            /// Foglalás kérés (Ezt a kérést a Tracking modul küldi a Kanban rendszernek, és ott kell megvalósítani a kiszolgálkását!)
            /// </summary>
            public class ReservationRequest
            {
                /// <summary>
                /// Csomag azonosító
                /// </summary>
                public string PackagingUnitId { get; set; }
            }

            /// <summary>
            /// Foglalás kérésre adott válasz
            /// </summary>
            public class ReservationResponse
            {
                /// <summary>
                /// LeFoglalt lokáció
                /// </summary>
                public string ReservedLocation { get; set; }
            }

            /// <summary>
            /// Sikeres betárolás
            /// </summary>
            public class SuccessStoreIn
            {
                /// <summary>
                /// Betárolt csomagolási egység
                /// </summary>
                public string PackageUnitId { get; set; }
            }

            /// <summary>
            /// Sikeres kitárolás
            /// </summary>
            public class SuccessStoreOut
            {
                /// <summary>
                /// Kitárolt csomagolási egység
                /// </summary>
                public string PackageUnitId { get; set; }
            }

            /// <summary>
            /// Változás esemény
            /// </summary>
            public class ChangeEvent
            {
                /// <summary>
                /// Kanban állvány tárhelytérképe
                /// </summary>
                public List<LocationMap> LocationMap { get; set; }
            }

            /// <summary>
            /// Tárolóterület térkép
            /// </summary>
            public class LocationMap
            {
                /// <summary>
                /// Lokáció azonosító
                /// </summary>
                public string Location { get; set; }

                /// <summary>
                /// A cellára várt csomagolási egység azonosítója (nincs rá foglalás: null, vagy string.Empty)
                /// </summary>
                public string ExpectedPackagingUnit { get; set; }

                /// <summary>
                /// A lokáción lévő csomagolási egység azonosítója (üres: null, vagy string.Empty)
                /// </summary>
                public string LoadedPackageUnitId { get; set; }

                /// <summary>
                /// A lokáció státusza
                /// </summary>
                public KanbanLocationStatus Status { get; set; }

                /// <summary>
                /// Hiba (nem az van a tárhelyen, aminek kell)
                /// </summary>
                public bool IsError { get; set; }
            }

            /// <summary>
            /// Kanban tárhelyek lehetséges státusza
            /// </summary>
            public enum KanbanLocationStatus
            {
                /// <summary>
                /// Inaktív/használhatatlan tárolóhely
                /// </summary>
                Inactive,
                /// <summary>
                /// Szabad tárolóhely
                /// </summary>
                Free,
                /// <summary>
                /// Foglalt tárhely
                /// </summary>
                Reserved,
                /// <summary>
                /// Töltött tárhely
                /// </summary>
                Loaded,
            }
        }
    }
}
