using NetworkWrapper.Core.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkWrapper.Core.Interfaces.ClientManagement
{
    public class Client
    {
        public string Username { get; set; }
        public Guid UID { get; set; }
        public TcpClient ClientSocket { get; set; }
        PacketReader PacketReader;

        public Client(TcpClient client)
        {
            ClientSocket = client;
            UID = Guid.NewGuid();
            PacketReader = new PacketReader(ClientSocket.GetStream());
            var opCode = PacketReader.ReadByte();
            Username = PacketReader.ReadMessage();
            Console.WriteLine($"{DateTime.Now} | Capacity {Console.ForegroundColor = ConsoleColor.Green}[{Username}] {Console.ForegroundColor = ConsoleColor.Green} has Connected to the Server");
            Task.Run(() => Process());
        }
        void Process()
        {
            while (true)
            {
                try
                {
                    var opcode = PacketReader.ReadByte();
                    switch (opcode)
                    {
                        case 5:
                            var message = PacketReader.ReadMessage();
                            Console.WriteLine($"[{DateTime.Now}]: Message received: {message}");
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine($"[{UID.ToString()}]: Disconnected");
                    Program.BroadcastDisconnect(UID.ToString());
                    ClientSocket.Close();
                    break;
                }
            }
        }
    }
}
