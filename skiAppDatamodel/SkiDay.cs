﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skiAppDatamodel
{
   public class SkiDay
    {
        public SkiDay() { }

        public int SkiDayId { get; set; }
        
       [Required]
        public User SkiDayUser { get; set; }
        public string Destination { get; set; }

         public string Date { get; set; }

         public string StartTime { get; set; }

         public string StopTime { get; set; }

         public string Equipment { get; set; }

         public int NumberOfTrips { get; set; }

         public string Comment { get; set; }

         [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
         //Får feilmelding hvis jeg fjerner set.
         public ICollection<Lift> Lifts { get; set; }

         [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
         //Får feilmelding hvis jeg fjerner set.
         public ICollection<Slope> Slopes { get; set; }

    }
}