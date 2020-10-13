using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Square
{
    public class Hrobecek
    {
        private readonly Texture2D sprite;
        private Rectangle rect;        
        private bool active;        
        private Color vyslednaBarva = Color.White;

        public short Obsah { get; internal set; }

        public Hrobecek(bool zije, Rectangle obdelnik, Texture2D sprite)
        {
            active = zije;
            rect = obdelnik;

            this.sprite = sprite;
            rect.Width = rect.Height = sprite.Height;
        }

        public void Nastav(Rectangle novy, short score)
        {
            rect = novy;
            active = true;
            Obsah = score;
            if (score > 0) 
                vyslednaBarva = Color.White;
            else 
                vyslednaBarva = Color.Sienna;
        }

        public void Odstran()
        {
            active = false;
            Obsah = 0;
        }

        public bool ZkontrolujMisto(Point location)
        {
            if (active)
            { 
                if (rect.Location == location) 
                    return true;
            }

            return false;
        }

        public void Draw(SpriteBatch sb)
        {
            if (active) 
                sb.Draw(sprite, rect, vyslednaBarva);
        }
    }
}