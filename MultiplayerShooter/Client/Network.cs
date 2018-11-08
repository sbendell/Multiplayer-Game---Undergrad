using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Lidgren.Network;

namespace Client
{
    class Network
    {
        public static NetClient Client;
        public static NetPeerConfiguration Config;
        public static NetIncomingMessage incmsg;
        public static NetOutgoingMessage outmsg;

        public static void Update(Game1 game)
        {
            while ((incmsg = Client.ReadMessage()) != null)
            {
                string header = incmsg.ReadString();

                switch (header)
                {
                    case "connect":
                        {
                            float timer = incmsg.ReadFloat();

                            GameLobby.timer = timer;
                        }
                        break;

                    case "CharacterSelection":
                        {
                            string username = incmsg.ReadString();
                            string selectedChar = incmsg.ReadString();
                            int x = incmsg.ReadInt32();
                            int y = incmsg.ReadInt32();

                            if (selectedChar == "Ironman")
                            {
                                Player.players.Add(new Player(username,
                                new Vector2(x, y),
                                Game1.ironManTexture,
                                Game1.healthTexture,
                                new Rectangle(0, 0, 32, 48)));
                            }
                            else if (selectedChar == "Starlord")
                            {
                                Player.players.Add(new Player(username,
                                new Vector2(x, y),
                                Game1.starlordTexture,
                                Game1.healthTexture,
                                new Rectangle(0, 0, 32, 48)));
                            }
                            else if (selectedChar == "America")
                            {
                                Player.players.Add(new Player(username,
                                new Vector2(x, y),
                                Game1.captainAmericaTexture,
                                Game1.healthTexture,
                                new Rectangle(0, 0, 32, 48)));
                            }
                            else
                            {
                                Player.players.Add(new Player(username,
                                new Vector2(x, y),
                                Game1.warmachineTexture,
                                Game1.healthTexture,
                                new Rectangle(0, 0, 32, 48)));
                            }

                            for (int a = 0; a < Player.players.Count; a++)
                            {
                                for (int b = 0; b < Player.players.Count; b++)
                                {
                                    if (a != b && Player.players[a].name.Equals(Player.players[b].name))
                                    {
                                        Player.players.RemoveAt(a);
                                        a--;
                                        break;
                                    }
                                }
                            }
                        }
                        break;

                    case "move":
                        {
                            try {
                                string username = incmsg.ReadString();
                                float x = incmsg.ReadInt32();
                                float y = incmsg.ReadInt32();
                                string direction = incmsg.ReadString();

                                for (int i = 0; i < Player.players.Count; i++)
                                {
                                    if (Player.players[i].name.Equals(username) && Player.players[i].name != Login.userName)
                                    {
                                        Player.players[i].playerSprite.Location = new Vector2(x, y);
                                        Player.players[i].playerSprite.direction = direction;
                                        break;
                                    }
                                }

                            }
                            catch
                            {
                                continue;
                            }
                        }
                        break;

                    case "health":
                        {
                            string username = incmsg.ReadString();
                            float health = incmsg.ReadFloat();

                            for (int i = 0; i < Player.players.Count; i++)
                            {
                                if (Player.players[i].name == username)
                                {
                                    Player.players[i].currentHealth = health;
                                }
                            }
                        }
                        break;

                    case "chat":
                        string chatItem = incmsg.ReadString();
                        ChatManager.chatList.Add(new ChatItem(chatItem));
                        ChatManager.chatStrings.Add(chatItem);
                        break;

                    case "newshot":
                        {
                            string playername = incmsg.ReadString();
                            float posX = incmsg.ReadFloat();
                            float posY = incmsg.ReadFloat();
                            float velocityX = incmsg.ReadFloat();
                            float velocityY = incmsg.ReadFloat();
                            float damage = incmsg.ReadFloat();
                            string selectedChar = incmsg.ReadString();
                            ShotManager.AddBullet(playername, new Vector2(posX, posY), new Vector2(velocityX, velocityY), damage, selectedChar);
                            break;
                        }
                }
                Client.Recycle(incmsg);
            }
        }


    }
}
