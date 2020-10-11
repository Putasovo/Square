using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.InteropServices;
using System.Text;

namespace Square
{
    public class Storage
    {
        private static readonly IsolatedStorageFile store;
        private static readonly string scoreFilename = "score.txt";
        private static readonly string levelFilename = "uroven";
        private static readonly string volumeFilename = "volumes";

        public static bool SaveGameExists { get; set; }
        public static float VolumeSound { get; set; } = .9f;
        public static float VolumeHudby { get; set; } = .9f;
        public static short SkoreTotal { get; set; }
        public static byte MaxEpisoda { get; set; }
        public static byte MaxLevel { get; set; }
        public static int RekordSkore { get; private set; }

        static Storage() 
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                store = IsolatedStorageFile.GetUserStoreForDomain();
            else
                store = IsolatedStorageFile.GetUserStoreForApplication();
        }

        public static int GetScore()
        {
            RekordSkore = 0;
            if (store.FileExists(scoreFilename))
            {
                var isoStream = new IsolatedStorageFileStream(scoreFilename, FileMode.Open, FileAccess.Read);
                using (var sr = new StreamReader(isoStream))
                {
                    string line = sr.ReadLine();
                    if (line != null)
                    {
                        RekordSkore = int.Parse(line);
                    }
                }
                isoStream.Close();
            }
            else
            {
                var isoStream = new IsolatedStorageFileStream(scoreFilename, FileMode.Create, FileAccess.Write);
                isoStream.Close();
            }

            return RekordSkore;
        }

        public static bool SaveScore()
        {
            bool result = false;
            if (SkoreTotal > RekordSkore)
            {
                RekordSkore = SkoreTotal;
                if (store.FileExists(scoreFilename))
                {
                    var isoStream = new IsolatedStorageFileStream(scoreFilename, FileMode.Open, FileAccess.Write);
                    using (var sw = new StreamWriter(isoStream))
                    {
                        sw.Flush();
                        sw.WriteLine(SkoreTotal.ToString());
                        result = true;
                        
                    }
                    isoStream.Close();
                }
            }

            return result;
        }

        public static void LoadGame()
        {
            if (store.FileExists(levelFilename))
            {
                var isoStream = new IsolatedStorageFileStream(levelFilename, FileMode.Open, FileAccess.Read);
                using (var sr = new StreamReader(isoStream))
                {
                    string line = sr.ReadLine();
                    if (line != null)
                    {
                        MaxLevel = byte.Parse(line);

                        line = sr.ReadLine();
                        if (line != null)
                        {
                            MaxEpisoda = byte.Parse(line);

                            line = sr.ReadLine();
                            if (line != null) 
                                SkoreTotal = short.Parse(line);
                        }
                    }
                }
                isoStream.Dispose();
            }
            else
            {
                MaxLevel = byte.MinValue; MaxEpisoda = byte.MinValue;
            }
        }

        /// <summary>
        /// Saves level, episode and skore
        /// </summary>
        public static void SaveGame(Level uroven)
        {
            if (uroven.epizoda > 0 && uroven.epizoda >= MaxEpisoda)
            {
                MaxEpisoda = uroven.epizoda;
                MaxLevel = uroven.cisloUrovne;
                IsolatedStorageFileStream isoStream;

                if (!store.FileExists(levelFilename))
                {
                    isoStream = new IsolatedStorageFileStream(levelFilename, FileMode.Create, FileAccess.Write);
                    isoStream.Dispose();
                }

                isoStream = new IsolatedStorageFileStream(levelFilename, FileMode.Open, FileAccess.Write);
                using (var sw = new StreamWriter(isoStream))
                {
                    sw.Flush();
                    sw.WriteLine(MaxLevel.ToString());
                    sw.WriteLine(uroven.epizoda.ToString());
                    sw.WriteLine(SkoreTotal.ToString());
                }

                isoStream.Dispose();
            }
        }

        public static bool LoadVolumes()
        {
            if (store.FileExists(volumeFilename))
            {
                var isoStream = new IsolatedStorageFileStream(volumeFilename, FileMode.Open, FileAccess.Read);
                using (var sr = new StreamReader(isoStream))
                {
                    string line = sr.ReadLine();
                    if (line != null)
                    {
                        VolumeSound = float.Parse(line);

                        line = sr.ReadLine();
                        if (line != null)
                        {
                            VolumeHudby = float.Parse(line);
                        }
                    }
                }
                isoStream.Dispose();

                return true;
            }
            else
            {
                var isoStream = new IsolatedStorageFileStream(volumeFilename, FileMode.Create, FileAccess.Write);
                isoStream.Dispose();

                return false;
            }
        }

        public static void SaveVolumes()
        {
            if (store.FileExists(volumeFilename))
            {
                var isoStream = new IsolatedStorageFileStream(volumeFilename, FileMode.Open, FileAccess.Write);
                using (var sw = new StreamWriter(isoStream))
                {
                    sw.Flush();
                    sw.WriteLine(VolumeSound.ToString());
                    sw.WriteLine(VolumeHudby.ToString());
                }

                isoStream.Dispose();
            }
        }
    }
}
