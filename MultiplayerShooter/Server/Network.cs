using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Lidgren.Network;

namespace Server
{
    class Network
    {
        public static NetServer Server; 
        public static NetPeerConfiguration Config;
        public static NetIncomingMessage incmsg;
        public static NetOutgoingMessage outmsg;
        public static float gameLobbyTimer = 30000;

        public static void Uodate(Game1 game, GameTime gameTime)
        {
            gameLobbyTimer -= (float)gameTime.ElapsedGameTime.Milliseconds;

            while ((incmsg = Server.ReadMessage()) != null)
            {
                switch (incmsg.MessageType)
                {

                    case NetIncomingMessageType.Data:

                        string header = incmsg.ReadString();

                        switch (header) {
                            case "connect":
                                {
                                    string username = incmsg.ReadString();
                                    game.Window.Title = "" + Player.players.Count + header;

                                    outmsg = Server.CreateMessage();
                                    outmsg.Write("connect");
                                    outmsg.Write(gameLobbyTimer);
                                    Server.SendMessage(outmsg, Server.Connections, NetDeliveryMethod.ReliableOrdered, 0);
                                }

                                break;

                            case "CharacterSelection":
                                {
                                    string username = incmsg.ReadString();
                                    string selectedChar = incmsg.ReadString();
                                    int x = incmsg.ReadInt32();
                                    int y = incmsg.ReadInt32();
                                    Player.players.Add(new Player(username, new Vector2(x, y), selectedChar));
                                    game.Window.Title = "" + Player.players.Count + header;

                                    for (int i = 0; i < Player.players.Count; i++)
                                    {
                                        outmsg = Server.CreateMessage();
                                        outmsg.Write("CharacterSelection");
                                        outmsg.Write(Player.players[i].name);
                                        outmsg.Write(Player.players[i].selectedCharacter);
                                        outmsg.Write((int)Player.players[i].pos.X);
                                        outmsg.Write((int)Player.players[i].pos.Y);
                                        Server.SendMessage(outmsg, Server.Connections, NetDeliveryMethod.ReliableOrdered, 0);
                                    }
                                }
                                break;

                            case "move":
                                {
                                    game.Window.Title = "" + Player.players.Count + header;
                                    try {
                                        string username = incmsg.ReadString();
                                        float x = incmsg.ReadFloat();
                                        float y = incmsg.ReadFloat();
                                        string direction = incmsg.ReadString();

                                        for (int i = 0; i < Player.players.Count; i++)
                                        {
                                            if (Player.players[i].name.Equals(username))
                                            {
                                                Player.players[i].pos = new Vector2(x, y);
                                                Player.players[i].direction = direction;
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        continue;
                                    }
                                }
                                        
                                break;

                            case "chat":
                                string chatItem = incmsg.ReadString();
                                outmsg = Server.CreateMessage();
                                outmsg.Write("chat");
                                outmsg.Write(chatItem);
                                Server.SendMessage(outmsg, Server.Connections, NetDeliveryMethod.ReliableOrdered, 0);

                                break;

                            case "newshot":
                                string bulletname = incmsg.ReadString();
                                float velocityX = incmsg.ReadFloat();
                                float velocityY = incmsg.ReadFloat();
                                float damage = incmsg.ReadFloat();

                                foreach (Player p in Player.players)
                                {
                                    if (p.name == bulletname)
                                    {
                                        outmsg = Server.CreateMessage();
                                        outmsg.Write("newshot");
                                        outmsg.Write(p.name);
                                        outmsg.Write(p.pos.X + 16);
                                        outmsg.Write(p.pos.Y + 24);
                                        outmsg.Write(velocityX);
                                        outmsg.Write(velocityY);
                                        outmsg.Write(damage);
                                        outmsg.Write(p.selectedCharacter);
                                        Server.SendMessage(outmsg, Server.Connections, NetDeliveryMethod.ReliableOrdered, 0);
                                    }
                                }
                                break;

                            case "bulletCollision":
                                string name = incmsg.ReadString();
                                float Damage = incmsg.ReadFloat();

                                for (int i = 0; i < Player.players.Count; i++)
                                {
                                    if (Player.players[i].name == name)
                                    {
                                        Console.WriteLine(Damage);
                                        Player.players[i].health -= Damage;
                                        Console.WriteLine("bullet message receieved");
                                        Console.WriteLine(Player.players[i].health);
                                    }
                                }
                                break;

                        }

                        break;

                }
            }
        }


    }
}
