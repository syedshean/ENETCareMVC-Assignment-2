using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ENETCareMVCApp.Models
{
    public class User
    {
        [Required, Key]
        public int UserID { set; get; }

        [Required]
        [Display(Name = "User Name")]
        public string UserName { set; get; }

        [Required]
        public string LoginName { set; get; }

        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { set; get; }

        [Required]
        [Display(Name = "Role")]
        public string UserType { set; get; }

        public int? MaxHour { set; get; }

        public int? MaxCost { set; get; }

        [Required]
        [Display(Name = "District")]
        public int DistrictID { get; set; }

        [ForeignKey("DistrictID")]
        public virtual District District { set; get; }

        [InverseProperty("User")]
        public virtual ICollection<Intervention> Interventions { set; get; }

        [InverseProperty("ApprovalUser")]
        public virtual ICollection<Intervention> ApprovedInterventions { set; get; }
    }
}