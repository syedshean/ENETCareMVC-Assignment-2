using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ENETCareMVCApp.Models
{
    public class User
    {
        [Required, Key]
        public int UserID { set; get; }

        [Required]
        public string UserName { set; get; }

        [Required]
        public string LoginName { set; get; }

        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { set; get; }

        [Required]
        public string UserType { set; get; }

        public int MaxHour { set; get; }

        public int MaxCost { set; get; }

        public virtual District District { set; get; }

        public virtual ICollection<Intervention> Interventions { set; get; }
    }
}