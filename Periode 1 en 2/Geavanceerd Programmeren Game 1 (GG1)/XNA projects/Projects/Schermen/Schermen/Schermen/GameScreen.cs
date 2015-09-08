using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Schermen
{
    public class GameScreen : Game1
    {
        Game1 game;
        Song song;

        public GameScreen(Game1 game)
        {
            this.game = game;

            song = game.Content.Load<Song>("yes");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(song);
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
