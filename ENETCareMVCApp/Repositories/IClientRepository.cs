using ENETCareMVCApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ENETCareMVCApp.Repositories
{
    public interface IClientRepository
    {
        Client AddClients(Client aClient);
        bool IsUserNameExits(string clientName);
    }
}