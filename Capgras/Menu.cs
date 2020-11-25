using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;
using MathLibrary;

namespace Capgras
{
    /// <summary>
    /// This is the goal the player must reach to end the game. 
    /// </summary>
    class Menu : Actor
    {
        private Actor _player;
        private Sprite _sprite;
        private Actor _menu;

        public Menu(float x, float y, Actor player, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base(x, y, icon, color)
        {
            _player = player;
        }

        public Menu(float x, float y, Color rayColor, Actor player, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base(x, y, rayColor, icon, color)
        {
            _player = player;
            _sprite = new Sprite("GAme/Menu.png");
        }

        private bool CheckPlayerDistance()
        {
            float distance = (_player.LocalPosition - LocalPosition).Magnitude;
            return distance <= 100;
        }

        public override void Draw()
        {
            _sprite.Draw(_globalTransform);
            base.Draw();
        }

        public override void Update(float deltaTime)
        {
            //moves to next scene when player presses E
            if (CheckPlayerDistance() && Game.GetKeyDown((int)KeyboardKey.KEY_E) == true)
            {
                _sprite = new Sprite("");
            }
            

            base.Update(deltaTime);
        }

    }
}

