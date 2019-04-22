using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.IO;
using System.IO.IsolatedStorage;

namespace Mojehra
{
    /// <summary>
    /// This is the main type
    /// </summary>
    public class Hlavni : Game
    {
        //       List<Button> tlacitka = new List<Button>();
        //       static Texture2D texturaTlacitka;
        //Color[] barvaTlacitka;
        //private static Android.OS.Vibrator vibrator;
        KeyboardState keys; GamePadState pad; MouseState mouse;

        private static Hrobecek hrob; private static Texture2D hrobSprite;

        private IsolatedStorageFile store;
        public bool debug = false; private bool soft = false;
        private bool easy = true; private bool hard = false;

        SplashScreen splashScreen;

        private byte procentProVitezstvi;

        private float casMilisekund, scaleOpening = 0;
        private bool sudyTik = true;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        const ushort tileSize = 32;
        private const ushort windowWidth = tileSize * 15;
        private const short suggestedHeight = tileSize * 10;// WindowWidth / borderThick * 20; //16:10
        private ushort windowHeight = suggestedHeight / tileSize * tileSize;
        private Rectangle oknoHry;
        //private int suggestedPixels = windowWidth * suggestedHeight;
        //private int numberOfPixels;
        //ResolutionRenderer RendererOfRealResolution;
        private Matrix scaleMatrix = Matrix.Identity;

        private Pozadi pozadi;
        private static Texture2D square64, snow;

        bool sound = true; bool music = true;
        MediaQueue musicFronta = new MediaQueue(); private static Song stara;
        List<string> skladby = new List<string>();

        private bool paused, pauseKeyDown, pauseKeyDownThisFrame, chciPausu, pausedByUser, pristeUzNekreslim;
        Random rand = new Random();

        private Stavy gameState;
        private string staryState;

        private float volumeSound = .5f, volumeHudby = .5f;
        private static Song levelwon, menu, intro;
        private static SoundEffect sezrani;
        private static SoundEffect quake; SoundEffectInstance zemetres;
        private static SoundEffect ton1, odraz, kolize;//SoundEffectInstance rachot;
        private static SoundEffectInstance ton2, ton3, instanceOdrazu;
        public static bool hrajOdraz;

        private static Texture2D openingScreen;
        private static Vector2 stred;

        private static ushort maxDotyku;

        ushort waitFrames = 0; private uint krokIntra; private float trvaniAnimacky;

        //board field
        //Vector2 boardCenter;
        //NumberBoard deska;

        const ushort borderThick = tileSize;
        Rectangle borderV; //Rectangle borderH;
        List<Rectangle> okrajeH = new List<Rectangle>();
        List<Rectangle> okrajeV = new List<Rectangle>();
        private bool zvetsujBorderV;
        private Rectangle borderVanim;
        private static Texture2D texOkrajeH, texOkrajeV;

        private short numBalls, numAttackBalls;
        Vector2 balLoc; static Vector2 ballVelocity;
        static Texture2D ballSprite;
        private Rectangle boxKoule = new Rectangle(0, 0, 30, 30);
        public static List<Rectangle> hitboxyKouli = new List<Rectangle>();
        private static List<Ball> balls = new List<Ball>();
        private static List<Ball> ballsUtocne = new List<Ball>();
        private static List<Ball> ballsAll = new List<Ball>();
        private bool rigid = true;

        private Color[] barvaV, barvaH;
        private Color[] barvaVanim;

        private static SpriteFont font, font14, font20;
        private Vector2 debugTextLocation, velkyTextLocation;
        //Dictionary<Vector2, string> Texty = new Dictionary<Vector2, string>();
        private List<Zprava> Texty = new List<Zprava>();
        private Color barvaZpravy;
        private List<string> debugText = new List<string>();
        private string debugvar1, debugvar2, debugPrvniDlazdice, debugKolize;
        private bool letiZrovnaText; private string leticiText; private ushort snimkuLeticihoTextu; private Vector2 pohybLeticihoTextu, polohaLeticihoTextu;

        private ushort i = 0, plnychDlazdic, okrajovychDlazdic, potrebnychPlnych;
        public static byte zemetreseni; private const byte dobaZemetreseni = 60; private static int vybuchujuciMina; private static bool probihaVybuch;
        private static Texture2D tileSprite, explozeSprite;
        private static Texture2D tileOznacenaSprite, tileOznacena2Sprite, tileDruhaSprite;
        private List<Tile> druheRadyTiles = new List<Tile>();
        public static List<Tile> tiles = new List<Tile>();
        public static List<Tile> tilesVnitrni = new List<Tile>();
        private static List<Tile> tilesMenu = new List<Tile>();
        private static List<Tile> tilesMenuVnitrni = new List<Tile>();
        private static List<Tile> tilesMenuOptions = new List<Tile>();
        private Rectangle menuNew, menuLoad, menuSettings, menuExit, menuSound, menuMusic;
        private Vector2 menuNewLoc, menuContinueLoc, menuSettingsLoc, menuExitLoc, menuSoundLoc, menuMusicLoc, menuBottomLoc;
        private const string menuNewString = "New", menuContinueString = "Resume", menuSettingsString = "Options",
            menuExitString = "Exit", menuSoundString = "Sound", menuMusicString = "Music";
        private string menuBottomString = string.Empty;
        private bool options;
        private Rectangle posuvnikSound, posuvnikMusic;

        private Point tileSound, tileMusic;
        private Point klik, staryklik;
        ushort pocetDlazdic, pocetVnitrnichDlazdic;
        public static ushort columns, rows, columnsVnitrni, rowsVnitrni;
        private bool animovatDlazdici, videtDlazdici = true;

        private float deltaSeconds;
        private short mAlphaValue = 1, mFadeIncrement = 3;
        private double mFadeDelay = .035;

        private float _colorAmount;
        Color prvniBarva = new Color(000, 000, 100, 100);
        Color druhaBarva = new Color(100, 150, 100, 100);
        Color barvaCerna = new Color(000, 000, 100, 100);
        Color druhaFialova = new Color(100, 150, 100, 100);
        Color prvniViteznaBarva = new Color(100, 200, 200, 0);
        Color druhaViteznaBarva = new Color(200, 200, 100, 0);
        Color vyblitaZelena = new Color(22, 100, 22, 127);
        private bool preklop;

        private Hrac player;
        private ushort delkaAnimaceHrace, sloupcuAnimace;
        private const byte pocatecniZivoty = 3; byte zivoty;
        static Texture2D hracsprite, hracMrtvySprite;
        private Vector2 zadanyKlik, predchoziKlik;

        private string zivotuString;
        private string skoreString = "0", skoreTotalString = "0", excesBonus, procentaString;
        private static short skore, skoreTotal, bonus, exces, pricistSkore;
        private ushort minBonus;//uroven pro gratulaci
        private Vector2 zivotuLocation, skoreLocation, skoreTotalLocation, procentaLocation;

        private static Texture2D spriteMonstra;
        Monster monstrum; private List<Monster> monstra = new List<Monster>();

        private Level uroven;
        private static string scoreFilename; private int rekordSkore;
        private static string levelFilename; private byte maxLevel, maxEpisoda;
        private static string volumeFilename;

        internal static void PripravZemetreseni(int indexMiny)
        {
            if (zemetreseni == 0)
            {
                zemetreseni = dobaZemetreseni;
                tiles[indexMiny].Odminovat();
                vybuchujuciMina = indexMiny;
                probihaVybuch = true;
                VybuchKolem(vybuchujuciMina, 1);
            }
        }
        //internal static void VybuchNajednou(int index, byte dosah)
        //{
        //    for (byte i = 1; i <= dosah; i++)
        //    {
        //        VybuchKolem(index, dosah);
        //    }
        //}
        internal static void VybuchPostupne()
        {
            if (zemetreseni == 51 && tiles[vybuchujuciMina].dosahMiny > 1)
            {
                VybuchKolem(vybuchujuciMina, 2);
                //if (vibrator.HasVibrator) vibrator.Vibrate(600);
            }
            else if (zemetreseni == 44 && tiles[vybuchujuciMina].dosahMiny > 2) VybuchKolem(vybuchujuciMina, 3);
            else if (zemetreseni == 37 && tiles[vybuchujuciMina].dosahMiny > 3) VybuchKolem(vybuchujuciMina, 4);
            else if (zemetreseni == 30 && tiles[vybuchujuciMina].dosahMiny > 4) VybuchKolem(vybuchujuciMina, 5);
        }
        /// <summary>
        /// zatim nekontroluju, jestli jde vybuch za okrajove dlazdice
        /// </summary>
        /// <param name="index"></param>
        /// <param name="vzdalenost"></param>
        internal static void VybuchKolem(int index, byte vzdalenost)
        {
            tiles[index - vzdalenost].Zborit(true);
            foreach (Ball ball in ballsAll) if (tiles[index - vzdalenost].drawRectangle.Intersects(ball.rect)) ball.Zasazen();

            tiles[index + vzdalenost].Zborit(true);
            foreach (Ball ball in ballsAll) if (tiles[index + vzdalenost].drawRectangle.Intersects(ball.rect)) ball.Zasazen();

            if (index - columns * vzdalenost > 0)
            {
                tiles[index - columns * vzdalenost].Zborit(true);
                foreach (Ball ball in ballsAll) if (tiles[index - columns * vzdalenost].drawRectangle.Intersects(ball.rect)) ball.Zasazen();
            }

            if (index + columns * vzdalenost < tiles.Count)
            {
                tiles[index + columns * vzdalenost].Zborit(true);
                foreach (Ball ball in ballsAll) if (tiles[index + columns * vzdalenost].drawRectangle.Intersects(ball.rect)) ball.Zasazen();
            }
        }

        public Hlavni()
        {
            IsMouseVisible = true;
            //vibrator = (Android.OS.Vibrator)Activity.GetSystemService(Android.Content.Context.VibratorService);
            graphics = new GraphicsDeviceManager(this);
            oknoHry = new Rectangle(0, 0, windowWidth, windowHeight);
            //if (suggestedHeight % borderThick != 0)
            //{
            //    WindowHeight = (suggestedHeight / borderThick) * borderThick;
            //}
            //else WindowHeight = suggestedHeight;

            Content.RootDirectory = "Content";

            //IsMouseVisible = true;

            if (soft)
            {
                GraphicsAdapter.UseReferenceDevice = true;
                //graphics.PreferredBackBufferWidth = 384;
                //graphics.PreferredBackBufferHeight = 256;
            }
            else
            {//tohle asi na nadroidech nejde
                //graphics.PreferredBackBufferWidth = WindowWidth;
                //graphics.PreferredBackBufferHeight = WindowHeight;
            }
            if (debug)
            {
                debugvar1 = "HardwareModeSwitch: " + graphics.HardwareModeSwitch.ToString();
                debugvar2 = "UseDriverType: " + GraphicsAdapter.UseDriverType.ToString();
                debugPrvniDlazdice = ""; debugKolize = "";
                debugText.Add(debugvar1); debugText.Add(debugvar2); debugText.Add(debugPrvniDlazdice); debugText.Add(debugKolize);
            }
            //default droid
            graphics.IsFullScreen = false;
            //graphics.PreferredBackBufferWidth = 800;
            //graphics.PreferredBackBufferHeight = 480;
        }

