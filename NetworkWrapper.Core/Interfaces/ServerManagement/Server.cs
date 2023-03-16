using NetworkWrapper.Core.NetworkExceptions;
using NetworkWrapper.Core.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkWrapper.Core.Interfaces.ServerManagement
{
    public class Server
    {
        TcpClient _client;
        public PacketReader PacketReader;

        public event Action NewClientConnectedEvent;
        public event Action CommandReceivedEvent;
        public event Action ClientDisconnectedEvent;
        public Server()
        {
            _client = new TcpClient();
        }
        public void ConnectToServer(string username, string ipAddress, int port)
        {
            if (!_client.Connected)
            {
                _client.Connect(ipAddress, port);
                PacketReader = new PacketReader(_client.GetStream());
                if (!string.IsNullOrEmpty(username))
                {
                    var connectPacket = new PacketBuilder();
                    connectPacket.WriteOpCode(0);
                    connectPacket.WriteString(username);
                    _client.Client.Send(connectPacket.GetPacketBytes());
                }
                ReadPackets();
            }
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
                            NewClientConnectedEvent?.Invoke();
                            break;
                        case 5:
                            CommandReceivedEvent?.Invoke();
                            break;
                        case 10:
                            ClientDisconnectedEvent?.Invoke();
                            break;
                        default:
                            throw new NoValidNetworkOpCodeException("Enter an Valid OpCode");
                    }
                }
            });
        }
        public void SendMessageToServer(string message)
        {
            var messagePacket = new PacketBuilder();
            messagePacket.WriteOpCode(5);
            messagePacket.WriteString(message);
            _client.Client.Send(messagePacket.GetPacketBytes());
        }
    }
}
