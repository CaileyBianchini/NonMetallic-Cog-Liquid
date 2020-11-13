using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;
using MathLibrary;

namespace MathForGames3D
{
    class Actor
    {
        protected char _icon = ' ';

        protected Vector3 _velocity;
        protected Matrix4 _globalTransform = new Matrix4();
        protected Matrix4 _localTransform = new Matrix4();
        private Matrix4 _translation = new Matrix4();
        private Matrix4 _scale = new Matrix4();
        private Matrix4 _rotation = new Matrix4();

        protected ConsoleColor _color;
        protected Color _rayColor;
        protected Actor _parent;
        protected Actor[] _children = new Actor[0];
        protected float _rotationAngle; //not needed for lookout
        private float _collisionRadius;

        public bool Started { get; private set; }

        //SET 
        public void SetTranslate(Vector3 position)
        {
            _translation = Matrix4.CreateTranslation(position);
        }

        public void SetTranslate(float x, float y, float z)
        {
            _translation.m13 = x;
            _translation.m23 = y;
            _translation.m33 = z;
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
            _rotation *= Matrix4.CreateRotation(radians);
        }

        public void localRotate(float angle)
        {
            Matrix4 m = new Matrix4((float)Math.Cos(angle), -(float)Math.Sin(angle), 0, 0, (float)Math.Sin(angle), (float)Math.Cos(angle), 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);


            // temperary:\/ original:m._localTransform = m._localTransform * m;
            _localTransform = _localTransform * m;
        }

        public void LookAt(Vector3 position)
        {

            Vector3 direction = (position - LocalPosition).Normalized;

            float dotProd = Vector3.DotProduct(Forward, direction);
            if (Math.Abs(dotProd) > 1)
                return;
            float angle = (float)Math.Acos(dotProd);

            Vector3 perp = new Vector3(direction.Y, -direction.X, direction.Z); //<Don't know if Z should be - or not

            float perpDot = Vector3.DotProduct(perp, Forward);

            if (perpDot != 0)
                angle *= -perpDot / Math.Abs(perpDot);

            Rotate(angle);
        }

        public void SetScale(float x, float y, float z)
        {
            _scale = Matrix4.CreateScale(new Vector3(x, y, z));
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
        }

        public Vector3 Forward
        {
            get
            {
                return new Vector3(_globalTransform.m11, _globalTransform.m21, _globalTransform.m23);
            }
        }

        public Vector3 WorldPosition
        {
            get
            {
                return new Vector3(_globalTransform.m13, _globalTransform.m23, _globalTransform.m33);
            }
        }


        public Vector3 LocalPosition
        {
            get { return new Vector3(_localTransform.m13, _localTransform.m23, _localTransform.m33); }
            set
            {
                _translation.m13 = value.X;
                _translation.m23 = value.Y;
                _translation.m33 = value.Z;
                //gotta fix this somehow
                Vector3 lookPosition = LocalPosition + value.Normalized;
                LookAt(lookPosition);

            }
        }

        public Vector3 Velocity
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
        public Actor(float y, float x, float z, char icon = ' ')
        {
            _icon = icon;
            _localTransform = new Matrix4();
            LocalPosition = new Vector3(x, y, z);
            _velocity = new Vector3();
        }


        /// <param name="x">Position on the x axis</param>
        /// <param name="y">Position on the y axis</param>
        /// <param name="rayColor">The color of the symbol that will appear when drawn to raylib</param>
        /// <param name="icon">The symbol that will appear when drawn</param>
        /// <param name="color">The color of the symbol that will appear when drawn to the console</param>
        public Actor(float x, float y, Color rayColor, char icon = ' ', ConsoleColor color = ConsoleColor.White)
            : this((char)x, y, icon)
        {
            _localTransform = new Matrix4();
            _translation = new Matrix4();
            _scale = new Matrix4();
            _rotation = new Matrix4();
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
