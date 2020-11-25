using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;
using MathLibrary;

namespace Capgras
{
    /// <summary>
    /// An actor that moves based on input given by the user
    /// </summary>
    class Player : Actor
    {
        private float _speed = 1;
        private Sprite _sprite;

        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        /// <param name="x">Position on the x axis</param>
        /// <param name="y">Position on the y axis</param>
        /// <param name="icon">The symbol that will appear when drawn</param>
        /// <param name="color">The color of the symbol that will appear when drawn</param>
        public Player(float x, float y, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base(x, y, icon, color)
        {

        }

        /// <param name="x">Position on the x axis</param>
        /// <param name="y">Position on the y axis</param>
        /// <param name="rayColor">The color of the symbol that will appear when drawn to raylib</param>
        /// <param name="icon">The symbol that will appear when drawn</param>
        /// <param name="color">The color of the symbol that will appear when drawn to the console</param>
        public Player(float x, float y, Color rayColor, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base(x, y, rayColor, icon, color)
        {
            _sprite = new Sprite("GAme/body body.png");
        }

        public override void Draw()
        {
            _sprite.Draw(_globalTransform);
            base.Draw();
        }

        public override void Update(float deltaTime)
        {
            //Gets the player's input to determine which direction the actor will move in on each axis 
            int xDirection = -Convert.ToInt32(Game.GetKeyDown((int)KeyboardKey.KEY_A))
                + Convert.ToInt32(Game.GetKeyDown((int)KeyboardKey.KEY_D));
            int yDirection = 0;

            //JUMPING (temperarily disabling it but keeping the visuals)
            //yDirection = -Convert.ToInt32(Game.GetKeyDown((int)KeyboardKey.KEY_SPACE));

            if (Game.GetKeyDown((int)KeyboardKey.KEY_A) == true) //facing left
            {
                _sprite = new Sprite("GAme/A.png");
            }
            else if (Game.GetKeyDown((int)KeyboardKey.KEY_D) == true) //facing right
            {
                _sprite = new Sprite("GAme/D.png");
            }
            else if(Game.GetKeyDown((int)KeyboardKey.KEY_SPACE) == true) // jumping while facing player
            {
                _sprite = new Sprite("GAme/jump D.png"); //temperarily D facing
            }
            else if (Game.GetKeyDown((int)KeyboardKey.KEY_SPACE) == true && Game.GetKeyDown((int)KeyboardKey.KEY_D) == true) // juming right
            {
                _sprite = new Sprite("GAme/jump D.png");
            }
            else if (Game.GetKeyDown((int)KeyboardKey.KEY_SPACE) == true && Game.GetKeyDown((int)KeyboardKey.KEY_A) == true) // jumping left
            {
                _sprite = new Sprite("GAme/jump A.png");
            }
            else //will make this facing player
            {
                _sprite = new Sprite("GAme/facing.png");
            }

            //Set the actors current velocity to be the a vector with the direction found scaled by the speed
            //Acceleration = new Vector2(xDirection, yDirection); //NOT BEING USED!!!
            Velocity = new Vector2(xDirection, yDirection);
            Velocity = Velocity.Normalized * Speed;

            base.Update(deltaTime);
        }
    }
}
