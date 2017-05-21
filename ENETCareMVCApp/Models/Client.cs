using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ENETCareMVCApp.Models
{
    public class Client
    {
        [Required, Key]
        public int ClientID { set; get; }

        [Required]
        public string ClientName { set; get; }

        [Required]
        public string Address { set; get; }


        [Required]
        [Display(Name ="District")]
        public int DistrictID { get; set; }

        public virtual District District { set; get; }

        public virtual ICollection<Intervention> Interventions { set; get; }
    }
}