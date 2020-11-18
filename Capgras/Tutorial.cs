using System;
using System.Collections.Generic;
using System.Text;
using MathLibrary;
using Raylib_cs;

namespace Capgras
{
    class Tutorial : Actor
    {
        private float _speed = 1;
        private Sprite _sprite;

        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

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
                
            }

        }

        public override void Draw()
        {
            _sprite.Draw(_globalTransform);
            base.Draw();
        }
    }
}
