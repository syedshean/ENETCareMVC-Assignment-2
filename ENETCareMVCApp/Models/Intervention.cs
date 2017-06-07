using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
    public class Intervention : IValidatableObject
    {
        [Required, Key]
        public int InterventionID { set; get; }

        [Required]
        [Display(Name = "Labour Hour Required")]
        [Range(1, 720, ErrorMessage = "You entered an unreasonable Labour Hour for Intervention")]
        public float LabourRequired {get; set;}

        [Required]
        [Display(Name = "Cost Required")]
        [Range(1, 200000, ErrorMessage = "You entered an unreasonable Cost for Intervention")]
        public float CostRequired {get; set;}

        
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Intervention Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string InterventionDate {get; set;}


        [Display(Name = "Intervention State")]
        public InterventionState InterventionState { set; get; }

        [DataType(DataType.MultilineText)]
        public string Notes {get; set;}

        public int? RemainingLife {get; set;}


        [Display(Name = "Last Edit Date")]
        public string LastEditDate {get; set;}

        [Required]
        [Display(Name = "Client Name")]
        public int ClientID { get; set; }


        [ForeignKey("ClientID")]
        public virtual Client Client { get; set; }

        [Required]
        [Display(Name = "Site Engineer")]
        public int UserID { get; set; }


        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        [Display(Name = "Approve User Name")]
        public int ApproveUserID { get; set; }
        
        [ForeignKey("UserID")]
        public virtual User ApprovalUser { get; set; }

        [Required]
        [Display(Name = "Intervention Type Name")]
        public int InterventionTypeID { get; set; }

        [ForeignKey("InterventionTypeID")]
        public virtual InterventionType InterventionType {get; set;}

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if ((RemainingLife < 0) || (RemainingLife > 100))
            {
                yield return new ValidationResult("Remaining life must be between 0 to 100", new[] { "RemainingLife" });
            }

            if(Notes != null)
            {
                if (Notes.ToCharArray().Length > 30000)
                {
                    yield return new ValidationResult("Notes has to be within 5000 words", new[] { "Notes" });
                }
            }         
  
        }
    }
}