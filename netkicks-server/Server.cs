using System;
using System.Collections.Generic;
using System.Text;
using LiteNetLib;
using Newtonsoft.Json;
using LiteNetLib.Utils;
using System.Text.Json.Serialization;

namespace netkicks_server
{
    class Server
    {
        public static NetManager server;
     
        
        public Match[] matches = new Match[9999];

        public Server()
        {
            matches[0] = new Match();
            EventBasedNetListener listener = new EventBasedNetListener();
            listener.ConnectionRequestEvent += this.onConnectionRequest;
            listener.PeerDisconnectedEvent += onDisconnected;
            listener.NetworkReceiveEvent += onGotMessage;
            server = new NetManager(listener);
            server.Start(9090);
        }

        private void onDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            Program.Log("onDisconnected");
            if (matches[peer.matchIndex] != null)
                matches[peer.matchIndex].RemovePlayerFromMatch(peer.inGameIndex);
        }

        private void onGotMessage(NetPeer peer, NetPacketReader reader, byte channel, DeliveryMethod deliveryMethod)
        {
            byte type = reader.GetByte();

            if (type == NetworkMessageType.REQUEST_MATCH_DATA)
            {
                Player incomingPlayer = 
                    JsonConvert.DeserializeObject<Player>(reader.GetString());
                Match desiredMatch = matches[incomingPlayer.desiredMatchId];

                if (desiredMatch == null)
                {
                    peer.Disconnect();
                    return;
                }

                else if (desiredMatch.isFull())
                {
                    peer.Disconnect();
                    return;
                }

                desiredMatch.AddPlayerToMatch(incomingPlayer, peer);
                NetDataWriter writer = Utils.GetNetDataWriter(NetworkMessageType.REQUEST_MATCH_DATA);
                writer.Put(JsonConvert.SerializeObject(desiredMatch));
                peer.Send(writer, DeliveryMethod.ReliableOrdered);
            }
        }

        private void onConnectionRequest(ConnectionRequest request)
        {
            Player incomingPlayer = JsonConvert.DeserializeObject<Player>(request.Data.GetString());
            Match desiredMatch;
            Console.WriteLine("onConnectionRequest");
            string rejectReason = null;
            if ((desiredMatch = matches[incomingPlayer.desiredMatchId]) == null)
                rejectReason = "Match not found";
            else if (desiredMatch.isFull())
                rejectReason = "Match is full";
            else if (isBanned(incomingPlayer, request.RemoteEndPoint.Address.ToString()))
                rejectReason = "You are banned from this server";
            else if (desiredMatch.password != null && (incomingPlayer.enteredPassword != desiredMatch.password))
                rejectReason = "Wrong password";
            if (rejectReason != null)
                request.Reject(Utils.GetNetDataWriter(rejectReason));
            else
                request.Accept();
        }

        bool isBanned(Player incomingPlayer, string ipAddress)
        {
            //@todo implementar logica pra saber se jogador esta banido ou nao
            return false;
        }
    }
}
