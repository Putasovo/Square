﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Square
{
    public static class PlayBoard
    {
        private static Color[] barvaV;
        private static Color[] barvaH;
        private static Rectangle borderV; // Rectangle borderH;
        public static Rectangle BorderVanim;
        private static ushort borderSize;   
        public static readonly List<Rectangle> okrajeH = new List<Rectangle>(2);
        public static readonly List<Rectangle> okrajeV = new List<Rectangle>(2);        
        public static Texture2D texOkrajeH, texOkrajeV;        
        public static List<Tile> tiles;
        public static List<Tile> tilesVnitrni;
        public static int sloupcu;

        public static void Init(ushort tileSize, List<Tile> tiles, List<Tile> tilesVnitrni, ushort windowHeight, ushort windowWidth)
        {
            borderSize = tileSize;
            barvaV = new Color[windowHeight * borderSize];
            barvaH = new Color[borderSize * (windowWidth - borderSize * 2)];
            PlayBoard.tiles = tiles;
            PlayBoard.tilesVnitrni = tilesVnitrni;
            sloupcu = windowWidth / tileSize;
        }

        public static void VybarviOkraje(GraphicsDeviceManager graphics, Rectangle oknoHry, Color barvaVOkraje, Color barvaHOkraje)
        {
            barvaHOkraje.A = 22;

            texOkrajeH = new Texture2D(graphics.GraphicsDevice, oknoHry.Width - borderSize * 2, borderSize);
            texOkrajeV = new Texture2D(graphics.GraphicsDevice, borderSize, oknoHry.Height);
            // Set the texture data with our color information.
            for (uint i = 0; i < barvaH.Length; ++i) 
                barvaH[i] = barvaHOkraje;
 
            for (uint i = 0; i < barvaV.Length; ++i) 
                barvaV[i] = barvaVOkraje;

            texOkrajeH.SetData(barvaH);
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
            borderV = new Rectangle(0, 0, borderSize, oknoHry.Height);
            okrajeV.Add(borderV);
            okrajeV.Add(new Rectangle(oknoHry.Width - borderSize, 0, borderSize, oknoHry.Height));
            okrajeH.Add(new Rectangle(borderSize, 0, oknoHry.Width - borderSize * 2, borderSize));
            okrajeH.Add(new Rectangle(borderSize, oknoHry.Height - borderSize, oknoHry.Width - borderSize * 2, borderSize));
        }
    }
}
