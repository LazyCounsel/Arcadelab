using System;
using System.Numerics;
using ZeroElectric.Vinculum;
using static System.Formats.Asn1.AsnWriter;


public unsafe static class ArcadeLab
{
    public static int Main()
    {
        bool radar = false; 
        bool playarkanoid = false;
        bool playsnake = false;
        bool playasteroids = false;
        bool KeyA = false;
        bool KeyB = false;
        bool KeyC = false;
        string message = "Привет, мир!";

        const int screenWidth = 1280;
        const int screenHeight = 720;

        Raylib.InitWindow(screenWidth, screenHeight, "3Д ДЕМО ЛАБИРИНТ АРКАД");
        Raylib.SetTargetFPS(60);
        Raylib.ToggleFullscreen();  //разкомментировать если надо полный экран

        Camera3D camera = new(new(0.2f, 0.4f, 0.2f), new(0.0f, 0.0f, 0.0f), new(0.0f, 1.0f, 0.0f), 45.0f, 0);
        Raylib.HideCursor();


        Image imMap = Raylib.LoadImage("resources/levelmap.png");      // рам
        Texture cubicmap = Raylib.LoadTextureFromImage(imMap);       // ->врам
        Mesh mesh = Raylib.GenMeshCubicmap(imMap, new(1.0f, 1.0f, 1.0f));
        Model model = Raylib.LoadModelFromMesh(mesh);
        Texture texture = Raylib.LoadTexture("resources/maintexture.png");
        model.materials[0].maps[(int)Raylib.MATERIAL_MAP_DIFFUSE].texture = texture; 


        Color* mapPixels = Raylib.LoadImageColors(imMap); //пиксель на кол
        Raylib.UnloadImage(imMap);

        Vector3 mapPosition = new(-16.0f, 0.0f, -8.0f);

        // МГЛ
        while (!Raylib.WindowShouldClose())
        {
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_M))
            {
                if (radar == false)
                {
                    radar = true;
                }
                else
                {
                    radar = false;
                }

            }

            Vector3 oldCamPos = camera.position;    // старая позиция для отброса на коллизиях
            if ((!playarkanoid)&&(!playsnake)&&(!playasteroids)) { Raylib.UpdateCamera(ref camera, CameraMode.CAMERA_FIRST_PERSON); } 

            Raylib.SetMousePosition(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2);

            Vector2 playerPos = new(camera.position.X, camera.position.Z);
            float playerRadius = 0.2f;  // толщина тушки "игрока"

            int playerCellX = (int)(playerPos.X - mapPosition.X + 0.5f);
            int playerCellY = (int)(playerPos.Y - mapPosition.Z + 0.5f);

            if (playerCellX < 0) playerCellX = 0;
            else if (playerCellX >= cubicmap.width) playerCellX = cubicmap.width - 1; //это чтобы игрок не пошел гулять за карту

            if (playerCellY < 0) playerCellY = 0;
            else if (playerCellY >= cubicmap.height) playerCellY = cubicmap.height - 1;

