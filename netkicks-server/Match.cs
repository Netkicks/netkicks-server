using LiteNetLib;
using LiteNetLib.Utils;
using System.Text.Json.Serialization;
using System.Timers;

namespace netkicks_server
{
    public class Match
    {
        public int id;
        public string password;
        public Player[] players = new Player[Constants.MAX_PLAYERS_PER_MATCH];        
        [JsonIgnore] NetPeer[] peers = new NetPeer[Constants.MAX_PLAYERS_PER_MATCH];
        [JsonIgnore] Ball ball = new Ball();
        [JsonIgnore] Timer worldUpdateTimer;
        [JsonIgnore] Timer sendBallPositionTimer;
        [JsonIgnore] NetDataWriter[] singletonMessages = new NetDataWriter[5];

        public Match()
        {
            for (int i = 0; i <= singletonMessages.Length - 1; i++)
                singletonMessages[i] = new NetDataWriter();

            worldUpdateTimer = new Timer();
            worldUpdateTimer.Interval = Constants.WORLD_UPDATE_FREQUENCY;
            worldUpdateTimer.Elapsed += onGameUpdateTick;
            worldUpdateTimer.Start();

            sendBallPositionTimer = new Timer();
            sendBallPositionTimer.Interval = Constants.SEND_BALL_POSITION_FREQUENCY;
            sendBallPositionTimer.Elapsed += sendBallPositionTick;
            sendBallPositionTimer.Start();
        }

        private void sendBallPositionTick(object sender, ElapsedEventArgs e)
        {
            singletonMessages[0].Reset();
            singletonMessages[0].Put(NetworkMessageType.BALL_POSITION_UPDATE);
            singletonMessages[0].Put(ball.x);
            singletonMessages[0].Put(ball.y);
            singletonMessages[0].Put(ball.z);
            for (int i = 0; i <= peers.Length - 1; i++)
            {
                if (peers[i] != null)
                    peers[i].Send(singletonMessages[0], DeliveryMethod.Unreliable);
            }
        }

        private void onGameUpdateTick(object sender, ElapsedEventArgs e)
        {
            ball.Update();
        }

        #region GAME LOGIC
        public void AddPlayerToMatch(Player incomingPlayer, NetPeer peer)
        {
            int inGameIndex = Utils.GetNextEmptySlot(players);
            incomingPlayer.inGameIndex = inGameIndex;
            players[inGameIndex] = incomingPlayer;
            peers[inGameIndex] = peer;
            peer.inGameIndex = inGameIndex;
            peer.matchIndex = id;
        }

        public void RemovePlayerFromMatch(int inGameIndex)
        {
            players[inGameIndex] = null;
            peers[inGameIndex] = null;
        }

        #endregion

        public bool isFull()
        {
            //@todo
            return false;
        }
    }
}