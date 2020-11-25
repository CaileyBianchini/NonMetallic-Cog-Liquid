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
    class NextSide : Actor
    {
        private Actor _player;
        private Sprite _sprite;

        public NextSide(float x, float y, Actor player, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base(x, y, icon, color)
        {
            _player = player;
        }

        public NextSide(float x, float y, Color rayColor, Actor player, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base(x, y, rayColor, icon, color)
        {
            _player = player;
            _sprite = new Sprite("");
        }

        private bool CheckPlayerDistance()
        {
            float distance = (_player.LocalPosition - LocalPosition).Magnitude;
            return distance <= 5;
        }

        public override void Draw()
        {
            _sprite.Draw(_globalTransform);
            base.Draw();
        }

        public override void Update(float deltaTime)
        {
            //If the player is in range of the door and selects W, it will move to next scene...hopefully
            if (CheckPlayerDistance())
                Game.SetCurrentScene(Game.CurrentSceneIndex + 1);

            base.Update(deltaTime);
        }
    }
}
