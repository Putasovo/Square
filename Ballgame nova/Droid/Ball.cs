using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace MojehraDroid
{
    internal class Ball : IDisposable
    {
        private readonly float vyslednaRotace = 0f;
        private static readonly float nahoru = 0f;
        private static readonly float doprava = MathHelper.Pi * 3 / 2;
        private static readonly float dolu = MathHelper.Pi;
        private static readonly float doleva = MathHelper.ToRadians(90);
        //private float doleva = MathHelper.Pi / 2;
        private static readonly Vector2 originRotace = new Vector2(16, 16);

        private float prolnuti;
        private Color color1, color2;
        private short dobijeni = -1;
        public bool srazena, cinna, utocna;
        public bool utocnaLeva, utocnaHorni, utocnaPrava, utocnaDolni;
        private Vector2 velocity, vychoziVelocity, minVelocity, maxVelocity;
        private Random rand = new Random();
        private byte[] nahodnyBajt = new byte[1];
        private System.Security.Cryptography.RNGCryptoServiceProvider safeRand = new System.Security.Cryptography.RNGCryptoServiceProvider();
        public Rectangle rect = new Rectangle(0, 0, 32, 32);
        private Rectangle rectBall = new Rectangle(0, 0, 32, 32);
        private Rectangle rectRed = new Rectangle(32, 0, 32, 32);
        private Rectangle rectNabijeci = new Rectangle(64, 0, 32, 32);
        private Rectangle rectZnicena = new Rectangle(96, 0, 32, 32);

        private Point novaPoloha;
        private Vector2 presnaPoloha;
        private readonly int pravyOkrajDesky, dolniOkrajDesky, levyOkrajDesky, horniOkrajDesky;
        private readonly byte rozmer, polomer;
        private float odchylka;
        private float faktorCasu;
        private float faktorRychlosti = 0.01f;
        private int flipped;
        private bool svislyObrat, vodorovnyObrat, povolVariace, predchoziObrat;
        private int indexDlazdice;
        private readonly ushort maxIndexDlazdice;
        SoundEffect narazDoCesty;

        /// <summary>
        ///  Creates ball
        /// </summary>
        /// <param name="balLoc"></param>
        /// <param name="balVec"></param>
        /// <param name="windowX">X</param>
        /// <param name="windowY">Y</param>
        /// <param name="dimension">dimenze</param>
        /// <param name="rigidita">rigid</param>
        /// <param name="attackLeft">left</param>
        /// <param name="attackUp">up</param>
        /// <param name="attackRight">right</param>
        /// <param name="attackDown">down</param>
        /// <param name="bludiste">bludiste</param>
        /// <param name="kolize">collide sound</param>
        public Ball(Vector2 balLoc, Vector2 balVec, int windowX, int windowY, byte dimension, bool rigidita,
            bool attackLeft = false, bool attackUp = false, bool attackRight = false, bool attackDown = false,
            bool bludiste = false, SoundEffect kolize = null)
        {
            maxIndexDlazdice = (ushort)((windowX / dimension) * (windowY / dimension));
            rect.X = MathHelper.Clamp((int)balLoc.X, dimension * 2, windowX - dimension);
            rect.Y = MathHelper.Clamp((int)balLoc.Y, dimension * 2, windowY - dimension);
            presnaPoloha = new Vector2(rect.X, rect.Y);
            Hlavni.hitboxyKouli.Add(rect); //ted jen pro kontrolu stretu
            velocity = balVec;
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
                dolniOkrajDesky = windowY -1;// bez -1 by byl prekrocen indexdlazdice
            }

            odchylka = ( (float)(rand.NextDouble() - 0.5f ) * .11f); //pro jedinecnost
            velocity.X += +odchylka; velocity.Y += +odchylka;
            vychoziVelocity = velocity;
            minVelocity = velocity * .81f; maxVelocity = velocity * 1.2f;
            //faktorCasu = 1;
            if (utocna)
            {
                dobijeni = 10;
                color1 = new Color(255, 255, 255, 255);// jinak bude střet černý
                color2 = new Color(0, 0, 0, 0);
            }
            else dobijeni = -100;

            povolVariace = true; // zatim jsem nikdy nepouzil false
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

        internal void Update(int time)
        {
            if (cinna)
            { 
                prolnuti = (float)dobijeni / 10;
                faktorCasu = time * faktorRychlosti;
                presnaPoloha += velocity * faktorCasu;
                novaPoloha.X = (int)presnaPoloha.X;
                novaPoloha.Y = (int)presnaPoloha.Y + polomer;
                indexDlazdice = (novaPoloha.X / rozmer + novaPoloha.Y / rozmer * Hlavni.columns);

                if (velocity.X < 0) LevyNaraz();
                else PravyNaraz();

                if (!vodorovnyObrat) rect.X = novaPoloha.X;
                else flipped += 1;

                novaPoloha.X = novaPoloha.X + polomer;
                //novaPoloha.Y = (int)(presnaPoloha.Y + velocity.Y * faktorCasu);
                novaPoloha.Y = (int)(presnaPoloha.Y);
                indexDlazdice = (novaPoloha.X / rozmer + novaPoloha.Y / rozmer * Hlavni.columns);

                if (velocity.Y < 0) HorniNaraz();
                else DolniNaraz();

                if (!svislyObrat) rect.Y = novaPoloha.Y;
                else flipped += 1;

                if (povolVariace) VariacePoOdrazech(2);
            }
        }

        internal void UpdateAnimace(int time, short maxX = 9999, short maxY = 9999, short minX = -9999, short minY = -9999)
        {
            prolnuti = (float)dobijeni / 10;

            faktorCasu = time * faktorRychlosti;
            presnaPoloha += velocity * faktorCasu;
            if (minX != -9999 || maxX != 9999) if (presnaPoloha.X < minX || presnaPoloha.X > maxX) velocity.X *= -1;
            if (minY != -9999 || maxY != 9999) if (presnaPoloha.Y < minY || presnaPoloha.Y + rect.Height > maxY) velocity.Y *= -1;
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
            else if (Hlavni.tiles[indexDlazdice].plna)
            {
                if (utocnaLeva && dobijeni == 0 && !Hlavni.tiles[indexDlazdice].plnaPredem && Hlavni.tiles[indexDlazdice].pruchodna)
                {
                    Hlavni.tiles[indexDlazdice].Zborit(false);
                    Hlavni.HrajOdraz();
                    dobijeni = 10;
                }
                OdrazVodorovne();
            }
            else vodorovnyObrat = false;
        }

        private void PravyNaraz()
        {
            if (novaPoloha.X + rozmer > pravyOkrajDesky)
            {
                OdrazVodorovne();
            }
            
            else if (
                //indexDlazdice != Hlavni.tiles.Count && //ochrana když neni okraj - jinak musim vyplnit posledni roh
                Hlavni.tiles[indexDlazdice + 1].plna)
            {
                if (utocnaPrava && dobijeni == 0 && !Hlavni.tiles[indexDlazdice + 1].plnaPredem && Hlavni.tiles[indexDlazdice + 1].pruchodna)
                {
                    Hlavni.tiles[indexDlazdice + 1].Zborit(false);
                    Hlavni.HrajOdraz();
                    dobijeni = 10;
                }
                OdrazVodorovne();
            }
            else vodorovnyObrat = false;
        }

        private void HorniNaraz()
        {
            if (novaPoloha.Y < horniOkrajDesky)
            {
                OdrazSvisle();
            }
            else if (Hlavni.tiles[indexDlazdice].plna)
            {
                if (utocnaHorni && dobijeni == 0 && !Hlavni.tiles[indexDlazdice].plnaPredem && Hlavni.tiles[indexDlazdice].pruchodna)
                {
                    Hlavni.tiles[indexDlazdice].Zborit(false);
                    Hlavni.HrajOdraz();
                    dobijeni = 10;
                }
                OdrazSvisle();
            }
            else svislyObrat = false;
        }

        private bool DolniNaraz()
        {
            if (novaPoloha.Y + rozmer > dolniOkrajDesky)
            {
                OdrazSvisle();
            }
            else if (Hlavni.tiles[indexDlazdice + Hlavni.columns].plna)
            {
                if (utocnaDolni && dobijeni == 0 && !Hlavni.tiles[indexDlazdice + Hlavni.columns].plnaPredem && Hlavni.tiles[indexDlazdice + Hlavni.columns].pruchodna)
                {
                    Hlavni.tiles[indexDlazdice + Hlavni.columns].Zborit(false);
                    Hlavni.HrajOdraz();
                    dobijeni = 10;
                }
                OdrazSvisle();
            }
            else svislyObrat = false;
            return svislyObrat;
        }


        private void OdrazSvisle()
        {
            //Hlavni.hrajOdraz = true;
            if (predchoziObrat && dobijeni > 0) dobijeni--;
            velocity.Y = velocity.Y * -1;
            svislyObrat = true; predchoziObrat = false;
        }

        private void OdrazVodorovne()
        {
            //Hlavni.hrajOdraz = true;
            if (!predchoziObrat && dobijeni > 0) dobijeni--;
            velocity.X = velocity.X * -1;
            vodorovnyObrat = predchoziObrat = true;
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
            if (anone) povolVariace = true;
            else povolVariace = false;
        }

        internal void NastavOdchylku(float nova)
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
                else { velocity.X -= odchylka; velocity.Y += odchylka; }
            }
        }

        public void NasobRychlost(float nasobic)
        {
            faktorRychlosti *= nasobic;
        }

        public void NastavRychlost(float rychlost)
        {
            faktorRychlosti = rychlost;
        }

        public void NastavPolohu(Vector2 poloha)
        {
            presnaPoloha = poloha;
        }

        internal void Obzivni()
        {
            cinna = true;
            if (utocna) { dobijeni = 10; }
        }
        internal void Zasazen()
        {
            cinna = false;
        }

        public void Draw(SpriteBatch spritebatch, Texture2D sprite)
        {
            if (cinna)
            { 
                if (dobijeni == -1)
                {
                    if (srazena) spritebatch.Draw(sprite, rect, rectBall, Color.Crimson);
                    else spritebatch.Draw(sprite, rect, rectBall, Color.White);
                }
                else if (dobijeni == 0) //nabita
                    spritebatch.Draw(sprite, rect.Center.ToVector2(), rectRed, Color.White, vyslednaRotace, originRotace, 1f, SpriteEffects.None, 0);
                else
                {
                    spritebatch.Draw(sprite, rect.Location.ToVector2(), rectBall, Color.White); //zaklad
                    //pridavek
                    spritebatch.Draw(sprite, rect.Location.ToVector2()+originRotace, rectNabijeci, Color.Lerp(color1, color2, prolnuti), vyslednaRotace, originRotace, 1f, SpriteEffects.None, 0);
                }
            }
            else spritebatch.Draw(sprite, rect.Location.ToVector2(), rectZnicena, Color.White);
        }
    }
}