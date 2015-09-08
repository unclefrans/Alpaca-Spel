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
        public bool dashState = false;

        public Player(DrawablePhysicsObject playerBody,  ContentManager Content)
        {
            // Zet de eigenschappen van het playerobject goed
            this.playerBody = playerBody;
            this.playerBody.body.BodyType = BodyType.Dynamic;
            this.playerBody.body.FixedRotation = true;
            this.playerBody.body.Awake = true;
            this.jumpSound = Content.Load<SoundEffect>("Audio/JumpAlpaca");
        }

        public void Update(GameTime gameTime, KeyboardState curKeyboardState, KeyboardState prevKeyboardState, List<Platform> list_Platform, int score)
        {
            #region Jump
            // Kijkt of de z-toets is ingedrukt en er nog sprongen zijn toegestaan
            if (curKeyboardState.IsKeyDown(Keys.Z) && !prevKeyboardState.IsKeyDown(Keys.Z) && this.jumpCounter < 1 && this.playerBody.body.Awake == true)
            {
                // De jumpCounter houdt bij hoe vaak de speler al geeft gesprongen
                this.jumpCounter++;
                this.playerBody.body.ApplyLinearImpulse(ref jumpForce);
                this.jumpSound.Play();
            }
            else
            {
                // Zorgt ervoor dat de player weer naar beneden valt
                this.playerBody.body.Awake = true;
            }

            // Zodra de speler in contact komt met een fysiek element word de jumpCounter weer op nul gezet
            if (this.playerBody.body.ContactList != null)
                this.jumpCounter = 0;
            #endregion

            #region Dash
            // De duratie van een dash is 0.05
            // Na een dash moet het weer terug naar de oude snelheid
            this.timeDashing += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (this.timeDashing >= 0.08F)
            {
                this.dashState = false;
                // Zet snelheid platformen weer goed
                foreach (Platform platform in list_Platform)
                {
                    platform.f_platformSpeed = 4 + (score / 100); // SNELHEID AANPASSEN AAN DE HAND VAN DE SCORE
                }
            }
            //Console.WriteLine(timeDashing);
            if (curKeyboardState.IsKeyDown(Keys.X) && !prevKeyboardState.IsKeyDown(Keys.X))
            {
                // Tijd tussen elke dash is 2 seconden
                if (this.timeDashing >= 2F)
                {
                    this.dashState = true; // Nu mag hij de bomen stuk maken
                    this.timeDashing = 0f;
                    // Tijdens eenn dash moeten de platformen sneller gaan
                    foreach (Platform platform in list_Platform)
                    {
                        platform.f_platformSpeed = 40;
                    }
                    this.playerBody.body.Awake = false;
                    //Console.WriteLine(dashState);

                }
                else
                    this.dashState = false; // Nu mag hij de bomen niet stuk maken
            }
            #endregion
        }

        public void Draw(SpriteBatch spritebatch)
        {
            this.playerBody.Draw(spritebatch);
        }
    }
}
