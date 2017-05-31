using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ENETCareMVCApp.Models
{
    public class ClientWithInterventionModel
    {
        public Client aClient { get; set; }
        public string DistrictName { get; set; }
        public int InterventionID { get; set; }
        public string UserName { get; set; }
        public string InterventionTypeName { get; set; }
        public string InterventionState { get; set; }
    }
}