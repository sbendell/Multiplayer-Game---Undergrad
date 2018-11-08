using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client
{
    class Sprite
    {
        protected Texture2D Texture;
        protected Texture2D healthTexture;

        public string direction = "Right";

        protected List<Rectangle> frames = new List<Rectangle>();
        private int frameWidth = 0;
        private int frameHeight = 0;
        public int currentFrame;
        private float frameTime = 0.1f;
        public float timeForCurrentFrame = 0.0f;
        public float startHealth;
        public float currentHealth;

        protected Vector2 location = Vector2.Zero;

        public Sprite(
            Vector2 location,
            Texture2D texture,
            Texture2D healthTexture,
            Rectangle initialFrame)
        {
            this.location = location;
            Texture = texture;
            this.healthTexture = healthTexture;

            frames.Add(initialFrame);
            frameWidth = initialFrame.Width;
            frameHeight = initialFrame.Height;
        }

        public Vector2 Location
        {
            get { return location; }
            set { location = value; }
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

        public float FrameTime
        {
            get { return frameTime; }
            set { frameTime = MathHelper.Max(0, value); }
        }

        public Rectangle Source
        {
            get { return frames[currentFrame]; }
        }

        public Vector2 Center
        {
            get
            {
                return location +
                    new Vector2(frameWidth / 2, frameHeight / 2);
            }
        }

        public Vector2 Top
        {
            get
            {
                return location +
                    new Vector2(frameWidth / 2, 0);
            }
        }

        public Rectangle CollisionRectangle
        {
            get
            {
                return new Rectangle(
                    (int)Location.X,
                    (int)Location.Y,
                    frameWidth,
                    frameHeight);
            }
        }

        public void AddFrame(Rectangle frameRectangle)
        {
            frames.Add(frameRectangle);
        }

        public virtual void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            timeForCurrentFrame += elapsed;

            if (timeForCurrentFrame >= FrameTime)
            {
                timeForCurrentFrame = 0.0f;
                currentFrame = (currentFrame + 1) % (frames.Count);

                if (direction == "Left")
                {
                    if (currentFrame > 4)
                    {
                        currentFrame = 1;
                    }
                }
                else if (direction == "Right")
                {
                    if (currentFrame < 5 || currentFrame > 9)
                    {
                        currentFrame = 5;
                    }
                }
            }
        }

        private float HealthBarFloat
        {
            get { return (currentHealth / startHealth); }
        }

        private int healthBarRectWidth(float healthBarFloat)
        {
            return (int)(32 * healthBarFloat);
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

            spriteBatch.Draw(
                healthTexture,
                new Rectangle((int)Location.X,
                (int)Top.Y - 10,
                healthBarRectWidth(HealthBarFloat), 5),
                new Rectangle(0, 0, 2, 2),
                Color.White);
        }
    }
}
