using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Content;

namespace MAS
{
    class Platform : Object
    {
        public DrawablePhysicsObject platformBody;
        public float f_platformSpeed = 4;

        // Elk platform heeft een lijst waar de voorwerpen die op het platform liggen in zitten
        public List<Tree> list_platformItems = new List<Tree>();
        Random random = new Random();


        public Platform(DrawablePhysicsObject platformBody)
        {
            this.platformBody = platformBody;
            this.platformBody.body.BodyType = BodyType.Static;
            this.platformBody.body.FixedRotation = true;
        }

        public void CreateItems(World world, ContentManager content)
        {
            // Een willekeurig getal bepaalt of er een boom op het platform staat op niet
            if (this.random.Next(0, 2) == 1)
                this.list_platformItems.Add(new Tree(new DrawablePhysicsObject(world, content.Load<Texture2D>("Pictures/tree"), new Vector2(127, 173), 1000F), this.platformBody));
        }

        public void Update(GameTime gameTime, int score, bool allowRemove)
        {
                //float j = this.platformSpeed;
                // if (j != this.platformSpeed)
                //     this.platformTimeInterval -= 0.2f;

            this.platformBody.Position -= new Vector2(this.f_platformSpeed, 0);
            foreach (Tree tree in this.list_platformItems)
                tree.Update(this.f_platformSpeed);

            if (this.f_platformSpeed != 40)
                this.f_platformSpeed = (4 + (score / 700));

            foreach (Tree tree in this.list_platformItems)
            {
                if (tree.isHit == true)
                    this.list_platformItems.Remove(tree);
            }

            if (allowRemove == true)
            {
                Tree toRemove = null;
                foreach (Tree tree in this.list_platformItems)
                {
                    if (tree.treeBody.body.ContactList != null)
                    {
                        toRemove = tree;
                        break;
                    }
                }
                if (toRemove != null)
                    this.list_platformItems.Remove(toRemove);
            }

        }

        public void Draw(SpriteBatch spritebatch)
        {
            this.platformBody.Draw(spritebatch);
            foreach (Tree tree in this.list_platformItems)
                tree.Draw(spritebatch);
        }
    }
}
