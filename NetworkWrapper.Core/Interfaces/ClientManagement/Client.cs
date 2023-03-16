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
            //var opCode = PacketReader.ReadByte();
            Username = PacketReader.ReadMessage();
            WriteConnectedClientMessage(Username);
            Task.Run(() => Process());
        }
        void WriteConnectedClientMessage(string value1)
        {
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("Connected");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" [|] ");
            Console.Write(value1 + $" hat sich mit dem Netzwerk verbunden [|] {DateTime.Now}\n");
            Console.ForegroundColor = ConsoleColor.White;
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
                    WriteDisConnectedClientMessage(Username);
                    ClientSocket.Close();
                    break;
                }
            }
        }
        void WriteDisConnectedClientMessage(string value1)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("Disconnected");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" [|] ");
            Console.Write(value1 + $" hat das Netzwerk verlassen [|] {DateTime.Now}\n");
        }
    }
}
