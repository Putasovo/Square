using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Square
{
    public static class PlayBoard
    {
        private const byte dobaZemetreseni = 60;
        
        private static Color[] barvaV;
        private static Color[] barvaH;
        private static Rectangle borderV; // Rectangle borderH;        
        private static int vybuchujuciMina;
        internal static short borderSize;
        
        public static Rectangle BorderVanim;
        public static readonly List<Rectangle> okrajeH = new List<Rectangle>(2);
        public static readonly List<Rectangle> okrajeV = new List<Rectangle>(2);         
        public static List<Tile> tiles;
        public static List<Tile> tilesVnitrni;        
        public static Texture2D TexOkrajeH { get; private set; }
        public static Texture2D TexOkrajeV { get; set; }
        
        public static int Sloupcu { get; internal set; }
        public static bool ProbihaVybuch { get; set; }
        public static byte Zemetreseni { get; private set; }

        public static void Init(short tileSize, int cols, int rows)
        {
            borderSize = tileSize;
            barvaV = new Color[rows * borderSize * borderSize];
            barvaH = new Color[borderSize * (cols * borderSize - borderSize * 2)];
            Sloupcu = cols;
            tiles = new List<Tile>(cols * rows);
            tilesVnitrni = new List<Tile>((cols - 2) * (rows - 2));
        }

        public static void VybarviOkraje(GraphicsDeviceManager graphics, Rectangle oknoHry, Color barvaVOkraje, Color barvaHOkraje)
        {
            barvaHOkraje.A = 22;
            TexOkrajeH = new Texture2D(graphics.GraphicsDevice, oknoHry.Width - borderSize * 2, borderSize);
            TexOkrajeV = new Texture2D(graphics.GraphicsDevice, borderSize, oknoHry.Height);
            // Set the texture data with our color information.
            for (uint i = 0; i < barvaH.Length; ++i) 
                barvaH[i] = barvaHOkraje;
 
            for (uint i = 0; i < barvaV.Length; ++i) 
                barvaV[i] = barvaVOkraje;

            TexOkrajeH.SetData(barvaH);
            TexOkrajeV.SetData(barvaV);
        }

        public static void OdstranOkraje()
        {
            okrajeH.Clear(); okrajeV.Clear();
            foreach (Tile tile in tiles)
            {
                tile.Okrajova = false;
            }
        }

        public static void PostavOkraje(Rectangle oknoHry)
        {
            okrajeV.Clear(); okrajeH.Clear();
            borderV = new Rectangle(0, 0, borderSize, oknoHry.Height);
            okrajeV.Add(borderV);
            okrajeV.Add(new Rectangle(oknoHry.Width - borderSize, 0, borderSize, oknoHry.Height));
            okrajeH.Add(new Rectangle(borderSize, 0, oknoHry.Width - borderSize * 2, borderSize));
            okrajeH.Add(new Rectangle(borderSize, oknoHry.Height - borderSize, oknoHry.Width - borderSize * 2, borderSize));
        }


        public static void PripravZemetreseni(int indexMiny)
        {
            if (Zemetreseni == 0)
            {
                Zemetreseni = dobaZemetreseni;
                tiles[indexMiny].Odminovat();
                vybuchujuciMina = indexMiny;
                ProbihaVybuch = true;
                VybuchKolem(vybuchujuciMina, 1);
            }
        }

        public static void VybuchPostupne()
        {
            if (Zemetreseni == 51 && tiles[vybuchujuciMina].DosahMiny > 1)
            {
                VybuchKolem(vybuchujuciMina, 2);
                Vibrace.Vibruj(600);
            }
            else if (Zemetreseni == 44 && tiles[vybuchujuciMina].DosahMiny > 2)
                VybuchKolem(vybuchujuciMina, 3);
            else if (Zemetreseni == 37 && tiles[vybuchujuciMina].DosahMiny > 3)
                VybuchKolem(vybuchujuciMina, 4);
            else if (Zemetreseni == 30 && tiles[vybuchujuciMina].DosahMiny > 4)
                VybuchKolem(vybuchujuciMina, 5);
        }

        /// <summary>
        /// zatim nekontroluju, jestli jde vybuch za okrajove dlazdice
        /// </summary>
        /// <param name="index"></param>
        /// <param name="vzdalenost"></param>
        internal static void VybuchKolem(int index, byte vzdalenost)
        {
            tiles[index - vzdalenost].Zborit(true);
            foreach (Ball ball in Balls.ballsAll) 
                if (tiles[index - vzdalenost].drawRectangle.Intersects(ball.rect)) 
                    ball.Zasazen();

            tiles[index + vzdalenost].Zborit(true);
            foreach (Ball ball in Balls.ballsAll) 
                if (tiles[index + vzdalenost].drawRectangle.Intersects(ball.rect)) 
                    ball.Zasazen();

            if (index - Sloupcu * vzdalenost > 0)
            {
                tiles[index - Sloupcu * vzdalenost].Zborit(true);
                foreach (Ball ball in Balls.ballsAll) 
                    if (tiles[index - Sloupcu * vzdalenost].drawRectangle.Intersects(ball.rect)) 
                        ball.Zasazen();
            }

            if (index + Sloupcu * vzdalenost < tiles.Count)
            {
                tiles[index + Sloupcu * vzdalenost].Zborit(true);
                foreach (Ball ball in Balls.ballsAll)
                    if (tiles[index + Sloupcu * vzdalenost].drawRectangle.Intersects(ball.rect)) 
                        ball.Zasazen();
            }
        }

        public static void KrokZemetreseni()
        {
            Zemetreseni--;
            if (Zemetreseni == 0)
            {
                tiles[vybuchujuciMina].NastavSource(new Rectangle(0, 0, 32, 32));
                ProbihaVybuch = false;
            }
        }

        public static void VyplnPoleProOznaceni()
        {
            for (int index = tiles.Count - Sloupcu; index > Sloupcu; index--)
            {
                if (tiles[index].Kvyplneni)
                {   //potrebuju se pustit do vsech stran
                    int indexLevy = index - 1;
                    int indexPravy = index + 1;
                    int indexHorni = index - Sloupcu;
                    int indexDolni = index + Sloupcu;
                    if (!tiles[indexLevy].Plna && !tiles[indexLevy].Projeta && !tiles[indexLevy].Okrajova)
                    {
                        VyplnDoleva(index - 1);
                    }
                    if (!tiles[indexPravy].Plna && !tiles[indexPravy].Projeta && !tiles[indexPravy].Okrajova)
                    {
                        VyplnDoprava(index + 1);
                    }
                    if (!tiles[indexHorni].Plna && !tiles[indexHorni].Projeta && !tiles[indexHorni].Okrajova)
                    {
                        VyplnNahoru(indexHorni);
                    }
                    if (!tiles[indexDolni].Plna && !tiles[indexDolni].Projeta && !tiles[indexDolni].Okrajova)
                    {
                        VyplnDolu(indexDolni);
                    }
                    //if (debug)
                    //{
                    //    if (tiles[index].plna) throw new System.Exception("podruhe vyplnujes" + i);
                    //}
                    break;
                }
            }
        }

        private static void VyplnDoleva(int index)
        {
            int indexHorni; int indexDolni;
            while (!tiles[index].Kvyplneni && !tiles[index].Okrajova && !tiles[index].Plna && !tiles[index].Projeta)
            {
                tiles[index].KVyplneni(true);
                indexHorni = index - Sloupcu;
                indexDolni = index + Sloupcu;
                if (!tiles[indexHorni].Kvyplneni && !tiles[indexHorni].Okrajova && !tiles[indexHorni].Plna && !tiles[indexHorni].Projeta)
                {
                    VyplnNahoru(indexHorni);
                }
                if (!tiles[indexDolni].Kvyplneni && !tiles[indexDolni].Okrajova && !tiles[indexDolni].Plna && !tiles[indexDolni].Projeta)
                {
                    VyplnDolu(indexDolni);
                }
                index -= 1;
            }
        }

        private static void VyplnDoprava(int index)
        {
            int indexHorni; int indexDolni;
            while (!tiles[index].Kvyplneni && !tiles[index].Okrajova && !tiles[index].Plna && !tiles[index].Projeta)
            {
                tiles[index].KVyplneni(true);
                indexHorni = index - Sloupcu;
                indexDolni = index + Sloupcu;
                if (!tiles[indexHorni].Kvyplneni && !tiles[indexHorni].Okrajova && !tiles[indexHorni].Plna && !tiles[indexHorni].Projeta)
                {
                    VyplnNahoru(indexHorni);
                }
                if (!tiles[indexDolni].Kvyplneni && !tiles[indexDolni].Okrajova && !tiles[indexDolni].Plna && !tiles[indexDolni].Projeta)
                {
                    VyplnDolu(indexDolni);
                }
                index += 1;
            }
        }

        private static void VyplnNahoru(int index)
        {
            int indexLevy; int indexPravy;
            while (!tiles[index].Kvyplneni && !tiles[index].Okrajova && !tiles[index].Plna && !tiles[index].Projeta)
            {
                tiles[index].KVyplneni(true);
                indexLevy = index - 1;
                indexPravy = index + 1;
                if (!tiles[indexLevy].Kvyplneni && !tiles[indexLevy].Okrajova && !tiles[indexLevy].Plna && !tiles[indexLevy].Projeta)
                {
                    VyplnDoleva(indexLevy);
                }
                if (!tiles[indexPravy].Kvyplneni && !tiles[indexPravy].Okrajova && !tiles[indexPravy].Plna && !tiles[indexPravy].Projeta)
                {
                    VyplnDoprava(indexPravy);
                }
                index -= Sloupcu;
            }
        }

        private static void VyplnDolu(int index)
        {
            int indexLevy; int indexPravy;
            while (!tiles[index].Kvyplneni && !tiles[index].Okrajova && !tiles[index].Plna && !tiles[index].Projeta)
            {
                tiles[index].KVyplneni(true);
                indexLevy = index - 1;
                indexPravy = index + 1;
                if (!tiles[indexLevy].Kvyplneni && !tiles[indexLevy].Okrajova && !tiles[indexLevy].Plna && !tiles[indexLevy].Projeta)
                {
                    VyplnDoleva(indexLevy);
                }
                if (!tiles[indexPravy].Kvyplneni && !tiles[indexPravy].Okrajova && !tiles[indexPravy].Plna && !tiles[indexPravy].Projeta)
                {
                    VyplnDoprava(indexPravy);
                }
                index += Sloupcu;
            }
        }

        public static void OdznacProjete()
        {
            foreach (Tile tile in tiles)
            {
                if (tile.Projeta)
                    tile.Projeta = false;
            }
        }

        public static bool NajdiOznacPrazdnou()
        {
            bool nalezena = false;
            foreach (Tile tile in tilesVnitrni)
            {
                if (!tile.Plna && !tile.Kvyplneni)
                {
                    tile.Kvyplneni = true;
                    tile.Prvni = true;
                    nalezena = true;
                }
            }

            return nalezena;
        }

        public static void VyprazdnitZneviditelnit()
        {
            foreach (Tile dlazdice in tilesVnitrni)
            {
                if (dlazdice.Kvyplneni)
                {
                    dlazdice.Plna = false;
                    dlazdice.visible = false;
                }
            }
        }
    }
}
