using NetworkWrapper.Core.Enums;
using NetworkWrapper.Core.Interfaces.ClientManagement;
using NetworkWrapper.Core.Packet;
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
        public Client ClientAPI { get; set; }
        public TcpClient Client { get; set; }
        public PacketReader PacketReader { get; set; }

        public event Action ClientInitialized;

        public void InitializeClient(string clientName, string ipAddress, int port)
        {
            Client = new TcpClient();
            Client.Connect(ipAddress, port);
            PacketReader = new PacketReader(Client.GetStream());
            var connectPacket = new PacketBuilder();
            connectPacket.WriteOpCode(0);
            connectPacket.WriteString(clientName);
            Client.Client.Send(connectPacket.GetPacketBytes());
            ClientInitialized?.Invoke();
        }
    }
}
