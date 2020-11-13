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
    class Bedroom : Actor
    {
        private float _speed = 1;
        private Sprite _sprite;

        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public Bedroom(float x, float y, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base(x, y, icon, color)
        {

        }

        public Bedroom(float x, float y, Color rayColor, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base(x, y, rayColor, icon, color)
        {
            _sprite = new Sprite("GAme/bedroom.png");
        }

        public override void Draw()
        {
            _sprite.Draw(_globalTransform);
            base.Draw();
        }

    }
}