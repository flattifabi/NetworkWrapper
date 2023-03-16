using NetworkWrapper.Core.Interfaces.ClientManagement;
using NetworkWrapper.Core.Packet;
using NetworkWrapper.Services;
using Spectre.Console;
using System;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    internal class Program
    {
        static List<Client> _users;
        static TcpListener _listener;
        static void Main(string[] args)
        {
            _users = new List<Client>();
            WriteBaseMonitoring();
            _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7891);
            _listener.Start();

            while (true)
            {
                var client = new Client(_listener.AcceptTcpClient());
                _users.Add(client);
            }
        }
        public static Table table = new Table();
        public static void WriteBaseMonitoring()
        {
            var selected = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title("Verwende verschiedenste Funktionen von [green]E.P.A[/]?")
        .PageSize(10)
        .MoreChoicesText("[grey](Wähle eine Aktion)[/]")
        .AddChoices(new[] {
            "Verbindungen", "Live Ansicht", "Fehler behandlung",
            "Aktionen", "Configuration",
        }));

            if(selected == "Live Ansicht")
            {
                AnsiConsole.Write(new BarChart()
                    .Width(60)
                    .Label("[green bold underline]Live Daten des E.P.A Systems[/]")
                    .CenterLabel()
                    .AddItem("Aktive Verbindungen", 35, Color.Yellow)
                    .AddItem("Anzahl der Fehler", 2, Color.Green)
                    .AddItem("Verbindungen verloren", 1, Color.Red));
                Console.WriteLine("\n[x] Zurück");
                Console.Write(" > ");
                string str = Console.ReadLine();
                if(str == "x")
                {
                    Console.Clear();
                    WriteBaseMonitoring();
                }
            }
            else if(selected == "Verbindungen")
            {

            }



            // Echo the fruit back to the terminal
            //AnsiConsole.WriteLine($"I agree. {fruit} is tasty!");
        }

        public static void BuildLiveView()
        {

        }

        public static void BroadcastMessage(string message)
        {
            WriteBaseMonitoring();
            foreach (var user in _users)
            {
                var messagePacket = new PacketBuilder();
                messagePacket.WriteOpCode(5);
                messagePacket.WriteString(message);
                user.ClientSocket.Client.Send(messagePacket.GetPacketBytes());
            }
        }
        public static void BroadcastDisconnect(string uid)
        {
            var disconnectedUSer = _users.Where(x => x.UID.ToString() == uid).FirstOrDefault();
            _users.Remove(disconnectedUSer);
            foreach (var user in _users)
            {
                var broadcastPacket = new PacketBuilder();
                broadcastPacket.WriteOpCode(10);
                broadcastPacket.WriteString(uid);
                user.ClientSocket.Client.Send(broadcastPacket.GetPacketBytes());
            }
            BroadcastMessage($"[{disconnectedUSer.Username}] disconnected");
        }
        public static int count = 0;
        public static void WriteConnectedClientMessage(string value1)
        {
            if(count == 20)
            {
                WriteBaseMonitoring();
            }
            count = count+1;
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("Connected");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" [|] ");
            Console.Write(value1 + $" hat sich mit dem Netzwerk verbunden [|] {DateTime.Now}\n");
        }
    }
}