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
    class Tree : Object
    {
        public DrawablePhysicsObject treeBody;
        public bool isHit = false;
        Random random = new Random();

        public Tree(DrawablePhysicsObject treeBody, DrawablePhysicsObject platformBody )
        {
            this.treeBody = treeBody;
            this.treeBody.body.BodyType = BodyType.Static;
            this.treeBody.body.FixedRotation = true;

            // De x-positie is een willekeurig getal over de breedde van de platform waar de boom op komt
            int positionX = this.random.Next((int)platformBody.Position.X, (int)(platformBody.Position.X + platformBody.Size.X / 2) - (int)this.treeBody.Size.X);
            // De y-positie is precies op het platform waar de boom op komt
            int positionY = (int)((platformBody.Position.Y - (this.treeBody.Size.Y / 2) - platformBody.Size.Y / 2) + 1);
            this.treeBody.Position = new Vector2(positionX, positionY);
        }

        public void Update(float f_platformSpeed)
        {
            // De boom beweegt met dezelfde snelheid als het platform
            this.treeBody.Position -= new Vector2(f_platformSpeed, 0);
            if (this.treeBody.body.ContactList != null)
                this.isHit = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.treeBody.Draw(spriteBatch);
        }
    }
}
