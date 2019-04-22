using Microsoft.Xna.Framework;

namespace MojehraDroid
{
    /// <summary>
    /// An enumeration for possible game states
    /// </summary>
    internal enum Stavy
    {
        Menu,
        Play,
        Vitez,
        Prohra,
        Animace,
        Pause
    }

    internal class Barvy
    {
        public static Color prvniBarva = new Color(000, 000, 100, 100);
        public static Color druhaBarva = new Color(100, 150, 100, 100);
        public static readonly Color barvaCerna = new Color(000, 000, 100, 100);
        public static readonly Color druhaFialova = new Color(100, 150, 100, 100);
        public static readonly Color vyblitaZelena = new Color(22, 100, 22, 127);
        public static Color prvniViteznaBarva = new Color(100, 200, 200, 0);
        public static Color druhaViteznaBarva = new Color(200, 200, 100, 0);
        public static Color E2prvni = new Color(000, 000, 100, 100);
        public static Color E2druha = new Color(59, 31, 32, 100);
        internal static Color modra = new Color(22, 22, 150, 127);
        internal static Color vyblitamodra = new Color(22, 22, 100, 127);
        internal static Color oblibena = new Color(50, 181, 181, 127);
        internal static Color vyblitaOblibena = new Color(66, 158, 188, 127);

        internal static void NastavBarvy(Color prvni, Color druha)
        {
            prvniBarva = prvni; druhaBarva = druha;
        }
    }
}