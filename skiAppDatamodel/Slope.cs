using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skiAppDatamodel
{
    public class Slope
    {
        public Slope() { }

        public int SlopeId { get; set; }
         
        [Required]
         [StringLength(50)]
        public string SlopeName { get; set; }

        public Destination SlopeDestination { get; set; }

        // public int NumberOfTrips { get; set; }
    }
}
