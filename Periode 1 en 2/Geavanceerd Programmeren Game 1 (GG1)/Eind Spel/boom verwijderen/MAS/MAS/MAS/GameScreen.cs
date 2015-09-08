using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;


namespace MAS
{
    public class GameScreen
    {
        // Muziek
        Song song;
        // Get Game1 class
        HUD hud;
        public World world;
        KeyboardState curKeyboardState, prevKeyboardState;
        Random random;
        ContentManager Content;
        GraphicsDevice graphics;

        Platform platform;
        Player player;

        List<Platform> list_Platform = new List<Platform>();
        List<Platform> list_ToRemove = new List<Platform>();

        private ScrollingBackground background;

        float f_timer = 0f;
        float f_platformTimeInterval = 2f;

        private GameScreen gameScreen;

        public GameScreen(HUD hud, World world, ContentManager content, GraphicsDevice graphics)
        {
            this.Content = content;
            this.world = world;
            this.hud = hud;
            this.graphics = graphics;
            // Laad de muziek
            this.song = this.Content.Load<Song>("Audio/Soundtrack");
            
            // Repeat de muziek
            MediaPlayer.IsRepeating = true;
            // Plays de muziek
            MediaPlayer.Play(song);
        }

        public GameScreen(GameScreen gameScreen)
        {
            // TODO: Complete member initialization
            this.gameScreen = gameScreen;
        }

        public void LoadContent()
        {
            this.hud.Score = 0;
            this.hud.timer_sec = 0;

            // de vloer (zal verwijdert worden!
            /*
            game.dpo_Floor = new DrawablePhysicsObject(game.world, game.Content.Load<Texture2D>("Floor"), new Vector2(game.GraphicsDevice.Viewport.Width, 2f), 1000);
            game.dpo_Floor.Position = new Vector2(game.GraphicsDevice.Viewport.Width / 2.0f, game.GraphicsDevice.Viewport.Height);
            game.dpo_Floor.body.BodyType = BodyType.Static; // Will interact with other objects but won't change unless told to by user
            */
            // de speler

            /// <summary>
            /// Begin van het platformen trappetje,
            /// dit is omdat de platformen later worden gemaakt,
            /// en anders ben je al meteen af
            /// </summary>

            this.random = new Random();

            // Maakt de speler aan en geeft hem zijn beginpositie mee
            this.player = new Player(new DrawablePhysicsObject(this.world, this.Content.Load<Texture2D>("Pictures/Alpaca"), new Vector2(75, 75), 0.1f), this.Content);
            this.player.playerBody.Position = new Vector2(50, 50);

            #region
            // De speler spawnt op een trappetje van platform zodra het spel begint, dit doen we zodat je zeker bent van een goed begin
            // De beginposities van het trappetje
            float platformX = 300;
            float platformY = this.Content.Load<Texture2D>("Pictures/levelBackground").Height / 1.5f;

            // Maakt het trappetje met platformen waar de speler op begint
            for (int i = 0; i < 3; i++)
            {
                this.platform = new Platform(new DrawablePhysicsObject(this.world, this.Content.Load<Texture2D>("Pictures/Platform"), new Vector2(400, 50), 0.1f));
                this.platform.platformBody.Position = new Vector2(platformX, platformY);
                this.list_Platform.Add(this.platform);
                platformX += 500;
                platformY += 20;
            }
            #endregion

            //
            this.background = new ScrollingBackground();
            this.background.Load(graphics, Content.Load<Texture2D>("Pictures/levelBackground"));

            // Remove list for apples
            /*
            DrawablePhysicsObject toRemove = null;
            foreach (DrawablePhysicsObject apple in appleList)
            {
                toRemove = apple;
                break;
            }
            if (toRemove != null)
                appleList.Remove(toRemove);
            */
        }

        /// <summary>
        /// Behandelen van de speler input
        /// Z = spring
        /// TODO:
        /// f_cameraX = dash
        /// ESC = pauze?
        /// </summary>

