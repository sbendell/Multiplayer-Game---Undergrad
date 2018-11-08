using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Lidgren.Network;

namespace Client
{
    static class GameLobby
    {
        static Texture2D background;
        static Texture2D ironmanIcon;
        static Texture2D americaIcon;
        static Texture2D starlordIcon;
        static Texture2D warmachineIcon;

        static Icon iron;
        static Icon star;
        static Icon murica;
        static Icon war;

        public static string selectedHero = "";
        public static float timer = 30000;

        static Rectangle ironRect = new Rectangle(535, 345, 70, 70);
        static Rectangle starRect = new Rectangle(745, 345, 70, 70);
        static Rectangle muricaRect = new Rectangle(535, 555, 70, 70);
        static Rectangle warRect = new Rectangle(745, 555, 70, 70);

        public static void LoadContent(ContentManager Content)
        {
            background = Content.Load<Texture2D>("GameLobby");
            ironmanIcon = Content.Load<Texture2D>("ironmanhead");
            americaIcon = Content.Load<Texture2D>("captainamericahead");
            starlordIcon = Content.Load<Texture2D>("starlordhead");
            warmachineIcon = Content.Load<Texture2D>("warmachinehead");

            Rectangle InitialFrame = new Rectangle(0, 0, 70, 70);

            iron = new Icon(ironmanIcon,
                new Vector2(535, 345),
                new Rectangle(0, 0, 70, 70));

            for (int x = 1; x < 2; x++)
            {
                iron.AddFrame(
                    new Rectangle(
                        InitialFrame.X + (InitialFrame.Width * x),
                        InitialFrame.Y,
                        InitialFrame.Width,
                        InitialFrame.Height));
            }

            star = new Icon(starlordIcon,
                new Vector2(745, 345),
                new Rectangle(0, 0, 70, 70));

            for (int x = 1; x < 2; x++)
            {
                star.AddFrame(
                    new Rectangle(
                        InitialFrame.X + (InitialFrame.Width * x),
                        InitialFrame.Y,
                        InitialFrame.Width,
                        InitialFrame.Height));
            }

            murica = new Icon(americaIcon,
                new Vector2(535, 555),
                new Rectangle(0, 0, 70, 70));

            for (int x = 1; x < 2; x++)
            {
                murica.AddFrame(
                    new Rectangle(
                        InitialFrame.X + (InitialFrame.Width * x),
                        InitialFrame.Y,
                        InitialFrame.Width,
                        InitialFrame.Height));
            }

            war = new Icon(warmachineIcon,
                new Vector2(745, 555),
                new Rectangle(0, 0, 70, 70));

            for (int x = 1; x < 2; x++)
            {
                war.AddFrame(
                    new Rectangle(
                        InitialFrame.X + (InitialFrame.Width * x),
                        InitialFrame.Y,
                        InitialFrame.Width,
                        InitialFrame.Height));
            }
        }

        public static void Update(GameTime gt, MouseState ms)
        {
            if (timer > 0)
            {
                timer -= gt.ElapsedGameTime.Milliseconds;
            }

            if (ironRect.Contains(ms.Position))
            {
                selectedHero = "Ironman";
            }
            if (muricaRect.Contains(ms.Position))
            {
                selectedHero = "America";
            }
            if (starRect.Contains(ms.Position))
            {
                selectedHero = "Starlord";
            }
            if (warRect.Contains(ms.Position))
            {
                selectedHero = "Warmachine";
            }

            if (selectedHero == "Ironman")
            {
                iron.currentFrame = 1;
                murica.currentFrame = 0;
                star.currentFrame = 0;
                war.currentFrame = 0;
            }
            if (selectedHero == "America")
            {
                iron.currentFrame = 0;
                murica.currentFrame = 1;
                star.currentFrame = 0;
                war.currentFrame = 0;
            }
            if (selectedHero == "Starlord")
            {
                iron.currentFrame = 0;
                murica.currentFrame = 0;
                star.currentFrame = 1;
                war.currentFrame = 0;
            }
            if (selectedHero == "Warmachine")
            {
                iron.currentFrame = 0;
                murica.currentFrame = 0;
                star.currentFrame = 0;
                war.currentFrame = 1;
            }
        }

        public static void Draw(SpriteBatch sb, SpriteFont sf)
        {
            sb.Draw(background, new Rectangle(0, 0, 1280, 900), Color.White);

            sb.DrawString(sf, timer.ToString(), new Vector2(620, 50), Color.White);

            iron.Draw(sb);
            star.Draw(sb);
            murica.Draw(sb);
            war.Draw(sb);
        }
    }
}
