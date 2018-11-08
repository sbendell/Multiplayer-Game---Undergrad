using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Client
{
    class ShotManager
    {
        public static List<Bullet> Bullets = new List<Bullet>();

        private static Texture2D muricaShield;
        private static Texture2D redlaser;
        private static Texture2D bluelaser;
        private static Texture2D greenlaser;

        public ShotManager(Texture2D MuricaShield, Texture2D Redlaser, Texture2D Bluelaser, Texture2D Greenlaser)
        {
            muricaShield = MuricaShield;
            redlaser = Redlaser;
            bluelaser = Bluelaser;
            greenlaser = Greenlaser;
        }

        public static void AddBullet(string Name, Vector2 Position, Vector2 Velocity, float Damage, string character)
        {
            if (character == "America")
            {
                Bullets.Add(new Bullet(muricaShield, Name, Position, Velocity, Damage));
            }
            if (character == "Starlord")
            {
                Bullets.Add(new Bullet(bluelaser, Name, Position, Velocity, Damage));
            }
            if (character == "Ironman")
            {
                Bullets.Add(new Bullet(redlaser, Name, Position, Velocity, Damage));
            }
            if (character == "Warmachine")
            {
                Bullets.Add(new Bullet(greenlaser, Name, Position, Velocity, Damage));
            }
        }

        public void Update()
        {
            for (int i = 0; i < Bullets.Count; i++)
            {
                if (Bullets[i].IsActive)
                {
                    Bullets[i].Update();
                }
                else
                {
                    Bullets.RemoveAt(i);
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Bullet bullet in Bullets)
            {
                bullet.Draw(sb);
            }
        }
    }
}