        /// <summary>
        /// Het maken van platformen om op te springen
        /// In het begin hoort hij in het midden te beginnen,
        /// met de speler er al op
        /// </summary>
        public void SpawnPlatform(GameTime gameTime)
        {
            this.world.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
            this.f_timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            var y = this.list_Platform.Last().platformBody.Position.Y;

            if (this.f_timer >= this.f_platformTimeInterval)
            {
                this.platform = new Platform(new DrawablePhysicsObject(this.world, this.Content.Load<Texture2D>("Pictures/Platform"), new Vector2(400, 50), 1000f));
                this.platform.platformBody.Position = new Vector2(this.graphics.Viewport.Width + this.platform.platformBody.texture.Width, (y + random.Next(-100, 100)));
                this.platform.CreateItems(this.world, this.Content);
                /*while (this.platform.platformBody.Position.Y >= 50 || this.platform.platformBody.Position.Y <= (this.background.myTexture.Height - 50))
                    this.platform.platformBody.Position = new Vector2(this.graphics.Viewport.Width + this.platform.platformBody.texture.Width, (y + random.Next(-100, 100)));*/
                this.list_Platform.Add(platform);
                this.f_timer = 0f;
            }
        }
        /// <summary>
        /// Behandelen van de al gespawnde platformen
        /// verwijderen van buiten scherm platform
        /// bewegen van de platformen
        /// </summary>
        public void HandlePlatform()
        {
            if (this.list_ToRemove.Count > 0)
            {
                foreach (Platform platform in this.list_ToRemove.ToList())
                {
                    this.list_ToRemove.Remove(platform);
                }
            }

            foreach (Platform platform in this.list_Platform)
            {
                if (platform.platformBody.Position.X + platform.platformBody.texture.Width < 0)
                {
                    this.list_ToRemove.Add(platform);
                }
            }
        }
        
        /// <summary>
        /// Level difficulty
        /// Bepalend hoeveel score je hebt,
        /// zal het spel moeilijker worden
        /// 
        /// Tijd interval van wanneer de platformen
        /// worden gespawned zal ook verandert moeten worden.
        /// 
        /// Snelheid van de platformen
        /// Kan formule van gemaakt worden
        /// </summary>
       
        public void Update(GameTime gameTime)
        {
            this.world.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
            this.curKeyboardState = Keyboard.GetState();
            this.SpawnPlatform(gameTime);

            foreach (Platform platform in this.list_Platform)
            {
                platform.Update(gameTime, this.hud.Score, this.player.dashState);
            }
            this.player.Update(gameTime, this.curKeyboardState, this.prevKeyboardState, this.list_Platform, this.hud.Score);
            this.HandlePlatform();


            /*// f_cameraY Camera Boven Limiet
            game.f_cameraY = (game.GraphicsDevice.Viewport.Height / 2) - Convert.ToInt32(game.dpo_Player.Position.f_cameraY);
            if (game.f_cameraY >= 0) game.f_cameraY = 0;
            // f_cameraY Camera Onder Limiet
            if ((game.GraphicsDevice.Viewport.Height / 2) + Convert.ToInt32(game.dpo_Player.Position.f_cameraY) >= 800)
                game.f_cameraY = -320;
            // HUD score gaat mee
            game.hud.scorePos.f_cameraY = game.dpo_Player.Position.f_cameraY - ((game.GraphicsDevice.Viewport.Height / 2) - 20);
            if (game.hud.scorePos.f_cameraY <= 20) game.hud.scorePos.f_cameraY = 20;
            if (game.hud.scorePos.f_cameraY >= 340) game.hud.scorePos.f_cameraY = 340;

            HandlePlatform();
            HandleInput();
            HandleGameOver();
            game.hud.Update(gameTime);
            SpawnPlatform(gameTime);*/
            this.prevKeyboardState = this.curKeyboardState;
            background.Update((float)gameTime.ElapsedGameTime.TotalSeconds * 500);
        }

        public float HandleCamera(float f_cameraY)
        {
            if (((this.graphics.Viewport.Height / 2) - Convert.ToInt32(this.player.playerBody.Position.Y)) <= 0)
            {
                if (((this.graphics.Viewport.Height / 2) + Convert.ToInt32(this.player.playerBody.Position.Y)) <= 800)
                    f_cameraY = (this.graphics.Viewport.Height / 2) - Convert.ToInt32(this.player.playerBody.Position.Y);
                else
                    f_cameraY = -320;
            }
            else
                f_cameraY = 0;

            return f_cameraY;
        }
        /// <summary>
        /// Bepalen wanneer het spel voorbij is
        /// - Geraakt door zij kant platform
        /// - Geraakt door onder kant platform
        /// - Valt naar beneden uit scherm
        /// - ?? Geraakt door boer's kogel
        /// </summary>
        public bool HandleGameOver()
        {
            // Geraakt door zij kant platform
            bool b_setGameOver = false;
            if (this.player.dashState == true)
            {
                if (this.player.playerBody.Position.X < 50 || this.player.playerBody.Position.Y > this.background.myTexture.Height)
                    b_setGameOver = true;
            }
                
            // Geraakt door onder kant platform
            //if (game.dpo_Platform.body.)
            // Valt naar beneden uit scherm
            //if (game.dpo_Player.Position.f_cameraY > game.GraphicsDevice.Viewport.Height)
            //    game.EndGame();
            return b_setGameOver;
        }

        public float GivePlayerPos()
        {
            return this.player.playerBody.Position.Y;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.background.Draw(spriteBatch);
            foreach (Platform platform in this.list_Platform)
                platform.Draw(spriteBatch);
            this.player.Draw(spriteBatch);
            //game.dpo_Floor.Draw(spriteBatch);
            this.hud.Draw(spriteBatch);
        }
    }
}
