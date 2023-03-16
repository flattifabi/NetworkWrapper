using NetworkWrapper.Core.Interfaces.ClientManagement;
using NetworkWrapper.Core.Interfaces.ServerManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkWrapper.Services
{
    public static class ServiceHandler
    {
        public static IServerManager ServerManager { get; set; } = new ServerManagement();
        public static IClientManager ClientManager { get; set; } = new ClientManagement();
    }
}
