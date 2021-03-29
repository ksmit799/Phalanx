using System.Threading.Tasks;
using ENet;
using TaleWorlds.Core;
using Phalanx.Util;

namespace Phalanx.Net
{
    public class NetworkManager
    {
        private Host _connection;
        private Address _address;

        private Peer _peer;

        private Phalanx _phalanx;

        public NetworkManager(Phalanx phalanx)
        {
            _phalanx = phalanx;

            // Initialize ENet.
            Library.Initialize();
        }

        public void Tick(float dt)
        {
            if (_connection == null)
            {
                return;
            }

            if (_connection.CheckEvents(out Event netEvent) <= 0)
            {
                if (_connection.Service(0, out netEvent) <= 0)
                {
                    return;
                }
            }

            switch (netEvent.Type)
            {
                case EventType.None:
                    break;

                case EventType.Connect:
                    HandleConnect(netEvent);
                    break;

                case EventType.Disconnect:
                    HandleDisconnect(netEvent);
                    break;

                case EventType.Timeout:
                    HandleTimeout(netEvent);
                    break;

                case EventType.Receive:
                    HandleReceive(netEvent);
                    break;
            }

            _connection.Flush();
        }

        public void StartCoopGame()
        {
            Phalanx.Host = true;
        }

        public void JoinCoopGame(string connectionAddress)
        {
            if (connectionAddress.IsEmpty())
            {
                _phalanx.ShowJoinGameInquiry();
                return;
            }

            Phalanx.Host = false;

            _connection = new Host();

            string[] host = connectionAddress.Split(':');
            string ip = host[0];
            ushort port = 25575;

            if (host.Length > 1)
            {
                ushort.TryParse(host[1], out port);
            }

            DialogueManager.ShowDialogue("Joining Co-op Game", $"Attempting to join {ip}:{port}...");

            _address = new Address();
            _address.SetHost(ip);
            _address.Port = port;
            _connection.Create();

            // Attempt to connect to the server.
            _peer = _connection.Connect(_address);

            // Check if we've connected within 5 seconds.
            Task.Delay(5000).ContinueWith(_ => HandleConnectFailed());
        }

        private void HandleConnect(Event netEvent)
        {
        }

        private void HandleDisconnect(Event netEvent)
        {
        }

        private void HandleTimeout(Event netEvent)
        {
        }

        private void HandleReceive(Event netEvent)
        {
        }

        private void HandleConnectFailed()
        {
            // If we're already connected...
            if (_peer.State == PeerState.Connected)
            {
                return;
            }

            _peer.Reset();
            
            InformationManager.HideInquiry();

            // Otherwise, notify the user we've failed to connect.
            string connectionString = $"{_address.GetIP()}:{_address.Port}";
            InformationManager.ShowInquiry(new InquiryData(
                "Failed to Connect",
                $"Failed to connect on {connectionString}",
                true,
                true,
                "Retry",
                "Cancel",
                () => JoinCoopGame(connectionString),
                null));
        }
    }
}
