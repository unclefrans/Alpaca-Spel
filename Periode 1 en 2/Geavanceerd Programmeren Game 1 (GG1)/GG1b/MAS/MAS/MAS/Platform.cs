using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;

namespace MAS
{
    class Platform : Object
    {
        public DrawablePhysicsObject platformBody;
        public float platformSpeed = 4;
        public List<Object> list_platformItems = new List<Object>();

        public Platform(DrawablePhysicsObject platformBody)
        {
            this.platformBody = platformBody;
            this.platformBody.body.BodyType = BodyType.Static;
            this.platformBody.body.FixedRotation = true;
        }

        public void Update(GameTime gameTime, int score)
        {
            this.platformBody.Position -= new Vector2(this.platformSpeed, 0);
            if (this.platformSpeed != 40)
                this.platformSpeed = (4 + (score / 500));
            //Console.Write(this.platformSpeed);
        }

        public void Draw(SpriteBatch spritebatch)
        {
            this.platformBody.Draw(spritebatch);
        }
    }
}
