using System;
using System.Numerics;
using ZeroElectric.Vinculum;

namespace Snake
{
    public class Snake
    {
        const int CELL_SIZE = 20;

        static List<Vector2> snake = new List<Vector2>();
        static Vector2 food;
        static Vector2 direction = new Vector2(1, 0);
        static bool gameOver = false;
        static bool ini = false;
        static int score = 0;

        public static int getscore()
        {
            return score;
        }

        public static bool getGameOver()
        {
            return gameOver;
        }

        public static void init()
        {
            ini = false;
        }


        public static void PlaySnake()
        {
            if (ini == false) { InitializeGame(); }
            Update();
            Draw();

        }

        static void InitializeGame()
        {
            snake.Clear();
            score = 0;
            Raylib.SetTargetFPS(10);
            snake.Add(new Vector2(1240 / 2 / CELL_SIZE * CELL_SIZE, 680 / 2 / CELL_SIZE * CELL_SIZE));
            GenerateFood();
            gameOver = false;
            ini = true;
        }

        static void GenerateFood()
        {
            Random rand = new Random();
            food = new Vector2(rand.Next(40 / CELL_SIZE, 1240 / CELL_SIZE) * CELL_SIZE, rand.Next(40 / CELL_SIZE, 680 / CELL_SIZE) * CELL_SIZE);
        }

        static void Update()
        {

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_W) && direction.Y == 0)
                direction = new Vector2(0, -1);
            else if (Raylib.IsKeyPressed(KeyboardKey.KEY_S) && direction.Y == 0)
                direction = new Vector2(0, 1);
            else if (Raylib.IsKeyPressed(KeyboardKey.KEY_A) && direction.X == 0)
                direction = new Vector2(-1, 0);
            else if (Raylib.IsKeyPressed(KeyboardKey.KEY_D) && direction.X == 0)
                direction = new Vector2(1, 0);

            Vector2 head = snake[0];
            head += direction * CELL_SIZE;

            if (head.X < 40 || head.X >= 1240 || head.Y < 40 || head.Y >= 680 || snake.Contains(head)) //стенки
            {
                gameOver = true;
                return;
            }

            if (head == food) // вкусноесть
            {
                snake.Insert(0, head);
                score += 100;
                GenerateFood();
            }
            else
            {
                snake.Insert(0, head);
                snake.RemoveAt(snake.Count - 1);
            }
        }

        static void Draw()
        {
            Raylib.DrawRectangle(40, 40, 1200, 640, Raylib.BLACK);
            Raylib.DrawText($"Score: {score}", 60, 60, 20, Raylib.WHITE);

            foreach (var segment in snake)
            {
                Raylib.DrawRectangle((int)segment.X, (int)segment.Y, CELL_SIZE, CELL_SIZE, Raylib.GREEN);
            }

            Raylib.DrawRectangle((int)food.X, (int)food.Y, CELL_SIZE, CELL_SIZE, Raylib.RED);

            if (gameOver)
            {
                Raylib.SetTargetFPS(60);
            }

        }
    }

}
