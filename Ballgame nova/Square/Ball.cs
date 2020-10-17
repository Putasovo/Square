using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Square
{
    public class Ball : IDisposable
    {
        private static readonly Random rand = new Random();
        private const float nahoru = 0f;
        private const float doprava = MathHelper.Pi * 3 / 2;
        private const float dolu = MathHelper.Pi;
        private const float doleva = MathHelper.Pi / 2; // MathHelper.ToRadians(90)
        private static Vector2 originRotace;
        private readonly float vyslednaRotace = 0f;

        private float prolnuti;
        private Color color1, color2;
        private short dobijeni = -1;
        public bool srazena, cinna, utocna;
        public bool utocnaLeva, utocnaHorni, utocnaPrava, utocnaDolni;
        private Vector2 velocity, vychoziVelocity, minVelocity, maxVelocity;
        private readonly byte[] nahodnyBajt = new byte[1];
        private readonly System.Security.Cryptography.RNGCryptoServiceProvider safeRand = new System.Security.Cryptography.RNGCryptoServiceProvider();
        
        public float FaktorRychlosti { get; set; } = 0.01f;
        public Rectangle rect = new Rectangle(0, 0, 32, 32);
        private static readonly Rectangle rectBall = new Rectangle(0, 0, 32, 32);
        private static readonly Rectangle rectRed = new Rectangle(32, 0, 32, 32);
        private static readonly Rectangle rectNabijeci = new Rectangle(64, 0, 32, 32);
        private static readonly Rectangle rectZnicena = new Rectangle(96, 0, 32, 32);

        private Point novaPoloha;
        private Vector2 presnaPoloha;
        private readonly int pravyOkrajDesky, dolniOkrajDesky, levyOkrajDesky, horniOkrajDesky;
        private readonly byte rozmer, polomer;
        private float odchylka;
        private float faktorCasu;
        private int flipped;
        private bool svislyObrat, vodorovnyObrat, povolVariace, predchoziObrat;
        private int indexDlazdice;
        // private readonly ushort maxIndexDlazdice;
        private static SoundEffect s_respawn;
        private readonly SoundEffect narazDoCesty;
        private static SoundEffectInstance instanceOdrazu;

        /// <summary>
        ///  Creates ball
        /// </summary>
        /// <param name="ballLoc"></param>
        /// <param name="ballVec"></param>
        /// <param name="windowX">X</param>
        /// <param name="windowY">Y</param>
        /// <param name="dimension">dimenze</param>
        /// <param name="rigidita">rigid</param>
        /// <param name="attackLeft">left</param>
        /// <param name="attackUp">up</param>
        /// <param name="attackRight">right</param>
        /// <param name="attackDown">down</param>
        /// <param name="bludiste">bludiste</param>
        /// <param name="obzivni">respawn sound</param>
        /// <param name="kolize">collide sound</param>
        /// <param name="odraz">deflect sound</param>
        public Ball(Vector2 ballLoc, Vector2 ballVec, int windowX, int windowY, byte dimension, bool rigidita,
            bool attackLeft = false, bool attackUp = false, bool attackRight = false, bool attackDown = false,
            bool bludiste = false, SoundEffect obzivni = null, SoundEffect kolize = null, SoundEffect odraz = null)
        {
            // maxIndexDlazdice = (ushort)((windowX / dimension) * (windowY / dimension));
            originRotace = new Vector2(dimension / 2, dimension / 2);
            rect.X = MathHelper.Clamp((int)ballLoc.X, dimension * 2, windowX - dimension);
            rect.Y = MathHelper.Clamp((int)ballLoc.Y, dimension * 2, windowY - dimension);
            presnaPoloha = new Vector2(rect.X, rect.Y);
            // Hlavni.hitboxyKouli.Add(rect); // ted jen pro kontrolu stretu
            velocity = ballVec;
            rozmer = dimension; polomer = (byte)(dimension / 2);
            cinna = rigidita;
            if (attackDown)
            {
                utocna = utocnaDolni = attackDown;
                vyslednaRotace = nahoru;
            }
            else if (attackLeft)
            {
                utocna = utocnaLeva = attackLeft;
                vyslednaRotace = doleva;
            }
            else if (attackRight)
            {
                utocna = utocnaPrava = attackRight;
                vyslednaRotace = doprava;
            }
            else if (attackUp)
            {
                utocna = utocnaHorni = attackUp;
                vyslednaRotace = dolu;
            }

            narazDoCesty = kolize;

            if (!bludiste)
            {
                pravyOkrajDesky = windowX - dimension;
                dolniOkrajDesky = windowY - dimension;
                levyOkrajDesky = dimension;
                horniOkrajDesky = dimension;
            }
            else
            {
                pravyOkrajDesky = windowX;
                dolniOkrajDesky = windowY -1; // bez -1 by byl prekrocen indexdlazdice
            }

            odchylka = ( (float)(rand.NextDouble() - 0.5f ) * .11f); // pro jedinecnost
            velocity.X += +odchylka;
            velocity.Y += +odchylka;
            vychoziVelocity = velocity;
            minVelocity = velocity * .81f; maxVelocity = velocity * 1.2f;
            //faktorCasu = 1;
            if (utocna)
            {
                dobijeni = 10;
                color1 = new Color(255, 255, 255, 255); // jinak bude střet černý
                color2 = new Color(0, 0, 0, 0);
            }
            else 
                dobijeni = -100;

            povolVariace = true; // zatim jsem nikdy nepouzil false

            s_respawn = obzivni;
            instanceOdrazu = odraz.CreateInstance();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                safeRand.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Update(int time)
        {
            if (cinna)
            { 
                prolnuti = (float)dobijeni / 10;
                faktorCasu = time * FaktorRychlosti;
                presnaPoloha += velocity * faktorCasu;
                novaPoloha.X = (int)presnaPoloha.X;
                novaPoloha.Y = (int)presnaPoloha.Y + polomer;
                indexDlazdice = novaPoloha.X / rozmer + novaPoloha.Y / rozmer * PlayBoard.sloupcu;

                if (velocity.X < 0) 
                    LevyNaraz();
                else 
                    PravyNaraz();

                if (!vodorovnyObrat) 
                    rect.X = novaPoloha.X;
                else 
                    flipped += 1;

                // novaPoloha.Y = (int)(presnaPoloha.Y + velocity.Y * faktorCasu);
                novaPoloha.Y = (int)(presnaPoloha.Y);
                novaPoloha.X += polomer;
                indexDlazdice = (novaPoloha.X / rozmer + novaPoloha.Y / rozmer * PlayBoard.sloupcu);

                if (velocity.Y < 0)
                    HorniNaraz();
                else 
                    DolniNaraz();

                if (!svislyObrat) 
                    rect.Y = novaPoloha.Y;
                else 
                    flipped += 1;

                if (povolVariace) VariacePoOdrazech(2);
            }
        }

        public void UpdateAnimace(int time, short maxX = 9999, short maxY = 9999, short minX = -9999, short minY = -9999)
        {
            prolnuti = (float)dobijeni / 10;

            faktorCasu = time * FaktorRychlosti;
            presnaPoloha += velocity * faktorCasu;
            if (minX != -9999 || maxX != 9999) 
                if (presnaPoloha.X < minX || presnaPoloha.X > maxX) 
                    velocity.X *= -1;
            if (minY != -9999 || maxY != 9999) 
                if (presnaPoloha.Y < minY || presnaPoloha.Y + rect.Height > maxY) 
                    velocity.Y *= -1;
            novaPoloha.X = (int)presnaPoloha.X;
            novaPoloha.Y = (int)presnaPoloha.Y;
            rect.X = novaPoloha.X; rect.Y = novaPoloha.Y;
        }

        private void LevyNaraz()
        {
            if (novaPoloha.X < levyOkrajDesky)
            {
                OdrazVodorovne();
            }
            else if (PlayBoard.tiles[indexDlazdice].plna)
            {
                if (utocnaLeva && dobijeni == 0 && !PlayBoard.tiles[indexDlazdice].plnaPredem && PlayBoard.tiles[indexDlazdice].pruchodna)
                {
                    PlayBoard.tiles[indexDlazdice].Zborit(false);
                    // Hlavni.HrajOdraz();
                    dobijeni = 10;
                }
                OdrazVodorovne();
            }
            else 
                vodorovnyObrat = false;
        }

        private void PravyNaraz()
        {
            if (novaPoloha.X + rozmer > pravyOkrajDesky)
            {
                OdrazVodorovne();
            }            
            else if (
                //indexDlazdice != Hlavni.tiles.Count && //ochrana když neni okraj - jinak musim vyplnit posledni roh
                PlayBoard.tiles[indexDlazdice + 1].plna)
            {
                if (utocnaPrava && dobijeni == 0 && !PlayBoard.tiles[indexDlazdice + 1].plnaPredem && PlayBoard.tiles[indexDlazdice + 1].pruchodna)
                {
                    PlayBoard.tiles[indexDlazdice + 1].Zborit(false);
                    HrajOdraz();
                    dobijeni = 10;
                }
                OdrazVodorovne();
            }
            else 
                vodorovnyObrat = false;
        }

        private void HorniNaraz()
        {
            if (novaPoloha.Y < horniOkrajDesky)
            {
                OdrazSvisle();
            }
            else if (PlayBoard.tiles[indexDlazdice].plna)
            {
                if (utocnaHorni && dobijeni == 0 && !PlayBoard.tiles[indexDlazdice].plnaPredem && PlayBoard.tiles[indexDlazdice].pruchodna)
                {
                    PlayBoard.tiles[indexDlazdice].Zborit(false);
                    // Hlavni.HrajOdraz();
                    dobijeni = 10;
                }
                OdrazSvisle();
            }
            else 
                svislyObrat = false;
        }

        private bool DolniNaraz()
        {
            if (novaPoloha.Y + rozmer > dolniOkrajDesky)
            {
                OdrazSvisle();
            }
            else if (PlayBoard.tiles[indexDlazdice + PlayBoard.sloupcu].plna)
            {
                if (utocnaDolni && dobijeni == 0 && !PlayBoard.tiles[indexDlazdice + PlayBoard.sloupcu].plnaPredem && PlayBoard.tiles[indexDlazdice + PlayBoard.sloupcu].pruchodna)
                {
                    PlayBoard.tiles[indexDlazdice + PlayBoard.sloupcu].Zborit(false);
                    // Hlavni.HrajOdraz();
                    dobijeni = 10;
                }
                OdrazSvisle();
            }
            else 
                svislyObrat = false;

            return svislyObrat;
        }

        private void OdrazSvisle()
        {
            // Hlavni.hrajOdraz = true;
            velocity.Y *= -1;
            svislyObrat = true; predchoziObrat = false;
            if (predchoziObrat && dobijeni > 0)
                dobijeni--;
        }

        private void OdrazVodorovne()
        {
            // Hlavni.hrajOdraz = true;
            velocity.X *= -1;
            vodorovnyObrat = predchoziObrat = true;
            if (!predchoziObrat && dobijeni > 0) 
                dobijeni--;
        }

        public void SrazkasProjetou()
        {
            narazDoCesty.Play();
            srazena = true;
        }

        public void ZrusSrazku()
        {
            srazena = false;
        }

        public void PovolVariace(bool anone)
        {
            if (anone) 
                povolVariace = true;
            else 
                povolVariace = false;
        }

        public void NastavOdchylku(float nova)
        {
            odchylka = nova;
        }

        private void VariacePoOdrazech(short potrebnychOdrazu)
        {
            if (flipped >= potrebnychOdrazu)
            {
                flipped = 0;
                safeRand.GetBytes(nahodnyBajt);
                odchylka = (float)( (nahodnyBajt[0]-128) * .0015 );
                //float odchylkaY = (float)((nahodnyBajt[0] - 128) * .0012);
                //velocity += new Vector2(odchylka, odchylkaY);vede k extremum
                //smerove deleni nutne, jinak mi pretece index
                if (velocity.X > 0)
                {
                    velocity.X = MathHelper.Clamp(velocity.X + odchylka, minVelocity.X, maxVelocity.X);
                    velocity.Y = MathHelper.Clamp(velocity.Y - odchylka, minVelocity.Y, maxVelocity.Y);
                }
                else 
                { 
                    velocity.X -= odchylka;
                    velocity.Y += odchylka; 
                }
            }
        }

        public void NasobRychlost(float nasobic)
        {
            FaktorRychlosti *= nasobic;
        }

        public void NastavRychlost(float rychlost)
        {
            FaktorRychlosti = rychlost;
        }

        public void NastavPolohu(Vector2 poloha)
        {
            presnaPoloha = poloha;
        }

        public void Obzivni()
        {
            cinna = true;
            if (utocna) 
                dobijeni = 10;

            s_respawn.Play();
        }

        public void Zasazen()
        {
            cinna = false;
        }

        private static void HrajOdraz()
        {
            if (instanceOdrazu.State == SoundState.Stopped) //stejne prekracuju limit at omezuju jak chci
            {
                instanceOdrazu.Pitch = 0;
                instanceOdrazu.Play();
            }
            else if (instanceOdrazu.Pitch < .99f) instanceOdrazu.Pitch += .03f;
        }

        public void Draw(SpriteBatch spritebatch, Texture2D sprite)
        {
            if (cinna)
            { 
                if (dobijeni == -1)
                {
                    if (srazena) 
                        spritebatch.Draw(sprite, rect, rectBall, Color.Crimson);
                    else 
                        spritebatch.Draw(sprite, rect, rectBall, Color.White);
                }
                else if (dobijeni == 0) // nabita
                    spritebatch.Draw(sprite, rect.Center.ToVector2(), rectRed, Color.White, vyslednaRotace, originRotace, 1f, SpriteEffects.None, 0);
                else
                {
                    spritebatch.Draw(sprite, rect.Location.ToVector2(), rectBall, Color.White); // zaklad
                    // pridavek
                    spritebatch.Draw(sprite, rect.Location.ToVector2()+originRotace, rectNabijeci, Color.Lerp(color1, color2, prolnuti), vyslednaRotace, originRotace, 1f, SpriteEffects.None, 0);
                }
            }
            else 
                spritebatch.Draw(sprite, rect.Location.ToVector2(), rectZnicena, Color.White);
        }
    }
}