using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;


namespace skiAppDatamodel
{
   
   public class SkiDay
   {
        public SkiDay() { }

        public int SkiDayId { get; set; }
        
       [Required]
        public User SkiDayUser { get; set; }

       [StringLength(50)] 
       public string Destination { get; set; }

       [StringLength(10)] 
         public string Date { get; set; }

        [StringLength(5)] 
         public string StartTime { get; set; }

        [StringLength(5)] 
         public string StopTime { get; set; }

        [StringLength(30)] 
         public string Equipment { get; set; }

         public int NumberOfTrips { get; set; }

        [StringLength(200)] 
         public string Comment { get; set; }

         [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
         //Får feilmelding hvis jeg fjerner set.
         public ICollection<Lift> Lifts { get; set; }

         [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
         //Får feilmelding hvis jeg fjerner set.
         public ICollection<Slope> Slopes { get; set; }

    }
}
