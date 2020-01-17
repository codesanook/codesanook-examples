using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace Codesanook.Examples.OpenGL
{
    public class Game
    {
        private readonly GameWindow window;
        private double theta;

        public Game(GameWindow window)
        {
            this.window = window;
            Start();
        }

        private void Start()
        {
            window.Load += Load;
            window.Resize += Resize;
            window.RenderFrame += RenderFrame;
            window.Run(1.0 / 60.0);//60 frames in 1 second	
        }

        private void Resize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, window.Width, window.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            var matrix = Matrix4.CreatePerspectiveFieldOfView(45.0f, window.Width / window.Height, 1.0f, 100.0f);
            GL.LoadMatrix(ref matrix);
            GL.Ortho(-50.0, 50.0, -50.0, 50.0, -1.0, 1.0);
            GL.MatrixMode(MatrixMode.Modelview);
        }

        private void RenderFrame(object sender, FrameEventArgs e)
        {
            GL.LoadIdentity();
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.Rotate(theta, 0, 0, 1);
            GL.Begin(BeginMode.Quads);

            GL.Color3(1.0, 0, 0);
            GL.Vertex2(30.0, 30.0);

            GL.Color3(0, 1.0, 0);
            GL.Vertex2(-30.0, 30.0);

            GL.Color3(0, 0, 1.0);
            GL.Vertex2(-30.0, -30.0);

            GL.Color3(1.0, 1.0, 0);
            GL.Vertex2(30.0, -30.0);

            GL.End();
            window.SwapBuffers();
            theta += 1;

        }

        private void Load(object sender, EventArgs e)
        {
            GL.ClearColor(1.0F, 1.0F, 1.0F, 0.0F);
        }
    }
}
