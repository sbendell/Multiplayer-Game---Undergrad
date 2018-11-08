using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Client
{
    class Bullet
    {
        protected Vector2 position;
        protected Vector2 velocity;

        private Texture2D texture;
        private int activeCount;

        public string name;
        private float damage;

        public Bullet(Texture2D Texture, string Name, Vector2 Position, Vector2 Velocity, float Damage)
        {
            texture = Texture;
            name = Name;
            position = Position;
            velocity = Velocity;
            damage = Damage;
        }

        public float Damage
        {
            get { return damage; }
        }

        public Vector2 Position
        {
            get { return position; }
        }

        public bool IsActive
        {
            get
            {
                if (activeCount > 200)
                    return false;
                else
                    return true;
            }
        }

        public Rectangle CollisionRectangle
        {
            get { return new Rectangle((int)position.X, (int)position.Y, 16, 16); }
        }

        public void Update()
        {
            position += velocity;
            activeCount++;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, new Rectangle((int)position.X, (int)position.Y, 16, 16), Color.White);
        }
    }
}
