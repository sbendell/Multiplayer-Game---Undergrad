using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Lidgren.Network;
using System;
using System.Collections.Generic;

namespace Client
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        enum GameStates { TitleScreen, GameLobby, PlayingState, EndGameScreen };
        GameStates gameState = GameStates.TitleScreen;

        public static Texture2D ironManTexture;
        public static Texture2D captainAmericaTexture;
        public static Texture2D starlordTexture;
        public static Texture2D warmachineTexture;
        public static Texture2D healthTexture;

        Texture2D titlescreen;

        Texture2D snowMountainBackground;
        Texture2D greenForestBackground;
        Texture2D snowy4block;
        Texture2D snowy8block;
        Texture2D forest4block;
        Texture2D forest8block;
        Texture2D chatBackground;
        Texture2D muricaShield;
        Texture2D redLaser;
        Texture2D blueLaser;
        Texture2D greenLaser;

        string currentMap = "SnowMap";
        
        KeyboardState CurrentKey, PrevKey;
        MouseState ms, prevms;

        Login loginForm;
        ChatBox chatBox;

        SpriteFont sf;

        Rectangle playButton;
        Rectangle exitButton;

        ShotManager shotManager;
        CollisionManager collisionManager;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Network.Config = new NetPeerConfiguration("game");
            Network.Client = new NetClient(Network.Config);
            loginForm = new Login();
            Window.Title = "Smash Bros 6.0";
            chatBox = new ChatBox();
            base.Initialize();
            playButton = new Rectangle(this.Window.ClientBounds.Width / 2 - 90, this.Window.ClientBounds.Height / 2 - 70, 180, 70);
            exitButton = new Rectangle(this.Window.ClientBounds.Width / 2 - 90, this.Window.ClientBounds.Height / 2, 180, 70);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            titlescreen = Content.Load<Texture2D>("TitleScreen");
            snowMountainBackground = Content.Load<Texture2D>("SnowyMountainBackground");
            greenForestBackground = Content.Load<Texture2D>("GreenForestBackground");
            forest4block = Content.Load<Texture2D>("GreenTile4Long");
            forest8block = Content.Load<Texture2D>("GreenTile8Long");
            snowy4block = Content.Load<Texture2D>("SnowyTile4Long");
            snowy8block = Content.Load<Texture2D>("SnowyTile8Long");
            captainAmericaTexture = Content.Load<Texture2D>("captainamerica");
            ironManTexture = Content.Load<Texture2D>("ironman");
            starlordTexture = Content.Load<Texture2D>("starlord");
            warmachineTexture = Content.Load<Texture2D>("warmachine");
            healthTexture = Content.Load<Texture2D>("redpixel");
            chatBackground = Content.Load<Texture2D>("chatBackground");
            sf = Content.Load<SpriteFont>("NewSpriteFont");
            muricaShield = Content.Load<Texture2D>("muricashot");
            redLaser = Content.Load<Texture2D>("redlaser");
            blueLaser = Content.Load<Texture2D>("bluelaser");
            greenLaser = Content.Load<Texture2D>("greenlaser");

            GameLobby.LoadContent(Content);
            shotManager = new ShotManager(muricaShield, redLaser, blueLaser, greenLaser);
            collisionManager = new CollisionManager(new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            PrevKey = CurrentKey;
            CurrentKey = Keyboard.GetState();
            prevms = ms;
            ms = Mouse.GetState();
            string username = Login.userName;

            switch (gameState)
            {
                case GameStates.TitleScreen:
                    if (playButton.Contains(ms.Position))
                    {
                        if (ms.LeftButton == ButtonState.Pressed && prevms.LeftButton == ButtonState.Released)
                        {
                            loginForm.Show();
                            System.Threading.Thread.Sleep(300);
                            gameState = GameStates.GameLobby;
                        }
                    }
                    if (exitButton.Contains(ms.Position))
                    {
                        if (ms.LeftButton == ButtonState.Pressed && prevms.LeftButton == ButtonState.Released)
                        {
                            Exit();
                        }
                    }
                    break;
                case GameStates.GameLobby:
                    Network.Update(this);
                    ChatManager.Update();
                    GameLobby.Update(gameTime, ms);

                    if (Login.IsConnected)
                    {
                        if (CurrentKey.IsKeyDown(Keys.E) && PrevKey.IsKeyUp(Keys.E))
                        {
                            chatBox.Show();
                        }
                    }

                    if (GameLobby.timer <= 0)
                    {
                        Random randX = new Random();
                        Random randY = new Random();
                        int x, y;

                        x = randX.Next(0, 500);
                        y = randY.Next(0, 400);

                        Network.outmsg = Network.Client.CreateMessage();
                        Network.outmsg.Write("CharacterSelection");
                        Network.outmsg.Write(Login.userName);
                        Network.outmsg.Write(GameLobby.selectedHero);
                        Network.outmsg.Write(x);
                        Network.outmsg.Write(y);
                        Network.Client.SendMessage(Network.outmsg, NetDeliveryMethod.ReliableOrdered);
                        gameState = GameStates.PlayingState;
                    }
                    break;
                case GameStates.PlayingState:
                    Network.Update(this);
                    collisionManager.Update(this);
                    Player.Update(gameTime);
                    shotManager.Update();
                    break;
                case GameStates.EndGameScreen:
                    Network.Update(this);
                    break;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            if (gameState == GameStates.TitleScreen)
            {
                spriteBatch.Draw(titlescreen,
                    new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height),
                    Color.White);
            }
            if (gameState == GameStates.GameLobby)
            {
                GameLobby.Draw(spriteBatch, sf);
                ChatManager.Draw(spriteBatch, sf, chatBackground);
            }
            if (gameState == GameStates.PlayingState)
            {
                if (currentMap == "SnowMap")
                {
                    spriteBatch.Draw(snowMountainBackground,
                        new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height),
                        Color.White);
                    collisionManager.Draw(spriteBatch, snowy4block, snowy8block);
                }
                if (currentMap == "ForestMap")
                {
                    spriteBatch.Draw(greenForestBackground,
                        new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height),
                        Color.White);
                    collisionManager.Draw(spriteBatch, forest4block, forest8block);
                }
                Player.Draw(spriteBatch);
                shotManager.Draw(spriteBatch);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
