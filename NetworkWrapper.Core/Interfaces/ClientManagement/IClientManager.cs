using NetworkWrapper.Core.Enums;
using NetworkWrapper.Core.Packet;
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
        Client ClientAPI { get; set; }
        TcpClient Client { get; set; }
        PacketReader PacketReader { get; set; }
        event Action ClientInitialized;
        void InitializeClient(string clientName, string ipAddress, int port);
    }
}
