using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using MathLibrary;
using Raylib_cs;

namespace Capgras
{
    class Game
    {
        private static bool _gameOver = false;
        private static Scene[] _scenes;
        private static int _currentSceneIndex;
        public static int CurrentSceneIndex
        {
            get { return _currentSceneIndex; }
        }

        //Below are the basic codes
        public static ConsoleColor DefaultColor { get; set; } = ConsoleColor.White;

        public static void SetGameOver(bool value)
        {
            _gameOver = value;
        }

        public static Scene GetScene(int index)
        {
            if (index < 0 || index >= _scenes.Length)
                return new Scene();

            return _scenes[index];
        }

        /// <summary>
        /// Returns the scene that is at the index of the 
        /// current scene index
        /// </summary>
        /// <returns></returns>
        public static Scene GetCurrentScene()
        {
            return _scenes[_currentSceneIndex];
        }

        public static Scene CurrentScene
        {
            get { return _scenes[_currentSceneIndex]; }
        }

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

        public static bool GetKeyDown(int key)
        {
            return Raylib.IsKeyDown((KeyboardKey)key);
        }

        public static bool GetKeyPressed(int key)
        {
            return Raylib.IsKeyPressed((KeyboardKey)key);
        }

        public Game()
        {
            _scenes = new Scene[0];
        }

        
        //Called when the game begins. Use this for initialization.
        public void Start()
        {
            //Creates a new window for raylib
            Raylib.InitWindow(1920, 1080, "Capgras");
            Raylib.SetTargetFPS(30);

            //Set up console window
            Console.CursorVisible = false;
            Console.Title = "Capgras";

            //Create a new scene for our actors to exist in
            Scene scene1 = new Scene();
            Scene scene2 = new Scene();



            //Scene
            Setting bedroom = new Setting(50, 50, Color.YELLOW, ' ', ConsoleColor.Red); //I think the math is incorrect so I can't resize it and 
            bedroom.SetTranslate(new Vector2(30, 17));
            bedroom.SetScale(60, 30);
            
            //this will be done termperarily <sorry for misspelling 
            //Setting hallway = new Setting(50, 50, Color.YELLOW, ' ', ConsoleColor.Red); //I think the math is incorrect so I can't resize it and 
            //hallway.SetTranslate(new Vector2(25, 25));
            //hallway.SetScale(50, 50);

            Hallway hallway = new Hallway(50, 50, Color.YELLOW, ' ', ConsoleColor.Red); 
            hallway.SetTranslate(new Vector2(30, 17));
            hallway.SetScale(60, 30);

            Fog fog = new Fog(50, 50, Color.YELLOW, ' ', ConsoleColor.Red); 
            fog.SetTranslate(new Vector2(30, 17));
            fog.SetScale(60, 30);

            Tutorial wasd = new Tutorial(50, 50, Color.YELLOW, ' ', ConsoleColor.Red); 
            wasd.SetTranslate(new Vector2(25, 25));
            wasd.SetScale(50, 50);

            Tutorial2 SPACE = new Tutorial2(50, 50, Color.YELLOW, ' ', ConsoleColor.Red); 
            SPACE.SetTranslate(new Vector2(25, 25));
            SPACE.SetScale(50, 50);

            //Player

            Player player = new Player(10, 26, Color.YELLOW, ' ', ConsoleColor.Red);
            player.Speed = 5;
            player.SetTranslate(new Vector2(10, 26));
            player.SetScale(6, 9);

            Arm rightArm = new Arm(0, 0, Color.YELLOW, ' ', ConsoleColor.Red);
            player.AddChild(rightArm);
            rightArm.SetTranslate(new Vector2(0, 0));
            rightArm.SetScale(0.75f, 1f);

            Leg rightLeg = new Leg(0, 0, Color.YELLOW, ' ', ConsoleColor.Red);
            player.AddChild(rightLeg);
            rightLeg.SetTranslate(new Vector2(0, 0));
            rightLeg.SetScale(.45f, 1f);

            Leg leftLeg = new Leg(0, 0, Color.YELLOW, ' ', ConsoleColor.Red);
            player.AddChild(leftLeg);
            leftLeg.SetTranslate(new Vector2(-.06f, 0));
            leftLeg.SetScale(.40f, 0.95f);

            //Objects

            Door door = new Door(20, 20, Color.GREEN, player, ' ', ConsoleColor.Green);
            door.SetTranslate(new Vector2(50, 22f)); // Y should always be at 22f!
            door.SetScale(8, 15);

            Wardrobe wardrobe = new Wardrobe(20, 20, Color.GREEN, player, ' ', ConsoleColor.Green);
            wardrobe.SetTranslate(new Vector2(30, 19f));
            wardrobe.SetScale(12, 22);

            Paintings painting1 = new Paintings(50, 50, Color.YELLOW, ' ', ConsoleColor.Red);
            painting1.SetTranslate(new Vector2(25, 17));
            painting1.SetScale(55, 25);

            NextSide side = new NextSide(20, 20, Color.GREEN, player, ' ', ConsoleColor.Green);
            side.SetTranslate(new Vector2(50, 22f)); // Y should always be at 22f!
            side.SetScale(8, 15);

            // SCENE 1
            //Setting adding
            scene1.AddActor(bedroom);
            scene1.AddActor(door); //must be where its at

            //player adding
            scene1.AddActor(leftLeg);
            scene1.AddActor(player);
            scene1.AddActor(rightLeg);           
            scene1.AddActor(rightArm);

            scene1.AddActor(fog);
            scene1.AddActor(wasd);

            //SCENE 2
            //Setting adding
            scene2.AddActor(hallway);
            scene2.AddActor(painting1);
            scene2.AddActor(door);
            scene2.AddActor(wardrobe);
            scene2.AddActor(SPACE);


            //player adding
            scene2.AddActor(leftLeg);
            scene2.AddActor(player);
            scene2.AddActor(rightLeg);
            scene2.AddActor(rightArm);

            scene2.AddActor(fog);
            
            //Sets the starting scene index and adds the scenes to the scenes array
            int startingSceneIndex = 0;
            startingSceneIndex = AddScene(scene1);
            AddScene(scene2);

            //This is what will set the first scene to start
            SetCurrentScene(startingSceneIndex);
        }


