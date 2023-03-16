using NetworkWrapper.Core.Enums;
using NetworkWrapper.Core.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkWrapper.Core.Interfaces.ServerManagement
{
    public interface IServerManager
    {
        Server Server { get; set; }
        PacketReader PacketReader { get; set; }
        TcpClient Client { get; set; }
        event Action ConnectedEvent;
        event Action CommandReceived;
        event Action AnotherClientConnectedEvent;

        ClientState ConnectToServer(string clientName, string ipAdress, int port);
        void SendMessageToServer(string message);


    }
}
