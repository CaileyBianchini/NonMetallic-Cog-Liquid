using System;
using System.Collections.Generic;
using System.Text;
using MathLibrary;
using Raylib_cs;

namespace MathForGames
{

    /// <summary>
    /// This is the base class for all objects that will 
    /// be moved or interacted with in the game
    /// </summary>
    class Actor
    {
        protected char _icon = ' ';

        protected Vector2 _velocity;
        protected Matrix3 _transform = new Matrix3();
        private Matrix3 _translation = new Matrix3();
        private Matrix3 _scale = new Matrix3();
        private Matrix3 _rotation = new Matrix3();

        protected ConsoleColor _color;
        protected Color _rayColor;
        public bool Started { get; private set; }

        public void SetTranslate(Vector2 position)
        {
            _translation.m13 = position.X;
            _translation.m23 = position.Y;
        }

        //rotation
        public void SetRotation(float radions)
        {
            _rotation.m12 = ((float)Math.Sin(radions));
            _rotation.m12 = ((float)Math.Cos(radions));

            _rotation.m22 = ((float)Math.Sin(radions));
            _rotation.m22 = ((float)Math.Cos(radions));

            _rotation.m11 = ((float)Math.Sin(radions));
            _rotation.m11 = ((float)Math.Cos(radions));

            _rotation.m21 = ((float)Math.Sin(radions));
            _rotation.m21 = ((float)Math.Cos(radions));
        }

        public void SetScale(float x, float y)
        {
           _scale.m13 = x;
            _scale.m23 = y;
        }

        private void UpdateTransform()
        {
            //combine translation, rotation and scale
            _transform = SetScale() + SetRotation() + SetTranslate();
        }

        public Vector2 Forward
        {
            get 
            { 
                return new Vector2(_transform.m11, _transform.m12); 
            }
            set
            {
                _transform.m11 = value.X;
                _transform.m12 = value.Y;
            }
        }

        public Vector2 Position
        {
            get{return new Vector2(_transform.m13, _transform.m23);}
            set
            {
                _transform.m13 = value.X;
                _transform.m23 = value.Y;
            }
        }

        public Vector2 Velocity
        {
            get
            {
                return _velocity;
            }
            set
            {
                _velocity = value;
            }
        }

        ///<summary>
        ///this is the base class for all objects that will
        /// be moverd or interacted with in the game
        /// 
        /// Create new matrices to transform the actors matrix. The user should be able
        /// to translate, rotate, scale the actor.
        /// </summary>

        /// <param name="x">Position on the x axis</param>
        /// <param name="y">Position on the y axis</param>
        /// <param name="icon">The symbol that will appear when drawn</param>
        /// <param name="color">The color of the symbol that will appear when drawn</param>
        public Actor( float y, float x, char icon = ' ', ConsoleColor color = ConsoleColor.White)
        {
            _rayColor = Color.WHITE;
            _icon = icon;
            _transform = new Matrix3();
            Position = new Vector2(x, y);
            _velocity = new Vector2();
            _color = color;
            Forward = new Vector2(1, 0);
        }


        /// <param name="x">Position on the x axis</param>
        /// <param name="y">Position on the y axis</param>
        /// <param name="rayColor">The color of the symbol that will appear when drawn to raylib</param>
        /// <param name="icon">The symbol that will appear when drawn</param>
        /// <param name="color">The color of the symbol that will appear when drawn to the console</param>
        public Actor(float x, float y, Color rayColor, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : this((char)x,y,icon,color)
        {
            _transform = new Matrix3();
            //maybe \/
            _translation = new Matrix3();
            _scale = new Matrix3();
            _rotation = new Matrix3();
            //maybe not /\
            _rayColor = rayColor;
        }

        /// <summary>
        /// Updates the actors forward vector to be
        /// the last direction it moved in
        /// </summary>
        private void UpdateFacing()
        {
            if (_velocity.Magnitude <= 0)
                return;

            Forward = Velocity.Normalized;
        }

        public virtual void Start()
        {
            Started = true;
        }

        
        public virtual void Update(float deltaTime)
        {
            //Before the actor is moved, update the direction it's facing
            UpdateFacing();

            //Increase position by the current velocity
            Position += _velocity;

            //Changes position by using Tranform
            //_position *= _transform;
        }

        public virtual void Draw()
        {
            //Draws the actor and a line indicating it facing to the raylib window.
            //Scaled to match console movement
            Raylib.DrawText(_icon.ToString(), (int)Position.X * 32, (int)(Position.Y * 32), 32, _rayColor);
            Raylib.DrawLine(
                (int)(Position.X * 32),
                (int)(Position.Y * 32),
                (int)((Position.X + Forward.X) * 32),
                (int)((Position.Y + Forward.Y) * 32),
                Color.WHITE
            );

            //Changes the color of the console text to be this actors color
            Console.ForegroundColor = _color;

            //Only draws the actor on the console if it is within the bounds of the window
            if(Position.X >= 0 && Position.X < Console.WindowWidth 
                && Position.Y >= 0  && Position.Y < Console.WindowHeight)
            {
                Console.SetCursorPosition((int)Position.X, (int)Position.Y);
                Console.Write(_icon);
            }
            
            //Reset console text color to be default color
            Console.ForegroundColor = Game.DefaultColor;
        }

        public virtual void End()
        {
            Started = false;
        }

    }
}
