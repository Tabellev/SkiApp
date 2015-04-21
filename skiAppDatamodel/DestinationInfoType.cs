using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skiAppDatamodel
{
    public class DestinationInfoType
    {
        public DestinationInfoType() { }

        public int DestinationInfoTypeId { get; set; }
         
        [Required]
        [StringLength(50)]
        public string InfoType { get; set; }
    }
}
