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
    class Player
    {
        public Sprite playerSprite;

        public string name;
        private int shotTimer;
        public bool canJump = false;
        public float gravity;
        private float startHealth = 100;
        public float currentHealth = 100;
        private float damage = 10;
        private float fallTimer;

        static KeyboardState ActualKeyState;

        public static List<Player> players = new List<Player>();

        public Player(string username, Vector2 Position, Texture2D Texture, Texture2D healthTexture, Rectangle InitialFrame)
        {
            name = username;

            playerSprite = new Sprite(Position, Texture, healthTexture, InitialFrame);
            playerSprite.Location = Position;

            for (int x = 0; x < 8; x++)
            {
                playerSprite.AddFrame(
                    new Rectangle(
                        InitialFrame.X + (InitialFrame.Width * x),
                        InitialFrame.Y,
                        InitialFrame.Width,
                        InitialFrame.Height));
            }
            currentHealth = startHealth;
        }

        public float Damage
        {
            get
            {
                return damage;
            }
        }

        public bool IsDead
        {
            get
            {  if (currentHealth <= 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool StopCollisions
        {
            get
            {
                if (fallTimer > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static void Update(GameTime gt)
        {
            KeyboardState prevKS = ActualKeyState;
            ActualKeyState = Keyboard.GetState();
            MouseState ms = Mouse.GetState();

            foreach (Player p in players)
            {
                if (p.name == Login.userName)
                {
                    if (!p.IsDead)
                    {
                        if (p.fallTimer > 0)
                        {
                            p.fallTimer -= gt.ElapsedGameTime.Milliseconds;
                        }
                        p.shotTimer++;
                        if (ActualKeyState.IsKeyDown(Keys.D))
                        {
                            p.playerSprite.direction = "Right";
                            p.playerSprite.Location += new Vector2(5, 0);
                        }
                        if (ActualKeyState.IsKeyDown(Keys.A))
                        {
                            p.playerSprite.direction = "Left";
                            p.playerSprite.Location -= new Vector2(5, 0);
                        }
                        if (ActualKeyState.IsKeyDown(Keys.Space) && prevKS.IsKeyUp(Keys.Space))
                            if (p.canJump)
                            {
                                p.gravity = 12f;
                                p.canJump = false;
                            }
                        if (ActualKeyState.IsKeyDown(Keys.S) && prevKS.IsKeyUp(Keys.S))
                        {
                            if (!p.StopCollisions)
                            {
                                p.fallTimer = 450f;
                            }
                        }
                        if (ms.LeftButton == ButtonState.Pressed)
                        {
                            if (p.shotTimer > 10)
                            {
                                Vector2 tempVelocity = new Vector2((ms.Position.X - 24) - p.playerSprite.Location.X,
                                    (ms.Position.Y - 32) - p.playerSprite.Location.Y);
                                Vector2 SentVelocity = Movement.Direction(tempVelocity) * 20;
                                Network.outmsg = Network.Client.CreateMessage();
                                Network.outmsg.Write("newshot");
                                Network.outmsg.Write(Login.userName);
                                Network.outmsg.Write(SentVelocity.X);
                                Network.outmsg.Write(SentVelocity.Y);
                                Network.outmsg.Write(p.Damage);
                                Network.Client.SendMessage(Network.outmsg, NetDeliveryMethod.ReliableOrdered);
                                p.shotTimer = 0;
                            }
                        }

                        Network.outmsg = Network.Client.CreateMessage();
                        Network.outmsg.Write("move");
                        Network.outmsg.Write(p.name);
                        Network.outmsg.Write(p.playerSprite.Location.X);
                        Network.outmsg.Write(p.playerSprite.Location.Y);
                        Network.outmsg.Write(p.playerSprite.direction);
                        Network.Client.SendMessage(Network.outmsg, NetDeliveryMethod.ReliableOrdered);

                        p.playerSprite.Location -= new Vector2(0, p.gravity);
                        p.gravity -= 0.3f;
                        Console.WriteLine(p.canJump);
                    }
                }
                p.playerSprite.currentHealth = p.currentHealth;
                p.playerSprite.startHealth = p.startHealth;
                p.playerSprite.Update(gt);
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (Player p in players)
            {
                if (!p.IsDead)
                {
                    p.playerSprite.Draw(spriteBatch);
                }
            }
        }
    }
}
