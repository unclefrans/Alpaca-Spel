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
using FarseerPhysics.Dynamics;


/// <summary>
/// PLAN
/// Wat zijn wij nog van plan
/// 
/// Appels oppakken
/// Bomen dashen
/// Pauze
/// Speel opnieuw
/// Delta tijd ergens?
/// </summary>

/// <summary>
/// TODO
/// Wat moeten wij nog doen
/// (22-09-2014)
/// Scherm moet player volgen - <Frans>
/// Player moet op platform beginnen - <Frans>
///  Platform moet nu ook ergens anders beginnen - <Frans>
/// Platform spawnen formule aanpassen - <Frans>
///  Zodat deze in de buurt van player spawnt waarbij de player er bij kan
///  
/// Verschillende planeten met verschillende zwaartekrachten. Dit zijn dan de verschillende levels?
/// 
/// </summary>
namespace MAS
{
    /// <summary>
    /// GameStates class
    /// Het spel zal drie schermen hebben
    /// Start scherm met informatie
    /// Het speelscherm
    /// Game over scherm
    /// </summary>
    enum GameState
    {
        StartScreen,
        GameScreen,
        GameOverScreen,
    }

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Schermen die we gaan gebruiken
        StartScreen screen_start;
        GameScreen screen_game;
        GameOverScreen screen_gameover;
        // Dit is de huidige scherm
        GameState currentGameState;

        // Sprites
        // Maakt de wereld aan
        public World world;

        public DrawablePhysicsObject dpo_Floor, dpo_Player, dpo_Platform;

        // HUD with score
        public HUD hud;
        public int currentScore { get; set; }
        public float f_cameraY, f_cameraX, p;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Begint het spel,
        /// wanneer het spel begonnen is,
        /// zet hij het huidige scherm op spel scherm (game screen)
        /// 
        /// Misschien een soort refresh actie erin?
        /// </summary>
        public void StartGame()
        {
            // Hij hoort eigenlijk alles te resetten
            
            screen_game = new GameScreen(this.hud, this.world, this.Content, this.GraphicsDevice);
            currentGameState = GameState.GameScreen;

            world = new World(new Vector2(0, 9.8f));

            screen_game.LoadContent();
            
            screen_start = null; // ??
        }
        /// <summary>
        /// Eindigt het spel,
        /// een spel hoort te eindigen,
        /// hierin sluit hij het spel
        /// </summary>
        public void EndGame()
        {
            MediaPlayer.Stop();
            // Misschien een 'speel opnieuw' knopje?
            screen_gameover = new GameOverScreen(this);
            currentGameState = GameState.GameOverScreen;
            screen_game.world.Clear();
            // No more start screen
            screen_game = null;
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
            base.Initialize();
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
            currentGameState = GameState.StartScreen;

            // Het laden van de HUD met score
            hud = new HUD();
            hud.Font = Content.Load<SpriteFont>("Fonts/Arial");

            // Laadt het spel, dus het zal het titel scherm moeten laden
            screen_start = new StartScreen(this);
            currentGameState = GameState.StartScreen;

            // Objecten en lijsten maken
            world = new World(new Vector2(0, 5.6f));

            base.LoadContent();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            base.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// 
        /// Hierin wordt alle math en aanpassingen gedaan
        /// dit gebeurt niet in de draw
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            // De switch van speel schermen (currentGameState)
            #region switch
            switch (currentGameState)
            {
                // The title screen, when game starts
                case GameState.StartScreen:
                    screen_start.Update();
                    hud.scorePos.Y = 0;
                    f_cameraY = 0;
                    break;

                // Het speelscherm, hierin speel je
                case GameState.GameScreen:
                    screen_game.Update(gameTime);
                    this.f_cameraY = screen_game.HandleCamera(this.f_cameraY);
                    this.hud.scorePos.Y = screen_game.GivePlayerPos() - ((this.GraphicsDevice.Viewport.Height / 2) - 20);
                    this.hud.Update(gameTime);
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape) || screen_game.HandleGameOver() == true)
                        this.EndGame();
                    break;

                // Game over scherm, wanneer je dood bent
                case GameState.GameOverScreen:
                    f_cameraY = 0;
                    hud.scorePos.Y = 0;
                    screen_gameover.Update();
                    break;
            }
            #endregion
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin(0,
                null, null, null, null, null,
                Matrix.CreateTranslation(f_cameraX, f_cameraY, 0));

            switch (currentGameState)
            {
                case GameState.StartScreen:
                    screen_start.Draw(spriteBatch);
                    break;
                case GameState.GameScreen:
                    screen_game.Draw(spriteBatch);
                    break;
                case GameState.GameOverScreen:
                    screen_gameover.Draw(spriteBatch);
                    break;
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}