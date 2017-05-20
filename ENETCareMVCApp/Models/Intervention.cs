using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ENETCareMVCApp.Models
{
    public enum InterventionState
    {
        Proposed = 1,
        Approved = 2,
        Completed = 3,
        Cancelled = 4
    };
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
        
        public InterventionState InterventionState { set; get; }

        public string Notes {get; set;}

        public int? RemainingLife {get; set;}

        public string LastEditDate {get; set;}

        [Display(Name = "Client Name")]
        public int ClientID { get; set; }


        [Required]
        public virtual Client Client { get; set; }

        [Display(Name = "User Name")]
        public int UserID { get; set; }


        [Required]
        public virtual User User { get; set; }

        [Display(Name = "Approve User Name")]
        public int ApproveUserID { get; set; }


        public virtual User ApprovalUser { get; set; }

        [Display(Name = "Intervention Type Name")]
        public int InterventionTypeID { get; set; }

        public virtual InterventionType InterventionType {get; set;}
    }
}