using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Square
{
    public class SplashScreen
    {
        private const ushort pozadovanaVydrz = 60;
        private static ushort vydrz;
        private static ushort okrajX;
        private static ushort stredXoriznuty;
        private static Rectangle splashRect;
        private static Texture2D splashScreen;
        private Color splashColor; 
        private readonly Color[] splash;
        private bool zvysStep, budeUpdate;
        private byte splashStep;
        private string napis;
        private static SpriteFont pismo;
        private bool kreslitPismo;
        private Vector2 polohaNapisu;
        
        /// <summary>
        /// by these main Update will decide whether to proceed
        /// </summary>
        public bool ProvedUpdate { get; internal set; }
        public bool KreslitSplash { get; internal set; }

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
            ProvedUpdate = true;
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
            if (KreslitSplash)
            {
                if (splashStep == byte.MaxValue)
                {
                    zvysStep = false;
                    if (vydrz > 0) 
                        vydrz--;
                    else if (budeUpdate)
                    {
                        if (!ProvedUpdate) 
                            ProvedUpdate = true;
                        else 
                            budeUpdate = ProvedUpdate = false;
                    }
                }
                else if (splashStep == byte.MinValue) // dokonceni
                {
                    KreslitSplash = kreslitPismo = budeUpdate = false;
                    ProvedUpdate = true;
                    splashRect.X = okrajX;
                    splashColor.A = byte.MaxValue;
                }

                if (zvysStep) 
                    ZvedejStep();
                else if (!budeUpdate && !ProvedUpdate && vydrz == 0)
                    StahniStep();
            }
        }

        public void KresliSplash(bool okamzite, string text, bool zavedPriUpdejtu)
        {
            KreslitSplash = true;
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
            ProvedUpdate = zavedPriUpdejtu;
        }

        public void ZatemniSplash(bool zavedPriUpdejtu)
        {
            KreslitSplash = zvysStep = true;
            vydrz = pozadovanaVydrz;
            splashRect.X = 0;
            ZvedejStep();
            ProvedUpdate = zavedPriUpdejtu;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(splashScreen, splashRect, splashColor);
            if (kreslitPismo && vydrz > 0) sb.DrawString(pismo, napis, polohaNapisu, splashColor);
        }
    }
}