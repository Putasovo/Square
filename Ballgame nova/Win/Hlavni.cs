using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Square;

namespace Mojehra
{
    /// <summary>
    /// This is the main type
    /// </summary>
    public class Hlavni : Game
    {
        private KeyboardState keys;
        private GamePadState pad;
        private MouseState mouse;

        private readonly bool debug = false;
        private readonly bool soft = false; // software renderer
        private readonly bool easy = true; private readonly bool hard = false;
        private bool paused, pauseKeyDown, pauseKeyDownThisFrame, chciPausu, pausedByUser, pristeUzNekreslim;
        private static readonly Random rand = new Random();

        private static Stavy gameState;
        private Stavy staryState;
        private byte procentProVitezstvi;

        private float casMilisekund, scaleOpening = 0;
        private bool sudyTik = true;
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private const short tileSize = 32;
        private const ushort windowWidth = tileSize * 15;
        private const ushort windowWidthProOvladani = windowWidth - 1;
        private const short suggestedHeight = tileSize * 10; // WindowWidth / borderThick * 20; // 16:10
        private const ushort windowHeight = suggestedHeight / tileSize * tileSize;
        private const ushort windowHeigthProOvladani = windowHeight - 1;
        private readonly Rectangle oknoHry;
        //private int suggestedPixels = windowWidth * suggestedHeight;
        //private int numberOfPixels;
        //ResolutionRenderer RendererOfRealResolution;
        private Matrix scaleMatrix = Matrix.Identity;

        // private Pozadi pozadi;
        private SplashScreen splashScreen;
        private static Hrobecek hrob;
        private static Texture2D hrobSprite;
        private static Texture2D square64; //, snow;

        private readonly bool sound = true;
        private readonly bool music = true;
        private readonly List<string> skladby = new List<string>(8);
        private static Song levelwon, menu, stara;
        private static SoundEffect sezrani;
        private static SoundEffect quake;
        private SoundEffectInstance zemetres;
        private static SoundEffect ton1; // SoundEffectInstance rachot;
        private static SoundEffectInstance ton2, ton3;

        private static Texture2D openingScreen;
        private static Vector2 stred;

        private ushort waitFrames = 0;
        private uint krokIntra;
        private float trvaniAnimacky;

        private Color[] barvaVanim;

        private static SpriteFont font12, font14, font20;
        private Vector2 debugTextLocation;
        private readonly List<Zprava> Texty = new List<Zprava>(2);
        private Color barvaZpravy;
        private readonly List<string> debugText = new List<string>();
        private readonly string debugvar1, debugvar2, debugPrvniDlazdice;
        private bool letiZrovnaText; private string leticiText; private ushort snimkuLeticihoTextu;
        private Vector2 pohybLeticihoTextu, polohaLeticihoTextu;

        private ushort i = 0, plnychDlazdic, potrebnychPlnych;
        private static Texture2D tileSprite;

        private static readonly List<Tile> tilesMenuVnitrni = new List<Tile>(105);
        private static readonly List<Tile> tilesMenuOptions = new List<Tile>(151);
        private Rectangle menuNew, menuLoad, menuSettings, menuExit, menuSound, menuMusic;
        private Vector2 menuNewLoc, menuContinueLoc, menuSettingsLoc, menuExitLoc, menuSoundLoc, menuMusicLoc, menuBottomLoc;
        private string menuBottomString = string.Empty;
        private bool options;
        private Rectangle posuvnikSound, posuvnikMusic;

        private Point tileSound, tileMusic;
        private Point klik, staryklik;
        private static ushort columns, rows, columnsVnitrni, rowsVnitrni;
        private bool animovatDlazdici, videtDlazdici = true;

        private float deltaSeconds;
        private short mAlphaValue = 1, mFadeIncrement = 3;
        private double mFadeDelay = .035;

        private float colorAmount;
        private bool preklop;

        private static Hrac player;
        private ushort delkaAnimaceHrace, sloupcuAnimace;
        private const byte pocatecniZivoty = 3; private byte zivoty;
        private static Texture2D hracsprite;
        private Vector2 zadanyKlik, predchoziKlik;

        private string zivotuString;
        private string skoreString = "0", skoreTotalString = "0", excesBonus, procentaString;
        private static short skore, bonus, exces, pricistSkore;
        private ushort minBonus; // uroven pro gratulaci
        private Vector2 zivotuLocation, skoreLocation, skoreTotalLocation, procentaLocation;

        private static Texture2D spriteMonstra;
        private readonly List<Monster> monstra = new List<Monster>();

        private Level uroven;
        
        public Hlavni()
        {
            IsMouseVisible = true;
            Content.RootDirectory = "Content";
            graphics = new GraphicsDeviceManager(this);
            oknoHry = new Rectangle(0, 0, windowWidth, windowHeight);
            if (soft)
            {
                // graphics.PreferredBackBufferWidth = 384;
                // graphics.PreferredBackBufferHeight = 256;
                GraphicsAdapter.UseReferenceDevice = true;
            }
            // else
            // {   // tohle asi na droidech nejde
            //     graphics.PreferredBackBufferWidth = WindowWidth;
            //     graphics.PreferredBackBufferHeight = WindowHeight;
            // }

            if (debug)
            {
                debugvar1 = $"HardwareModeSwitch: {graphics.HardwareModeSwitch}";
                debugvar2 = $"UseDriverType: {GraphicsAdapter.UseDriverType}";
                debugPrvniDlazdice = string.Empty;
                debugText.Add(debugvar1); debugText.Add(debugvar2);
                debugText.Add(debugPrvniDlazdice);
            }

            graphics.IsFullScreen = false; // default true in android
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            stred = new Vector2(windowWidth / 2, windowHeight / 2);

            float vodorovnyPomer = graphics.PreferredBackBufferWidth / (float)windowWidth;
            float svislyPomer = graphics.PreferredBackBufferHeight / (float)windowHeight;
            if (vodorovnyPomer > svislyPomer) scaleMatrix = Matrix.CreateScale(svislyPomer, svislyPomer, 1);
            else scaleMatrix = Matrix.CreateScale(vodorovnyPomer, vodorovnyPomer, 1);

            //UI
            zivotuLocation = new Vector2(38, 11);
            skoreLocation = new Vector2(windowWidth / 2 - tileSize / 2, 11);
            skoreTotalLocation = new Vector2(windowWidth - 68, 11);
            procentaLocation = new Vector2(stred.X - tileSize, windowHeight - 21);
            debugTextLocation = new Vector2(windowWidth / 11, windowHeight / 11);

            columns = windowWidth / tileSize;
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
            //else
            //  throw new SystemException("Touchpannel needed");

            Store.LoadVolumes();
            SoundEffect.MasterVolume = Store.VolumeSound;
            MediaPlayer.Volume = Store.VolumeHudby;

            zivoty = pocatecniZivoty;
            zivotuString = $"{zivoty}";

            // Vibrace.Init(null);
            PlayBoard.Init(tileSize, columns, rows);
            uroven = new Level(rows);            

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
            tileSprite = Content.Load<Texture2D>(@"gfx/tile");
            hracsprite = Content.Load<Texture2D>(@"gfx/hrac");
            Balls.LoadSprite(Content);

            if (sound)
            {
                NahrajZvuky();
                ton1.Play(.4f, 0, 0);
            }

            if (music)
            {
                NahrajHudbu();
                byte pocetSkladeb = 5;
                skladby.Add(levelwon.Name); skladby.Add(menu.Name);
                for (byte i = 0; i <= pocetSkladeb; i++)
                {
                    skladby.Add($"level{i}");
                }
            }

            font12 = Content.Load<SpriteFont>("PressStart2P12");
            font14 = Content.Load<SpriteFont>("PressStart2P14");
            font20 = Content.Load<SpriteFont>("PressStart2P20");

            // splashScreen = new SplashScreen(graphics, new Rectangle(windowWidth, 0, (int)(oknoHry.Width * scaleMatrix.M11), oknoHry.Height), font20);
            splashScreen = new SplashScreen(graphics, new Rectangle(windowWidth, 0, (int)(oknoHry.Width * 1.34), oknoHry.Height), font20);
            PlayBoard.VybarviOkraje(graphics, oknoHry, Barvy.vyblitaZelena, Color.Green);

            Store.GetScore();
            if (Store.RekordSkore > 0)
                NapisVelkouZpravu($"Your best: {Store.RekordSkore}", 8000, -9999, 200, true, true, Color.Yellow);

            // PostavPozadi("snow", new Vector2(.1f, 1), true, true);
            PlayBoard.Init(tileSize, columns, rows);
            BuildMenu();
            BuildMenuOptions();
            animovatDlazdici = true;
            BuildTiles(columns, rows, tileSize); // muzu az po assetech

            gameState = Stavy.Menu;
        }

