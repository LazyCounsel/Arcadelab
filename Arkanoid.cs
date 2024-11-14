using System;
using System.Numerics;
using ZeroElectric.Vinculum;
using static System.Formats.Asn1.AsnWriter;

namespace Arkanoid
{
    public class ArkanoidGame
    {
        private static int x = 600;
        private static int score = 0;
        private static int ballX = 600;
        private static int ballY = 500;
        private static int ballSpeedX = 5;
        private static int ballSpeedY = -5;
        private static bool[] blocks = new bool[16] { true, true, true, true, true, true, true, true,
                                    true, true, true, true, true, true, true, true };
        private static int[] blockX = new int[16] { 200, 400, 600, 800, 1000, 100, 300, 500,
                                    700, 900, 1100, 200, 400, 600, 800, 1000 };
        private static int[] blockY = new int[16] { 100, 100, 100, 100, 100, 200, 200, 200,
                                    200, 200, 200, 300, 300, 300, 300, 300 };

        public static int getscore()
        {
            return score;
        }
        public static void PlayArkanoid()
        {
            Raylib.DrawRectangle(40, 40, 1200, 640, Raylib.BLACK);
            Raylib.DrawRectangle(x, 600, 150, 30, Raylib.WHITE);
            if (ballY < 670) { Raylib.DrawCircle(ballX, ballY, 10, Raylib.RED); }

            if (ballY < 350)
            {
                for (int i = 0; i <= 15; i++)
                {
                    if ((blocks[i] == true) && (Raylib.CheckCollisionCircleRec(new Vector2(ballX, ballY), 10, new Rectangle(blockX[i], blockY[i], 100, 30))))
                    {
                        if ((ballSpeedX > 0) && (ballX < blockX[i])) { ballSpeedX *= -1; }
                        if ((ballSpeedX < 0) && (ballX > blockX[i] + 100)) { ballSpeedX *= -1; }
                        if ((ballSpeedY > 0) && (ballY < blockY[i])) { ballSpeedY *= -1; }
                        if ((ballSpeedY < 0) && (ballY > blockY[i] + 30)) { ballSpeedY *= -1; }

                        blocks[i] = false;
                        score++;
                    }
                }
            }

            for (int i = 0; i <= 15; i++)
            {
                if (blocks[i] == true) 
                { 
                    Raylib.DrawRectangleGradientH(blockX[i], blockY[i], 100, 30, Raylib.BEIGE, Raylib.GOLD);
                    Raylib.DrawRectangleLines(blockX[i], blockY[i], 100, 30, Raylib.WHITE);
                }
            }

            if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
            {
                if (x > 60) {
                    x -= 6;

                }
            }

            if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
            {
                if (x < 1070)
                {
                    x += 6;
                }
            }

            if (ballX >= 1240) { ballSpeedX = ballSpeedX * (-1); }
            if (ballX <= 40) { ballSpeedX = ballSpeedX * (-1); }
            if (ballY <= 40) { ballSpeedY = ballSpeedY * (-1); }
            if ((ballY == 600) && (ballX >= x) && (ballX <= x + 150))
            {
                if ((ballX >= x) && (ballX <= x + 50) && (ballSpeedX < 0)) { ballSpeedX--; ballSpeedY--; }
                if ((ballX >= x) && (ballX <= x + 50) && (ballSpeedX > 0)) { ballSpeedX--; ballSpeedY++; }
                if ((ballX >= x + 100) && (ballX <= x + 150) && (ballSpeedX < 0)) { ballSpeedX++; ballSpeedY--; }
                if ((ballX >= x + 100) && (ballX <= x + 150) && (ballSpeedX > 0)) { ballSpeedX++; ballSpeedY++; }


                ballSpeedY = ballSpeedY * (-1);
            }
            ballX += ballSpeedX;
            ballY += ballSpeedY;

            if (ballY > 680) { Raylib.DrawRectangle(40, 40, 1200, 640, Raylib.RED); }

        }

        public static void setStartArkanoid()
        {
            ballX = 600;
            ballY = 500;
            ballSpeedX = 5;
            ballSpeedY = -5;
            blocks = [true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true];
            score = 0;
        }


        public static int getBall()
        {
            return ballY;
        }
    }
}
