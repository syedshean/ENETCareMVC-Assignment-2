using ENETCareMVCApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ENETCareMVCApp.Repositories
{
    public interface IInterventionRepository
    {
        Intervention EditIntervention(Intervention intervention);
    }
}