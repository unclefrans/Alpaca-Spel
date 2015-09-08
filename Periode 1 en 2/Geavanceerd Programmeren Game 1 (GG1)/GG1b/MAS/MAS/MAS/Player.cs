using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;

namespace MAS
{
    class Player : Object
    {
        public DrawablePhysicsObject playerBody;
        Vector2 jumpForce = new Vector2(0, (float)-1.8);
        int jumpCounter = 0;
        SoundEffect jumpSound;

        float timeDashing = 1.5F;
        bool allowedDashing = false;

        public Player(DrawablePhysicsObject playerBody,  ContentManager Content)
        {
            this.playerBody = playerBody;
            this.playerBody.body.BodyType = BodyType.Dynamic;
            this.playerBody.body.FixedRotation = true;
            this.playerBody.body.Awake = true;
            this.jumpSound = Content.Load<SoundEffect>("Audio/JumpAlpaca");
        }

        public void Update(GameTime gameTime, KeyboardState curKeyboardState, KeyboardState prevKeyboardState, List<Platform> list_Platform, int score)
        {
            if (curKeyboardState.IsKeyDown(Keys.Z) && !prevKeyboardState.IsKeyDown(Keys.Z) && this.jumpCounter < 1 && this.playerBody.body.Awake == true)
            {
                this.jumpCounter++;
                this.playerBody.body.ApplyLinearImpulse(ref jumpForce);
                this.jumpSound.Play();
            }
            else
            {
                this.playerBody.body.Awake = true;
            }
            if (this.playerBody.body.ContactList != null)
                this.jumpCounter = 0;

            Console.WriteLine(jumpCounter);

            // De duratie van een dash is 0.05
            // Na een dash moet het weer terug naar de oude snelheid
            timeDashing += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timeDashing >= 0.05F)
            {
                allowedDashing = false;
                // Zet snelheid platformen weer goed
                foreach (Platform platform in list_Platform)
                {
                    platform.platformSpeed = 4 + (score / 1000); // SNELHEID AANPASSEN AAN DE HAND VAN DE SCORE
                }
            }
            //Console.WriteLine(timeDashing);
            if (curKeyboardState.IsKeyDown(Keys.X) && !prevKeyboardState.IsKeyDown(Keys.X))
            {
                // Tijd tussen elke dash is 2 seconden
                if (timeDashing >= 2F)
                {
                    allowedDashing = true; // Nu mag hij de bomen stuk maken
                    timeDashing = 0f;
                    // Tijdens zo'n dash moeten de platformen wat sneller gaan
                    foreach (Platform platform in list_Platform)
                    {
                        platform.platformSpeed = 40;
                    }
                    this.playerBody.body.Awake = false;
                    //Console.WriteLine(allowedDashing);
                }
                else
                    allowedDashing = false; // Nu mag hij de bomen niet stuk maken
            }
        }

        public void Draw(SpriteBatch spritebatch)
        {
            this.playerBody.Draw(spritebatch);
        }
    }
}
