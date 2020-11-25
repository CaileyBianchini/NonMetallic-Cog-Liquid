using System;
using System.Collections.Generic;
using System.Text;
using MathLibrary;
using Raylib_cs;

namespace Capgras
{
    class Leg : Actor
    {
        private Sprite _sprite;

        /// <param name="x">Position on the x axis</param>
        /// <param name="y">Position on the y axis</param>
        /// <param name="icon">The symbol that will appear when drawn</param>
        /// <param name="color">The color of the symbol that will appear when drawn</param>
        public Leg(float x, float y, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base(x, y, icon, color)
        {

        }

        /// <param name="x">Position on the x axis</param>
        /// <param name="y">Position on the y axis</param>
        /// <param name="rayColor">The color of the symbol that will appear when drawn to raylib</param>
        /// <param name="icon">The symbol that will appear when drawn</param>
        /// <param name="color">The color of the symbol that will appear when drawn to the console</param>
        public Leg(float x, float y, Color rayColor, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base(x, y, rayColor, icon, color)
        {
            _sprite = new Sprite("GAme/body foot.png");
        }

        public override void Draw()
        {
            _sprite.Draw(_globalTransform);
            base.Draw();
        }

        float i = 0;
        public override void Update(float deltaTime)
        {
            if ((Game.GetKeyDown((int)KeyboardKey.KEY_A)) == true)
            {
                Rotate(i);
                SetRotation(i);
                localRotate(i);

                i = i - 0.1f;
                i *= -1;
            }
            if ((Game.GetKeyDown((int)KeyboardKey.KEY_D)) == true)
            {
                Rotate(i);
                SetRotation(i);
                localRotate(i);

                i = i - 0.1f;
                i /= -1;
            }

            if (Game.GetKeyDown((int)KeyboardKey.KEY_A) == true)
            {
                _sprite = new Sprite("GAme/body foot A.png");
            }
            else if (Game.GetKeyDown((int)KeyboardKey.KEY_D) == true)
            {
                _sprite = new Sprite("GAme/body foot.png");
            }
            else if (Game.GetKeyDown((int)KeyboardKey.KEY_SPACE) == true) //jumping while facing
            {
                _sprite = new Sprite("");
            }
            else if (Game.GetKeyDown((int)KeyboardKey.KEY_SPACE) == true && Game.GetKeyDown((int)KeyboardKey.KEY_D) == true) // juming right
            {
                _sprite = new Sprite("");
            }
            else if (Game.GetKeyDown((int)KeyboardKey.KEY_SPACE) == true && Game.GetKeyDown((int)KeyboardKey.KEY_A) == true) // jumping left
            {
                _sprite = new Sprite("");
            }
            else if (Game.GetKeyDown((int)KeyboardKey.KEY_E) == true) // erases
            {
                _sprite = new Sprite("");
            }
            else
            {
                _sprite = new Sprite("");
            }

            base.Update(deltaTime);
        }
    }
}
