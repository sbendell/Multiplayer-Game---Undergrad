using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client
{
    class CollisionManager
    {
        Rectangle ScreenBounds;

        Rectangle floor = new Rectangle(0, 648, 1280, 88);
        Rectangle PlatformOne = new Rectangle(120, 432, 288, 72);
        Rectangle PlatformTwo = new Rectangle(872, 432, 288, 72);
        Rectangle PlatformThree = new Rectangle(352, 216, 576, 72);

        public CollisionManager(Rectangle Screenbounds)
        {
            ScreenBounds = Screenbounds;
        }

        public void PlayerScreenBoundsCollisions()
        {
            foreach (var player in Player.players)
            {
                if (player.name == Login.userName)
                {
                    if (player.playerSprite.Location.X < ScreenBounds.Left)
                    {
                        player.playerSprite.Location = new Vector2(ScreenBounds.Left, player.playerSprite.Location.Y);
                    }
                    if (player.playerSprite.Location.X > ScreenBounds.Right - 32)
                    {
                        player.playerSprite.Location = new Vector2(ScreenBounds.Right -32, player.playerSprite.Location.Y);
                    }
                    if (player.playerSprite.Location.Y < ScreenBounds.Top)
                    {
                        player.playerSprite.Location = new Vector2(player.playerSprite.Location.X, ScreenBounds.Top);
                        player.gravity = 0f;
                    }
                }
            }
        }

        public void PlayerPlatformCollisions()
        {
            for(int i = 0; i < Player.players.Count; i++)
            {
                if (Player.players[i].name == Login.userName)
                {
                    if (!Player.players[i].StopCollisions)
                    {
                        if (floor.Intersects(Player.players[i].playerSprite.CollisionRectangle))
                        {
                            Player.players[i].canJump = true;
                            if (Player.players[i].gravity <= 0f)
                                Player.players[i].gravity = 0f;
                        }
                        else if (PlatformOne.Intersects(Player.players[i].playerSprite.CollisionRectangle))
                        {
                            Player.players[i].canJump = true;
                            if (Player.players[i].gravity <= 0f)
                                Player.players[i].gravity = 0f;
                        }
                        else if (PlatformTwo.Intersects(Player.players[i].playerSprite.CollisionRectangle))
                        {
                            Player.players[i].canJump = true;
                            if (Player.players[i].gravity <= 0f)
                                Player.players[i].gravity = 0f;
                        }
                        else if (PlatformThree.Intersects(Player.players[i].playerSprite.CollisionRectangle))
                        {
                            Player.players[i].canJump = true;
                            if (Player.players[i].gravity <= 0f)
                                Player.players[i].gravity = 0f;
                        }
                        else
                        {
                            Player.players[i].canJump = false;
                        }
                    }
                }
            }
        }

        public void ShotToPlatformCollision()
        {
            for (int i = 0; i < ShotManager.Bullets.Count; i++)
            {
                if (PlatformOne.Contains(ShotManager.Bullets[i].Position))
                {
                    ShotManager.Bullets.RemoveAt(i);
                    return;
                }
                if (PlatformTwo.Contains(ShotManager.Bullets[i].Position))
                {
                    ShotManager.Bullets.RemoveAt(i);
                    return;
                }
                if (PlatformThree.Contains(ShotManager.Bullets[i].Position))
                {
                    ShotManager.Bullets.RemoveAt(i);
                    return;
                }
            }
        }

        public void ShotToPlayerCollision()
        {
            for (int i = 0; i < Player.players.Count; i++)
            {
                for (int x = 0; x < ShotManager.Bullets.Count; x++)
                {
                    if (ShotManager.Bullets[x].name != Player.players[i].name && !Player.players[i].IsDead)
                    {
                        if (ShotManager.Bullets[x].CollisionRectangle.Intersects(Player.players[i].playerSprite.CollisionRectangle))
                        {
                            if (Login.userName == Player.players[i].name)
                            {
                                Network.outmsg = Network.Client.CreateMessage();
                                Network.outmsg.Write("bulletCollision");
                                Network.outmsg.Write(Player.players[i].name);
                                Network.outmsg.Write(ShotManager.Bullets[x].Damage);
                                Network.Client.SendMessage(Network.outmsg, Lidgren.Network.NetDeliveryMethod.ReliableOrdered);
                            }
                            ShotManager.Bullets.RemoveAt(x);
                        }
                    }
                }
            }
        }

        public void Update(Game1 game)
        {
            PlayerPlatformCollisions();
            PlayerScreenBoundsCollisions();
            ShotToPlatformCollision();
            ShotToPlayerCollision();
        }

        public void Draw(SpriteBatch sb, Texture2D Tile4Long, Texture2D Tile8Long)
        {
            sb.Draw(Tile4Long, PlatformOne, Color.White);
            sb.Draw(Tile4Long, PlatformTwo, Color.White);
            sb.Draw(Tile8Long, PlatformThree, Color.White);
        }
    }
}