        private void NahrajHudbu()
        {
            if (gameState == Stavy.Play)
            {
                if (!Level.Bludiste)
                {
                    string skladba = $"level{uroven.CisloUrovne}";
                    if (skladby.Contains(skladba))
                    {
                        Song level = Content.Load<Song>(@"audio/" + skladba);
                        MediaPlayer.Play(level);
                        MediaPlayer.IsRepeating = true;
                        stara = level;
                    }
                    else 
                        MediaPlayer.Play(menu);
                }
                else
                {
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Play(menu);
                }
            }
            else
            {
                // prvni spusteni
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
            // ton2 = Content.Load<SoundEffect>(@"audio/ton2");
            // instanceOdrazu = odraz.CreateInstance();
            sezrani = Content.Load<SoundEffect>(@"audio/lost2");
            // zvukKolizeKouli = Content.Load<SoundEffect>(@"audio/explosion");
            // rachot = zvukKolizeKouli.CreateInstance();
            // rachot.Volume = .3f;
            quake = Content.Load<SoundEffect>(@"audio/zemetreseni");
            zemetres = quake.CreateInstance();
            Balls.LoadContent(Content);
        }

        // UnloadContent will be called once per game and is the place to unload game-specific content.
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            Content.Unload();
        }

        protected override void OnActivated(object sender, System.EventArgs args)
        {
            Window.Title = Texts.ActiveLabel;
            if (debug) 
                Window.Title = Window.Title + gameState;

            base.OnActivated(sender, args);
        }

