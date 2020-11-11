using System;
using System.Collections.Generic;
using System.Text;
using MathLibrary;
using Raylib_cs;

namespace MathForGames3D
{
    class Game
    {
        private static bool _gameOver;
        private Camera3D _camera = new Camera3D();
        private static Scene[] _scenes;
        private static int _currentSceneIndex;


        public static int CurrentSceneIndex
        {
            get { return _currentSceneIndex; }
        }

        public static ConsoleColor DefaultColor { get; set; } = ConsoleColor.White;

        /// <summary>
        /// Used to set the value of game over.
        /// </summary>
        /// <param name="value">If this value is true, the game will end</param>
        public static void SetGameOver(bool value)
        {
            _gameOver = value;
        }


        /// <summary>
        /// Returns the scene at the index given.
        /// Returns an empty scene if the index is out of bounds
        /// </summary>
        /// <param name="index">The index of the desired scene</param>
        /// <returns></returns>
        public static Scene GetScene(int index)
        {
            if (index < 0 || index >= _scenes.Length)
                return new Scene();

            return _scenes[index];
        }

        public static Scene CurrentScene
        {
            get { return _scenes[_currentSceneIndex]; }
        }

        /// <summary>
        /// Adds the given scene to the array of scenes.
        /// </summary>
        /// <param name="scene">The scene that will be added to the array</param>
        /// <returns>The index the scene was placed at. Returns -1 if
        /// the scene is null</returns>
        public static int AddScene(Scene scene)
        {
            //If the scene is null then return before running any other logic
            if (scene == null)
                return -1;

            //Create a new temporary array that one size larger than the original
            Scene[] tempArray = new Scene[_scenes.Length + 1];

            //Copy values from old array into new array
            for (int i = 0; i < _scenes.Length; i++)
            {
                tempArray[i] = _scenes[i];
            }

            //Store the current index
            int index = _scenes.Length;

            //Sets the scene at the new index to be the scene passed in
            tempArray[index] = scene;

            //Set the old array to the tmeporary array
            _scenes = tempArray;

            return index;
        }

        /// <summary>
        /// Finds the instance of the scene given that inside of the array
        /// and removes it
        /// </summary>
        /// <param name="scene">The scene that will be removed</param>
        /// <returns>If the scene was successfully removed</returns>
        public static bool RemoveScene(Scene scene)
        {
            //If the scene is null then return before running any other logic
            if (scene == null)
                return false;

            bool sceneRemoved = false;

            //Create a new temporary array that is one less than our original array
            Scene[] tempArray = new Scene[_scenes.Length - 1];

            //Copy all scenes except the scene we don't want into the new array
            int j = 0;
            for (int i = 0; i < _scenes.Length; i++)
            {
                if (tempArray[i] != scene)
                {
                    tempArray[j] = _scenes[i];
                    j++;
                }
                else
                {
                    sceneRemoved = true;
                }
            }

            //If the scene was successfully removed set the old array to be the new array
            if (sceneRemoved)
                _scenes = tempArray;

            return sceneRemoved;
        }

        public Game()
        {
            _scenes = new Scene[0];
        }



        /// <summary>
        /// Sets the current scene in the game to be the scene at the given index
        /// </summary>
        /// <param name="index">The index of the scene to switch to</param>
        public static void SetCurrentScene(int index)
        {
            //If the index is not within the range of the the array return
            if (index < 0 || index >= _scenes.Length)
                return;

            //Call end for the previous scene before changing to the new one
            if (_scenes[_currentSceneIndex].Started)
                _scenes[_currentSceneIndex].End();

            //Update the current scene index
            _currentSceneIndex = index;
        }
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

            Scene scene1 = new Scene();
            Scene scene2 = new Scene();

            // ! Space ! //
            Planets earth = new Planets(10, 10, 10);
            Sun sun = new Sun(50, 25, 50);

            //Initialize the enmies starting values
            earth.SetScale(1, 1, 1);
            earth.SetRotation(1);
            earth.SetTranslate(0, 1, 0);

            //Set sun's starting speed
            sun.SetTranslate(new Vector3(30, 10, 10));
            sun.SetRotation(1);
            sun.SetScale(6, 6, 6);
            sun.AddChild(earth);

            //Add actors to the scenes
            scene1.AddActor(sun);
            //scene1.AddActor(goal);
            scene1.AddActor(earth);

            //Sets the starting scene index and adds the scenes to the scenes array
            int startingSceneIndex = 0;
            startingSceneIndex = AddScene(scene1);
            AddScene(scene2);

            //Sets the current scene to be the starting scene index
            SetCurrentScene(startingSceneIndex);
        }

        private void Update(float deltaTime)
        {
            if (_scenes[_currentSceneIndex].Started)
                _scenes[_currentSceneIndex].Start();

            _scenes[_currentSceneIndex].Update(deltaTime);
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
            if (_scenes[_currentSceneIndex].Started)
                _scenes[_currentSceneIndex].End();
        }

        public void Run()
        {
            Start();
            while (!_gameOver && !Raylib.WindowShouldClose())
            {
                float deltaTime = Raylib.GetFrameTime();
                Update(deltaTime);
                Draw();
            }
            End();
        }
    }
}
