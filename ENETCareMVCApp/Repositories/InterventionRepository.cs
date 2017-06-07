using ENETCareMVCApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ENETCareMVCApp.Repositories
{
    public class InterventionRepository :IInterventionRepository
    {
        private DBContext db = new DBContext();
        public Intervention EditIntervention(Intervention intervention)
        {
            db.Interventions.Attach(intervention);
            //db.Entry(intervention).Property(i => i.InterventionState).IsModified = true;
            db.Entry(intervention).Property(i => i.Notes).IsModified = true;
            db.Entry(intervention).Property(i => i.RemainingLife).IsModified = true;
            db.Entry(intervention).Property(i => i.LastEditDate).IsModified = true;
            //db.Entry(intervention).State = EntityState.Modified;
            db.SaveChanges();
            return intervention;
        }
    }
}