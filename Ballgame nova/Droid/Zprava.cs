using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MojehraDroid
{
    public class Zprava
    {
        private Vector2 souradnice;
        private string zprava;
        private SpriteFont font;
        private Color barva;
        private int trvani;
        public bool hotova, vpyj, odpyj;
        //private bool animovan;
        //private Vector2 scale, stredOtaceni, pozice;
        //private float rotace; short kroku, celkemKroku;

        public Zprava(Vector2 poloha, string text, Color barva, int trvaniZpravy, bool vpyj, bool odpyj, SpriteFont font)
        {
            souradnice = poloha; zprava = text; this.font = font; trvani = trvaniZpravy; this.barva = barva;
            this.vpyj = vpyj; this.odpyj = odpyj;
        }

        internal void Update(int milliseconds)
        {
            trvani -= milliseconds;
            if (trvani <= 0) hotova = true;
            else
            {
                if (vpyj && barva.A != byte.MaxValue)
                    barva.A++;
                if (odpyj && trvani < 1000)
                   barva.A -= 4;
            }
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            if (!hotova)
                spriteBatch.DrawString(font, zprava, souradnice, barva);
        }
    }
}