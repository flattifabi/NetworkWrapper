using NetworkWrapper.Core.Enums;
using NetworkWrapper.Core.Interfaces.ServerManagement;
using NetworkWrapper.Core.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkWrapper.Services
{
    public class ServerManagement : IServerManager
    {
        public Server Server { get; set; }
        public TcpClient Client { get; set; }
        public PacketReader PacketReader { get; set; }

        public event Action ConnectedEvent;
        public event Action DisconnectedEvent;
        public event Action AnotherClientConnectedEvent;

        public ClientState ConnectToServer(string clientName, string ipAdress, int port)
        {
            Client = new TcpClient();
            if(!Client.Connected)
            {
                Client.Connect(ipAdress, port);
                PacketReader = new PacketReader(Client.GetStream());
                if(!string.IsNullOrEmpty(clientName))
                {
                    return ClientState.WAITING;
                    var connectPacket = new PacketBuilder();
                    connectPacket.WriteOpCode(0);
                    connectPacket.WriteString(clientName);
                    Client.Client.Send(connectPacket.GetPacketBytes());
                }
                ReadPackets();
                return ClientState.CONNECTED;
            }
            return ClientState.CONNECTED;
        }

        public void SendMessageToServer(string message)
        {
            var messagePacket = new PacketBuilder();
            messagePacket.WriteOpCode(5);
            messagePacket.WriteString(message);
            Client.Client.Send(messagePacket.GetPacketBytes());
        }

        private void ReadPackets()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    var opcode = PacketReader.ReadByte();
                    switch (opcode)
                    {
                        case 1:
                            ConnectedEvent?.Invoke();
                            break;
                        case 5:
                            DisconnectedEvent?.Invoke();
                            break;
                        case 10:
                            AnotherClientConnectedEvent?.Invoke();
                            break;
                        default:
                            Console.WriteLine("ah yes..");
                            break;
                    }
                }
            });
        }
    }
}
