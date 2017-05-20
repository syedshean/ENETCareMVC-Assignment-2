using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ENETCareMVCApp.Models
{
    public class Intervention
    {
        [Required, Key]
        public int InterventionID { set; get; }

        [Required]
        public float LabourRequired {get; set;}

        [Required]
        public float CostRequired {get; set;}

        
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string InterventionDate {get; set;}

        [Required]
        public string InterventionState {get; set;}

        public string Notes {get; set;}

        public int? RemainingLife {get; set;}

        public string LastEditDate {get; set;}

        [Required]
        public virtual Client Client { get; set; }

        [Required]
        public virtual User User { get; set; }

        public virtual User ApprovalUser { get; set; }

        public virtual InterventionType InterventionType {get; set;}
    }
}