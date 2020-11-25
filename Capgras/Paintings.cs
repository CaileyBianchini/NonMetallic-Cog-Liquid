using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;
using MathLibrary;

namespace Capgras
{
    class Paintings : Actor
    {
        private Sprite _sprite;


        public Paintings(float x, float y, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base(x, y, icon, color)
        {

        }

        public Paintings(float x, float y, Color rayColor, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base(x, y, rayColor, icon, color)
        {

            _sprite = new Sprite("GAme/paintings 1.png");


        }

        public override void Draw()
        {
            _sprite.Draw(_globalTransform);
            base.Draw();
        }

    }
}
