using ENETCareMVCApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ENETCareMVCApp.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private DBContext db = new DBContext();
        public Client AddClients(Client aClient)
        {
            db.Clients.Add(aClient);
            db.SaveChanges();
            return aClient;
        }
    }
}