using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MAS
{
    public class HUD
    {
        // Links boven
        public Vector2 scorePos = new Vector2(20, 10);
        public int Score { get; set; }
        // Er is een SpriteFont class gemaakt met font info
        public SpriteFont Font { get; set; }

        public float timer_sec;

        public HUD()
        {

        }

        public void AddScore()
        {
            Score += 1;
        }

        public void Update(GameTime gameTime)
        {
            timer_sec += ((float)gameTime.ElapsedGameTime.TotalMilliseconds) / 10;

            if (timer_sec >= 1.0F)
            {
                AddScore();
                timer_sec = 0F;
            }

            if (this.scorePos.Y <= 20)
                this.scorePos.Y = 20;

            if (this.scorePos.Y >= 340)
                this.scorePos.Y = 340;


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the Score in the top-left of screen
            spriteBatch.DrawString(
                Font,                          // SpriteFont
                "Score: " + Score.ToString(),  // Text
                scorePos,                      // Position
                Color.Black);                  // Tint
        }
    }
}
