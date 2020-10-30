using System;

namespace MathLibrary
{
    public class Vector3
    {
        private float _x;
        private float _y;
        private float _z;

        public float X
        {
            get { return _x; }
            set { _x = value; }
        }

        public float Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public float Z
        {
            get { return _z; }
            set { _z = value; }
        }

        public float Magnitude
        {
            get { return (float)Math.Sqrt(X * X + Y * Y); }
        }

        public Vector3 Normalized
        {
            get { return Normalize(this); }
        }



        public Vector3()
        {
            _x = 0;
            _y = 0;
            _z = 0;
        }

        public Vector3(float x, float y, float z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        /// <summary>
        /// Returns the normalized version of a the vector passed in.
        /// </summary>
        /// <param name="vector">The vector that will be normalized</param>
        /// <returns></returns>
        public static Vector3 Normalize(Vector3 vector)
        {
            //less than zero? Not Great :(
            if (vector.Magnitude == 0)
                return new Vector3();
            //greater that zero? Great!
            return vector / vector.Magnitude;
        }

        /// <summary>
        /// Returns the dot product of the two vectors given.
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static float DotProduct(Vector3 lhs, Vector3 rhs)
        {
            // X^2 + Y^2 = Dot Product
            return (lhs.X * rhs.X) + (lhs.Y * rhs.Y) + (lhs.Z * rhs.Z);
        }

        public static Vector3 operator +(Vector3 lhs, Vector3 rhs)
        {
            return new Vector3(lhs.X += rhs.X, lhs.Y += rhs.Y, lhs.Z += rhs.Z);
        }

        public static Vector3 operator -(Vector3 lhs, Vector3 rhs)
        {
            return new Vector3(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z);
        }

        //this helps with scaleing (is that how you spell it?
        public static Vector3 operator *(Vector3 lhs, float scalar)
        {
            return new Vector3(lhs.X * scalar, lhs.Y * scalar, lhs.Z * scalar);
        }

        public static Vector3 operator /(Vector3 lhs, float scalar)
        {
            return new Vector3(lhs.X / scalar, lhs.Y / scalar, lhs.Z / scalar);
        }
    }
}
