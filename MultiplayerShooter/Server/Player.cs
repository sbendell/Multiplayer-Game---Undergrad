using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Lidgren.Network;

namespace Server
{
    class Player
    {
        public string direction;
        public Vector2 pos;
        public string name;
        public float health = 100;
        public string selectedCharacter = "";

        public static List<Player> players = new List<Player>();

        public Player(string username, Vector2 position, string character)
        {
            name = username;
            pos = position;
            selectedCharacter = character;
        }

        public static void Update()
        {
            if(Network.Server.ConnectionsCount == players.Count)
            {
                for (int i = 0; i < players.Count; i++)
                {
                    Network.outmsg = Network.Server.CreateMessage();

                    Network.outmsg.Write("move");
                    Network.outmsg.Write(players[i].name);
                    Network.outmsg.Write((int)players[i].pos.X);
                    Network.outmsg.Write((int)players[i].pos.Y);
                    Network.outmsg.Write(players[i].direction);
                    Network.Server.SendMessage(Network.outmsg, Network.Server.Connections, NetDeliveryMethod.ReliableOrdered, 0);


                    Network.outmsg = Network.Server.CreateMessage();
                    Network.outmsg.Write("health");
                    Network.outmsg.Write(players[i].name);
                    Network.outmsg.Write(players[i].health);
                    Network.Server.SendMessage(Network.outmsg, Network.Server.Connections, NetDeliveryMethod.ReliableOrdered, 0);
                }
            }
        }

    }
}
