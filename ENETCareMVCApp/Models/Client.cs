using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ENETCareMVCApp.Models
{
    public class Client : IValidatableObject
    {
        [Required, Key]
        public int ClientID { set; get; }

        [Required]
        [Display(Name = "Client Name")]
        public string ClientName { set; get; }

        [Required]
        [Display(Name = "Address")]
        public string Address { set; get; }


        [Required]
        [Display(Name ="District")]
        public int DistrictID { get; set; }

        [ForeignKey("DistrictID")]
        public virtual District District { set; get; }

        public virtual ICollection<Intervention> Interventions { set; get; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!(System.Text.RegularExpressions.Regex.IsMatch(ClientName, "^[a-zA-Z]{1,50}$")))
            {
                yield return new ValidationResult("Invalid  Client name. Client name only contains letters and has to be between 1 to 50 letters.", new[] { "ClientName" });
            }

            if (!(System.Text.RegularExpressions.Regex.IsMatch(Address, "^[a-zA-Z ]{1,50}$")))
            {
                yield return new ValidationResult("Invalid  Address. Address only contains letters and space. Address has to be between 1 to 50 letters.", new[] { "Address" });
            }
        }
    }
}