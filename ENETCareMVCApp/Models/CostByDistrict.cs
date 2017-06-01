using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ENETCareMVCApp.Models
{
    public class CostByDistrict
    {
        public string DistrictName { get; set; }

        public string TotalCost { get; set; }

        public string TotalLabour { get; set; }

        [Display(Name ="Month")]
        public string Date { get; set; }
    }
}