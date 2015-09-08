using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Schermen
{
    class TitleScreen : Game1
    {
        private Texture2D texture;
        private Game1 game;
        private KeyboardState lastState;

        public TitleScreen(Game1 game)
        {
            this.game = game;
            texture = game.Content.Load<Texture2D>("TitleScreen");
            lastState = Keyboard.GetState();
        }

        public void Update()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Enter) && lastState.IsKeyUp(Keys.Enter))
            {
                game.StartGame();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Vector2(0f, 0f), Color.White);
        }
    }
}
