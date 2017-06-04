using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ENETCareMVCApp.Data
{
    public class District
    {
        [Required, Key]
        public int DistrictID { set; get; }

        [Required]
        public string DistrictName { set; get; }
        public virtual ICollection<Client> Clients { get; set; }

        public virtual ICollection<User> Users { get; set; }

    }
}