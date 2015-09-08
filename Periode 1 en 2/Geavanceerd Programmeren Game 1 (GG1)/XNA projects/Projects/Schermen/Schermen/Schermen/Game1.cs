using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Schermen
{
    /// <summary>
    /// Different screen states;
    /// TitleScreen, shows button input for player and start button
    /// GameScreen, the game itself
    /// GameoverScreen, when dead this screen shows
    /// </summary>
    enum GameState
    {
        TitleScreen,
        GameScreen,
        GameoverScreen,
    }

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        TitleScreen titlescreen;
        GameScreen gamescreen;
        GameoverScreen gameoverscreen;

        GameState currentGameState = GameState.TitleScreen;

        Sprite mSprite;

        Sprite player;
        Texture2D cakeTexture;
        List<Sprite> cakeList = new List<Sprite>();

        float timer = 0f;
        float dropInterval = 2f;
        float speed = 4f;

        Random random;

        HorizontallyScrollingBackground mScrollingBackground;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            mSprite = new Sprite();

            random = new Random();

            base.Initialize();
        }


        private void HandleInput()
        {
            // Retrieve the current state of the keyboard
            KeyboardState keyboardState = Keyboard.GetState();

            Vector2 playerVelocity = Vector2.Zero;

            // Check if the Z key is pressed and change the velocity of the character accordingly
            if (keyboardState.IsKeyDown(Keys.Z))
            {
                playerVelocity += new Vector2(0, -speed);
            }

            // Check if the X key is pressed and change the velocity of the character accordingly
            if (keyboardState.IsKeyDown(Keys.X))
            {
                playerVelocity += new Vector2(0, speed);
            }

            // Apply the velocity to the character's position
            player.Position += playerVelocity;

            // Prevent player from moving off the left edge of the screen
            if (player.Position.Y < 0)
                player.Position = new Vector2(player.Position.X, 0);

            // Prevent player from moving off the right edge of the screen
            int bottomEdge = GraphicsDevice.Viewport.Height - player.Texture.Height;
            if (player.Position.Y > bottomEdge)
                player.Position = new Vector2(player.Position.X, bottomEdge);
        }

        private void HandleMovingCake()
        {
            List<Sprite> toRemove = new List<Sprite>();

            foreach (Sprite cake in cakeList)
            {
                if (cake.Position.Y > (GraphicsDevice.Viewport.Height + 20))
                    toRemove.Add(cake);
                else
                    cake.Position += new Vector2(-speed, 0);
            }

            if (toRemove.Count > 0)
            {
                foreach (Sprite cake in toRemove)
                {
                    cakeList.Remove(cake);
                }
            }
        }

        private void HandleCollisions()
        {
            Sprite toRemove = null;

            foreach (Sprite cake in cakeList)
            {
                if (player.BoundingBox.Intersects(cake.BoundingBox))
                {
                    toRemove = cake;
                    break;
                }
            }

            if (toRemove != null)
                cakeList.Remove(toRemove);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            titlescreen = new TitleScreen(this);
            currentGameState = GameState.TitleScreen;

            // TODO: use this.Content to load your game content here
            //mSprite.LoadContent(this.Content, "WizardSquare");
            //mSprite.Position = new Vector2(125, 245);
            //mSprite.Position.X = GraphicsDevice.Viewport.Width / 2 - 50;
            //mSprite.Position.Y = GraphicsDevice.Viewport.Height / 2 - 50;

            // BACKGROUND
            mScrollingBackground = new HorizontallyScrollingBackground(this.GraphicsDevice.Viewport);
            mScrollingBackground.AddBackground("Background01");
            mScrollingBackground.AddBackground("Background02");
            mScrollingBackground.AddBackground("Background03");
            mScrollingBackground.AddBackground("Background04");
            mScrollingBackground.AddBackground("Background05");

            mScrollingBackground.LoadContent(this.Content);

            // TODO: use this.Content to load your game content here
            cakeTexture = Content.Load<Texture2D>("Platform");

            player = new Sprite();
            player.Texture = Content.Load<Texture2D>("WizardSquare");
            player.SetRelativeBoundingBox(20, 74, player.Texture.Width - 50, 20);

            player.Position = new Vector2(
                0, 
                GraphicsDevice.Viewport.Height - player.Texture.Height);
 

            base.LoadContent();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            switch (currentGameState)
            {
                case GameState.TitleScreen:
                        titlescreen.Update();
                        break;
                case GameState.GameScreen:
                        gamescreen.Update();
                        break;
                case GameState.GameoverScreen:
                        gameoverscreen.Update();
                        break;
            }

            //Update the scrolling backround. You can scroll to the left or to the right by changing the scroll direction
            mScrollingBackground.Update(gameTime, 160, HorizontallyScrollingBackground.HorizontalScrollDirection.Left);

            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer >= dropInterval)
            {
                int yPos = random.Next(GraphicsDevice.Viewport.Height - 50);

                cakeList.Add(new Sprite(cakeTexture, new Vector2(GraphicsDevice.Viewport.Width, yPos)));
                timer = 0f;
            }

            HandleMovingCake();

            HandleInput();

            HandleCollisions();

            base.Update(gameTime);
        }

        public void StartGame()
        {
            gamescreen = new GameScreen(this);
            currentGameState = GameState.GameScreen;

            titlescreen = null;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
 
            switch (currentGameState)
            {
                case GameState.TitleScreen:
                    titlescreen.Draw(spriteBatch);
                    break;
                case GameState.GameScreen:
                    //mScrollingBackground.Draw(spriteBatch);
                    mSprite.Draw(this.spriteBatch);
                    player.Draw(spriteBatch);

                    foreach (Sprite cake in cakeList)
                    {
                        cake.Draw(spriteBatch);
                    }

                    gamescreen.Draw(spriteBatch);
                    break;
                case GameState.GameoverScreen:
                    gameoverscreen.Draw(spriteBatch);
                    break;
            }

            spriteBatch.End(); 

            base.Draw(gameTime);
        }

        public Vector2 playerVelocity { get; set; }
    }
}
