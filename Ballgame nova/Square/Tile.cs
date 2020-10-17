using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace Square
{
    /// <summary>
    /// Dlazdice
    /// </summary>
    public class Tile
    {
        // drawing support
        private readonly Texture2D sprite;
        private readonly Texture2D spriteOznaceny;
        private readonly Texture2D spriteOznaceny2;
        private readonly Texture2D spriteDruha;
        private static Rectangle minaPozice = new Rectangle(0,0,32,32);
        private static Rectangle minaSkrytaPozice = new Rectangle(14, 14, 3, 3);
        private static Rectangle cestaPozice = new Rectangle(32, 0, 32, 32);
        private static Rectangle cilovaPozice = new Rectangle(64, 0, 32, 32);
        private static Rectangle plnaPredemPozice = new Rectangle(128, 0, 32, 32);
        private static Rectangle plnaPoziceAlt = new Rectangle(160, 0, 32, 32);
        private static Rectangle zpomalPozice = new Rectangle(192, 0, 32, 32);
        private static Rectangle ozivovaciPozice = new Rectangle(224, 0, 32, 32);
        private static Rectangle zvyrazniMalaPozice1 = new Rectangle(192, 32, 16, 16);
        private static Rectangle zvyrazniMalaPozice2 = new Rectangle(208, 32, 16, 16);
        private static Rectangle zvyrazniMalaPozice3 = new Rectangle(192, 48, 16, 16);
        private static Rectangle zvyrazniMalaPozice4 = new Rectangle(208, 48, 16, 16);
        private static Rectangle zvyrazniMalaPozice0 = new Rectangle(224, 32, 32, 32);
        private static Rectangle plnaPozice = new Rectangle(160, 32, 32, 32);
        private static Rectangle plna1Pozice = new Rectangle(128, 32, 32, 32);
        private static Rectangle plna2Pozice = new Rectangle(96, 32, 32, 32);
        private static Rectangle plna3Pozice = new Rectangle(64, 32, 32, 32);
        private static Rectangle plna4Pozice = new Rectangle(32, 32, 32, 32);
        private static Rectangle plna5Pozice = new Rectangle(0, 32, 32, 32);
        private static Rectangle nepruchodnaPozice = new Rectangle(96, 0, 32, 32);
        private static Rectangle exploze0Pozice = new Rectangle(0, 64, 32, 32);
        private static Rectangle exploze1Pozice = new Rectangle(32, 64, 32, 32);
        private static Rectangle exploze2Pozice = new Rectangle(64, 64, 32, 32);
        private static Rectangle exploze3Pozice = new Rectangle(96, 64, 32, 32);
        private static Rectangle exploze4Pozice = new Rectangle(128, 64, 32, 32);
        private static Rectangle exploze5Pozice = new Rectangle(160, 64, 32, 32);
        private static Rectangle exploze6Pozice = new Rectangle(192, 64, 32, 32);
        private static Rectangle exploze7Pozice = new Rectangle(224, 64, 32, 32);
        private Rectangle vyslednaTextura, vyslednaTexturaExploze;
        private readonly bool animated;
        private bool visible, zvyrazni, exploduje;
        private byte alfaZvyrazneni;
        private Color barvaZvyrazneni = Color.White;
        
        private short krokPlneni; 
        private bool zaplnujese, licha;
        public Rectangle drawRectangle;
        private Vector2 pozice, origin;
        private Vector2 velocity;
        private float rotace = 0.1f;

        // params
        private bool debuguju;
        public bool pruchodna = true;
        public bool okrajova, projeta, kvyplneni, plna, plnaPredem, cilova, zpomalovaci, mina, ozivovaci;
        public byte dosahMiny, casExploze;
        public bool prvni, druha;
        private Color barvaDlazdice;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sprite">sprites for the tile</param>
        /// <param name="location">location of first pixel</param>
        /// <param name="velocity">velocity</param>
        public Tile(Texture2D atlas, Texture2D exploze, Texture2D spriteOznaceny, Texture2D spriteOznaceny2, Texture2D spriteDruha,
            Vector2 location, Vector2 velocity,
            int width, int height, bool animated, bool viditelna, bool naokraji, bool debug)
        {
            this.sprite = atlas;
            this.spriteOznaceny = spriteOznaceny; this.spriteOznaceny2 = spriteOznaceny2; this.spriteDruha = spriteDruha;
            drawRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);
            this.velocity = velocity;
            this.animated = animated;
            this.visible = viditelna;
            this.debuguju = debug;
            this.okrajova = naokraji;
            barvaDlazdice = Color.White;
            this.origin = location + new Vector2(width/2); //stred
            //kolem stredu if (animated) this.origin = new Vector2(Hlavni.columns * plnaPozice.Width / 2, Hlavni.rows * plnaPozice.Width / 2);
            if (animated)
            {
                vyslednaTextura = plnaPozice;//bez ni neni videt
            }
        }

        /// <summary>
        /// Sets the rock's velocity
        /// </summary>
        public Vector2 Velocity
        {
            set
            {
                velocity.X = value.X;
                velocity.Y = value.Y;
            }
        }

        /// <summary>
        /// Updates the tile
        /// </summary>
        /// <param name="gameTime">game time</param>
        public void Update(GameTime gameTime)
        {
            //if (debuguju && prvni) //prvni nalezena
            //{ vyslednaTextura = spriteOznaceny; }
            //else if (debuguju && druha) //druha nalezena
            //{ vyslednaTextura = spriteOznaceny2; }
            //else if (debuguju && vyslednaTextura == spriteDruha)//jinak by byla plna
            //{ }
            if (exploduje)       //nemel bych zadrzet hrace, dokud neskonci vybuch?
            {
                casExploze--;
                if (casExploze == 0)
                {
                    exploduje = false;
                    if (!mina)
                    {
                        plna = false;
                        if (!ozivovaci) visible = false;
                    }
                    if (!pruchodna) ZpruchodnitHracovi(visible);
                }
                else if (casExploze < 8) vyslednaTexturaExploze = exploze7Pozice;
                else if (casExploze < 16) vyslednaTexturaExploze = exploze6Pozice;
                else if (casExploze < 24) vyslednaTexturaExploze = exploze5Pozice;
                else if (casExploze < 32) vyslednaTexturaExploze = exploze4Pozice;
                else if (casExploze < 40) vyslednaTexturaExploze = exploze3Pozice;
                else if (casExploze < 47) vyslednaTexturaExploze = exploze2Pozice;
                else if (casExploze < 54) vyslednaTexturaExploze = exploze1Pozice;
            }
            else if (zaplnujese)
            {
                krokPlneni -= 1;
                if (krokPlneni == 16) { vyslednaTextura = plna4Pozice; }
                else if (krokPlneni == 12) { vyslednaTextura = plna3Pozice; }
                else if (krokPlneni == 8) { vyslednaTextura = plna2Pozice; }
                else if (krokPlneni == 4) { vyslednaTextura = plna1Pozice; }
                else if (krokPlneni == 0)
                {
                    if (licha) vyslednaTextura = plnaPozice;
                    else vyslednaTextura = plnaPoziceAlt;
                    zaplnujese = false;
                }
            }

            if (animated)
            {
                rotace += 0.01f;
                drawRectangle.Offset(velocity);
            }
        }

        public void DebugDlazdice(short param)
        {
            if (param == 0)
            {
                prvni = false; druha = false;
                if (!plna) visible = false;
            }
            else if (param == 1)    { prvni = true; visible = true; }
            else if (param == 2)    { druha = true; visible = true; }
        }

        public void NastavOzivovaci(bool anone)
        {
            if (anone)
            {
                ozivovaci = true;
                NastavSource(ozivovaciPozice);
                visible = true;
            }
            else
            {
                ozivovaci = false;
                visible = false;
            }
        }

        public void NastavZpomalovac(bool anone)
        {
            if (anone)
            {
                zpomalovaci = visible = true;
                vyslednaTextura = zpomalPozice;
            }
            else
            {
                zpomalovaci = visible = false;
                vyslednaTextura = plnaPozice;
            }
        }

        public void SudaNeboLicha(int index)
        {
            if (index % 2 != 0)
                licha = true;
        }

        public void KVyplneni(bool anone)
        {
#if (debuguju)
            if (okrajova == true) throw new System.ArgumentException("resis okrajovou?");
#endif
            if (anone)
            {
                if (debuguju && kvyplneni == true) throw new System.ArgumentException("uz je oznacena");
                if (debuguju && plna == true) throw new System.ArgumentException("uz je plna");
                else kvyplneni = true;
            }
            else kvyplneni = false;
        }

        public void OznacitJakoProjetou(bool anone)
        {
            if (anone)
            {
                if (!visible)
                { 
                    projeta = true;
                    vyslednaTextura = cestaPozice;
                }
            }
            else
            {
                projeta = false;
            }
        }

        public void OznacJakoDruhePole(bool anone)
        {
            //if (anone) vyslednaTextura = spriteDruha;
        }

        public void OznacJakoCilovou(bool anone)
        {
            if (anone)
            {
                cilova = visible = true;
                vyslednaTextura = cilovaPozice;
            }
            else cilova = visible = false;
        }

        public void Zneviditelnit()
        {
            visible = false;
        }

        public void VyplnitZvyditelnit()
        {
            visible = plna = zaplnujese = true; kvyplneni = false;
            krokPlneni = 16;
            vyslednaTextura = plna5Pozice;
        }

        public void VyplnitZvyditelnitOkamzite()
        {
            visible = true; plna = true; kvyplneni = false;
            vyslednaTextura = plnaPozice;
        }

        public void VyplnitPredemZvyditelnit()
        {
            plnaPredem = plna = visible = true;
            kvyplneni = false;
            vyslednaTextura = plnaPredemPozice;
        }

        public void Znepruchodnit()
        {
            visible = plna = true; pruchodna = false;
            vyslednaTextura = nepruchodnaPozice;
        }

        private void ZpruchodnitHracovi(bool ozivovaci)
        {
            pruchodna = true;
            //vyslednaTextura = plnaPozice;
            barvaDlazdice.A = byte.MaxValue;
            if (ozivovaci) vyslednaTextura = ozivovaciPozice;
        }
        public void ZnepruchodnitHraci()
        {
            visible = true; pruchodna = false;
            vyslednaTextura = nepruchodnaPozice;
            barvaDlazdice.A = 22;
        }

        public void Zaminovat(byte range = 3)
        {
            dosahMiny = range;
            visible = mina = true;
            vyslednaTextura = minaSkrytaPozice;
        }
        public void Odminovat()
        {
            mina = false;
            vyslednaTextura = minaPozice;
        }
        public void Zborit(bool bouchla)
        {
            if (!plnaPredem) plna = projeta = false;
            if (!bouchla)
            {
                if (!mina)
                {
                    plna = visible = false;
                }
                else vyslednaTextura = minaSkrytaPozice;
            }
            else
            {
                vyslednaTexturaExploze = exploze0Pozice;
                exploduje = true;
                casExploze = 60;
            }
        }

        public void Zvyrazni()
        {
            zvyrazni = true;
            alfaZvyrazneni = 255;
        }

        public void Odvyrazni()
        {
            zvyrazni = false;
        }

        public void NastavBarvu(Color barva)
        {
            barvaDlazdice = barva;
        }

        public void NastavSource(Rectangle novy)
        {
            vyslednaTextura = novy;
        }

        /// <summary>
        /// kresli tile
        /// </summary>
        /// <param name="spriteBatch">sprite batch</param>
        /// puvodni animace: spriteBatch.Draw(sprite, null, drawRectangle, null, origin, rotace, null, Color.White, SpriteEffects.None, 0 );
        public void Draw(SpriteBatch spriteBatch)
        {
            if ((visible || projeta) && !okrajova)
            {
                if (animated)
                {
                    pozice = (drawRectangle.Location.ToVector2());
                    //spriteBatch.Draw(sprite, null, drawRectangle, vyslednaTextura, origin, rotace, null, Color.Black);
                    spriteBatch.Draw(sprite, pozice, vyslednaTextura, Color.Black, rotace, origin, 1f, SpriteEffects.None, 0);
                }
                else
                {
                    spriteBatch.Draw(sprite, drawRectangle, vyslednaTextura, barvaDlazdice);//pozor na alfu, s Black bylo vše černé
                }
            }

            if (zvyrazni)
            {
                //animuj stlaceni dlazdice
                if (alfaZvyrazneni <= 0) Odvyrazni();
                else
                {
                    barvaZvyrazneni.A = alfaZvyrazneni;
                    if (alfaZvyrazneni > 222) spriteBatch.Draw(sprite, drawRectangle, zvyrazniMalaPozice0, barvaZvyrazneni);
                    else if (alfaZvyrazneni > 180) spriteBatch.Draw(sprite, drawRectangle, zvyrazniMalaPozice1, barvaZvyrazneni);
                    else if (alfaZvyrazneni > 130) spriteBatch.Draw(sprite, drawRectangle, zvyrazniMalaPozice2, barvaZvyrazneni);
                    else if (alfaZvyrazneni > 70) spriteBatch.Draw(sprite, drawRectangle, zvyrazniMalaPozice3, barvaZvyrazneni);
                    else spriteBatch.Draw(sprite, drawRectangle, zvyrazniMalaPozice4, barvaZvyrazneni);
                }
                
                //spriteBatch.Draw(sprite, drawRectangle, zvyrazniMalaPozice, new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, alfaZvyrazneni));
                alfaZvyrazneni -= 3; 
            }
        }

        public void DrawZemetreseni(SpriteBatch spriteBatch, int posun)
        {
            pozice = drawRectangle.Location.ToVector2() + new Vector2(posun, posun);
            if (visible)
                spriteBatch.Draw(sprite, pozice, vyslednaTextura, Color.Snow, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
            
            if (exploduje)
                spriteBatch.Draw(sprite, drawRectangle, vyslednaTexturaExploze, Color.White);
        }

        public void DrawPlusOkrajove(SpriteBatch spriteBatch)
        {
            if (visible)
            {
                if (animated)
                {
                    pozice = drawRectangle.Location.ToVector2();
                    //spriteBatch.Draw(sprite, null, drawRectangle, vyslednaTextura, origin, rotace, null, Color.Black);
                    spriteBatch.Draw(sprite, pozice, vyslednaTextura, Color.Black, rotace, origin, 1f, SpriteEffects.None, 0);
                }
                else
                {
                    spriteBatch.Draw(sprite, drawRectangle, vyslednaTextura, barvaDlazdice);//pozor na alfu, s Black bylo vše černé
                }
            }

            if (zvyrazni)
            {
                //animuj stlaceni dlazdice
                if (alfaZvyrazneni <= 0) Odvyrazni();
                else
                {
                    barvaZvyrazneni.A = alfaZvyrazneni;
                    if (alfaZvyrazneni > 222) spriteBatch.Draw(sprite, drawRectangle, zvyrazniMalaPozice0, barvaZvyrazneni);
                    else if (alfaZvyrazneni > 180) spriteBatch.Draw(sprite, drawRectangle, zvyrazniMalaPozice1, barvaZvyrazneni);
                    else if (alfaZvyrazneni > 130) spriteBatch.Draw(sprite, drawRectangle, zvyrazniMalaPozice2, barvaZvyrazneni);
                    else if (alfaZvyrazneni > 70) spriteBatch.Draw(sprite, drawRectangle, zvyrazniMalaPozice3, barvaZvyrazneni);
                    else spriteBatch.Draw(sprite, drawRectangle, zvyrazniMalaPozice4, barvaZvyrazneni);
                }

                //spriteBatch.Draw(sprite, drawRectangle, zvyrazniMalaPozice, new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, alfaZvyrazneni));
                alfaZvyrazneni -= 3;
            }
        }

        public void DrawJednoduse(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, drawRectangle, Color.White);
        }

        public void DrawSlozite(SpriteBatch sb, float otaceni, float scale)
        {
             sb.Draw(sprite, drawRectangle.Location.ToVector2(), minaPozice, barvaDlazdice, otaceni, origin, scale, SpriteEffects.None, 1);
        }
    }
}