        /// <summary>
        /// Called every frame
        /// </summary>
        /// <param name="deltaTime">The time between each frame</param>
        public void Update(float deltaTime)
        {
            if (!_scenes[_currentSceneIndex].Started)
            {
                    _scenes[_currentSceneIndex].Start();
            }

            _scenes[_currentSceneIndex].Update(deltaTime);
        }

        //Used to display objects and other info on the screen.
        public void Draw()
        {
            Raylib.BeginDrawing();

            Raylib.ClearBackground(Color.BLACK);
            Console.Clear();
            _scenes[_currentSceneIndex].Draw();

            Raylib.EndDrawing();
        }


        //Called when the game ends.
        public void End()
        {
            if (_scenes[_currentSceneIndex].Started)
                _scenes[_currentSceneIndex].End();
        }


        //Handles all of the main game logic including the main game loop.
        public void Run()
        {
            //Call start for all objects in game
            Start();

            //Loops the game until either the game is set to be over or the window closes
            while (!_gameOver && !Raylib.WindowShouldClose())
            {
                //Stores the current time between frames
                float deltaTime = Raylib.GetFrameTime();
                //Call update for all objects in game
                Update(deltaTime);
                //Call draw for all objects in game
                Draw();
                //Clear the input stream for the console window
                while (Console.KeyAvailable)
                    Console.ReadKey(true);
            }

            //this will be applied when _gameover = true ou other conditions are met
            End();
        }
    }
}