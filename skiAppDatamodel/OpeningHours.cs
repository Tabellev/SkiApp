using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skiAppDatamodel
{
    public class OpeningHours
    {
        public OpeningHours() { }

        public int OpeningHoursId { get; set; }
        public string FromDate { get; set; }

        public string ToDate { get; set; }

        //public enum Days { Man, Tirs, Ons, Tors, Fre, Lør, Søn };
        public string FromClockToClockWeek { get; set; }
        public string FromClockToClockSat { get; set; }
        public string FromClockToClockSun { get; set; }

        public string NightRiding { get; set; }

        public string MorningRiding { get; set; }
    }
}
