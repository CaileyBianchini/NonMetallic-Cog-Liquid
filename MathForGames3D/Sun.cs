using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;
using MathLibrary;

namespace MathForGames3D
{
    class Sun : Actor
    {
        /// <param name="x">Position on the x axis</param>
        /// <param name="y">Position on the y axis</param>
        /// <param name="icon">The symbol that will appear when drawn</param>
        /// <param name="color">The color of the symbol that will appear when drawn</param>
        public Sun(float x, float y, float z, ConsoleColor color = ConsoleColor.White)
            : base(x, y, z)
        {

        }

        /// <param name="x">Position on the x axis</param>
        /// <param name="y">Position on the y axis</param>
        /// <param name="rayColor">The color of the symbol that will appear when drawn to raylib</param>
        /// <param name="icon">The symbol that will appear when drawn</param>
        /// <param name="color">The color of the symbol that will appear when drawn to the console</param>
        public Sun(float x, float y, float z)
            : base(x, y, z)
        {
            Raylib.DrawSphere(new System.Numerics.Vector3(), 1, Color.YELLOW); //<for now
        }

        public override void Draw()
        {
            Raylib.DrawSphere(new System.Numerics.Vector3(), 1, Color.YELLOW); // < for now
            base.Draw();
        }

        float a = 0.0f;

        public override void Update(float deltaTime)
        {
            Rotate(a);
            SetRotation(a);

            a = a + 0.01f;

            base.Update(deltaTime);
        }
    }
}
