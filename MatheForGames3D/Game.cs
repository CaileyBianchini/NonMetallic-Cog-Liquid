using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;
using MathLibrary;

namespace MatheForGames3D
{
    class Game
    {
        private static bool _gameOver;
        private Camera3D _camera = new Camera3D();

        public static bool GameOver
        {
            get { return _gameOver; }
            set { _gameOver = value; }
        }

        private void Start()
        {
            Raylib.InitWindow(1920, 1080, "Math For Games 3D");
            Raylib.SetTargetFPS(30);
            Console.Title = "Math For Games";
            _camera.position = new System.Numerics.Vector3(0.0f, 10.0f, 10.0f); //camera position
            _camera.target = new System.Numerics.Vector3(0.0f, 0.0f, 0.0f);     //camera target at point
            _camera.up = new System.Numerics.Vector3(0.0f, 1.0f, 10.0f);        //camera up vector
            _camera.fovy = 45.0f;                                               //Camera feild-of-view Y
            _camera.type = CameraType.CAMERA_PERSPECTIVE;                       //Camera mode type
        }

        private void Update()
        {

        }

        private void Draw()
        {
            Raylib.BeginDrawing();
            Raylib.BeginMode3D(_camera);

            Raylib.ClearBackground(Color.RAYWHITE);

            Raylib.DrawGrid(10, 1.0f);

            Raylib.EndMode2D();
            Raylib.EndDrawing();
        }

        private void End()
        {

        }

        public void Run()
        {
            Start();
            while(!_gameOver)
            {
                Update();
                Draw();
            }
            End();
        }
    }
}
