using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Square
{
    public class Monster
    {
        private readonly ushort rychlost;
        private readonly Texture2D textura;
        public Rectangle obdelnik;
        private Rectangle source;
        private Point source1, source2, source3;
        private Point vychozi;
        private bool poSmeruHodin;
        private readonly ushort maxX, maxY;
        private ushort i=5;

        public Monster(short length, ushort speed, Rectangle position, Texture2D texture, bool direction, ushort maxX, ushort maxY)
        {
            rychlost = speed;
            obdelnik = position; 
            textura = texture;
            vychozi = obdelnik.Location;
            source = new Rectangle(0, 0, length, length);
            source1 = new Point(0, 0); source2 = new Point(length, 0); source3 = new Point(length * 2, 0);
            poSmeruHodin = direction;
            this.maxX = maxX;
            this.maxY = maxY;
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

        public void Respawn()
        {
            obdelnik.Location = vychozi;
            poSmeruHodin = !poSmeruHodin;
        }

        public void Draw(SpriteBatch SB)
        {
            SB.Draw(textura, obdelnik, source, Color.White);
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
    }
}