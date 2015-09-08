using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace MAS
{
    class StartScreen
    {
        // Define a texture, the start screen image
        private Texture2D texture;
        // Get Game1 class
        private Game1 game;

        public StartScreen(Game1 game)
        {
            this.game = game;
            // Laad start scherm plaatje
            texture = game.Content.Load<Texture2D>("Pictures/TitleScreen");
        }

        public void Update()
        {
            // Wanneer op key gedrukt,
            // begin spel
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Enter))
                game.StartGame();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Vector2(0f, 0f), Color.White);
        }
    }
}
