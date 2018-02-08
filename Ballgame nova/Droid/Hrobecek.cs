using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MojehraDroid
{
    internal class Hrobecek
    {
        private Rectangle rect;
        private Texture2D sprite;
        private bool active;
        internal short obsah;
        private Color vyslednaBarva = Color.White;

        internal Hrobecek(bool zije, Rectangle obdelnik, Texture2D sprite)
        {
            active = zije;
            rect = obdelnik;

            this.sprite = sprite;
            rect.Width = rect.Height = sprite.Height;
        }

        internal void Nastav(Rectangle novy, short score)
        {
            rect = novy;
            active = true;
            obsah = score;
            if (score > 0) vyslednaBarva = Color.White;
            else vyslednaBarva = Color.Sienna;
        }

        internal void Odstran()
        {
            active = false;
            obsah = 0;
        }

        internal bool ZkontrolujMisto(Point location)
        {
            if (active)
            { 
                if (rect.Location == location) return true;
            }
            return false;
        }

        internal void Draw(SpriteBatch sb)
        {
            if (active) sb.Draw(sprite, rect, vyslednaBarva);
        }
    }
}