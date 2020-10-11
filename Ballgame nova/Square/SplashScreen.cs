using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Square
{
    public class SplashScreen
    {
        /// <summary>
        /// by these main Update will decide whether to proceed
        /// </summary>
        public bool kresliSplash, provedUpdate;

        private static Rectangle splashRect;
        private static Texture2D splashScreen;
        private Color splashColor; private Color[] splash;
        private bool zvysStep, budeUpdate;
        private byte splashStep;
        
        private static ushort okrajX; private static ushort stredXoriznuty;
        private ushort vydrz;
        const ushort pozadovanaVydrz = 60;
        private string napis;
        private static SpriteFont pismo;
        private bool kreslitPismo;
        private Vector2 polohaNapisu;

        public SplashScreen(GraphicsDeviceManager graphics, Rectangle rect, SpriteFont font)
        {
            splashRect = rect;
            okrajX = (ushort)rect.X;
            stredXoriznuty = (ushort)(okrajX * .5f);
            splash = new Color[splashRect.Width * splashRect.Height];
            splashScreen = new Texture2D(graphics.GraphicsDevice, splashRect.Width, splashRect.Height);

            for (uint i = 0; i < splash.Length; ++i) 
                splash[i] = Color.Black;

            splashScreen.SetData(splash);
            splashColor = Color.White;

            pismo = font;
            polohaNapisu = rect.Center.ToVector2()*.8f;
            provedUpdate = true;
        }

        private void StahniStep()
        {
            splashStep -=3;
            splashColor.A = splashStep;
        }

        private void ZvedejStep()
        {
            splashStep +=3;
            splashColor.A = splashStep;
        }

        public void Update()
        {
            if (kresliSplash)
            {
                if (splashStep == byte.MaxValue)
                {
                    zvysStep = false;
                    if (vydrz > 0) 
                        vydrz--;
                    else if (budeUpdate)
                    {
                        if (!provedUpdate) 
                            provedUpdate = true;
                        else 
                            budeUpdate = provedUpdate = false;
                    }
                }
                else if (splashStep == byte.MinValue) // dokonceni
                {
                    kresliSplash = kreslitPismo = budeUpdate = false;
                    provedUpdate = true;
                    splashRect.X = okrajX;
                    splashColor.A = byte.MaxValue;
                }

                if (zvysStep) 
                    ZvedejStep();
                else if (!budeUpdate && !provedUpdate && vydrz == 0)
                    StahniStep();
            }
        }

        public void KresliSplash(bool okamzite, string text, bool zavedPriUpdejtu)
        {
            kresliSplash = true;
            vydrz = pozadovanaVydrz;
            splashRect.X = 0;
            polohaNapisu.X = (stredXoriznuty - (pismo.MeasureString(text).X / 2));
            polohaNapisu.Y = (splashRect.Center.Y - (pismo.MeasureString(text).Y / 2));
            napis = text;
            kreslitPismo = true;
            if (!okamzite)
            {
                splashStep = byte.MinValue;
                zvysStep = budeUpdate = true;
                ZvedejStep();
            }
            else
            {
                splashStep = byte.MaxValue;
                budeUpdate = true;
            }
            provedUpdate = zavedPriUpdejtu;
        }

        public void ZatemniSplash(bool zavedPriUpdejtu)
        {
            kresliSplash = zvysStep = true;
            vydrz = pozadovanaVydrz;
            splashRect.X = 0;
            ZvedejStep();
            provedUpdate = zavedPriUpdejtu;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(splashScreen, splashRect, splashColor);
            if (kreslitPismo && vydrz > 0) sb.DrawString(pismo, napis, polohaNapisu, splashColor);
        }
    }
}