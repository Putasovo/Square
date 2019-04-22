using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MojehraDroid
{
    internal class Monster
    {
        private ushort delka, rychlost;
        private Texture2D textura;
        public Rectangle obdelnik;
        private Rectangle source;
        Point source1, source2, source3;
        Point vychozi;
        private bool poSmeruHodin;
        private ushort maxX, maxY, i=5;

        public Monster(ushort length, ushort speed, Rectangle position, Texture2D texture, bool direction, ushort maxX, ushort maxY)
        {
            this.delka = length; rychlost = speed; //this.krok = krok;
            obdelnik = position; textura = texture;
            vychozi = (obdelnik.Location);
            source = new Rectangle(0,0, 32, 32);
            source1 = new Point(0,0); source2 = new Point(32, 0); source3 = new Point(64, 0);
            poSmeruHodin = direction;
            this.maxX = maxX; this.maxY = maxY;
        }

        public void Update()
        {
            if (poSmeruHodin)
            {
                if (obdelnik.Y == 0)
                    if (obdelnik.X != maxX) obdelnik.X += rychlost;
                    else obdelnik.Y += rychlost;
                if (obdelnik.X == maxX)
                    if (obdelnik.Y != maxY) obdelnik.Y += rychlost;
                    else obdelnik.X -= rychlost;
                else if (obdelnik.Y == maxY)
                    if (obdelnik.X != 0) obdelnik.X -= rychlost;
                    else obdelnik.Y -= rychlost;
                else if (obdelnik.X == 0)
                    if (obdelnik.Y != 0) obdelnik.Y -= rychlost;
                    else obdelnik.X += rychlost;
            }
            else
            {
                if (obdelnik.Y == 0)
                    if (obdelnik.X != 0) obdelnik.X -= rychlost;
                    else obdelnik.Y += rychlost;
                if (obdelnik.X == maxX)
                    if (obdelnik.Y != 0) obdelnik.Y -= rychlost;
                    else obdelnik.X -= rychlost;
                else if (obdelnik.Y == maxY)
                    if (obdelnik.X != maxX) obdelnik.X += rychlost;
                    else obdelnik.Y -= rychlost;
                else if (obdelnik.X == 0)
                    if (obdelnik.Y != maxY) obdelnik.Y += rychlost;
                    else obdelnik.X += rychlost;
            }

            Animuj();
        }

        private void Animuj()
        {
            if (i != 0)
            {
                if (i == 10) source.Location = source3;
                else if (i == 5) source.Location = source2;
            }
            else
            {
                source.Location = source1;
                i = 15;
            }
            i--;
        }

        public void Respawn()
        {
            obdelnik.Location = vychozi;
            poSmeruHodin = !poSmeruHodin;
        }

        public void Draw(SpriteBatch SB)
        {
            SB.Draw(textura, obdelnik, source, Color.White);
        }

    }

}