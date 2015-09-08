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
        DrawablePhysicsObject treeBody;

        public Tree(DrawablePhysicsObject treeBody)
        {
            this.treeBody = treeBody;
            this.treeBody.body.BodyType = BodyType.Static;
            this.treeBody.body.FixedRotation = true;
        }
    }
}
