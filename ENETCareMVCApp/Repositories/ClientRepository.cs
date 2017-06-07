using ENETCareMVCApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

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

        public bool IsUserNameExits(string clientName)
        {
            Client client;
            using (var db = new DBContext())
            {
                client = db.Clients.Where(i => i.ClientName == clientName).FirstOrDefault();
            }
            if (client == null)
                return false;
            else return true;
        }
    }
}