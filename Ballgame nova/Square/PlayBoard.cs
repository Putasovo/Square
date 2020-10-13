using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Square
{
    public static class PlayBoard
    {
        public static ushort borderThick;
        private static Rectangle borderV; //Rectangle borderH;
        public readonly static List<Rectangle> okrajeH = new List<Rectangle>(2);
        public readonly static List<Rectangle> okrajeV = new List<Rectangle>(2);
        public static Rectangle borderVanim;
        public static Texture2D texOkrajeH, texOkrajeV;
        private static Color[] barvaV;
        private static Color[] barvaH;
        public static List<Tile> tiles;
        public static List<Tile> tilesVnitrni;
        public static int sloupcu, radku;

        public static void Init(ushort tileSize, List<Tile> tiles, List<Tile> tilesVnitrni, ushort windowHeight, ushort windowWidth)
        {
            borderThick = tileSize;
            barvaV = new Color[windowHeight * borderThick];
            barvaH = new Color[borderThick * (windowWidth - borderThick * 2)];
            PlayBoard.tiles = tiles;
            PlayBoard.tilesVnitrni = tilesVnitrni;
            sloupcu = windowWidth / tileSize;
            radku = windowHeight / tileSize;
        }

        public static void VybarviOkraje(GraphicsDeviceManager graphics, Rectangle oknoHry, Color barvaVOkraje, Color barvaHOkraje)
        {
            barvaHOkraje.A = 22;

            texOkrajeH = new Texture2D(graphics.GraphicsDevice, oknoHry.Width - borderThick * 2, borderThick);
            texOkrajeV = new Texture2D(graphics.GraphicsDevice, borderThick, oknoHry.Height);
            // Set the texture data with our color information.
            for (uint i = 0; i < barvaH.Length; ++i) barvaH[i] = barvaVOkraje;
            texOkrajeH.SetData(barvaH);
            for (uint i = 0; i < barvaV.Length; ++i) barvaV[i] = barvaHOkraje;
            texOkrajeV.SetData(barvaV);
        }

        public static void OdstranOkraje()
        {
            okrajeH.Clear(); okrajeV.Clear();
            foreach (Tile tile in tiles)
            {
                tile.okrajova = false;
            }
        }

        public static void PostavOkraje(Rectangle oknoHry)
        {
            okrajeV.Clear(); okrajeH.Clear();
            borderV = new Rectangle(0, 0, borderThick, oknoHry.Height);
            okrajeV.Add(borderV);
            okrajeV.Add(new Rectangle(oknoHry.Width - borderThick, 0, borderThick, oknoHry.Height));
            okrajeH.Add(new Rectangle(borderThick, 0, oknoHry.Width - borderThick * 2, borderThick));
            okrajeH.Add(new Rectangle(borderThick, oknoHry.Height - borderThick, oknoHry.Width - borderThick * 2, borderThick));
        }
    }
}
