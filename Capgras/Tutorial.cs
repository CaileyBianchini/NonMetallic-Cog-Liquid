using System;
using System.Collections.Generic;
using System.Text;
using MathLibrary;
using Raylib_cs;

namespace Capgras
{
    class Tutorial : Actor
    {
        private Sprite _sprite;


        public Tutorial(float x, float y, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base(x, y, icon, color)
        {

        }

        public Tutorial(float x, float y, Color rayColor, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base(x, y, rayColor, icon, color)
        {
            if (Game.CurrentSceneIndex == 0)
            {
                _sprite = new Sprite("GAme/tutorial 1.png");
            }
            else if (Game.CurrentSceneIndex == 1)
            {
                _sprite = new Sprite("GAme/tutorial 2.png");
            }

        }

        public override void Draw()
        {
            _sprite.Draw(_globalTransform);
            base.Draw();
        }
    }
}
