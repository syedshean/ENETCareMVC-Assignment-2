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

        public List<Client> GetClientListByDistrict(int districtID)
        {
            List<Client> aClientList = new List<Client>();
            using (var db = new DBContext())
            {
                aClientList = (from c in db.Clients
                               where c.District.DistrictID == districtID
                               select c).ToList();

            }
            return aClientList;
        }
    }
}