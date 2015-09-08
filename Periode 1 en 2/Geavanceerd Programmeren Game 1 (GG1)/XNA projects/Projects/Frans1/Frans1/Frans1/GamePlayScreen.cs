using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;

namespace Frans1
{
    public class GamePlayScreen : Game1
    {
        private Game1 game;

        Sprite player;
        Texture2D cakeTexture;
        List<Sprite> cakeList = new List<Sprite>();

        float timer = 0f;
        float dropInterval = 2f;
        float speed = 4f;

        Random random;

        HUD hud;

        Song song;
        SoundEffect soundEffect;

        public GamePlayScreen(Game1 game)
        {
            this.game = game;

            random = new Random();

            cakeTexture = game.Content.Load<Texture2D>("Cake");

            player = new Sprite();
            player.Texture = game.Content.Load<Texture2D>("Face");

            // Zoekt het midden op beiden de y op
            int screenCenterY = game.GraphicsDevice.Viewport.Height / 2;
            player.Position = new Vector2(
                10,                                             // x- as
                (screenCenterY) - (player.Texture.Height / 2)); // y-as
            // Player mond is collision met cake
            player.SetRelativeBoundingBox(20, 74, player.Texture.Width - 40, 20);
            
            // Score
            hud = new HUD();
            hud.Font = game.Content.Load<SpriteFont>("Arial");
        
            soundEffect = game.Content.Load<SoundEffect>("bite");
            song = game.Content.Load<Song>("yes");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(song);
        }

        protected override void Update(GameTime gameTime)
        {
           timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer >= dropInterval)
            {
                int yPos = random.Next(GraphicsDevice.Viewport.Height - 50);
                cakeList.Add(new Sprite(cakeTexture, new Vector2(GraphicsDevice.Viewport.Width + 100, yPos)));
                timer = 0f;
            }

            HandleCollisions();

            HandleFallingCake();

            HandleInput();
        }

        private void HandleInput()
        {
            // Player movements
            // Haalt huidige staat toetsenbord op
            KeyboardState keyboardState = Keyboard.GetState();
            Vector2 playerVelocity = Vector2.Zero;
            // Checks voor boven toets input
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                playerVelocity += new Vector2(0, -speed);
            }
            // Checks voor onderen toets input
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                playerVelocity += new Vector2(0, speed);
            }
            // Voegt de snelheid toe aan de sprites positie
            player.Position += playerVelocity;

            // Voorkomen dat player niet buiten scherm komt
            // Boven
            if (player.Position.Y < 0)
            {
                player.Position = new Vector2(player.Position.X, 0);
            }
            // Onderen
            int bottomEdge = GraphicsDevice.Viewport.Height - player.Texture.Height;
            if (player.Position.Y > bottomEdge)
            {
                player.Position = new Vector2(player.Position.X, bottomEdge);
            }
        }

        private void HandleFallingCake()
        {
            List<Sprite> toRemove = new List<Sprite>();

            foreach (Sprite cake in cakeList)
            {
                if (cake.Position.Y > (GraphicsDevice.Viewport.Height + 20))
                {
                    toRemove.Add(cake);
                }
                else
                {
                    cake.Position += new Vector2(-speed, 0);
                }
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

                    soundEffect.Play();

                    hud.Score += 1;
                    break;
                }
            }

            if (toRemove != null)
            {
                cakeList.Remove(toRemove);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            player.Draw(spriteBatch);

            foreach (Sprite cake in cakeList)
            {
                cake.Draw(spriteBatch);
            }

            hud.Draw(spriteBatch);
        }
    }
}
