using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace MojehraDroid
{
    internal class Level
    {
        private static ushort sloupcu, radku; private static byte radkuUvnitr, sloupcuUvnitr;
        private static short numBalls, numAttackBalls;
        internal sbyte numUtocnychBallsLeft, numUtocnychBallsRight, numUtocnychBallsUp, numUtocnychBallsDown; //stejne poradi spawnu
        private int koordinat;
        private static int dlazdic;
        internal bool zrodMonstrum, poSmeru, bludiste, performanceTest, bezOdchylky;
        public List<Point> poziceKouli = new List<Point>(4);
        public List<Point> poziceUtocnychKouli = new List<Point>(4);
        internal string levelText;
        internal byte viteznychProcent;
        internal byte epizoda = 1; internal byte cisloUrovne;
        internal static string epizodaSplash;
        internal static bool zpomalovatUtocne;

        internal Level(ushort rows, ushort columns)
        {
            radku = rows;
            radkuUvnitr = (byte)(rows - 2);
            sloupcu = columns;
            sloupcuUvnitr = (byte)(columns - 2);
            dlazdic = radku * sloupcu;
        }

        internal void NastavUroven()
        {
            if (epizoda == 1)
            {
                switch (cisloUrovne)
                {
                    case 0: Level10(); break;
                    case 1: Level11(); break;
                    case 2: Level12(); break;
                    case 3: Level13(); break;
                    case 4: Level14(); break;
                    case 5: Level15(); break;
                    case 6: Bludiste10(); break;
                    case 7: Level20(); epizoda++; cisloUrovne = 0; break;
                }
             }
            else if (epizoda == 2)
            {
                switch (cisloUrovne)
                {
                    case 0: Level20(); break;
                    case 1: Level21(); break;
                    case 2: Level22(); break;
                    case 3: Level23(); break;
                    case 4: Level24(); break;
                    case 5: Level25(); break;
                    case 6: Bludiste20(); break;
                    case 7: Level30(); epizoda++; cisloUrovne = 0; break;
                }
            }
            else if (epizoda == 3)
            {
                switch (cisloUrovne)
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
                    case 7: epizoda++; cisloUrovne = 0; break;
                }
            }
            else if (epizoda == 4)
            {
                switch (cisloUrovne)
                {
                    case 0: PerformanceTest256(); break;
                    case 1: PerformanceTest512(); break;
                    case 2: Level10(); epizoda=1; cisloUrovne = 0; levelText = "From the beginning"; break;//"otocena" hra
                }
            }
        }

        private void Victory()
        {
            bludiste = true;
            for (int i = 0; i< Hlavni.tiles.Count; i++)
            {
                if (i < 15) Hlavni.tiles[i].Znepruchodnit();
                else if (i > 134) Hlavni.tiles[i].Znepruchodnit();
                else if (i % 14 == 0) Hlavni.tiles[i].Znepruchodnit();
                else if (i> sloupcu * 2 && i < sloupcu * radkuUvnitr) Hlavni.tiles[i].Zaminovat(1);
            }
            Hlavni.tiles[Hlavni.tiles.Count-16].OznacJakoCilovou(true);
            numAttackBalls = 499;
            numUtocnychBallsLeft = numUtocnychBallsRight = numUtocnychBallsDown = numUtocnychBallsUp = 1;
            koordinat = (ushort)(sloupcu / 2 + sloupcu - 2);
            Hlavni.tiles[koordinat].VyplnitPredemZvyditelnit();
            while (!Hlavni.tiles[koordinat + sloupcu].okrajova)
            {
                koordinat += sloupcu;
                if (koordinat % 2 == 0) Hlavni.tiles[koordinat].VyplnitPredemZvyditelnit();
                else Hlavni.tiles[koordinat].VyplnitZvyditelnit();
            }
            poziceUtocnychKouli.Add(new Point(8, 5));
            poziceUtocnychKouli.Add(new Point(2, 5));
            poziceKouli.Add(new Point(6, 3));
            poziceKouli.Add(new Point(6, 6));
            levelText = "Victoria";
        }

        /// <summary>
        /// leva strana jde vyplnit hned, ale lepsi je postupovat zprava
        /// </summary>
        private void Level30()
        {
            zpomalovatUtocne = true;
            for (int j = 0; j < sloupcuUvnitr; j++)
            {
                for (int k = 0; k < radkuUvnitr; k++)
                {
                    if (j == sloupcuUvnitr / 2 - 2) Hlavni.tilesVnitrni[k * sloupcuUvnitr + j].VyplnitZvyditelnitOkamzite();
                    else if (j == sloupcuUvnitr / 2 + 2) Hlavni.tilesVnitrni[k * sloupcuUvnitr + j].VyplnitZvyditelnitOkamzite();
                    else if (k == radkuUvnitr / 2 && j == sloupcuUvnitr - 2) Hlavni.tilesVnitrni[k * sloupcuUvnitr + j].NastavZpomalovac(true);
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
            zrodMonstrum = true;
            viteznychProcent = 85;
            levelText = "Priorities";
        }

        private void Level31()
        {
            int j = sloupcuUvnitr - 1;
            for (byte k = 0; k < radku; k++)
            {
                if (k < radkuUvnitr)
                { 
                    Hlavni.tilesVnitrni[k * sloupcuUvnitr + j].VyplnitZvyditelnit();
                    Hlavni.tilesVnitrni[k * sloupcuUvnitr + j - 5].VyplnitZvyditelnitOkamzite();
                }

                if (k > radku - 3)
                {
                    Hlavni.tiles[k * sloupcu + sloupcuUvnitr - 2].ZnepruchodnitHraci();
                    Hlavni.tiles[k * sloupcu + sloupcuUvnitr - 2].ZnepruchodnitHraci();
                }
                else if (k == radkuUvnitr - 1)
                {
                    Hlavni.tiles[k * sloupcu - 1].ZnepruchodnitHraci();
                    Hlavni.tiles[k * sloupcu - 2].ZnepruchodnitHraci();
                }
                j--;
            }

            Hlavni.tilesVnitrni[sloupcuUvnitr * radkuUvnitr - 1].NastavZpomalovac(true);

            Hlavni.tilesVnitrni[sloupcuUvnitr * (radkuUvnitr / 3) + 2].Zaminovat(3);
            Hlavni.tilesVnitrni[sloupcuUvnitr * radkuUvnitr - 4].Zaminovat(2);
            Hlavni.tilesVnitrni[sloupcuUvnitr * (radkuUvnitr - 3) - 1].Zaminovat(2);

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
            viteznychProcent = 72;
            levelText = "Diagonal";
        }

        private void Level32()
        {
            for (int j = 0; j < sloupcuUvnitr; j++)
            {
                for (int k = 0; k < radkuUvnitr; k++)
                {
                    if (j == sloupcuUvnitr / 3) Hlavni.tilesVnitrni[k * sloupcuUvnitr + j].VyplnitZvyditelnitOkamzite();
                    else if (k == radkuUvnitr / 2 || k == radkuUvnitr / 2 - 1)
                    {
                        if (j < sloupcuUvnitr / 3) Hlavni.tilesVnitrni[k * sloupcuUvnitr + j].VyplnitZvyditelnitOkamzite();
                        else if (j == sloupcuUvnitr / 3 + 1) Hlavni.tilesVnitrni[k * sloupcuUvnitr + j].Zaminovat();
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

            zrodMonstrum = true;
            //viteznychProcent = 79;
            levelText = "Containment";
        }

        private void Level33()
        {
            int k = radkuUvnitr;
            for (int j = 1; j < sloupcuUvnitr; j+=2)
            {
                k--;
                {
                    if (k >= 2)
                    {
                        Hlavni.tilesVnitrni[k * sloupcuUvnitr + j].VyplnitZvyditelnitOkamzite();
                        Hlavni.tilesVnitrni[(k - 1) * sloupcuUvnitr + j].VyplnitZvyditelnitOkamzite();
                        Hlavni.tilesVnitrni[(k - 2) * sloupcuUvnitr + j].VyplnitZvyditelnitOkamzite();
                    }
                }
            }

            Hlavni.tilesVnitrni[1 * sloupcuUvnitr + 1].VyplnitZvyditelnit();
            Hlavni.tilesVnitrni[1 * sloupcuUvnitr + 2].VyplnitZvyditelnitOkamzite();
            Hlavni.tilesVnitrni[1 * sloupcuUvnitr + 3].VyplnitZvyditelnitOkamzite();
            Hlavni.tilesVnitrni[1 * sloupcuUvnitr + 4].VyplnitZvyditelnit();

            Hlavni.tilesVnitrni[(radkuUvnitr - 1) * sloupcuUvnitr - 5].VyplnitZvyditelnit();
            Hlavni.tilesVnitrni[(radkuUvnitr - 1) * sloupcuUvnitr - 2].VyplnitZvyditelnit();
            Hlavni.tilesVnitrni[(radkuUvnitr - 1) * sloupcuUvnitr - 3].VyplnitZvyditelnitOkamzite();
            Hlavni.tilesVnitrni[(radkuUvnitr - 1) * sloupcuUvnitr - 4].VyplnitZvyditelnitOkamzite();

            numBalls = 18;
            byte i = byte.MinValue;
            while (i < numAttackBalls)
            {
                poziceUtocnychKouli.Add(new Point(sloupcuUvnitr / 2, i - sloupcuUvnitr));
                i++;
            }

            zrodMonstrum = true;
            levelText = "Traps";
        }

        private void Level34()
        {
            Hlavni.tilesVnitrni[sloupcuUvnitr * 2 + 2].NastavZpomalovac(true);
            Hlavni.tilesVnitrni[sloupcuUvnitr * 3 - 3].NastavZpomalovac(true);
            Hlavni.tilesVnitrni[sloupcuUvnitr * (radkuUvnitr - 3) + 2].NastavZpomalovac(true);
            Hlavni.tilesVnitrni[sloupcuUvnitr * (radkuUvnitr - 2) - 3].NastavZpomalovac(true);

            Hlavni.tilesVnitrni[radkuUvnitr / 2 * sloupcuUvnitr + sloupcuUvnitr / 2].Zaminovat(3);
            Hlavni.tilesVnitrni[(radkuUvnitr / 2 - 1) * sloupcuUvnitr + sloupcuUvnitr / 2].Zaminovat(3);

            numBalls = 22;
            numUtocnychBallsLeft = 4;
            numAttackBalls = 3;
            byte i = byte.MinValue;
            while (i < numBalls)
            {
                poziceUtocnychKouli.Add(new Point(sloupcuUvnitr / 2, i - sloupcuUvnitr));
                i++;
            }

            zrodMonstrum = true;
            levelText = "slowdown";
        }

        private void Level35()
        {
            Hlavni.tiles[3].ZnepruchodnitHraci();
            Hlavni.tiles[sloupcu - 4].ZnepruchodnitHraci();
            for (int j = 0; j < sloupcuUvnitr; j++)
            {
                for (int k = 0; k < radkuUvnitr; k++)
                {
                    if (k == 0)
                    {
                        Hlavni.tilesVnitrni[2].Znepruchodnit();
                        Hlavni.tilesVnitrni[sloupcuUvnitr - 3].Znepruchodnit();
                        Hlavni.tilesVnitrni[sloupcuUvnitr/2 - 1].NastavZpomalovac(true);
                        Hlavni.tilesVnitrni[sloupcuUvnitr / 2 + 1].NastavZpomalovac(true);
                    }
                    else if (k == 1)
                    {
                        Hlavni.tilesVnitrni[sloupcuUvnitr + 3].Znepruchodnit();
                        Hlavni.tilesVnitrni[sloupcuUvnitr * 2 - 4].Znepruchodnit();
                        Hlavni.tilesVnitrni[sloupcuUvnitr + sloupcuUvnitr / 2].NastavZpomalovac(true);
                    }
                    else if (k == 2)
                    {
                        Hlavni.tilesVnitrni[sloupcuUvnitr * 2 + 4].Znepruchodnit();
                        Hlavni.tilesVnitrni[sloupcuUvnitr * 3 - 5].Znepruchodnit();
                    }
                    else if (k == 3)
                    {
                        Hlavni.tilesVnitrni[sloupcuUvnitr * 3 + 5].Znepruchodnit();
                        Hlavni.tilesVnitrni[sloupcuUvnitr * 4 - 6].Znepruchodnit();
                        Hlavni.tilesVnitrni[sloupcuUvnitr * 3 + 6].NastavOzivovaci(true);

                        Hlavni.tilesVnitrni[sloupcuUvnitr * 3 + 2].Zaminovat(2);
                        Hlavni.tilesVnitrni[sloupcuUvnitr * 4 - 3].Zaminovat(2);
                    }
                    else if (k == 4)
                    {
                        Hlavni.tilesVnitrni[sloupcuUvnitr * 4 + 6].NastavOzivovaci(true);
                        Hlavni.tilesVnitrni[sloupcuUvnitr * 4 + 6].Znepruchodnit();
                        Hlavni.tilesVnitrni[sloupcuUvnitr * 4 + 8].Zaminovat(2);

                        Hlavni.tilesVnitrni[sloupcuUvnitr * 4].VyplnitPredemZvyditelnit();
                        Hlavni.tilesVnitrni[sloupcuUvnitr * 4 + 1].VyplnitZvyditelnit();
                        Hlavni.tilesVnitrni[sloupcuUvnitr * 5 - 1].VyplnitPredemZvyditelnit();
                        Hlavni.tilesVnitrni[sloupcuUvnitr * 5 - 2].VyplnitZvyditelnit();
                    }
                    else if (k == 5)
                    {
                        Hlavni.tilesVnitrni[sloupcuUvnitr * 5 + 5].ZnepruchodnitHraci();
                        Hlavni.tilesVnitrni[sloupcuUvnitr * 6 - 6].ZnepruchodnitHraci();
                        Hlavni.tilesVnitrni[sloupcuUvnitr * 5 + 6].NastavOzivovaci(true);
                    }
                    else if (k > radkuUvnitr - 3)
                    {
                        if (j == 3 || j == sloupcuUvnitr - 4)
                        {
                            Hlavni.tilesVnitrni[k * sloupcuUvnitr + j].VyplnitPredemZvyditelnit();
                        }
                    }
                }
            }
            numBalls = 8;
            byte i = byte.MinValue;
            while (i < numBalls)
            {
                poziceKouli.Add(new Point(sloupcuUvnitr / 2 + 1, (ushort)(i/1.3)));
                poziceKouli.Add(new Point(sloupcuUvnitr / 2, sloupcuUvnitr - 2));
                i +=2;
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

            zrodMonstrum = true;
            levelText = "Priorities";
        }

        private void Bludiste30()
        {
            bludiste = true; numBalls = 3;
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
                    ) Hlavni.tiles[i - 1].Znepruchodnit();

                Hlavni.tiles[62].Zaminovat(2); Hlavni.tiles[66].Zaminovat(2);

                Hlavni.tiles[46].ZnepruchodnitHraci(); Hlavni.tiles[47].ZnepruchodnitHraci(); Hlavni.tiles[48].ZnepruchodnitHraci();
                //Hlavni.tiles[49].Znepruchodnit(); Hlavni.tiles[50].Znepruchodnit();
                //Hlavni.tiles[51].Znepruchodnit(); Hlavni.tiles[52].Znepruchodnit();
                //Hlavni.tiles[53].Znepruchodnit(); Hlavni.tiles[54].Znepruchodnit();
                Hlavni.tiles[55].ZnepruchodnitHraci(); Hlavni.tiles[55].VyplnitPredemZvyditelnit();
                //Hlavni.tiles[56].Znepruchodnit(); Hlavni.tiles[57].Znepruchodnit(); Hlavni.tiles[59].Znepruchodnit();

                //Hlavni.tiles[52].ZnepruchodnitHraci(); Hlavni.tiles[67].ZnepruchodnitHraci();
                Hlavni.tiles[92].ZnepruchodnitHraci(); Hlavni.tiles[96].ZnepruchodnitHraci();
                Hlavni.tiles[100].ZnepruchodnitHraci(); Hlavni.tiles[100].VyplnitPredemZvyditelnit();
                Hlavni.tiles[135].OznacJakoCilovou(true);
            }
            poziceKouli.Add(new Point(2, 8)); poziceKouli.Add(new Point(14, 8)); poziceKouli.Add(new Point(7, 8));
            numAttackBalls = 2; numUtocnychBallsDown = 2;
            poziceUtocnychKouli.Add(new Point(2, 1)); poziceUtocnychKouli.Add(new Point(5, 1));

            levelText = "Not Straight Outta";
        }

        private void Bludiste20()
        {
            bludiste = true; numBalls = 3;
            for (int i = 1; i <= dlazdic; i++)
            {
                if (i == 4 || i == 7 || i == 9 || i == 10|| i==14
                    || i == 21 || i == 23 || i == 27||i==29
                    || i == 32 || i == 34 || i == 35 || i == 36 || i == 38 || i == 39 || i == 40 || i == 42
                    || i == 47 || i == 55 || i == 57 || i == 58 || i == 59 || i == 60
                    || i == 63 || i == 64 || i == 65 
                    || i == 77 || i == 82 || i == 83 || i == 84 || i == 85 || i == 87 || i == 88 || i == 89
                    || i == 92 || i == 94 || i == 95 || i == 97 || i == 98 || i == 100 || i == 102
                    || i == 107 || i == 111 || i == 112 || i == 117 || i == 119
                    || i == 122 || i == 123 || i == 124 || i == 126 || i == 127 || i == 129 || i == 130 || i == 132 || i == 134
                    || i == 141 || i == 146 || i == 150
                    ) Hlavni.tiles[i - 1].Znepruchodnit();
                Hlavni.tiles[51].Zaminovat(1); Hlavni.tiles[52].ZnepruchodnitHraci(); Hlavni.tiles[66].Zaminovat(1); Hlavni.tiles[67].ZnepruchodnitHraci();
                Hlavni.tiles[14].OznacJakoCilovou(true);
            }
            poziceKouli.Add(new Point(14, 8)); poziceKouli.Add(new Point(14, 2)); poziceKouli.Add(new Point(7, 4));
            levelText = "Breakaway"; //v bludisti by svitil porad?
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
                    if (j < radku / 2) Hlavni.tilesVnitrni[k * sloupcuUvnitr + j].VyplnitZvyditelnitOkamzite();
                    else if (j == sloupcuUvnitr - 1) Hlavni.tilesVnitrni[k * sloupcuUvnitr + j].VyplnitZvyditelnitOkamzite();
                }
            }

            koordinat = (sloupcu / 2 + 1);
            byte i = byte.MinValue;
            while (i < radkuUvnitr)
            {
                Hlavni.tilesVnitrni[koordinat].ZnepruchodnitHraci();
                if (i != 1 && i != 2 && i != 3 && i != 4 && i != 5 && i != 6)
                {
                    Hlavni.tilesVnitrni[koordinat + 1].ZnepruchodnitHraci();
                    Hlavni.tilesVnitrni[koordinat + 2].ZnepruchodnitHraci();
                }
                else if (i == 3 || i == 4) Hlavni.tilesVnitrni[koordinat - 1].Zaminovat(3);
                Hlavni.tilesVnitrni[koordinat + 3].ZnepruchodnitHraci();
                i++;
                koordinat += sloupcuUvnitr;
            }
            poziceKouli.Add(new Point(14, radku / 2)); poziceKouli.Add(new Point(14, radku / 2 - 1));
            poziceKouli.Add(new Point(14, radku / 2 - 2)); poziceKouli.Add(new Point(12, radku / 2 - 3));
            poziceUtocnychKouli.Add(new Point(14, radku / 2)); poziceUtocnychKouli.Add(new Point(14, radku / 2 - 1));
            poziceUtocnychKouli.Add(new Point(sloupcuUvnitr/5, radkuUvnitr / 2));
            numBalls = 4; numAttackBalls = 3;
            numUtocnychBallsLeft = 2; numUtocnychBallsRight = 1;
            zrodMonstrum = true;
            viteznychProcent = 76;
            levelText = "Out of ideas";
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
                    if (j < radku / 2) Hlavni.tilesVnitrni[k * sloupcuUvnitr + j].VyplnitZvyditelnitOkamzite();
                    else if (j == sloupcuUvnitr - 1) Hlavni.tilesVnitrni[k * sloupcuUvnitr + j].VyplnitZvyditelnitOkamzite();
                }
            }

            koordinat = (sloupcu/2 + 1);
            byte i = byte.MinValue;
            while (i < radkuUvnitr)
            {
                Hlavni.tilesVnitrni[koordinat].ZnepruchodnitHraci();
                if (i != 1 && i != 2 && i != 3 && i != 4 && i != 5 && i != 6)
                {
                    Hlavni.tilesVnitrni[koordinat + 1].ZnepruchodnitHraci();
                    Hlavni.tilesVnitrni[koordinat + 2].ZnepruchodnitHraci();
                }
                else if (i==3 || i ==4) Hlavni.tilesVnitrni[koordinat - 1].Zaminovat(3);
                Hlavni.tilesVnitrni[koordinat + 3].ZnepruchodnitHraci();
                i++;
                koordinat += sloupcuUvnitr;
            }
            poziceKouli.Add(new Point(12, radku/2)); poziceKouli.Add(new Point(12, radku/2 - 1));
            poziceKouli.Add(new Point(14, radku/2 - 2)); poziceKouli.Add(new Point(14, radku/2 - 3));
            poziceUtocnychKouli.Add(new Point(14, radku/2)); poziceUtocnychKouli.Add(new Point(14, radku/2 - 1));
            poziceUtocnychKouli.Add(new Point(12, radku / 2 + 1)); poziceUtocnychKouli.Add(new Point(12, radku/2 + -3));
            numBalls = 4; numAttackBalls = 4;
            numUtocnychBallsLeft = 2; numUtocnychBallsRight = 2;
            viteznychProcent = 74;
            levelText = "Contra";
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
            int indexVedlejsi = radku / 2 * sloupcu + sloupcu / 2;
            if (flipCoin()) indexVedlejsi -= sloupcu;
            Hlavni.tiles[indexVedlejsi].VyplnitPredemZvyditelnit();
            levelText = "More than you think";
            bezOdchylky = true;
        }

        /// <summary>
        /// uvede bomby ničící dlaždice neprůchodné hráčem. 
        /// </summary>
        private void Level22()
        {
            koordinat = ((radku - 5) * sloupcu);
            Hlavni.tiles[koordinat+1].Znepruchodnit(); Hlavni.tiles[koordinat + 2].Znepruchodnit();
            koordinat += sloupcu;
            Hlavni.tiles[koordinat].Znepruchodnit(); Hlavni.tiles[koordinat + 2].Znepruchodnit(); Hlavni.tiles[koordinat + 3].Zaminovat(3);
            koordinat += sloupcu;
            Hlavni.tiles[koordinat + 2].Znepruchodnit(); Hlavni.tiles[koordinat + 3].Znepruchodnit(); Hlavni.tiles[koordinat + 4].Znepruchodnit();
            koordinat += sloupcu; poziceKouli.Add(new Point(0, koordinat/(radku - 1) ) );
            Hlavni.tiles[koordinat + 1].NastavZpomalovac(true); Hlavni.tiles[koordinat + 4].Znepruchodnit();
            koordinat += sloupcu;
            Hlavni.tiles[koordinat + 3].Znepruchodnit();
            numBalls = 7; 
            viteznychProcent = 75;
            levelText = "Drawbacks";
        }

        /// <summary>
        /// uvede bomby
        /// </summary>
        private void Level21()
        {
            Hlavni.tilesVnitrni[(radkuUvnitr/2 - 3) * sloupcuUvnitr + sloupcuUvnitr/2].Zaminovat(3);
            Hlavni.tilesVnitrni[(radku/2) * sloupcuUvnitr + sloupcu/2].Zaminovat(3);
            Hlavni.tilesVnitrni[0].VyplnitPredemZvyditelnit(); Hlavni.tilesVnitrni[sloupcuUvnitr + 1].VyplnitPredemZvyditelnit();
            Hlavni.tilesVnitrni[sloupcuUvnitr * 2 +2].VyplnitPredemZvyditelnit();

            Hlavni.tilesVnitrni[Hlavni.tilesVnitrni.Count - 1].VyplnitPredemZvyditelnit();
            Hlavni.tilesVnitrni[Hlavni.tilesVnitrni.Count - 2 - sloupcuUvnitr].VyplnitPredemZvyditelnit();
            Hlavni.tilesVnitrni[Hlavni.tilesVnitrni.Count - 3 - sloupcuUvnitr * 2].VyplnitPredemZvyditelnit();
            numBalls = 3; numAttackBalls = 3;
            viteznychProcent = 75;
            levelText = "Sweeper?";
        }

        /// <summary>
        /// uvede dlaždice neprůchodné hráčem, pole 7x5
        /// </summary>
        private void Level20()
        {
            koordinat = (sloupcuUvnitr * 2 + (sloupcuUvnitr - 7) / 2);
            if (flipCoin()) koordinat -= sloupcuUvnitr;
            byte i = byte.MinValue;
            while (i < 5)
            {
                Hlavni.tilesVnitrni[koordinat].ZnepruchodnitHraci();
                if (i != 1 && i != 2 && i != 3)
                { 
                    Hlavni.tilesVnitrni[koordinat + 1].ZnepruchodnitHraci();
                    Hlavni.tilesVnitrni[koordinat + 2].ZnepruchodnitHraci();
                    Hlavni.tilesVnitrni[koordinat + 3].ZnepruchodnitHraci();
                    Hlavni.tilesVnitrni[koordinat + 4].ZnepruchodnitHraci();
                    Hlavni.tilesVnitrni[koordinat + 5].ZnepruchodnitHraci();
                    if (i % 2 == 0) poziceKouli.Add(new Point(PrevedNaIndexVsech(koordinat), i + 1));
                    else poziceUtocnychKouli.Add(new Point(PrevedNaIndexVsech(koordinat), i + 1));
                }
                Hlavni.tilesVnitrni[koordinat + 6].ZnepruchodnitHraci();
                i++;
                koordinat += sloupcuUvnitr;
            }
            numBalls = 2; numAttackBalls = 2;
            numUtocnychBallsRight = 2;
            viteznychProcent = 60;
            levelText = "Where you can't go";
        }

        //level vzhuru nohama

        private void Level10()
        {
            numBalls = 1;
            poziceKouli.Add(new Point(8, 4));
        }

        private void Level11() //uvádí předem vyplněné koule
        {
            numBalls = 2;
            koordinat = (ushort)(sloupcu * 2 + 3);
            if (flipCoin()) koordinat++;
            if (flipCoin()) koordinat+=sloupcu;
            Hlavni.tiles[koordinat].VyplnitPredemZvyditelnit();
            Hlavni.tiles[koordinat + 1].VyplnitPredemZvyditelnit();
            Hlavni.tiles[koordinat + sloupcu].VyplnitPredemZvyditelnit();
            Hlavni.tiles[koordinat + sloupcu + 1].VyplnitPredemZvyditelnit();
            poziceKouli.Add(new Point(7, 5));
            poziceKouli.Add(new Point(6, 6));
        }

        private void Level12() //uvádí monstrum
        {
            levelText = "Nowhere is safe?";
            numBalls = 2; zrodMonstrum = true;
            koordinat = (ushort)(sloupcu * 3 + 4);
            if (flipCoin()) koordinat--;
            Hlavni.tiles[koordinat].VyplnitPredemZvyditelnit();
            Hlavni.tiles[koordinat + 1].VyplnitPredemZvyditelnit();
            Hlavni.tiles[dlazdic - sloupcu * 2 - 5].VyplnitPredemZvyditelnit();
            Hlavni.tiles[dlazdic - sloupcu * 2 - 4].VyplnitPredemZvyditelnit();
        }

        private void Level13() //sloupec napříč a první útočná
        {
            levelText = "What lasts forever?";
            numBalls = 2; numAttackBalls = 2;
            numUtocnychBallsLeft = 1; numUtocnychBallsRight = 1; numUtocnychBallsDown = numUtocnychBallsUp = 1;
            koordinat = (ushort)(sloupcu / 2 + sloupcu - 2);
            Hlavni.tiles[koordinat].VyplnitPredemZvyditelnit();
            while (!Hlavni.tiles[koordinat + sloupcu].okrajova)
            {
                koordinat += sloupcu;
                if (koordinat % 2 == 0) Hlavni.tiles[koordinat].VyplnitPredemZvyditelnit();
                else Hlavni.tiles[koordinat].VyplnitZvyditelnit();
            }
            poziceUtocnychKouli.Add(new Point(8, 5));
            poziceUtocnychKouli.Add(new Point(2, 5));
            poziceKouli.Add(new Point(6, 3));
            poziceKouli.Add(new Point(6, 6));
        }

        private void Level14() //uvádí neprůchodné dlaždice
        {
            numBalls = 3; numAttackBalls = 1;
            levelText = "There you don't go";
            koordinat = (ushort)(sloupcu * 3 + 4);
            if (flipCoin()) koordinat -= sloupcu;
            Hlavni.tiles[koordinat].Znepruchodnit(); Hlavni.tiles[koordinat + 2].Znepruchodnit();
            Hlavni.tiles[koordinat + sloupcu + 1].Znepruchodnit();
            koordinat += (ushort)(sloupcu * 2);
            Hlavni.tiles[koordinat].Znepruchodnit(); Hlavni.tiles[koordinat + 2].Znepruchodnit();
            zrodMonstrum = true;
        }

        /// <summary>
        /// prulez se zpomalovacem
        /// </summary>
        private void Level15()
        {
            viteznychProcent = 75;
            levelText = "Objective opinion of time";
            numBalls = 2; numAttackBalls = 2;
            koordinat = (sloupcu * 3 + 3);
            Hlavni.tiles[koordinat + 1].Znepruchodnit();
            Hlavni.tiles[koordinat + 2].Znepruchodnit();
            Hlavni.tiles[koordinat + 3].Znepruchodnit();
            Hlavni.tiles[koordinat + 4].Znepruchodnit();
            //Hlavni.tiles[koordinat + 5].Znepruchodnit();
            if (flipCoin()) Hlavni.tiles[koordinat + sloupcu + 4].NastavZpomalovac(true);
            else Hlavni.tiles[koordinat + sloupcu * 2 + 4].NastavZpomalovac(true);
            Hlavni.tiles[koordinat + sloupcu + 5].Znepruchodnit();
            Hlavni.tiles[koordinat + sloupcu + 6].Znepruchodnit();
            Hlavni.tiles[koordinat + sloupcu + 7].Znepruchodnit();
            koordinat += sloupcu * 2;
            Hlavni.tiles[koordinat + 1].Znepruchodnit();
            Hlavni.tiles[koordinat + 2].Znepruchodnit();
            Hlavni.tiles[koordinat + 3].Znepruchodnit();
            koordinat += sloupcu;
            Hlavni.tiles[koordinat + 4].Znepruchodnit();
            Hlavni.tiles[koordinat + 5].Znepruchodnit();
            Hlavni.tiles[koordinat + 6].Znepruchodnit();
            Hlavni.tiles[koordinat + 7].Znepruchodnit();
            poziceKouli.Add(new Point(12, 3));
            poziceKouli.Add(new Point(12, 8));
        }

        private void Bludiste10()
        {
            levelText = string.Empty;
            bludiste = true; numBalls = 2;
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
                    ) Hlavni.tiles[i - 1].Znepruchodnit();
                Hlavni.tiles[132].OznacJakoCilovou(true);
            }
            poziceKouli.Add(new Point(11, 14));
            poziceKouli.Add(new Point(8, 3));
            //poziceUtocnychKouli.Add(new Point(10, 13));
        }


        private void Level16() //level se zemetresenim
        {
            koordinat = sloupcu * 3;
            if (flipCoin()) koordinat += sloupcu;
            if (flipCoin()) koordinat--;
            Hlavni.tilesVnitrni[koordinat].Zaminovat(3);
            Hlavni.tilesVnitrni[koordinat + sloupcu - 2].Zaminovat(5);
            numBalls = 1; numUtocnychBallsRight = 1;
            poziceKouli.Add(new Point(8, 4));
            viteznychProcent = 90;
        }

        private void PerformanceTest128() //sloupec napříč a první útočná
        {
            performanceTest = true;
            levelText = "Performance Test 128 + 128";
            numBalls = numAttackBalls = 128;
            numUtocnychBallsLeft = 1; numUtocnychBallsRight = 1; numUtocnychBallsDown = numUtocnychBallsUp = 1;
            koordinat = (ushort)(sloupcu / 2 + sloupcu - 2);
            Hlavni.tiles[koordinat].VyplnitPredemZvyditelnit();
            while (!Hlavni.tiles[koordinat + sloupcu].okrajova)
            {
                koordinat += sloupcu;
                if (koordinat % 2 == 0) Hlavni.tiles[koordinat].VyplnitPredemZvyditelnit();
                else Hlavni.tiles[koordinat].VyplnitZvyditelnit();
            }
            poziceUtocnychKouli.Add(new Point(8, 5));
            poziceUtocnychKouli.Add(new Point(2, 5));
            poziceKouli.Add(new Point(6, 3));
            poziceKouli.Add(new Point(6, 6));
        }

        private void PerformanceTest256() //sloupec napříč a první útočná
        {
            performanceTest = true;
            levelText = "Performance Test 256 + 256";
            numBalls = 255; numAttackBalls = 255;
            numUtocnychBallsLeft = 1; numUtocnychBallsRight = 1; numUtocnychBallsDown = numUtocnychBallsUp = 1;
            koordinat = (ushort)(sloupcu / 2 + sloupcu - 2);
            Hlavni.tiles[koordinat].VyplnitPredemZvyditelnit();
            while (!Hlavni.tiles[koordinat + sloupcu].okrajova)
            {
                koordinat += sloupcu;
                if (koordinat % 2 == 0) Hlavni.tiles[koordinat].VyplnitPredemZvyditelnit();
                else Hlavni.tiles[koordinat].VyplnitZvyditelnit();
            }
            poziceUtocnychKouli.Add(new Point(8, 5));
            poziceUtocnychKouli.Add(new Point(2, 5));
            poziceKouli.Add(new Point(6, 3));
            poziceKouli.Add(new Point(6, 6));
        }

        private void PerformanceTest512() //sloupec napříč a první útočná
        {
            performanceTest = true;
            levelText = "Performance Test 512+512";
            numBalls = 512; numAttackBalls = 512;
            numUtocnychBallsLeft = 1; numUtocnychBallsRight = 1; numUtocnychBallsDown = numUtocnychBallsUp = 1;
            koordinat = (ushort)(sloupcu / 2 + sloupcu - 2);
            Hlavni.tiles[koordinat].VyplnitPredemZvyditelnit();
            while (!Hlavni.tiles[koordinat + sloupcu].okrajova)
            {
                koordinat += sloupcu;
                if (koordinat % 2 == 0) Hlavni.tiles[koordinat].VyplnitPredemZvyditelnit();
                else Hlavni.tiles[koordinat].VyplnitZvyditelnit();
            }
            poziceUtocnychKouli.Add(new Point(8, 5));
            poziceUtocnychKouli.Add(new Point(2, 5));
            poziceKouli.Add(new Point(6, 3));
            poziceKouli.Add(new Point(6, 6));
        }

        private void TestujUtocne()//prulez se zpomalovacem
        {
            levelText = "Objective opinion of time";
            numBalls = 2; numAttackBalls = 8;
            koordinat = (sloupcu * 3 + 3);
            Hlavni.tiles[koordinat + 1].Znepruchodnit();
            Hlavni.tiles[koordinat + 2].Znepruchodnit();
            Hlavni.tiles[koordinat + 3].Znepruchodnit();
            Hlavni.tiles[koordinat + 4].Znepruchodnit();
            //Hlavni.tiles[koordinat + 5].Znepruchodnit();
            if (flipCoin()) Hlavni.tiles[koordinat + sloupcu + 4].NastavZpomalovac(true);
            else Hlavni.tiles[koordinat + sloupcu * 2 + 4].NastavZpomalovac(true);
            Hlavni.tiles[koordinat + sloupcu + 5].Znepruchodnit();
            Hlavni.tiles[koordinat + sloupcu + 6].Znepruchodnit();
            Hlavni.tiles[koordinat + sloupcu + 7].Znepruchodnit();
            koordinat += sloupcu * 2;
            Hlavni.tiles[koordinat + 1].Znepruchodnit();
            Hlavni.tiles[koordinat + 2].Znepruchodnit();
            Hlavni.tiles[koordinat + 3].Znepruchodnit();
            koordinat += sloupcu;
            Hlavni.tiles[koordinat + 4].Znepruchodnit();
            Hlavni.tiles[koordinat + 5].Znepruchodnit();
            Hlavni.tiles[koordinat + 6].Znepruchodnit();
            Hlavni.tiles[koordinat + 7].Znepruchodnit();
            poziceKouli.Add(new Point(12, 3));
            poziceKouli.Add(new Point(12, 8));
        }

        private void TestBomb()
        {
            koordinat = (sloupcu / 3);
            if (flipCoin()) koordinat--;
            while (koordinat < Hlavni.tiles.Count - sloupcu)
            {
                koordinat += sloupcu;
                Hlavni.tiles[koordinat].Zaminovat(3);
                Hlavni.tiles[koordinat + 2].ZnepruchodnitHraci();
                poziceKouli.Add(new Point(koordinat + 5));
                poziceUtocnychKouli.Add(new Point(koordinat + 5));
            }
            numBalls = 3; numAttackBalls = 6;
            numUtocnychBallsRight = 3;
            viteznychProcent = 75;
            levelText = "More than you think";
        }

        private int PrevedNaIndexVsech(int koordinat)
        {
            int cols = koordinat / sloupcuUvnitr;
            int rows = koordinat % sloupcuUvnitr;
            koordinat += cols * 2 + rows;
            return koordinat;
        }

        internal void NastavLevel(byte level)
        {
            cisloUrovne = level;
            PripravUroven();
        }

        internal void NastavEpisodu(byte maxEpisoda)
        {
            epizoda = maxEpisoda;
        }

        internal void ZvedniUroven()
        {
            cisloUrovne += 1;
            PripravUroven();
        }

        private void PripravUroven()
        {
            zpomalovatUtocne = false;
            numBalls = numAttackBalls = 0;
            numUtocnychBallsLeft = numUtocnychBallsDown = numUtocnychBallsRight = numUtocnychBallsUp = 0;
            bludiste = zrodMonstrum = poSmeru = performanceTest = bezOdchylky = false;
            levelText = null;
            poziceKouli.Clear(); poziceUtocnychKouli.Clear();
            viteznychProcent = 70;
        }

        internal void PripravEpizodu()
        {            
            if (epizoda == 1)
            {
                if (cisloUrovne == 0) epizodaSplash = "Basics";
                else epizodaSplash = "Episode 1";
            }
            else if (epizoda == 2)
            {
                if (cisloUrovne == 0)
                {
                    epizodaSplash = "Crowd control";
                    Barvy.NastavBarvy(Barvy.E2prvni, Barvy.E2druha);
                }
                else epizodaSplash = "Episode 2";
            }
            else if (epizoda == 3)
            {
                if (cisloUrovne == 0)
                {
                    epizodaSplash = "Episode 3";
                    Barvy.NastavBarvy(Barvy.E2prvni, Barvy.E2druha);
                }
                else epizodaSplash = "Episode 3";
            }
            else epizodaSplash = "Square got Almighty";
        }

        internal static short GetNumBalls()
        {
            return numBalls;
        }

        internal static short GetNumAttackBalls()
        {
            return numAttackBalls;
        }

        internal static bool flipCoin()
        {
            Random rand = new Random();
            if (rand.Next(2) == 0) return false;
            else return true;
        }
    }
}