using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace Square
{
    public class Level
    {
        private static ushort radku;
        private static byte radkuUvnitr, sloupcuUvnitr;
        private static short numBalls, numAttackBalls;
        private int koordinat;
        private static int dlazdic;
        internal readonly List<Point> poziceKouli = new List<Point>(4);
        internal readonly List<Point> poziceUtocnychKouli = new List<Point>(4);
        internal static sbyte numUtocnychBallsLeft, numUtocnychBallsRight, numUtocnychBallsUp, numUtocnychBallsDown; //stejne poradi spawnu
        
        public byte Epizoda { get; internal set; } = 1;
        public byte CisloUrovne { get; internal set; }
        public static string EpizodaSplash { get; internal set; }
        public static bool ZpomalovatUtocne{ get; internal set; }
        public string LevelText { get; internal set; }
        public byte ViteznychProcent { get; internal set; }
        public bool ZrodMonstrum { get; internal set; }
        public bool PoSmeru { get; internal set; }
        public static bool Bludiste { get; internal set; }
        public bool PerformanceTest { get; internal set; }
        public bool BezOdchylky { get; internal set; }
        
        public Level(ushort rows)
        {
            radku = rows;
            radkuUvnitr = (byte)(rows - 2);
            sloupcuUvnitr = (byte)(PlayBoard.Sloupcu - 2);
            dlazdic = radku * PlayBoard.Sloupcu;
        }

        public void NastavUroven()
        {
            if (Epizoda == 1)
            {
                switch (CisloUrovne)
                {
                    case 0: Level10(); break;
                    case 1: Level11(); break;
                    case 2: Level12(); break;
                    case 3: Level13(); break;
                    case 4: Level14(); break;
                    case 5: Level15(); break;
                    case 6: Bludiste10(); break;
                    case 7: Level20(); Epizoda++; CisloUrovne = 0; break;
                }
            }
            else if (Epizoda == 2)
            {
                switch (CisloUrovne)
                {
                    case 0: Level20(); break;
                    case 1: Level21(); break;
                    case 2: Level22(); break;
                    case 3: Level23(); break;
                    case 4: Level24(); break;
                    case 5: Level25(); break;
                    case 6: Bludiste20(); break;
                    case 7: Level30(); Epizoda++; CisloUrovne = 0; break;
                }
            }
            else if (Epizoda == 3)
            {
                switch (CisloUrovne)
                {
                    case 0: Bludiste30(); break;
                    case 1: Level31(); break;
                    case 2: Level32(); break;
                    case 3: Level33(); break;
                    case 4: Level34(); break;
                    case 5: Level35(); break;
                    //case 6: Bludiste30(); break;
                    case 6: Victory(); break;
                    //case 7: PerformanceTest256();
                    case 7: Epizoda++; CisloUrovne = 0; break;
                }
            }
            else if (Epizoda == 4)
            {
                switch (CisloUrovne)
                {
                    case 0: PerformanceTest256(); break;
                    case 1: PerformanceTest512(); break;
                    case 2: Level10(); Epizoda = 1; CisloUrovne = 0; LevelText = Texts.ReturnToStart; break; // otocena hra
                }
            }
        }

        private void Victory()
        {
            Bludiste = true;
            for (int i = 0; i < PlayBoard.tiles.Count; i++)
            {
                if (i < 15) 
                    PlayBoard.tiles[i].Znepruchodnit();
                else if (i > 134) 
                    PlayBoard.tiles[i].Znepruchodnit();
                else if (i % 14 == 0) 
                    PlayBoard.tiles[i].Znepruchodnit();
                else if (i > PlayBoard.Sloupcu * 2 && i < PlayBoard.Sloupcu * radkuUvnitr) 
                    PlayBoard.tiles[i].Zaminovat(1);
            }

            PlayBoard.tiles[PlayBoard.tiles.Count - 16].OznacJakoCilovou(true);
            numAttackBalls = 499;
            numUtocnychBallsLeft = numUtocnychBallsRight = numUtocnychBallsDown = numUtocnychBallsUp = 1;
            koordinat = (ushort)(PlayBoard.Sloupcu / 2 + PlayBoard.Sloupcu - 2);
            PlayBoard.tiles[koordinat].VyplnitPredemZvyditelnit();
            while (!PlayBoard.tiles[koordinat + PlayBoard.Sloupcu].Okrajova)
            {
                koordinat += PlayBoard.Sloupcu;
                if (koordinat % 2 == 0) 
                    PlayBoard.tiles[koordinat].VyplnitPredemZvyditelnit();
                else 
                    PlayBoard.tiles[koordinat].VyplnitZvyditelnit();
            }
            poziceUtocnychKouli.Add(new Point(8, 5));
            poziceUtocnychKouli.Add(new Point(2, 5));
            poziceKouli.Add(new Point(6, 3));
            poziceKouli.Add(new Point(6, 6));
            LevelText = "Victoria";
        }

        /// <summary>
        /// leva strana jde vyplnit hned, ale lepsi je postupovat zprava
        /// </summary>
        private void Level30()
        {
            ZpomalovatUtocne = true;
            for (int j = 0; j < sloupcuUvnitr; j++)
            {
                for (int k = 0; k < radkuUvnitr; k++)
                {
                    if (j == sloupcuUvnitr / 2 - 2)
                        PlayBoard.tilesVnitrni[k * sloupcuUvnitr + j].VyplnitZvyditelnitOkamzite();
                    else if (j == sloupcuUvnitr / 2 + 2)
                        PlayBoard.tilesVnitrni[k * sloupcuUvnitr + j].VyplnitZvyditelnitOkamzite();
                    else if (k == radkuUvnitr / 2 && j == sloupcuUvnitr - 2)
                        PlayBoard.tilesVnitrni[k * sloupcuUvnitr + j].NastavZpomalovac(true);
                }
            }
            numAttackBalls = 15;
            byte i = byte.MinValue;
            while (i < numAttackBalls)
            {
                poziceUtocnychKouli.Add(new Point(sloupcuUvnitr / 2, i - radkuUvnitr));
                i++;
            }
            numUtocnychBallsLeft = 20;
            ZrodMonstrum = true;
            ViteznychProcent = 85;
            LevelText = "Priorities";
        }

        private void Level31()
        {
            int j = sloupcuUvnitr - 1;
            for (byte k = 0; k < radku; k++)
            {
                if (k < radkuUvnitr)
                {
                    PlayBoard.tilesVnitrni[k * sloupcuUvnitr + j].VyplnitZvyditelnit();
                    PlayBoard.tilesVnitrni[k * sloupcuUvnitr + j - 5].VyplnitZvyditelnitOkamzite();
                }

                if (k > radku - 3)
                {
                    PlayBoard.tiles[k * PlayBoard.Sloupcu + sloupcuUvnitr - 2].ZnepruchodnitHraci();
                    PlayBoard.tiles[k * PlayBoard.Sloupcu + sloupcuUvnitr - 2].ZnepruchodnitHraci();
                }
                else if (k == radkuUvnitr - 1)
                {
                    PlayBoard.tiles[k * PlayBoard.Sloupcu - 1].ZnepruchodnitHraci();
                    PlayBoard.tiles[k * PlayBoard.Sloupcu - 2].ZnepruchodnitHraci();
                }
                j--;
            }

            PlayBoard.tilesVnitrni[sloupcuUvnitr * radkuUvnitr - 1].NastavZpomalovac(true);

            PlayBoard.tilesVnitrni[sloupcuUvnitr * (radkuUvnitr / 3) + 2].Zaminovat(3);
            PlayBoard.tilesVnitrni[sloupcuUvnitr * radkuUvnitr - 4].Zaminovat(2);
            PlayBoard.tilesVnitrni[sloupcuUvnitr * (radkuUvnitr - 3) - 1].Zaminovat(2);

            numUtocnychBallsLeft = 2; numUtocnychBallsRight = 2;
            numUtocnychBallsUp = 2; numUtocnychBallsDown = 2;
            numAttackBalls = 10;
            numBalls = 4;

            poziceUtocnychKouli.Add(new Point(5, 4)); poziceUtocnychKouli.Add(new Point(6, 5)); //leve
            poziceUtocnychKouli.Add(new Point(2, 2)); poziceUtocnychKouli.Add(new Point(2, 3)); //prave
            poziceUtocnychKouli.Add(new Point(6, 4)); poziceUtocnychKouli.Add(new Point(7, 5)); //nahoru
            poziceUtocnychKouli.Add(new Point(3, 2)); poziceUtocnychKouli.Add(new Point(3, 3)); //dolu
            //poziceUtocnychKouli.Add(new Point(4, 2)); poziceUtocnychKouli.Add(new Point(4, 3));

            poziceKouli.Add(new Point(12, 7)); poziceKouli.Add(new Point(13, 6));
            //poziceKouli.Add(new Point(12, 8)); poziceKouli.Add(new Point(13, 7));

            //zrodMonstrum = true;
            ViteznychProcent = 72;
            LevelText = "Diagonal";
        }

        private void Level32()
        {
            for (int j = 0; j < sloupcuUvnitr; j++)
            {
                for (int k = 0; k < radkuUvnitr; k++)
                {
                    if (j == sloupcuUvnitr / 3)
                        PlayBoard.tilesVnitrni[k * sloupcuUvnitr + j].VyplnitZvyditelnitOkamzite();
                    else if (k == radkuUvnitr / 2 || k == radkuUvnitr / 2 - 1)
                    {
                        if (j < sloupcuUvnitr / 3)
                            PlayBoard.tilesVnitrni[k * sloupcuUvnitr + j].VyplnitZvyditelnitOkamzite();
                        else if (j == sloupcuUvnitr / 3 + 1) 
                            PlayBoard.tilesVnitrni[k * sloupcuUvnitr + j].Zaminovat();
                    }
                }
            }

            numBalls = 5;
            for (int i = 0; i <= numBalls; i++)
            {
                poziceKouli.Add(new Point(sloupcuUvnitr / 2, radkuUvnitr / 5 + i));
            }

            numUtocnychBallsRight = 2;
            numUtocnychBallsDown = 2;
            numAttackBalls = 5;
            poziceUtocnychKouli.Add(new Point(sloupcuUvnitr / 4, radkuUvnitr / 2 - 2));
            poziceUtocnychKouli.Add(new Point(sloupcuUvnitr / 4, radkuUvnitr / 2 - 3));

            poziceUtocnychKouli.Add(new Point(sloupcuUvnitr / 4, radkuUvnitr / 2 + 2));
            poziceUtocnychKouli.Add(new Point(sloupcuUvnitr / 4, radkuUvnitr / 2 + 3));

            ZrodMonstrum = true;
            //viteznychProcent = 79;
            LevelText = "Containment";
        }

        private void Level33()
        {
            int k = radkuUvnitr;
            for (int j = 1; j < sloupcuUvnitr; j += 2)
            {
                k--;
                {
                    if (k >= 2)
                    {
                        PlayBoard.tilesVnitrni[k * sloupcuUvnitr + j].VyplnitZvyditelnitOkamzite();
                        PlayBoard.tilesVnitrni[(k - 1) * sloupcuUvnitr + j].VyplnitZvyditelnitOkamzite();
                        PlayBoard.tilesVnitrni[(k - 2) * sloupcuUvnitr + j].VyplnitZvyditelnitOkamzite();
                    }
                }
            }

            PlayBoard.tilesVnitrni[1 * sloupcuUvnitr + 1].VyplnitZvyditelnit();
            PlayBoard.tilesVnitrni[1 * sloupcuUvnitr + 2].VyplnitZvyditelnitOkamzite();
            PlayBoard.tilesVnitrni[1 * sloupcuUvnitr + 3].VyplnitZvyditelnitOkamzite();
            PlayBoard.tilesVnitrni[1 * sloupcuUvnitr + 4].VyplnitZvyditelnit();

            PlayBoard.tilesVnitrni[(radkuUvnitr - 1) * sloupcuUvnitr - 5].VyplnitZvyditelnit();
            PlayBoard.tilesVnitrni[(radkuUvnitr - 1) * sloupcuUvnitr - 2].VyplnitZvyditelnit();
            PlayBoard.tilesVnitrni[(radkuUvnitr - 1) * sloupcuUvnitr - 3].VyplnitZvyditelnitOkamzite();
            PlayBoard.tilesVnitrni[(radkuUvnitr - 1) * sloupcuUvnitr - 4].VyplnitZvyditelnitOkamzite();

            numBalls = 18;
            byte i = byte.MinValue;
            while (i < numAttackBalls)
            {
                poziceUtocnychKouli.Add(new Point(sloupcuUvnitr / 2, i - sloupcuUvnitr));
                i++;
            }

            ZrodMonstrum = true;
            LevelText = "Traps";
        }

        private void Level34()
        {
            PlayBoard.tilesVnitrni[sloupcuUvnitr * 2 + 2].NastavZpomalovac(true);
            PlayBoard.tilesVnitrni[sloupcuUvnitr * 3 - 3].NastavZpomalovac(true);
            PlayBoard.tilesVnitrni[sloupcuUvnitr * (radkuUvnitr - 3) + 2].NastavZpomalovac(true);
            PlayBoard.tilesVnitrni[sloupcuUvnitr * (radkuUvnitr - 2) - 3].NastavZpomalovac(true);

            PlayBoard.tilesVnitrni[radkuUvnitr / 2 * sloupcuUvnitr + sloupcuUvnitr / 2].Zaminovat(3);
            PlayBoard.tilesVnitrni[(radkuUvnitr / 2 - 1) * sloupcuUvnitr + sloupcuUvnitr / 2].Zaminovat(3);

            numBalls = 22;
            numUtocnychBallsLeft = 4;
            numAttackBalls = 3;
            byte i = byte.MinValue;
            while (i < numBalls)
            {
                poziceUtocnychKouli.Add(new Point(sloupcuUvnitr / 2, i - sloupcuUvnitr));
                i++;
            }

            ZrodMonstrum = true;
            LevelText = "slowdown";
        }

        private void Level35()
        {
            PlayBoard.tiles[3].ZnepruchodnitHraci();
            PlayBoard.tiles[PlayBoard.Sloupcu - 4].ZnepruchodnitHraci();
            for (int j = 0; j < sloupcuUvnitr; j++)
            {
                for (int k = 0; k < radkuUvnitr; k++)
                {
                    if (k == 0)
                    {
                        PlayBoard.tilesVnitrni[2].Znepruchodnit();
                        PlayBoard.tilesVnitrni[sloupcuUvnitr - 3].Znepruchodnit();
                        PlayBoard.tilesVnitrni[sloupcuUvnitr / 2 - 1].NastavZpomalovac(true);
                        PlayBoard.tilesVnitrni[sloupcuUvnitr / 2 + 1].NastavZpomalovac(true);
                    }
                    else if (k == 1)
                    {
                        PlayBoard.tilesVnitrni[sloupcuUvnitr + 3].Znepruchodnit();
                        PlayBoard.tilesVnitrni[sloupcuUvnitr * 2 - 4].Znepruchodnit();
                        PlayBoard.tilesVnitrni[sloupcuUvnitr + sloupcuUvnitr / 2].NastavZpomalovac(true);
                    }
                    else if (k == 2)
                    {
                        PlayBoard.tilesVnitrni[sloupcuUvnitr * 2 + 4].Znepruchodnit();
                        PlayBoard.tilesVnitrni[sloupcuUvnitr * 3 - 5].Znepruchodnit();
                    }
                    else if (k == 3)
                    {
                        PlayBoard.tilesVnitrni[sloupcuUvnitr * 3 + 5].Znepruchodnit();
                        PlayBoard.tilesVnitrni[sloupcuUvnitr * 4 - 6].Znepruchodnit();
                        PlayBoard.tilesVnitrni[sloupcuUvnitr * 3 + 6].NastavOzivovaci(true);

                        PlayBoard.tilesVnitrni[sloupcuUvnitr * 3 + 2].Zaminovat(2);
                        PlayBoard.tilesVnitrni[sloupcuUvnitr * 4 - 3].Zaminovat(2);
                    }
                    else if (k == 4)
                    {
                        PlayBoard.tilesVnitrni[sloupcuUvnitr * 4 + 6].NastavOzivovaci(true);
                        PlayBoard.tilesVnitrni[sloupcuUvnitr * 4 + 6].Znepruchodnit();
                        PlayBoard.tilesVnitrni[sloupcuUvnitr * 4 + 8].Zaminovat(2);

                        PlayBoard.tilesVnitrni[sloupcuUvnitr * 4].VyplnitPredemZvyditelnit();
                        PlayBoard.tilesVnitrni[sloupcuUvnitr * 4 + 1].VyplnitZvyditelnit();
                        PlayBoard.tilesVnitrni[sloupcuUvnitr * 5 - 1].VyplnitPredemZvyditelnit();
                        PlayBoard.tilesVnitrni[sloupcuUvnitr * 5 - 2].VyplnitZvyditelnit();
                    }
                    else if (k == 5)
                    {
                        PlayBoard.tilesVnitrni[sloupcuUvnitr * 5 + 5].ZnepruchodnitHraci();
                        PlayBoard.tilesVnitrni[sloupcuUvnitr * 6 - 6].ZnepruchodnitHraci();
                        PlayBoard.tilesVnitrni[sloupcuUvnitr * 5 + 6].NastavOzivovaci(true);
                    }
                    else if (k > radkuUvnitr - 3)
                    {
                        if (j == 3 || j == sloupcuUvnitr - 4)
                        {
                            PlayBoard.tilesVnitrni[k * sloupcuUvnitr + j].VyplnitPredemZvyditelnit();
                        }
                    }
                }
            }
            numBalls = 8;
            byte i = byte.MinValue;
            while (i < numBalls)
            {
                poziceKouli.Add(new Point(sloupcuUvnitr / 2 + 1, (ushort)(i / 1.3)));
                poziceKouli.Add(new Point(sloupcuUvnitr / 2, sloupcuUvnitr - 2));
                i += 2;
            }

            numUtocnychBallsLeft = 1;
            numUtocnychBallsRight = 1;
            numUtocnychBallsDown = 1;
            numUtocnychBallsUp = 1;
            numAttackBalls = 4;
            i = byte.MinValue;
            while (i < numAttackBalls)
            {
                poziceUtocnychKouli.Add(new Point((int)(sloupcuUvnitr / 1.2), sloupcuUvnitr - i));
                i++;
            }

            ZrodMonstrum = true;
            LevelText = "Priorities";
        }

        private void Bludiste30()
        {
            Bludiste = true; numBalls = 3;
            for (int i = 1; i <= dlazdic; i++)
            {
                if (i == 4 || i == 8
                    || i == 16 || i == 17 || i == 19 || i == 23 || i == 25 || i == 26 || i == 27
                    || i == 32 || i == 34 || i == 38 || i == 42 || i == 44
                    || i == 47 || i == 48 || i == 49 || i == 50 || i == 51 || i == 52 || i == 53 || i == 54 || i == 55 || i == 56 || i == 57 || i == 59

                    || i == 87 || i == 88 || i == 89 || i == 90
                    || i == 91 || i == 92 || i == 93 || i == 94 || i == 95 || i == 96 || i == 97 || i == 98 || i == 99 || i == 100 || i == 101
                    || i == 107 || i == 108 || i == 110 || i == 111 || i == 133 || i == 116 || i == 117 || i == 118 || i == 119
                    || i == 122 || i == 123 || i == 124 || i == 126 || i == 127 || i == 129 || i == 130 || i == 132 || i == 134
                    || i == 141 || i == 146 || i == 150
                    ) PlayBoard.tiles[i - 1].Znepruchodnit();

                PlayBoard.tiles[62].Zaminovat(2); PlayBoard.tiles[66].Zaminovat(2);
                PlayBoard.tiles[46].ZnepruchodnitHraci(); PlayBoard.tiles[47].ZnepruchodnitHraci(); PlayBoard.tiles[48].ZnepruchodnitHraci();
                //PlayBoard.tiles[49].Znepruchodnit(); PlayBoard.tiles[50].Znepruchodnit();
                //PlayBoard.tiles[51].Znepruchodnit(); PlayBoard.tiles[52].Znepruchodnit();
                //PlayBoard.tiles[53].Znepruchodnit(); PlayBoard.tiles[54].Znepruchodnit();
                PlayBoard.tiles[55].ZnepruchodnitHraci(); PlayBoard.tiles[55].VyplnitPredemZvyditelnit();
                //PlayBoard.tiles[56].Znepruchodnit(); PlayBoard.tiles[57].Znepruchodnit(); PlayBoard.tiles[59].Znepruchodnit();

                //PlayBoard.tiles[52].ZnepruchodnitHraci(); PlayBoard.tiles[67].ZnepruchodnitHraci();
                PlayBoard.tiles[92].ZnepruchodnitHraci(); PlayBoard.tiles[96].ZnepruchodnitHraci();
                PlayBoard.tiles[100].ZnepruchodnitHraci(); PlayBoard.tiles[100].VyplnitPredemZvyditelnit();
                PlayBoard.tiles[135].OznacJakoCilovou(true);
            }
            poziceKouli.Add(new Point(2, 8)); 
            poziceKouli.Add(new Point(14, 8)); 
            poziceKouli.Add(new Point(7, 8));
            numAttackBalls = 2; numUtocnychBallsDown = 2;
            poziceUtocnychKouli.Add(new Point(2, 1)); 
            poziceUtocnychKouli.Add(new Point(5, 1));

            LevelText = "Not Straight Outta";
        }

        private void Bludiste20()
        {
            Bludiste = true; numBalls = 3;
            for (int i = 1; i <= dlazdic; i++)
            {
                if (i == 4 || i == 7 || i == 9 || i == 10 || i == 14
                    || i == 21 || i == 23 || i == 27 || i == 29
                    || i == 32 || i == 34 || i == 35 || i == 36 || i == 38 || i == 39 || i == 40 || i == 42
                    || i == 47 || i == 55 || i == 57 || i == 58 || i == 59 || i == 60
                    || i == 63 || i == 64 || i == 65
                    || i == 77 || i == 82 || i == 83 || i == 84 || i == 85 || i == 87 || i == 88 || i == 89
                    || i == 92 || i == 94 || i == 95 || i == 97 || i == 98 || i == 100 || i == 102
                    || i == 107 || i == 111 || i == 112 || i == 117 || i == 119
                    || i == 122 || i == 123 || i == 124 || i == 126 || i == 127 || i == 129 || i == 130 || i == 132 || i == 134
                    || i == 141 || i == 146 || i == 150)
                    PlayBoard.tiles[i - 1].Znepruchodnit();
                    PlayBoard.tiles[51].Zaminovat(1);
                    PlayBoard.tiles[52].ZnepruchodnitHraci(); 
                    PlayBoard.tiles[66].Zaminovat(1); 
                    PlayBoard.tiles[67].ZnepruchodnitHraci();
                    PlayBoard.tiles[14].OznacJakoCilovou(true);
            }
            poziceKouli.Add(new Point(14, 8)); poziceKouli.Add(new Point(14, 2)); poziceKouli.Add(new Point(7, 4));
            LevelText = "Breakaway"; //v bludisti by svitil porad?
        }

        /// <summary>
        /// jako 24 + monstrum
        /// </summary>
        private void Level25()
        {
            for (int j = 0; j < sloupcuUvnitr; j++)
            {
                for (int k = 0; k < radkuUvnitr; k++)
                {
                    if (j < radku / 2) 
                        PlayBoard.tilesVnitrni[k * sloupcuUvnitr + j].VyplnitZvyditelnitOkamzite();
                    else if (j == sloupcuUvnitr - 1) 
                        PlayBoard.tilesVnitrni[k * sloupcuUvnitr + j].VyplnitZvyditelnitOkamzite();
                }
            }

            koordinat = PlayBoard.Sloupcu / 2 + 1;
            byte i = byte.MinValue;
            while (i < radkuUvnitr)
            {
                PlayBoard.tilesVnitrni[koordinat].ZnepruchodnitHraci();
                if (i != 1 && i != 2 && i != 3 && i != 4 && i != 5 && i != 6)
                {
                    PlayBoard.tilesVnitrni[koordinat + 1].ZnepruchodnitHraci();
                    PlayBoard.tilesVnitrni[koordinat + 2].ZnepruchodnitHraci();
                }
                else if (i == 3 || i == 4) 
                    PlayBoard.tilesVnitrni[koordinat - 1].Zaminovat(3);

                PlayBoard.tilesVnitrni[koordinat + 3].ZnepruchodnitHraci();
                i++;
                koordinat += sloupcuUvnitr;
            }
            poziceKouli.Add(new Point(14, radku / 2)); poziceKouli.Add(new Point(14, radku / 2 - 1));
            poziceKouli.Add(new Point(14, radku / 2 - 2)); poziceKouli.Add(new Point(12, radku / 2 - 3));
            poziceUtocnychKouli.Add(new Point(14, radku / 2)); poziceUtocnychKouli.Add(new Point(14, radku / 2 - 1));
            poziceUtocnychKouli.Add(new Point(sloupcuUvnitr / 5, radkuUvnitr / 2));
            numBalls = 4; numAttackBalls = 3;
            numUtocnychBallsLeft = 2; numUtocnychBallsRight = 1;
            ZrodMonstrum = true;
            ViteznychProcent = 76;
            LevelText = "Out of ideas";
        }

        /// <summary>
        /// vhodnější je nechat koule chvíli žrát
        /// </summary>
        private void Level24()
        {
            for (int j = 0; j < sloupcuUvnitr; j++)
            {
                for (int k = 0; k < radkuUvnitr; k++)
                {
                    if (j < radku / 2) 
                        PlayBoard.tilesVnitrni[k * sloupcuUvnitr + j].VyplnitZvyditelnitOkamzite();
                    else if (j == sloupcuUvnitr - 1) 
                        PlayBoard.tilesVnitrni[k * sloupcuUvnitr + j].VyplnitZvyditelnitOkamzite();
                }
            }

            koordinat = PlayBoard.Sloupcu / 2 + 1;
            byte i = byte.MinValue;
            while (i < radkuUvnitr)
            {
                PlayBoard.tilesVnitrni[koordinat].ZnepruchodnitHraci();
                if (i != 1 && i != 2 && i != 3 && i != 4 && i != 5 && i != 6)
                {
                    PlayBoard.tilesVnitrni[koordinat + 1].ZnepruchodnitHraci();
                    PlayBoard.tilesVnitrni[koordinat + 2].ZnepruchodnitHraci();
                }
                else if (i == 3 || i == 4) 
                    PlayBoard.tilesVnitrni[koordinat - 1].Zaminovat(3);

                PlayBoard.tilesVnitrni[koordinat + 3].ZnepruchodnitHraci();
                i++;
                koordinat += sloupcuUvnitr;
            }
            poziceKouli.Add(new Point(12, radku / 2)); poziceKouli.Add(new Point(12, radku / 2 - 1));
            poziceKouli.Add(new Point(14, radku / 2 - 2)); poziceKouli.Add(new Point(14, radku / 2 - 3));
            poziceUtocnychKouli.Add(new Point(14, radku / 2)); poziceUtocnychKouli.Add(new Point(14, radku / 2 - 1));
            poziceUtocnychKouli.Add(new Point(12, radku / 2 + 1)); poziceUtocnychKouli.Add(new Point(12, radku / 2 + -3));
            numBalls = 4; numAttackBalls = 4;
            numUtocnychBallsLeft = 2; numUtocnychBallsRight = 2;
            ViteznychProcent = 74;
            LevelText = "Contra";
        }
        /// <summary>
        /// Více koulí na stejném místě.
        /// </summary>
        private void Level23()
        {
            numBalls = 8;
            numAttackBalls = 1;
            for (short i = numBalls; i > 0; i--)
            {
                poziceKouli.Add(new Point(8, 5));
            }
            poziceUtocnychKouli.Add(new Point(sloupcuUvnitr / 2, radkuUvnitr / 2));
            int indexVedlejsi = radku / 2 * PlayBoard.Sloupcu + PlayBoard.Sloupcu / 2;
            if (FlipCoin()) 
                indexVedlejsi -= PlayBoard.Sloupcu;
            PlayBoard.tiles[indexVedlejsi].VyplnitPredemZvyditelnit();
            LevelText = "More than you think";
            BezOdchylky = true;
        }

        /// <summary>
        /// uvede bomby ničící dlaždice neprůchodné hráčem. 
        /// </summary>
        private void Level22()
        {
            koordinat = (radku - 5) * PlayBoard.Sloupcu;
            PlayBoard.tiles[koordinat + 1].Znepruchodnit(); PlayBoard.tiles[koordinat + 2].Znepruchodnit();
            koordinat += PlayBoard.Sloupcu;
            PlayBoard.tiles[koordinat].Znepruchodnit(); PlayBoard.tiles[koordinat + 2].Znepruchodnit(); PlayBoard.tiles[koordinat + 3].Zaminovat(3);
            koordinat += PlayBoard.Sloupcu;
            PlayBoard.tiles[koordinat + 2].Znepruchodnit(); PlayBoard.tiles[koordinat + 3].Znepruchodnit(); PlayBoard.tiles[koordinat + 4].Znepruchodnit();
            koordinat += PlayBoard.Sloupcu; poziceKouli.Add(new Point(0, koordinat / (radku - 1)));
            PlayBoard.tiles[koordinat + 1].NastavZpomalovac(true); PlayBoard.tiles[koordinat + 4].Znepruchodnit();
            koordinat += PlayBoard.Sloupcu;
            PlayBoard.tiles[koordinat + 3].Znepruchodnit();
            numBalls = 7;
            ViteznychProcent = 75;
            LevelText = "Drawbacks";
        }

        /// <summary>
        /// uvede bomby
        /// </summary>
        private void Level21()
        {
            PlayBoard.tilesVnitrni[(radkuUvnitr / 2 - 3) * sloupcuUvnitr + sloupcuUvnitr / 2].Zaminovat(3);
            PlayBoard.tilesVnitrni[(radku / 2) * sloupcuUvnitr + PlayBoard.Sloupcu / 2].Zaminovat(3);
            PlayBoard.tilesVnitrni[0].VyplnitPredemZvyditelnit(); PlayBoard.tilesVnitrni[sloupcuUvnitr + 1].VyplnitPredemZvyditelnit();
            PlayBoard.tilesVnitrni[sloupcuUvnitr * 2 + 2].VyplnitPredemZvyditelnit();

            PlayBoard.tilesVnitrni[PlayBoard.tilesVnitrni.Count - 1].VyplnitPredemZvyditelnit();
            PlayBoard.tilesVnitrni[PlayBoard.tilesVnitrni.Count - 2 - sloupcuUvnitr].VyplnitPredemZvyditelnit();
            PlayBoard.tilesVnitrni[PlayBoard.tilesVnitrni.Count - 3 - sloupcuUvnitr * 2].VyplnitPredemZvyditelnit();
            numBalls = 3; numAttackBalls = 3;
            ViteznychProcent = 75;
            LevelText = "Sweeper?";
        }

        /// <summary>
        /// uvede dlaždice neprůchodné hráčem, pole 7x5
        /// </summary>
        private void Level20()
        {
            koordinat = sloupcuUvnitr * 2 + (sloupcuUvnitr - 7) / 2;
            if (FlipCoin()) 
                koordinat -= sloupcuUvnitr;
            byte i = byte.MinValue;
            while (i < 5)
            {
                PlayBoard.tilesVnitrni[koordinat].ZnepruchodnitHraci();
                if (i != 1 && i != 2 && i != 3)
                {
                    PlayBoard.tilesVnitrni[koordinat + 1].ZnepruchodnitHraci();
                    PlayBoard.tilesVnitrni[koordinat + 2].ZnepruchodnitHraci();
                    PlayBoard.tilesVnitrni[koordinat + 3].ZnepruchodnitHraci();
                    PlayBoard.tilesVnitrni[koordinat + 4].ZnepruchodnitHraci();
                    PlayBoard.tilesVnitrni[koordinat + 5].ZnepruchodnitHraci();
                    if (i % 2 == 0) 
                        poziceKouli.Add(new Point(PrevedNaIndexVsech(koordinat), i + 1));
                    else 
                        poziceUtocnychKouli.Add(new Point(PrevedNaIndexVsech(koordinat), i + 1));
                }
                PlayBoard.tilesVnitrni[koordinat + 6].ZnepruchodnitHraci();
                i++;
                koordinat += sloupcuUvnitr;
            }
            numBalls = 2; numAttackBalls = 2;
            numUtocnychBallsRight = 2;
            ViteznychProcent = 60;
            LevelText = "Where you can't go";
        }

        //level vzhuru nohama

        private void Level10()
        {
            numBalls = 1;
            poziceKouli.Add(new Point(8, 4));
        }

        private void Level11() // uvádí předem vyplněné pole
        {
            numBalls = 2;
            koordinat = (ushort)(PlayBoard.Sloupcu * 2 + 3);
            if (FlipCoin()) 
                koordinat++;
            if (FlipCoin()) 
                koordinat += PlayBoard.Sloupcu;
            PlayBoard.tiles[koordinat].VyplnitPredemZvyditelnit();
            PlayBoard.tiles[koordinat + 1].VyplnitPredemZvyditelnit();
            PlayBoard.tiles[koordinat + PlayBoard.Sloupcu].VyplnitPredemZvyditelnit();
            PlayBoard.tiles[koordinat + PlayBoard.Sloupcu + 1].VyplnitPredemZvyditelnit();
            poziceKouli.Add(new Point(7, 5));
            poziceKouli.Add(new Point(6, 6));
        }

        private void Level12() // uvádí monstrum
        {
            LevelText = "Nowhere is safe?";
            numBalls = 2; ZrodMonstrum = true;
            koordinat = (ushort)(PlayBoard.Sloupcu * 3 + 4);
            if (FlipCoin()) 
                koordinat--;
            PlayBoard.tiles[koordinat].VyplnitPredemZvyditelnit();
            PlayBoard.tiles[koordinat + 1].VyplnitPredemZvyditelnit();
            PlayBoard.tiles[dlazdic - PlayBoard.Sloupcu * 2 - 5].VyplnitPredemZvyditelnit();
            PlayBoard.tiles[dlazdic - PlayBoard.Sloupcu * 2 - 4].VyplnitPredemZvyditelnit();
        }

        private void Level13() // sloupec napříč a první útočná
        {
            LevelText = "What lasts forever?";
            numBalls = 2; numAttackBalls = 2;
            numUtocnychBallsLeft = 1; numUtocnychBallsRight = 1; numUtocnychBallsDown = numUtocnychBallsUp = 1;
            koordinat = (ushort)(PlayBoard.Sloupcu / 2 + PlayBoard.Sloupcu - 2);
            PlayBoard.tiles[koordinat].VyplnitPredemZvyditelnit();
            while (!PlayBoard.tiles[koordinat + PlayBoard.Sloupcu].Okrajova)
            {
                koordinat += PlayBoard.Sloupcu;
                if (koordinat % 2 == 0) 
                    PlayBoard.tiles[koordinat].VyplnitPredemZvyditelnit();
                else 
                    PlayBoard.tiles[koordinat].VyplnitZvyditelnit();
            }
            poziceUtocnychKouli.Add(new Point(8, 5));
            poziceUtocnychKouli.Add(new Point(2, 5));
            poziceKouli.Add(new Point(6, 3));
            poziceKouli.Add(new Point(6, 6));
        }

        private void Level14() // uvádí neprůchodné dlaždice
        {
            numBalls = 3; numAttackBalls = 1;
            LevelText = "There you don't go";
            koordinat = (ushort)(PlayBoard.Sloupcu * 3 + 4);
            if (FlipCoin()) 
                koordinat -= PlayBoard.Sloupcu;
            PlayBoard.tiles[koordinat].Znepruchodnit(); 
            PlayBoard.tiles[koordinat + 2].Znepruchodnit();
            PlayBoard.tiles[koordinat + PlayBoard.Sloupcu + 1].Znepruchodnit();
            koordinat += (ushort)(PlayBoard.Sloupcu * 2);
            PlayBoard.tiles[koordinat].Znepruchodnit();
            PlayBoard.tiles[koordinat + 2].Znepruchodnit();
            ZrodMonstrum = true;
        }

        /// <summary>
        /// prulez se zpomalovacem
        /// </summary>
        private void Level15()
        {
            ViteznychProcent = 75;
            LevelText = "Objective opinion of time";
            numBalls = 2; numAttackBalls = 2;
            koordinat = PlayBoard.Sloupcu * 3 + 3;
            PlayBoard.tiles[koordinat + 1].Znepruchodnit();
            PlayBoard.tiles[koordinat + 2].Znepruchodnit();
            PlayBoard.tiles[koordinat + 3].Znepruchodnit();
            PlayBoard.tiles[koordinat + 4].Znepruchodnit();
            //PlayBoard.tiles[koordinat + 5].Znepruchodnit();
            if (FlipCoin())
                PlayBoard.tiles[koordinat + PlayBoard.Sloupcu + 4].NastavZpomalovac(true);
            else 
                PlayBoard.tiles[koordinat + PlayBoard.Sloupcu * 2 + 4].NastavZpomalovac(true);
            PlayBoard.tiles[koordinat + PlayBoard.Sloupcu + 5].Znepruchodnit();
            PlayBoard.tiles[koordinat + PlayBoard.Sloupcu + 6].Znepruchodnit();
            PlayBoard.tiles[koordinat + PlayBoard.Sloupcu + 7].Znepruchodnit();
            koordinat += PlayBoard.Sloupcu * 2;
            PlayBoard.tiles[koordinat + 1].Znepruchodnit();
            PlayBoard.tiles[koordinat + 2].Znepruchodnit();
            PlayBoard.tiles[koordinat + 3].Znepruchodnit();
            koordinat += PlayBoard.Sloupcu;
            PlayBoard.tiles[koordinat + 4].Znepruchodnit();
            PlayBoard.tiles[koordinat + 5].Znepruchodnit();
            PlayBoard.tiles[koordinat + 6].Znepruchodnit();
            PlayBoard.tiles[koordinat + 7].Znepruchodnit();
            poziceKouli.Add(new Point(12, 3));
            poziceKouli.Add(new Point(12, 8));
        }

        private void Bludiste10()
        {
            LevelText = string.Empty;
            Bludiste = true; numBalls = 2;
            for (int i = 1; i <= dlazdic; i++)
            {
                if (i == 4 || i == 7 || i == 12 || i == 13 || i == 14
                    || i == 17 || i == 19 || i == 20 || i == 22 || i == 23 || i == 25 || i == 27
                    || i == 32 || i == 38 || i == 40 || i == 42 || i == 43 || i == 44
                    || i == 47 || i == 48 || i == 49 || i == 51 || i == 53 || i == 55
                    || i == 62 || i == 66 || i == 70 || i == 71 || i == 72 || i == 73 || i == 74
                    || i == 77 || i == 79 || i == 81 || i == 82 || i == 83 || i == 84
                    || i == 92 || i == 94 || i == 95 || i == 99 || i == 101 || i == 102 || i == 103 || i == 104
                    || i == 107 || i == 112 || i == 114 || i == 116
                    || i == 122 || i == 124 || i == 125 || i == 127 || i == 129 || i == 131 || i == 135
                    || i == 139 || i == 142 || i == 146 || i == 150
                    ) PlayBoard.tiles[i - 1].Znepruchodnit();
                PlayBoard.tiles[132].OznacJakoCilovou(true);
            }
            poziceKouli.Add(new Point(11, 14));
            poziceKouli.Add(new Point(8, 3));
            // poziceUtocnychKouli.Add(new Point(10, 13));
        }

        private void PerformanceTest256() //sloupec napříč a první útočná
        {
            PerformanceTest = true;
            LevelText = "Performance Test 256 + 256";
            numBalls = 255; numAttackBalls = 255;
            numUtocnychBallsLeft = 1; numUtocnychBallsRight = 1; numUtocnychBallsDown = numUtocnychBallsUp = 1;
            koordinat = (ushort)(PlayBoard.Sloupcu / 2 + PlayBoard.Sloupcu - 2);
            PlayBoard.tiles[koordinat].VyplnitPredemZvyditelnit();
            while (!PlayBoard.tiles[koordinat + PlayBoard.Sloupcu].Okrajova)
            {
                koordinat += PlayBoard.Sloupcu;
                if (koordinat % 2 == 0) 
                    PlayBoard.tiles[koordinat].VyplnitPredemZvyditelnit();
                else 
                    PlayBoard.tiles[koordinat].VyplnitZvyditelnit();
            }
            poziceUtocnychKouli.Add(new Point(8, 5));
            poziceUtocnychKouli.Add(new Point(2, 5));
            poziceKouli.Add(new Point(6, 3));
            poziceKouli.Add(new Point(6, 6));
        }

        private void PerformanceTest512() //sloupec napříč a první útočná
        {
            PerformanceTest = true;
            LevelText = "Performance Test 512 + 512";
            numBalls = 512; numAttackBalls = 512;
            numUtocnychBallsLeft = 1; numUtocnychBallsRight = 1; numUtocnychBallsDown = numUtocnychBallsUp = 1;
            koordinat = (ushort)(PlayBoard.Sloupcu / 2 + PlayBoard.Sloupcu - 2);
            PlayBoard.tiles[koordinat].VyplnitPredemZvyditelnit();
            while (!PlayBoard.tiles[koordinat + PlayBoard.Sloupcu].Okrajova)
            {
                koordinat += PlayBoard.Sloupcu;
                if (koordinat % 2 == 0) 
                    PlayBoard.tiles[koordinat].VyplnitPredemZvyditelnit();
                else 
                    PlayBoard.tiles[koordinat].VyplnitZvyditelnit();
            }
            poziceUtocnychKouli.Add(new Point(8, 5));
            poziceUtocnychKouli.Add(new Point(2, 5));
            poziceKouli.Add(new Point(6, 3));
            poziceKouli.Add(new Point(6, 6));
        }

        private static int PrevedNaIndexVsech(int koordinat)
        {
            int cols = koordinat / sloupcuUvnitr;
            int rows = koordinat % sloupcuUvnitr;
            koordinat += cols * 2 + rows;
            return koordinat;
        }

        public void NastavLevel(byte level)
        {
            CisloUrovne = level;
            PripravUroven();
        }

        public void NastavEpisodu(byte maxEpisoda)
        {
            Epizoda = maxEpisoda;
        }

        public void ZvedniUroven()
        {
            CisloUrovne += 1;
            PripravUroven();
        }

        private void PripravUroven()
        {
            ZpomalovatUtocne = false;
            numBalls = numAttackBalls = 0;
            numUtocnychBallsLeft = numUtocnychBallsDown = numUtocnychBallsRight = numUtocnychBallsUp = 0;
            Bludiste = ZrodMonstrum = PoSmeru = PerformanceTest = BezOdchylky = false;
            LevelText = null;
            poziceKouli.Clear(); poziceUtocnychKouli.Clear();
            ViteznychProcent = 70;
        }

        public void PripravEpizodu()
        {
            if (Epizoda == 1)
            {
                if (CisloUrovne == 0) 
                    EpizodaSplash = "Basics";
                else 
                    EpizodaSplash = "Episode 1";
            }
            else if (Epizoda == 2)
            {
                if (CisloUrovne == 0)
                {
                    EpizodaSplash = "Crowd control";
                    Barvy.NastavBarvy(Barvy.E2prvni, Barvy.E2druha);
                }
                else 
                    EpizodaSplash = "Episode 2";
            }
            else if (Epizoda == 3)
            {
                if (CisloUrovne == 0)
                {
                    EpizodaSplash = "Episode 3";
                    Barvy.NastavBarvy(Barvy.E2prvni, Barvy.E2druha);
                }
                else 
                    EpizodaSplash = "Episode 3";
            }
            else EpizodaSplash = "Square got Almighty";
        }

        public static short GetNumBalls()
        {
            return numBalls;
        }

        public static short GetNumAttackBalls()
        {
            return numAttackBalls;
        }

        private static bool FlipCoin()
        {
            if (new Random().Next(2) == 0)
                return false;

            return true;
        }

        //private void TestujUtocne() // prulez se zpomalovacem
        //{
        //    LevelText = "Objective opinion of time";
        //    numBalls = 2; numAttackBalls = 8;
        //    koordinat = PlayBoard.sloupcu * 3 + 3;
        //    PlayBoard.tiles[koordinat + 1].Znepruchodnit();
        //    PlayBoard.tiles[koordinat + 2].Znepruchodnit();
        //    PlayBoard.tiles[koordinat + 3].Znepruchodnit();
        //    PlayBoard.tiles[koordinat + 4].Znepruchodnit();
        //    //PlayBoard.tiles[koordinat + 5].Znepruchodnit();
        //    if (FlipCoin())
        //        PlayBoard.tiles[koordinat + PlayBoard.sloupcu + 4].NastavZpomalovac(true);
        //    else
        //        PlayBoard.tiles[koordinat + PlayBoard.sloupcu * 2 + 4].NastavZpomalovac(true);
        //    PlayBoard.tiles[koordinat + PlayBoard.sloupcu + 5].Znepruchodnit();
        //    PlayBoard.tiles[koordinat + PlayBoard.sloupcu + 6].Znepruchodnit();
        //    PlayBoard.tiles[koordinat + PlayBoard.sloupcu + 7].Znepruchodnit();
        //    koordinat += PlayBoard.sloupcu * 2;
        //    PlayBoard.tiles[koordinat + 1].Znepruchodnit();
        //    PlayBoard.tiles[koordinat + 2].Znepruchodnit();
        //    PlayBoard.tiles[koordinat + 3].Znepruchodnit();
        //    koordinat += PlayBoard.sloupcu;
        //    PlayBoard.tiles[koordinat + 4].Znepruchodnit();
        //    PlayBoard.tiles[koordinat + 5].Znepruchodnit();
        //    PlayBoard.tiles[koordinat + 6].Znepruchodnit();
        //    PlayBoard.tiles[koordinat + 7].Znepruchodnit();
        //    poziceKouli.Add(new Point(12, 3));
        //    poziceKouli.Add(new Point(12, 8));
        //}
        //private void TestBomb()
        //{
        //    koordinat = PlayBoard.sloupcu / 3;
        //    if (FlipCoin()) koordinat--;
        //    while (koordinat < PlayBoard.tiles.Count - PlayBoard.sloupcu)
        //    {
        //        koordinat += PlayBoard.sloupcu;
        //        PlayBoard.tiles[koordinat].Zaminovat(3);
        //        PlayBoard.tiles[koordinat + 2].ZnepruchodnitHraci();
        //        poziceKouli.Add(new Point(koordinat + 5));
        //        poziceUtocnychKouli.Add(new Point(koordinat + 5));
        //    }
        //    numBalls = 3; numAttackBalls = 6;
        //    numUtocnychBallsRight = 3;
        //    viteznychProcent = 75;
        //    LevelText = "More than you think";
        //}
        //private void PerformanceTest128() // sloupec napříč
        //{
        //    performanceTest = true;
        //    LevelText = "Performance Test 128 + 128";
        //    numBalls = numAttackBalls = 128;
        //    numUtocnychBallsLeft = 1; numUtocnychBallsRight = 1; numUtocnychBallsDown = numUtocnychBallsUp = 1;
        //    koordinat = (ushort)(PlayBoard.sloupcu / 2 + PlayBoard.sloupcu - 2);
        //    PlayBoard.tiles[koordinat].VyplnitPredemZvyditelnit();
        //    while (!PlayBoard.tiles[koordinat + PlayBoard.sloupcu].okrajova)
        //    {
        //        koordinat += PlayBoard.sloupcu;
        //        if (koordinat % 2 == 0) 
        //            PlayBoard.tiles[koordinat].VyplnitPredemZvyditelnit();
        //        else 
        //            PlayBoard.tiles[koordinat].VyplnitZvyditelnit();
        //    }
        //    poziceUtocnychKouli.Add(new Point(8, 5));
        //    poziceUtocnychKouli.Add(new Point(2, 5));
        //    poziceKouli.Add(new Point(6, 3));
        //    poziceKouli.Add(new Point(6, 6));
        //}
    }
}