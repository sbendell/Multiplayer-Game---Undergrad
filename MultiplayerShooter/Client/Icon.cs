using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client
{
    class Icon
    {
        protected Texture2D Texture;
        protected List<Rectangle> frames = new List<Rectangle>();
        private int frameWidth = 0;
        private int frameHeight = 0;
        public int currentFrame;
        protected Vector2 location;

        public Icon(Texture2D texture, Vector2 location, Rectangle initialFrame)
        {
            this.location = location;
            Texture = texture;
            frames.Add(initialFrame);
            frameWidth = initialFrame.Width;
            frameHeight = initialFrame.Height;
        }

        public int Frame
        {
            get { return currentFrame; }
            set
            {
                currentFrame = (int)MathHelper.Clamp(value, 0,
                frames.Count - 1);
            }
        }

        public Vector2 Center
        {
            get
            {
                return location +
                    new Vector2(frameWidth / 2, frameHeight / 2);
            }
        }

        public Rectangle Source
        {
            get { return frames[currentFrame]; }
        }

        public void AddFrame(Rectangle frameRectangle)
        {
            frames.Add(frameRectangle);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                Texture,
                Center,
                Source,
                Color.White,
                0.0f,
                new Vector2(frameWidth / 2, frameHeight / 2),
                1.0f,
                SpriteEffects.None,
                0.0f);
        }
    }
}
