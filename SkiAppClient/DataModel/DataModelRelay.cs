using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiAppClient.DataModel
{

    public class User
    {

        public User() { }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

    }

    public class UserText
    {
        public UserText(string information) 
        {
            Information = information;
        }

        public string Information { get; set; }

    }
    public class Lift
    {
        public Lift() { }

        public int LiftId { get; set; }

        public string LiftName { get; set; }

        public Destination LiftDestination { get; set; }
    }

    public class Slope
    {
        public Slope() { }

        public int SlopeId { get; set; }

        public string SlopeName { get; set; }

        public Destination SlopeDestination { get; set; }
    }

    public class SkiDay
    {
         public SkiDay() { }

        public int SkiDayId { get; set; }

        public User SkiDayUser { get; set; }

        public string Destination { get; set; }

        public string Date { get; set; }

        public string StartTime { get; set; }

        public string StopTime { get; set; }

        public string Equipment { get; set; }

        public int NumberOfTrips { get; set; }

        public string Comment { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //Må ha med setter eller så får jeg feilmelding.
        public ObservableCollection<Lift> Lifts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //Må ha med setter eller så får jeg feilmelding.
        public ObservableCollection<Slope> Slopes { get; set; }

    }

    public class Destination
    {
        public Destination() { }

        public int DestinationId { get; set; }

        public string DestinationName { get; set; }

        //public string DestinationUrl { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Img")]
        //Short for image
        public string ImgPath { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //Må ha med setter eller så får jeg feilmelding.
        public ObservableCollection<OpeningHours> DestinationOpeningHours { get; set; }
    }
    public class DestinationInfoType
    {
        public DestinationInfoType(string infoType) 
        {
            InfoType = infoType;
        }
        public string InfoType { get; set; }
    }

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

    public class Price
    {
        public Price() { }

        public int PriceId { get; set; }

        public string Duration { get; set; }

        public int PassPriceAdult { get; set; }
        public int PassPriceTeenSenior { get; set; }

        public const string Adult = "Voksen";
        public const string TeenSenior = "Ungdom/Senior";
        public const string Children = "Barn";
        public const string PassPriceChildren = "Gratis";

       
    }
}
