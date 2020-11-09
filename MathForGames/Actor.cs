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
        protected Matrix3 _globalTransform = new Matrix3();
        protected Matrix3 _localTransform = new Matrix3();
        private Matrix3 _translation = new Matrix3();
        private Matrix3 _scale = new Matrix3();
        private Matrix3 _rotation = new Matrix3();

        protected ConsoleColor _color;
        protected Color _rayColor;
        protected Actor _parent;
        protected Actor[] _children = new Actor[0];
        protected float _rotationAngle;
        private float _collisionRadius;

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

        public void Rotate(float radians)
        {
            _rotationAngle += radians;
            SetRotation(_rotationAngle);
        }

        public void localRotate(float angle)
        {
            Matrix3 m = new Matrix3((float)Math.Cos(angle), -(float)Math.Sin(angle), 0, (float)Math.Sin(angle), (float)Math.Cos(angle), 0, 0, 0, 1);


            // temperary:\/ original:m._localTransform = m._localTransform * m;
            _localTransform = _localTransform * m;
        }

        public void LookAt(Vector2 position)
        {

            Vector2 direction = (position - LocalPosition).Normalized;

            float dotProd = Vector2.DotProduct(Forward, direction);
            if (Math.Abs(dotProd) > 1)
                return;
            float angle = (float)Math.Acos(dotProd);

            Vector2 perp = new Vector2(direction.Y, -direction.X);

            float perpDot = Vector2.DotProduct(perp, Forward);

            if (perpDot != 0)
                angle *= -perpDot / Math.Abs(perpDot);

            Rotate(angle);
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

        private void UpdateGlobalTransform()
        {

            if (_parent != null)
                _globalTransform = _parent._globalTransform * _localTransform;
            else
                _globalTransform = Game.CurrentScene.World * _localTransform;

            //parently not needed - Jude
            //foreach (Actor child in _children)
            //    child.UpdateGlobalTransform();
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
                //gotta fix this somehow
                Vector2 lookPosition = LocalPosition + value.Normalized;
                LookAt(lookPosition);
                
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

            UpdateGlobalTransform();

            
        }

        public virtual void Draw()
        {
            //Draws the actor and a line indicating it facing to the raylib window.
            //Scaled to match console movement
            Raylib.DrawText(_icon.ToString(), (int)WorldPosition.X * 32, (int)(WorldPosition.Y * 32), 32, _rayColor);
            Raylib.DrawLine(
                (int)(WorldPosition.X * 32),
                (int)(WorldPosition.Y * 32),
                (int)((WorldPosition.X + Forward.X) * 32),
                (int)((WorldPosition.Y + Forward.Y) * 32),
                Color.WHITE
            );

            //Changes the color of the console text to be this actors color
            Console.ForegroundColor = _color;

            //Only draws the actor on the console if it is within the bounds of the window
            if(WorldPosition.X >= 0 && WorldPosition.X < Console.WindowWidth 
                && WorldPosition.Y >= 0  && WorldPosition.Y < Console.WindowHeight)
            {
                Console.SetCursorPosition((int)WorldPosition.X, (int)WorldPosition.Y);
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