        protected override void OnDeactivated(object sender, System.EventArgs args)
        {
            Window.Title = Texts.InactiveLabel;
            base.OnActivated(sender, args);
        }

        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (IsActive) // je okno videt?
            {
                if (waitFrames > 0)
                    waitFrames -= 1;
                else
                {
                    ModulujAlfu(gameTime.ElapsedGameTime.TotalSeconds);
                    keys = Keyboard.GetState(); mouse = Mouse.GetState();
                    pad = GamePad.GetState(PlayerIndex.One);
                    CheckPauseKey(pad);

                    splashScreen.Update();

                    if (splashScreen.ProvedUpdate)
                    {
                        //TouchCollection doteky = TouchPanel.GetState();
                        UrciStav(keys);
                        sudyTik = !sudyTik;

                        // if (pozadi != null) pozadi.Update();

                        foreach (Zprava zprava in Texty)
                        {
                            zprava.Update(gameTime.ElapsedGameTime.Milliseconds);
                        }

                        if (PlayBoard.ProbihaVybuch)
                        {
                            PlayBoard.VybuchPostupne();
                            if (zemetres.State == SoundState.Stopped)
                                zemetres.Play();
                        }

                        if (gameState == Stavy.Play)
                        {
                            if (!Level.Bludiste)
                            {
                                player.Input(Dotek(mouse));

                                if (uroven.PerformanceTest)
                                {
                                    skoreString = $"{gameTime.ElapsedGameTime.Milliseconds}";
                                    if (player.hracovo.X > stred.X)
                                        Zvitezit();
                                }
                            }
                            else
                            {
                                player.BludisteInput(Dotek(mouse));

                                foreach (Ball ball in Balls.balls)
                                {
                                    if (ball.Cinna && ball.rect.Intersects(player.hracovo))
                                    {
                                        SmrtKvuliKouli(ball);
                                    }                                    
                                }
                            }

                            if (letiZrovnaText)
                                PosliTextSectiSkore();

                            if (player.IsAlive())
                            {
                                if (player.PrepoctemeSkore())
                                {
                                    TotalSkore();
                                }
                                else
                                {
                                    if (hrob.ZkontrolujMisto(player.hracovo.Location))
                                    {
                                        if (hrob.Obsah > 0)
                                        {
                                            skore += hrob.Obsah;
                                            PosliLeticiSkore($"{hrob.Obsah}", player.hracovo.Location, 60);
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
                                        if (monstrum.CheckIntersection(player.hracovo))
                                        {
                                            if (sound)
                                                sezrani.Play();
                                            Umri();
                                        }
                                        else
                                            monstrum.Update();
                                    }
                                }
                            }

                            #region balls
                            if (delkaAnimaceHrace > 0)
                                delkaAnimaceHrace -= 1;
                            else // žije
                            {
                                foreach (Ball ball in Balls.ballsUtocne)
                                {
                                    ball.Update(gameTime.ElapsedGameTime.Milliseconds);
                                }
                                foreach (Ball ball in Balls.balls)
                                {
                                    ball.Update(gameTime.ElapsedGameTime.Milliseconds);
                                }
                                // if (rachot.State != SoundState.Playing)
                                {
                                    // if (debug) { int i = balls.Count;int j = hitboxyKouli.Count; if (j != i) { throw new SystemException("blbe pocitam koule"); }  }
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
                            #endregion
                        }
                        else if (gameState == Stavy.Animace)
                        {
                            HrajIntro(gameTime.ElapsedGameTime);
                        }

                        foreach (Tile tile in PlayBoard.tiles) //pro animace
                        {
                            tile.Update();
                        }

                        // if (hrajOdraz) HrajOdraz();
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
            MenBarvuPozadi(gameTime);
            GraphicsDevice.Clear(Color.Lerp(Barvy.PrvniBarva, Barvy.DruhaBarva, colorAmount));

            if (gameState == Stavy.Vitez)
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, scaleMatrix);
            else
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, scaleMatrix);

            // if (pozadi != null) pozadi.DrawBezRotace(spriteBatch);

            foreach (Rectangle okraj in PlayBoard.okrajeV)
            {
                spriteBatch.Draw(PlayBoard.TexOkrajeV, okraj, Color.White);
            }
            foreach (Rectangle okraj in PlayBoard.okrajeH)
            {
                spriteBatch.Draw(PlayBoard.TexOkrajeH, okraj, Color.White);
            }

            if (gameState == Stavy.Pause)
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
            else if (gameState == Stavy.Animace)
            {
                foreach (Tile tile in PlayBoard.tiles)
                {
                    tile.DrawPlusOkrajove(spriteBatch);
                }
            }
            else // tady se hraje
            {
                hrob?.Draw(spriteBatch);

                if (PlayBoard.Zemetreseni == 0)
                {
                    foreach (Tile tile in PlayBoard.tiles)
                    {
                        tile.Draw(spriteBatch);
                    }
                }
                else
                {
                    PlayBoard.KrokZemetreseni();

                    if (sudyTik)
                    {
                        int random = rand.Next(-4, 4);
                        foreach (Tile tile in PlayBoard.tiles)
                        {
                            tile.DrawZemetreseni(spriteBatch, random);
                        }
                    }
                    else if (!Level.Bludiste)
                        ZkontrolujVitezstvi();
                }

                if (gameState == Stavy.Menu)
                {
                    casMilisekund += gameTime.ElapsedGameTime.Milliseconds;
                    float mujcas = casMilisekund / 777;
                    if (scaleOpening <= 1) scaleOpening += .0083f;
                    var poloha = new Vector2(oknoHry.Center.X + tileSize - colorAmount * 100, oknoHry.Center.Y);
                    float rotace = colorAmount * .6f - .2f;
                    float scale = ((float)oknoHry.Height / 69);
                    var stredOtaceni = new Vector2(32, 32);

                    spriteBatch.Draw(Balls.ballSprite, new Vector2(300, oknoHry.Height / 2), new Rectangle(0, 0, 32, 32), new Color(160, 60, 60, 177),
                        mujcas, new Vector2(60, oknoHry.Height / 2), 1.7f, SpriteEffects.None, 1);
                    spriteBatch.Draw(square64, poloha, new Rectangle(0, 0, 64, 64), Color.White,
                        rotace, stredOtaceni, scale, SpriteEffects.None, 1);
                    spriteBatch.Draw(openingScreen, new Vector2(oknoHry.Center.X - scaleOpening * oknoHry.Center.X,
                         oknoHry.Center.Y - scaleOpening / .8f * oknoHry.Center.Y),
                         openingScreen.Bounds, Color.White, 0f, Vector2.Zero, scaleOpening, SpriteEffects.None, 1);

                    if (Store.SaveGameExists)
                    {
                        if (casMilisekund > 4044 && casMilisekund < 4077)
                            NapisVelkouZpravu14("Continue?", 22222, -9999, -9999, true, true, Barvy.druhaViteznaBarva);
                    }
                    else if (casMilisekund > 7000 && casMilisekund < 7040)
                        NapisVelkouZpravu14("Ready?", 12000, -9999, -9999, true, true, Barvy.druhaViteznaBarva);

                    if (casMilisekund > 22000 && casMilisekund < 22033)
                        NapisVelkouZpravu14("Press Enter to play", 14000, -9999, -9999, true, true, Color.Red);
                }
            }

            if (gameState != Stavy.Pause)
            {
                if (player != null)
                    player.Kresli(spriteBatch);

                foreach (Ball ball in Balls.balls)
                    ball.Draw(spriteBatch, Balls.ballSprite);

                foreach (Ball ball in Balls.ballsUtocne)
                    ball.Draw(spriteBatch, Balls.ballSprite);

                foreach (Monster monstrum in monstra) 
                    monstrum.Draw(spriteBatch);
            }

            #region drawing texts
            foreach (Zprava zprava in Texty)
                zprava.Draw(spriteBatch);

            if (debug)
            {
                Vector2 poziceRadky = debugTextLocation;
                for (i = 0; i < debugText.Count; i++)
                {
                    if (debugText[i] != null)
                    {
                        spriteBatch.DrawString(font12, debugText[i], poziceRadky, Color.Cyan);
                        poziceRadky.Y += 22f;
                    }
                }
            }

            if (gameState == Stavy.Play || gameState == Stavy.Vitez || gameState == Stavy.Prohra)
            {
                spriteBatch.DrawString(font12, zivotuString, zivotuLocation, Color.Aqua);
                spriteBatch.DrawString(font12, skoreString, skoreLocation, Color.Aqua);
                spriteBatch.DrawString(font12, skoreTotalString, skoreTotalLocation, Color.Aqua);
                spriteBatch.DrawString(font12, procentaString, procentaLocation, Color.White);
            }
            else if (gameState == Stavy.Pause)
            {
                if (!options)
                {
                    spriteBatch.DrawString(font14, Texts.New, menuNewLoc, Color.Azure);
                    spriteBatch.DrawString(font14, Texts.Resume, menuContinueLoc, Color.Azure);
                    spriteBatch.DrawString(font14, Texts.Exit, menuExitLoc, Color.Azure);
                    spriteBatch.DrawString(font14, Texts.Options, menuSettingsLoc, Color.Azure);
                }
                else
                {
                    spriteBatch.DrawString(font14, Texts.Sound, menuSoundLoc, Color.Beige);
                    spriteBatch.DrawString(font14, Texts.Music, menuMusicLoc, Color.Coral);
                }
                spriteBatch.DrawString(font12, menuBottomString, menuBottomLoc, Color.White);
            }

            if (letiZrovnaText)
            {
                spriteBatch.DrawString(font12, leticiText, polohaLeticihoTextu, Color.Aqua);
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
            if (gameState != Stavy.Vitez)
                deltaSeconds /= 4;
            if (colorAmount >= 1.0f)
                preklop = true;
            else if (colorAmount <= 0 && preklop == true)
                preklop = false;

            if (preklop == false)
                colorAmount += deltaSeconds;
            else
                colorAmount -= deltaSeconds;
        }

        private Point Dotek(MouseState mouseState)
        {
            predchoziKlik = zadanyKlik;
            if (mouseState.Position.ToVector2() != predchoziKlik)
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    //zadanyPohyb = touch.Position/ new Vector2( (float)Math.Truncate(scaleMatrix.M11), (float)Math.Truncate(scaleMatrix.M22) );
                    zadanyKlik.X = MathHelper.Clamp(mouseState.Position.X / scaleMatrix.M11, 0, windowWidthProOvladani);//jinak pude tukat mimo herni plochu
                    zadanyKlik.Y = MathHelper.Clamp(mouseState.Position.Y / scaleMatrix.M22, 0, windowHeigthProOvladani);// nestaci (float)Math.Truncate(touch.Position.Y / scaleMatrix.M22); (float)Math.Truncate(mouseState.Position.Y / scaleMatrix.M22);
                }
            }

            if (zadanyKlik != predchoziKlik)
                return zadanyKlik.ToPoint();

            return Point.Zero;
        }

        private void UrciStav(KeyboardState kb)
        {
            if (gameState == Stavy.Prohra)
            {
                if (delkaAnimaceHrace == 0)
                {
                    player = null;
                    NapisVelkouZpravu("You lost", 10000);
                    if (kb.IsKeyDown(Keys.Enter))
                    {
                        staryState = gameState;
                        gameState = Stavy.Menu;
                    }
                }
                else delkaAnimaceHrace--;
            }
            else if (gameState == Stavy.Vitez)
            {
                if (MediaPlayer.IsRepeating)
                {
                    MediaPlayer.IsRepeating = false;
                    MediaPlayer.Play(levelwon);
                }

                if (letiZrovnaText)
                    PosliTextSectiSkore();
                else
                {
                    if (skore > 0)
                    {
                        if (waitFrames % 4 == 0)
                        {
                            skore--;
                            skoreString = $"{skore}";
                            Store.SkoreTotal++;
                            skoreTotalString = $"{Store.SkoreTotal}";
                        }
                    }
                    else if (kb.IsKeyDown(Keys.Enter))
                    {
                        uroven.ZvedniUroven();
                        Store.SaveGame(uroven);
                        PustUroven();
                    }
                }
            }
            else if (gameState == Stavy.Pause)
            {
                if (!pristeUzNekreslim)
                {
                    // NapisVelkouZpravu("Back Button = Pause" + Environment.NewLine + Environment.NewLine + "Press again for menu", 100); neukaze se napred
                    pristeUzNekreslim = true;
                    SuppressDraw();  // melo by snizit zatizeni
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
                                if (staryState != Stavy.Menu)
                                {
                                    Store.LoadGame();
                                    if (Store.SaveGameExists)
                                    {
                                        skoreTotalString = $"{Store.SkoreTotal}";
                                        uroven.NastavEpisodu(Store.MaxEpisoda);
                                        ZacniNovouEpizodu();
                                        uroven.NastavLevel(Store.MaxLevel);
                                        PustUroven();
                                    }
                                    else
                                    {
                                        menuBottomString = Texts.No_Valid_Save;
                                        PustUroven();
                                    }
                                }
                                else
                                {
                                    NapisVelkouZpravu(Texts.Too_soon, 1000);
                                    pristeUzNekreslim = false;
                                }
                            }
                            else if (menuExit.Contains(klik.X, klik.Y))
                            {
                                if (Store.SaveScore())
                                    NapisVelkouZpravu(Texts.skore_saved, 5555);
                                else
                                    NapisVelkouZpravu(Texts.Error__no_fajl, 5555);
                                waitFrames = 55;

                                Store.SaveGame(uroven); Store.SaveVolumes();

                                Exit();
                                if (staryState == Stavy.Play)
                                    gameState = Stavy.Play;
                                else if (staryState == Stavy.Vitez)
                                    gameState = Stavy.Vitez;
                                else
                                    gameState = Stavy.Animace;
                                // System.Environment.Exit(0);
                            }
                            else if (menuNew.Contains(klik.X, klik.Y))
                            {
                                uroven.NastavLevel(0); uroven.NastavEpisodu(1);
                                Store.SkoreTotal = 0;
                                skoreTotalString = $"{Store.SkoreTotal}";
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
                                    if (tile.Cilova && tile.drawRectangle.Y == posuvnikSound.Y)
                                        tile.OznacJakoCilovou(false);

                                    if (tile.drawRectangle.Contains(klik))
                                    {
                                        tile.OznacJakoCilovou(true);
                                        Store.VolumeSound = (float)(tile.drawRectangle.X / tileSize - 2) / 10;
                                    }
                                }

                                SoundEffect.MasterVolume = Store.VolumeSound;
                                ton1.Play();
                                pristeUzNekreslim = false;
                            }
                            else if (posuvnikMusic.Contains(klik.X, klik.Y))
                            {
                                foreach (Tile tile in tilesMenuOptions)
                                {
                                    if (tile.Cilova && tile.drawRectangle.Y == posuvnikMusic.Y)
                                        tile.OznacJakoCilovou(false);

                                    if (tile.drawRectangle.Contains(klik))
                                    {
                                        tile.OznacJakoCilovou(true);
                                        Store.VolumeHudby = (float)(tile.drawRectangle.X / tileSize - 2) / 10;
                                    }
                                }
                                MediaPlayer.Volume = Store.VolumeHudby;
                                MediaPlayer.Play(levelwon);
                                pristeUzNekreslim = false;
                            }
                        }
                    }
                }
            }
            else if (gameState == Stavy.Menu)
            {
                // if (debug) debugText[0] = "jsi v menu!";
                if (kb.IsKeyDown(Keys.Enter) || mouse.LeftButton == ButtonState.Pressed)
                {
                    if (staryState == Stavy.Play)
                    {
                        staryState = gameState;
                        gameState = Stavy.Play;
                    }
                    else if (staryState == Stavy.Prohra)
                    {
                        zivoty = pocatecniZivoty;
                        PustUroven();
                    }
                    else // prvni spuštění
                    {
                        if (Store.SaveGameExists)
                        {
                            animovatDlazdici = false; videtDlazdici = false;
                            Store.LoadGame();
                            skoreTotalString = $"{Store.SkoreTotal}";
                            uroven.NastavEpisodu(Store.MaxEpisoda);
                            ZacniNovouEpizodu();
                            uroven.NastavLevel(Store.MaxLevel);
                            PustUroven();
                        }
                        else 
                            Intro();
                    }
                }
            }
            else // hraje intro
            {
                if (mouse.LeftButton == ButtonState.Pressed || kb.IsKeyDown(Keys.Enter))
                    trvaniAnimacky -= 1;
            }
        }

        private void Intro()
        {
            Texty.Clear();
            waitFrames = 1;
            animovatDlazdici = false; videtDlazdici = false;

            barvaVanim = new Color[oknoHry.Height * tileSize * 2];
            PlayBoard.TexOkrajeV = new Texture2D(graphics.GraphicsDevice, tileSize * 2, oknoHry.Height);
            PlayBoard.BorderVanim = new Rectangle(oknoHry.Width, 0, tileSize * 2, oknoHry.Height);

            for (int i = 0; i < barvaVanim.Length; ++i)
                barvaVanim[i] = Color.Green;

            PlayBoard.TexOkrajeV.SetData(barvaVanim);
            PlayBoard.okrajeV.Clear();
            PlayBoard.okrajeV.Add(PlayBoard.BorderVanim);

            player = new Hrac(true, 4, tileSize, -tileSize * 10, rows * tileSize / 2 - tileSize / 2, windowWidth, windowHeight, hracsprite);
            player.NastavTexturu(new Rectangle(0, tileSize, tileSize, tileSize));
            sloupcuAnimace = columns;
            sloupcuAnimace++;
            BuildTiles(sloupcuAnimace, rows, tileSize);
            foreach (Tile tile in PlayBoard.tiles)
            {
                if (tile.drawRectangle.Y == 0 || tile.drawRectangle.Y == (rows * tileSize) - tileSize)
                {
                    tile.VyplnitZvyditelnitOkamzite();
                }
            }

            Balls.SpawnWithVelocity(new Vector2(4.7f, 3f));
            Balls.SpawnWithVelocity(new Vector2(4.7f, -3f));
            ushort j = 0;
            foreach (Ball ball in Balls.balls)
            {
                ball.PovolVariace(false); ball.NastavRychlost(.01f);
                ball.NastavPolohu(new Vector2(-tileSize * (9 + j), stred.Y - tileSize / 2));
                j++;
            }

            gameState = Stavy.Animace;
            trvaniAnimacky = 21.2f;
            Barvy.PrvniBarva = new Color(100, 0, 100, 0);
            Barvy.DruhaBarva = new Color(0, 24, 0, 0);
            MediaPlayer.Play(Content.Load<Song>(@"audio/intro"));
        }

        private void HrajIntro(TimeSpan elapsedTime)
        {
            short posun = 2;
            int cyklus = tileSize / posun;
            short cilovyXokraje = windowWidth - tileSize * 2;

            if (trvaniAnimacky > 0)
            {
                trvaniAnimacky -= (float)elapsedTime.Milliseconds / 1000;

                if (PlayBoard.BorderVanim.X != cilovyXokraje)
                {
                    if (krokIntra < cyklus)
                    {
                        krokIntra++;
                        foreach (Tile tile in PlayBoard.tiles)
                        {
                            tile.drawRectangle.X -= 2;
                        }
                    }
                    else
                    {
                        krokIntra = 0;
                        foreach (Tile tile in PlayBoard.tiles)
                        {
                            tile.drawRectangle.X += tileSize;
                        }
                    }
                }
                if (player.hracovo.X < windowWidth - tileSize * 1.5)
                    player.hracovo.X++;

                foreach (Ball ball in Balls.balls)
                {
                    if (ball.rect.Right == cilovyXokraje)
                    {
                        ton1.Play();
                        player.NastavTexturu(new Rectangle(0, tileSize, tileSize, tileSize));
                    }
                    ball.UpdateAnimace(
                        elapsedTime.Milliseconds,
                        (short)(oknoHry.Width - tileSize * 3),
                        (short)(oknoHry.Height - tileSize),
                        -9999,
                        (short)tileSize);
                }

                if (trvaniAnimacky < 7)
                {
                    if (PlayBoard.BorderVanim.X > cilovyXokraje)
                    {
                        PlayBoard.BorderVanim = PlayBoard.okrajeV[0];
                        PlayBoard.BorderVanim.X -= 2;
                        PlayBoard.okrajeV[0] = PlayBoard.BorderVanim;
                    }
                }

                if (trvaniAnimacky < 19 && trvaniAnimacky > 18.9)
                    NapisVelkouZpravu("Running away", 1000, tileSize * 2, tileSize * 2, true, true);
                else if (trvaniAnimacky < 18 && trvaniAnimacky > 17.93)
                    NapisVelkouZpravu("Running away .", 1000, tileSize * 2, tileSize * 2);
                else if (trvaniAnimacky < 17 && trvaniAnimacky > 16.93)
                    NapisVelkouZpravu("Running away ..", 1000, tileSize * 2, tileSize * 2);
                else if (trvaniAnimacky < 16 && trvaniAnimacky > 15.93)
                    NapisVelkouZpravu("Running away ...", 1900, tileSize * 2, tileSize * 2);
                else if (trvaniAnimacky < 14 && trvaniAnimacky > 13.93)
                    NapisVelkouZpravu("for so long ...", 3900, tileSize * 4, (short)((rows - 3) * tileSize));
                else if (trvaniAnimacky < 10 && trvaniAnimacky > 9.93)
                    NapisVelkouZpravu("either breaks you...", 3900, tileSize * 6, tileSize * 3);
                else if (trvaniAnimacky < 5 && trvaniAnimacky > 4.93)
                {
                    NapisVelkouZpravu("or helps you stand", 4000, (short)((sloupcuAnimace - 8) * tileSize), (short)((rows - 3) * tileSize));
                    player.NastavTexturu(new Rectangle(tileSize, tileSize, tileSize, tileSize));
                }
                else if (trvaniAnimacky < 0 && !splashScreen.KreslitSplash)
                    splashScreen.ZatemniSplash(true);
            }
            else
            {
                if (MediaPlayer.State == MediaState.Stopped || trvaniAnimacky < -1f)
                {
                    gameState = Stavy.Play;
                    PustUroven();
                }
            }
        }

        private void ZjistiNarazDoCesty()
        {
            foreach (Tile dlazdice in PlayBoard.tilesVnitrni)
            {
                if (!dlazdice.Okrajova && !dlazdice.Plna && dlazdice.Projeta)
                {
                    foreach (Ball ball in Balls.balls)
                    {
                        if (ball.Cinna && ball.rect.Intersects(dlazdice.drawRectangle))
                        {
                            SmrtKvuliKouli(ball);
                            return;
                        }
                    }
                    foreach (Ball ball in Balls.ballsUtocne)
                    {
                        if (ball.Cinna && ball.rect.Intersects(dlazdice.drawRectangle))
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
            // Decrement the delay by seconds elapsed since the last Update call
            mFadeDelay -= vteriny;
            if (mFadeDelay <= 0)
            {   // time to fade in/out a bit more.
                mFadeDelay = .35;             // Reset the Fade delay
                mAlphaValue += mFadeIncrement;
                if (mAlphaValue >= 255 || mAlphaValue <= 0)
                { 
                    mFadeIncrement *= -1; // preklop
                }
            }
        }

        private void SmrtKvuliKouli(Ball ball)
        {
            Umri();
            ball.SrazkasProjetou();
        }

        private void Umri()
        {
            delkaAnimaceHrace = 50;
            player.NastavAnimaci(byte.MinValue, delkaAnimaceHrace);
            zivoty -= 1;
            if (zivoty == 0)
            {
                gameState = Stavy.Prohra;
                zivotuString = "0";
                Vibrace.Vibruj(1666);
            }
            else
            {
                Vibrace.Vibruj(1111);

                PlayBoard.OdznacProjete();
                zivotuString = $"{zivoty}";
                hrob.Nastav(player, skore);
                if (uroven.ZrodMonstrum) 
                    foreach (Monster monstrum in monstra) 
                        monstrum.Respawn();
            }

            skore = pricistSkore = 0;
            skoreString = "0";
        }

        protected void BuildTiles(ushort columns, ushort rows, short tilesize)
        {
            PlayBoard.tiles.Clear(); PlayBoard.tilesVnitrni.Clear();
            var velocity = Vector2.Zero;
            int pravyokraj = windowWidth - tilesize;
            int dolniokraj = windowHeight - tilesize;
            for (ushort i = 0; i < rows; i++)
            {
                for (byte j = 0; j < columns; j++)
                {
                    var location = new Vector2(j * tileSize, i * tileSize);
                    bool naokraji = false;
                    if (location.X == 0 || location.X == pravyokraj || location.Y == 0 || location.Y == dolniokraj)
                    {
                        naokraji = true;
                    }
                    
                    var tile = new Tile(tileSprite, location, velocity, tileSize, tileSize, animovatDlazdici, videtDlazdici, naokraji, debug);
                    PlayBoard.tiles.Add(tile);
                    if (!naokraji)
                    {
                        PlayBoard.tilesVnitrni.Add(tile);
                        tile.SudaNeboLicha(i * j);
                    }
                }
            }

            if (debug)
                procentaString = $"{SectiPlneDlazdice()} / {PlayBoard.tilesVnitrni.Count}";
            else
                procentaString = $"{PlayBoard.tilesVnitrni.Count} tiles to fill";
        }

        protected void BuildMenu()
        {
            menuBottomLoc = procentaLocation;
            var velocity = Vector2.Zero;
            int pravyokraj = windowWidth - tileSize;
            int dolniokraj = windowHeight - tileSize;
            for (byte i = 0; i < rows; i++)
            {
                for (ushort j = 0; j < columns; j++)
                {
                    var location = new Vector2(j * tileSize, i * tileSize);
                    bool naokraji = false;
                    if (location.X == 0 || location.X == pravyokraj || location.Y == 0 || location.Y == dolniokraj)
                    {
                        naokraji = true;
                    }

                    var tile = new Tile(tileSprite, location, velocity, tileSize, tileSize, animovatDlazdici, videtDlazdici, naokraji, debug);

                    if (!naokraji)
                    {
                        tilesMenuVnitrni.Add(tile);
                        tile.SudaNeboLicha(i);
                    }
                }
            }

            for (byte i = 0; i < rowsVnitrni; i++)
            {
                for (byte j = 0; j < columnsVnitrni; j++)
                {
                    if (i == 0 || i == 3 || i == 4 || i == 7) 
                        tilesMenuVnitrni[i * columnsVnitrni + j].VyplnitPredemZvyditelnit();
                    if (j == 0 || j == 6 || j == 12) 
                        tilesMenuVnitrni[i * columnsVnitrni + j].VyplnitPredemZvyditelnit();
                }
            }

            menuNew = new Rectangle(64, 64, 160, 64);
            Vector2 measured = font14.MeasureString(Texts.New);
            menuNewLoc = new Vector2(menuNew.X + (menuNew.Width - measured.X) / 2, menuNew.Y + ((menuNew.Height - measured.Y) / 2));

            menuLoad = new Rectangle(256, 64, 160, 64);
            measured = font14.MeasureString(Texts.Resume);
            menuContinueLoc = new Vector2(menuLoad.X + (menuLoad.Width - measured.X) / 2, menuLoad.Y + ((menuLoad.Height - measured.Y) / 2));

            menuSettings = new Rectangle(64, 192, 160, 64);
            measured = font14.MeasureString(Texts.Options);
            menuSettingsLoc = new Vector2(menuSettings.X + (menuSettings.Width - measured.X) / 2, menuSettings.Y + ((menuSettings.Height - measured.Y) / 2));

            menuExit = new Rectangle(256, 192, 160, 64);
            measured = font14.MeasureString(Texts.Exit);
            menuExitLoc = new Vector2(menuExit.X + (menuExit.Width - measured.X) / 2, menuExit.Y + ((menuExit.Height - measured.Y) / 2));
        }

        protected void BuildMenuOptions()
        {
            var velocity = Vector2.Zero;
            int pravyokraj = oknoHry.Width - tileSize;
            int dolniokraj = oknoHry.Height - tileSize;
            for (byte i = 0; i < rows; i++)
            {
                for (ushort j = 0; j < columns; j++)
                {
                    var location = new Vector2(j * tileSize, i * tileSize);
                    bool naokraji = false;
                    if (location.X == 0 || location.X == pravyokraj || location.Y == 0 || location.Y == dolniokraj)
                    {
                        naokraji = true;
                    }
                    var tile = new Tile(tileSprite, location, velocity, tileSize, tileSize, animovatDlazdici, videtDlazdici, naokraji, debug);
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
            Vector2 measured = font14.MeasureString(Texts.Sound);
            menuSoundLoc = new Vector2((menuSound.X + (menuSound.Width - measured.X) / 2), menuSound.Y + ((menuSound.Height - measured.Y) / 2));

            menuMusic = new Rectangle(64, 160, 352, 32);
            measured = font14.MeasureString(Texts.Music);
            menuMusicLoc = new Vector2((menuMusic.X + (menuMusic.Width - measured.X) / 2), menuMusic.Y + ((menuMusic.Height - measured.Y) / 2));

            posuvnikSound = new Rectangle(menuSound.X, menuSound.Y + 64, tileSize * 11, tileSize);
            posuvnikMusic = new Rectangle(menuMusic.X, menuMusic.Y + 64, tileSize * 11, tileSize);
            tileSound = new Point(menuSound.X + (int)(Store.VolumeSound * 10) * tileSize, menuSound.Y + 64);
            tileMusic = new Point(menuMusic.X + (int)(Store.VolumeSound * 10) * tileSize, menuMusic.Y + 64);
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
            {   
                foreach (Tile tile in PlayBoard.tilesVnitrni)
                {
                    if (tile.Prvni || tile.Druha)
                        tile.DebugDlazdice(0); // odeberu zvlastni textury predchoziho vyplneni
                }
            }

            if (!Level.Bludiste)
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
                else if (NajdiDruhouDlazdici())
                    ZpracujDruhePole();

                if (letiZrovnaText)
                { // zrusim stary let a sectu jeho hodnotu hned, abych nekazil novou
                    snimkuLeticihoTextu = 0;
                    PosliTextSectiSkore();
                }

                pricistSkore = VyznacCestuVycistiSpocti();
                PlayBoard.OdznacProjete();
                if (BlahoprejSkore(pricistSkore))
                    PosliLeticiSkore($"{pricistSkore} + {bonus}", player.hracovo.Location, 60);
                else
                    PosliLeticiSkore($"{pricistSkore}", player.hracovo.Location, 60);

                ZkontrolujVitezstvi();
                // if (debug) chciPausu = true;
            }
            else
            {
                Store.SkoreTotal = +100;
                gameState = Stavy.Vitez;
                ZastavKoule(); ZastavAgresivniKoule();
                NapisVelkouZpravu("What did you find?", 10000);
                procentaString = " bonus:" + Store.SkoreTotal;
                waitFrames += 30;
            }
        }

        private void ZkontrolujVitezstvi()
        {
            plnychDlazdic = SectiPlneDlazdice();
            if ((float)plnychDlazdic / PlayBoard.tilesVnitrni.Count * 100 > procentProVitezstvi)
            {
                Zvitezit();
            }
            else
            {
                // procentaString = plnychDlazdic + " / " + pocetVnitrnichDlazdic; 
                int zbyva = potrebnychPlnych - plnychDlazdic;
                if (zbyva > potrebnychPlnych / 8)
                    procentaString = $"To do: {zbyva}";
                else
                    procentaString = $"Only {zbyva} more!";
            }
        }

        private void Zvitezit()
        {
            // NastavBarvy(Barvy.prvniViteznaBarva, Barvy.druhaViteznaBarva);
            mFadeIncrement *= 4;
            gameState = Stavy.Vitez;
            ZastavKoule(); ZastavAgresivniKoule();
            short delayedBonus = ZpetnePlneni();
            if (delayedBonus > 0)
                NapisVelkouZpravu($"You Win{Environment.NewLine}Delayed Bonus: {delayedBonus}", 10000, -9999, -9999, true, true, Color.Aqua);
            else
                NapisVelkouZpravu("You Win", 10000, -9999, -9999, true, true, Color.Aqua);
            exces = (short)(plnychDlazdic - potrebnychPlnych);
            if (exces > 0)
                excesBonus = $"Excess Bonus: {exces}";
            else
                excesBonus = string.Empty;

            procentaString = excesBonus;
            Store.SkoreTotal += exces;
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
                skoreString = $"{skore}";
                letiZrovnaText = false;
            }
        }

        private static void ZastavKoule()
        {
            foreach (Ball koule in Balls.balls)
            {
                koule.NastavRychlost(0);
            }
        }
        private static void ZastavAgresivniKoule()
        {
            foreach (Ball koule in Balls.ballsUtocne)
            {
                koule.NastavRychlost(0);
            }
        }

        private short ZpetnePlneni()
        {
            short bonus = 0;
            while (PlayBoard.NajdiOznacPrazdnou())
            {
                if (VyplnPrvniPole()) //oznaci kvyplneni
                {
                    if (!ZjistiKoliziPoleKoule())
                    {
                        foreach (Tile dlazdice in PlayBoard.tilesVnitrni)
                        {
                            if (dlazdice.Kvyplneni)
                            {
                                dlazdice.VyplnitZvyditelnit();
                                bonus += 1;
                            }
                        }
                    }
                    else
                    {
                        PlayBoard.VyprazdnitZneviditelnit();
                    }
                }
            }
            return bonus;
        }

        private bool VyplnPrvniPole()
        {
            if (NajdiPrvniDlazdici())
            {
                PlayBoard.VyplnPoleProOznaceni();
                return true;
            }
            else 
                return false; //{ throw new System.ArgumentException("dlazdice nenalezena"); }
        }

        private void ZpracujDruhePole()
        {
            PlayBoard.VyplnPoleProOznaceni();
            if (!ZjistiKoliziPoleKoule())
            {
                if (debug)
                {
                    foreach (Tile druhetiles in PlayBoard.tilesVnitrni)
                    {
                        if (druhetiles.Kvyplneni && !druhetiles.Projeta)
                        { druhetiles.OznacJakoDruhePole(true); }
                    }
                }
            }
            else OdznacDruhePole();
        }

        private static void OdznacDruhePole()
        {
            foreach (Tile dlazdice in PlayBoard.tilesVnitrni)
            {
                if (dlazdice.Kvyplneni)
                {
                    dlazdice.BudeKVyplneni(false);
                }
            }
        }

        private static void OdznacPrvniPole()
        {
            foreach (Tile dlazdice in PlayBoard.tilesVnitrni)
            {
                if (dlazdice.Kvyplneni && !dlazdice.Druha)
                {
                    dlazdice.BudeKVyplneni(false);
                }
            }
        }

        private short VyznacCestuVycistiSpocti()
        {
#if (debug)
            short i = 0;
#endif
            short soucet = 0;
            foreach (Tile dlazdice in PlayBoard.tilesVnitrni)
            {
                if (dlazdice.Projeta && !dlazdice.Plna)
                {
                    dlazdice.BudeKVyplneni(true);
#if (debug)
                    // if (dlazdice.plna) throw new System.Exception("podruhe vyplnujes vnitrni " + i);
                    // i += 1;
#endif
                }
                if (!debug)
                {
                    if (dlazdice.Prvni) 
                        dlazdice.Prvni = false;
                    else if (dlazdice.Druha) 
                        dlazdice.Druha = false;
                }
                if (dlazdice.Kvyplneni)
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
            for (int i = PlayBoard.tiles.Count - columns; i > columns; i--)
            {
                if (!PlayBoard.tiles[i].Okrajova && !PlayBoard.tiles[i].Plna && !PlayBoard.tiles[i].Projeta &&
                    (PlayBoard.tiles[i - 1].Projeta || PlayBoard.tiles[i + columns].Projeta || PlayBoard.tiles[i + 1].Projeta || PlayBoard.tiles[i - columns].Projeta))
                {
                    PlayBoard.tiles[i].BudeKVyplneni(true);
                    prvniNalezena = true;
                    if (debug)
                    {
                        PlayBoard.tiles[i].DebugDlazdice(1); // zvlastni texturu prvni dlazdici
                        // int j = i + 1;             // tohle chci pocitat od jedne
                        // int radek = j / columnsVnitrni;
                        // int sloupec = j / rowsVnitrni;
                        // debugPrvniDlazdice = $"Vybrana dlazdice: {i} radek: {radek} / {rows} sloupec: {sloupec} / {columns}";
                        debugText[2] = debugPrvniDlazdice;
                    }
                    else
                        PlayBoard.tiles[i].Prvni = true;

                    return prvniNalezena;
                }
            }
            return prvniNalezena;
        }

        private bool NajdiDruhouDlazdici()
        {
            for (int i = PlayBoard.tiles.Count - columns; i > columns; i--)
            {
                if (!PlayBoard.tiles[i].Kvyplneni && !PlayBoard.tiles[i].Okrajova && !PlayBoard.tiles[i].Plna && !PlayBoard.tiles[i].Projeta
                    && (PlayBoard.tiles[i + columns].Projeta || PlayBoard.tiles[i + 1].Projeta || PlayBoard.tiles[i - columns].Projeta || PlayBoard.tiles[i - 1].Projeta))
                {
                    PlayBoard.tiles[i].BudeKVyplneni(true);
                    if (debug)
                        PlayBoard.tiles[i].DebugDlazdice(2);
                    else
                        PlayBoard.tiles[i].Druha = true;

                    return true;
                }
            }

            return false;
        }

        private static bool ZjistiKoliziPoleKoule()
        {
            foreach (Tile tile in PlayBoard.tilesVnitrni) //!!! bude vic prazdnych prostoru
            {
                if (tile.Kvyplneni)
                {
                    foreach (Ball ball in Balls.balls)
                        if (ball.rect.Intersects(tile.drawRectangle))
                            return true;

                    foreach (Ball aball in Balls.ballsUtocne)
                        if (aball.rect.Intersects(tile.drawRectangle))
                            return true;
                }
            }

            return false;
        }

        private static ushort SectiPlneDlazdice()
        {
            ushort plnychDlazdic = 0;
            foreach (Tile tile in PlayBoard.tilesVnitrni) 
                if (tile.Plna) 
                    plnychDlazdic++;

            return plnychDlazdic;
        }

        private void PustUroven()
        {
            waitFrames = 60;
            Store.SaveScore();
            skore = 0; skoreString = string.Empty;
            staryState = gameState;
            gameState = Stavy.Play;
            byte staraepizoda = uroven.Epizoda;

            hrobSprite ??= Content.Load<Texture2D>(@"gfx/hrob");
            hrob = new Hrobecek(false, Rectangle.Empty, hrobSprite);

            StartGame();
            Texty.Clear();
            uroven.NastavUroven();
            if (Level.Bludiste)
            {
                PlayBoard.OdstranOkraje();
                zivotuString = string.Empty;
                //splashScreen.KresliSplash(false, "Level " + Level.cisloUrovne, false); -vypada blbe kdyz uz mam kolo postavene
            }
            uroven.PripravEpizodu();

            if (uroven.Epizoda != staraepizoda)
            {
                ZacniNovouEpizodu();
            }

            splashScreen.KresliSplash(true, $"{Level.EpizodaSplash}{System.Environment.NewLine}{System.Environment.NewLine}Level {uroven.CisloUrovne}", false);
            //NapisVelkouZpravu("Level " + Level.cisloUrovne, 7000, -9999, -9999, false, true);
            if (uroven.LevelText != null)
                procentaString = uroven.LevelText;

            if (uroven.ViteznychProcent != 0)
                procentProVitezstvi = uroven.ViteznychProcent;
            else
                procentProVitezstvi = 70;

            if (procentProVitezstvi == 70)
                potrebnychPlnych = (ushort)(PlayBoard.tilesVnitrni.Count / 1.42);
            else
                potrebnychPlnych = (ushort)(PlayBoard.tilesVnitrni.Count * procentProVitezstvi / 100 + 1);

            minBonus = (ushort)(PlayBoard.tilesVnitrni.Count / 8);

            if (music) NahrajHudbu();
            monstra.Clear();
            if (uroven.ZrodMonstrum)
                ZrodMonstrum(uroven.PoSmeru);

            Balls.Generate(easy, hard, rows, uroven);
        }

        private void ZacniNovouEpizodu()
        {
            if (uroven.Epizoda == 1) // pri pokracovani hry z uvodni obrazovky?
            {
                PlayBoard.VybarviOkraje(graphics, oknoHry, Barvy.vyblitaZelena, Color.Green);
            }
            else if (uroven.Epizoda == 2)
                PlayBoard.VybarviOkraje(graphics, oknoHry, Barvy.modra, Barvy.vyblitaModra);
            else if (uroven.Epizoda == 3)
                PlayBoard.VybarviOkraje(graphics, oknoHry, Barvy.oblibena, Barvy.vyblitaOblibena);
            else if (uroven.Epizoda == 4)
            {
                Intro();
            }
        }

        private void StartGame()
        {
            // if (debug) debugText[0] = "lives: " + zivoty;
            mFadeIncrement = 3;
            BuildTiles(columns, rows, tileSize);
            player = new Hrac(true, 4, tileSize, 0, tileSize * 2, oknoHry.Width, oknoHry.Height, hracsprite);
            zivoty++;
            zivotuString = $"{zivoty}";
            PlayBoard.PostavOkraje(oknoHry);
        }

        private void NapisVelkouZpravu(string inputString, short miliseconds, short x = -9999, short vyska = -9999,
            bool fadein = false, bool fadeout = false, Color color = new Color())
        {
            Vector2 poloha = font12.MeasureString(inputString);
            if (x == -9999)
                poloha.X = stred.X - poloha.X / 2;
            else 
                poloha.X = x;

            if (vyska == -9999)
                poloha.Y = stred.Y - poloha.Y / 2;
            else
                poloha.Y = vyska;

            byte alfa = fadein ? byte.MinValue : byte.MaxValue;
            if (color == new Color())
            {
                if (gameState == Stavy.Prohra || gameState == Stavy.Menu)
                    barvaZpravy = new Color(byte.MinValue, byte.MaxValue, 122); //0 0 0 - cerna
                else
                    barvaZpravy = new Color((byte)100, (byte)111, (byte)50, alfa);
            }
            else
                barvaZpravy = color;

            var zprava = new Zprava(poloha, inputString, barvaZpravy, miliseconds, fadein, fadeout, font12);
            ProjedZpravy(zprava);
        }
        private void NapisVelkouZpravu14(string inputString, short miliseconds, short X = -9999, short vyska = -9999,
            bool fadein = false, bool fadeout = false, Color color = new Color())
        {
            Vector2 poloha = font14.MeasureString(inputString);
            if (X == -9999)
                poloha.X = stred.X - poloha.X / 2;
            else
                poloha.X = X;

            if (vyska == -9999)
                poloha.Y = stred.Y;
            else
                poloha.Y = vyska;

            byte alfa = fadein ? byte.MinValue : byte.MaxValue;
            if (color == new Color())
            {
                if (gameState == Stavy.Prohra)
                    barvaZpravy = new Color(byte.MinValue, byte.MinValue, byte.MinValue, alfa); //0 0 0 - cerna
                else
                    barvaZpravy = new Color((byte)100, (byte)111, (byte)50, alfa);
            }
            else
                barvaZpravy = color;
            var zprava = new Zprava(poloha, inputString, barvaZpravy, miliseconds, fadein, fadeout, font14);
            ProjedZpravy(zprava);
        }

        private void ProjedZpravy(Zprava zprava)
        {
            for (int i = 0; i < Texty.Count; i++)
            {
                if (Texty[i].Hotova)
                {
                    Texty[i] = zprava;
                    return;
                }
            }
            Texty.Add(zprava);
        }

        //private void KresliVelkouZpravu()
        //{
        //    //foreach (KeyValuePair<Vector2, string> entry in Texty)
        //    //{
        //    //    if (trvaniVelkeZpravy > 0)
        //    //    {
        //    //        trvaniVelkeZpravy -= gameTime.ElapsedGameTime.Milliseconds;
        //    //        string text = entry.Value;
        //    //        if (gameState == Stavy.Prohra || gameState == Stavy.Menu)
        //    //            spriteBatch.DrawString(font, text, entry.Key,
        //    //                new Color(nulovyBajt, nulovyBajt, nulovyBajt, (byte)MathHelper.Clamp(mAlphaValue, 0, 255))); //0 0 0 - cerna
        //    //        else
        //    //            spriteBatch.DrawString(font, text, entry.Key,
        //    //            new Color((byte)100, (byte)111, (byte)50, (byte)MathHelper.Clamp(mAlphaValue, 0, 255)));
        //    //    }
        //    //    else Texty.Clear();
        //    //    return;
        //    //}
        //    foreach (Zprava zprava in Texty)
        //    {
        //        zprava.Draw(spriteBatch);
        //    }
        //}

        private bool BlahoprejSkore(short pricistSkore)
        {
            if (pricistSkore > minBonus)
            {
                NapisVelkouZpravu($"Awesome! {pricistSkore} squares", 3000, -9999, 100, false, true);
                bonus = (short)((pricistSkore - minBonus) * 2);
                if (sound)
                    ton2.Play();

                return true;
            }
            else
            {
                bonus = 0;
                if (sound) 
                    ton1.Play();
            }

            return false;
        }

        private void ZrodMonstrum(bool poSmeru)
        {
            if (spriteMonstra == null)
                spriteMonstra = Content.Load<Texture2D>(@"gfx/monstrum");

            var obdelnik = new Rectangle(0, 0, tileSize, tileSize);
            ushort maxX = windowWidth - tileSize;
            ushort maxY = (ushort)(windowHeight - tileSize);
            if (player.VychoziX < 100) 
                obdelnik.X = maxX;
            if (player.VychoziY < 100) 
                obdelnik.Y = maxY;
            monstra.Add(new Monster(tileSize, 2, obdelnik, spriteMonstra, poSmeru, maxX, maxY));
        }

        private void BeginPause(bool UserInitiated)
        {
            paused = true;
            pausedByUser = UserInitiated;
            staryState = gameState;
            gameState = Stavy.Pause;
            stara = MediaPlayer.Queue.ActiveSong;
            MediaPlayer.Pause();
            //TODO: Pause controller vibration
        }

        private void EndPause()
        {
            //TODO: Resume controller vibration
            if (MediaPlayer.Queue.ActiveSong.Name == levelwon.Name)
                MediaPlayer.Stop();
            else if (MediaPlayer.State == MediaState.Paused)
                MediaPlayer.Resume();
            else
                MediaPlayer.Play(stara);

            pausedByUser = paused = pristeUzNekreslim = options = false;
            if (staryState == Stavy.Menu) gameState = Stavy.Menu;
            else if (staryState == Stavy.Play) gameState = Stavy.Play;
            else if (staryState == Stavy.Prohra) gameState = Stavy.Prohra;
            else if (staryState == Stavy.Animace) gameState = Stavy.Animace;
            else if (staryState == Stavy.Pause) gameState = Stavy.Play;
            else { gameState = Stavy.Vitez; }
        }

        private void CheckPauseKey(GamePadState gamePadState)
        {
            pauseKeyDownThisFrame = gamePadState.Buttons.Back == ButtonState.Pressed
                            || keys.IsKeyDown(Keys.Pause) || keys.IsKeyDown(Keys.Escape);

            if (pauseKeyDownThisFrame)
            {
                // If key was not down before, but is down now, toggle the pause setting
                if (!pauseKeyDown)
                {
                    if (!paused)
                        BeginPause(true);
                    else
                        EndPause();  
                }                
            }

            pauseKeyDown = pauseKeyDownThisFrame;
        }
    }
}