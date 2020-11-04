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
    /// 

    /// <summary>
    /// create a "solar System" using the matrix heirarchy
    /// </summary>
    class Actor
    {
        protected char _icon = ' ';

        protected Vector2 _velocity;
        protected Matrix3 _globalTransform;
        protected Matrix3 _localTransform = new Matrix3();
        private Matrix3 _translation = new Matrix3();
        private Matrix3 _scale = new Matrix3();
        private Matrix3 _rotation = new Matrix3();

        protected ConsoleColor _color;
        protected Color _rayColor;
        protected Actor _parent;
        protected Actor[] _children = new Actor[0];

        public bool Started { get; private set; }

        public void SetTranslate(Vector2 position)
        {
            _translation.m13 = position.X;
            _translation.m23 = position.Y;
        }

        //rotation
        public void SetRotation(float radions)
        {
            _rotation.m11 = (float)Math.Cos(radions);
            _rotation.m21 = -(float)Math.Sin(radions);
            _rotation.m12 = (float)Math.Sin(radions);
            _rotation.m22 = (float)Math.Cos(radions);
        }

        public void SetScale(float x, float y)
        {
           _scale.m11 = x;
            _scale.m22 = y;
        }

        private void UpdateTransform()
        {
            //combine translation, rotation and scale
            _localTransform = (_translation * _rotation * _scale);
        }

        public Vector2 Forward
        {
            get 
            { 
                return new Vector2(_localTransform.m11, _localTransform.m21); 
            }
        }

        public Vector2 WorldPosition
        {
            get
            {
                return new Vector2(_globalTransform.m13, _globalTransform.m23);
            }
        }


        public Vector2 LocalPosition
        {
            get{return new Vector2(_localTransform.m13, _localTransform.m23);}
            set
            {
                _translation.m13 = value.X;
                _translation.m23 = value.Y;
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
            _localTransform = new Matrix3();
            LocalPosition = new Vector2(x, y);
            _velocity = new Vector2();
            _color = color;
        }


        /// <param name="x">Position on the x axis</param>
        /// <param name="y">Position on the y axis</param>
        /// <param name="rayColor">The color of the symbol that will appear when drawn to raylib</param>
        /// <param name="icon">The symbol that will appear when drawn</param>
        /// <param name="color">The color of the symbol that will appear when drawn to the console</param>
        public Actor(float x, float y, Color rayColor, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : this((char)x,y,icon,color)
        {
            _localTransform = new Matrix3();
            //maybe \/
            _translation = new Matrix3();
            _scale = new Matrix3();
            _rotation = new Matrix3();
            //maybe not /\
            _rayColor = rayColor;
        }

        public void AddChild(Actor child)
        {
            Actor[] tempArray = new Actor[_children.Length + 1];

            for (int i = 0; i < _children.Length; i++)
            {
                tempArray[i] = _children[i];
            }

            tempArray[_children.Length] = child;
            _children = tempArray;
            child._parent = this;
        }

        public bool RemoveChild(Actor child)
        {
            bool childRemoved = false;
            if (child == null)
                return false;

            Actor[] tempArray = new Actor[_children.Length - 1];

            int j = 0;
            for (int i = 0; i < _children.Length; i++)
            {
                if (child != _children[i])
                {
                    tempArray[j] = _children[i];
                    j++;
                }
                else
                {
                    childRemoved = true;
                }
            }

            _children = tempArray;
            child._parent = null;
            return childRemoved;
        }

        /// <summary>
        /// Updates the actors forward vector to be
        /// the last direction it moved in
        /// </summary>
        private void UpdateFacing()
        {
            if (_velocity.Magnitude <= 0)
                return;

        }

        public virtual void Start()
        {
            Started = true;
        }

        
        public virtual void Update(float deltaTime)
        {
            //Before the actor is moved, update the direction it's facing
            UpdateFacing();

            UpdateTransform();

            //Increase position by the current velocity
            LocalPosition += _velocity * deltaTime;

            //Changes position by using Tranform
            //_position *= _transform;
        }

        public virtual void Draw()
        {
            //Draws the actor and a line indicating it facing to the raylib window.
            //Scaled to match console movement
            Raylib.DrawText(_icon.ToString(), (int)LocalPosition.X * 32, (int)(LocalPosition.Y * 32), 32, _rayColor);
            Raylib.DrawLine(
                (int)(LocalPosition.X * 32),
                (int)(LocalPosition.Y * 32),
                (int)((LocalPosition.X + Forward.X) * 32),
                (int)((LocalPosition.Y + Forward.Y) * 32),
                Color.WHITE
            );

            //Changes the color of the console text to be this actors color
            Console.ForegroundColor = _color;

            //Only draws the actor on the console if it is within the bounds of the window
            if(LocalPosition.X >= 0 && LocalPosition.X < Console.WindowWidth 
                && LocalPosition.Y >= 0  && LocalPosition.Y < Console.WindowHeight)
            {
                Console.SetCursorPosition((int)LocalPosition.X, (int)LocalPosition.Y);
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
