using NetworkWrapper.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkWrapper.Core.Interfaces.ClientManagement
{
    public interface IClientManager
    {
        Client Client { get; set; }
        TcpListener Listener { get; set; }
        ClientState InitializeClient(string clientName, string ipAddress, int port);
    }
}
