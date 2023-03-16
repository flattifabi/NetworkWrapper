using Client.ViewModel.Base;
using NetworkWrapper.Core.Interfaces.ServerManagement;
using NetworkWrapper.Core.Model;
using NetworkWrapper.Core.Packet;
using NetworkWrapper.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Client.ViewModel
{
    public class MainWindowViewModel : MainWindowBaseViewModel, INotifyPropertyChanged
    {
        public ObservableCollection<ClientModel> Clients { get; set; }
        public ObservableCollection<string> Messages { get; set; }
        public ICommand _connectCommand;
        public ICommand _sendMessageCommand;
        public string Message { get; set; }
        public string Username { get; set; }
        TcpClient _client;
        PacketReader _packetReader;
        public MainWindowViewModel()
        {
            //_client = new TcpClient();
            //_client.Connect("127.0.0.1", 7891);
            //_packetReader = new PacketReader(_client.GetStream());
            //Username = "Test";
            //var connectPacket = new PacketBuilder();
            //connectPacket.WriteOpCode(0);
            //connectPacket.WriteString("Test");
            //_client.Client.Send(connectPacket.GetPacketBytes());

            ServiceHandler.ClientManager.InitializeClient("TestClient", "127.0.0.1", 7891);

            //Server Events
            ServiceHandler.ServerManager.ConnectedEvent += UserConnected;
            ServiceHandler.ServerManager.CommandReceived += MessageReceived;
            ServiceHandler.ServerManager.AnotherClientConnectedEvent += UserDisconeced;
        }

        private void UserDisconeced()
        {
            var uid = ServiceHandler.ServerManager.Server.PacketReader.ReadMessage();
            var user = Clients.Where(x => x.UID == uid).FirstOrDefault();
            Application.Current.Dispatcher.Invoke(() => Clients.Remove(user));
        }

        private void MessageReceived()
        {
            var message = ServiceHandler.ServerManager.Server.PacketReader.ReadMessage();
            Application.Current.Dispatcher.Invoke(() => Messages.Add(message));
        }

        private void UserConnected()
        {
            var user = new ClientModel
            {
                ClientName = ServiceHandler.ServerManager.Server.PacketReader.ReadMessage(),
                UID = ServiceHandler.ServerManager.Server.PacketReader.ReadMessage()
            };

            if (!Clients.Any(x => x.UID == user.UID))
            {
                Application.Current.Dispatcher.Invoke(() => Clients.Add(user));
            }
        }
        public ICommand SendMessageCommand
        {
            get
            {
                if (_sendMessageCommand == null)
                    _sendMessageCommand = new RelayCommand(param => ExecuteSendMessage(), param => true);
                return _sendMessageCommand;
            }
        }
        public ICommand ConnectCommand
        {
            get
            {
                if (_connectCommand == null)
                    _connectCommand = new RelayCommand(param => Executecommand(), param => true);
                return _connectCommand;
            }
        }
        public void ExecuteSendMessage()
        {
            ServiceHandler.ServerManager.Server.SendMessageToServer(Message);
        }
        public void Executecommand()
        {
            //ServiceHandler.ServerManager.ConnectToServer(Username, "127.0.0.1", 7891);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
