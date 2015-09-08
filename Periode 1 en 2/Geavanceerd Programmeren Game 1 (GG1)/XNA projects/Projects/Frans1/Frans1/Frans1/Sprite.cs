using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Frans1
{
    public class Sprite
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }

        private Rectangle relativeBoundingBox = new Rectangle(0, 0, 0, 0);
        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle(
                    (int)Position.X + relativeBoundingBox.X,
                    (int)Position.Y + relativeBoundingBox.Y,
                    (relativeBoundingBox.Width > 0) ? relativeBoundingBox.Width : Texture.Width,
                    (relativeBoundingBox.Height > 0) ? relativeBoundingBox.Height : Texture.Height);
            }
        }

        public void SetRelativeBoundingBox(int xOffset, int yOffset, int width, int height)
        {
            relativeBoundingBox = new Rectangle(xOffset, yOffset, width, height);
        }

        public Sprite() { }

        public Sprite(Texture2D texture, Vector2 position)
        {
            Texture = texture;
            Position = position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(Texture != null)
            {
                spriteBatch.Draw(Texture, Position, Color.White);
            }
        }
    }
}
