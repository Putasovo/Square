using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Square
{
    public class Pozadi
    {
        private const float scale = 1F;
        private readonly Texture2D sprite;
        private Vector2 pohyb, presnaPoloha;
        private readonly List<Tile> tilesPozadi = new List<Tile>();
        private short cilovyX, cilovyY, stareX, stareY, rozdil;
        private bool doleva, nahoru, rotujici;
        private float otaceni, hustotaVodorovne;
        private readonly Color barvaSnehu = new Color(255, 255, 255, 11);
        private readonly ushort rows, columns, sirkaStrany, vyskaStrany;

        public Pozadi(Texture2D textura, ushort width, ushort height, Vector2 motion, float rotace = 0f,
            bool nastridacku = false, bool stridatNepravidelne = false, float hustotaVodorovne = 1)
        {
            sprite = textura;
            vyskaStrany = (ushort)sprite.Height;
            sirkaStrany = (ushort)sprite.Width;
            columns = (ushort)(width / sirkaStrany);
            rows = (ushort)(height / vyskaStrany);
            Vydlazdickuj(nastridacku, stridatNepravidelne);
            pohyb = motion;            
            PripravPohyb();
            this.hustotaVodorovne = hustotaVodorovne;
            if (rotace != 0)
            {
                otaceni = rotace; rotujici = true; // ale nezvladam nastavovat stred otaceni
            }
            if (rotace == 0f)
                rotujici = false;
            else
                rotujici = true;            
        }

        private void PripravPohyb()
        {
            if (pohyb.X < 0)
            {
                cilovyX = (short)(-sprite.Width);
                doleva = true;
            }
            else
            {
                cilovyX = (short)(sprite.Width);
                doleva = false;
            }
            if (pohyb.Y < 0)
            {
                cilovyY = (short)(-sprite.Height);
                nahoru = true;
            }
            else
            {
                cilovyY = (short)vyskaStrany;
                nahoru = false;
            }
        }

        public void Update()
        {
            if (rotujici) 
                otaceni += .02f; //v radianech
            presnaPoloha += pohyb;
            rozdil = (short)(presnaPoloha.X - stareX);
            if (rozdil != 0)
            {
                if ((doleva && cilovyX <= (short)presnaPoloha.X) || (!doleva && cilovyX >= (short)presnaPoloha.X))
                {
                    foreach (Tile tile in tilesPozadi)
                    {
                        tile.drawRectangle.X += rozdil;
                    }
                    stareX = (short)presnaPoloha.X;
                }
                else
                {
                    foreach (Tile tile in tilesPozadi)
                    {
                        if (doleva) 
                            tile.drawRectangle.X += sirkaStrany;
                        else
                            tile.drawRectangle.X -= sirkaStrany;
                    }
                    presnaPoloha.X = stareX = 0;
                }
            }

            rozdil = (short)(presnaPoloha.Y - stareY);
            if (rozdil != 0)
            {
                if ((nahoru && cilovyY <= (short)presnaPoloha.Y) || (!nahoru && cilovyY >= (short)presnaPoloha.Y))
                {
                    foreach (Tile tile in tilesPozadi)
                    {
                        tile.drawRectangle.Y += rozdil;
                    }
                    stareY = (short)presnaPoloha.Y;
                }
                else
                {
                    foreach (Tile tile in tilesPozadi)
                    {
                        if (nahoru) tile.drawRectangle.Y += vyskaStrany;
                        else tile.drawRectangle.Y -= vyskaStrany;
                    }
                    presnaPoloha.Y = stareY = 0;
                }
            }
        }

        public void DrawBezRotace(SpriteBatch sb)
        {
            foreach (Tile tile in tilesPozadi)
            {
                tile.DrawJednoduse(sb);
            }
        }

        internal void DrawsRotaci(SpriteBatch sb) // blbne
        {
            foreach (Tile tile in tilesPozadi)
            {
                tile.DrawSlozite(sb, otaceni, 1f);
            }
        }

        private void Vydlazdickuj(bool nastridacku, bool nepravidelne)
        {
            for (short i = -1; i <= rows; i++)
            {
                for (short j = -1; j <= columns; j++)
                {
                    var location = new Vector2(j * sirkaStrany, i * vyskaStrany);
                    if (nastridacku)
                    {
                        if (i % 2 == 0) 
                            location.X += sirkaStrany / 2;
                        if (nepravidelne) 
                            if (j % 2 != 0)
                                location.Y += vyskaStrany / 2;
                    }
                    bool naokraji = false;
                    ///if ((location.X == 0||location.X == windowWidth) || (location.Y == 0||location.Y == windowHeight))
                    //{
                    // naokraji = true;
                    // okrajovychDlazdic++;
                    //}
                    var tile = new Tile(sprite, null, null, null, null,
                        location, pohyb, sirkaStrany, vyskaStrany, false, true, naokraji, false);
                    tile.NastavBarvu(barvaSnehu);
                    tilesPozadi.Add(tile);
                    //if (!naokraji)
                    //{
                    //    tilesVnitrni.Add(tile);
                    //    tile.SudaNeboLicha(i* j);
                    //}
                }
            }
        }

        internal void NastavSmerPohybu(Vector2 novySmer)
        {
            pohyb = novySmer;
            PripravPohyb();
        }
    }
}