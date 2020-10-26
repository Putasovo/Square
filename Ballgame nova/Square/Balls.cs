using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Square
{
    public static class Balls
    {
        private static readonly Random rand = new Random();
        private static SoundEffect respawnball, kolize, odraz, zpomalit;
        private const bool rigid = true;
        private static short numBalls, numAttackBalls;
        private static Vector2 balLoc = new Vector2(222, 222);
        private static Vector2 ballVelocity;
        public static Texture2D ballSprite { get; private set ;}
        
        public static readonly List<Ball> balls = new List<Ball>(8);
        public static readonly List<Ball> ballsUtocne = new List<Ball>(4);
        public static readonly List<Ball> ballsAll = new List<Ball>(16);

        public static void SpawnBalls(int X = -1, int Y = -1)
        {
            if (balls.Count < numBalls)
            {
                balLoc.X = X == -1 ? balLoc.X = rand.Next(64, PlayBoard.TexOkrajeH.Width) : X;
                balLoc.Y = Y == -1 ? balLoc.Y = rand.Next(64, PlayBoard.TexOkrajeV.Height - 64) : Y;

                balls.Add(new Ball(balLoc, ballVelocity, PlayBoard.Sloupcu * PlayBoard.borderSize, PlayBoard.TexOkrajeV.Height,
                    rigid, false, false, false, false, Level.Bludiste, respawnball, kolize, odraz));
            }
        }

        private static void SpawnBallsUtocne(int X = -1, int Y = -1)
        {
            if (ballsUtocne.Count < numAttackBalls)
            {
                balLoc.X = X == -1 ? balLoc.X = rand.Next(64, PlayBoard.TexOkrajeH.Width) : X;
                balLoc.Y = Y == -1 ? balLoc.Y = rand.Next(64, PlayBoard.TexOkrajeV.Height - 64) : Y;

                byte levych = byte.MinValue, pravych = byte.MinValue, dolnich = byte.MinValue, hornich = byte.MinValue;
                foreach (Ball ball in ballsUtocne)
                {
                    if (ball.UtocnaDolni)
                        dolnich++;
                    else if (ball.UtocnaHorni)
                        hornich++;
                    else if (ball.UtocnaLeva)
                        levych++;
                    else
                        pravych++;
                }

                if (Level.numUtocnychBallsLeft > levych)
                {
                    SpawnBallLeft();
                }
                else if (Level.numUtocnychBallsRight > pravych)
                {
                    SpawnBallRight();
                }
                else if (Level.numUtocnychBallsUp > hornich)
                {
                    SpawnBallUp();
                }
                else if (Level.numUtocnychBallsDown > dolnich)
                {
                    SpawnBallDown();
                }
                else
                    SpawnRandomAttackBall();
            }
        }

        private static void SpawnRandomAttackBall()
        {
            bool leva = false; bool prava = false; bool nahoru = false; bool dolu = false;
            while (!leva && !prava && !nahoru && !dolu)
            {
                int i = rand.Next(0, 4);
                {
                    if (i == 0)
                        leva = rand.NextDouble() >= 0.5;
                    else if (i == 1)
                        prava = rand.NextDouble() >= 0.5;
                    else if (i == 2)
                        nahoru = rand.NextDouble() >= 0.5;
                    else
                        dolu = rand.NextDouble() >= 0.5;
                }
            }

            ballsUtocne.Add(new Ball(balLoc, ballVelocity, PlayBoard.borderSize * PlayBoard.Sloupcu, PlayBoard.TexOkrajeV.Height,
                    rigid, leva, prava, nahoru, dolu, Level.Bludiste, respawnball, kolize, odraz));
        }

        private static void SpawnBallDown()
        {
            byte nalezena = 0;
            foreach (Ball ball in ballsUtocne)
                if (ball.UtocnaDolni)
                    nalezena++;

            if (nalezena != Level.numUtocnychBallsDown)
            {
                ballsUtocne.Add(new Ball(balLoc, ballVelocity, PlayBoard.borderSize * PlayBoard.Sloupcu, PlayBoard.TexOkrajeV.Height,
                    rigid, false, false, false, true, Level.Bludiste, respawnball, kolize, odraz));
            }
        }

        private static void SpawnBallUp()
        {
            byte nalezena = 0;
            foreach (Ball ball in ballsUtocne)
                if (ball.UtocnaHorni)
                    nalezena++;

            if (nalezena != Level.numUtocnychBallsUp)
            {
                ballsUtocne.Add(new Ball(balLoc, ballVelocity, PlayBoard.borderSize * PlayBoard.Sloupcu, PlayBoard.TexOkrajeV.Height,
                    rigid, false, true, false, false, Level.Bludiste, respawnball, kolize, odraz));
            }
        }

        private static void SpawnBallRight()
        {
            byte nalezena = 0;
            foreach (Ball ball in ballsUtocne)
                if (ball.UtocnaPrava)
                    nalezena++;

            if (nalezena != Level.numUtocnychBallsRight)
            {
                ballsUtocne.Add(new Ball(balLoc, ballVelocity, PlayBoard.borderSize * PlayBoard.Sloupcu, PlayBoard.TexOkrajeV.Height,
                    rigid, false, false, true, false, Level.Bludiste, respawnball, kolize, odraz));
            }
        }

        private static void SpawnBallLeft()
        {
            byte nalezena = 0;
            foreach (Ball ball in ballsUtocne)
                if (ball.UtocnaLeva)
                    nalezena++;

            if (nalezena < Level.numUtocnychBallsLeft)
            {
                ballsUtocne.Add(new Ball(balLoc, ballVelocity, PlayBoard.borderSize * PlayBoard.Sloupcu, PlayBoard.TexOkrajeV.Height,
                    rigid, true, false, false, false, Level.Bludiste, respawnball, kolize, odraz));
            }
        }

        public static void LoadContent(ContentManager content)
        {
            respawnball = content.Load<SoundEffect>(@"audio/ozivKouli");
            kolize = content.Load<SoundEffect>(@"audio/lost");
            odraz = content.Load<SoundEffect>(@"audio/odraz");
            zpomalit = content.Load<SoundEffect>(@"audio/zpomalit");
        }

        public static void SpawnWithVelocity(Vector2 velocity)
        {
            numBalls++;
            ballVelocity = velocity;
            SpawnBalls();
        }

        public static void Generate(bool easy, bool hard, ushort rows, Level uroven)
        {
            balls.Clear(); ballsUtocne.Clear();
            numBalls = Level.GetNumBalls();
            numAttackBalls = Level.GetNumAttackBalls();
            if (easy)
                ballVelocity = new Vector2(1.4f, 1.4f);
            else if (hard)
                ballVelocity = new Vector2(2.0f, 2.0f);
            else
                ballVelocity = new Vector2(1.6f, 1.6f);

            // if (!soft)
            ballVelocity *= rows / 2;

            foreach (Point pozice in uroven.poziceKouli)
                SpawnBalls(pozice.X * ballSprite.Height, pozice.Y * ballSprite.Height);
            foreach (Point pozice in uroven.poziceUtocnychKouli)
                SpawnBallsUtocne(pozice.X * ballSprite.Height, pozice.Y * ballSprite.Height);

            //pak muzu vlozit nahodne polohy
            for (short i = (short)(numBalls - balls.Count); i > 0; i--)
                SpawnBalls();

            for (short i = (short)(numAttackBalls - ballsUtocne.Count); i > 0; i--)
                Balls.SpawnBallsUtocne();

            ballsAll.AddRange(balls);
            ballsAll.AddRange(ballsUtocne);
            if (uroven.BezOdchylky)
                foreach (Ball ball in ballsAll)
                    ball.NastavOdchylku(0f);
        }

        public static void LoadSprite(ContentManager content)
        {
            ballSprite = content.Load<Texture2D>(@"gfx/ball");
        }

        internal static void ZrusSrazkuKouli()
        {
            foreach (Ball ball in balls)
            {
                ball.ZrusSrazku();
            }
        }

        internal static void OzivKouli(int indexDlazdice)
        {
            foreach (Ball ball in balls)
            {
                if (!ball.Cinna)
                {
                    ball.Obzivni();
                    break;
                }
            }

            if (PlayBoard.tiles[indexDlazdice].ozivovaci)
            {
                foreach (Ball ball in ballsUtocne)
                {
                    if (!ball.Cinna)
                    {
                        ball.Obzivni();
                        break;
                    }
                }
            }

            PlayBoard.tiles[indexDlazdice].NastavOzivovaci(false);
        }

        public static void NastavRychlostKouli(float nasobic, bool zvukZpomaleni)
        {
            if (zvukZpomaleni)
                zpomalit.Play();

            if (Level.ZpomalovatUtocne)
                foreach (Ball ball in ballsUtocne)
                {
                    ball.NasobRychlost(nasobic);
                    //Zprava nova = new Zprava(stred, "Speed Altered", Color.Red, 4444, true, true, font);
                }
            else
                foreach (Ball ball in balls)
                {
                    ball.NasobRychlost(nasobic);
                    //Zprava nova = new Zprava(stred, "Speed Altered", Color.Red, 4444, true, true, font);
                }
        }
    }
}