        internal static void HrajOdraz()
        {
            if (instanceOdrazu.State == SoundState.Stopped) //stejne prekracuju limit at omezuju jak chci
            {
                instanceOdrazu.Pitch = 0;
                instanceOdrazu.Play();
            }
            else if (instanceOdrazu.Pitch < .99f) instanceOdrazu.Pitch += .03f;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
           // openingRect.Width = graphics.PreferredBackBufferWidth;
            //openingRect.Height = graphics.PreferredBackBufferHeight;

            #if WINDOWS
                        store = IsolatedStorageFile.GetUserStoreForDomain();
            #else
                        store = IsolatedStorageFile.GetUserStoreForApplication();
            #endif

            stred = new Vector2(windowWidth / 2, windowHeight / 2);

            float vodorovnyPomer = graphics.PreferredBackBufferWidth / (float)windowWidth;
            float svislyPomer = graphics.PreferredBackBufferHeight / (float)windowHeight;
            if (vodorovnyPomer > svislyPomer) scaleMatrix = Matrix.CreateScale(svislyPomer, svislyPomer, 1);
            else scaleMatrix = Matrix.CreateScale(vodorovnyPomer, vodorovnyPomer, 1);

            //UI
            zivotuLocation = new Vector2(6, 6);
            skoreLocation = new Vector2(windowWidth / 2, 12);
            skoreTotalLocation = new Vector2(windowWidth - 28, 12);
            procentaLocation = new Vector2(stred.X, windowHeight - tileSize * .9f);
            debugTextLocation = new Vector2(windowWidth / 11, windowHeight / 11);
            velkyTextLocation = stred;

            columns = (windowWidth / tileSize);
            rows = (ushort)(windowHeight / tileSize);
            columnsVnitrni = (ushort)(columns - 2);
            rowsVnitrni = (ushort)(rows - 2);

            //graphics.ApplyChanges(); should be called only during update
            //numberOfPixels = graphics.PreferredBackBufferWidth * graphics.PreferredBackBufferHeight;
            //Tuple<int, int> resolution = GetVirtualResolution(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            //RendererOfRealResolution = new ResolutionRenderer(this, resolution.Item1, resolution.Item2, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            //TouchPanelCapabilities tc = TouchPanel.GetCapabilities();
            //if (tc.IsConnected)
            //{
            //    TouchPanel.EnabledGestures = GestureType.None;
            //    maxDotyku = (ushort)(tc.MaximumTouchCount - 1);
            //}
            //else throw new SystemException("Touchpannel needed");

            scoreFilename = "score.txt";
            levelFilename = "uroven";
            volumeFilename = "volumes";

            if (!LoadVolumes())
            {
                SoundEffect.MasterVolume = volumeSound;
                MediaPlayer.Volume = volumeHudby;
            }

            uroven = new Level(rows, columns);

            zivoty = pocatecniZivoty;
            zivotuString = zivoty.ToString();

            barvaV = new Color[windowHeight * borderThick];
            barvaH = new Color[borderThick * (windowWidth - borderThick * 2)];

            balLoc = new Vector2(222, 222);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            square64 = Content.Load<Texture2D>(@"gfx/square64");
            openingScreen = Content.Load<Texture2D>(@"gfx/openingscreen");
            ballSprite = Content.Load<Texture2D>(@"gfx/ball");
            tileSprite = Content.Load<Texture2D>(@"gfx/tile");
            tileOznacenaSprite = Content.Load<Texture2D>(@"gfx/oznacena");
            //tileOznacena2Sprite = Content.Load<Texture2D>(@"gfx/oznacena2");
            hracsprite = Content.Load<Texture2D>(@"gfx/hrac");
            hracMrtvySprite = Content.Load<Texture2D>(@"gfx/hracMrtvy");
            explozeSprite = Content.Load<Texture2D>(@"gfx/explode");

            if (sound)
            {
                NahrajZvuky(); ton1.Play(.4f, 0, 0);
            }
            if (music)
            {
                NahrajHudbu();
                byte pocetSkladeb = 5; skladby.Add("levelwon"); skladby.Add("menu");
                for (byte i = 0; i <= pocetSkladeb; i++)
                {
                    string skladba = "level" + i.ToString(); skladby.Add(skladba);
                }
            }
            font = Content.Load<SpriteFont>("AARDC");
            font14 = Content.Load<SpriteFont>("PressStart2P14");
            font20 = Content.Load<SpriteFont>("PressStart2P20");

            //splashScreen = new SplashScreen(graphics, new Rectangle(windowWidth, 0, (int)(oknoHry.Width * scaleMatrix.M11), oknoHry.Height), font20);
            splashScreen = new SplashScreen(graphics, new Rectangle(windowWidth, 0, (int)(oknoHry.Width * 1.34), oknoHry.Height), font20);

            texOkrajeH = new Texture2D(graphics.GraphicsDevice, oknoHry.Width - borderThick * 2, borderThick);
            texOkrajeV = new Texture2D(graphics.GraphicsDevice, borderThick, oknoHry.Height);
            // Set the texture data with our color information.
            for (uint i = 0; i < barvaH.Length; ++i) barvaH[i] = vyblitaZelena;
            texOkrajeH.SetData(barvaH);

            Color barvaHOkraje = Color.Green; barvaHOkraje.A = 22;
            for (uint i = 0; i < barvaV.Length; ++i) barvaV[i] = barvaHOkraje;
            texOkrajeV.SetData(barvaV);

            gameState = Mojehra.Stavy.Menu;
            GetScore();
            if (rekordSkore > 0)
                NapisVelkouZpravu($"Your best: {rekordSkore.ToString()}", 8000, -9999, 200, true, true, Color.Yellow);

            BuildMenu(); BuildMenuOptions();
            animovatDlazdici = true;
            BuildTiles(columns, rows, tileSize);//muzu az po assetech
            //PostavPozadi("snow", new Vector2(.1f, 1), true, true);
        }

        private void NahrajHudbu()
        {
            if (gameState == Mojehra.Stavy.Play)
            {
                if (!uroven.bludiste)
                {
                    string skladba = $"level{uroven.cisloUrovne.ToString()}";
                    if (skladby.Contains(skladba))
                    {
                        Song level = Content.Load<Song>(@"audio/" + skladba);
                        MediaPlayer.Play(level); stara = level;
                        MediaPlayer.IsRepeating = true;
                    }
                    else MediaPlayer.Play(menu);
                }
                else
                {
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Play(menu);
                }
            }
            else
            {
                menu = Content.Load<Song>(@"audio/menu");
                levelwon = Content.Load<Song>(@"audio/win");
                MediaPlayer.Play(menu);
                MediaPlayer.IsRepeating = false;
            }
        }

        private void NahrajZvuky()
        {
            ton1 = Content.Load<SoundEffect>(@"audio/ton1");
            ton2 = ton1.CreateInstance(); ton2.Pitch = .6f;
            ton3 = ton1.CreateInstance(); ton3.Volume = 1f; ton3.Pitch = .6f;
            //ton2 = Content.Load<SoundEffect>(@"audio/ton2");
            odraz = Content.Load<SoundEffect>(@"audio/odraz");
            instanceOdrazu = odraz.CreateInstance();
            //winSound = Content.Load<SoundEffect>(@"audio/applause");
            kolize = Content.Load<SoundEffect>(@"audio/lost");
            sezrani = Content.Load<SoundEffect>(@"audio/lost2");
            //zvukKolizeKouli = Content.Load<SoundEffect>(@"audio/explosion");
            //rachot = zvukKolizeKouli.CreateInstance();
            //rachot.Volume = .3f;
            quake = Content.Load<SoundEffect>(@"audio/zemetreseni");
            zemetres = quake.CreateInstance();
        }

        /// UnloadContent will be called once per game and is the place to unload game-specific content.
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            Content.Unload();
        }

        /// zmena okna
        
        protected override void OnActivated(object sender, System.EventArgs args)
        {
            this.Window.Title = "Square it!";
            if (debug) { this.Window.Title = this.Window.Title + gameState; }
            base.OnActivated(sender, args);
        }

