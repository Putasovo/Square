using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Square
{
    public class Hrac
    {
        // pro kb?
        private int xSpeed, ySpeed;

        private bool animovan;
        private Vector2 scale, stredOtaceni, pozice;
        private float rotace; 
        private ushort kroku, celkemKroku;
        private readonly ushort maxX, maxY;
        private readonly ushort sloupcu;
        private readonly ushort speed;
        private readonly short krok, pulkrok;
        private bool pohybVlevo, pohybVpravo, pohybNahoru, pohybDolu;

        private bool alive;
        private bool prepocistSkore;
        private bool vpoli, namiste; // , svislyVyjezd;
        internal bool zleva, zhora, zprava, zdola;

        // private int vysledek, vysledekX, vysledekY;     
        // private static int pulsirky, pulvysky;
        private int predesleX, predesleY;
        private Point souradnice;        
        
        private int indexPristiDlazdice;
        private ushort indexCiloveDlazdice;

        private readonly Texture2D spriteHracovo;
        private Rectangle souradniceVysledneTextury;
        private static Rectangle stoji, doprava, doleva, nahoru, dolu, strach, mrtvy;
        public Rectangle hracovo;
        internal int IndexDlazdice { get; private set; }
        public int VychoziX { get; private set; }
        public int VychoziY { get; private set; }

        public Hrac(bool zije, ushort rychlost, short dimenze, int X, int Y, int fieldWidth, int fieldHeight,
            Texture2D sprite)
        {
            alive = zije;
            speed = rychlost; 
            krok = dimenze;
            pulkrok = (short)(krok/2);
            hracovo = new Rectangle(X, Y, krok, krok);            
            stoji = new Rectangle(0, krok, krok, krok); 
            strach = new Rectangle(krok, krok, krok, krok);
            mrtvy = new Rectangle(64, 32, krok, krok);
            doprava = new Rectangle(0, 0, krok, krok); 
            doleva = new Rectangle(32, 0, krok, krok);
            nahoru = new Rectangle(64, 0, krok, krok); 
            dolu = new Rectangle(96, 0, krok, krok);
            souradnice = new Point(hracovo.X, hracovo.Y);
                        
            // pulsirky = fieldWidth / 2;
            // pulvysky = fieldHeight / 2;
            maxX = (ushort)(fieldWidth - dimenze);
            maxY = (ushort)(fieldHeight - dimenze);
            sloupcu = (ushort)(maxX / krok);
            spriteHracovo = sprite;
        }

        /// <summary>
        /// Updejt by měl dostat souřadnici doteku, za ní hráč pojede
        /// </summary>
        /// <param name="novasouradnice"></param>
        public void Update(Point novasouradnice)
        {
            if (alive)
            {
                if (hracovo.X % krok == 0 && hracovo.Y % krok == 0)
                {
                    namiste = true;
                    predesleX = hracovo.X; predesleY = hracovo.Y;
                    ZpracujZvlastniDlazdice();
                    ZpracujCestu();
                    if (novasouradnice != Point.Zero && souradnice != novasouradnice)
                    {
                        souradnice = novasouradnice;
                        PlayBoard.tiles[indexCiloveDlazdice].Odvyrazni();
                        indexCiloveDlazdice = (ushort)(souradnice.X / krok + souradnice.Y / krok * PlayBoard.Sloupcu);                        
                        PlayBoard.tiles[indexCiloveDlazdice].Zvyrazni();
                        // souradnice.X = UrovnejSouradnici((int)souradnice.X);//bez castu nemuzu zkouset modulo                        
                    }
                    UrciKamJet(souradnice);
                }
                else 
                    namiste = false;

                Hejbni(vpoli);
            }
        }

        public void UpdateBludiste(Point novaSouradnice)
        {
            if (alive)
            {
                if (hracovo.X % krok == 0 && hracovo.Y % krok == 0)
                {
                    namiste = true;
                    predesleX = hracovo.X; predesleY = hracovo.Y;
                    ZpracujZvlastniDlazdice();
                    if (PlayBoard.tiles[IndexDlazdice].Cilova) 
                        prepocistSkore = true;
                    else if (novaSouradnice != Point.Zero && souradnice != novaSouradnice)
                        souradnice = novaSouradnice;

                    UrciKamJet(souradnice);
                }
                else 
                    namiste = false;

                Hejbni(vpoli);
            }
        }

        /// <summary>
        /// Updejt by měl dostat souřadnici doteku, za ní hráč pojede
        /// </summary>
        /// <param name="novasouradnice"></param>
        public void UpdateMouse(Point novasouradnice)
        {
            if (alive)
            {
                if (hracovo.X % krok == 0 && hracovo.Y % krok == 0)
                {
                    namiste = true;
                    predesleX = hracovo.X; predesleY = hracovo.Y;
                    ZpracujZvlastniDlazdice();
                    ZpracujCestu();
                    if (novasouradnice != Point.Zero && souradnice != novasouradnice)
                    {
                        souradnice.X = novasouradnice.X < maxX + krok ? novasouradnice.X : maxX; // osekat kdyz jde klikat mimo plochu
                        souradnice.Y = novasouradnice.Y < maxY + krok ? novasouradnice.Y : maxY;
                        PlayBoard.tiles[indexCiloveDlazdice].Odvyrazni();
                        indexCiloveDlazdice = (ushort)(souradnice.X / krok + souradnice.Y / krok * PlayBoard.Sloupcu);
                        PlayBoard.tiles[indexCiloveDlazdice].Zvyrazni();
                        // souradnice.X = UrovnejSouradnici((int)souradnice.X); // bez castu nemuzu zkouset modulo                        
                    }
                    UrciKamJet(souradnice);
                }
                else
                    namiste = false;

                Hejbni(vpoli);
            }
        }

        public void Update(KeyboardState keys)
        {
            if (alive)
            {
                if (namiste)
                {
                    predesleX = hracovo.X; predesleY = hracovo.Y;
                    ZpracujZvlastniDlazdice();
                    ZpracujCestu();
                    if ((keys.IsKeyDown(Keys.A) || keys.IsKeyDown(Keys.Left)) && hracovo.X > 0)
                    {
                        xSpeed = -speed; namiste = false;
                        pohybVlevo = true; pohybVpravo = false; pohybDolu = false; pohybNahoru = false;
                    }
                    else if ((keys.IsKeyDown(Keys.D) || keys.IsKeyDown(Keys.Right)) && hracovo.X < maxX)
                    {
                        xSpeed = speed; namiste = false;
                        pohybVpravo = true; pohybVlevo = false; pohybDolu = false; pohybNahoru = false;
                    }
                    else if ((keys.IsKeyDown(Keys.W) || keys.IsKeyDown(Keys.Up)) && hracovo.Y > 0)
                    {
                        ySpeed = -speed; namiste = false;
                        pohybNahoru = true; pohybVlevo = false; pohybVpravo = false; pohybDolu = false;
                    }
                    else if ((keys.IsKeyDown(Keys.S) || keys.IsKeyDown(Keys.Down)) && hracovo.Y < maxY)
                    {
                        ySpeed = speed; namiste = false;
                        pohybDolu = true; pohybVlevo = false; pohybVpravo = false; pohybNahoru = false;
                    }

                    PlayBoard.tiles[IndexDlazdice].OznacitJakoProjetou(true); //projeta = true
                }
                else
                {
                    hracovo.X += xSpeed;
                    hracovo.Y += ySpeed;
                    if (hracovo.X % krok == 0 && hracovo.Y % krok == 0)
                    {
                        namiste = true;
                        xSpeed = 0; ySpeed = 0;
                    }
                }
            }
        }

        private void ZpracujCestu()
        {
            PlayBoard.tiles[IndexDlazdice].OznacitJakoProjetou(true);
            if (!vpoli && !PlayBoard.tiles[IndexDlazdice].Plna && !PlayBoard.tiles[IndexDlazdice].Okrajova) //dostal se na volnou dlazdici
            {
                vpoli = true;
                Vyputoval();
            }
            else if (vpoli && (PlayBoard.tiles[IndexDlazdice].Plna || PlayBoard.tiles[IndexDlazdice].Okrajova)) //navrat do bezpeci
            {
                vpoli = false;
                prepocistSkore = true;
            }
        }

        //private float UrovnejSouradnici(int cislo)
        //{
        //    if (cislo % krok < pulkrok) cislo = cislo/ krok * krok;
        //    else cislo = (cislo / krok + 1) * krok;
        //    return cislo;
        //}

        private void Hejbni(bool masebat)
        {
            if (pohybVlevo && hracovo.X > 0)
            {
                indexPristiDlazdice = IndexDlazdice - 1;
                if (PlayBoard.tiles[indexPristiDlazdice].Pruchodna)
                {
                    hracovo.X -= speed; 
                    souradniceVysledneTextury = doleva;
                }
                else 
                    ZkusObjetSvisle(true);
            }
            else if (pohybVpravo && hracovo.X < maxX)
            {
                indexPristiDlazdice = IndexDlazdice + 1;
                if (PlayBoard.tiles[indexPristiDlazdice].Pruchodna)
                {
                    hracovo.X += speed; 
                    souradniceVysledneTextury = doprava;
                }
                else 
                    ZkusObjetSvisle(false);
            }
            else if (pohybNahoru && hracovo.Y > 0 && hracovo.Y > 0)
            {
                indexPristiDlazdice = IndexDlazdice - sloupcu - 1;
                if (PlayBoard.tiles[indexPristiDlazdice].Pruchodna)
                { 
                    hracovo.Y -= speed; 
                    souradniceVysledneTextury = nahoru;
                }
            }
            else if (pohybDolu && hracovo.Y < maxY)
            {
                indexPristiDlazdice = IndexDlazdice + sloupcu + 1;
                if (PlayBoard.tiles[indexPristiDlazdice].Pruchodna)
                { 
                    hracovo.Y += speed; 
                    souradniceVysledneTextury = dolu;
                }
            }
            else
            {
                if (masebat)
                    souradniceVysledneTextury = strach;
                else 
                    souradniceVysledneTextury = stoji;
            }
        }

        private void ZkusObjetSvisle(bool zleva)
        {
            if (souradnice.Y < hracovo.Y)
            {
                indexPristiDlazdice = IndexDlazdice - sloupcu - 1;
                if (indexPristiDlazdice > 0 && PlayBoard.tiles[indexPristiDlazdice].Pruchodna)
                {
                    pohybNahoru = true;
                    if (zleva) 
                        pohybVlevo = false;
                    else 
                        pohybVpravo = false;

                    hracovo.Y -= speed; 
                    souradniceVysledneTextury = nahoru;
                }
            }
            else if (souradnice.Y > hracovo.Bottom)
            {
                indexPristiDlazdice = IndexDlazdice + sloupcu + 1;
                if (PlayBoard.tiles[indexPristiDlazdice].Pruchodna)
                {
                    pohybDolu = true;
                    if (zleva) 
                        pohybVlevo = false;
                    else 
                        pohybVpravo = false;
                    hracovo.Y += speed; 
                    souradniceVysledneTextury = dolu;
                }
            }
        }

        private void UrciKamJet(Point souradnice)
        {
            if (souradnice.X < hracovo.X)
            {
                pohybVlevo = true; pohybVpravo = false;
            }
            else if (souradnice.X > hracovo.Right)
            {
                pohybVpravo = true; pohybVlevo = false;
            }
            else pohybVlevo = pohybVpravo = false;

            if (souradnice.Y < hracovo.Y)
            {
                pohybNahoru = true; pohybDolu = false; 
            }
            else if (souradnice.Y > hracovo.Bottom)
            {
                pohybDolu = true; pohybNahoru = false; 
            }
            else pohybNahoru = pohybDolu = false;
        }

        private void Vyputoval()
        {
            if (hracovo.X != predesleX) // vodorovny vyjezd
            {
                if (predesleX == hracovo.X - krok)
                {
                    zleva = true;
                    VychoziX = hracovo.X - krok;
                }
                else
                {
                    zprava = true;
                    VychoziX = hracovo.X + krok;
                }

                // svislyVyjezd = false;
                VychoziY = hracovo.Y;
            }
            else if (pohybNahoru || pohybDolu) // svisly
            {
                if (predesleY == hracovo.Y - krok)
                {
                    zhora = true;
                    VychoziY = hracovo.Y - krok;
                }
                else
                {
                    zdola = true;
                    VychoziY = hracovo.Y + krok;
                }

                // svislyVyjezd = true;
                VychoziX = hracovo.X;
            }
        }

        //private int Doputoval() //jen obsah obdelniku, nepouziju
        //{
        //    prepocistSkore = true;
        //    if (VychoziX == hracovo.X)
        //    {
        //        vysledekY = System.Math.Abs((hracovo.Y - VychoziY) / krok);
        //        if (VychoziX < pulsirky)
        //        {
        //            vysledekX = VychoziX / krok;
        //        }
        //        else { vysledekX = (maxX - VychoziX) / krok; }
        //    }
        //    else
        //    {
        //        vysledekX = System.Math.Abs((hracovo.X - VychoziX) / krok);

        //        if (VychoziY == hracovo.Y)
        //        {
        //            if (VychoziY < pulvysky)
        //            {
        //                vysledekY = VychoziY / krok;
        //            }
        //            else
        //            {
        //                vysledekY = (maxY - VychoziY) / krok;
        //            }
        //        }
        //        else
        //        {
        //            vysledekY = System.Math.Abs((hracovo.Y - VychoziY) / krok);
        //        }
        //    }
        //    vysledek = vysledekX * vysledekY;
        //    return vysledek;
        //}

        public void Kresli(SpriteBatch spritebatch)
        {
            //Vector2 topLeftOfSprite = new Vector2(hracovo.X, hracovo.Y);
            //Color tintColor = Color.White;
            //var sourceRectangle = currentAnimation.CurrentRectangle;
            //spritebatch.Draw(characterSheetTexture, topLeftOfSprite, sourceRectangle, Color.White);

            if (!animovan)
            {
                spritebatch.Draw(spriteHracovo, hracovo, souradniceVysledneTextury, Color.White);
            }
            else 
            {
                if (kroku == 0) 
                    Respawn();
                else
                {
                    kroku -= 1;
                    rotace += .12f; //v radianech
                    spritebatch.Draw(spriteHracovo, pozice, souradniceVysledneTextury, Color.Yellow, rotace, stredOtaceni, scale, SpriteEffects.None, 1);
                    if (kroku > celkemKroku / 2) 
                        scale *= 1.01f;
                    else 
                        scale /= 1.04f;
                }
            }
        }

        /// <summary>
        /// Sprite animace, zatím jen smrti
        /// </summary>
        /// <param name="animace"> 0 = smrt </param>
        /// <param name="kroky"> doba animace</param>
        public void NastavAnimaci(byte animace, ushort kroky)
        {
            //namiste = false; //zastavi pohyb mimo desku, ale ne respawn posun
            pozice = new Vector2(hracovo.X + pulkrok, hracovo.Y + pulkrok);
            stredOtaceni = new Vector2(pulkrok, krok);
            animovan = true;
            kroku = celkemKroku = kroky;
            if (animace == 0)
            {
                alive = false;
                souradniceVysledneTextury = mrtvy;
                scale = new Vector2(.8f, .8f);
            }
        }

        internal void Respawn()
        {
            animovan = false;
            vpoli = false; 
            alive = true;
            hracovo.X = krok * 2; hracovo.Y = 0;
            souradnice = new Point(hracovo.X, hracovo.Y);
            souradniceVysledneTextury = stoji;
            Balls.ZrusSrazkuKouli();
        }

        public void NastavPrepocet(bool anone)
        {
            prepocistSkore = anone;
        }

        public void NastavTexturu(Rectangle hracuv)
        {
            souradniceVysledneTextury = hracuv;
        }

        public void ZpracujZvlastniDlazdice()
        {
            IndexDlazdice = hracovo.X / krok + (hracovo.Y / krok * PlayBoard.Sloupcu); // na jake dlazdici je
            if (PlayBoard.tiles[IndexDlazdice].zpomalovaci)
            {
                Balls.NastavRychlostKouli(.6f, true);                
                PlayBoard.tiles[IndexDlazdice].NastavZpomalovac(false);
            }
            else if (PlayBoard.tiles[IndexDlazdice].mina)
            {
                PlayBoard.PripravZemetreseni(IndexDlazdice);
            }
            else if (PlayBoard.tiles[IndexDlazdice].ozivovaci)
            {
                Balls.OzivKouli(IndexDlazdice);
            }
        }

        public void BludisteInput(Point dotek)
        {
            if (alive && namiste)
                UpdateBludiste(dotek);
            else
                UpdateBludiste(Point.Zero);
        }

        public void Input(Point input)
        {
            if (namiste)
            {
                if (!vpoli)
                    Balls.SpawnBalls();

                Update(input); // povoli ovladani hrace
            }
            else
                Update(Point.Zero);
        }

        public bool PrepoctemeSkore()
        {
            if (prepocistSkore)
            {
                prepocistSkore = false;
                return true;
            }

            return false;
        }


        public bool IsAlive()
        {
            return alive;
        }
    }
}