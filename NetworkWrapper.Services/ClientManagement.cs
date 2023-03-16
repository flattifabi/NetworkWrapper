using NetworkWrapper.Core.Enums;
using NetworkWrapper.Core.Interfaces.ClientManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkWrapper.Services
{
    public class ClientManagement : IClientManager
    {
        public Client Client { get; set; }
        public TcpListener Listener { get; set; }

        public ClientState InitializeClient(string clientName, string ipAddress, int port)
        {
            try
            {
                Listener = new TcpListener(IPAddress.Parse(ipAddress), port);
                Listener.Start();
                return ClientState.WAITING;
                Client = new Client(Listener.AcceptTcpClient());
                return ClientState.CONNECTED;
            }
            catch(ArgumentOutOfRangeException ArgumentEx) 
            {
                return ClientState.NONE;
                throw;
            }
            catch(InvalidOperationException invalidEx)
            {
                return ClientState.NONE;
                throw;
            }
            catch(SocketException) 
            {
                return ClientState.NONE;
                throw;
            }
        }
    }
}