        protected override void OnDeactivated(object sender, System.EventArgs args)
        {
            this.Window.Title = "Square Inactive ";
            base.OnActivated(sender, args);
        }

        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (IsActive)//je okno videt?
            {
                if (waitFrames > 0) waitFrames -= 1;
                else
                {
                    keys = Keyboard.GetState(); mouse = Mouse.GetState();
                    pad = GamePad.GetState(PlayerIndex.One);
                    if (pad.Buttons.Back == ButtonState.Pressed || keys.IsKeyDown(Keys.Escape))
                    {
                        if (gameState == Mojehra.Stavy.Menu) Exit();
                        else gameState = Mojehra.Stavy.Menu;
                    }

                    if (splashScreen.kresliSplash) splashScreen.Update();
                    if (splashScreen.provedUpdate)
                    {
                        //TouchCollection doteky = TouchPanel.GetState();
                        Stavy(keys);
                        sudyTik = !sudyTik;

                        if (pozadi != null) pozadi.Update();

                        foreach (Zprava zprava in Texty)
                        {
                            zprava.Update(gameTime.ElapsedGameTime.Milliseconds);
                        }
                        if (probihaVybuch)
                        {
                            VybuchPostupne();
                            if (zemetres.State == SoundState.Stopped) zemetres.Play();
                        }
                        // if playing, update controller state and update board
                        if (gameState == Mojehra.Stavy.Play)
                        {
                            if (!uroven.bludiste)
                            {
                                if (player.namiste)
                                {
                                    if (!player.vpoli) SpawnBalls();
                                    //player.Update(keys); //povoli ovladani hrace
                                    player.UpdateMouse(Dotek(mouse));
                                }
                                else player.UpdateMouse(Point.Zero);
                                //else player.Update(Point.Zero);
                                if (uroven.performanceTest)
                                {
                                    skoreString = gameTime.ElapsedGameTime.Milliseconds.ToString();
                                    if (player.hracovo.X > stred.X) Zvitezit();
                                }
                            }

                            else
                            {
                                if (player.alive && player.namiste) player.UpdateBludiste(Dotek(mouse)); //player.UpdateBludiste(keys);
                                else player.UpdateBludiste(Point.Zero);
                                foreach (Ball ball in balls)
                                {
                                    if (ball.cinna && ball.rect.Intersects(player.hracovo))
                                    {
                                        SmrtKvuliKouli(ball);
                                    }
                                }
                            }
                            if (letiZrovnaText) PosliTextSectiSkore();

                            if (player.alive)
                            {
                                if (player.prepocistSkore)
                                {
                                    TotalSkore();
                                    player.prepocistSkore = false;
                                }
                                else
                                {
                                    if (hrob.ZkontrolujMisto(player.hracovo.Location))
                                    {
                                        if (hrob.obsah > 0)
                                        {
                                            skore += hrob.obsah;
                                            PosliLeticiSkore(hrob.obsah.ToString(), player.hracovo.Location, 60);
                                            ton3.Play();
                                        }
                                        else
                                        {
                                            ton1.Play();
                                        }
                                        hrob.Odstran();
                                    }
                                    foreach (Monster monstrum in monstra)
                                    {
                                        if (monstrum.obdelnik.Intersects(player.hracovo))
                                        {
                                            if (sound) sezrani.Play();
                                            Umri();
                                        }
                                        else monstrum.Update();
                                    }
                                }
                            }

                            #region balls
                            if (delkaAnimaceHrace > 0) delkaAnimaceHrace -= 1;
                            else //žije
                            {
                                foreach (Ball ball in ballsUtocne)
                                {
                                    ball.Update(gameTime.ElapsedGameTime.Milliseconds);
                                }
                                foreach (Ball ball in balls)
                                {
                                    ball.Update(gameTime.ElapsedGameTime.Milliseconds);
                                }
                                //if (rachot.State != SoundState.Playing)
                                {
                                    //if (debug) { int i = balls.Count;int j = hitboxyKouli.Count; if (j != i) { throw new SystemException("blbe pocitam koule"); }  }
                                    if (zivoty > 0)
                                    {
                                        ZjistiNarazDoCesty();
                                    }
                                    //kontrola kolize kouli navzajem
                                    //i -= 1;
                                    //if (i > 1)
                                    //{
                                    //    int j = i;
                                    //    while (j > 1)
                                    //    {
                                    //        if (balls[j].rigidni && balls[j - 1].rigidni &&
                                    //            hitboxyKouli[j].Intersects(hitboxyKouli[j - 1]))
                                    //        {
                                    //            rachot.Play(); break;
                                    //        }
                                    //        j -= 1;
                                    //    }
                                    //}
                                }
                            }
                        }
                        #endregion
                        else if (gameState == Mojehra.Stavy.Animace)
                        {
                            HrajIntro(gameTime);
                        }

                        #region tiles
                        foreach (Tile tile in tiles) //pro animace
                        {
                            tile.Update(gameTime);
                        }
                        #endregion tiles
                        if (hrajOdraz) HrajOdraz();

                        base.Update(gameTime);
                    }
                }
            }
        }

        /// <summary>
        /// Called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //RendererOfRealResolution.Draw(); RendererOfRealResolution.SetupFullViewport();
            //background color
            MenBarvuPozadi(gameTime);
            GraphicsDevice.Clear(Color.Lerp(prvniBarva, druhaBarva, _colorAmount));

            if (gameState == Mojehra.Stavy.Vitez)
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, scaleMatrix);
            else
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, scaleMatrix);

            if (pozadi != null) pozadi.DrawBezRotace(spriteBatch);

            foreach (Rectangle okraj in okrajeV)
            {
                spriteBatch.Draw(texOkrajeV, okraj, Color.White);
            }
            foreach (Rectangle okraj in okrajeH)
            {
                spriteBatch.Draw(texOkrajeH, okraj, Color.White);
            }

            if (gameState == Mojehra.Stavy.Pause)
            {
                if (!options)
                {
                    foreach (Tile tile in tilesMenuVnitrni)
                    {
                        tile.Draw(spriteBatch);
                    }
                }
                else foreach (Tile tile in tilesMenuOptions)
                    {
                        tile.Draw(spriteBatch);
                    }
            }
            else if (gameState == Mojehra.Stavy.Animace)
            {
                foreach (Tile tile in tiles)
                {
                    tile.DrawPlusOkrajove(spriteBatch);
                }
            }
            else //tady se hraje
            {
                if (hrob != null) hrob.Draw(spriteBatch);

                if (zemetreseni == 0)
                {
                    foreach (Tile tile in tiles)
                    {
                        tile.Draw(spriteBatch);
                    }
                }
                else
                {
                    zemetreseni--;
                    if (zemetreseni == 0)
                    {
                        tiles[vybuchujuciMina].NastavSource(new Rectangle(0, 0, 32, 32));
                        probihaVybuch = false;
                    }
                    if (sudyTik)
                    {
                        int random = rand.Next(-4, 4);
                        foreach (Tile tile in tiles)
                        {
                            tile.DrawZemetreseni(spriteBatch, random);
                        }
                    }
                    else if (!uroven.bludiste) ZkontrolujVitezstvi();
                }

                if (gameState == Mojehra.Stavy.Menu)
                {
                    casMilisekund += gameTime.ElapsedGameTime.Milliseconds;
                    float mujcas = casMilisekund / 777;
                    if (scaleOpening <= 1) scaleOpening += .0083f;
                    Vector2 poloha = new Vector2(oknoHry.Center.X + tileSize - _colorAmount * 100, oknoHry.Center.Y);
                    float rotace = _colorAmount * .6f - .2f;
                    float scale = ((float)oknoHry.Height / 69);
                    Vector2 stredOtaceni = new Vector2(32, 32);

                    spriteBatch.Draw(ballSprite, new Vector2(300, oknoHry.Height / 2), new Rectangle(0, 0, 32, 32), new Color(160, 60, 60, 177),
                        mujcas, new Vector2(60, oknoHry.Height / 2), 1.7f, SpriteEffects.None, 1);
                    spriteBatch.Draw(square64, poloha, new Rectangle(0, 0, 64, 64), Color.White,
                        rotace, stredOtaceni, scale, SpriteEffects.None, 1);
                    spriteBatch.Draw(openingScreen, new Vector2(oknoHry.Center.X - scaleOpening * oknoHry.Center.X,
                         oknoHry.Center.Y - scaleOpening / .8f * oknoHry.Center.Y),
                         openingScreen.Bounds, Color.White, 0f, Vector2.Zero, scaleOpening, SpriteEffects.None, 1
                    );
                    if (casMilisekund > 7000 && casMilisekund < 7040) NapisVelkouZpravu14("Ready?", 12000, -9999, -9999, true, true, druhaViteznaBarva);
                }

                //else
                //{
                //deska.Draw(spriteBatch);
                //}

            }
            if (gameState != Mojehra.Stavy.Pause)
            {
                foreach (Ball ball in balls)
                {
                    ball.Draw(spriteBatch, ballSprite);
                }
                foreach (Ball ball in ballsUtocne)
                {
                    ball.Draw(spriteBatch, ballSprite);
                }

                if (player != null)
                {
                    player.Kresli(spriteBatch);
                }

                foreach (Monster monstrum in monstra) monstrum.Draw(spriteBatch);
            }

            #region drawing texts
            foreach (Zprava zprava in Texty) zprava.Draw(spriteBatch);

            if (debug)
            {
                Vector2 poziceRadky = debugTextLocation;
                for (i = 0; i < debugText.Count; i++)
                {
                    if (debugText[i] != null)
                    {
                        string text = debugText[i];
                        spriteBatch.DrawString(font, text, poziceRadky, Color.Cyan);
                        poziceRadky.Y += 22f;
                    }
                }
            }

            if (gameState == Mojehra.Stavy.Play || gameState == Mojehra.Stavy.Vitez || gameState == Mojehra.Stavy.Prohra)
            {
                spriteBatch.DrawString(font, zivotuString, zivotuLocation, Color.Aqua);
                spriteBatch.DrawString(font, skoreString, skoreLocation, Color.Aqua);
                spriteBatch.DrawString(font, skoreTotalString, skoreTotalLocation, Color.Aqua);
                spriteBatch.DrawString(font, procentaString, procentaLocation, Color.White);
            }
            else if (gameState == Mojehra.Stavy.Pause)
            {
                if (!options)
                {
                    spriteBatch.DrawString(font14, menuNewString, menuNewLoc, Color.Azure);
                    spriteBatch.DrawString(font14, menuContinueString, menuContinueLoc, Color.Azure);
                    spriteBatch.DrawString(font14, menuExitString, menuExitLoc, Color.Azure);
                    spriteBatch.DrawString(font14, menuSettingsString, menuSettingsLoc, Color.Azure);
                }
                else
                {
                    spriteBatch.DrawString(font14, menuSoundString, menuSoundLoc, Color.Beige);
                    spriteBatch.DrawString(font14, menuMusicString, menuMusicLoc, Color.Coral);
                }
                spriteBatch.DrawString(font, menuBottomString, menuBottomLoc, Color.White);
            }

            if (letiZrovnaText)
            {
                spriteBatch.DrawString(font, leticiText, polohaLeticihoTextu, Color.Aqua);
            }
            #endregion

            splashScreen.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
            if (debug && chciPausu)
            {
                BeginPause(true); chciPausu = false;
            }
        }

        private void MenBarvuPozadi(GameTime gameTime)
        {
            deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (gameState != Mojehra.Stavy.Vitez) deltaSeconds /= 4;
            if (_colorAmount >= 1.0f)
                preklop = true;
            else if (_colorAmount <= 0 && preklop == true)
                preklop = false;

            if (preklop == false) { _colorAmount += deltaSeconds; }
            else { _colorAmount -= deltaSeconds; }
        }

        private Point Dotek(MouseState mouse)
        {
            predchoziKlik = zadanyKlik;
            if (mouse.Position.ToVector2() != predchoziKlik)
            {
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    //zadanyPohyb = touch.Position/ new Vector2( (float)Math.Truncate(scaleMatrix.M11), (float)Math.Truncate(scaleMatrix.M22) );
                    zadanyKlik.X = (float)Math.Truncate(mouse.Position.X / scaleMatrix.M11);
                    zadanyKlik.Y = (float)Math.Truncate(mouse.Position.Y / scaleMatrix.M22);
                }
            }
            if (zadanyKlik != predchoziKlik) return zadanyKlik.ToPoint();
            else return Point.Zero;
        }

        private void Stavy(KeyboardState keys)
        {
            if (gameState == Mojehra.Stavy.Prohra)
            {
                if (delkaAnimaceHrace == 0)
                {
                    player = null;
                    NapisVelkouZpravu("You lost", 10000);
                    if (keys.IsKeyDown(Keys.Enter))
                    { staryState = gameState.ToString(); gameState = Mojehra.Stavy.Menu; }
                }
                else delkaAnimaceHrace--;
            }
            else if (gameState == Mojehra.Stavy.Vitez)
            {
                if (MediaPlayer.IsRepeating)
                {
                    MediaPlayer.IsRepeating = false;
                    MediaPlayer.Play(levelwon);
                }
                if (borderV.Width > windowWidth / 3)
                { zvetsujBorderV = false; }
                else if (borderV.Width < 33)
                { zvetsujBorderV = true; }
                if (zvetsujBorderV)
                { borderV.Offset(1, 0); }
                else { borderV.Width -= 1; }
                if (letiZrovnaText) PosliTextSectiSkore();
                else
                {
                    if (skore > 0)
                    {
                        if (waitFrames % 4 == 0)
                        {
                            skore--; skoreString = skore.ToString();
                            skoreTotal++; skoreTotalString = skoreTotal.ToString();
                        }
                    }
                    else if (keys.IsKeyDown(Keys.Enter))
                    {
                        uroven.ZvedniUroven();
                        PustUroven();
                    }
                }
            }
            else if (gameState == Mojehra.Stavy.Pause)
            {
                if (!pristeUzNekreslim)
                {
                    //NapisVelkouZpravu("Back Button = Pause" + Environment.NewLine + Environment.NewLine + "Press again for menu", 100); neukaze se napred
                    pristeUzNekreslim = true;
                    SuppressDraw();  //melo by snizit zatizeni
                }
                else
                {
                    menuBottomString = string.Empty;
                    klik = Dotek(mouse);
                    if (klik != Point.Zero && klik != staryklik)
                    {
                        staryklik = klik;
                        if (!options)
                        {
                            if (menuLoad.Contains(klik.X, klik.Y))
                            {
                                if (staryState != Mojehra.Stavy.Menu.ToString())
                                {
                                    LoadGame();
                                    if (maxEpisoda + maxLevel > 0)
                                    {
                                        skoreTotalString = skoreTotal.ToString();
                                        uroven.NastavLevel(maxLevel);
                                        uroven.NastavEpisodu(maxEpisoda);
                                        PustUroven();
                                    }
                                    else
                                    {
                                        menuBottomString = "No Valid Save";
                                        gameState = Mojehra.Stavy.Play;
                                        PustUroven();
                                    }
                                }
                                else
                                {
                                    NapisVelkouZpravu("Too soon", 1000);
                                    pristeUzNekreslim = false;
                                }
                            }
                            else if (menuExit.Contains(klik.X, klik.Y))
                            {
                                SaveScore(); SaveGame(); SaveVolumes();
                                Exit();
                                if (staryState == Mojehra.Stavy.Play.ToString()) gameState = Mojehra.Stavy.Play;
                                else if (staryState == Mojehra.Stavy.Vitez.ToString()) gameState = Mojehra.Stavy.Vitez;
                                else gameState = Mojehra.Stavy.Animace;
                                //System.Environment.Exit(0);
                            }
                            else if (menuNew.Contains(klik.X, klik.Y))
                            {
                                uroven.NastavLevel(0); uroven.NastavEpisodu(1);
                                skoreTotal = 0; skoreTotalString = skoreTotal.ToString();
                                zivoty = pocatecniZivoty;
                                PustUroven();
                            }
                            else if (menuSettings.Contains(klik.X, klik.Y))
                            {
                                options = true;
                                pristeUzNekreslim = false;
                            }
                        }
                        else
                        {
                            if (posuvnikSound.Contains(klik.X, klik.Y))
                            {
                                foreach (Tile tile in tilesMenuOptions)
                                {
                                    if (tile.cilova && tile.drawRectangle.Y == posuvnikSound.Y) tile.OznacJakoCilovou(false);
                                    if (tile.drawRectangle.Contains(klik))
                                    {
                                        tile.OznacJakoCilovou(true);
                                        volumeSound = (float)(tile.drawRectangle.X / tileSize - 2) / 10;
                                    }
                                }
                                SoundEffect.MasterVolume = volumeSound;
                                ton1.Play();
                                pristeUzNekreslim = false;
                            }
                            else if (posuvnikMusic.Contains(klik.X, klik.Y))
                            {
                                foreach (Tile tile in tilesMenuOptions)
                                {
                                    if (tile.cilova && tile.drawRectangle.Y == posuvnikMusic.Y) tile.OznacJakoCilovou(false);
                                    if (tile.drawRectangle.Contains(klik))
                                    {
                                        tile.OznacJakoCilovou(true);
                                        volumeHudby = (float)(tile.drawRectangle.X / tileSize - 2) / 10;
                                    }
                                }
                                MediaPlayer.Volume = volumeHudby;
                                MediaPlayer.Play(levelwon);
                                pristeUzNekreslim = false;
                            }
                        }
                    }
                }
            }
            else if (gameState == Mojehra.Stavy.Menu)
            {
                if (debug) debugText[0] = "jsi v menu!";
                if (keys.IsKeyDown(Keys.Enter) || mouse.LeftButton == ButtonState.Pressed)
                {
                    if (staryState == "Play")
                    {
                        staryState = gameState.ToString();
                        gameState = Mojehra.Stavy.Play;
                    }
                    else if (staryState == "Prohra")
                    {
                        zivoty = pocatecniZivoty;
                        PustUroven();
                    }
                    else
                    {
                        //PustUroven();//prvni spuštění
                        Intro();
                    }
                }
            }
            else //hraje intro
            {
                if (mouse.LeftButton == ButtonState.Pressed || keys.IsKeyDown(Keys.Enter)) trvaniAnimacky -= 1;
            }
        }

        private void Intro()
        {
            Texty.Clear();
            waitFrames = 1; animovatDlazdici = false; videtDlazdici = false;
            barvaVanim = new Color[oknoHry.Height * borderThick * 2];
            texOkrajeV = new Texture2D(graphics.GraphicsDevice, borderThick * 2, oknoHry.Height);
            borderVanim = new Rectangle(oknoHry.Width, 0, borderThick * 2, oknoHry.Height);
            okrajeV.Add(borderVanim);
            // Set the texture data with our color information.
            for (int i = 0; i < barvaVanim.Length; ++i) barvaVanim[i] = Color.Green;
            texOkrajeV.SetData(barvaVanim);
            player = new Hrac(true, 4, tileSize, -tileSize * 10, rows * tileSize / 2 - tileSize / 2, windowWidth, windowHeight,
                            hracsprite, hracMrtvySprite);
            player.NastavTexturu(new Rectangle(160, 0, 32, 32));
            sloupcuAnimace = (ushort)(columns + 1);
            BuildTiles(sloupcuAnimace, rows, tileSize);
            foreach (Tile tile in tiles)
            {
                if (tile.drawRectangle.Y == 0 || tile.drawRectangle.Y == (rows * tileSize) - tileSize)
                {
                    tile.VyplnitZvyditelnitOkamzite();
                }
                else if (tile.drawRectangle.Y == tileSize || tile.drawRectangle.Y == (rows * tileSize) - tileSize * 2)
                {
                    druheRadyTiles.Add(tile);
                }
            }
            numBalls = 2;
            ballVelocity = new Vector2(4.7f, 3f);
            SpawnBalls();
            ballVelocity = new Vector2(4.7f, -3f);
            SpawnBalls();
            int j = 0;
            foreach (Ball ball in balls)
            {
                ball.PovolVariace(false); ball.NastavRychlost(.01f);
                ball.NastavPolohu(new Vector2(-tileSize * (9 + j), stred.Y - tileSize / 2));
                j++;
            }
            gameState = Mojehra.Stavy.Animace;
            trvaniAnimacky = 21.2f;
            //MediaPlayer.Pause();
            prvniBarva = new Color(100, 0, 100, 0);
            druhaBarva = new Color(0, 24, 0, 0);
            MediaPlayer.Play(intro = Content.Load<Song>(@"audio/intro"));
        }

        private void HrajIntro(GameTime gameTime)
        {
            short posun = 2; int cyklus = tileSize / posun;
            short cilovyXokraje = windowWidth - tileSize * 2;

            if (trvaniAnimacky > 0)
            {
                trvaniAnimacky -= (float)gameTime.ElapsedGameTime.Milliseconds / 1000;

                if (borderVanim.X != cilovyXokraje)
                {
                    if (krokIntra < cyklus)
                    {
                        foreach (Tile tile in tiles)
                        {
                            tile.drawRectangle.X -= 2;
                        }
                        krokIntra++;
                    }
                    else
                    {
                        foreach (Tile tile in tiles)
                        {
                            tile.drawRectangle.X += tileSize;
                        }
                        krokIntra = 0;
                    }
                }
                if (player.hracovo.X < windowWidth - tileSize * 1.5) player.hracovo.X += 1;

                foreach (Ball ball in balls)
                {
                    if (ball.rect.Right == cilovyXokraje) ton1.Play();
                    ball.UpdateAnimace(gameTime.ElapsedGameTime.Milliseconds,
                        (short)(oknoHry.Width - tileSize * 3), (short)(oknoHry.Height - tileSize), -9999, (short)tileSize);
                }

                if (trvaniAnimacky < 7)
                {
                    if (borderVanim.X > cilovyXokraje)
                        borderVanim.X -= 2;
                    okrajeV.Clear(); okrajeV.Add(borderVanim);
                }

                if (trvaniAnimacky < 19 && trvaniAnimacky > 18.9) NapisVelkouZpravu("Running away", 1000, (tileSize * 2), tileSize * 2, true, true);
                else if (trvaniAnimacky < 18 && trvaniAnimacky > 17.93) NapisVelkouZpravu("Running away .", 1000, (tileSize * 2), tileSize * 2);
                else if (trvaniAnimacky < 17 && trvaniAnimacky > 16.93) NapisVelkouZpravu("Running away ..", 1000, (tileSize * 2), tileSize * 2);
                else if (trvaniAnimacky < 16 && trvaniAnimacky > 15.93) NapisVelkouZpravu("Running away ...", 1900, (tileSize * 2), tileSize * 2);
                else if (trvaniAnimacky < 14 && trvaniAnimacky > 13.93) NapisVelkouZpravu("for so long ...", 3900, tileSize * 4, (short)((rows - 3) * tileSize));
                else if (trvaniAnimacky < 10 && trvaniAnimacky > 9.93) NapisVelkouZpravu("either breaks you...", 3900, tileSize * 6, tileSize * 3);
                else if (trvaniAnimacky < 5 && trvaniAnimacky > 4.93)
                {
                    NapisVelkouZpravu("or helps you stand", 4000, (short)((sloupcuAnimace - 8) * tileSize), (short)((rows - 3) * tileSize));
                    player.NastavTexturu(new Rectangle(0, 0, tileSize, tileSize));
                }
                else if (trvaniAnimacky < 0 && !splashScreen.kresliSplash) splashScreen.ZatemniSplash(true);
            }
            else
            {
                if (MediaPlayer.State == MediaState.Stopped || trvaniAnimacky < -1f)
                {
                    gameState = Mojehra.Stavy.Play;
                    PustUroven();
                }
            }
        }

        internal static void ZrusSrazkuKouli()
        {
            foreach (Ball ball in balls)
            {
                ball.ZrusSrazku();
            }
        }

        internal static void NastavRychlostKouli(float nasobic)
        {
            foreach (Ball ball in balls)
            {
                ball.NastavRychlost(ball.faktorRychlosti * nasobic);
                Zprava nova = new Zprava(stred, "Speed Altered", Color.Red, 4444, true, true, font);
            }
        }

        private void ZjistiNarazDoCesty()
        {
            foreach (Tile dlazdice in tilesVnitrni)
            {
                if (!dlazdice.okrajova && !dlazdice.plna && dlazdice.projeta)
                {
                    foreach (Ball ball in balls)
                    {
                        if (ball.cinna && ball.rect.Intersects(dlazdice.drawRectangle))
                        {
                            SmrtKvuliKouli(ball);
                            return;
                        }
                    }
                    foreach (Ball ball in ballsUtocne)
                    {
                        if (ball.cinna && ball.rect.Intersects(dlazdice.drawRectangle))
                        {
                            SmrtKvuliKouli(ball);
                            return;
                        }
                    }
                }
            }
        }

        private void ModulujAlfu(double vteriny)
        {
            //Decrement the delay by seconds elapsed since the last Update call
            mFadeDelay -= vteriny;
            if (mFadeDelay <= 0)
            {   //time to fade in/out a bit more.
                mFadeDelay = .35;             //Reset the Fade delay
                mAlphaValue += mFadeIncrement;
                if (mAlphaValue >= 255 || mAlphaValue <= 0)
                { //preklop
                    mFadeIncrement *= -1;
                }
            }
        }

        private void NastavBarvy(Color prvni, Color druha)
        {
            prvniBarva = prvni; druhaBarva = druha;
        }

        private void SpawnBalls(int X = -1, int Y = -1)
        {
            if (balls.Count < numBalls)
            {
                if (X == -1)
                {
                    balLoc.X = rand.Next(64, oknoHry.Width - 64);
                    //boxKoule.X = MathHelper.Clamp((int)balLoc.X, ballSprite.Width * 2, openingRect.Width - ballSprite.Width);
                }
                else balLoc.X = X;
                if (Y == -1)
                {
                    balLoc.Y = rand.Next(64, oknoHry.Height - 64);
                    //boxKoule.Y = MathHelper.Clamp((int)balLoc.Y, ballSprite.Width * 2, openingRect.Height - ballSprite.Width);
                }
                else balLoc.Y = Y;
                balls.Add(new Ball(balLoc, ballVelocity, oknoHry.Width, oknoHry.Height, (byte)tileSize,
                    rigid, false, false, false, false, uroven.bludiste, kolize));
            }
        }
        private void SpawnBallsUtocne(int X = -1, int Y = -1)
        {
            if (ballsUtocne.Count < numAttackBalls)
            {
                if (X == -1)
                {
                    balLoc.X = rand.Next(64, oknoHry.Width - 64);
                    //boxKoule.X = MathHelper.Clamp((int)balLoc.X, ballSprite.Width * 2, openingRect.Width - ballSprite.Width);
                }
                else balLoc.X = X;
                if (Y == -1)
                {
                    balLoc.Y = rand.Next(64, oknoHry.Height - 64);
                    //boxKoule.Y = MathHelper.Clamp((int)balLoc.Y, ballSprite.Width * 2, openingRect.Height - ballSprite.Width);
                }
                else balLoc.Y = Y;

                byte levych = byte.MinValue; byte pravych = byte.MinValue; byte dolnich = byte.MinValue; byte hornich = byte.MinValue;
                foreach (Ball ball in ballsUtocne)
                {
                    if (ball.utocnaDolni) dolnich++;
                    else if (ball.utocnaHorni) hornich++;
                    else if (ball.utocnaLeva) levych++;
                    else pravych++;
                }

                if (uroven.numUtocnychBallsLeft > levych)
                {
                    SpawnBallLeft();
                }
                else if (uroven.numUtocnychBallsRight > pravych)
                {
                    SpawnBallRight();
                }
                else if (uroven.numUtocnychBallsUp > hornich)
                {
                    SpawnBallUp();
                }
                else if (uroven.numUtocnychBallsDown > dolnich)
                {
                    SpawnBallDown();
                }
                else SpawnRandomAttackBall();
            }
        }
        private void SpawnRandomAttackBall()
        {
            bool leva = false; bool prava = false; bool nahoru = false; bool dolu = false;
            while (!leva && !prava && !nahoru && !dolu)
            {
                int i = rand.Next(0, 4);
                {
                    if (i == 0) leva = rand.NextDouble() >= 0.5;
                    else if (i == 1) prava = rand.NextDouble() >= 0.5;
                    else if (i == 2) nahoru = rand.NextDouble() >= 0.5;
                    else dolu = rand.NextDouble() >= 0.5;
                }
            }
            ballsUtocne.Add(new Ball(balLoc, ballVelocity, oknoHry.Width, oknoHry.Height, (byte)tileSize,
                    rigid, leva, prava, nahoru, dolu, uroven.bludiste, kolize));
        }
        private void SpawnBallDown()
        {
            byte nalezena = 0;
            foreach (Ball ball in ballsUtocne)
            {
                if (ball.utocnaDolni) nalezena++;
            }
            if (nalezena != uroven.numUtocnychBallsDown)
            {
                ballsUtocne.Add(new Ball(balLoc, ballVelocity, oknoHry.Width, oknoHry.Height, (byte)tileSize,
                    rigid, false, false, false, true, uroven.bludiste, kolize));
            }
        }
        private void SpawnBallUp()
        {
            byte nalezena = 0;
            foreach (Ball ball in ballsUtocne)
            {
                if (ball.utocnaHorni) nalezena++;
            }
            if (nalezena != uroven.numUtocnychBallsUp)
            {
                ballsUtocne.Add(new Ball(balLoc, ballVelocity, oknoHry.Width, oknoHry.Height, (byte)tileSize,
                    rigid, false, true, false, false, uroven.bludiste, kolize));
            }
        }
        private void SpawnBallRight()
        {
            byte nalezena = 0;
            foreach (Ball ball in ballsUtocne)
            {
                if (ball.utocnaPrava) nalezena++;
            }
            if (nalezena != uroven.numUtocnychBallsRight)
            {
                ballsUtocne.Add(new Ball(balLoc, ballVelocity, oknoHry.Width, oknoHry.Height, (byte)tileSize,
                    rigid, false, false, true, false, uroven.bludiste, kolize));
            }
        }
        private void SpawnBallLeft()
        {
            byte nalezena = 0;
            foreach (Ball ball in ballsUtocne)
            {
                if (ball.utocnaLeva) nalezena++;
            }
            if (nalezena < uroven.numUtocnychBallsLeft)
            {
                ballsUtocne.Add(new Ball(balLoc, ballVelocity, oknoHry.Width, oknoHry.Height, (byte)tileSize,
                    rigid, true, false, false, false, uroven.bludiste, kolize));
            }
        }

        private void SmrtKvuliKouli(Ball ball)
        {
            Umri();
            ball.SrazkasProjetou();
        }

        private void Umri()
        {
            zivoty -= 1; zivotuString = zivoty.ToString();
            delkaAnimaceHrace = 50;
            player.NastavAnimaci(byte.MinValue, delkaAnimaceHrace);
            if (zivoty == 0)
            {
                skore = 0; skoreString = skore.ToString();
                gameState = Mojehra.Stavy.Prohra;
                //if (vibrator.HasVibrator) vibrator.Vibrate(2);
            }
            else
            {
                OdznacProjete();
                //if (vibrator.HasVibrator) vibrator.Vibrate(2);
                if (uroven.zrodMonstrum) foreach (Monster monstrum in monstra) monstrum.Respawn();
                hrob.Nastav(tiles[player.indexDlazdice].drawRectangle, skore);
                skore = pricistSkore = 0; skoreString = "0";
            }
        }

        private void OdznacProjete()
        {
            foreach (Tile tile in tiles)
            {
                if (tile.projeta) tile.projeta = false;
            }
        }

        protected void BuildTiles(ushort columns, ushort rows, ushort Tilesize)
        {
            tiles.Clear(); tilesVnitrni.Clear();
            Vector2 velocity = new Vector2(0, 0);
            okrajovychDlazdic = 0; int pravyokraj = windowWidth - Tilesize; int dolniokraj = windowHeight - Tilesize;
            for (ushort i = 0; i < rows; i++)
            {
                for (byte j = 0; j < columns; j++)
                {
                    Vector2 location = new Vector2(j * tileSize, i * tileSize);
                    bool naokraji = false;
                    if ((location.X == 0 || location.X == pravyokraj) || (location.Y == 0 || location.Y == dolniokraj))
                    {
                        naokraji = true;
                        okrajovychDlazdic++;
                    }
                    Tile tile = new Tile(tileSprite, explozeSprite, tileOznacenaSprite, tileOznacena2Sprite, tileDruhaSprite,
                        location, velocity, tileSize, tileSize, animovatDlazdici, videtDlazdici, naokraji, debug);
                    tiles.Add(tile);
                    if (!naokraji)
                    {
                        tilesVnitrni.Add(tile);
                        tile.SudaNeboLicha(i * j);
                    }
                }
            }
            if (debug) procentaString = SectiPlneDlazdice() + " / " + (tiles.Count - okrajovychDlazdic).ToString();
            else procentaString = $"There are {pocetVnitrnichDlazdic} tiles to fill";
            pocetDlazdic = (ushort)tiles.Count;
            pocetVnitrnichDlazdic = (ushort)tilesVnitrni.Count;
        }

        protected void BuildMenu()
        {
            menuBottomLoc = procentaLocation;
            Vector2 velocity = new Vector2(0, 0);
            okrajovychDlazdic = 0; int pravyokraj = windowWidth - tileSize; int dolniokraj = windowHeight - tileSize;
            for (byte i = 0; i < rows; i++)
            {
                for (ushort j = 0; j < columns; j++)
                {
                    Vector2 location = new Vector2(j * tileSize, i * tileSize);
                    bool naokraji = false;
                    if ((location.X == 0 || location.X == pravyokraj) || (location.Y == 0 || location.Y == dolniokraj))
                    {
                        naokraji = true;
                        okrajovychDlazdic++;
                    }
                    Tile tile = new Tile(tileSprite, explozeSprite, tileOznacenaSprite, tileOznacena2Sprite, tileDruhaSprite,
                        location, velocity, tileSize, tileSize, animovatDlazdici, videtDlazdici, naokraji, debug);
                    tilesMenu.Add(tile);
                    if (!naokraji)
                    {
                        tilesMenuVnitrni.Add(tile);
                        tile.SudaNeboLicha(i);
                    }
                }
            }

            if (debug)
                procentaString = SectiPlneDlazdice() + " / " + (tiles.Count - okrajovychDlazdic).ToString();
            else
                procentaString = $"There are {pocetVnitrnichDlazdic} tiles to fill";

            for (byte i = 0; i < rowsVnitrni; i++)
            {
                for (byte j = 0; j < columnsVnitrni; j++)
                {
                    if (i == 0 || i == 3 || i == 4 || i == 7) tilesMenuVnitrni[i * columnsVnitrni + j].VyplnitPredemZvyditelnit();
                    if (j == 0 || j == 6 || j == 12) tilesMenuVnitrni[i * columnsVnitrni + j].VyplnitPredemZvyditelnit();
                }
            }

            menuNew = new Rectangle(64, 64, 160, 64);
            Vector2 measured = font14.MeasureString(menuNewString);
            menuNewLoc = new Vector2((menuNew.X + (menuNew.Width - measured.X) / 2), menuNew.Y + ((menuNew.Height - measured.Y) / 2));

            menuLoad = new Rectangle(256, 64, 160, 64);
            measured = font14.MeasureString(menuContinueString);
            menuContinueLoc = new Vector2((menuLoad.X + (menuLoad.Width - measured.X) / 2), menuLoad.Y + ((menuLoad.Height - measured.Y) / 2));

            menuSettings = new Rectangle(64, 192, 160, 64);
            measured = font14.MeasureString(menuSettingsString);
            menuSettingsLoc = new Vector2((menuSettings.X + (menuSettings.Width - measured.X) / 2), menuSettings.Y + ((menuSettings.Height - measured.Y) / 2));

            menuExit = new Rectangle(256, 192, 160, 64);
            measured = font14.MeasureString(menuExitString);
            menuExitLoc = new Vector2((menuExit.X + (menuExit.Width - measured.X) / 2), menuExit.Y + ((menuExit.Height - measured.Y) / 2));
        }

        protected void BuildMenuOptions()
        {
            Vector2 velocity = new Vector2(0, 0);
            int pravyokraj = oknoHry.Width - tileSize; int dolniokraj = oknoHry.Height - tileSize;
            for (byte i = 0; i < rows; i++)
            {
                for (ushort j = 0; j < columns; j++)
                {
                    Vector2 location = new Vector2(j * tileSize, i * tileSize);
                    bool naokraji = false;
                    if ((location.X == 0 || location.X == pravyokraj) || (location.Y == 0 || location.Y == dolniokraj))
                    {
                        naokraji = true;
                    }
                    Tile tile = new Tile(tileSprite, null, tileOznacenaSprite, tileOznacena2Sprite, tileDruhaSprite,
                        location, velocity, tileSize, tileSize, animovatDlazdici, videtDlazdici, naokraji, debug);
                    if (!naokraji)
                    {
                        tilesMenuOptions.Add(tile);
                    }
                }
            }

            for (byte i = 0; i < rowsVnitrni; i++)
            {
                for (ushort j = 0; j < columnsVnitrni; j++)
                {
                    if (i == 1 || i == 3 || i == 5 || i == 7 || i == 9) tilesMenuOptions[i * columnsVnitrni + j].VyplnitPredemZvyditelnit();
                    else if (j == 0 || j == 1 || j == 11 || j == 12)
                    {
                        tilesMenuOptions[i * columnsVnitrni + j].VyplnitZvyditelnitOkamzite();
                        if (i == 2 && j == 1) tilesMenuOptions[i * columnsVnitrni + j].Znepruchodnit();
                        else if (i == 6 && j == 1) tilesMenuOptions[i * columnsVnitrni + j].Znepruchodnit();
                        else if (i == 2 && j == 11) tilesMenuOptions[i * columnsVnitrni + j].Znepruchodnit();
                        else if (i == 6 && j == 11) tilesMenuOptions[i * columnsVnitrni + j].Znepruchodnit();
                    }
                }
            }

            menuSound = new Rectangle(64, 32, 352, 32);
            Vector2 measured = font14.MeasureString(menuSoundString);
            menuSoundLoc = new Vector2((menuSound.X + (menuSound.Width - measured.X) / 2), menuSound.Y + ((menuSound.Height - measured.Y) / 2));

            menuMusic = new Rectangle(64, 160, 352, 32);
            measured = font14.MeasureString(menuMusicString);
            menuMusicLoc = new Vector2((menuMusic.X + (menuMusic.Width - measured.X) / 2), menuMusic.Y + ((menuMusic.Height - measured.Y) / 2));

            posuvnikSound = new Rectangle(menuSound.X, menuSound.Y + 64, tileSize * 11, tileSize);
            posuvnikMusic = new Rectangle(menuMusic.X, menuMusic.Y + 64, tileSize * 11, tileSize);
            tileSound = new Point(menuSound.X + (int)(volumeSound * 10) * tileSize, menuSound.Y + 64);
            tileMusic = new Point(menuMusic.X + (int)(volumeSound * 10) * tileSize, menuMusic.Y + 64);
            foreach (Tile tile in tilesMenuOptions)
            {
                if (tile.drawRectangle.Location == tileSound)
                {
                    tile.OznacJakoCilovou(true);
                }
                else if (tile.drawRectangle.Location == tileMusic)
                {
                    tile.OznacJakoCilovou(true);
                }
            }
        }

        private void TotalSkore()
        {
            if (debug)
            {//odeberu zvlastni textury predchoziho vyplneni
                foreach (Tile tile in tilesVnitrni)
                {
                    if (tile.prvni || tile.druha)
                        tile.DebugDlazdice(0);
                }
            }

            if (!uroven.bludiste)
            {
                if (VyplnPrvniPole())
                {
                    if (ZjistiKoliziPoleKoule())
                    {
                        if (NajdiDruhouDlazdici())
                        {
                            OdznacPrvniPole();
                            ZpracujDruhePole();
                        }
                        else OdznacPrvniPole();
                    }
                }
                else if (NajdiDruhouDlazdici()) ZpracujDruhePole();

                if (letiZrovnaText)
                { //zrusim stary let a sectu jeho hodnotu hned, abych nekazil novou
                    snimkuLeticihoTextu = 0;
                    PosliTextSectiSkore();
                }
                pricistSkore = VyznacCestuVycistiSpocti();
                OdznacProjete();
                if (BlahoprejSkore(pricistSkore))
                    PosliLeticiSkore($"{pricistSkore} + {bonus}", player.hracovo.Location, 60);
                else
                    PosliLeticiSkore(pricistSkore.ToString(), player.hracovo.Location, 60);

                ZkontrolujVitezstvi();
                //if (debug) chciPausu = true;
            }
            else
            {
                skoreTotal = +100;
                gameState = Mojehra.Stavy.Vitez;
                ZastavKoule(); ZastavAgresivniKoule();
                if (uroven.epizoda == 2) NapisVelkouZpravu("End of beta", short.MaxValue, -9999, 50, false, false, Color.GreenYellow);
                NapisVelkouZpravu("What did you find?", 10000);
                procentaString = " bonus:" + skoreTotal;
                waitFrames += 30;
            }
        }

        private void ZkontrolujVitezstvi()
        {
            plnychDlazdic = SectiPlneDlazdice();
            if ((float)plnychDlazdic / pocetVnitrnichDlazdic * 100 > procentProVitezstvi)
            {
                Zvitezit();
            }
            else
            {
                //procentaString = plnychDlazdic + " / " + pocetVnitrnichDlazdic; 
                int zbyva = potrebnychPlnych - plnychDlazdic;
                if (zbyva > potrebnychPlnych / 8)
                    procentaString = $"To do: {zbyva}";
                else
                    procentaString = $"Only {zbyva} more!";
            }
        }

        private void Zvitezit()
        {
            mFadeIncrement *= 4; //NastavBarvy(prvniViteznaBarva, druhaViteznaBarva);
            gameState = Mojehra.Stavy.Vitez;
            ZastavKoule(); ZastavAgresivniKoule();
            short delayedBonus = ZpetnePlneni();
            if (delayedBonus > 0)
                NapisVelkouZpravu($"You Win{Environment.NewLine}Delayed Bonus: {delayedBonus}", 10000, -9999, -9999, true, true, Color.Aqua);
            else
                NapisVelkouZpravu("You Win", 10000, -9999, -9999, true, true, Color.Aqua);
            exces = (short)(plnychDlazdic - potrebnychPlnych);
            if (exces > 0) excesBonus = "Excess Bonus: " + exces;
            else excesBonus = string.Empty;
            procentaString = "Win!   " + excesBonus;
            skoreTotal += exces;
            //waitFrames = 60;
        }

        private void PosliLeticiSkore(string skore, Point vychozi, ushort snimku)
        {
            polohaLeticihoTextu = vychozi.ToVector2();
            pohybLeticihoTextu = (skoreLocation - polohaLeticihoTextu) / snimku;
            leticiText = skore;
            snimkuLeticihoTextu = snimku;
            letiZrovnaText = true;
        }

        private void PosliTextSectiSkore()
        {
            if (snimkuLeticihoTextu > 0)
            {
                polohaLeticihoTextu += pohybLeticihoTextu;
                snimkuLeticihoTextu--;
            }
            else
            {
                pricistSkore += bonus;
                skore += pricistSkore;
                skoreString = skore.ToString();
                letiZrovnaText = false;
            }
        }

        private void ZastavKoule()
        {
            foreach (Ball koule in balls)
            {
                koule.NastavRychlost(0);
            }
        }
        private void ZastavAgresivniKoule()
        {
            foreach (Ball koule in ballsUtocne)
            {
                koule.NastavRychlost(0);
            }
        }

        private short ZpetnePlneni()
        {
            short bonus = 0;
            while (NajdiOznacPrazdnou())
            {
                if (VyplnPrvniPole()) //oznaci kvyplneni
                {
                    if (!ZjistiKoliziPoleKoule())
                    {
                        foreach (Tile dlazdice in tilesVnitrni)
                        {
                            if (dlazdice.kvyplneni)
                            {
                                dlazdice.VyplnitZvyditelnit();
                                bonus += 1;
                            }
                        }
                    }
                    else
                    {
                        foreach (Tile dlazdice in tilesVnitrni)
                        {
                            if (dlazdice.kvyplneni)
                            {
                                dlazdice.plna = false; dlazdice.Zneviditelnit();
                            }
                        }
                    }
                }
            }
            return bonus;
        }

        private bool NajdiOznacPrazdnou()
        {
            bool nalezena = false;
            foreach (Tile tile in tilesVnitrni)
            {
                if (!tile.plna && !tile.kvyplneni)
                {
                    tile.kvyplneni = true;
                    tiles[i].prvni = true;
                    nalezena = true;
                }
            }
            return nalezena;
        }

        private bool VyplnPrvniPole()
        {
            if (NajdiPrvniDlazdici())
            {
                VyplnPoleProOznaceni();
                return true;
            }
            else return false; //{ throw new System.ArgumentException("dlazdice nenalezena"); }
        }

        private void VyplnPoleProOznaceni()
        {
            for (int index = pocetDlazdic - columns; index > columns; index--)
            {
                if (tiles[index].kvyplneni)
                {   //potrebuju se pustit do vsech stran
                    int indexLevy = index - 1;
                    int indexPravy = index + 1;
                    int indexHorni = index - columns;
                    int indexDolni = index + columns;
                    if (!tiles[indexLevy].plna && !tiles[indexLevy].projeta && !tiles[indexLevy].okrajova)
                    {
                        VyplnDoleva(index - 1);
                    }
                    if (!tiles[indexPravy].plna && !tiles[indexPravy].projeta && !tiles[indexPravy].okrajova)
                    {
                        VyplnDoprava(index + 1);
                    }
                    if (!tiles[indexHorni].plna && !tiles[indexHorni].projeta && !tiles[indexHorni].okrajova)
                    {
                        VyplnNahoru(indexHorni);
                    }
                    if (!tiles[indexDolni].plna && !tiles[indexDolni].projeta && !tiles[indexDolni].okrajova)
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

        private void VyplnDoleva(int index)
        {
            int indexHorni; int indexDolni;
            while (!tiles[index].kvyplneni && !tiles[index].okrajova && !tiles[index].plna && !tiles[index].projeta)
            {
                tiles[index].KVyplneni(true);
                indexHorni = index - columns;
                indexDolni = index + columns;
                if (!tiles[indexHorni].kvyplneni && !tiles[indexHorni].okrajova && !tiles[indexHorni].plna && !tiles[indexHorni].projeta)
                {
                    VyplnNahoru(indexHorni);
                }
                if (!tiles[indexDolni].kvyplneni && !tiles[indexDolni].okrajova && !tiles[indexDolni].plna && !tiles[indexDolni].projeta)
                {
                    VyplnDolu(indexDolni);
                }
                index -= 1;
            }
        }

        private void VyplnDoprava(int index)
        {
            int indexHorni; int indexDolni;
            while (!tiles[index].kvyplneni && !tiles[index].okrajova && !tiles[index].plna && !tiles[index].projeta)
            {
                tiles[index].KVyplneni(true);
                indexHorni = index - columns;
                indexDolni = index + columns;
                if (!tiles[indexHorni].kvyplneni && !tiles[indexHorni].okrajova && !tiles[indexHorni].plna && !tiles[indexHorni].projeta)
                {
                    VyplnNahoru(indexHorni);
                }
                if (!tiles[indexDolni].kvyplneni && !tiles[indexDolni].okrajova && !tiles[indexDolni].plna && !tiles[indexDolni].projeta)
                {
                    VyplnDolu(indexDolni);
                }
                index += 1;
            }
        }

        private void VyplnNahoru(int index)
        {
            int indexLevy; int indexPravy;
            while (!tiles[index].kvyplneni && !tiles[index].okrajova && !tiles[index].plna && !tiles[index].projeta)
            {
                tiles[index].KVyplneni(true);
                indexLevy = index - 1;
                indexPravy = index + 1;
                if (!tiles[indexLevy].kvyplneni && !tiles[indexLevy].okrajova && !tiles[indexLevy].plna && !tiles[indexLevy].projeta)
                {
                    VyplnDoleva(indexLevy);
                }
                if (!tiles[indexPravy].kvyplneni && !tiles[indexPravy].okrajova && !tiles[indexPravy].plna && !tiles[indexPravy].projeta)
                {
                    VyplnDoprava(indexPravy);
                }
                index -= columns;
            }
        }

        private void VyplnDolu(int index)
        {
            int indexLevy; int indexPravy;
            while (!tiles[index].kvyplneni && !tiles[index].okrajova && !tiles[index].plna && !tiles[index].projeta)
            {
                tiles[index].KVyplneni(true);
                indexLevy = index - 1;
                indexPravy = index + 1;
                if (!tiles[indexLevy].kvyplneni && !tiles[indexLevy].okrajova && !tiles[indexLevy].plna && !tiles[indexLevy].projeta)
                {
                    VyplnDoleva(indexLevy);
                }
                if (!tiles[indexPravy].kvyplneni && !tiles[indexPravy].okrajova && !tiles[indexPravy].plna && !tiles[indexPravy].projeta)
                {
                    VyplnDoprava(indexPravy);
                }
                index += columns;
            }
        }

        private void ZpracujDruhePole()
        {
            VyplnPoleProOznaceni();
            if (!ZjistiKoliziPoleKoule())
            {
                if (debug)
                {
                    foreach (Tile druhetiles in tilesVnitrni)
                    {
                        if (druhetiles.kvyplneni && !druhetiles.projeta)
                        { druhetiles.OznacJakoDruhePole(true); }
                    }
                }
            }
            else OdznacDruhePole();
        }

        private void OdznacDruhePole()
        {
            foreach (Tile dlazdice in tilesVnitrni)
            {
                if (dlazdice.kvyplneni)
                {
                    dlazdice.KVyplneni(false);
                }
            }
        }

        private void OdznacPrvniPole()
        {
            foreach (Tile dlazdice in tilesVnitrni)
            {
                if (dlazdice.kvyplneni && !dlazdice.druha)
                {
                    dlazdice.KVyplneni(false);
                }
            }
        }

        private short VyznacCestuVycistiSpocti()
        {
            short soucet = 0; i = 0;
            foreach (Tile dlazdice in tilesVnitrni)
            {
                if (dlazdice.projeta && !dlazdice.plna)
                {
                    dlazdice.KVyplneni(true);
                    if (debug)
                    {
                        //if (dlazdice.plna) throw new System.Exception("podruhe vyplnujes vnitrni " + i);
                        i += 1;
                    }
                }
                if (!debug)
                {
                    if (dlazdice.prvni) dlazdice.prvni = false;
                    else if (dlazdice.druha) dlazdice.druha = false;
                }
                if (dlazdice.kvyplneni)
                {
                    dlazdice.VyplnitZvyditelnit();
                    soucet += 1;
                }
            }
            return soucet;
        }

        private bool NajdiPrvniDlazdici()
        {
            bool prvniNalezena = false;
            for (int i = pocetDlazdic - columns; i > columns; i--)
            {
                if (!tiles[i].okrajova && !tiles[i].plna && !tiles[i].projeta &&
                    (tiles[i - 1].projeta || tiles[i + columns].projeta || tiles[i + 1].projeta || tiles[i - columns].projeta))
                {
                    tiles[i].KVyplneni(true);
                    prvniNalezena = true;
                    if (debug)
                    {
                        tiles[i].DebugDlazdice(1); //tohle da zvlastni texturu prvni dlazdici
                        int j = i + 1;//tohle chci pocitat od jedne
                        int radek = j / columnsVnitrni;
                        int sloupec = j / rowsVnitrni;
                        debugPrvniDlazdice = $"Vybrana dlazdice: {i} radek: {radek} / {rows} sloupec: {sloupec} / {columns}";
                        debugText[2] = debugPrvniDlazdice;
                    }
                    else tiles[i].prvni = true;
                    return prvniNalezena;
                }
            }
            return prvniNalezena;
        }

        private bool NajdiDruhouDlazdici()
        {
            for (int i = pocetDlazdic - columns; i > columns; i--)
            {
                if (!tiles[i].kvyplneni && !tiles[i].okrajova && !tiles[i].plna && !tiles[i].projeta
                    //&& (tiles[i + 1].projeta || tiles[i - columns].projeta || tiles[i - 1].projeta) )
                    && (tiles[i + columns].projeta || tiles[i + 1].projeta || tiles[i - columns].projeta || tiles[i - 1].projeta))
                {
                    tiles[i].KVyplneni(true);
                    if (debug)
                    {
                        tiles[i].DebugDlazdice(2);
                    }
                    else tiles[i].druha = true;
                    return true;
                }
            }
            return false;
        }

        private bool ZjistiKoliziPoleKoule()
        {
            foreach (Tile tile in tilesVnitrni) //!!! bude vic prazdnych prostoru
            {
                if (tile.kvyplneni)
                {
                    foreach (Ball ball in balls)
                    {
                        if (ball.rect.Intersects(tile.drawRectangle))
                            return true;
                        else
                            foreach (Ball aball in ballsUtocne)
                            {
                                if (aball.rect.Intersects(tile.drawRectangle))
                                    return true;
                            }
                    }
                }
            }
            return false;
        }

        private static ushort SectiPlneDlazdice()
        {
            ushort plnychDlazdic = 0;
            foreach (Tile tile in tilesVnitrni) if (tile.plna) plnychDlazdic++;
            return plnychDlazdic;
        }

        internal void OdectiPlnouDlazdici()
        {
            player.NastavPrepocet(true);
        }

        private void PustUroven()
        {
            if (hrobSprite == null) hrobSprite = Content.Load<Texture2D>(@"gfx/hrob");
            hrob = new Hrobecek(false, Rectangle.Empty, hrobSprite);
            waitFrames = 60;
            SaveScore();
            SaveGame();
            staryState = gameState.ToString();
            gameState = Mojehra.Stavy.Play;
            skore = 0; skoreString = "";
            StartGame();
            Texty.Clear();
            uroven.NastavUroven();
            if (uroven.bludiste)
            {
                OdstranOkraje();
                zivotuString = "";
                //splashScreen.KresliSplash(false, "Level " + Level.cisloUrovne, false); -vypada blbe kdyz uz mam kolo postavene
            }
            string epizodaSplash;
            if (uroven.epizoda == 1)
            {
                if (uroven.cisloUrovne == 0) epizodaSplash = "Basics";
                else epizodaSplash = "Episode 1";
            }
            else if (uroven.epizoda == 2)
            {
                if (uroven.cisloUrovne == 0) epizodaSplash = "Crowd control";
                else epizodaSplash = "Episode 2";
            }
            else epizodaSplash = "Tests";
            splashScreen.KresliSplash(true, $"epizodaSplash{Environment.NewLine}{Environment.NewLine} Level {uroven.cisloUrovne}", false);
            //NapisVelkouZpravu("Level " + Level.cisloUrovne, 7000, -9999, -9999, false, true);
            if (uroven.levelText != null) procentaString = uroven.levelText;
            if (uroven.viteznychProcent != 0) procentProVitezstvi = uroven.viteznychProcent;
            else procentProVitezstvi = 70;
            if (procentProVitezstvi == 70) potrebnychPlnych = (ushort)(pocetVnitrnichDlazdic / 1.42);
            else potrebnychPlnych = (ushort)(pocetVnitrnichDlazdic * procentProVitezstvi / 100 + 1);
            minBonus = (ushort)(pocetVnitrnichDlazdic / 8);

            if (music) NahrajHudbu();
            monstra.Clear();
            if (uroven.zrodMonstrum) ZrodMonstrum(uroven.poSmeru);
            balls.Clear(); ballsUtocne.Clear(); //hitboxyKouli.Clear();
            numBalls = Level.GetNumBalls();
            numAttackBalls = Level.GetNumAttackBalls();
            if (easy) ballVelocity = new Vector2(1.4f, 1.4f);
            else if (hard) ballVelocity = new Vector2(2.0f, 2.0f);
            else ballVelocity = new Vector2(1.6f, 1.6f);
            if (!soft) ballVelocity *= rows / 2;
            foreach (Point pozice in uroven.poziceKouli) SpawnBalls(pozice.X * tileSize, pozice.Y * tileSize);
            foreach (Point pozice in uroven.poziceUtocnychKouli) SpawnBallsUtocne(pozice.X * tileSize, pozice.Y * tileSize);
            //pak muzu vlozit nahodne polohy
            for (short i = (short)(numBalls - balls.Count); i > 0; i--) { SpawnBalls(); }
            for (short i = (short)(numAttackBalls - ballsUtocne.Count); i > 0; i--) { SpawnBallsUtocne(); }
            ballsAll.AddRange(balls); ballsAll.AddRange(ballsUtocne);
            if (uroven.bezOdchylky) foreach (Ball ball in ballsAll) { ball.NastavOdchylku(0f); }
        }

        private void OdstranOkraje()
        {
            okrajeH.Clear(); okrajeV.Clear();
            foreach (Tile tile in tiles)
            {
                tile.okrajova = false;
            }
        }

        private void StartGame()
        {
            mFadeIncrement = 3;
            // Increment 5: randomly generate new number for game
            int correctNum = rand.Next(1, 10);
            // Increment 5: create the board object
            //deska = new NumberBoard(Content, stred,
            //    (int)(oknoHry.Height / 1.1), correctNum);
            BuildTiles(columns, rows, tileSize);
            player = new Hrac(true, 4, 32, 0, 32 * 2,
                oknoHry.Width, oknoHry.Height, hracsprite, hracMrtvySprite);
            zivoty++; zivotuString = zivoty.ToString();
            okrajeV.Clear(); okrajeH.Clear();
            borderV = new Rectangle(0, 0, borderThick, oknoHry.Height);
            okrajeV.Add(borderV);
            okrajeV.Add(new Rectangle(oknoHry.Width - borderThick, 0, borderThick, oknoHry.Height));
            okrajeH.Add(new Rectangle(borderThick, 0, oknoHry.Width - borderThick * 2, borderThick));
            okrajeH.Add(new Rectangle(borderThick, oknoHry.Height - borderThick, oknoHry.Width - borderThick * 2, borderThick));
            //if (debug) debugText[0] = "lives: " + zivoty;
        }

        public void NapisVelkouZpravu(string inputString, short miliseconds, short X = -9999, short vyska = -9999,
            bool fadein = false, bool fadeout = false, Color color = new Color())
        {
            Vector2 poloha = font.MeasureString(inputString);
            if (X == -9999) poloha.X = (short)(stred.X - poloha.X / 2);
            else poloha.X = X;
            if (vyska == -9999) poloha.Y = (short)(stred.Y - poloha.Y / 2);
            else poloha.Y = vyska;
            byte alfa = byte.MaxValue;
            if (fadein) alfa = 0;
            if (gameState == Mojehra.Stavy.Prohra || gameState == Mojehra.Stavy.Menu)
                barvaZpravy = new Color(byte.MinValue, byte.MinValue, byte.MinValue, alfa); //0 0 0 - cerna
            else
                barvaZpravy = new Color((byte)100, (byte)111, (byte)50, alfa);
            Zprava zprava = new Zprava(poloha, inputString, barvaZpravy, miliseconds, fadein, fadeout, font);
            ProjedZpravy(zprava);
        }
        private void NapisVelkouZpravu14(string inputString, short miliseconds, short X = -9999, short vyska = -9999,
            bool fadein = false, bool fadeout = false, Color color = new Color())
        {
            Vector2 poloha = font14.MeasureString(inputString);
            if (X == -9999) poloha.X = (short)(stred.X - poloha.X / 2);
            else poloha.X = X;
            if (vyska == -9999) poloha.Y = (short)(stred.Y - poloha.Y / 2);
            else poloha.Y = vyska;
            byte alfa = byte.MaxValue;
            if (fadein) alfa = 0;
            if (color == new Color())
            {
                if (gameState == Mojehra.Stavy.Prohra)
                    barvaZpravy = new Color(byte.MinValue, byte.MinValue, byte.MinValue, alfa); //0 0 0 - cerna
                else
                    barvaZpravy = new Color((byte)100, (byte)111, (byte)50, alfa);
            }
            else { barvaZpravy = color; }
            Zprava zprava = new Zprava(poloha, inputString, barvaZpravy, miliseconds, fadein, fadeout, font14);
            ProjedZpravy(zprava);
        }
        private void ProjedZpravy(Zprava zprava)
        {
            for (int i = 0; i < Texty.Count; i++)
            {
                if (Texty[i].hotova)
                {
                    Texty[i] = zprava;
                    return;
                }
            }
            Texty.Add(zprava);
        }
        private void KresliVelkouZpravu()
        {
            #region Dictionary
            //foreach (KeyValuePair<Vector2, string> entry in Texty)
            //{
            //    if (trvaniVelkeZpravy > 0)
            //    {
            //        trvaniVelkeZpravy -= gameTime.ElapsedGameTime.Milliseconds;
            //        string text = entry.Value;
            //        if (gameState == Mojehra.Stavy.Prohra || gameState == Mojehra.Stavy.Menu)
            //            spriteBatch.DrawString(font, text, entry.Key,
            //                new Color(nulovyBajt, nulovyBajt, nulovyBajt, (byte)MathHelper.Clamp(mAlphaValue, 0, 255))); //0 0 0 - cerna
            //        else
            //            spriteBatch.DrawString(font, text, entry.Key,
            //            new Color((byte)100, (byte)111, (byte)50, (byte)MathHelper.Clamp(mAlphaValue, 0, 255)));
            //    }
            //    else Texty.Clear();
            //    return;
            //} 
            #endregion
            foreach (Zprava zprava in Texty)
            {
                zprava.Draw(spriteBatch);
            }
        }
        private bool BlahoprejSkore(short pricistSkore)
        {
            bonus = 0;
            if (pricistSkore > minBonus)
            {
                NapisVelkouZpravu($"Awesome! {pricistSkore} squares", 3000, -9999, 100, false, true);
                bonus = (short)((pricistSkore - minBonus) * 2);
                ton2.Play();
                return true;
            }
            else
            {
                if (sound) ton1.Play();
                return false;
            }
        }

        private void ZrodMonstrum(bool poSmeru)
        {
            spriteMonstra = Content.Load<Texture2D>(@"gfx/monstrum");
            Rectangle obdelnik = new Rectangle(0, 0, tileSize, tileSize);
            ushort maxX = windowWidth - tileSize; ushort maxY = (ushort)(windowHeight - tileSize);
            if (player.vychoziX < 100) obdelnik.X = maxX;
            if (player.vychoziY < 100) obdelnik.Y = maxY;
            monstra.Add(monstrum = new Monster(tileSize, 2, obdelnik, spriteMonstra, poSmeru, maxX, maxY));
        }

        private void GetScore()
        {
            if (store.FileExists(scoreFilename))
            {
                IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(scoreFilename, FileMode.Open, FileAccess.Read);
                using (StreamReader sr = new StreamReader(isoStream))
                {
                    string line = sr.ReadLine();
                    if (line != null)
                    {
                        rekordSkore = int.Parse(line);
                    }
                }
                isoStream.Close();
            }
            else
            {
                IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(scoreFilename, FileMode.Create, FileAccess.Write);
                isoStream.Close();
                rekordSkore = 0;
            }
        }
        private void SaveScore()
        {
            if (skoreTotal > rekordSkore)
            {
                rekordSkore = skoreTotal;
                if (store.FileExists(scoreFilename))
                {
                    IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(scoreFilename, FileMode.Open, FileAccess.Write);
                    using (StreamWriter sw = new StreamWriter(isoStream))
                    {
                        sw.Flush();
                        sw.WriteLine(skoreTotal.ToString());
                        NapisVelkouZpravu("skore saved", 5555); waitFrames = 55;
                    }
                    isoStream.Close();
                }
                else NapisVelkouZpravu("Error: no fajl", 5555); waitFrames = 55;
            }
        }

        private void LoadGame()
        {
            if (store.FileExists(levelFilename))
            {
                IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(levelFilename, FileMode.Open, FileAccess.Read);
                using (StreamReader sr = new StreamReader(isoStream))
                {
                    string line = sr.ReadLine();
                    if (line != null)
                    {
                        maxLevel = byte.Parse(line);

                        line = sr.ReadLine();
                        if (line != null)
                        {
                            maxEpisoda = byte.Parse(line);

                            line = sr.ReadLine();
                            if (line != null) skoreTotal = short.Parse(line);
                        }
                    }
                }
                isoStream.Close();
            }
            else
            {
                IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(levelFilename, FileMode.Create, FileAccess.Write);
                isoStream.Close();
                maxLevel = byte.MinValue; maxEpisoda = byte.MinValue;
            }
        }
        /// <summary>
        /// Saves level, episode and skore
        /// </summary>
        private void SaveGame()
        {
            if (uroven.epizoda > 0 && uroven.epizoda >= maxEpisoda)
            {
                maxEpisoda = uroven.epizoda;
                if (uroven.cisloUrovne > maxLevel)
                {
                    maxLevel = uroven.cisloUrovne;
                    if (store.FileExists(levelFilename))
                    {
                        IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(levelFilename, FileMode.Open, FileAccess.Write);
                        using (StreamWriter sw = new StreamWriter(isoStream))
                        {
                            sw.Flush();
                            sw.WriteLine(maxLevel.ToString());
                            sw.WriteLine(uroven.epizoda.ToString());
                            sw.WriteLine(skoreTotal.ToString());
                        }
                        isoStream.Close();
                    }
                    //else NapisVelkouZpravu("Error: no fajl", 5555); waitFrames = 55;
                }
            }
        }


        private bool LoadVolumes()
        {
            if (store.FileExists(volumeFilename))
            {
                IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(volumeFilename, FileMode.Open, FileAccess.Read);
                using (StreamReader sr = new StreamReader(isoStream))
                {
                    string line = sr.ReadLine();
                    if (line != null)
                    {
                        volumeSound = float.Parse(line);
                        SoundEffect.MasterVolume = volumeSound;

                        line = sr.ReadLine();
                        if (line != null)
                        {
                            volumeHudby = float.Parse(line);
                            MediaPlayer.Volume = volumeHudby;
                        }
                    }
                }
                isoStream.Close();
                return true;
            }
            else
            {
                IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(volumeFilename, FileMode.Create, FileAccess.Write);
                isoStream.Close();
                return false;
            }
        }
        private void SaveVolumes()
        {
            if (store.FileExists(volumeFilename))
            {
                IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(volumeFilename, FileMode.Open, FileAccess.Write);
                using (StreamWriter sw = new StreamWriter(isoStream))
                {
                    sw.Flush();
                    sw.WriteLine(volumeSound.ToString());
                    sw.WriteLine(volumeHudby.ToString());
                }
                isoStream.Close();
            }
        }

        private void BeginPause(bool UserInitiated)
        {
            paused = true;
            pausedByUser = UserInitiated;
            staryState = gameState.ToString();
            gameState = Mojehra.Stavy.Pause;
            MediaPlayer.Pause();
            //TODO: Pause controller vibration
        }
        private void EndPause()
        {
            //TODO: Resume controller vibration
            if (stara != null)
            {
                if (musicFronta.ActiveSong == levelwon) MediaPlayer.Stop();
                MediaPlayer.Play(stara);
            }
            pausedByUser = paused = pristeUzNekreslim = options = false;
            if (staryState == Mojehra.Stavy.Menu.ToString()) gameState = Mojehra.Stavy.Menu;
            else if (staryState == Mojehra.Stavy.Play.ToString()) gameState = Mojehra.Stavy.Play;
            else if (staryState == Mojehra.Stavy.Prohra.ToString()) gameState = Mojehra.Stavy.Prohra;
            else if (staryState == Mojehra.Stavy.Animace.ToString()) gameState = Mojehra.Stavy.Animace;
            else if (staryState == Mojehra.Stavy.Pause.ToString()) gameState = Mojehra.Stavy.Play;
            else { gameState = Mojehra.Stavy.Vitez; }
        }
        private void checkPauseKey(GamePadState gamePadState)
        {
            pauseKeyDownThisFrame = (gamePadState.Buttons.Back == ButtonState.Pressed);
            // If key was not down before, but is down now, toggle the pause setting
            if (!pauseKeyDown && pauseKeyDownThisFrame)
            {
                if (!paused) BeginPause(true);
                else EndPause();
            }
            pauseKeyDown = pauseKeyDownThisFrame;
        }

        private void PostavPozadi(string textura, Vector2 pohyb, bool vodorovnaHustota, bool stridat = false)
        {
            Texture2D texture = Content.Load<Texture2D>(@"gfx/" + textura);
            pozadi = new Pozadi(texture, windowWidth, windowHeight, pohyb, 0f, stridat, vodorovnaHustota);
        }
    }
}