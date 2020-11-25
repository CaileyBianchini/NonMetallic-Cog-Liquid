using System;
using System.Collections.Generic;
using System.Text;
using MathLibrary;
using Raylib_cs;

namespace Capgras
{
    class Arm : Actor
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
        public Arm(float x, float y, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base(x, y, icon, color)
        {

        }

        /// <param name="x">Position on the x axis</param>
        /// <param name="y">Position on the y axis</param>
        /// <param name="rayColor">The color of the symbol that will appear when drawn to raylib</param>
        /// <param name="icon">The symbol that will appear when drawn</param>
        /// <param name="color">The color of the symbol that will appear when drawn to the console</param>
        public Arm(float x, float y, Color rayColor, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base(x, y, rayColor, icon, color)
        {
            _sprite = new Sprite("GAme/body hand.png");
        }

        public override void Draw()
        {
            _sprite.Draw(_globalTransform);
            base.Draw();
        }

        float i = 0;
        public override void Update(float deltaTime)
        {
            //!this will effect the rotaton of the sprite!
            if((Game.GetKeyDown((int)KeyboardKey.KEY_A)) == true)
            {
                Rotate(i);
                SetRotation(i);
                localRotate(i);

                i = i + 0.2f;
                i /= -1;
            }
            if ((Game.GetKeyDown((int)KeyboardKey.KEY_D)) == true)
            {
                Rotate(i);
                SetRotation(i);
                localRotate(i);

                i = i + 0.2f;
                i *= -1;
            }

            //!this will change what the icon looks like!
            if (Game.GetKeyDown((int)KeyboardKey.KEY_A) == true) // left arm
            {
                _sprite = new Sprite("GAme/body hand A.png");
            }
            else if (Game.GetKeyDown((int)KeyboardKey.KEY_D) == true) // right arm
            {
                _sprite = new Sprite("GAme/body hand.png");
            }
            else if (Game.GetKeyDown((int)KeyboardKey.KEY_SPACE) == true) //erases arm
            {
                _sprite = new Sprite("");
            }
            else if (Game.GetKeyDown((int)KeyboardKey.KEY_SPACE) == true && Game.GetKeyDown((int)KeyboardKey.KEY_D) == true) // juming right and erasing arm
            {
                _sprite = new Sprite("");
            }
            else if (Game.GetKeyDown((int)KeyboardKey.KEY_SPACE) == true && Game.GetKeyDown((int)KeyboardKey.KEY_A) == true) // jumping left and erasing arm
            {
                _sprite = new Sprite("");
            }
            else if (Game.GetKeyDown((int)KeyboardKey.KEY_E) == true) // erases
            {
                _sprite = new Sprite("");
            }
            else //erases arm
            {
                _sprite = new Sprite("");
            }

            base.Update(deltaTime);
        }
    }
}