            for (int y = 0; y < cubicmap.height; y++)
            {
                for (int x = 0; x < cubicmap.width; x++)
                {
                    if ((mapPixels[y * cubicmap.width + x].r == 255) &&       // только эРка
                        (Raylib.CheckCollisionCircleRec(playerPos, playerRadius,
                        new Rectangle(mapPosition.X - 0.5f + x * 1.0f, mapPosition.Z - 0.5f + y * 1.0f, 1.0f, 1.0f))))
                    {
                        camera.position = oldCamPos;
                    }
                }
            }

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.RAYWHITE);

            Raylib.BeginMode3D(camera);
            Raylib.DrawModel(model, mapPosition, 1.0f, Raylib.WHITE);// карта

            if (!KeyB)
            {
                Raylib.DrawCylinder(new Vector3(33.0f, 0.8f, 36.0f), 0.5f, 0.5f, 0.2f, 16, Raylib.WHITE);
                Raylib.DrawCylinder(new Vector3(33.0f, 0.2f, 36.0f), 0.5f, 0.5f, 0.6f, 16, Raylib.BLUE);
                Raylib.DrawCylinder(new Vector3(33.0f, 0.0f, 36.0f), 0.5f, 0.5f, 0.2f, 16, Raylib.RED);
            }

            if (!KeyC)
            {
                Raylib.DrawCylinder(new Vector3(12.0f, 0.8f, 0.0f), 0.5f, 0.5f, 0.2f, 16, Raylib.WHITE);
                Raylib.DrawCylinder(new Vector3(12.0f, 0.2f, 0.0f), 0.5f, 0.5f, 0.6f, 16, Raylib.BLUE);
                Raylib.DrawCylinder(new Vector3(12.0f, 0.0f, 0.0f), 0.5f, 0.5f, 0.2f, 16, Raylib.RED);
            }

            if (!KeyA)
            {
                Raylib.DrawCylinder(new Vector3(40.0f, 0.8f, 17.0f), 0.5f, 0.5f, 0.2f, 16, Raylib.WHITE);
                Raylib.DrawCylinder(new Vector3(40.0f, 0.2f, 17.0f), 0.5f, 0.5f, 0.6f, 16, Raylib.BLUE);
                Raylib.DrawCylinder(new Vector3(40.0f, 0.0f, 17.0f), 0.5f, 0.5f, 0.2f, 16, Raylib.RED);
            }

            Raylib.DrawCylinder(new Vector3(-2.0f, 0.95f, -8.0f), 0.5f, 0.5f, 0.05f, 16, Raylib.RAYWHITE);
            Raylib.DrawCylinder(new Vector3(-2.0f, 0.0f, -8.0f), 0.5f, 0.5f, 0.05f, 16, Raylib.RAYWHITE);

            Raylib.EndMode3D();

            if (Raylib.CheckCollisionCircles(playerPos, playerRadius, new Vector2(-2.0f, -8.0f), 0.5f))
            {
                if ((!KeyA)||(!KeyB)||(!KeyC)) {Raylib.DrawText($"You need keys", 600, 300, 40, Raylib.DARKGREEN); }
                if ((KeyA) && (KeyB) && (KeyC)) { Raylib.DrawText($"A winner is you, press ENTER to exit maze", 150, 300, 40, Raylib.DARKGREEN); }
                if ((KeyA) && (KeyB) && (KeyC)&& (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))) { return 0; }
            }

            if (radar == true)
            {
                Raylib.DrawTextureEx(cubicmap, new(Raylib.GetScreenWidth() - cubicmap.width * 4.0f - 20, 20.0f), 0.0f, 4.0f, Raylib.WHITE);
                Raylib.DrawRectangleLines(Raylib.GetScreenWidth() - cubicmap.width * 4 - 20, 20, cubicmap.width * 4, cubicmap.height * 4, Raylib.GREEN);
                Raylib.DrawRectangle(Raylib.GetScreenWidth() - cubicmap.width * 4 - 20 + playerCellX * 4, 20 + playerCellY * 4, 4, 4, Raylib.RED);
            }

            if (playarkanoid == true)
            {
                Arkanoid.ArkanoidGame.PlayArkanoid();
                if (Arkanoid.ArkanoidGame.getBall() > 720)
                {
                    playarkanoid = false;
                    Arkanoid.ArkanoidGame.setStartArkanoid();
                }
                if (Arkanoid.ArkanoidGame.getscore()>=16)
                {
                    KeyC = true;
                    playarkanoid = false;
                }
            }

            if (playasteroids == true)
            {
                Asteroids.Asteroids.PlayAsteroid();
                if (Asteroids.Asteroids.isGameOver()) { playasteroids = false; }
                if (Asteroids.Asteroids.getscore() == 5000) {
                    KeyA = true;
                    playasteroids = false;
                }
            }

            if (playsnake == true)
            {
                Snake.Snake.PlaySnake();
                if (Snake.Snake.getscore()>=2000)
                {
                    KeyB = true;
                    playsnake = false;
                    Raylib.SetTargetFPS(60);

                }
                if (Snake.Snake.getGameOver())
                {
                    playsnake = false;
                    Snake.Snake.init();
                }
            }



            if (Raylib.CheckCollisionCircles(playerPos, playerRadius, new Vector2(33.0f, 36.0f), 0.5f))
            {
                if ((playsnake == false)&&(!KeyB)) { Raylib.DrawText($"Press ENTER to play", 400, 300, 20, Raylib.WHITE); }
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
                {
                    if (playsnake == false)
                    {
                        playsnake = true;
                        Raylib.SetTargetFPS(10);

                    }

                }
            }

            if (Raylib.CheckCollisionCircles(playerPos, playerRadius, new Vector2(12.0f, 0.0f), 0.5f))
            {
                if ((playarkanoid == false)&&(!KeyC)) { Raylib.DrawText($"Press ENTER to play", 400, 300, 20, Raylib.WHITE); }
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
                {
                    if (playarkanoid == false)
                    {
                        playarkanoid = true;
                        Raylib.SetTargetFPS(60);

                    }

                }
            }


            if (Raylib.CheckCollisionCircles(playerPos, playerRadius, new Vector2(40.0f, 17.0f), 0.5f))
            {
                if ((playasteroids == false)&&(!KeyA)) { Raylib.DrawText($"Press ENTER to play", 400, 300, 20, Raylib.WHITE); }
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
                {
                    if (playasteroids == false)
                    {
                        Asteroids.Asteroids.ResetButton();
                        playasteroids = true;
                        Raylib.SetTargetFPS(60);

                    }

                }
            }

            if ((!playarkanoid) && (!playsnake) && (!playasteroids))
            {
                if (KeyA) { Raylib.DrawRectangle(1200, 640,20,40,Raylib.YELLOW); }
                if (KeyB) { Raylib.DrawRectangle(1150, 640, 20, 40, Raylib.GREEN); }
                if (KeyC) { Raylib.DrawRectangle(1100, 640, 20, 40, Raylib.SKYBLUE); }
            }


            Raylib.EndDrawing();

        }


        Raylib.UnloadTexture(cubicmap);        // Очистка памяти
        Raylib.UnloadImageColors(mapPixels);   // 
        Raylib.UnloadTexture(texture);         // 
        Raylib.UnloadModel(model);             // 

        Raylib.CloseWindow();                  

        return 0;
    }
}