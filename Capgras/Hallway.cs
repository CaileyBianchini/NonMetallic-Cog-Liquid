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
    class Hallway : Actor
    {
        //THIS IS TEMPERARY!!! 
        private Sprite _sprite;


        public Hallway(float x, float y, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base(x, y, icon, color)
        {

        }

        public Hallway(float x, float y, Color rayColor, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base(x, y, rayColor, icon, color)
        {
            
            _sprite = new Sprite("GAme/Hallway.png");
            

        }

        public override void Draw()
        {
            _sprite.Draw(_globalTransform);
            base.Draw();
        }

    }
}