using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MAS
{
    class ScrollingBackground
    {
        private Vector2 screenpos, origin, texturesize;
        public Texture2D myTexture;
        private int screenheight;
        private int screenwidth;

        public void Load(GraphicsDevice device, Texture2D backgroundTexture)
        {
            myTexture = backgroundTexture;
            screenheight = device.Viewport.Height;
            screenwidth = device.Viewport.Width;
            origin = new Vector2(myTexture.Width / 2, 0);
            screenpos = new Vector2(screenwidth / 2, 0);
            texturesize = new Vector2(myTexture.Width, 0);
        }

        public void Update(float deltaX)
        {
            screenpos.X -= deltaX;
            screenpos.X = screenpos.X % myTexture.Width;
        }

        public void Draw(SpriteBatch batch)
        {
            if (screenpos.X < screenwidth)
                batch.Draw(myTexture, screenpos, null, Color.White, 0, origin, 1, SpriteEffects.None, 0f);
            batch.Draw(myTexture, screenpos + texturesize, null, Color.White, 0, origin, 1, SpriteEffects.None, 0f);
        }
    }
}
