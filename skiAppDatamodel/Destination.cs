﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skiAppDatamodel
{
    public class Destination
    {
        public Destination() { }

        public int DestinationId { get; set; }
         
        [Required]
         [StringLength(50)]
        public string DestinationName { get; set; }

        public string ImagePath { get; set; }

         [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly"), Required]
        //Får feilmelding hvis jeg fjerner setter.
         public ICollection<OpeningHours> DestinationOpeningHours { get; set; }
    }
}
