using System;
using System.Collections.Generic;
using System.Linq;

namespace MojehraDroid
{
    /// <summary>
    /// vypocty
    /// </summary>
    internal class vypocty
    {
        public static string tileStret(int X, int Y, int rozmer)
        {
            string stret;
            int indexDlazdice = (X / rozmer + Y / rozmer * Hlavni.columns); //na jake dlazdici je
            int indexDleVrchniHrany = ( (X + rozmer/2) / rozmer + Y / rozmer * Hlavni.columns); //na jake dlazdici je
            if (Hlavni.tiles[indexDlazdice].plna)
            {
                return stret = "vlevo";
            }
            else if (indexDleVrchniHrany > Hlavni.columns && Hlavni.tiles[indexDleVrchniHrany - Hlavni.columns].plna)
            {
                stret = "nahore";
                return stret;
            }
            else if (indexDlazdice + Hlavni.columns < Hlavni.tiles.Count)
            { 
                if (Hlavni.tiles[indexDlazdice + 1].plna)
                {
                    return stret = "vpravo";
                }
            }
            else if (indexDlazdice + Hlavni.columns < Hlavni.tiles.Count)
            {
                if (Hlavni.tiles[indexDlazdice + Hlavni.columns - 1].plna)
                {
                    return stret = "dole";
                }
            }
            return stret = "nic";
        }
    }
}
