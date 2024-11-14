using System;
using System.Collections.Generic;
using System.Numerics;
using ZeroElectric.Vinculum;

namespace Asteroids {

    public class Asteroids
    {
        private static Vector2 Ship1 = new Vector2(640, 600);
        private static Vector2 Ship2 = new Vector2(620, 640);
        private static Vector2 Ship3 = new Vector2(660, 640);
        private static Vector2 Shot = new Vector2(0, 0);
        private static Vector2[] stars = new Vector2[79];
        private static Vector2[] asteroids = new Vector2[19];
        private static bool reload = false;
        private static int score = 0;
        private static bool Gameover = false;
        private static bool ini = false;
        public static Random rand = new Random();

        public static bool isGameOver()
        {
            return Gameover;
        }

        public static void ResetButton()
        {
            Gameover=false;
        }

        public static int getscore()
        {
            return score;
        }

        public static void PlayAsteroid()
        {


            if (ini == false)
            {

                for (int i = 0; i < stars.Length; i++)
                {
                    float x = (float)rand.Next(40, 1500);
                    float y = (float)rand.Next(-200, 40);
                    stars[i] = new Vector2(x, y);
                }

                for (int i = 0; i < asteroids.Length; i++)
                {
                    float x = (float)rand.Next(40, 1500);
                    float y = (float)rand.Next(-800, 40);
                    asteroids[i] = new Vector2(x, y);
                }

                ini = true;
            }

            Raylib.DrawRectangle(20, 10, 1240, 700, Raylib.BLACK);
            Raylib.DrawText($"Score: {score}", 60, 60, 20, Raylib.WHITE);
            if (Gameover) { ini = false; }
            if (Gameover) { Raylib.WaitTime(2.0); }

            if (Raylib.IsKeyDown(KeyboardKey.KEY_A)&&(Ship1.X>60))
            {
                Ship1.X -= 5;
                Ship2.X -= 5;
                Ship3.X -= 5;
            }


            if (Raylib.IsKeyDown(KeyboardKey.KEY_D)&&(Ship1.X < 1200))
            {
                Ship1.X += 5;
                Ship2.X += 5;
                Ship3.X += 5;
            }

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_W) && (reload == false))
            {
                Shot.X = Ship1.X;
                Shot.Y = Ship1.Y + 10;
                reload = true;
            }

            for (int i = 0; i < stars.Length; i++)
             {
                if (stars[i].Y > 720)
                {
                    float x = (float)rand.Next(40, 1500);
                    float y = (float)rand.Next(-400, 40);
                    stars[i] = new Vector2(x, y);
                }
                stars[i].X -= 3;
                stars[i].Y += 8;
            }

            for (int i = 0; i < asteroids.Length; i++)
            {
                if (asteroids[i].Y > 720)
                {
                    float x = (float)rand.Next(50, 1190);
                    float y = (float)rand.Next(-400, 40);
                    asteroids[i] = new Vector2(x, y);
                }
                asteroids[i].Y += 4;
            }

            for (int i = 0; i < stars.Length; i++)
            {
                if ((stars[i].Y > 40) && (stars[i].Y < 680) &&
                    (stars[i].X < 1240) && (stars[i].X > 40))
                {
                    if (i % 3 == 1) { Raylib.DrawPixel((int)stars[i].X, (int)stars[i].Y, Raylib.RAYWHITE); }
                    else { Raylib.DrawPixel((int)stars[i].X, (int)stars[i].Y, Raylib.VIOLET); }
                }

            }

            for (int i = 0; i < asteroids.Length; i++)
            {
                if ((asteroids[i].Y > 40) && (asteroids[i].Y < 680) &&
                    (asteroids[i].X < 1240) && (asteroids[i].X > 40))
                {
                    Raylib.DrawCircleGradient((int)asteroids[i].X, (int)asteroids[i].Y, 30, Raylib.DARKBROWN, Raylib.DARKGRAY);
                }

            }

            if (reload == true)
            {
                if (Shot.Y < 40) { reload = false; }
                Shot.Y += -6;

                Raylib.DrawCircle((int)Shot.X, (int)Shot.Y, 2, Raylib.RED);

                for (int i = 0; i < asteroids.Length; i++)
                {
                    if (Raylib.CheckCollisionPointCircle(Shot, asteroids[i], 30))
                    {
                        asteroids[i].Y = 900;
                        score += 100;
                        reload = false;
                    }
                }

            }

            for (int i = 0; i < asteroids.Length; i++)
            {
                if (Raylib.CheckCollisionPointCircle(new Vector2(Ship1.X, Ship1.Y + 10), asteroids[i], 30))
                {
                    Raylib.DrawRectangle(40, 40, 1200, 680, Raylib.RED);
                    Gameover = true;
                    ini = false;
                }
            }

            Raylib.DrawTriangle(Ship1, Ship2, Ship3, Raylib.WHITE);
    


        }
    }

}