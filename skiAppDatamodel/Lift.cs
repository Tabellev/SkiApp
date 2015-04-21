using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skiAppDatamodel
{
    public class Lift
    {
        public Lift() { }

        public int LiftId { get; set; }
         
        [Required]
         [StringLength(50)]
        public string LiftName { get; set; }

        public Destination LiftDestination { get; set; }
    }
}
