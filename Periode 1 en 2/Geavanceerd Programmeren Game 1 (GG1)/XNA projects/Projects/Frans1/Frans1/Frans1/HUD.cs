using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Frans1
{
    class HUD
    {
        private Vector2 scorePos = new Vector2(20, 10);

        public SpriteFont Font { get; set; }

        public int Score { get; set; }

        public HUD()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(
                Font,
                "Score: " + Score.ToString(),
                scorePos,
                Color.Black);
        }
    }
}
