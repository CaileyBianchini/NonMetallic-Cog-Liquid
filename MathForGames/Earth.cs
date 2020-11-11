using System;
using System.Collections.Generic;
using System.Text;
using MathLibrary;
using Raylib_cs;

namespace MathForGames
{
    class Earth : Actor
    {
        //similar to enimes but will be child
        private Actor _target;
        private Color _alertColor;
        private float _speed = 1;
        private Sprite _sprite;

        public float Speed
        {
            get
            {
                return _speed;
            }
            set
            {
                _speed = value;
            }
        }

        public Actor Target
        {
            get { return _target; }
            set { _target = value; }
        }

        /// <param name="x">Position on the x axis</param>
        /// <param name="y">Position on the y axis</param>
        /// <param name="icon">The symbol that will appear when drawn</param>
        /// <param name="color">The color of the symbol that will appear when drawn</param>
        public Earth(float x, float y, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base((char)x, y, icon, color)
        {
        }

        /// <param name="x">Position on the x axis</param>
        /// <param name="y">Position on the y axis</param>
        /// <param name="rayColor">The color of the symbol that will appear when drawn to raylib</param>
        /// <param name="icon">The symbol that will appear when drawn</param>
        /// <param name="color">The color of the symbol that will appear when drawn to the console</param>
        public Earth(float x, float y, Color rayColor, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : base(x, y, rayColor, icon, color)
        {
            _alertColor = Color.RED;
            _sprite = new Sprite("Images/earth.png");
        }

        /// <summary>
        /// Checks to see if the target is within the given angle
        /// and within the given distance. Returns false if no
        /// target has been set. Both the angle and the distance are inclusive.
        /// </summary>
        /// <param name="maxAngle">The maximum angle (in radians) 
        /// that the target can be detected.</param>
        /// <param name="maxDistance">The maximum distance that the target can be detected.</param>
        /// <returns></returns>
        public bool CheckTargetInSight(float maxAngle, float maxDistance)
        {
            //Checks if the target has a value before continuing
            if (Target == null)
                return false;

            //Find the vector representing the distance between the actor and its target
            Vector2 direction = Target.LocalPosition - LocalPosition;
            //Get the magnitude of the distance vector
            float distance = direction.Magnitude;
            //Use the inverse cosine to find the angle of the dot product in radians
            float angle = (float)Math.Acos(Vector2.DotProduct(Forward, direction.Normalized));

            //Return true if the angle and distance are in range
            if (angle <= maxAngle && distance <= maxDistance)
                return true;

            return false;
        }


        public override void Draw()
        {
            _sprite.Draw(_globalTransform);
            base.Draw();
        }

        float a = 0.0f;
        public override void Update(float deltaTime)
        {
            //If the target can be seen change the color to red and reset the player's position
            //If the target can't be seen change the color to blue
            if (CheckTargetInSight(1.5f, 5))
            {
                _rayColor = Color.RED;
                Target.LocalPosition = new Vector2();
            }
            else
            {
                _rayColor = Color.BLUE;
            }

            Rotate(a);
            SetRotation(a);
            localRotate(a);

            a = a + 0.2f;

            base.Update(deltaTime);
        }
    }
}
