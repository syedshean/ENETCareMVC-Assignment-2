using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ENETCareMVCApp.Data
{
    public class InterventionType
    {
        [Required, Key]
        public int InterventionTypeID {get; set;}

        [Required]
        public string InterventionTypeName {get; set;}

        [Required]
        public double EstimatedLabour {get; set;}

        [Required]
        public double EstimatedCost{get; set;}

        public virtual ICollection<Intervention> Interventions { set; get; }
    }
}