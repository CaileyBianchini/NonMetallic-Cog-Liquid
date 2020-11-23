using System;
using System.Collections.Generic;
using System.Text;
using MathLibrary;
using Raylib_cs;

namespace Capgras
{

    /// <summary>
    /// This is the base class for all objects that will 
    /// be moved or interacted with in the game
    /// </summary>
    /// 

    /// <summary>
    /// create a "solar System" using the matrix heirarchy
    /// </summary>
    public class Actor
    {
        protected char _icon = ' ';

        protected Vector2 _velocity = new Vector2();
        protected Matrix3 _globalTransform = new Matrix3();
        protected Matrix3 _localTransform = new Matrix3();
        private Matrix3 _translation = new Matrix3();
        private Matrix3 _scale = new Matrix3();
        private Matrix3 _rotation = new Matrix3();

        protected ConsoleColor _color;
        protected Color _rayColor;
        protected Actor _parent;
        protected Actor[] _children = new Actor[0];
        protected float _rotationAngle; //not needed for lookout
        private float _collisionRadius;

        public bool Started { get; private set; }

        //SET 
        public void SetTranslate(Vector2 position)
        {
            _translation = Matrix3.CreateTranslation(position);
        }

        //rotation = the matrix3 create roatation using radians
        public void SetRotation(float radians)
        {
            _rotation = Matrix3.CreateRotation(radians);
        }

        //rotation is multiplied using matrix3 create rotation using radians
        public void Rotate(float radians)
        {
            _rotation *= Matrix3.CreateRotation(radians);
        }

        public void localRotate(float angle)
        {
            Matrix3 m = new Matrix3((float)Math.Cos(angle), -(float)Math.Sin(angle), 0, (float)Math.Sin(angle), (float)Math.Cos(angle), 0, 0, 0, 1);


            // temperary:\/ original:m._localTransform = m._localTransform * m;
            _localTransform = _localTransform * m;
        }

        public void LookAt(Vector2 position)
        {
            //Find the direction that the actor should look in
            Vector2 direction = (position - LocalPosition).Normalized;

            //Use the dotproduct to find the angle the actor needs to rotate
            float dotProd = Vector2.DotProduct(Forward, direction);
            if (Math.Abs(dotProd) > 1)
                return;
            float angle = (float)Math.Acos(dotProd);

            //Find a perpindicular vector to the direction
            Vector2 perp = new Vector2(direction.Y, -direction.X);

            //Find the dot product of the perpindicular vector and the current forward
            float perpDot = Vector2.DotProduct(perp, Forward);

            //If the result isn't 0, use it to change the sign of the angle to be either positive or negative
            if (perpDot != 0)
                angle *= -perpDot / Math.Abs(perpDot);

            Rotate(angle);
        }

        public void SetScale(float x, float y)
        {
            _scale = Matrix3.CreateScale(new Vector2(x, y));
        }

        private void UpdateTransform()
        {
            //combine translation, rotation and scale
            _localTransform = (_translation * _rotation * _scale);

            if (_parent != null)
                _globalTransform = _parent._globalTransform * _localTransform;
            else
                _globalTransform = Game.GetCurrentScene().World * _localTransform;
        }

        private void UpdateGlobalTransform()
        {

            if (_parent != null)
                _globalTransform = _parent._globalTransform * _localTransform;
            else
                _globalTransform = Game.GetCurrentScene().World * _localTransform;
        }

        public Vector2 Forward
        {
            get
            {
                return new Vector2(_globalTransform.m11, _globalTransform.m21);
            }
            set
            {
                Vector2 lookPosition = LocalPosition + value.Normalized;
                LookAt(lookPosition);
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
            get { return new Vector2(_localTransform.m13, _localTransform.m23); }
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

        public Actor(float y, float x, char icon = ' ', ConsoleColor color = ConsoleColor.White)
        {
            _rayColor = Color.WHITE;
            _icon = icon;
            _localTransform = new Matrix3();
            LocalPosition = new Vector2(x, y);
            _velocity = new Vector2();
            _color = color;
        }

        public Actor(float x, float y, Color rayColor, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : this((char)x, y, icon, color)
        {
            _localTransform = new Matrix3();
            _translation = new Matrix3();
            _scale = new Matrix3();
            _rotation = new Matrix3();
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

        private void UpdateFacing()
        {
            if (_velocity.Magnitude <= 0)
                return;
            else
            {
                Forward = Velocity.Normalized;
                return;
            }
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
            //Raylib.DrawLine(
            //    (int)(WorldPosition.X * 32),
            //    (int)(WorldPosition.Y * 32),
            //    (int)((WorldPosition.X + Forward.X) * 32),
            //    (int)((WorldPosition.Y + Forward.Y) * 32),                        //This comment draws the white line that was in it
            //    Color.WHITE
            //);

            //Changes the color of the console text to be this actors color
            Console.ForegroundColor = _color;

            //Only draws the actor on the console if it is within the bounds of the window
            if (WorldPosition.X >= 0 && WorldPosition.X < Console.WindowWidth
                && WorldPosition.Y >= 0 && WorldPosition.Y < Console.WindowHeight)
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
